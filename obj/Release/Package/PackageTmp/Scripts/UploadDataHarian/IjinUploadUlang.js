var wndLeaveGrid, wnd, wndDetail, prt, err, del, wndKirimEmail,wndHasilKirimEmail;
var dataFaktur;
var hdr_IjinUploadUlangId, mainBudidayaId, subBudidayaId, dtl_IjinUploadUlangId;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var rowNumber = 0;
var menuId = '6bb66d77-dfbb-49b0-b03e-8c3785c4d0c6';

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
    var h1 = parseInt($(".content-header").css("height"));
    var h2 = parseInt($(".hdr").css("height")) + parseInt($("._headerlainnya").css("height"));
    var h3 = parseInt($(".k-grid-toolbar").css("height"));
    var h4 = parseInt($(".k-grid-header-wrap").css("height"));
    var h5 = parseInt($(".__footer").css("height")) + 20;
    if ((window.screen.height - (h1 + h2 + h3 + h4 + h5)) >= 400)
        $("#grdDetail").css("height", window.screen.height - (h1 + h2 + h3 + h4 + h5));
    else
        $("#grdDetail").css("height", 500);
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

    wndKirimEmail = $("#wKirimEmail").kendoWindow({
        title: "Kirim Email",
        modal: true,
        visible: false,
        resizable: false,
        width: 300,
        height: 250
    }).data("kendoWindow");

    wndHasilKirimEmail = $("#wHasilKirimEmail").kendoWindow({
        title: "Hasil Kirim Email",
        modal: true,
        visible: false,
        resizable: false,
        width: 150,
        height: 120
    }).data("kendoWindow");

    gridStatus = 'belum-ngapa-ngapain';

    // copykan ke semua view 22/09/16 16:40
    $("#grdDetail").kendoValidator({
        rules: { // custom rules
            chopvalidation: function (input) {
                if (input.attr("name") == "NamaKebun" && input.val() != "") {
                    cekNamaKebun(input.val()).done(function (data) {
                        if (data.length > 0) {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_IjinUploadUlangId)) == "undefined" ? grid.dataSource.getByUid(dtl_IjinUploadUlangId) : grid.dataSource.get(dtl_IjinUploadUlangId);
                            var ubahData = false;
                            if (dataItem.KebunId != data[0][0]) {
                                dataItem.KebunId = data[0][0];
                                ubahData = true;
                            }
                            if (dataItem.Tanggal_Kadaluwarsa != data[0][2]) {
                                dataItem.Tanggal_Kadaluwarsa = data[0][2];
                                ubahData = true;
                            }
                            if (dataItem.KeyCodeUploadUlang == "") {
                                dataItem.KeyCodeUploadUlang = data[0][1];
                                ubahData = true;
                            }
                            if (ubahData) {
                                dataItem.dirty = true;
                                grid.refresh();
                            }
                        }
                    });
                } else {
                    return true;
                };
            }
        }
    });
    //////// sampe sini
});

$(window).on("beforeunload", function () {
    return "Please don't leave me!";
});

$(window).on("unload", function () {
    hapusPenggunaPortalYangAktif();
});


function cekNamaKebun(namaKebun) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select KebunId,newid(),getdate()+2 from [ReferensiEF].[dbo].[Kebun]" +
                " where lower(Nama)='" + namaKebun.toLowerCase() + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}
