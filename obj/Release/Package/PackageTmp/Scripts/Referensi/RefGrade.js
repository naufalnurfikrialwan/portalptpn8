var gradeId;
var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var vessel;
var model;
var headerBaru, detailBaru;
var gridStatus;
var subBudidayaId,IdBudidaya;
var menuId = 'cda40f9d-0298-4404-8c0b-f4cc6f07e689';

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
			var h1 = parseInt($(".content-header").css("height"));
	    var h2 = parseInt($(".hdr").css("height"));
	    var h3 = parseInt($(".k-grid-toolbar").css("height"));
	    var h4 = parseInt($(".k-grid-header-wrap").css("height"));
	    var h5 = parseInt($(".__footer").css("height")) + 20;
	    $("#grdDetail").css("height", window.screen.height - (h1 + h2 + h3 + h4 + h5));
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
                //                    disableHeader();
                $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                $('#grdDetail').removeClass('disabledbutton');
                $("#grdDetail").data("kendoGrid").dataSource.read({
                    id: $('#SubBudidayaId').val()
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

function UploadSuccess(e) {
    var files = e.files;
    var filename = files[0].name;
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = grid.dataSource.get(gradeId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");

    dataItem.dirty = true;
    dataItem.FileFoto = filename;
    GetByteFromUpload(filename, gradeId).done(function (data) {
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
/* Grid Buyer */

function grdOnChange(e) {
}

function grdOnEdit(e) {
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.GradeId = model.uid;
        model.SubBudidayaId = subBudidayaId;
    }
    gradeId = model.GradeId;
    gridStatus = 'sudah-diapa-apain';
}

function grdOnDataBinding(e) {
}

function ddlRekeningOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetailBudidaya_' + gradeId).data('kendoGrid');
    var dataItem = grid.dataSource.get(buyerBudidayaId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    ddlRekening = ddlItem;
    data.set("Rekening", ddlItem);
    row.find("input[name=RekeningId]").val(ddlItem.RekeningId).change();
}

function ddlRekeningOnDataBound(e) {
}

function filterRead() {
    return {
        id: subBudidayaId,
        _menuId: menuId // teh
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
            spnew.push("EXEC InsertBuyerBudidaya '" + currentData[i].GradeId + "', '" + IdBudidaya + "', '00000000-0000-0000-0000-000000000000'");
        } else if (currentData[i].dirty) {
            updatedRecords.push(JSON.stringify(currentData[i]));
        }
    }

    //this records are deleted
    var deletedRecords = [];
    var spdeleted = [];
    for (var i = 0; i < grid.dataSource._destroyed.length; i++) {
        deletedRecords.push(JSON.stringify(grid.dataSource._destroyed[i]));
        spdeleted.push("EXEC DeleteBuyerBudidaya '" + currentData[i].GradeId + "', '" + IdBudidaya + "'");
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

function aucNamaSatuanOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(gradeId)) == "undefined" ? grid.dataSource.getByUid(gradeId) : grid.dataSource.get(gradeId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.SatuanId = ddlItem.SatuanId;
    $('#NamaSatuan').val(data.Nama);
}

function aucNamaSatuanOnDataBound(e) {

}

function filterSubBudidaya(e) {
    return {
        mainBudidayaId: IdBudidaya,
        value: $('#NamaSubBudidaya').val()
    };
}

function filterSatuan(e) {
    return {
        mainBudidayaId: IdBudidaya,
        value: $('#NamaSatuan').val()
    };
}

function NamaSubBudidayaOnSelect(e) {
    var subBudidaya = this.dataItem(e.item);
    $('#SubBudidayaId').val(subBudidaya.SubBudidayaId);
    subBudidayaId = subBudidaya.SubBudidayaId;
}

function filterBudidaya(e) {
    return {
        value: $('#NamaBudidaya').val()
    };
}

function NamaBudidayaOnSelect(e) {
    IdBudidaya = this.dataItem(e.item);
    $('#MainBudidayaId').val(IdBudidaya.MainBudidayaId);
    IdBudidaya = IdBudidaya.MainBudidayaId;
}
