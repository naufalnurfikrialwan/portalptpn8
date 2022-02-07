var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var rowNumber = 0;
var menuId = '21BA273F-C5B0-407A-B1CE-ACDF9ED6B1C8';
var ruangRapatPTPNVIIIId, bulan, tahun;
var usrName = "@HttpContext.Current.User.Identity.Name";

function resetRowNumber(e) {
    rowNumber = 0;
        var rows = e.sender.tbody.children();
    for (var j = 0; j < rows.length; j++) {
        var row = $(rows[j]);
        var dataItem = e.sender.dataItem(row);

        if (dataItem.get("Verifikasi") == "Disetujui") {
	    row.addClass("sudahDisetujui");
        }
        else {
            row.addClass("belumDisetujui");
        }
    }
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
                if (input.attr("name") == "JumlahPeserta" && input.val() != "") {
                    cekNamaBagian(input.val()).done(function (data) {
                        if (data.length > 0) {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(ruangRapatPTPNVIIIId)) == "undefined" ? grid.dataSource.getByUid(ruangRapatPTPNVIIIId) : grid.dataSource.get(ruangRapatPTPNVIIIId);
                            dataItem.BagianId = data[0][0];
                            $('#BagianId').val(data[0][0]);
                            grid.refresh();
                        }
                    });
                } 
                else if (input.attr("name") == "NamaKegiatan" && input.val() != "") {
                    input.attr("data-chopvalidation-msg", "Ruangan Sudah Terisi!")
                    cekKetersediaanRuangan(input).done(function (r) {
                        if (r.length == 0) {
                            return true;
                        } else {
                            for (var i = 0; i < r.length; i++) {
                                if (r[i][0] != ruangRapatPTPNVIIIId) {
                                    $('#errMsg').text('Ruangan sudah terisi, harap cari ruangan atau waktu lain!');
                                    openErrWindow();
                                    var grid = $('#grdDetail').data('kendoGrid');
                                    var dataItem = typeof (grid.dataSource.get(ruangRapatPTPNVIIIId)) == "undefined" ? grid.dataSource.getByUid(ruangRapatPTPNVIIIId) : grid.dataSource.get(ruangRapatPTPNVIIIId);
                                    dataItem.TanggalMulai = '';
                                    dataItem.TanggalSelesai = '';
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
                tahun = $('#Tahun').val();
                bulan = $('#Bulan').data('kendoDropDownList').value();
                $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                $('#grdDetail').removeClass('disabledbutton');
                $("#grdDetail").data("kendoGrid").dataSource.read({
                    id: $('#RuangRapatPTPNVIII').val()
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

function cekKetersediaanRuangan(cekKamar) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(ruangRapatPTPNVIIIId)) == "undefined" ? grid.dataSource.getByUid(ruangRapatPTPNVIIIId) : grid.dataSource.get(ruangRapatPTPNVIIIId);
    var tanggalMasukCek = dataItem.TanggalMulai;
    var tanggalKeluarCek = dataItem.TanggalSelesai;
    var jenisRuangRapat = dataItem.JenisRuangRapat;
    var dateStringMasuk = new Date();
    dateStringMasuk = tanggalMasukCek.getFullYear() + "/" + (tanggalMasukCek.getMonth() + 1) + "/" + (tanggalMasukCek.getDate());
    var dateStringKeluar = new Date();
    dateStringKeluar = tanggalKeluarCek.getFullYear() + "/" + (tanggalKeluarCek.getMonth() + 1) + "/" + (tanggalKeluarCek.getDate());
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select JenisRuangRapat from [SPDK_KanpusEF].[dbo].[RuangRapat]" +
                " where JenisRuangRapat='" + jenisRuangRapat + "' and TanggalSelesai >='" + dateStringMasuk + "' and TanggalMulai <='" + dateStringKeluar+"'",
            _menuId: menuId
        },
        type: 'GET',
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
        model.RuangRapatPTPNVIIIId = model.uid;
    }
    ruangRapatPTPNVIIIId = model.RuangRapatPTPNVIIIId;
    gridStatus = 'sudah-diapa-apain';
    $(e.container).find("input[name='Catatan']").prop('disabled', true);
    $(e.container).find("input[name='Verifikasi']").prop('disabled', true);
}

function grdOnDataBinding(e) {
}

function filterdetailRead() {
    return {
        _menuId: menuId, // teh
        _filter: ["Year(TanggalMulai)", "" + tahun + "", "Month(TanggalMulai)", "" + bulan + ""]
    };
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
    kirimNotifikasiEmail();
}

function _sendData(data) {
    return $.ajax({
        url: "/Input/UpdateCreateDelete",
        data: data,
        type: "POST"
    })
}

function kirimNotifikasiEmail() {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            strClassView: "Ptpn8.RuangRapat.Models.View_RuangRapatPTPNVIII",
            scriptSQL: "exec RuangRapat_NotifikasiEmail '" + ruangRapatPTPNVIIIId + "'",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function ddlRuangRapatOnChange(e) {
    var jenisRuangRapat = $('#JenisRuangRapat').val();
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(ruangRapatPTPNVIIIId)) == "undefined" ? grid.dataSource.getByUid(ruangRapatPTPNVIIIId) : grid.dataSource.get(ruangRapatPTPNVIIIId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
}

function cekNamaBagian(namaBagian) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select BagianId, Nama from [ReferensiEF].[dbo].[Bagian]" +
                " where bagianId=(select posisilokasikerjaid from [Identity].[dbo].[AspNetUsers] where UserName ='" + usrName + "' )",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}