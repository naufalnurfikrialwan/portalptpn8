var reservasiId, dtl_ReservasiId;
var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var model;
var headerBaru, detailBaru;
var gridStatus;
var mainBudidayaId;
var menuId = '18f25fdb-fbc8-44ff-96d3-182fedc213c3';
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
                                    var grid = $('#grdDetailReservasi_' + reservasiId).data('kendoGrid');
                                    var dataItem = typeof (grid.dataSource.get(dtl_ReservasiId)) == "undefined" ? grid.dataSource.getByUid(dtl_ReservasiId) : grid.dataSource.get(dtl_ReservasiId);
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
                                    var grid = $('#grdDetailReservasi_' + reservasiId).data('kendoGrid');
                                    var dataItem = typeof (grid.dataSource.get(dtl_ReservasiId)) == "undefined" ? grid.dataSource.getByUid(dtl_ReservasiId) : grid.dataSource.get(dtl_ReservasiId);
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
                } else {
                    return true;
                };
            }
        }
    });
});

angular.module("__header", ["kendo.directives"])
    .controller("MyCtrl", function ($scope) {
        $scope.validate = function (event) {
            event.preventDefault();
            if ($scope.validator.validate()) {
                enableHeader();
                kodeBooking = $('#eVoucher').val();
                $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                $('#_detail').removeClass('disabledbutton');
                $("#grdDetail").data("kendoGrid").dataSource.read({
                    id: kodeBooking
                });
            } else {
                $(".k-button-icontext").addClass("k-state-disabled").removeClass("k-grid-add"); // edited
                gridDestroy();
            }
        }
    });



$(window).on("beforeunload", function () {
    return "Please don't leave me!";
});

$(window).on("unload", function () {
    hapusPenggunaPortalYangAktif();
});

function renderNumber(data) {
    var no = ++rowNumber;
    data.NoBaris = no;
    return no;
}
function resetRowNumber(e) {
    rowNumber = 0;
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

function disableHeader() {
    $("._key").find('span').addClass('disabledbutton');
    $("._key").addClass('disabledbutton');
    $("._nonkey").find('span').addClass('disabledbutton');
    $("._nonkey").addClass('disabledbutton');
    $('#btnSubmitHeader').addClass('disabledbutton');
    $('#btnPrintHeader').addClass('disabledbutton');
    $('#_detail').addClass('disabledbutton');
}

function enableHeader() {
    $("._key").find('span').removeClass('disabledbutton');
    $("._key").removeClass('disabledbutton');
    $("._nonkey").find('span').removeClass('disabledbutton');
    $("._nonkey").removeClass('disabledbutton');
    $('#btnSubmitHeader').removeClass('disabledbutton');
    $('#btnPrintHeader').removeClass('disabledbutton');
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

function openWindowDetail(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
        $('#grdDetailReservasi_' + reservasiId).data('kendoGrid').removeRow(row);
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

function btnPrintHeaderOnClick(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");

    var data = grid.dataItem(row);
    var reservasiId = data.ReservasiId;

    var viewer = $("#reportViewer").data("telerik_ReportViewer");
    viewer.reportSource({
        report: viewer.reportSource().report,
        parameters: { ReservasiId: reservasiId }
    });
    viewer.refreshReport();
    prt.open().center();
}

function gridDestroy() {
    $("#grdDetail").data("kendoGrid").dataSource.read([]);
}

function grdOnDetailExpand(e) {
    reservasiId = this.dataItem(e.masterRow).ReservasiId;
    var grd2 = $('#grdDetailReservasi_' + reservasiId).data('kendoGrid');
    grd2.dataSource.read();
}

function grdOnChange(e) {
}

function grdOnEdit(e) {
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.ReservasiId = model.uid;
    }
    reservasiId = model.ReservasiId;
    gridStatus = 'sudah-diapa-apain';

}

function grdOnDataBinding(e) {
}

function grdDetailReservasiOnEdit(e) {
    var model = e.model;
    if (model.isNew()) {
        model.id = model.uid;
        e.container.find("input[name=DTL_ReservasiId]").val(model.id).change(); // trigger change in order to notify the model binding
        e.container.find("input[name=ReservasiId]").val(reservasiId).change(); // trigger change in order to notify the model binding
    }
    uid = model.uid;
    dtl_ReservasiId = model.id;

    publicholiday = model.StatusHoliday;
    statusLiburNasional = model.StatusLiburNasional;
    tanggalMasuk = model.TanggalMasuk;
    var NamaHari = new Array("Minggu", "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu");
    var sekarang = new Date(tanggalMasuk);
    HariIni = NamaHari[sekarang.getDay()];

    tanggalKeluar = model.TanggalKeluar;
    var dateStringBerangkat = new Date();
    dateStringBerangkat = tanggalMasuk.getFullYear() + "/" + (tanggalMasuk.getMonth() + 1) + "/" + (tanggalMasuk.getDate())
    var dateStringKembali = new Date();
    dateStringKembali = tanggalKeluar.getFullYear() + "/" + (tanggalKeluar.getMonth() + 1) + "/" + (tanggalKeluar.getDate())

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
}

function grdDetailReservasiOnDataBound(e) {
}

function filterRead() {
    return {
        id: kodeBooking,
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

    for (var i = 0; i < currentData.length; i++) {
        if (currentData[i].isNew()) {
            //this record is new
            newRecords.push(JSON.stringify(currentData[i]));
        } else if (currentData[i].dirty) {
            updatedRecords.push(JSON.stringify(currentData[i]));
        }
    }

    //this records are deleted
    var deletedRecords = [];
    for (var i = 0; i < grid.dataSource._destroyed.length; i++) {
        deletedRecords.push(JSON.stringify(grid.dataSource._destroyed[i]));
    }

    var data = {};
    $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }), parameterMap({ new: newRecords }), parameterMap({ mnid: menuId }));
    _sendData(data).done(function (msg) {
        if (msg == "Success") {
            grid.dataSource.read([]);
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
        gridStatus = 'sudah-nyoba-disimpan-tapi-gagal';
    });
}

function _sendData(data) {
    return $.ajax({
        url: "/Input/UpdateCreateDelete",
        data: data,
        type: "POST"
    })
}

function TahunBukuOnChange(e) {
    if (gridStatus == 'sudah-nyoba-disimpan-tapi-model-masih-salah' ||
        gridStatus == 'sudah-diapa-apain') {
        openWindowLeaveGrid(e);
    }
    else {
        $('#msgInputView').text('');
        tahunBukuOnChange(e).done(function (data) {
            if (data) {
                $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                    disableHeader();
                    $(".k-button-icontext").addClass("k-state-disabled").removeClass("k-grid-add"); // edited
                    gridDestroy();
                });
            }
            else {
                alert("Error! Hubungi Bagian TI")
            }
        }).fail(function (x) {
            alert('Error! Hubungi Bagian TI');
        });
    }
}

