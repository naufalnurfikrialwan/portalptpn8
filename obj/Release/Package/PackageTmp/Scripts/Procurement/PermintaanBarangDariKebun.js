var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var buyer;
var hdr_PermintaanBarangDariKebunId, kebunId, barangId,namaBarang,norek, dtl_PermintaanBarangDariKebunId, rekeningId;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var rowNumber = 0;
var menuId = 'F1DD518B-6BB9-42E6-867B-E3D10AEF05C8';

function resetRowNumber(e) {
    rowNumber = 0;
    var jumlahharga = 0;
    var grid = $('#grdDetail').data('kendoGrid');
    var datasource = grid.dataSource.data();
    for (var i = 0; i < datasource.length; i++) {
        var model = datasource[i];
        model.JumlahHarga = hitungJumlah(model.DTL_PermintaanBarangDariKebunId);
        $('#jml' + model.DTL_PermintaanBarangDariKebunId).text(kendo.toString(model.JumlahHarga, 'n2'));
        jumlahharga = jumlahharga + model.Jumlahharga;
    }
    $('#Jumlahharga').text(kendo.toString(jumlahharga, 'n2'));
}

//function renderNumber(data) {
//    var no = ++rowNumber;
//    data.NoBaris = no;
//    return no;
//}

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
	    var h2 = parseInt($(".hdr").css("height"));
	    var h3 = parseInt($(".k-grid-toolbar").css("height"));
	    var h4 = parseInt($(".k-grid-header-wrap").css("height"));
	    var h5 = parseInt($(".__footer").css("height")) + 20;
	    $("#grdDetail").css("height", window.screen.height - (h1 + h2 + h3 + h4 + h5));
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


    // copykan ke semua view 22/09/16 16:40
    $("#grdDetail").kendoValidator({
        rules: { // custom rules
            chopvalidation: function (input) {
                if (input.attr("name") == "NamaBarang" && input.val() != "") {
                    cekNamaBarang(input.val()).done(function (data) {
                        if (data.length > 0) {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_PermintaanBarangDariKebunId)) == "undefined" ? grid.dataSource.getByUid(dtl_PermintaanBarangDariKebunId) : grid.dataSource.get(dtl_PermintaanBarangDariKebunId);
                            dataItem.BarangId = data[0][0];
                            dataItem.Norek = data[0][1];
                            $('#Norek').val(data[0][1]);
                        }
                    });
                }else if(input.attr("name") == "Norek" && input.val() != "") {
                    cekNorek(input.val()).done(function(data){
                        if(data.length>0)
                        {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_PermintaanBarangDariKebunId)) == "undefined" ? grid.dataSource.getByUid(dtl_PermintaanBarangDariKebunId) : grid.dataSource.get(dtl_PermintaanBarangDariKebunId);
                            dataItem.RekeningId = data[0][0];
                        }
                    });
                } else if(input.attr("name") == "Satuan" && input.val() != "") {
                    cekNamaSatuan(input.val()).done(function(data){
                        if(data.length>0)
                        {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_PermintaanBarangDariKebunId)) == "undefined" ? grid.dataSource.getByUid(dtl_PermintaanBarangDariKebunId) : grid.dataSource.get(dtl_PermintaanBarangDariKebunId);
                            dataItem.SatuanId = data[0][0];
                        }
                    });
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


