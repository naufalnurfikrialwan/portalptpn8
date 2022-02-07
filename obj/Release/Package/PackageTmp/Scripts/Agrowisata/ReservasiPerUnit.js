﻿var nama;
var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var model;
var headerBaru, detailBaru, kodebooking;
var HariIni;
var gridStatus, kebunId, customer_AgrowisataId, tanggalInap, agrowisata_HDR_ReservasiPerUnitId, tipeKamarId, agrowisata_DTL_ReservasiPerUnitId;
var menuId = 'a887e968-62f1-44fd-9993-e5986aa923c2';
var jumlahHari;
var potongan;
var jenisDiscount;
var rowNumber = 0;
var hariAwal;
var hariAkhir;
var publicholiday, statusLiburNasional;
var jenisPotongan, tanggalMasuk, tanggalKeluar;

$('.k-window.titlebar').css('style', 'display: none');
$(".k-button-icontext").addClass("k-state-disabled").removeClass("k-grid-add");

$(document).ajaxStart(function () {
    $("#_header").addClass('disabledbutton');
    $("#_detail").addClass('disabledbutton');
    $("#_footer").addClass('disabledbutton');
    $("#progress").show();
}).ajaxStop(function () {
    $("#progress").hide();
    $("#_header").removeClass('disabledbutton');
    $("#_detail").removeClass('disabledbutton');
    $("#_footer").removeClass('disabledbutton');
});

