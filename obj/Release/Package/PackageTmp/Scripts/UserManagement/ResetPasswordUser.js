var userId;
var lokasiKerjaId;
var roleId;
var uid;
var menuId = 'DF969080-9FEC-4F81-83DF-10B3475CA7A3';
var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var gridStatus;

$('.k-window.titlebar').css('style', 'display: none');
$(".k-button-icontext").addClass("k-state-disabled").removeClass("k-grid-add");
disableHeader();
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
                $('#grdDetail').data('kendoGrid').dataSource.read()
                    .done(function () {

                }
                    )
                    .fail(function () {

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

function openWindow(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
        $('#grduserRole_'+userId).data('kendoGrid').removeRow(row);
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
        //sendData();
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

function UploadSuccess(e) {
    var files = e.files;
    var filename = files[0].name;
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = grid.dataSource.get(brokerId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");

    dataItem.dirty = true;
    dataItem.FileFoto = filename;
    GetByteFromUpload(filename, brokerId).done(function (data) {
        dataItem.Foto = data.toString();
        row.find("input[name=Foto]").val(data).change();
    }).fail(function () {
        dataItem.Foto = null;
    });
}

function GetByteFromUpload(filename, id) {
    var url = '/Input/GetByteFromUpload';
    return $.ajax({
        url: url,
        type: 'POST',
        datatype: 'json',
        data: {
            _filename: filename,
            _id: id,
            _menuId: menuId
        }
    });
}

/* Grid Organisasi */

function grdOnChange(e) {
}

function grdOnEdit(e) {
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.Id = model.uid;
    }
    Id = model.Id;
    gridStatus = 'sudah-diapa-apain';
}

function grdOnDataBinding(e) {

}

function filterReadMaster(e) {
    return {
        _menuId: menuId,
        //_classDetailView: 'Ptpn8.UserManagement.Models.ApplicationUser',
        _filter: ["Email", "" + $('#Email').val() + ""]
    };
}

function filterUpdateMaster(e) {

    return {
        _model: JSON.stringify(""),
        _baru: false, 
        _menuId: menuId, 
        _classDetailView: 'Ptpn8.UserManagement.Models.ApplicationUser' 
    };
}


function filterLokasiKerja() {
    return {
        //id: IdBudidaya,
        value: $('#aucLokasiKerja').val()
    };
}

function filterRoleName() {
    return {
        value: $('#RoleName').val()
    };
}


function aucLokasiKerjaOnSelect(e)
{
    lokasiKerjaId = e.sender.dataItem().LokasiKerjaId;
}

function aucRoleNameOnSelect(e)
{
    roleId = e.dataItem.Id;
    var grid = $('#grduserRole_' + userId).data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(uid)) == "undefined" ? grid.dataSource.getByUid(uid) : grid.dataSource.get(uid);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.RoleId = roleId;
    data.UserId = userId;
    data.Discriminator = 'ApplicationUserRole';
}

function aucRoleNameOnDataBound(e) {

}

function sendDataHDR() {

    var grid = $("#grdDetail").data("kendoGrid"),
        parameterMap = grid.dataSource.transport.parameterMap;

    //get the new and the updated records
    var currentData = grid.dataSource.data();

    var updatedRecords = [];
    var newRecords = [];
    var spupdated = [];
    var spnew = [];

    for (var i = 0; i < currentData.length; i++) {
            updatedRecords.push(JSON.stringify(currentData[i]));
            spupdated.push("Update [Identity].[dbo].[AspNetUsers] set PasswordHash='APK9qyT2+GBpgAzvUuquhPFUn5gFT7nWB9PpRpyyflITeXfK25I2PIKJj2tmwITYtw==' Where Email='" + $('#Email').val()+"'");
        
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
        parameterMap({ classdv: 'Ptpn8.UserManagement.Models.ApplicationUser' })
        );

    _sendData(data).done(function (msg) {
        if (msg == "Success") {
            grid.dataSource.read();
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

function grdOnDetailExpand(e) {
    userId = this.dataItem(e.masterRow).Id;
}

function grduserRoleOnEdit(e) {
    var model = e.model;
    if (model.isNew()) {
        model.Idd = model.uid;
        //model.UserId = userId;
    }
    uid = model.uid;
}

function grduserRoleOnDataBound(e) {
    var x = this;
}

function disableHeader() {
    $("._key").find('span').addClass('disabledbutton');
    $("._key").addClass('disabledbutton');
    $("._nonkey").find('span').addClass('disabledbutton');
    $("._nonkey").addClass('disabledbutton');

    $('#btnSubmitHeader').addClass('disabledbutton');
    $('#btnDeleteHeader').addClass('disabledbutton');
    $('#btnPrintHeader').addClass('disabledbutton');
    $('#_detail').addClass('disabledbutton');
}
function enableHeader() {
    $("._key").find('span').removeClass('disabledbutton');
    $("._key").removeClass('disabledbutton');
    $("._nonkey").find('span').removeClass('disabledbutton');
    $("._nonkey").removeClass('disabledbutton');
    $('#btnSubmitHeader').removeClass('disabledbutton');
    $('#btnDeleteHeader').removeClass('disabledbutton');
    $('#btnPrintHeader').removeClass('disabledbutton');
}