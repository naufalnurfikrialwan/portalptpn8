var reservasiId, dtl_ReservasiId;
var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var model;
var headerBaru, detailBaru;
var gridStatus;
var mainBudidayaId;
var menuId = '6b775bd6-d04f-4657-80d3-bc4851cdb0f8';

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
});

angular.module("__header", ["kendo.directives"])
    .controller("MyCtrl", function ($scope) {
        $scope.validate = function (event) {
            event.preventDefault();
            if ($scope.validator.validate()) {
                enableHeader();
                tahun = $('#Tahun').val();
                bulan = $('#Bulan').data('kendoDropDownList').value();
                $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                $('#_detail').removeClass('disabledbutton');
                $("#grdDetail").data("kendoGrid").dataSource.read({
                    id: $('#ReservasiId').val()
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

function grdOnDataBound(e) {
    var rows = e.sender.tbody.children();
    for (var j = 0; j < rows.length; j++) {
        var row = $(rows[j]);
        var dataItem = e.sender.dataItem(row);

        //var grid = $('#grdDetail').data('kendoGrid');
        //var dataItem = typeof (grid.dataSource.get(reservasiId)) == "undefined" ? grid.dataSource.getByUid(reservasiId) : grid.dataSource.get(reservasiId);
        var tanggalReservasiCek = dataItem.TanggalReservasi;
        var dateStringReservasi = new Date();
        dateStringReservasi = tanggalReservasiCek.getFullYear() + "/" + (tanggalReservasiCek.getMonth() + 1) + "/" + (tanggalReservasiCek.getDate());
        var today = new Date();
        today = today.getFullYear() + "/" + (today.getMonth() + 1) + "/" + (today.getDate());

        var tglReservasiPisah = dateStringReservasi.split('/');
        var todayPisah = today.split('/');
        var objek_tgl = new Date();
        var pisahReservasi = objek_tgl.setFullYear(parseInt(tglReservasiPisah[0]), parseInt(tglReservasiPisah[1] - 1), parseInt(tglReservasiPisah[2]));
        var pisahToday = objek_tgl.setFullYear(parseInt(todayPisah[0]), parseInt(todayPisah[1] - 1), parseInt(todayPisah[2]));
        var jumlahHari = (((((pisahToday - pisahReservasi) / 1000) / 60) / 60) / 24);


        if (dataItem.get("StatusReservasi") == "BOOKING" && (jumlahHari < 3 && jumlahHari > 1)) {
            row.addClass("jatuhTempo");
        } else if (dataItem.get("StatusReservasi") == "BOOKING" && jumlahHari > 2) {
            row.addClass("lebihJatuhTempo");
        } else if (dataItem.get("StatusReservasi") == "BOOKING" && jumlahHari < 2) {
            row.addClass("kurangDariJatuhTempo");
        }
    }
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

function sendEmail(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");

    var data = grid.dataItem(row);
    var kodebookingemail = data.KodeBooking;

    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            strClassView: "Ptpn8.Agrowisata.Models.View_Reservasi",
            scriptSQL: "exec Agrowisata_NotifikasiEmail '" + kodebookingemail + "'",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
    alert('Email Notifikasi Telah Terkirim...');
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

function grdDetailReservasiOnEdit(e)
    {
        var model = e.model;
        if (model.isNew()) {
            model.id = model.uid;
            e.container.find("input[name=DTL_ReservasiId]").val(model.id).change(); // trigger change in order to notify the model binding
            e.container.find("input[name=ReservasiId]").val(reservasiId).change(); // trigger change in order to notify the model binding
        }
        uid = model.uid;
        dtl_ReservasiId = model.id;
    }

function grdDetailReservasiOnDataBound(e)
    {
    }

function filterRead() {
    return {
        _menuId: menuId, // teh
        _filter: ["Year(TanggalReservasi)", "" + tahun + "", "Month(TanggalReservasi)", "" + bulan + ""]
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