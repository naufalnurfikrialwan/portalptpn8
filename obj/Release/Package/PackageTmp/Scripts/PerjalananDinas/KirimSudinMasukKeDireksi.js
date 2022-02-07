var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var suratDinasMasukId
var menuId = 'f4b030ca-0c14-4649-a40f-448fa19762cf';

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
    InitHeader();
    disableHeader();
    wnd = $("#modalWindow").kendoWindow({
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
                        kirimSuratDinas($('#SuratDinasMasukId').val(), $('#TanggalDikirimKeDireksi').val(), $('#UserName').val(),
                            $('#TahunAgenda').data('kendoNumericTextBox').value(), $('#NoAgenda').data('kendoNumericTextBox').value()).done(function () {
                            InitHeader();
                            suratDinaMasukId = $('#SuratDinasMasukId').val();
                            $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                                disableHeader();
                                $('#btnDeleteHeader').removeClass('disabledbutton');
                                $('#btnPrintHeader').removeClass('disabledbutton');
                                $('#btnDeleteHeader').attr('disabled', false);
                                $('#btnPrintHeader').attr('disabled', false);
                                $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                            });
                        });
                    }
                    else {
                        openErrWindow();
                    }
                })
                .fail(function (x) {
                    alert("Error! Hubungi Bagian TI");
                });
            } else {
                $(".k-button-icontext").addClass("k-state-disabled").removeClass("k-grid-add"); // edited
            }
        }
    });

function kirimSuratDinas(id, tglkirim, namauser, thn, no) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "exec [dbo].[UbahStatusSudinDariBaruJadiDireksi] '" + id + "', '" + tglkirim.substr(0, 10) + "', '" + namauser + "'," + thn + "," + no,
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });

}


function openErrWindow(e) {
    err.open().center();
    $("#ok").click(function () {
        err.close();
    });
}