function cekNorek(norek) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select RekeningId, Norek, NamaRekening from [SPDK_KanpusEF].[dbo].[Norek]" +
                " where lower(Nama)='" + norek.toLowerCase() + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function cekNamaBarang(namaBarang) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "SELECT a.BarangId ,a.Norek ,a.KodeBarang,a.NamaBarang,a.KodeSatuan,a.NamaSatuan,a.InitSatuan,a.RekeningId,a.SatuanId " +
                        "FROM [ReferensiEF].[dbo].[Barang] a " +
                        " where lower(NamaBarang)='" + namaBarang.toLowerCase() + "'",
                        
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function cekNamaSatuan(satuan) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select SatuanId from [ReferensiEF].[dbo].[Satuan]" +
                " where lower(Nama)='" + satuan.toLowerCase() + "'",
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
                        hdr_PermintaanBarangDariKebunId = $('#HDR_PermintaanBarangDariKebunId').val();
                        $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                            disableHeader();
                            $('#btnDeleteHeader').removeClass('disabledbutton');
                            $('#btnPrintHeader').removeClass('disabledbutton');
                            $('#btnDeleteHeader').attr('disabled', false);
                            $('#btnPrintHeader').attr('disabled', false);
                            $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                            $('#grdDetail').removeClass('disabledbutton');
                            $("#grdDetail").data("kendoGrid").dataSource.read({
                                id: $('#HDR_PermintaanBarangDariKebunId').val()
                            });
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
    $('#HDR_PermintaanBarangDariKebunId').val(_NomorInputView.HDR_PermintaanBarangDariKebunId);
    $('#TahunBuku').val(_NomorInputView.TahunBuku);
    $('#KebunId').val(_NomorInputView.KebunId);
    $('#NamaKebun').val(_NomorInputView.NamaKebun);
    $('#NomorInput').val(_NomorInputView.NomorInput);
    $('#NomorInputView').val(_NomorInputView.NomorInputView);
    $('#NomorPermintaan').val(_NomorInputView.NomorPermintaan);
    $('#TanggalPermintaan').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.TanggalPermintaan.substr(6))));
    $('#UserName').val(_NomorInputView.UserName);
    $('#Pejabat').val(_NomorInputView.Pejabat);
    $('#No_SuratTugas').val(_NomorInputView.No_SuratTugas);
    $('#VerKaur').val(_NomorInputView.VerKaur);
    $('#VerKabag').val(_NomorInputView.VerKabag);
    var tglVerKaur = new Date(parseInt(_NomorInputView.TglVerKaur.substr(6)));
    var tglVerKabag = new Date(parseInt(_NomorInputView.TglVerKabag.substr(6)));
    $('#TglVerKaur').val(tglVerKaur.getDate() + "/" + (tglVerKaur.getMonth() + 1) + "/" + tglVerKaur.getFullYear());
    $('#TglVerKabag').val(tglVerKabag.getDate() + "/" + (tglVerKabag.getMonth() + 1) + "/" + tglVerKabag.getFullYear());
    $('#UserNameKaur').val(_NomorInputView.UserNameKaur);
    $('#UserNameKabag').val(_NomorInputView.UserNameKabag);
    $('#Bidang').val(_NomorInputView.Bidang);
    $('#Uraian').val(_NomorInputView.Uraian);
}

function ViewToModel() {
    var mdl = {
        HDR_PermintaanBarangDariKebunId: $('#HDR_PermintaanBarangDariKebunId').val(),
        TahunBuku: $('#TahunBuku').val(),
        KebunId: $('#KebunId').val(),
        NamaKebun: $('#NamaKebun').val().toUpperCase(),
        NomorInput: $('#NomorInput').val(),
        NomorInputView: Array(5 - String($('#NomorInput').val()).length + 1).join('0') + $('#NomorInput').val() + ' - ' + $('#NomorPermintaan').val().toUpperCase(),
        NomorPermintaan: $('#NomorPermintaan').val().toUpperCase(),
        TanggalPermintaan: $('#TanggalPermintaan').val(),
        UserName: $('#UserName').val(),
        Pejabat: $('#Pejabat').val().toUpperCase(),
        No_SuratTugas: $('#No_SuratTugas').val(),
        VerKaur: $('#VerKaur').val(),
        VerKabag: $('#VerKabag').val(),
        TglVerKaur: $('#TglVerKaur').val(),
        TglVerKabag: $('#TglVerKabag').val(),
        UserNameKaur: $('#UserNameKaur').val(),
        UserNameKabag: $('#UserNameKabag').val(),
        Bidang: $('#Bidang').val(),
        Uraian:$('#Uraian').val(),
    };
    return mdl;
}

