var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var hdr_PKBId, kebunId, barangId, namaBarang, norek, dtl_PKBId, rekeningId;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var rowNumber = 0;
var menuId = '7EE4265B-81B8-4DC8-8034-B1D9747CCED0';
var IHTBukan = 'Bukan';

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

$(document).ready(function () {
	disableHeader();

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

    $('#grdDetail').kendoValidator({
        rules: {
            validated: function (input) {
                if (input.attr("name") == "Nilai_Kebun" || input.attr("name") == "Nilai_Kandir") {
                    var namaField = input.attr("name");
                    var grd = $('#grdDetail').data('kendoGrid');
                    var dataItem = typeof (grd.dataSource.get(dtl_PKBId)) == "undefined" ? grd.dataSource.getByUid(dtl_PKBId) : grd.dataSource.get(dtl_PKBId);
                    dataItem[namaField] = parseFloat(input.val());
                    hitungSUM(namaField);
                    var row = $('#grdDetail').find('tbody>tr[data-uid=' + dataItem.uid + ']');
                }
            }

        }
    });

    namaKebunOnChange().done(function (data) {
        if (data) {
            //$('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
            disableHeader();
            $(".k-button-icontext").addClass("k-state-disabled").removeClass("k-grid-add"); // edited
            gridDestroy();
            //});
        }
        else {
            alert("Error ! Hubungi Bagian TI");
        }

    }).fail(function (x) {
        alert("Error! Hubungi Bagian TI");
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
                    if (data) {
                        hdr_PKBId = $('#HDR_PKBId').val();
                        disableHeader();
                        $('#btnDeleteHeader').removeClass('disabledbutton');
                        $('#btnPrintHeader').removeClass('disabledbutton');
                        $('#btnDeleteHeader').attr('disabled', false);
                        $('#btnPrintHeader').attr('disabled', false);
                        $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                        $('#tabstrip').removeClass('disabledbutton');

                        getDataFrom3().done(function (results) {
                            if (results == 0) {
                                InsertGridFrom().done(function (data) {
                                    $("#grdDetail").data("kendoGrid").dataSource.read({ id: $('#HDR_PKBId').val() });
                                }).fail(function (x) {
                                    alert('Error! Hubungi Bagian TI');
                                });
                            }
                            else {
                                var grid = $("#grdDetail").data("kendoGrid");
                                grid.dataSource.read({
                                    id: $('#HDR_PKBId').val()
                                }).done(function () {
                                    var currentData = grid.dataSource.data();
                                    for (var i = 0; i < currentData.length; i++) {
                                        currentData[i].dirty = true;
                                    }
                                });
                            }
                        });
                    }
                    else {
                        openErrWindow();
                        $('#tabstrip').addClass('disabledbutton');
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

function ModelToView(_NomorInputView) {
    $('#HDR_PKBId').val(_NomorInputView.HDR_PKBId);
    $('#TahunBuku').val(_NomorInputView.TahunBuku);
    $('#BulanRemise').val(_NomorInputView.BulanRemise);
    $('#TahunRemise').val(_NomorInputView.TahunRemise);
    $('#KebunId').val(_NomorInputView.KebunId);
    $('#NamaKebun').val(_NomorInputView.NamaKebun);
    $('#NomorInput').val(_NomorInputView.NomorInput);
    $('#NomorInputView').val(_NomorInputView.NomorInputView);
    $('#PeriodeRemise').val('TENGAH');
    $('#StatusFinal').val(_NomorInputView.StatusFinal);
    $('#UserName').val(_NomorInputView.UserName);
    $('#TanggalInput').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.TanggalInput.substr(6))));
}

function ViewToModel() {
    var mdl = {
        HDR_PKBId: $('#HDR_PKBId').val(),
        TahunBuku: $('#TahunBuku').val(),
        BulanRemise: $('#BulanRemise').val(),
        TahunRemise: $('#TahunRemise').val(),
        KebunId: $('#KebunId').val(),
        NamaKebun: $('#NamaKebun').val().toUpperCase(),
        NomorInput: $('#NomorInput').val(),
        NomorInputView: Array(5 - String($('#NomorInput').val()).length + 1).join('0') + $('#NomorInput').val(),
        PeriodeRemise: $('#PeriodeRemise').val(),
        StatusFinal: $('#StatusFinal').val(),
        UserName: $('#UserName').val(),
        TanggalInput: $('#TangglInput').val(),
    };
    return mdl;
}

function hapusHeader() {
    wnd.close();
    PeriksaConstraintHeader().done(function (data) {
        if (data == 0) {
            ConfirmedHapusHeader().done(function (data) {
                if (data) {
                    hdr_PermintaanBarangDariKebunId = $('#HDR_PKBId').val();
                    //$('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                    disableHeader();
                    $('#btnDeleteHeader').removeClass('disabledbutton');
                    $('#btnPrintHeader').removeClass('disabledbutton');
                    $('#btnDeleteHeader').attr('disabled', false);
                    $('#btnPrintHeader').attr('disabled', false);
                    $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                    $('#tabstrip').removeClass('disabledbutton');
                    $("#grdDetail").data("kendoGrid").dataSource.read({
                        id: $('#HDR_PKBId').val()
                    });
                    //});
                }
                else {
                    openErrWindow();
                    $('#tabstrip').addClass('disabledbutton');
                    gridDestroy();
                }
            }).fail(function (x) {
                alert('Error! Hubungi Bagian TI');
            });
        }
        else {
            openDelWindow();
        }
    }).fail(function (x) {
        alert('Error! Hubungi Bagian TI');
    });
}

function PeriksaConstraintHeader() {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_PKB]" +
                " where HDR_PKBId='" + $("#HDR_PKBId").val() + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function ConfirmedHapusHeader() {
    var url = '/Input/hapusHeader';
    var mdl = ViewToModel();
    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            _model: JSON.stringify(mdl),
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function btnDeleteHeaderOnClick(e) {
    wnd.open().center();
    $("#yes").click(function () {
        hapusHeader();
        wnd.close();
    });

    $("#no").click(function () {
        wnd.close();
    });
}

function NomorInputViewOnSelect(e) {
    if (gridStatus == 'sudah-nyoba-disimpan-tapi-model-masih-salah' ||
        gridStatus == 'sudah-diapa-apain') {
        openWindowLeaveGrid(e);
    }
    else {
        var dataItem = this.dataItem(e.item);
        var noInput = parseInt(dataItem.Value.substring(0, 5));
        $('#NomorInput').val(noInput);
        $('#BulanRemise').val(noInput);
        $('#TahunRemise').val($('#TahunBuku').val());
        $('#PeriodeRemise').val('TENGAH');

        getDataFrom4().done(function (results) {
            if (results.length) {
                _NomorInputView = results[0];
                _NomorInputView.NomorInputView = dataItem.Text;
                ModelToView(_NomorInputView);
                headerBaru = false;
            }
            else {
                _NomorInputView = ViewToModel();
                _NomorInputView.NomorInputView = dataItem.Text;
                headerBaru = true;
            }
            hdr_PKBId = _NomorInputView.HDR_PKBId;
            kebunId = _NomorInputView.KebunId;
            if (_NomorInputView.StatusFinal == "BELUM FINAL") {
                cekData(_NomorInputView.NomorInputView);
            }

        });

    }
}

function getDataFrom4() {
    // Hitung Jumlah Record di Detail
    var url = '/Input/GetDataFrom';
    return $.ajax({
        url: url,
        data: {
            strClassView: "Ptpn8.Remise.Models.View_HDRPKB",
            scriptSQL: "select A.*, B.Nama as NamaKebun, '' as NomorInputView from [SPDK_KanpusEF].[dbo].[HDR_PKB] A join [ReferensiEF].[dbo].[Kebun] B on A.KebunId=B.KebunId " +
                "where BulanRemise=" + $('#BulanRemise').val() + " and TahunRemise=" + $('#TahunRemise').val() + " and A.KebunId='" + $('#KebunId').val() + "' and PeriodeRemise='TENGAH' and NomorInput=" + $('#NomorInput').val(),
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function NomorInputViewOnDataBound(e) {
}

function cariUserYangLagiNgedit() {
    var url = '/Input/CariUserYangLagiNgedit';
    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });

}

function btnPrintHeaderOnClick() {
    var viewer = $("#reportViewer").data("telerik_ReportViewer");
    viewer.reportSource({
        report: viewer.reportSource().report,
        parameters: { HDR_PKBId: $('#HDR_PKBId').val(), KebunId: $('#KebunId').val(), Bulan: $('#BulanRemise').val(), Tahun: $('#TahunRemise').val() }
    });
    viewer.refreshReport();
    prt.open().center();
}

function disableHeader() {
    $("._key").find('span').addClass('disabledbutton');
    $("._key").addClass('disabledbutton');
    $("._nonkey").find('span').addClass('disabledbutton');
    $("._nonkey").addClass('disabledbutton');

    $('#btnSubmitHeader').addClass('disabledbutton');
    $('#btnDeleteHeader').addClass('disabledbutton');
    $('#btnPrintHeader').addClass('disabledbutton');
    $('#tabstrip').addClass('disabledbutton');
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

function cekData(nomorInputView) {
    if (headerBaru)   // Data Baru
    {
        enableHeader();
        $('#btnSubmitHeader').removeClass('disabledbutton');
        $('#btnDeleteHeader').removeClass('disabledbutton');
        $('#btnPrintHeader').removeClass('disabledbutton');
        $('#btnSubmitHeader').attr('disabled', false);
        $('#btnDeleteHeader').attr('disabled', true);
        $('#btnPrintHeader').attr('disabled', true);
        $('#tabstrip').addClass('disabledbutton');
        gridDestroy();
    }
    else // Data Lama
    {
        disableHeader();
        $("._nonkey").find('span').removeClass('disabledbutton');
        $("._nonkey").removeClass('disabledbutton');
        $('#btnSubmitHeader').removeClass('disabledbutton');
        $('#btnDeleteHeader').removeClass('disabledbutton');
        $('#btnPrintHeader').removeClass('disabledbutton');
        $('#btnSubmitHeader').attr('disabled', false);
        $('#btnDeleteHeader').attr('disabled', false);
        $('#btnPrintHeader').attr('disabled', false);
        $('#tabstrip').addClass('disabledbutton');
        gridDestroy();
    }
}

function filterNomorInput(e) {
    var mdl = JSON.stringify(ViewToModel());
    return { _model: mdl, _menuId: menuId };
}

function filterKebun() {
    return {
        key: "NamaKebun",
        value: $("#NamaKebun").val()
    };
}

function NamaKebunOnSelect(e) {
    var kebun = this.dataItem(e.item);
    if (typeof (kebun) != 'undefined') {
        $('#KebunId').val(kebun.KebunId);
        $('#NamaKebun').val(kebun.Nama);
        kebunId = kebun.KebunId;
    }

}
// Detail

function grdOnChange(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    var model = e.model;
}

function grdOnEdit(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.DTL_PKBId = model.uid;
        model.HDR_PKBId = hdr_PKBId;
    }

    dtl_PKBId = model.DTL_PKBId
    kebunId = $('#KebunId').val();
    gridStatus = 'sudah-diapa-apain';

    if (model.EditUraian == "readonly" || model.EditUraian.indexOf("sum") > -1)
        $(e.container).find("input[name='Uraian']").prop('disabled', true);
    else
        $(e.container).find("input[name='Uraian']").prop('disabled', false);

    if (model.EditPKB == "readonly" || model.EditPKB.indexOf("sum") > -1)
        $(e.container).find("input[name='Nilai_Kebun']").prop('disabled', true);
    else
        $(e.container).find("input[name='Nilai_Kebun']").prop('disabled', false);

    if (model.EditDisetujui == "readonly" || model.EditDisetujui.indexOf("sum") > -1)
        $(e.container).find("input[name='Nilai_Kandir']").prop('disabled', true);
    else
        $(e.container).find("input[name='Nilai_Kandir']").prop('disabled', false);
}

function filterGridDestroy(e) {
    return {
        _model: JSON.stringify(e),
        _menuId: menuId
    };
}

function TahunBukuOnChange(e) {
    if (gridStatus == 'sudah-nyoba-disimpan-tapi-model-masih-salah' ||
        gridStatus == 'sudah-diapa-apain') {
        openWindowLeaveGrid(e);
    }
    else {
        $('#msgInputView').text('');
        tahunBukuOnChange().done(function (data) {
            if (data) {
                //$('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                disableHeader();
                $(".k-button-icontext").addClass("k-state-disabled").removeClass("k-grid-add"); // edited
                gridDestroy();
                //});
            }
            else {
                alert("Error ! Hubungi Bagian TI");
            }

        }).fail(function (x) {
            alert("Error! Hubungi Bagian TI");
        });

    }
}

function tahunBukuOnChange() {
    var url = '/Input/hapusContext';
    return $.ajax({
        url: url,
        type: 'POST',
        datatype: 'json',
        data: {
            tahunBuku: $('#TahunBuku').val(),
            _menuId: menuId,
            mainBudidayaId: ''
        },

    });
}

function namaKebunOnChange() {
    var url = '/Input/hapusContextNomorInput';
    return $.ajax({
        url: url,
        type: 'POST',
        datatype: 'json',
        data: {
            _menuId: menuId
        },

    });

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


////// sampe sini
function hapusSeluruh(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
        //openDelDetailWindow();
        var grid = $('#grdDetail').data('kendoGrid');
        grid.dataSource.page(1);
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


function getDataFrom3() {
    // Hitung Jumlah Record di Detail
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_PKB]" +
                " where HDR_PKBId='" + $("#HDR_PKBId").val() + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function InsertGridFrom() {
    var grd = $('#grdDetail').data('kendoGrid');
    var url = '/Input/GetDataFrom';
    var kbnId = $('#KebunId').val();

    return $.ajax({
        url: url,
        data: {
            strClassView: "Ptpn8.Remise.Models.View_DTLPKB",
            scriptSQL: "EXEC PROC_ArusKasPKB '" + $('#HDR_PKBId').val() +"','" + $('#KebunId').val() + "'," + $('#BulanRemise').val() + "," + $('#TahunRemise').val() + ",'TENGAH'",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}


function hitungSUM(namaField) {
    var grd = $('#grdDetail').data('kendoGrid');
    var dataSource = grd.dataSource;
    var data = dataSource.data();
    var sort = dataSource.sort();
    var query = new kendo.data.Query(data);
    var sortedData = query.sort(sort).data;

    for (var i = 0; i < sortedData.length; i++) {
        var strEdit = sortedData[i]["EditPKB"];
        if (strEdit.indexOf("sum") > -1) {
            // model penulisasan rumus
            // sum(1,2,3,4) =>
            // sum(1..10)
            // sum(1,2..10)
            // sum(1,2,3,10..20,-10)

            var a = strEdit.substr(strEdit.indexOf('(') + 1);
            var b = a.substr(0, a.indexOf(')'));
            // 1,2,3,4
            // 1..10
            // 1,2..10
            // 1,2,3,10..20,-10

            var arrSum = b.split(',');
            var arrSumFinished = [];
            for (var j = 0; j < arrSum.length; j++) {
                var arrElement = arrSum[j];
                if (arrElement.indexOf('..') > -1) {
                    // 5..10
                    // 2..10
                    var elementAwal = parseInt(arrElement.substr(0, arrElement.indexOf('.')));
                    var elementAkhir = parseInt(arrElement.substr(arrElement.indexOf('.') + 2));
                    var n = elementAwal;
                    for (var k = 0; k < (elementAkhir - elementAwal + 1) ; k++) {
                        arrSumFinished.push(n);
                        n++;
                    }
                }
                else {
                    arrSumFinished.push(parseInt(arrElement));
                }
            }

            var hasilSum = 0;
            for (var j = 0; j < arrSumFinished.length; j++) {
                if (arrSumFinished[j] >= 0) {
                    var val = parseFloat(sortedData[arrSumFinished[j] - 1][namaField]);
                    hasilSum = hasilSum + val;
                } else {
                    var val = parseFloat(sortedData[-1 * arrSumFinished[j] - 1][namaField]);
                    hasilSum = hasilSum + (-1 * val);
                }
            }
            sortedData[i][namaField] = hasilSum;
        }
    }
    grd.refresh();
}

function grdOnBound(e) {
    var grd = $('#grdDetail').data('kendoGrid');

    var columns = grd.columns;
    var columnIndex = 0;
    var row, cell;

    var dataItems = grd.dataSource.view();
    for (var j = 0; j < dataItems.length; j++) {
        if (dataItems[j].get("EditUraian") == "readonly" || dataItems[j].get("EditUraian").indexOf("sum") > -1) {
            columnIndex = grd.wrapper.find(".k-grid-header [data-field=" + "Uraian" + "]").index();
            row = grd.tbody.find("[data-uid='" + dataItems[j].uid + "']");
            cell = row.children().eq(columnIndex);
            cell.addClass("rowStyle");
        }

        if (dataItems[j].get("EditPKB") == "readonly" || dataItems[j].get("EditPKB").indexOf("sum") > -1) {
            columnIndex = grd.wrapper.find(".k-grid-header [data-field=" + "Nilai_Kebun" + "]").index();
            row = grd.tbody.find("[data-uid='" + dataItems[j].uid + "']");
            cell = row.children().eq(columnIndex);
            cell.addClass("rowStyle");
        }

        if (dataItems[j].get("EditDisetujui") == "readonly" || dataItems[j].get("EditDisetujui").indexOf("sum") > -1) {
            columnIndex = grd.wrapper.find(".k-grid-header [data-field=" + "Nilai_Kandir" + "]").index();
            row = grd.tbody.find("[data-uid='" + dataItems[j].uid + "']");
            cell = row.children().eq(columnIndex);
            cell.addClass("rowStyle");
        }
    }
}