$(document).ready(function () {
    $("#btnAddCustomer").click(btnAddCustomer);
    $('#grdDetail').addClass('disabledbutton');
    function btnAddCustomer() {
        window.location.href = "/Input/Input?aplikasiId=d46b5ee3-4a74-41e5-9240-b4216145e227&menuId=5ca25358-dc7b-47bb-b852-92921a815ab6";
    }

    wnd = $("#modalWindow").kendoWindow({
        title: "Konfirmasi",
        modal: true,
        visible: false,
        resizable: false,
        width: 300
    }).data("kendoWindow");

    wndDetail = $("#konfirmasiDetail").kendoWindow({
        title: "Konfirmasi",
        modal: true,
        visible: false,
        resizable: false,
        width: 300
    }).data("kendoWindow");

    err = $("#errWindow").kendoWindow({
        title: "Error Data",
        modal: true,
        visible: false,
        resizable: false,
        width: 300
    }).data("kendoWindow");

    prt = $("#printWindow").kendoWindow({
        title: "Print",
        modal: true,
        visible: false,
        resizable: false,
        width: 800,
        height: 500
    }).data("kendoWindow");

    reservationWdw = $("#reservationWindow").kendoWindow({
        title: "Print",
        modal: true,
        visible: false,
        resizable: false,
        width: 600,
        height: 320
    }).data("kendoWindow");

    del = $("#delWindow").kendoWindow({
        title: "Hapus Header",
        modal: true,
        visible: false,
        resizable: false,
        width: 400,
        height: 150
    }).data("kendoWindow");

    wndLeaveGrid = $("#konfirmasiLeaveGrid").kendoWindow({
        title: "Konfirmasi",
        modal: true,
        visible: false,
        resizable: false,
        width: 400,
        height: 150
    }).data("kendoWindow");


    gridStatus = 'belum-ngapa-ngapain';

    $("#grdDetail").kendoValidator({
        rules: { // custom rules
            chopvalidation: function (input) {
                if (input.attr("name") == "KodeDiscount" && input.val() != "") {
                    var pot;
                    potonganDiskon(input.val()).done(function (data1) {
                        if (data1.length == 0) {
                            alert("Tidak ada Diskon/Promo!");
                            jenisPotongan = "";
                            //----------------------------------------
                            hargaKamar(input.val()).done(function (hargaKamar) {
                                if (hargaKamar.length > 0) {
                                    var grid = $('#grdDetail').data('kendoGrid');
                                    var dataItem = typeof (grid.dataSource.get(agrowisata_DTL_ReservasiPerUnitId)) == "undefined" ? grid.dataSource.getByUid(agrowisata_DTL_ReservasiPerUnitId) : grid.dataSource.get(agrowisata_DTL_ReservasiPerUnitId);
                                    var extraBed = dataItem.ExtraBed;
                                    var extraBreakfast = dataItem.ExtraBreakfast;
                                    var totalHarga = parseFloat(hargaKamar[0][0]) + (55000 * extraBed) + (16500 * extraBreakfast);
                                    dataItem.Harga = totalHarga;
                                    $('#Harga').val(totalHarga);
                                    grid.refresh();
                                }
                            }).fail(function (x) {
                                alert('Error! Hubungi Bagian TI');
                            });
                            //----------------------------------------
                        }
                        else {
                            jenisPotongan = data1[0][0];
                            if (data1[0][0] == "Prosentase") {
                                pot = parseInt(data1[0][1]);
                                potongan = pot * (1 / 100);
                            }
                            else if (data1[0][0] == "Free Inap") { potongan = parseInt(data1[0][1]) * 0 }
                            else if (data1[0][0] == "Cash") { potongan = parseInt(data1[0][1]) }

                            //----------------------------------------
                            hargaKamar(input.val()).done(function (hargaKamar) {
                                if (hargaKamar.length > 0) {
                                var grid = $('#grdDetail').data('kendoGrid');
                                    var dataItem = typeof (grid.dataSource.get(agrowisata_DTL_ReservasiPerUnitId)) == "undefined" ? grid.dataSource.getByUid(agrowisata_DTL_ReservasiPerUnitId) : grid.dataSource.get(agrowisata_DTL_ReservasiPerUnitId);
                                var extraBed = dataItem.ExtraBed;
                                var extraBreakfast = dataItem.ExtraBreakfast;
                                var totalHarga = parseFloat(hargaKamar[0][0]) + (55000 * extraBed) + (16500 * extraBreakfast);
                                dataItem.Harga = totalHarga;
                                $('#Harga').val(totalHarga);
                                grid.refresh();
                                }
                            }).fail(function (x) {
                                alert('Error! Hubungi Bagian TI');
                            });
                            //----------------------------------------

                        }
                    }).fail(function (x) {
                        alert('Error! Hubungi Bagian TI');
                    });
                }
                else if (input.attr("name") == "TanggalKeluar" && input.val() != "") {
                        input.attr("data-chopvalidation-msg", "Kamar Sudah Terisi!")
                        cekKetersediaanKamar(input).done(function (r) {
                            if(r.length==0) {
                                return true;
                            } else {
                                for (var i = 0; i < r.length; i++) {
                                    if (r[i][0] != dtl_ReservasiPerUnitId) {
                                        $('#errMsg').text('Kamar Sudah Terisi!');
                                        openErrWindow();
                                        var grid = $('#grdDetail').data('kendoGrid');
                                        var dataItem = typeof (grid.dataSource.get(agrowisata_DTL_ReservasiPerUnitId)) == "undefined" ? grid.dataSource.getByUid(agrowisata_DTL_ReservasiPerUnitId) : grid.dataSource.get(agrowisata_DTL_ReservasiPerUnitId);
                                        return false;
                                    }
                                }
                                return true;
                            }
                        })
                        .fail(function (x) {
                            return false;
                        });
                    }

                else {
                    return true;
                };
            }
        }
    });
});

$(window).on("beforeunload", function () {
    return "Please don't leave me!";
});

$(window).on("unload", function () {
    hapusPenggunaPortalYangAktif();
});

angular.module("__header", ["kendo.directives"])
    .controller("MyCtrl", function ($scope) {
        $scope.validate = function (event) {
            event.preventDefault();
            if ($scope.validator.validate()) {
                simpanHeader().done(function (data) {
                    alert("Data Customer Sudah Tersimpan. Lanjutkan pemilihan Kamar/Fasilitas di kolom bawah..");
                    $('#grdDetail').removeClass('disabledbutton');
                    $(".button").find('span').addClass('disabledbutton');
                    $(".button").addClass('disabledbutton');
                    if (data) {
                        $('#btnDeleteHeader').removeClass('disabledbutton');
                        $('#btnPrintHeader').removeClass('disabledbutton');
                        $('#btnDeleteHeader').attr('disabled', false);
                        $('#btnPrintHeader').attr('disabled', false);
                        $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                        $('#grdDetail').removeClass('disabledbutton');
                        $("#grdDetail").data("kendoGrid").dataSource.read({
                            id: $('#Agrowisata_HDR_ReservasiPerUnitId').val()
                        });
                    }
                    else {
                        openErrWindow();
                        $('#grdDetail').addClass('disabledbutton');
                        gridDestroy();
                    }
                })
                .fail(function (x) {
                    alert("Error! Hubungi Bagian TI");
                });
            } else {
                $(".k-button-icontext").addClass("k-state-disabled").removeClass("k-grid-add"); // edited
                gridDestroy();
            }
        }
    });