function simpanHeader() {
    var url = '/Input/simpanHeader';
    var mdl = ViewToModel();
    return $.ajax({
        url: url,
        data: {
            _model: JSON.stringify(mdl),
            _baru: headerBaru,
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function hapusHeader() {
    wnd.close();
    ConfirmedHapusHeader().done(function (data) {
        if (data) {
            $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                disableHeader();
                InitHeader();
                $('#btnDeleteHeader').removeClass('disabledbutton');
                $('#btnPrintHeader').removeClass('disabledbutton');
                $('#btnDeleteHeader').attr('disabled', false);
                $('#btnPrintHeader').attr('disabled', false);
                $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
            });
        }
        else {
            openErrWindow();
        }
    }).fail(function (x) {
        alert('Error! Hubungi Bagian TI');
    });
}
                

function ConfirmedHapusHeader() {
    var url = '/Input/hapusHeader';
    var mdl = ViewToModel();
    return $.ajax({
        url: url,
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
    if (typeof (e.item) != 'undefined')
        _NomorInputView = this.dataItem(e.item);
    else
        _NomorInputView = e.sender.dataItem();

    if (typeof (_NomorInputView.CCTujuanId_SuratDinasMasuk) == "string") {
        if (_NomorInputView.CCTujuanId_SuratDinasMasuk == "") {
            _NomorInputView.CCTujuanId_SuratDinasMasuk = [];
        }
        else {
            _NomorInputView.CCTujuanId_SuratDinasMasuk = _NomorInputView.CCTujuanId_SuratDinasMasuk.split(",");
        }
    }
    cekData(_NomorInputView.NomorInputView);
    ModelToView(_NomorInputView);

    if (_NomorInputView.StatusSuratDinas == "SUDIN-DIREKSI" || _NomorInputView.StatusSuratDinas == "SUDIN-BARU" || _NomorInputView.StatusSuratDinas == "" || _NomorInputView.StatusSuratDinas == null)
        enableHeader();
    else
        disableHeader();

    var CCTujuanId = $('#CCTujuanId_SuratDinasMasuk').data('kendoMultiSelect');
    CCTujuanId.dataSource.read().done(function (data) {
        CCTujuanId.value(_NomorInputView.CCTujuanId_SuratDinasMasuk);
    });
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
        data: {
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });

}

function filterNomorInput(e) {
    var mdl = JSON.stringify(ViewToModel());
    return { _model: mdl, _menuId: menuId };
}

function TahunBukuOnChange(e) {
    $('#msgInputView').text('');
    tahunBukuOnChange(e).done(function (data) {
        if (data) {
            $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                disableHeader();
                $(".k-button-icontext").addClass("k-state-disabled").removeClass("k-grid-add"); // edited
            });
        }
        else {
            alert("Error! Hubungi Bagian TI")
        }
    }).fail(function (x) {
        alert('Error! Hubungi Bagian TI');
    });
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

function TujuanOnSelect(e) {
    var tujuan = this.dataItem(e.item);
    $('#TujuanId_SuratDinasMasuk').val(tujuan.OrgId);
    $('#NamaTujuan_SuratDinasMasuk').val(tujuan.NamaUnit);
}

function filterTujuan(e) {
    var _scriptSQL = "SELECT * FROM [ReferensiEF].[dbo].[Unit] where NamaUnit like 'bod%'"; 
    return {
        strClassView: 'Ptpn8.Areas.Referensi.Models.Unit',
        scriptSQL: _scriptSQL,
        _menuId: menuId
    }
}

function filterTujuancc(e) {
    var ccTujuanId = $('#CCTujuanId_SuratDinasMasuk').data('kendoMultiSelect');
    var str = ccTujuanId.input.val() == "" || ccTujuanId.input.val() == "Pilih cc Tujuan" ? "XXX" : ccTujuanId.input.val();

    var listId = [];
    for (var i = 0; i < _NomorInputView.CCTujuanId_SuratDinasMasuk.length; i++) {
        listId.push("'" + _NomorInputView.CCTujuanId_SuratDinasMasuk[i].toString() + "'");
    }
    var _scriptSQL = "";

    if (headerBaru == true) {
        _scriptSQL = "select DISTINCT A.OrgId, A.NamaUnit from [ReferensiEF].[dbo].[Unit] as A " + (str != "" && str != "Pilih cc Tujuan" ? "where A.NamaUnit like 'bod%' and A.NamaUnit like '%" + str + "%'" : "");
        return {
            strClassView: 'Ptpn8.Areas.Referensi.Models.Unit',
            scriptSQL: _scriptSQL,
            _menuId: menuId
        }
    }
    else {
        if(listId.length>0) 
            _scriptSQL = "select DISTINCT A.OrgId, A.NamaUnit from [ReferensiEF].[dbo].[Unit] as A " + "where A.NamaUnit like 'bod%' and  OrgId in (" + listId.toString() + ")" + (str != "" && str != "Pilih cc Tujuan" ? " or A.NamaUnit like '%" + str + "%'" : "");
        else
            _scriptSQL = "select DISTINCT A.OrgId, A.NamaUnit from [ReferensiEF].[dbo].[Unit] as A " + (str != "" && str != "Pilih cc Tujuan" ? " where A.NamaUnit like 'bod%' and A.NamaUnit like '%" + str + "%'" : "");

        return {
            strClassView: 'Ptpn8.Areas.Referensi.Models.Unit',
            scriptSQL: _scriptSQL,
            _menuId: menuId
        }
    }
}

function filterCCTujuan(e) {
    var listId = [];
    for (var i = 0; i < _NomorInputView.CCTujuanId_SuratDinasMasuk.length; i++) {
        listId.push("'" + _NomorInputView.CCTujuanId_SuratDinasMasuk[i].toString() + "'");
    }
    return {
        value: $("#NamaTujuan_SuratDinasMasuk").val()
    };

}

function CCTujuanId_SuratDinasMasukOnSelect(e) {
    var ccTujuan = e.sender.dataItems();
    var arr = [];
    if (ccTujuan.length > 0)
    {
        for (var i = 0; i < ccTujuan.length; i++) {
            arr.push(ccTujuan[i].NamaUnit);
        }
        $('#CCNamaTujuan_SuratDinasMasuk').val(arr.toString());
    }
    else
    {
        $('#CCNamaTujuan_SuratDinasMasuk').val('');
    }
}

function btnUploadOnClick(e) {

}


function ModelToView(_NomorInputView) {
    $('#SuratDinasMasukId').val(_NomorInputView.SuratDinasMasukId);
    $('#TahunBuku').data('kendoNumericTextBox').value(_NomorInputView.TahunBuku);
    $('#NomorInput').val(_NomorInputView.NomorInput);
    $('#NomorInputView').val(_NomorInputView.NomorInputView);
    $('#TahunAgenda').data('kendoNumericTextBox').value(_NomorInputView.TahunAgenda);
    $('#NoAgenda').data('kendoNumericTextBox').value(_NomorInputView.NoAgenda);
    $('#UnitId').val(_NomorInputView.UnitId); 
    $('#NamaUnit').val(_NomorInputView.NamaUnit);
    $('#TujuanId_SuratDinasMasuk').val(_NomorInputView.TujuanId_SuratDinasMasuk);
    $('#NamaTujuan_SuratDinasMasuk').val(_NomorInputView.NamaTujuan_SuratDinasMasuk);
    $('#CCTujuanId_SuratDinasMasuk').val(_NomorInputView.CCTujuanId_SuratDinasMasuk);
    $('#CCNamaTujuan_SuratDinasMasuk').val(_NomorInputView.CCNamaTujuan_SuratDinasMasuk);
    $('#TanggalDikirimKeDireksi').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.TanggalDikirimKeDireksi.substr(6))));
    $('#Sifat_SuratDinasMasuk').val(_NomorInputView.Sifat_SuratDinasMasuk);
    $('#TujuanDisposisi_SuratDinasMasuk').val(_NomorInputView.TujuanDisposisi_SuratDinasMasuk);
    $('#IsiDisposisi_SuratDinasMasuk').val(_NomorInputView.IsiDisposisi_SuratDinasMasuk);
    $('#TanggalDidisposisiOlehDireksi').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.TanggalDidisposisiOlehDireksi.substr(6))));
    $('#Kirim_MemoDinasId').val(_NomorInputView.Kirim_MemoDinasId);
    $('#StatusSuratDinas').val(_NomorInputView.StatusSuratDinas);

    var listLampiranSuratDinas = _NomorInputView.Lampiran_SuratDinas;
    if (listLampiranSuratDinas == null || listLampiranSuratDinas == '') {
        listLampiranSuratDinas = [];
    }
    else {
        listLampiranSuratDinas = listLampiranSuratDinas.split(',');
    }

    $("#daftarFile").html("")
    for (var i = 0; i < listLampiranSuratDinas.length; i++) {
        listLampiranSuratDinas[i] = listLampiranSuratDinas[i].substr(_NomorInputView.UnitId.toString().length + 1, 100);
        renderNamaFile(listLampiranSuratDinas[i]);
    }
    $('#Lampiran_SuratDinas').val(listLampiranSuratDinas.toString());

    readSuratMasuk();
}