function ddlPublicHolidayOnChange(e) {
    var publicHoliday = $('#publicHoliday').val();
    var grid = $('#grdDetailReservasi_' + reservasiId).data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_ReservasiId)) == "undefined" ? grid.dataSource.getByUid(dtl_ReservasiId) : grid.dataSource.get(dtl_ReservasiId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
}

function cekDiscount(e) {

}

function potonganDiskon(discount) {
    var url = '/Input/ExecSQL';
    var grid = $('#grdDetailReservasi_' + reservasiId).data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_ReservasiId)) == "undefined" ? grid.dataSource.getByUid(dtl_ReservasiId) : grid.dataSource.get(dtl_ReservasiId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    var kodeDiscount = data.KodeDiscount;
    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            strClassView: "Ptpn8.Agrowisata.Models.View_Reservasi",
            scriptSQL: "SELECT JenisDiscount, JumlahDiscount from [SPDK_KanpusEF].[dbo].[Discount] " +
                " where KodeDiscount='" + kodeDiscount + "'",
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
                    strClassView: "Ptpn8.Agrowisata.Models.View_Reservasi",
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
                    strClassView: "Ptpn8.Agrowisata.Models.View_Reservasi",
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
                    strClassView: "Ptpn8.Agrowisata.Models.View_Reservasi",
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
                    strClassView: "Ptpn8.Agrowisata.Models.View_Reservasi",
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
                    strClassView: "Ptpn8.Agrowisata.Models.View_Reservasi",
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
                    strClassView: "Ptpn8.Agrowisata.Models.View_Reservasi",
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
                    strClassView: "Ptpn8.Agrowisata.Models.View_Reservasi",
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
                    strClassView: "Ptpn8.Agrowisata.Models.View_Reservasi",
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
                    strClassView: "Ptpn8.Agrowisata.Models.View_Reservasi",
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
                    strClassView: "Ptpn8.Agrowisata.Models.View_Reservasi",
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
                    strClassView: "Ptpn8.Agrowisata.Models.View_Reservasi",
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
                    strClassView: "Ptpn8.Agrowisata.Models.View_Reservasi",
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

function filterTipeKamar(e) {
    var grid = $('#grdDetailReservasi_' + reservasiId).data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_ReservasiId)) == "undefined" ? grid.dataSource.getByUid(dtl_ReservasiId) : grid.dataSource.get(dtl_ReservasiId);
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
    var grid = $('#grdDetailReservasi_' + reservasiId).data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_ReservasiId)) == "undefined" ? grid.dataSource.getByUid(dtl_ReservasiId) : grid.dataSource.get(dtl_ReservasiId);
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
    var grid = $('#grdDetailReservasi_' + reservasiId).data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_ReservasiId)) == "undefined" ? grid.dataSource.getByUid(dtl_ReservasiId) : grid.dataSource.get(dtl_ReservasiId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.ReservasiId = reservasiId;
    data.DTL_ReservasiId = dtl_ReservasiId;
    data.KebunId = ddlItem.KebunId;
    kebunId = ddlItem.KebunId;
}

function aucNamaKebunOnDataBound(e) {

}

function sendDataDTL() {

    var grid = $("#grdDetailReservasi_" + reservasiId).data("kendoGrid"),
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
        parameterMap({ mnid: menuId }),
        parameterMap({ classdv: 'Ptpn8.Agrowisata.Models.ViewDTL_Reservasi' })
        );

    _sendData(data).done(function (msg) {
        if (msg == "Success") {
            grid.dataSource.read();
            //gridStatus = 'belum-ngapa-ngapain';
        }
        else {
            $('#errMsg').text(msg);
            //gridStatus = 'sudah-nyoba-disimpan-tapi-model-masih-salah';
            openErrWindow();
        }
    }).fail(function (x) {
        $('#errMsg').text(msg);
        openErrWindow();
        grid.dataSource.read([]);
        //gridStatus = 'sudah-nyoba-disimpan-tapi-gagal';
    });

}