function renderNumber(data) {
    var no = ++rowNumber;
    data.NoBaris = no;
    return no;
}

function cekKetersediaanKamar(cekKamar) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(agrowisata_DTL_ReservasiPerUnitId)) == "undefined" ? grid.dataSource.getByUid(agrowisata_DTL_ReservasiPerUnitId) : grid.dataSource.get(agrowisata_DTL_ReservasiPerUnitId);
    var tanggalMasukCek = dataItem.TanggalMasuk;
    var tanggalKeluarCek = dataItem.TanggalKeluar;
    var dateStringMasuk = new Date();
    dateStringMasuk = tanggalMasukCek.getFullYear() + "/" + (tanggalMasukCek.getMonth() + 1) + "/" + (tanggalMasukCek.getDate());
    var dateStringKeluar = new Date();
    dateStringKeluar = tanggalKeluarCek.getFullYear() + "/" + (tanggalKeluarCek.getMonth() + 1) + "/" + (tanggalKeluarCek.getDate());
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select TipeKamarId from [SPDK_KanpusEF].[dbo].[DTL_ReservasiPerUnit]" +
                " where TipeKamarId='" + tipeKamarId + "' and (TanggalMasuk BETWEEN '" + dateStringMasuk + "' and '" + dateStringKeluar + "')",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function resetRowNumber(e) {
    rowNumber = 0;
}
function simpanHeader() {
    var url = '/Input/simpanHeader';
    var mdl = ViewToModel();
    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            _model: JSON.stringify(mdl),
            _baru: true,
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function ViewToModel() {
    kodebooking = makeid();
    var statusReservasi = "BOOKING";
    reservasiPerUnitId = guid();
    var mdl = {
        Agrowisata_HDR_ReservasiPerUnitId: reservasiPerUnitId,
        Customer_AgrowisataId: $('#Customer_AgrowisataId').val(),
        Nama: $('#Nama').val(),
        NoIdentitas: $('#NoIdentitas').val(),
        TanggalReservasi: $('#TanggalReservasi').val(),
        TahunBuku: $('#TahunBuku').val(),
        UnitId: $('#UnitId').data('kendoDropDownList').value(),
        TanggalCheckIn: $('#TanggalCheckIn').val(),
        LamaMenginap: $('#LamaMenginap').val(),
        StatusHoliday: $('#StatusHoliday').data('kendoDropDownList').value(),
        StatusLiburNasional: $('#StatusLiburNasional').data('kendoDropDownList').value(),
        MetodePembayaran: $('#MetodePembayaran').data('kendoDropDownList').value(),
        KodeBooking: kodebooking,
        StatusReservasi: statusReservasi,
        UserName: $('#UserName').val(),
        BisaCheckIn: $('#BisaCheckIn').val(),
        BankTransfer: $('#BankTransfer').data('kendoDropDownList').value()
    };
    return mdl;
}

function hapusPenggunaPortalYangAktif() {
    var url = '/Account/HapusPenggunaPortalYangAktif';
    return $.ajax({
        url: url,
        data: {
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
        s4() + '-' + s4() + s4() + s4();
}

function disableHeader() {
    $("._key").find('span').addClass('disabledbutton');
    $("._key").addClass('disabledbutton');
    $("._nonkey").find('span').addClass('disabledbutton');
    $("._nonkey").addClass('disabledbutton');
    $('#btnSubmitHeader').addClass('disabledbutton');
    $('#grdDetail').addClass('disabledbutton');
}

function enableHeader() {
    $("._key").find('span').removeClass('disabledbutton');
    $("._key").removeClass('disabledbutton');
    $("._nonkey").find('span').removeClass('disabledbutton');
    $("._nonkey").removeClass('disabledbutton');
    $('#btnSubmitHeader').removeClass('disabledbutton');
}


function openWindow(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
        $('#grdDetail').data('kendoGrid').removeRow(row);
        gridStatus = 'sudah-diapa-apain';
    });
    $("#noDetail").click(function () {
        wndDetail.close();
    });
}

function openWindowLeaveGrid(e) {
    e.preventDefault();
    wndLeaveGrid.open().center();
    $("#yesLeaveGrid").click(function () {
        wndLeaveGrid.close();
        sendData();
    });
    $("#noLeaveGrid").click(function () {
        gridStatus = 'belum-ngapa-ngapain';
        wndLeaveGrid.close();
    });
}

function openErrWindow(e) {
    err.open().center();
    $("#ok").click(function () {
        err.close();
    });
}

function openDelWindow(e) {
    del.open().center();
    $("#okDel").click(function () {
        del.close();
    });
}

function openDelDetailWindow(e) {
    del.open().center();
    $("#okDel").click(function () {
        del.close();
    });
}

function gridDestroy() {
    $("#grdDetail").data("kendoGrid").dataSource.read([]);
}

function btnPrintHeaderOnClick() {
    var viewer = $("#reportViewer").data("telerik_ReportViewer");
    viewer.reportSource({
        report: viewer.reportSource().report,
        parameters: { Agrowisata_HDR_ReservasiPerUnitId: reservasiPerUnitId }
    });
    viewer.refreshReport();
    prt.open().center();
}


/* Grid Buyer */

function grdOnChange(e) {
}

function grdOnEdit(e) {
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.Agrowisata_DTL_ReservasiPerUnitId = model.uid;
        model.Agrowisata_HDR_ReservasiPerUnitId = reservasiPerUnitId;
    }
    agrowisata_DTL_ReservasiPerUnitId = model.Agrowisata_DTL_ReservasiPerUnitId;
    publicholiday = model.StatusHoliday;
    statusLiburNasional = model.StatusLiburNasional;
    tanggalMasuk = model.TanggalMasuk;
    var NamaHari = new Array("Minggu", "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu");
    var sekarang = new Date(tanggalMasuk);
    HariIni = NamaHari[sekarang.getDay()];

    tanggalKeluar = model.TanggalKeluar;
    var dateStringBerangkat = new Date();
    dateStringBerangkat = tanggalMasuk.getFullYear() + "/" + (tanggalMasuk.getMonth() + 1) + "/" + (tanggalMasuk.getDate());
    var dateStringKembali = new Date();
    dateStringKembali = tanggalKeluar.getFullYear() + "/" + (tanggalKeluar.getMonth() + 1) + "/" + (tanggalKeluar.getDate());

    var hariAkhirPisah = dateStringKembali.split('/');
    var hariAwalPisah = dateStringBerangkat.split('/');
    var objek_tgl = new Date();
    var pisahTanggalAkhir = objek_tgl.setFullYear(parseInt(hariAkhirPisah[0]), parseInt(hariAkhirPisah[1] - 1), parseInt(hariAkhirPisah[2]));
    var pisahTanggalAwal = objek_tgl.setFullYear(parseInt(hariAwalPisah[0]), parseInt(hariAwalPisah[1] - 1), parseInt(hariAwalPisah[2]));

    //var jumlahHari;
    var dayberangkat = parseInt(hariAwalPisah[2]);
    var daypulang = parseInt(hariAkhirPisah[2]);
    var monthberangkat = parseInt(hariAwalPisah[1]);
    var monthpulang = parseInt(hariAkhirPisah[1]);

    if ((dayberangkat == daypulang) && (monthberangkat == monthpulang)) {
        jumlahHari = 0;
    }
    else {
        jumlahHari = (((((pisahTanggalAkhir - pisahTanggalAwal) / 1000) / 60) / 60) / 24);
    }

    gridStatus = 'sudah-diapa-apain';
}

function grdOnDataBinding(e) {
}

function filterdetailRead(e) {
    return { _menuId: menuId };
}

function filterdetailCreate(e) {
    return { _menuId: menuId };
}

function filterGridUpdate(e) {
    var mdl = e.models;
    var arr = [];
    for (var i = 0; i < mdl.length; i++) {
        arr.push(JSON.stringify(mdl[i]));
    }
    return {
        _model: arr,
        _menuId: menuId
    };
}

function filterGridDestroy(e) {
    return {
        _model: JSON.stringify(e),
        _menuId: menuId
    };
}

function sendData() {
    var grid = $("#grdDetail").data("kendoGrid"),
        parameterMap = grid.dataSource.transport.parameterMap;

    //get the new and the updated records
    var currentData = grid.dataSource.data();

    var updatedRecords = [];
    var newRecords = [];
    var spupdated = [];
    var spnew = [];

    for (var i = 0; i < currentData.length; i++) {
        if (currentData[i].isNew()) {
            newRecords.push(JSON.stringify(currentData[i]));
        } else if (currentData[i].dirty) {
            updatedRecords.push(JSON.stringify(currentData[i]));
        }
    }

    //this records are deleted
    var deletedRecords = [];
    var spdeleted = [];
    for (var i = 0; i < grid.dataSource._destroyed.length; i++) {
        deletedRecords.push(JSON.stringify(grid.dataSource._destroyed[i]));
    }

    var data = {};
    $.extend(data,
        parameterMap({ updated: updatedRecords }),
        parameterMap({ deleted: deletedRecords }),
        parameterMap({ new: newRecords }),
        parameterMap({ spupdated: spupdated }),
        parameterMap({ spdeleted: spdeleted }),
        parameterMap({ spnew: spnew }),
        parameterMap({ mnid: menuId })
    );

    _sendData(data).done(function (msg) {
        if (msg == "Success") {
            grid.dataSource.read([]);
            //disableHeader();
            gridStatus = 'belum-ngapa-ngapain';
        }
        else {
            $('#errMsg').text(msg);
            gridStatus = 'sudah-nyoba-disimpan-tapi-model-masih-salah';
            openErrWindow();
        }
    }).fail(function (x) {
        $('#errMsg').text(msg);
        openErrWindow();
        grid.dataSource.read([]);
        //disableHeader();
        gridStatus = 'sudah-nyoba-disimpan-tapi-gagal';
    });
    kirimNotifikasiEmail();
    $(".button").find('span').removeClass('disabledbutton');
    $(".button").removeClass('disabledbutton');
}

function _sendData(data) {
    return $.ajax({
        url: "/Input/UpdateCreateDelete",
        data: data,
        type: "POST"
    })
}

function filterTipeKamar(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(agrowisata_DTL_ReservasiPerUnitId)) == "undefined" ? grid.dataSource.getByUid(agrowisata_DTL_ReservasiPerUnitId) : grid.dataSource.get(agrowisata_DTL_ReservasiPerUnitId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    var kebunId = data.KebunId;
    return {
        value: $("#NamaKamar").val(),
        kebunId: kebunId
    };
}

function aucTipeKamarOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(agrowisata_DTL_ReservasiPerUnitId)) == "undefined" ? grid.dataSource.getByUid(agrowisata_DTL_ReservasiPerUnitId) : grid.dataSource.get(agrowisata_DTL_ReservasiPerUnitId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.TipeKamarId = ddlItem.TipeKamarId;
    tipeKamarId = ddlItem.TipeKamarId;
}

