var reservasiId, err;
var detailBaru = false;
var userName;
var kodeBooking, cancelReservasiId, jumlahHari;
var menuId = '95adf7c9-b7f3-42e5-9e26-3179202d70c7';

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
    prt = $("#printWindow").kendoWindow({
        title: "Print",
        modal: true,
        visible: false,
        resizable: false,
        width: 800,
        height: 500
    }).data("kendoWindow");

    wndDetail = $("#konfirmasiDetail").kendoWindow({
        title: "Konfirmasi",
        modal: true,
        visible: false,
        resizable: false,
        width: 300
    }).data("kendoWindow");

    del = $("#delWindow").kendoWindow({
        title: "Hapus Header",
        modal: true,
        visible: false,
        resizable: false,
        width: 400,
        height: 150
    }).data("kendoWindow");
    
    err = $("#errWindow").kendoWindow({
        title: "Error Data",
        modal: true,
        visible: false,
        resizable: false,
        width: 300
    }).data("kendoWindow");
    $("#grdDetail").kendoValidator({
        rules: { // custom rules
            chopvalidation: function (input) {
                if (input.attr("name") == "TanggalCancelReservasi" || input.attr("name") == "TanggalCheckIn") {
                    var grid = $('#grdDetail').data('kendoGrid');
                    var dataItem = typeof (grid.dataSource.get(cancelReservasiId)) == "undefined" ? grid.dataSource.getByUid(cancelReservasiId) : grid.dataSource.get(cancelReservasiId);
                    if (jumlahHari > 30) {
                        dataItem.Pengembalian = dataItem.UangYangSudahMasuk;
                        dataItem.SisaPengembalian = dataItem.UangYangSudahMasuk - dataItem.Pengembalian;
                        grid.refresh();
                    }
                    else if (jumlahHari < 31 && jumlahHari > 13) {
                        dataItem.Pengembalian = 0.5*dataItem.UangYangSudahMasuk;
                        dataItem.SisaPengembalian = dataItem.UangYangSudahMasuk - dataItem.Pengembalian;
                        grid.refresh();
                    }
                    else if (jumlahHari < 14) {
                        dataItem.Pengembalian = 0 * dataItem.UangYangSudahMasuk;
                        dataItem.SisaPengembalian = dataItem.UangYangSudahMasuk - dataItem.Pengembalian;
                        grid.refresh();
                    }
                } else {
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

angular.module("__header", ["kendo.directives"])
    .controller("MyCtrl", function ($scope) {
        $scope.validate = function (event) {
            event.preventDefault();
            if ($scope.validator.validate()) {
                kodeBooking = $('#eVoucher').val();
                $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                $('#grdDetail').removeClass('disabledbutton');
                //-------------------------------------------------------------------------------------------
                getDataFrom().done(function (data) {
                    if (data == 0) {
                        getDataFromUangMuka().done(function (data2) {
                            if (data2 == 0) {
                                InsertGridFromTanpaUangMuka().done(function (data3) {
                                    $("#grdDetail").data("kendoGrid").dataSource.read({ id: kodeBooking });
                                }).fail(function (x) {
                                    alert('Error! Hubungi Bagian TI');
                                });
                            }
                            else {
                                InsertGridFrom().done(function (data4) {
                                    $("#grdDetail").data("kendoGrid").dataSource.read({ id: kodeBooking });
                                }).fail(function (x) {
                                    alert('Error! Hubungi Bagian TI');
                                });
                            }
                        }).fail(function (x) {
                            alert('Error! Hubungi Bagian TI');
                        });
                    }
                    else {
                        var grid = $("#grdDetail").data("kendoGrid");
                        grid.dataSource.read({
                            id: kodeBooking
                        }).done(function () {
                            var currentData = grid.dataSource.data();
                            for (var i = 0; i < currentData.length; i++) {
                                currentData[i].dirty = true;
                            }
                        });
                    }
                }).fail(function (x) {
                    alert('Error! Hubungi Bagian TI');
                });
                //------------------------------------------------------------------------------
            } else {
                $(".k-button-icontext").addClass("k-state-disabled").removeClass("k-grid-add"); // edited
                gridDestroy();
            }
        }
    });


function getDataFrom() {
    // Hitung Jumlah Record di Detail
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[Agrowisata_CancelReservasi]" +
                " where KodeBooking='" + kodeBooking + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function getDataFromUangMuka() {
    // Hitung Jumlah Record di Detail
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[Agrowisata_UangMuka]" +
                " where KodeBooking='" + kodeBooking + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function InsertGridFrom() {
    var grd = $('#grdDetail').data('kendoGrid');
    var url = '/Input/GetDataFrom';

    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            strClassView: "Ptpn8.Agrowisata.Models.View_CancelReservasi",
            scriptSQL: "declare @@reservasiId uniqueidentifier " +
                " set @@reservasiId = (Select top 1 ReservasiId  from [SPDK_KanpusEF].dbo.Reservasi where KodeBooking='" + kodeBooking + "') " +
                " declare @@TotalUangMuka decimal (18,0) " +
                " set @@TotalUangMuka = (select sum([UangMukaDisetor]) from [SPDK_KanpusEF].[dbo].[Agrowisata_UangMuka] where ReservasiId = @@reservasiId ) " +
                "INSERT INTO [SPDK_KanpusEF].[dbo].[Agrowisata_CancelReservasi] " +
                " SELECT top 1 newid() as CancelReservasiId, A.ReservasiId,A.Customer_AgrowisataId,GETDATE () as TanggalCancelReservasi,  C.Nama as NamaCustomer,A.KodeBooking, A.MetodePembayaran, @@TotalUangMuka+E.PelunasanDisetor as UangYangSudahMasuk,0 as Pengembalian,0 as SisaPengembalian," +
                " '' as Keterangan, B.TanggalMasuk as TanggalCheckIn " +
                " from [SPDK_KanpusEF].[dbo].[Reservasi] as A join [SPDK_KanpusEF].[dbo].[DTL_Reservasi] as B on A.ReservasiId=B.ReservasiId" +
                " join [ReferensiEF].[dbo].[Customer_Agrowisata] as C on A.Customer_AgrowisataId=C.Customer_AgrowisataId" +
                " join [SPDK_KanpusEF].[dbo].[Agrowisata_UangMuka] as D on A.ReservasiId=D.ReservasiId" +
                " join [SPDK_KanpusEF].[dbo].[Agrowisata_Pelunasan] as E on A.ReservasiId=E.ReservasiId" +
                " where A.KodeBooking = '" + kodeBooking + "'" +
                " group by A.ReservasiId, A.KodeBooking, A.MetodePembayaran, A.Customer_AgrowisataId,C.Nama,D.UangMukaDisetor,E.PelunasanDisetor,B.TanggalMasuk ",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function InsertGridFromTanpaUangMuka() {
    var grd = $('#grdDetail').data('kendoGrid');
    var url = '/Input/GetDataFrom';

    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            strClassView: "Ptpn8.Agrowisata.Models.View_CancelReservasi",
            scriptSQL: 
                " INSERT INTO [SPDK_KanpusEF].[dbo].[Agrowisata_CancelReservasi] " +
                " SELECT newid() as CancelReservasiId, A.ReservasiId,A.Customer_AgrowisataId,GETDATE () as TanggalCancelReservasi,  C.Nama as NamaCustomer,A.KodeBooking, A.MetodePembayaran, E.PelunasanDisetor as UangYangSudahMasuk,0 as Pengembalian,0 as SisaPengembalian," +
                " '' as Keterangan, B.TanggalMasuk as TanggalCheckIn " +
                " from [SPDK_KanpusEF].[dbo].[Reservasi] as A join [SPDK_KanpusEF].[dbo].[DTL_Reservasi] as B on A.ReservasiId=B.ReservasiId" +
                " join [ReferensiEF].[dbo].[Customer_Agrowisata] as C on A.Customer_AgrowisataId=C.Customer_AgrowisataId" +
                " join [SPDK_KanpusEF].[dbo].[Agrowisata_Pelunasan] as E on A.ReservasiId=E.ReservasiId" +
                " where A.KodeBooking = '" + kodeBooking + "'" +
                " group by A.ReservasiId, A.KodeBooking, A.MetodePembayaran, A.Customer_AgrowisataId,C.Nama,E.PelunasanDisetor,B.TanggalMasuk ",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function grdOnChange(e) {
}

function grdOnEdit(e) {
    var model = e.model;
    uid = model.uid;
    this.expandRow(this.tbody.find("tr[data-uid='" + uid + "']"));
    if (model.isNew()) {
        model.CancelReservasiId = model.uid;
        e.container.find("input[name=ReservasiId]").val(model.id).change();
    }
    cancelReservasiId = model.CancelReservasiId;
    $(e.container).find("input[name='SisaPengembalian']").prop('disabled', true);
    getUserName().done(function (data) {
        if (data) {
            userName = data;
        }
        else {
        }
    }).fail(function (x) {
        alert('Error! Hubungi Bagian TI');
    });
    var tanggalMasuk = model.TanggalCancelReservasi;
    var tanggalKeluar = model.TanggalCheckIn;
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

function getUserName() {
    var url = '/Input/GetUserName';
    return $.ajax({
        url: url,
        data: {},
        type: 'GET',
        datatype: 'json'
    })
}

function grdOnDataBound(e) {
    $("#grdDetail").find(".k-icon.k-i-collapse").trigger("click");
}

function filterGridUpdate(e) {
    var mdl = e.models;
    var arr = [];
    for (var i = 0; i < mdl.length; i++) {
        var m = mdl[i];
        if (m.VerKaur == true)
            m.UserNameKaur = userName;
        else
            m.UserNameKaur = "";
        arr.push(JSON.stringify(m));
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
            //this record is new
            newRecords.push(JSON.stringify(currentData[i]));
        } else if (currentData[i].dirty) {
            updatedRecords.push(JSON.stringify(currentData[i]));
            spupdated.push("EXEC Agrowisata_UbahStatusReservasiCancelReservasi '" + kodeBooking + "'");
        }
    }

    //this records are deleted
    var deletedRecords = [];
    for (var i = 0; i < grid.dataSource._destroyed.length; i++) {
        deletedRecords.push(JSON.stringify(grid.dataSource._destroyed[i]));
    }

    var data = {};
    $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }), parameterMap({ new: newRecords }), parameterMap({ mnid: menuId }), parameterMap({ spupdated: spupdated }));
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

function openErrWindow(e) {
    err.open().center();
    $("#ok").click(function () {
        err.close();
    });
}

function _sendData(data) {
    return $.ajax({
        url: "/Input/UpdateCreateDelete",
        data: data,
        type: "POST"
    })
}
function filterdetailRead(e) {
    return {
        id: kodeBooking,
        _menuId: menuId
    };
}

function filterdetailCreate(e) {
    return { _menuId: menuId };
}