function renderNamaFile(nmFile) {
    var extension = "";
    var i = nmFile.lastIndexOf('.');
    if (i > 0)
        extension = nmFile.substring(i + 1).toLowerCase().substring(0, 3);
    else
        extension = "";
    $("<p><div class='daftarfile'><table><tr><td><img src='/Content/Images/FileTypeIcon/png/" + extension + ".png'/></td></tr><tr><td>[" + nmFile + "]</td></tr><tr><td><button type='button' onClick='btnHapusOnClick(\"" + nmFile + "\")'><span class='k-font-icon k-i-trash'></span>Hapus</button><button type='button' onClick='btnViewOnClick(\"" + nmFile + "\")'><span class='k-font-icon k-i-justify-clear'></span>View</button></td></tr></table></div></p>").appendTo($("#daftarFile"));
}

function btnViewOnClick(nmFile) {
    var file = $('#UnitId').val() + '_' + nmFile;
    var newwindow = window.open("/LampiranMemo/" + file, "window2", "");
}

function btnHapusOnClick(nmFile) {
    hapusFile(nmFile).done(function (data) {
        var listLampiranSuratDinas = $('#Lampiran_SuratDinas').val();
        if (listLampiranSuratDinas == null || listLampiranSuratDinas == '') {
            listLampiranSuratDinas = [];
        }
        else {
            listLampiranSuratDinas = listLampiranSuratDinas.split(',');
        }

        var index = listLampiranSuratDinas.indexOf(nmFile);
        if (index !== -1) listLampiranSuratDinas.splice(index, 1);
        $('#Lampiran_SuratDinas').val(listLampiranSuratDinas.toString())
        $("#daftarFile").html("")
        for (var i = 0; i < listLampiranSuratDinas.length; i++) {
            renderNamaFile(listLampiranSuratDinas[i]);
        }
    });
}

function hapusFile(nmFile) {
    var url = '/Input/RemoveUploadFile';
    var arr = [];
    arr.push($("#UnitId").val() + "_" + nmFile);
    return $.ajax({
        url: url,
        type: 'POST',
        datatype: 'json',
        data: {
            fileNames: arr,
            folder: "~/LampiranMemo",
            unit: ""
        }
    });
}



