var reservasiId, err;
var wndLeaveGrid, wnd, wndDetail, prt, del;
var detailBaru = false;
var userName;
var kodeBooking, uangMukaId;
var menuId = '46620c51-d0bd-4770-99ab-82bdc5040847';

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
                if (input.attr("name") == "UangMukaDisetor" && input.val() != "") {
                    var grid = $('#grdDetail').data('kendoGrid');
                    var dataItem = typeof (grid.dataSource.get(uangMukaId)) == "undefined" ? grid.dataSource.getByUid(uangMukaId) : grid.dataSource.get(uangMukaId);
                    dataItem.SisaPelunasan = dataItem.JmlYangHarusDibayar - dataItem.UangMukaDisetor;
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
                getDataFrom().done(function (data) {
                    if (data == 0) {
                        InsertGridFrom().done(function (data) {
                            $("#grdDetail").data("kendoGrid").dataSource.read({ id: kodeBooking });
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
                            getDataFromUangMukaSelanjutnya().done(function (data) {
                                if (data == 0) {
                                    InsertGridFromUangMukaSelanjutnya().done(function (data) {
                                        $("#grdDetail").data("kendoGrid").dataSource.read({ id: kodeBooking });
                                    }).fail(function (x) {
                                        alert('Error! Hubungi Bagian TI');
                                    });
                                }
                            }).fail(function (x) {
                                alert('Error! Hubungi Bagian TI');
                            });
                        });
                    }
                }).fail(function (x) {
                    alert('Error! Hubungi Bagian TI');
                });
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
            strClassView: "Ptpn8.Agrowisata.Models.View_UangMuka",
            scriptSQL: "declare @@reservasiId uniqueidentifier " +
                " set @@reservasiId = (Select top 1 ReservasiId  from [SPDK_KanpusEF].dbo.Reservasi where KodeBooking='" + kodeBooking + "') " +
                " declare @@HargaFasilitasTambahan decimal (18,0) " +
                " set @@HargaFasilitasTambahan = (select SUM(harga) from [SPDK_KanpusEF].[dbo].[Agrowisata_DTLReservasiTambahanFasilitas] where ReservasiId = @@reservasiId ) " +
                " INSERT INTO [SPDK_KanpusEF].[dbo].[Agrowisata_UangMuka] " +
                " SELECT newid() as UangMukaId, A.KodeBooking, A.MetodePembayaran, case when @@HargaFasilitasTambahan is null then sum(b.harga) else sum(b.harga)+@@HargaFasilitasTambahan end as JmlYangHarusDibayar,0 as UangMukaDisetor,0 as SisaPelunasan," +
                " '' as Keterangan, A.ReservasiId, A.Customer_AgrowisataId, C.Nama as NamaCustomer,GETDATE () as TanggalUangMuka " +
                " from [SPDK_KanpusEF].[dbo].[Reservasi] as A join [SPDK_KanpusEF].[dbo].[DTL_Reservasi] as B on A.ReservasiId=B.ReservasiId" +
                " join [ReferensiEF].[dbo].[Customer_Agrowisata] as C on A.Customer_AgrowisataId=C.Customer_AgrowisataId" +
                " where A.KodeBooking = '" + kodeBooking + "'" +
                " group by A.ReservasiId, A.KodeBooking, A.MetodePembayaran, A.Customer_AgrowisataId,C.Nama ",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function getDataFromUangMukaSelanjutnya() {
    // Hitung Jumlah Record di Detail
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[Agrowisata_Pelunasan]" +
                " where KodeBooking='" + kodeBooking + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}


function InsertGridFromUangMukaSelanjutnya() {
    var grd = $('#grdDetail').data('kendoGrid');
    var url = '/Input/GetDataFrom';

    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            strClassView: "Ptpn8.Agrowisata.Models.View_UangMuka",
            scriptSQL: "INSERT INTO [SPDK_KanpusEF].[dbo].[Agrowisata_UangMuka] " +
                " SELECT top 1 newid() as UangMukaId, A.KodeBooking, A.MetodePembayaran, B.SisaPelunasan,0 as UangMukaDisetor,0 as SisaPelunasan," +
                " '' as Keterangan, A.ReservasiId, A.Customer_AgrowisataId, C.Nama as NamaCustomer,GETDATE () as TanggalUangMuka " +
                " from [SPDK_KanpusEF].[dbo].[Reservasi] as A join [SPDK_KanpusEF].[dbo].[Agrowisata_UangMuka] as B on A.ReservasiId=B.ReservasiId" +
                " join [ReferensiEF].[dbo].[Customer_Agrowisata] as C on A.Customer_AgrowisataId=C.Customer_AgrowisataId" +
                " where A.KodeBooking = '" + kodeBooking + "' order by B.TanggalUangMuka desc",
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
        model.UangMukaId = model.uid;
        e.container.find("input[name=ReservasiId]").val(model.id).change();
    }
    uangMukaId = model.UangMukaId;
    $(e.container).find("input[name='SisaPelunasan']").prop('disabled', true);
    getUserName().done(function (data) {
        if (data) {
            userName = data;
        }
        else {
        }
    }).fail(function (x) {
        alert('Error! Hubungi Bagian TI');
    });
}

function btnPrintHeaderOnClick(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");

    var data = grid.dataItem(row);
    var uangMukaId = data.UangMukaId;

    var viewer = $("#reportViewer").data("telerik_ReportViewer");
    viewer.reportSource({
        report: viewer.reportSource().report,
        parameters: { UangMukaId: uangMukaId }
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
    var spdeleted = [];
    var spnew = [];

    for (var i = 0; i < currentData.length; i++) {
        if (currentData[i].isNew()) {
            //this record is new
            newRecords.push(JSON.stringify(currentData[i]));
        } else if (currentData[i].dirty) {
            updatedRecords.push(JSON.stringify(currentData[i]));
            spupdated.push("EXEC Agrowisata_UbahStatusReservasiUangMuka '" + kodeBooking + "'");
        }
    }

    //this records are deleted
    var deletedRecords = [];
    for (var i = 0; i < grid.dataSource._destroyed.length; i++) {
        deletedRecords.push(JSON.stringify(grid.dataSource._destroyed[i]));
        spdeleted.push("EXEC Agrowisata_UbahStatusReservasiUangMukaBatal '" + kodeBooking + "'");
    }

    var data = {};
    $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }), parameterMap({ new: newRecords }), parameterMap({ mnid: menuId }), parameterMap({ spupdated: spupdated }), parameterMap({ spdeleted: spdeleted }));
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