angular.module("__header", ["kendo.directives"])
    .controller("MyCtrl", function ($scope) {
        $scope.validate = function (event) {
            event.preventDefault();
            if ($scope.validator.validate()) {
                simpanHeader().done(function (data) {
                    if (data) {
                        hdr_IjinUploadUlangId = $('#HDR_IjinUploadUlangId').val();
                        $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                            disableHeader();
                            $('#btnDeleteHeader').removeClass('disabledbutton');
                            $('#btnPrintHeader').removeClass('disabledbutton');
                            $('#btnDeleteHeader').attr('disabled', false);
                            $('#btnPrintHeader').attr('disabled', false);
                            $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                            $('#grdDetail').removeClass('disabledbutton');
                            $("#grdDetail").data("kendoGrid").dataSource.read({
                                id: $('#HDR_IjinUploadUlangId').val()
                            });
                            GetDataFrom();
                        });
                    }
                    else {
                        openErrWindow();
                        $('#grdDetail').addClass('disabledbutton');
                        gridDestroy();
                    }

                }).fail(function (x) {
                    alert("Error! Hubungi Bagian TI");
                });  //edited
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


function openWindowKirimEmail(e) {

    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    wndKirimEmail.open().center();

    document.getElementById('txtNamaKebun').innerHTML = data.NamaKebun;
    document.getElementById('txtTglAwal').innerHTML = kendo.toString(data.Tanggal_Awal_Upload, "dd/MM/yyyy");
    document.getElementById('txtTglAkhir').innerHTML = kendo.toString(data.Tanggal_Akhir_Upload, "dd/MM/yyyy");
    document.getElementById('txtKeyCode').innerHTML = data.KeyCodeUploadUlang;
    document.getElementById('txtTglBatas').innerHTML = kendo.toString(data.Tanggal_Kadaluwarsa, "dd/MM/yyyy");

    var __body = "Key Code untuk mengulang upload data kebun "+data.NamaKebun+" mulai tanggal " + kendo.toString(data.Tanggal_Awal_Upload, "dd/MM/yyyy") +
        " sampai dengan tanggal " + kendo.toString(data.Tanggal_Akhir_Upload, "dd/MM/yyyy") +
        " adalah " + data.KeyCodeUploadUlang+". Batas waktu sd tanggal "+data.Tanggal_Kadaluwarsa+". Terima kasih.";

    $("#kirim").click(function () {
        kirimEmail(data.AlamatEmail, __body).done(function (data) {
            wndKirimEmail.close();
            if (data == 'true') {
                document.getElementById('txtHasilKirimEmail').innerHTML = 'Sukses';
            }
            else
            {
                document.getElementById('txtHasilKirimEmail').innerHTML = 'Gagal';
            }
            wndHasilKirimEmail.open().center();
            $("#okHasilKirimEmail").click(function () {
                wndHasilKirimEmail.close();
            });
        });
    });

    $("#batal").click(function () {
        wndKirimEmail.close();
    });
}

function kirimEmail(__to,__body)
{
    var url = '/Input/KirimEmail';
    return $.ajax({
        url: url,
        data: {
            _from: 'admportal.n8@gmail.com', 
            _to:__to, 
            _subject:'Key Code Untuk Ulang Upload Data', 
            _body:__body
        },
        type: 'POST',
        datatype: 'json'
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

function GetDataFrom() {
    // Hitung Jumlah Record di Detail
    getDataFrom().done(function (data) {
        if (data == 0) {
            //InsertGridFrom();
        }
        else {
        }
    }).fail(function (x) {
        alert('Error! Hubungi Bagian TI');
    });

}

function getDataFrom() {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_IjinUploadUlang]" +
                " where HDR_IjinUploadUlangId='" + $("#HDR_IjinUploadUlangId").val() + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function ModelToView(_NomorInputView) {
    $('#HDR_IjinUploadUlangId').val(_NomorInputView.HDR_IjinUploadUlangId);
    $('#TahunBuku').val(_NomorInputView.TahunBuku);
    $('#NomorInput').val(_NomorInputView.NomorInput);
    $('#NomorInputView').val(_NomorInputView.NomorInputView);
    $('#No_SuratIjin').val(_NomorInputView.No_SuratIjin);
    $('#Tanggal_Ijin').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.Tanggal_Ijin.substr(6))));
    $('#Keterangan').val(_NomorInputView.NamaPengangkut);
    $('#UserName').val(_NomorInputView.UserName);
}

function ViewToModel() {
    var mdl = {
        HDR_IjinUploadUlangId: $('#HDR_IjinUploadUlangId').val(),
        TahunBuku: $('#TahunBuku').val(),
        NomorInput: $('#NomorInput').val(),
        NomorInputView: Array(5 - String($('#NomorInput').val()).length + 1).join('0') + $('#NomorInput').val() + ' - ' + $('#No_SuratIjin').val().toUpperCase(),
        No_SuratIjin: $('#No_SuratIjin').val().toUpperCase(),
        Tanggal_Ijin: $('#Tanggal_Ijin').val(),
        Keterangan: $('#Keterangan').val(),
        UserName: $('#UserName').val()
    };
    return mdl;
}

function hapusHeader() {
    wnd.close();
    PeriksaConstraintHeader().done(function (data) {
        if (data == 0) {
            ConfirmedHapusHeader().done(function (data) {
                if (data) {
                    hdr_IjinUploadUlangId = $('#HDR_IjinUploadUlangId').val();
                    $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                        disableHeader();
                        $('#btnDeleteHeader').removeClass('disabledbutton');
                        $('#btnPrintHeader').removeClass('disabledbutton');
                        $('#btnDeleteHeader').attr('disabled', false);
                        $('#btnPrintHeader').attr('disabled', false);
                        $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                        $('#grdDetail').removeClass('disabledbutton');
                        $("#grdDetail").data("kendoGrid").dataSource.read({
                            id: $('#HDR_IjinUploadUlangId').val()
                        });
                    });
                }
                else {
                    openErrWindow();
                    $('#grdDetail').addClass('disabledbutton');
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_IjinUploadUlang]" +
                " where HDR_IjinUploadUlangId='" + $("#HDR_IjinUploadUlangId").val() + "'",
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
        if (typeof (e.item) != 'undefined')
            _NomorInputView = this.dataItem(e.item);
        else
            _NomorInputView = e.sender.dataItem();
        cekData(_NomorInputView.NomorInputView);
        ModelToView(_NomorInputView);
        hdr_IjinUploadUlangId = _NomorInputView.HDR_IjinUploadUlangId;
        $('#No_IjinUploadUlang').focus();
    }
}

function NomorInputViewOnDataBound(e) {
    var items = this.dataItems();
    var adaDataBaru = false;
    for (var i = 0; i < items.length; i++) {
        var str = items[i].NomorInputView;
        if (str.toLowerCase().indexOf('data baru') > 0) {
            adaDataBaru = true;
            break;
        }
    }
    if (!adaDataBaru) {
        cariUserYangLagiNgedit().done(function (data) {
            $('#msgInputView').text('Tidak dapat membuat data baru, dikarenakan sedang diedit oleh ' + data);
        }).fail(function () {
            $('#msgInputView').text('Tidak dapat membuat data baru, dikarenakan sedang diedit oleh user lain.');
        });
    }
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

function btnPrintHeaderOnClick() {
    var viewer = $("#reportViewer").data("telerik_ReportViewer");
    viewer.reportSource({
        report: viewer.reportSource().report,
        parameters: { HDR_IjinUploadUlangId: $('#HDR_IjinUploadUlangId').val() }
    });
    viewer.refreshReport();
    prt.open().center();
}

function disableHeader() {
    $("._key").find('IjinUploadUlangn').addClass('disabledbutton');
    $("._key").addClass('disabledbutton');
    $("._nonkey").find('IjinUploadUlangn').addClass('disabledbutton');
    $("._nonkey").addClass('disabledbutton');

    $('#btnSubmitHeader').addClass('disabledbutton');
    $('#btnDeleteHeader').addClass('disabledbutton');
    $('#btnPrintHeader').addClass('disabledbutton');
    $('#grdDetail').addClass('disabledbutton');
}

function enableHeader() {
    $("._key").find('IjinUploadUlangn').removeClass('disabledbutton');
    $("._key").removeClass('disabledbutton');
    $("._nonkey").find('IjinUploadUlangn').removeClass('disabledbutton');
    $("._nonkey").removeClass('disabledbutton');
    $('#btnSubmitHeader').removeClass('disabledbutton');
    $('#btnDeleteHeader').removeClass('disabledbutton');
    $('#btnPrintHeader').removeClass('disabledbutton');
}

function cekData(nomorInputView) {
    if (nomorInputView.indexOf("Data Baru") > -1)   // Data Baru
    {
        headerBaru = true;
        enableHeader();
        $('#btnSubmitHeader').removeClass('disabledbutton');
        $('#btnDeleteHeader').removeClass('disabledbutton');
        $('#btnPrintHeader').removeClass('disabledbutton');
        $('#btnSubmitHeader').attr('disabled', false);
        $('#btnDeleteHeader').attr('disabled', true);
        $('#btnPrintHeader').attr('disabled', true);
        $('#grdDetail').addClass('disabledbutton');
        gridDestroy();
    }
    else // Data Lama
    {
        headerBaru = false;
        disableHeader();
        $("._nonkey").find('IjinUploadUlangn').removeClass('disabledbutton');
        $("._nonkey").removeClass('disabledbutton');
        $('#btnSubmitHeader').removeClass('disabledbutton');
        $('#btnDeleteHeader').removeClass('disabledbutton');
        $('#btnPrintHeader').removeClass('disabledbutton');
        $('#btnSubmitHeader').attr('disabled', false);
        $('#btnDeleteHeader').attr('disabled', false);
        $('#btnPrintHeader').attr('disabled', false);
        $('#grdDetail').addClass('disabledbutton');
        gridDestroy();
    }
}

function filterNomorInput(e) {
    var mdl = JSON.stringify(ViewToModel());
    return {
        _model: mdl,
        _menuId: menuId
    };
}

// Detail

function grdOnChange(e) {
}

function grdOnEdit(e) {
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.DTL_IjinUploadUlangId = model.uid;
        model.HDR_IjinUploadUlangId = hdr_IjinUploadUlangId;
    }
    dtl_IjinUploadUlangId = model.DTL_IjinUploadUlangId;
    gridStatus = 'sudah-diapa-apain';
    $(e.container).find("input[name='KeyCodeUploadUlang']").prop('disabled', true);
    $(e.container).find("input[name='Tanggal_Kadaluwarsa']").prop('disabled', true);
}
function grdOnDataBinding(e) {

}

function aucNamaKebunOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_IjinUploadUlangId)) == "undefined" ? grid.dataSource.getByUid(dtl_IjinUploadUlangId) : grid.dataSource.get(dtl_IjinUploadUlangId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.KebunId = ddlItem.KebunId;
    CariEmail(data.KebunId).done(function (dta) {
        if(dta.length>0)
        {
            data.AlamatEmail = dta[0][0];
            data.dirty = true;
            grid.refresh();
        }
    });
}

function CariEmail(kebunId) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select top 1 username from [Identity].[dbo].[AspNetUsers]" +
                " where PosisiLokasiKerjaId='" + kebunId + "' and jabatan like '%tabin%'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function aucNamaKebunOnDataBound(e) {

}

function filterKebun(e) {
    return {
        value: $("#NamaKebun").val()
    };
}


function filterGridUpdate(e) {
    return {
        _model: JSON.stringify(e),
        _baru: detailBaru,
        _menuId: menuId
    };
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
        tahunBukuOnChange(e).done(function (data) {
            if (data) {
                $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                    disableHeader();
                    $(".k-button-icontext").addClass("k-state-disabled").removeClass("k-grid-add"); // edited
                    gridDestroy();
                });
            }
            else {
                alert('Error! Hubungi Bagian TI');
            }
        }).fail(function (x) {
            alert('Error! Hubungi Bagian TI');
        });
    }
}

function tahunBukuOnChange(e) {
    var url = '/Input/hapusContext';
    return $.ajax({
        url: url,
        type: 'POST',
        datatype: 'json',
        data: {
            tahunBuku: $('#TahunBuku').val(),
            _menuId: menuId
        }
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

function hapusSeluruh(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
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

function filterdetailRead(e) {
    return { _menuId: menuId };
}

function filterdetailCreate(e) {
    return { _menuId: menuId };
}