function ViewToModel() {
    var listId = $('#CCTujuanId_SuratDinasMasuk').val();
    if (listId == null) {
        listId = "";
    }
    else {
        listId = listId.toString();
    }

    var listTujuan = $('#CCNamaTujuan_SuratDinasMasuk').val();
    if (listTujuan == null) {
        listTujuan = "";
    }
    else {
        listTujuan = listTujuan.toString();
    }

    var listLampiranSuratDinas = $('#Lampiran_SuratDinas').val();
    if (listLampiranSuratDinas == null || listLampiranSuratDinas == '') {
        listLampiranSuratDinas = [];
    }
    else {
        listLampiranSuratDinas = listLampiranSuratDinas.split(',');
    }
    for (var i = 0; i < listLampiranSuratDinas.length; i++) {
        listLampiranSuratDinas[i] = $('#UnitId').val().toString() + '_' + listLampiranSuratDinas[i];
    }
    $('#Lampiran_SuratDinas').val(listLampiranSuratDinas.toString())

    var mdl = {
        SuratDinasMasukId: $('#SuratDinasMasukId').val(),
        TahunBuku: $('#TahunBuku').val(),
        NomorInput: $('#NomorInput').val(),
        NomorInputView: Array(5 - String($('#NomorInput').val()).length + 1).join('0') + $('#NomorInput').val() + ' - ' + $('#NomorInput').val().toUpperCase(),
        TahunAgenda: $('#TahunAgenda').val(),
        NoAgenda: $('#NoAgenda').val(),
        UnitId: $('#UnitId').val(),
        NamaUnit: $('#NamaUnit').val(),
        TujuanId_SuratDinasMasuk: $('#TujuanId_SuratDinasMasuk').val(),
        NamaTujuan_SuratDinasMasuk: $('#NamaTujuan_SuratDinasMasuk').val(),
        CCTujuanId_SuratDinasMasuk: listId.toString(),
        CCNamaTujuan_SuratDinasMasuk: listTujuan.toString(),
        TanggalDikirimKeDireksi: $('#TanggalDikirimKeDireksi').val(),
        Sifat_SuratDinasMasuk: $('#Sifat_SuratDinasMasuk').val(),
        TujuanDisposisi_SuratDinasMasuk: $('#TujuanDisposisi_SuratDinasMasuk').val(),
        IsiDisposisi_SuratDinasMasuk: $('#IsiDisposisi_SuratDinasMasuk').val(),
        TanggalDidisposisiOlehDireksi: $('#TanggalDidisposisiOlehDireksi').val(),
        Kirim_MemoDinasId: $('#Kirim_MemoDinasId').val(),
        StatusSuratDinas: $('#StatusSuratDinas').val(),
        Lampiran_SuratDinas: $('#Lampiran_SuratDinas').val()
    };
    return mdl;
}

function stringDecode(strDecode) {
    var url = '/Input/StringDecode';
    return $.ajax({
        url: url,
        data: {
            str: strDecode
        },
        type: 'POST',
        datatype: 'json'
    });
}


function InitHeader() {
    //$('#SuratDinasMasukId').val('');
    //$('#TahunBuku').val('');
    //$('#NomorInput').val('');
    //$('#NomorInputView').val('');
    $('#TahunAgenda').val('');
    $('#NoAgenda').val('');
    //$('#UnitId').val('');
    //$('#NamaUnit').val('');
    $('#TujuanId_SuratDinasMasuk').val('');
    $('#NamaTujuan_SuratDinasMasuk').val('');
    $('#CCTujuanId_SuratDinasMasuk').val('');
    $('#CCNamaTujuan_SuratDinasMasuk').val('');
    //$('#TanggalDikirimKeDireksi').val('');
    //$('#Sifat_SuratDinasMasuk').val('');
    //$('#TujuanDisposisi_SuratDinasMasuk').val('');
    //$('#IsiDisposisi_SuratDinasMasuk').val('');
    //$('#TanggalDidisposisiOlehDireksi').val('');
    //$('#Kirim_MemoDinasId').val('');
    $('#img1').attr('src', '');
    $('#img2').attr('src', '');
    $('#img3').attr('src', '');
    $('#img4').attr('src', '');
    $('#img5').attr('src', '');
    $('#imgdisposisi').attr('src', '');

}

