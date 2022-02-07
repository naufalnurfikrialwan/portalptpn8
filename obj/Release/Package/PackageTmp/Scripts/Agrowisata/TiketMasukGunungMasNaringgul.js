var tiketId=guid(), err, berangkat, berangkatdasar, month, year, date;
var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var detailBaru = false;
var userName;
var menuId = '1afd0bbf-7a88-4a04-958e-288565c4ef49';
var model;
var headerBaru, detailBaru;
var gridStatus;
var jumlahHari;
var jumlahtarif;

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

    ceknik = $("#ceknikWindow").kendoWindow({
        title: "Cari NIK",
        modal: true,
        visible: false,
        resizable: false,
        width: 700,
        height: 450
    }).data("kendoWindow");

    gridStatus = 'belum-ngapa-ngapain';
    
    //////// sampe sini
});

angular.module("__header", ["kendo.directives"])
    .controller("MyCtrl", function ($scope) {
        $scope.validate = function (event) {
            event.preventDefault();
            if ($scope.validator.validate()) {
                simpanHeader().done(function (data) {
                    if (data) {
                        alert("Sukses");
                            $("#grdDetail").data("kendoGrid").dataSource.read({
                                id: tiketId
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

function disableHeader() {
    $("._key").find('span').addClass('disabledbutton');
    $("._key").addClass('disabledbutton');
    $("._nonkey").find('span').addClass('disabledbutton');
    $("._nonkey").addClass('disabledbutton');
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

function simpanHeader() {
    var url = '/Input/simpanHeader';
    var mdl = ViewToModel();
    headerBaru = true;
    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            _model: JSON.stringify(mdl),
            _baru: headerBaru,
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function ViewToModel() {
    var tarifmasuk = parseInt($('#TarifMasuk').val());
    var tarifparkir = parseInt($('#TarifParkir').val());
    //var jumlahtarif = $('#JumlahTarif').data('kendoNumericTextBox').value(tarifmasuk + tarifparkir);
    var mdl = {
        TiketId: tiketId,
        AreaWisataId: $('#AreaWisataId').val(),
        TahunBuku: $('#TahunBuku').val(),
        JenisKendaraan: $('#JenisKendaraan').val(),      
        NomorPolisi: $('#NomorPolisi').val().toUpperCase(),
        TanggalMasukKendaraan: $('#TanggalMasukKendaraan').val(),
        TanggalKeluarKendaraan: $('#TanggalKeluarKendaraan').val(),
        TarifParkir: $('#TarifParkir').val(),
        JumlahDewasa: $('#JumlahDewasa').val(),
        JumlahAnak: $('#JumlahAnak').val(),
        NomorTelepon: $('#NomorTelepon').val(),
        TanggalMasuk: $('#TanggalMasuk').val(),
        TanggalKeluar: $('#TanggalKeluar').val(),
        TarifMasuk: $('#TarifMasuk').val(),
        JumlahTarif: jumlahtarif
    };
    return mdl;
}

/* Grid Buyer */

function grdOnChange(e) {

}

function grdOnEdit(e) {
    //var model = e.model;
    //this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    //if (model.isNew()) {
    //    model.TiketId = model.uid;
    //}
    //tiketId = model.TiketId;

    //gridStatus = 'sudah-diapa-apain';
}

function grdOnDataBinding(e) {
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
            disableHeader();
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
        disableHeader();
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

function filterdetailRead(e) {
    return {
        //id: berangkat,
        _menuId: menuId
    };
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

function btnPrintHeaderOnClick() {
    var viewer = $("#reportViewer").data("telerik_ReportViewer");
    viewer.reportSource({
        report: viewer.reportSource().report,
        parameters: { TiketId: tiketId }
    });
    viewer.refreshReport();
    prt.open().center();
}

function hapusSeluruh(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
        //openDelDetailWindow();
        var grid = $('#grdDetail').data('kendoGrid');
        var row = grid.tbody.find("tr:first");
        do {
            $('#grdDetail').data('kendoGrid').removeRow(row);
            row = grid.tbody.find("tr:first");
        }
        while (grid.tbody.contents().length > 0)

        gridStatus = 'sudah-diapa-apain';

    });
    $("#noDetail").click(function () {
        wndDetail.close();
    });
}

function btnReservationOnClick(e) {
    simpanHeader().done(function (results) {
        if (results) {
            alert("Sukses");
        }
    });

}

function JenisKendaraanOnChange(e) {
    var jenisKendaraan = $('#JenisKendaraan').val();
    if (jenisKendaraan == 'Bus') {
        $('#TarifParkir').data('kendoNumericTextBox').value(10000);
    }
    else if (jenisKendaraan == 'Mobil')
    {
        $('#TarifParkir').data('kendoNumericTextBox').value(7500);
    }
    else if (jenisKendaraan == 'Motor')
    {
        $('#TarifParkir').data('kendoNumericTextBox').value(2500);
    }
}

function JumlahDewasaOnChange(e) {
    var jumlahDewasa = parseInt($('#JumlahDewasa').val());
    $('#TarifMasuk').data('kendoNumericTextBox').value(15500 * jumlahDewasa);
    var tarifmasuk = parseInt($('#TarifMasuk').data('kendoNumericTextBox').value());
    var tarifparkir = parseInt($('#TarifParkir').data('kendoNumericTextBox').value());
    jumlahtarif = tarifparkir + tarifmasuk;
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