function hapusHeader() {
    wnd.close();
    PeriksaConstraintHeader().done(function (data) {
        if (data == 0) {
            PeriksaConstraintHeaderAnak().done(function (data) {
                if (data == 0) {
                    ConfirmedHapusHeader().done(function (data) {
                        if (data) {
                            hdr_PermintaanBarangDariKebunId = $('#HDR_PermintaaanBarangDariKebunId').val();
                            $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                                disableHeader();
                                $('#btnDeleteHeader').removeClass('disabledbutton');
                                $('#btnPrintHeader').removeClass('disabledbutton');
                                $('#btnDeleteHeader').attr('disabled', false);
                                $('#btnPrintHeader').attr('disabled', false);
                                $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                                $('#grdDetail').removeClass('disabledbutton');
                                $("#grdDetail").data("kendoGrid").dataSource.read({
                                    id: $('#HDR_PermintaanBarangDariKebunId').val()
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_PermintaanBarangDariKebun]" +
                " where HDR_PermintaanBarangDariKebunId='" + $("#HDR_PermintaanBarangDariKebunId").val() + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function PeriksaConstraintHeaderAnak() {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[HDR_TerimaPermintaanBarangDariKebun]" +
                " where patindex('%" + $("#HDR_TerimaPermintaanBarangDariKebunId").val() + "%',List_NamaKebun)>0",
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
        ModelToView(_NomorInputView);
        hdr_PermintaanBarangDariKebunId = _NomorInputView.HDR_PermintaanBarangDariKebunId;
        kebunId = _NomorInputView.KebunId;
        cekData(_NomorInputView.NomorInputView);
        $('#No_PO').focus();
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

function btnPrintHeaderOnClick() {
    var viewer = $("#reportViewer").data("telerik_ReportViewer");
    viewer.reportSource({
        report: viewer.reportSource().report,
        parameters: { HDR_PermintaanBarangDariKebunId: $('#HDR_PermintaanBarangDariKebunId').val() }
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
    $('#grdDetail').addClass('disabledbutton');
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
        $("._nonkey").find('span').removeClass('disabledbutton');
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
        $('#KebunId').val(kebun.KebunId);
        $('#NamaKebun').val(kebun.Nama);
        kebunId = kebun.KebunId;

}
// Detail

function grdOnChange(e) {
}

function grdOnEdit(e) {
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.DTL_PermintaanBarangDariKebunId = model.uid;
        model.HDR_PermintaanBarangDariKebunId = hdr_PermintaanBarangDariKebunId;
    }
    dtl_PermintaanBarangDariKebunId = model.DTL_PermintaanBarangDariKebunId;
    kebunId = $('#KebunId').val();
    gridStatus = 'sudah-diapa-apain';
    model.Jumlahharga = hitungJumlah(dtl_PermintaanBarangDariKebunId);
    $('#jml' + model.dtl_PermintaanBarangDariKebunId).text(kendo.toString(model.Jumlahharga, 'n2'));
    //$('#jmlHarga').text(kendo.toString(rekapJumlahHarga(), 'n2'));
}

function hitungJumlah(id) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(id)) == "undefined" ? grid.dataSource.getByUid(id) : grid.dataSource.get(id);
    var newValue = 0;
        newValue = dataItem.Qty_Kebutuhan * dataItem.HargaSatuan;
    return newValue;
}

function filterkebun(e) {
    return {
        kebunId: $('#KebunId').val(),
        value: $("#NamaKebun").val()
    };
}

//function rekapJumlahHarga() {
//    var grid = $('#grdDetail').data('kendoGrid');
//    var newValue = 0;
//    for (var i = 0; i < grid.dataItems().length; i++) {
//        newValue += grid.dataItems()[i].Jumlahharga;
//    }
//    return newValue;
//}


function filterNamaBarang(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_PermintaanBarangDariKebunId)) == "undefined" ? grid.dataSource.getByUid(dtl_PermintaanBarangDariKebunId) : grid.dataSource.get(dtl_PermintaanBarangDariKebunId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    return {
        value: $("#NamaBarang").val()
       
    };
}

function filterRekening(e) {
    return {
        rekeningId: $('#RekeningId').val(),
        value: $("#Norek").val()
    };
}

function filterNamaRekening(e) {
    return {
        rekeningId: $('#RekeningId').val(),
        value: $("#NamaRekening").val()
    };
}

function aucNamaBarangOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_PermintaanBarangDariKebunId)) == "undefined" ? grid.dataSource.getByUid(dtl_PermintaanBarangDariKebunId) : grid.dataSource.get(dtl_PermintaanBarangDariKebunId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.BarangId = ddlItem.BarangId;
}

function aucNamaBarangOnDataBound(e) {

}

function aucNorekOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_PermintaanBarangDariKebunId)) == "undefined" ? grid.dataSource.getByUid(dtl_PermintaanBarangDariKebunId) : grid.dataSource.get(dtl_PermintaanBarangDariKebunId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.RekeningId = ddlItem.RekeningId;
    rekeningId = ddlItem.RekeningId;
    if (ddlItem.Norek != "") {
        data.NamaRekening = ddlItem.NamaRekening; 
    }
}

function aucNorekOnDataBound(e) {

}

function aucNamaRekeningOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_PermintaanBarangDariKebunId)) == "undefined" ? grid.dataSource.getByUid(dtl_PermintaanBarangDariKebunId) : grid.dataSource.get(dtl_PermintaanBarangDariKebunId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.RekeningId = ddlItem.RekeningId;
    rekeningId = ddlItem.RekeningId;
}

function aucNamaRekeningOnDataBound(e) {

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

function filterSatuan(e) {
    return {
        value: $("#Satuan").val()
    };
}

function aucNamaSatuanOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_PermintaanBarangDariKebunId)) == "undefined" ? grid.dataSource.getByUid(dtl_PermintaanBarangDariKebunId) : grid.dataSource.get(dtl_PermintaanBarangDariKebunId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.SatuanId = ddlItem.SatuanId;
}

function aucNamaSatuanOnDataBound(e) {

}

function filterGridDestroy(e) {
    return {
        _model: JSON.stringify(e),
        _menuId: menuId
    };
}

function ddlBidangOnChange(e) {
        var bidang = $('#Bidang').val();
        var grid = $('#grdDetail').data('kendoGrid');
        var dataItem = typeof (grid.dataSource.get(dtl_PermintaanBarangDariKebunId)) == "undefined" ? grid.dataSource.getByUid(dtl_PermintaanBarangDariKebunId) : grid.dataSource.get(dtl_PermintaanBarangDariKebunId);
        var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
        var data = grid.dataItem(row);
}

function ddlBulanOnChange(e) {
    var bulan = $('#Bulan').val();
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_PermintaanBarangDariKebunId)) == "undefined" ? grid.dataSource.getByUid(dtl_PermintaanBarangDariKebunId) : grid.dataSource.get(dtl_PermintaanBarangDariKebunId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
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
                alert("Error ! Hubungi Bagian TI");
            }

        }).fail(function (x) {
            alert("Error! Hubungi Bagian TI");
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

// copykan ke semua view
//function cekNoChop(input) {
//    var grid = $('#grdDetail').data('kendoGrid');
//    var dataItem = typeof (grid.dataSource.get(dtl_PermintaanBarangDariKebunId)) == "undefined" ? grid.dataSource.getByUid(dtl_PermintaanBarangDariKebunId) : grid.dataSource.get(dtl_PermintaanBarangDariKebunId);
//    var items = grid.dataItems();
//    var arr = [, ];
//    for (var i = 0; i < items.length; i++) {
//        if (items[i].Chop == input.val() && items[i].DTL_PermintaanBarangDariKebunId != dtl_PermintaanBarangDariKebunId
//            && items[i].Chop.toLowerCase() != "na" && items[i].MerkId == dataItem.MerkId) {
//            arr.push([items[i].DTL_PermintaanBarangDariKebunId, 1]);
//            return $.ajax(arr);
//        }
//    }
//    return $.ajax({
//        url: '/Input/ExecSQL',
//        data: {
//            scriptSQL:
//                " select DTL_ShippingInstructionId as Id, count(*) as Cnt from [SPDK_KanpusEF].[dbo].[DTL_ShippingInstruction]" +
//                " where MerkId='" + dataItem.MerkId + "'" +
//                " and SubBudidayaId='" + dataItem.SubBudidayaId + "'" +
//                " and Chop='" + input.val() + "'  and lower(Chop)!='na' group by DTL_ShippingInstructionId UNION" +
//                " select DTL_PermintaanBarangDariKebunId as Id, count(*) as Cnt from [SPDK_KanpusEF].[dbo].[DTL_PO]" +
//                " where MerkId='" + dataItem.MerkId + "'" +
//                " and SubBudidayaId='" + dataItem.SubBudidayaId + "'" +
//                " and Chop='" + input.val() + "' and lower(Chop)!='na' group by DTL_PermintaanBarangDariKebunId",
//            _menuId: menuId
//        },
//        type: 'GET',
//        datatype: 'json'
//    });
//}

//function ambilStandarGrade() {
//    var grid = $('#grdDetail').data('kendoGrid');
//    var dataItem = typeof (grid.dataSource.get(dtl_PermintaanBarangDariKebunId)) == "undefined" ? grid.dataSource.getByUid(dtl_PermintaanBarangDariKebunId) : grid.dataSource.get(dtl_PermintaanBarangDariKebunId);
//    return $.ajax({
//        url: '/Input/ExecSQL',
//        data: {
//            scriptSQL:
//                " select JumlahSatuan,Standar_BeratPerSatuan,Tara from [ReferensiEF].[dbo].[Grade]" +
//                " where GradeId='" + dataItem.GradeId + "'",
//            _menuId: menuId
//        },
//        type: 'GET',
//        datatype: 'json'
//    });
//}
////// sampe sini
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


function filterdetailRead(e) {
    return { _menuId: menuId };
}

function filterdetailCreate(e) {
    return { _menuId: menuId };
}