function aucTipeKamarOnDataBound(e) {

}

function filterKebun(e) {
    return {
        value: $("#NamaKebun").val()
    };
}

function aucNamaKebunOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(agrowisata_DTL_ReservasiPerUnitId)) == "undefined" ? grid.dataSource.getByUid(agrowisata_DTL_ReservasiPerUnitId) : grid.dataSource.get(agrowisata_DTL_ReservasiPerUnitId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.KebunId = ddlItem.KebunId;
    kebunId = ddlItem.KebunId;
}

function aucNamaKebunOnDataBound(e) {

}

function filterCustomer_Agrowisata(e) {
    var noidentitas = $("#NoIdentitas").val()
    return {
        value: noidentitas
    };
}

function aucCustomer_AgrowisataOnSelect(e) {
    var nama = this.dataItem(e.item);
    auccustomer_AgrowisataOnSelect(e).done(function (data) {
        if (data == 0) {
            alert("Customer/Pelanggan belum terdaftar!");
            $("#NoIdentitas").val('');
            $('#Nama').data('kendoAutoComplete').value('');
            $('#Customer_AgrowisataId').val('');
            $('#Telepon').data('kendoAutoComplete').value('');
            $('#Email').data('kendoAutoComplete').value('');
        }
        else {
            $('#Nama').data('kendoAutoComplete').value(data[0].Nama);
            $('#Customer_AgrowisataId').val(data[0].Customer_AgrowisataId);
            $('#Telepon').data('kendoAutoComplete').value(data[0].Telepon);
            $('#Email').data('kendoAutoComplete').value(data[0].Email);
        }
    }).fail(function (x) {
        alert('Error! Hubungi Bagian TI');
    });
}