function disableHeader() {
    $("._key").find('span').addClass('disabledbutton');
    $("._key").addClass('disabledbutton');
    $("._nonkey").find('span').addClass('disabledbutton');
    $("._nonkey").addClass('disabledbutton');
    $('#btnSubmitHeader').addClass('disabledbutton');
    $('#btnDeleteHeader').addClass('disabledbutton');
    $('#btnPrintHeader').addClass('disabledbutton');
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

function readSuratMasuk() {
    ambilSuratMasuk($('#TahunAgenda').data('kendoNumericTextBox').value(), $('#NoAgenda').data('kendoNumericTextBox').value(),
        $('#SuratDinasMasukId').val()).done(function (dta) {
        if (dta.length > 0) {
            $('#Nomor_SuratMasuk').val(dta[0][5]);
            $('#Pengirim_SuratMasuk').val(dta[0][7]);
            $('#Perihal_SuratMasuk').val(dta[0][9]);
            $('#Tanggal_SuratMasuk').data('kendoDateTimePicker').value(new Date(dta[0][4]));
            $('#img1').removeAttr('src');
            $('#img2').removeAttr('src');
            $('#img3').removeAttr('src');
            $('#img4').removeAttr('src');
            $('#img5').removeAttr('src');
            $('#imgdisposisi').removeAttr('src');

            d = new Date();
            if (dta[0][10] == "System.Byte[]") {
                $('#img1').attr('src', '/Content/Images/View/' + $('#UserName').val() + '_img1.jpg?' + d.getTime());
            }
            else {
                $('#img1').removeAttr('src');
            }
            if (dta[0][11] == "System.Byte[]") {
                $('#img2').attr('src', '/Content/Images/View/' + $('#UserName').val() + '_img2.jpg?' + d.getTime());
            }
            else {
                $('#img2').removeAttr('src');
            }
            if (dta[0][12] == "System.Byte[]") {
                $('#img3').attr('src', '/Content/Images/View/' + $('#UserName').val() + '_img3.jpg?' + d.getTime());
            }
            else {
                $('#img3').removeAttr('src');
            }

            if (dta[0][13] == "System.Byte[]") {
                $('#img4').attr('src', '/Content/Images/View/' + $('#UserName').val() + '_img4.jpg?' + d.getTime());
            }
            else {
                $('#img4').removeAttr('src');
            }
            if (dta[0][14] == "System.Byte[]") {
                $('#img5').attr('src', '/Content/Images/View/' + $('#UserName').val() + '_img5.jpg?' + d.getTime());
            }
            else {
                $('#img5').removeAttr('src');
            }
            if (dta[0][15] == "System.Byte[]") {
                $('#imgdisposisi').attr('src', '/Content/Images/View/' + $('#UserName').val() + '_imgdisposisi.jpg?' + d.getTime());
            }
            else {
                $('#imgdisposisi').removeAttr('src');
            }

        }
        else {
            $('#Nomor_SuratMasuk').val('');
            $('#Pengirim_SuratMasuk').val('');
            $('#Perihal_SuratMasuk').val('');
            $('#Tanggal_SuratMasuk').data('kendoDateTimePicker').value(new Date('01/01/1900'));
            $('#img1').attr('src', '');
            $('#img2').attr('src', '');
            $('#img3').attr('src', '');
            $('#img4').attr('src', '');
            $('#img5').attr('src', '');
            $('#imgdisposisi').attr('src', '');
        }
    });
}

function ambilSuratMasuk(thn,nmr,id) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "SET DATEFORMAT DMY; SELECT TOP 1 [thn_agenda], [no_agenda], [tgl_terima], [penerima], [tgl_surat], [no_surat], [kel_pengirim], [pengirim], [kel_perihal], [perihal], [img1],img2,img3,img4,img5, [imgdisposisi] FROM[Sekper].[dbo].[agenda_surat_masuk] WHERE thn_agenda=" + thn + " and no_agenda=" + nmr + " and((select count(*) from [SPDK_KanpusEF].[dbo].[eOffice_RelasiAgendaSudinDanKirimKeDireksi] where TahunAgenda=" + thn + " and NoAgenda = " + nmr +") = 0 OR(select count(*) from [SPDK_KanpusEF].[dbo].[eOffice_RelasiAgendaSudinDanKirimKeDireksi] where SuratDinasMasukId = '"+id+"' AND TahunAgenda = "+thn+" and NoAgenda = "+nmr+")>=1)",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}


function onSuccess(e) {
    var fileLampiran = $('#Lampiran_SuratDinas').val().split(',');
    if ($('#Lampiran_SuratDinas').val() == "") fileLampiran = [];
    fileLampiran.push(e.files[0].name)
    $('#Lampiran_SuratDinas').val(fileLampiran.toString());
    renderNamaFile(e.files[0].name);
}

function UploadSuccess(e) {
    var fileLampiran = $('#Lampiran_SuratDinas').val().split(',');
    if ($('#Lampiran_SuratDinas').val() == "") fileLampiran = [];
    fileLampiran.push(e.files[0].name)
    $('#Lampiran_SuratDinas').val(fileLampiran.toString());
}
