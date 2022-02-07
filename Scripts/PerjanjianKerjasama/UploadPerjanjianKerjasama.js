var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var perjanjianKerjasama_DokumenId;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var rowNumber = 0;
var menuId = 'c251fbe1-5dba-43ec-861b-d8e38048b155';
var bagianId;

function resetRowNumber(e) {
    rowNumber = 0;
}

function renderNumber(data) {
    var no = ++rowNumber;
    data.NoBaris = no;
    return no;
}

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

    uploadFile = $("#uploadFile").kendoWindow({
        title: "Upload File",
        modal: true,
        visible: false,
        resizable: false,
        width: 500,
        height: 300
    }).data("kendoWindow");

    uploadFileSHP = $("#uploadFileSHP").kendoWindow({
        title: "Upload File SHP",
        modal: true,
        visible: false,
        resizable: false,
        width: 500,
        height: 300
    }).data("kendoWindow");

    pdf = $("#pdfWindow").kendoWindow({
        title: "PDF Viewer",
        modal: true,
        visible: false,
        resizable: false,
        width: 900,
        height: 550
    }).data("kendoWindow");

    gridStatus = 'belum-ngapa-ngapain';
    $("#grdDetail").kendoValidator({
        rules: { // custom rules
            chopvalidation: function (input) {
                if (input.attr("name") == "Perihal" && input.val() != "") {
                    cekNamaBagian(input.val()).done(function (data) {
                        if (data.length > 0) {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(perjanjianKerjasama_DokumenId)) == "undefined" ? grid.dataSource.getByUid(perjanjianKerjasama_DokumenId) : grid.dataSource.get(perjanjianKerjasama_DokumenId);
                            dataItem.BagianId = data[0][0];
                            $('#BagianId').val(data[0][0]);
                            var bagianId = data[0][0];
                            grid.refresh();
                        }
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
                $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                $('#grdDetail').removeClass('disabledbutton');
                $("#grdDetail").data("kendoGrid").dataSource.read({
                    id: $('#BagianId').val()
                });
            } else {
                $(".k-button-icontext").addClass("k-state-disabled").removeClass("k-grid-add"); // edited
                gridDestroy();
            }
        }
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

function gridDestroy() {
    $("#grdDetail").data("kendoGrid").dataSource.read([]);
}

/* Grid Buyer */

function grdOnChange(e) {
}

function grdOnEdit(e) {
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.PerjanjianKerjasama_DokumenId = model.uid;
        //model.BagianId = bagianId;
    }
    perjanjianKerjasama_DokumenId = model.PerjanjianKerjasama_DokumenId;
    gridStatus = 'sudah-diapa-apain';
}

function grdOnDataBinding(e) {
}

function filterdetailRead() {
    return { _menuId: menuId };
}

function filterdetailCreate(e) {
    return { _menuId: menuId };
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

}

function _sendData(data) {
    return $.ajax({
        url: "/Input/UpdateCreateDelete",
        data: data,
        type: "POST"
    })
}

function filterLokasiKerja() {
    return {
        //id: IdBudidaya,
        value: $('#aucLokasiKerja').val()
    };
}

function aucLokasiKerjaOnSelect(e) {
    kebunId = e.sender.dataItem().LokasiKerjaId;
}




function uplOnSuccess(e) {
    uploadFile.close();
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(perjanjianKerjasama_DokumenId)) == "undefined" ? grid.dataSource.getByUid(perjanjianKerjasama_DokumenId) : grid.dataSource.get(perjanjianKerjasama_DokumenId);
    dataItem.File = e.files[0].name;
    dataItem.dirty = true;
    grid.refresh();


}

function uplOnSuccessSHP(e) {
    uploadFileSHP.close();
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(perjanjianKerjasama_DokumenId)) == "undefined" ? grid.dataSource.getByUid(perjanjianKerjasama_DokumenId) : grid.dataSource.get(perjanjianKerjasama_DokumenId);
    dataItem.FileSHP = e.files[0].name;
    dataItem.dirty = true;
    grid.refresh();


}

//function onButtonClick(e) {
//    e.preventDefault();
//    var grid = this;
//    var row = $(e.currentTarget).closest("tr");

//    var data = grid.dataItem(row);
//    var file = data.File;

//    window.location.href = "/Content/Images/Document/SMPNVIII/" + file; //pass the desired url for the view
//}


function uploadData(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    perjanjianKerjasama_DokumenId = data.PerjanjianKerjasama_DokumenId;

    uploadFile.open().center();
}

function uploadDataSHP(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    perjanjianKerjasama_DokumenId = data.PerjanjianKerjasama_DokumenId;

    uploadFileSHP.open().center();
}

function aucLokasiKerjaOnSelect(e) {
    bagianId = e.sender.dataItem().LokasiKerjaId;
}

function filterLokasiKerja() {
    return {
        //id: IdBudidaya,
        value: $('#aucLokasiKerja').val()
    };
}

function onButtonClick(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");

    var data = grid.dataItem(row);
    var file = data.File;

    //window.location.href = "/Content/Images/Document/" + file; //pass the desired url for the view
    pdf.open().center();

    var options = {
        pdfOpenParams: {
            navpanes: 1,
            toolbar: 0,
            statusbar: 0,
            pagemode: 'none',
            pagemode: "none",
            page: 1,
            preventDefault: 1,
            zoom: "page-width",
            enableHandToolOnLoad: true
        } //,
        //forcePDFJS: true,
        //PDFJS_URL: "/PDF.js/web/viewer.html"
    }


    PDFObject.embed("/Content/Images/Document/PerjanjianKerjasama/" + file, "#testPDFObject", options)

}

function ddlJenisPerjanjianOnChange(e) {
    var jenisPerjanjian = $('#JenisPerjanjian').val();
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(perjanjianKerjasama_DokumenId)) == "undefined" ? grid.dataSource.getByUid(perjanjianKerjasama_DokumenId) : grid.dataSource.get(perjanjianKerjasama_DokumenId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
}

function ddlJangkaWaktuKerjasamaOnChange(e) {
    var jangkaWaktuKerjasama = $('#JangkaWaktuKerjasama').val();
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(perjanjianKerjasama_DokumenId)) == "undefined" ? grid.dataSource.getByUid(perjanjianKerjasama_DokumenId) : grid.dataSource.get(perjanjianKerjasama_DokumenId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
}

function ddlBentukKerjasamaOnChange(e) {
    var bentukKerjasama = $('#BentukKerjasama').val();
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(perjanjianKerjasama_DokumenId)) == "undefined" ? grid.dataSource.getByUid(perjanjianKerjasama_DokumenId) : grid.dataSource.get(perjanjianKerjasama_DokumenId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
}

function ddlStatusOtoritasOnChange(e) {
    var statusOtorisasi = $('#StatusOtorisasi').val();
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(perjanjianKerjasama_DokumenId)) == "undefined" ? grid.dataSource.getByUid(perjanjianKerjasama_DokumenId) : grid.dataSource.get(perjanjianKerjasama_DokumenId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
}

function cekNamaBagian(namaBagian) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select OrgId as BagianId from [ReferensiEF].[dbo].[Unit]" +
                " where orgId=(select posisilokasikerjaid from [Identity].[dbo].[AspNetUsers] where UserName ='" + usrName + "' )",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}