function cekDiscount(e) {
    
}

function potonganDiskon(discount) {
    var url = '/Input/ExecSQL';
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(agrowisata_DTL_ReservasiPerUnitId)) == "undefined" ? grid.dataSource.getByUid(agrowisata_DTL_ReservasiPerUnitId) : grid.dataSource.get(agrowisata_DTL_ReservasiPerUnitId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    var kodeDiscount = data.KodeDiscount;
    var tanggalMasukCek = dataItem.TanggalMasuk;
    var dateStringMasuk = new Date();
    dateStringMasuk = tanggalMasukCek.getFullYear() + "/" + (tanggalMasukCek.getMonth() + 1) + "/" + (tanggalMasukCek.getDate());
    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            strClassView: "Ptpn8.Agrowisata.Models.View_ReservasiPerUnit",
            scriptSQL: "SELECT JenisDiscount, JumlahDiscount from [SPDK_KanpusEF].[dbo].[Discount] " +
                " where KodeDiscount='" + kodeDiscount + "' and (Tanggal_Akhir_Diskon >= '" + dateStringMasuk + "' and Tanggal_Awal_Diskon <='" + dateStringMasuk+"')",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function kirimNotifikasiEmail() {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            strClassView: "Ptpn8.Agrowisata.Models.View_ReservasiPerUnit",
            scriptSQL: "exec Agrowisata_NotifikasiEmail '" + kodebooking + "'",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function hargaKamar(hargaKamar) {

    var url = '/Input/ExecSQL';

    var daysOfYear = [];

    for (var d = new Date(tanggalMasuk); d < tanggalKeluar; d.setDate(d.getDate() + 1)) {
        daysOfYear.push(new Date(d));
    }

    var NamaHari = new Array("Minggu", "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu");
    var jumlahHariWeekend = 0;
    var jumlahHariWeekday = 0;
    for (var i = 0; i < daysOfYear.length; i++) {
        if (NamaHari[daysOfYear[i].getDay()] == 'Jumat' || NamaHari[daysOfYear[i].getDay()] == 'Sabtu') {
            jumlahHariWeekend++;
        }
        else jumlahHariWeekday++;
    }

    if (jenisPotongan == 'Prosentase') {
        if (publicholiday == 'Non Public Holiday' && (HariIni == 'Jumat' || HariIni == 'Sabtu' || statusLiburNasional == "Libur Nasional")) {
            return $.ajax({
                url: url,
                //contentType: 'application/json; charset=utf-8',
                data: {
                    strClassView: "Ptpn8.Agrowisata.Models.View_ReservasiPerUnit",
                    scriptSQL: "SELECT ((((HargaWeekend)*" + jumlahHari + ")-(((HargaWeekend)*" + jumlahHari + ")*" + potongan + "))) as SisaPembayaran from [ReferensiEF].[dbo].[TipeKamar] as A" +
                        " where TipeKamarId='" + tipeKamarId + "'",
                    _menuId: menuId
                },
                type: 'POST',
                datatype: 'json'
            });
        }
        else if (publicholiday == 'Public Holiday' && (HariIni == 'Jumat' || HariIni == 'Sabtu' || statusLiburNasional == "Libur Nasional")) {
            return $.ajax({
                url: url,
                //contentType: 'application/json; charset=utf-8',
                data: {
                    strClassView: "Ptpn8.Agrowisata.Models.View_ReservasiPerUnit",
                    scriptSQL: "SELECT (((((0.25*HargaWeekend)*" + jumlahHari + ")+(HargaWeekend)*" + jumlahHari + ")-(((0.25*HargaWeekend))+(HargaWeekend)*" + jumlahHari + ")*" + potongan + ")) as SisaPembayaran from [ReferensiEF].[dbo].[TipeKamar] as A " +
                        " where TipeKamarId='" + tipeKamarId + "'",
                    _menuId: menuId
                },
                type: 'POST',
                datatype: 'json'
            });
        }
        else {
            return $.ajax({
                url: url,
                //contentType: 'application/json; charset=utf-8',
                data: {
                    strClassView: "Ptpn8.Agrowisata.Models.View_ReservasiPerUnit",
                    scriptSQL: "SELECT ((((Harga)*" + jumlahHariWeekday + ")-(((Harga)*" + jumlahHariWeekday + ")*" + potongan + ") + ((HargaWeekend)*" + jumlahHariWeekend + ")-(((HargaWeekend)*" + jumlahHariWeekend + ")*" + potongan + "))) as SisaPembayaran from [ReferensiEF].[dbo].[TipeKamar] as A " +
                        " where TipeKamarId='" + tipeKamarId + "'",
                    _menuId: menuId
                },
                type: 'POST',
                datatype: 'json'
            });
        }
    }
    else if (jenisPotongan == 'Free Inap') {
        if (publicholiday == 'Non Public Holiday' && (HariIni == 'Jumat' || HariIni == 'Sabtu' || statusLiburNasional == "Libur Nasional")) {
            return $.ajax({
                url: url,
                //contentType: 'application/json; charset=utf-8',
                data: {
                    strClassView: "Ptpn8.Agrowisata.Models.View_ReservasiPerUnit",
                    scriptSQL: "SELECT (((HargaWeekend)*" + jumlahHari + "))*0 as SisaPembayaran from [ReferensiEF].[dbo].[TipeKamar] as A" +
                        " where TipeKamarId='" + tipeKamarId + "'",
                    _menuId: menuId
                },
                type: 'POST',
                datatype: 'json'
            });
        }
        else if (publicholiday == 'Public Holiday' && (HariIni == 'Jumat' || HariIni == 'Sabtu' || statusLiburNasional == "Libur Nasional")) {
            return $.ajax({
                url: url,
                //contentType: 'application/json; charset=utf-8',
                data: {
                    strClassView: "Ptpn8.Agrowisata.Models.View_ReservasiPerUnit",
                    scriptSQL: "SELECT ((((0.25*HargaWeekend)*" + jumlahHari + ")+(HargaWeekend)*" + jumlahHari + "))*0 as SisaPembayaran from [ReferensiEF].[dbo].[TipeKamar] as A" +
                        " where TipeKamarId='" + tipeKamarId + "'",
                    _menuId: menuId
                },
                type: 'POST',
                datatype: 'json'
            });
        }
        else {
            return $.ajax({
                url: url,
                //contentType: 'application/json; charset=utf-8',
                data: {
                    strClassView: "Ptpn8.Agrowisata.Models.View_ReservasiPerUnit",
                    scriptSQL: "SELECT (((Harga)*" + jumlahHari + "))*0 as SisaPembayaran from [ReferensiEF].[dbo].[TipeKamar] as A" +
                        " where TipeKamarId='" + tipeKamarId + "'",
                    _menuId: menuId
                },
                type: 'POST',
                datatype: 'json'
            });
        }
    }
    else if (jenisPotongan == 'Cash') {
        if (publicholiday == 'Non Public Holiday' && (HariIni == 'Jumat' || HariIni == 'Sabtu' || statusLiburNasional == "Libur Nasional")) {
            return $.ajax({
                url: url,
                //contentType: 'application/json; charset=utf-8',
                data: {
                    strClassView: "Ptpn8.Agrowisata.Models.View_ReservasiPerUnit",
                    scriptSQL: "SELECT (((HargaWeekend)*" + jumlahHari + "))-" + potongan + " as SisaPembayaran from [ReferensiEF].[dbo].[TipeKamar] as A" +
                        " where TipeKamarId='" + tipeKamarId + "'",
                    _menuId: menuId
                },
                type: 'POST',
                datatype: 'json'
            });
        }
        else if (publicholiday == 'Public Holiday' && (HariIni == 'Jumat' || HariIni == 'Sabtu' || statusLiburNasional == "Libur Nasional")) {
            return $.ajax({
                url: url,
                //contentType: 'application/json; charset=utf-8',
                data: {
                    strClassView: "Ptpn8.Agrowisata.Models.View_ReservasiPerUnit",
                    scriptSQL: "SELECT ((((0.25*HargaWeekend)*" + jumlahHari + ")+(HargaWeekend)*" + jumlahHari + "))-" + potongan + " as SisaPembayaran from [ReferensiEF].[dbo].[TipeKamar] as A" +
                        " where TipeKamarId='" + tipeKamarId + "'",
                    _menuId: menuId
                },
                type: 'POST',
                datatype: 'json'
            });
        }
        else {
            return $.ajax({
                url: url,
                //contentType: 'application/json; charset=utf-8',
                data: {
                    strClassView: "Ptpn8.Agrowisata.Models.View_ReservasiPerUnit",
                    scriptSQL: "SELECT (((Harga)*" + jumlahHariWeekday + ") + ((HargaWeekend)*" + jumlahHariWeekend + "))-" + potongan + " as SisaPembayaran from [ReferensiEF].[dbo].[TipeKamar] as A" +
                        " where TipeKamarId='" + tipeKamarId + "'",
                    _menuId: menuId
                },
                type: 'POST',
                datatype: 'json'
            });
        }
    }
    else {
        if (publicholiday == 'Non Public Holiday' && (HariIni == 'Jumat' || HariIni == 'Sabtu' || statusLiburNasional == "Libur Nasional")) {
            return $.ajax({
                url: url,
                //contentType: 'application/json; charset=utf-8',
                data: {
                    strClassView: "Ptpn8.Agrowisata.Models.View_ReservasiPerUnit",
                    scriptSQL: "SELECT (((HargaWeekend)*" + jumlahHari + ")) as SisaPembayaran from [ReferensiEF].[dbo].[TipeKamar] as A" +
                        " where TipeKamarId='" + tipeKamarId + "'",
                    _menuId: menuId
                },
                type: 'POST',
                datatype: 'json'
            });
        }
        else if (publicholiday == 'Public Holiday' && (HariIni == 'Jumat' || HariIni == 'Sabtu' || statusLiburNasional == "Libur Nasional")) {
            return $.ajax({
                url: url,
                //contentType: 'application/json; charset=utf-8',
                data: {
                    strClassView: "Ptpn8.Agrowisata.Models.View_ReservasiPerUnit",
                    scriptSQL: "SELECT ((((0.25*HargaWeekend)*" + jumlahHari + ")+(HargaWeekend)*" + jumlahHari + ")) as SisaPembayaran from [ReferensiEF].[dbo].[TipeKamar] as A" +
                        " where TipeKamarId='" + tipeKamarId + "'",
                    _menuId: menuId
                },
                type: 'POST',
                datatype: 'json'
            });
        }
        else {
            return $.ajax({
                url: url,
                //contentType: 'application/json; charset=utf-8',
                data: {
                    strClassView: "Ptpn8.Agrowisata.Models.View_ReservasiPerUnit",
                    scriptSQL: "SELECT (((Harga)*" + jumlahHariWeekday + ") + ((HargaWeekend)*" + jumlahHariWeekend + ")) as SisaPembayaran from [ReferensiEF].[dbo].[TipeKamar] as A" +
                        " where TipeKamarId='" + tipeKamarId + "'",
                    _menuId: menuId
                },
                type: 'POST',
                datatype: 'json'
            });
        }
    }
}

function aucCustomer_AgrowisataOnDataBound(e) {

}

function auccustomer_AgrowisataOnSelect(e) {
    var url = '/Input/GetDataFrom';
    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            strClassView: "Ptpn8.Agrowisata.Models.View_ReservasiPerUnit",
            scriptSQL: "SELECT DISTINCT Nama, Customer_AgrowisataId, Telepon, Email from [ReferensiEF].[dbo].[Customer_Agrowisata] as A" +
                " where NoIdentitas='" + $('#NoIdentitas').val() + "'",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function makeid() {
    var text = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    for (var i = 0; i < 6; i++)
        text += possible.charAt(Math.floor(Math.random() * possible.length));

    return text;
}

function ddlPublicHolidayOnChange(e) {
    var publicHoliday = $('#publicHoliday').val();
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(agrowisata_DTL_ReservasiPerUnitId)) == "undefined" ? grid.dataSource.getByUid(agrowisata_DTL_ReservasiPerUnitId) : grid.dataSource.get(agrowisata_DTL_ReservasiPerUnitId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
}
