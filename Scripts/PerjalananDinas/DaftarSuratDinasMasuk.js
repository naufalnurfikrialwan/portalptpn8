var wndIsiSudin, wndDisposisiSudin, wndBuatMemoPengantar, wndBuktiSelesai;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var rowNumber = 0;
var menuId = '664cd944-fc12-404c-ae93-6164b538920d';

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

$(function () {
    hubMemoDinas = $.connection.memoDinasHub;
    $.connection.hub.start().done(function () {
        // sudah terhubung ke hub
    });

    hubMemoDinas.client.suratKeluarMemoDinas = function (data) {
        var grd = $("#grdDetail").data('kendoGrid');
        grd.dataSource.read();
    };

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
    //wndIsiSudin = $("#wIsiSuratDinas").kendoWindow({
    //    title: "Isi Surat Dinas",
    //    modal: true,
    //    visible: false,
    //    resizable: true
    //}).data("kendoWindow");

    wndDisposisiSudin = $("#wDisposisiSuratDinas").kendoWindow({
        title: "Disposisi Surat Dinas",
        modal: true,
        visible: false,
        resizable: true
    }).data("kendoWindow");

    wndBuatMemoPengantar = $("#wBuatMemoPengantar").kendoWindow({
        title: "Buat Memo Pengantar",
        modal: true,
        visible: false,
        resizable: true
    }).data("kendoWindow");


    wndIsiMemo = $("#wIsiMemo").kendoWindow({
        title: "Kirim Memo",
        modal: true,
        visible: false,
        resizable: true
    }).data("kendoWindow");

    wndBuktiSelesai = $("#wBuktiSelesai").kendoWindow({
        title: "Bukti Memo Selesai",
        modal: true,
        visible: false,
        resizable: true
        //width: 800,
        //height: 600
    }).data("kendoWindow");


    gridStatus = 'belum-ngapa-ngapain';
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
        }
    });

function openWindowIsiSudin(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);

    readSuratMasukSudin(data.TahunAgenda, data.NoAgenda);

    wndIsiSudin.open().maximize();
}

function openWindowBuatMemoPengantar(e) {
    //e.preventDefault();
    //var grid = this;
    //var row = $(e.currentTarget).closest("tr");
    //var data = grid.dataItem(row);

    //wndBuatMemoPengantar.open().center();

    //$("#Ok").click(function () {
    //    wndBuatMemoPengantar.close();
    //});

    //e.preventDefault();
    //$.ajax({
    //    url: 'Input/Input',
    //    data: { aplikasiId: '7b8b79fa-04e7-40a3-a7b5-d1de6bca2b83', menuId: '0dedeaed-9a05-4fa0-9b21-b9c61b3e5f52'},
    //    success: function () {
    //        alert('Added');
    //    }
    //});
}

function buatSuratPengantar(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);

    ubahStatusNotif($('#namaUser').val(), data.SuratDinasMasukId).done(function () {
        sisipParameterUntukSuratPengantar(data.SuratDinasMasukId.toString()+','+data.TujuanId_SuratDinasMasuk).done(function () {
            window.location.href = "/Input/Input?aplikasiId=7b8b79fa-04e7-40a3-a7b5-d1de6bca2b83&menuId=76633740-79e0-4077-829e-7b45ebd31ac2";
        });
    });
}

function sisipParameterUntukSuratPengantar(id) {
    var url = '/Input/buatContextParameter';
    return $.ajax({
        url: url,
        data: {
            namaContext: 'ParameterUntukSuratPengantar',
            isiContext: id
        },
        type: 'POST',
        datatype: 'json'
    });
}


function readSuratMasukSudin(thn, no) {
    ambilSuratMasukSudin(thn, no).done(function (dta) {
        if (dta.length > 0) {
            $('#_Nomor_SuratMasuk').text(dta[0][5]);
            $('#_Pengirim_SuratMasuk').text(dta[0][7]);
            $('#_Perihal_SuratMasuk').text(dta[0][9]);
            $('#_Tanggal_SuratMasuk').text(dta[0][4]);
            $('#img1').removeAttr('src');
            d = new Date();
            $('#img1').attr('src', '/Content/Images/View/' + $('#namaUser').val() + '_img1.jpg?' + d.getTime());
        }
    });
}

function ambilSuratMasukSudin(thn, nmr) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "SET DATEFORMAT DMY; SELECT TOP 1 [thn_agenda],[no_agenda],[tgl_terima],[penerima],[tgl_surat],[no_surat],[kel_pengirim],[pengirim],[kel_perihal],[perihal],[img1],[imgdisposisi] FROM [Sekper].[dbo].[agenda_surat_masuk] WHERE thn_agenda=" + thn + " and no_agenda=" + nmr,
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function openWindowDisposisiSudin(e) {
    e.preventDefault();

    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var __data = grid.dataItem(row);
    dataROW = __data;

    if (__data.StatusSuratDinas == 'SUDIN-DISPOSISI' || __data.StatusSuratDinas == 'SUDIN-KIRIM') {

        var _X = dataROW.CCTujuanId_SuratDinasMasuk.split(',');
        var _Y = dataROW.TujuanId_SuratDinasMasuk.split(',');
        _X.push(_Y[0]);

        wndDisposisiSudin.open().center();

        $('#_SifatSurat').data('kendoDropDownList').value(dataROW.Sifat_SuratDinasMasuk);

        // DIRUT
        if (_X.join().toUpperCase().search('C6DFDB6D-056E-45CC-9D07-A2207FF5FCC3') > -1) {
            $("#fieldlistDitujukanA").prop('checked', true).prop("disabled", true);
        }
        // DIROP
        if (_X.join().toUpperCase().search('BDD778FE-BE35-42D7-A891-4B2BE7126CC7') > -1) {
            $("#fieldlistDitujukanB").prop('checked', true).prop("disabled", true);
        }
        // DIRMA
        if (_X.join().toUpperCase().search('D89C3607-5739-4620-80AD-6259A5216907') > -1) {
            $("#fieldlistDitujukanC").prop('checked', true).prop("disabled", true);
        }
        // DIRKOM
        if (_X.join().toUpperCase().search('24BDBB95-0CD5-463E-9ABC-A4B03AB70A1A') > -1) {
            $("#fieldlistDitujukanD").prop('checked', true).prop("disabled", true);
        }

        $("#fieldlistDitujukanA").prop("disabled", true);
        $("#fieldlistDitujukanB").prop('disabled', true);
        $("#fieldlistDitujukanC").prop('disabled', true);
        $("#fieldlistDitujukanD").prop('disabled', true);


        var arrDitujukan = [];
        for (var i = 22; i > 0; i--) {
            if (typeof ($("#fieldlistDitujukan" + i.toString()).val()) != "undefined") {
                $("#fieldlistDitujukan" + i.toString()).prop("disabled", true);
            }
        }

        var tujuanDisposisi = dataROW.TujuanDisposisi_SuratDinasMasuk;
        if (tujuanDisposisi.toUpperCase().search("C7A94AC2-6148-400D-9235-A5DE1837A55F") > -1) {
            $("#fieldlistDitujukan1").prop('checked', true).prop("disabled", true);
        }
        else {
            $('#lbl1Ditujukan1').addClass('unChk');
            $('#lbl2Ditujukan1').addClass('unChk');
        }

        if (tujuanDisposisi.toUpperCase().search("580A7BA1-31E5-4337-ABB2-24A07F5E9778") > -1) {
            $("#fieldlistDitujukan2").prop('checked', true).prop("disabled", true);
        }
        else {
            $('#lbl1Ditujukan2').addClass('unChk');
            $('#lbl2Ditujukan2').addClass('unChk');
        }

        if (tujuanDisposisi.toUpperCase().search("8FC50B2C-2A83-42CD-9C3C-0A14B6C9CD5A") > -1) {
            $("#fieldlistDitujukan3").prop('checked', true).prop("disabled", true);
        }
        else {
            $('#lbl1Ditujukan3').addClass('unChk');
            $('#lbl2Ditujukan3').addClass('unChk');
        }

        if (tujuanDisposisi.toUpperCase().search("72928006-34F6-46D7-B668-4CA588DF714F") > -1) {
            $("#fieldlistDitujukan4").prop('checked', true).prop("disabled", true);
        }
        else {
            $('#lbl1Ditujukan4').addClass('unChk');
            $('#lbl2Ditujukan4').addClass('unChk');
        }

        if (tujuanDisposisi.toUpperCase().search("78D31B7B-DA29-4A49-B77B-861EE5458D6B") > -1) {
            $("#fieldlistDitujukan5").prop('checked', true).prop("disabled", true);
        }
        else {
            $('#lbl1Ditujukan5').addClass('unChk');
            $('#lbl2Ditujukan5').addClass('unChk');
        }

        if (tujuanDisposisi.toUpperCase().search("A492B23A-E11F-4650-BC19-A48713AFEFB6") > -1) {
            $("#fieldlistDitujukan6").prop('checked', true).prop("disabled", true);
        }
        else {
            $('#lbl1Ditujukan6').addClass('unChk');
            $('#lbl2Ditujukan6').addClass('unChk');
        }

        if (tujuanDisposisi.toUpperCase().search("977EF1BF-D05F-4680-9080-58093FBE7D95") > -1) {
            $("#fieldlistDitujukan7").prop('checked', true).prop("disabled", true);
        }
        else {
            $('#lbl1Ditujukan7').addClass('unChk');
            $('#lbl2Ditujukan7').addClass('unChk');
        }

        if (tujuanDisposisi.toUpperCase().search("30D4C2DB-8BB4-4EE6-884D-6FFBEF181D11") > -1) {
            $("#fieldlistDitujukan8").prop('checked', true).prop("disabled", true);
        }
        else {
            $('#lbl1Ditujukan8').addClass('unChk');
            $('#lbl2Ditujukan8').addClass('unChk');
        }

        if (tujuanDisposisi.toUpperCase().search("BBEEFC43-26E5-4F66-9DE3-D523A5462855") > -1) {
            $("#fieldlistDitujukan9").prop('checked', true).prop("disabled", true);
        }
        else {
            $('#lbl1Ditujukan9').addClass('unChk');
            $('#lbl2Ditujukan9').addClass('unChk');
        }

        if (tujuanDisposisi.toUpperCase().search("0AAFC5BE-96ED-4213-AEB4-6B8808404878") > -1) {
            $("#fieldlistDitujukan10").prop('checked', true).prop("disabled", true);
        }
        else {
            $('#lbl1Ditujukan10').addClass('unChk');
            $('#lbl2Ditujukan10').addClass('unChk');
        }

        if (tujuanDisposisi.toUpperCase().search("F0D332DB-817A-479B-A229-FF31BBBB3872") > -1) {
            $("#fieldlistDitujukan11").prop('checked', true).prop("disabled", true);
        }
        else {
            $('#lbl1Ditujukan11').addClass('unChk');
            $('#lbl2Ditujukan11').addClass('unChk');
        }

        if (tujuanDisposisi.toUpperCase().search("2FD211DD-610D-4EB7-B209-F4D1ED24D1CB") > -1) {
            $("#fieldlistDitujukan12").prop('checked', true).prop("disabled", true);
        }
        else {
            $('#lbl1Ditujukan12').addClass('unChk');
            $('#lbl2Ditujukan12').addClass('unChk');
        }

        if (tujuanDisposisi.toUpperCase().search("A17E11A6-81AB-4EAB-BD3F-936833EFA40B") > -1) {
            $("#fieldlistDitujukan13").prop('checked', true).prop("disabled", true);
        }
        else {
            $('#lbl1Ditujukan13').addClass('unChk');
            $('#lbl2Ditujukan13').addClass('unChk');
        }

        if (tujuanDisposisi.toUpperCase().search("FD73DF57-2F13-4A4D-980F-BEB92EBED1FC") > -1) {
            $("#fieldlistDitujukan14").prop('checked', true).prop("disabled", true);
        }
        else {
            $('#lbl1Ditujukan14').addClass('unChk');
            $('#lbl2Ditujukan14').addClass('unChk');
        }

        if (tujuanDisposisi.toUpperCase().search("E89C016D-D5F2-460F-8E88-225695B257B7") > -1) {
            $("#fieldlistDitujukan15").prop('checked', true).prop("disabled", true);
        }
        else {
            $('#lbl1Ditujukan15').addClass('unChk');
            $('#lbl2Ditujukan15').addClass('unChk');
        }

        if (tujuanDisposisi.toUpperCase().search("DD9B064B-6A3C-43EE-A3DC-8CF8E100A4BF") > -1) {
            $("#fieldlistDitujukan16").prop('checked', true).prop("disabled", true);
        }
        else {
            $('#lbl1Ditujukan16').addClass('unChk');
            $('#lbl2Ditujukan16').addClass('unChk');
        }

        if (tujuanDisposisi.toUpperCase().search("147031BD-D0E2-4730-87B8-F12E2E1EE1FC") > -1) {
            $("#fieldlistDitujukan17").prop('checked', true).prop("disabled", true);
        }
        else {
            $('#lbl1Ditujukan17').addClass('unChk');
            $('#lbl2Ditujukan17').addClass('unChk');
        }

        if (tujuanDisposisi.toUpperCase().search("329AAF85-2087-4F41-B4F6-304C70A178C0") > -1) {
            $("#fieldlistDitujukan18").prop('checked', true).prop("disabled", true);
        }
        else {
            $('#lbl1Ditujukan18').addClass('unChk');
            $('#lbl2Ditujukan18').addClass('unChk');
        }

        if (tujuanDisposisi.toUpperCase().search("60A6881F-B09B-440D-8742-778D83A455C1") > -1) {
            $("#fieldlistDitujukan19").prop('checked', true).prop("disabled", true);
        }
        else {
            $('#lbl1Ditujukan19').addClass('unChk');
            $('#lbl2Ditujukan19').addClass('unChk');
        }

        if (tujuanDisposisi.toUpperCase().search("E8B60A6F-C658-44A4-9486-DFEF5EADE092") > -1) {
            $("#fieldlistDitujukan20").prop('checked', true).prop("disabled", true);
        }
        else {
            $('#lbl1Ditujukan20').addClass('unChk');
            $('#lbl2Ditujukan20').addClass('unChk');
        }

        if (tujuanDisposisi.toUpperCase().search("E7F2EB45-A6FF-4FBD-9006-5FB34F7AFDC1") > -1) {
            $("#fieldlistDitujukan21").prop('checked', true).prop("disabled", true);
        }
        else {
            $('#lbl1Ditujukan21').addClass('unChk');
            $('#lbl2Ditujukan21').addClass('unChk');
        }

        if (tujuanDisposisi.toUpperCase().search("C4F3C41D-AAE2-44BD-8C8A-A22DFD50BA66") > -1) {
            $("#fieldlistDitujukan22").prop('checked', true).prop("disabled", true);
        }
        else {
            $('#lbl1Ditujukan22').addClass('unChk');
            $('#lbl2Ditujukan22').addClass('unChk');
        }


        for (var i = 1; i < 15; i++) {
            $("#fieldlistDisposisi" + i.toString()).prop('checked', false).prop("disabled", true);
        }
        arrDisposisi = __data.IsiDisposisi_SuratDinasMasuk.split(",");
        for (var i = 1; i < 15; i++) {
            if (jQuery.inArray($('#lblDisposisi' + i.toString()).text(), arrDisposisi) > -1) {
                $("#fieldlistDisposisi" + i.toString()).prop('checked', true).prop("disabled", true);
            }
            else {
                $('#lblDisposisi' + i.toString()).addClass("unChk");
            }
        }
        $("#penjelasan").val(__data.PenjelasanDisposisi_SuratDinasMasuk).prop("disabled", true);
        $('#tgldisposisi').data('kendoDateTimePicker').value(__data.TanggalDidisposisiOlehDireksi);
    }
}

function openWindowIsiMemo(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    var Isi = data.Isi_MemoDinasFinal;

    if (data.SifatSurat_MemoDinas == 'RAHASIA') {
        $('#wIsiMemo').css('display', 'none')
    }
    else {
        $('#wIsiMemo').css('display', 'block')
        wndIsiMemo.open().center();
        stringDecode(data.Isi_MemoDinasFinal).done(function (data) {
            $('#isiMemo').data('kendoEditor').value(data);
            $('#isiMemo').data('kendoEditor').body.setAttribute('contenteditable', false);
        });

        $('#_NoMemo').text(data.Nomor_MemoDinasView);
        $('#_Kepada').text(data.NamaTujuan_MemoDinas);
        $('#_Dari').text(data.NamaUnit);
        $('#_Perihal').text(data.Perihal_MemoDinas.toString().toUpperCase());
        $('#_Tanggal').text(data.Tanggal_MemoDinas.getDate().toString() + '/' + (data.Tanggal_MemoDinas.getMonth()+1).toString() + '/' + data.Tanggal_MemoDinas.getFullYear().toString());

        $('#_Lampiran_MemoDinas').text(data.Lampiran_MemoDinas)

        var listLampiranMemoDinas = data.Lampiran_MemoDinas;
        if (listLampiranMemoDinas == null || listLampiranMemoDinas == '') {
            listLampiranMemoDinas = [];
        }
        else {
            listLampiranMemoDinas = listLampiranMemoDinas.split(',');
        }
        $("#daftarFile").html("")
        for (var i = 0; i < listLampiranMemoDinas.length; i++) {
            listLampiranMemoDinas[i] = listLampiranMemoDinas[i].substr(data.UnitId.toString().length + 1, 100);
            renderNamaFile(listLampiranMemoDinas[i]);
        }

        var treeV = $('#_Tembusan').data('kendoTreeView');
        treeV.dataSource.data([])
        var arrTembusan = data.CCNamaTujuan_MemoDinas.split(',');
        for (var i = 0; i < arrTembusan.length; i++) {
            treeV.append({ id: i, text: '- ' + arrTembusan[i] });
        }

        //var tgl = new Date(parseInt(data.Tanggal_MemoDinas.substr(6)));
        var tgl = data.Tanggal_MemoDinas;
        $('#QRMemoDinas').data('kendoQRCode').value("No Memo: " + data.Nomor_MemoDinasView + "; Kepada: " + data.NamaTujuan_MemoDinas + "; Dari: " + data.NamaUnit + ";" + "Perihal: " + data.Perihal_MemoDinas.toString().toUpperCase() + "; Tanggal: " +
            tgl.getDate().toString() + '/' + tgl.getMonth().toString() + '/' + tgl.getFullYear().toString());

        if (data.StatusDistribusi == true) {
            $('#yaMemoDistribusi').show();
            $('#_TahunAgenda_SuratMasuk').text(data.TahunAgenda_SuratMasuk);
            $('#_NomorAgenda_SuratMasuk').text(data.NomorAgenda_SuratMasuk);
            readSuratMasuk();
            
        }
        else {
            $('#yaMemoDistribusi').hide();
            $('#_TahunAgenda_SuratMasuk').text('');
            $('#_NomorAgenda_SuratMasuk').text('');
            
        }


        $("#Ok").click(function () {
            wndIsiMemo.close();
        });
    }
}

function renderNamaFile(nmFile) {
    var extension = "";
    var i = nmFile.lastIndexOf('.');
    if (i > 0)
        extension = nmFile.substring(i + 1).toLowerCase().substring(0, 3);
    else
        extension = "";
    $("<p><div class='daftarfile'><table><tr><td><img src='/Content/Images/FileTypeIcon/png/" + extension + ".png'/></td></tr><tr><td>[" + nmFile + "]</td></tr><tr><td><button type='button' onClick='btnViewOnClick(\"" + nmFile + "\")'><span class='k-font-icon k-i-pencil'></span>Lihat</button></td></tr></table></div></p>").appendTo($("#daftarFile"));
}

function btnViewOnClick(nmFile) {
    var file = $('#UnitId').val() + '_' + nmFile;
    var newwindow = window.open("/LampiranMemo/" + file, "window2", "");
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

function gridDestroy() {
    $("#grdDetail").data("kendoGrid").dataSource.read([]);
}


function btnProsesOnClick(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    grid.dataSource.read();
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

function filterNomorInput(e) {
    var mdl = JSON.stringify(ViewToModel());
    return { _model: mdl, _menuId: menuId };
}

// Detail

function grdOnChange(e) {
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


function filterdetailRead(e) {

    var nBulan = $('#ddlBulan').data('kendoDropDownList').selectedIndex + 1;
    var nTahun = $('#TahunBuku').val();
    var aa =
        "SELECT DISTINCT A.SuratDinasMasukId,A.TahunAgenda, A.NoAgenda, A.UnitId, A.TujuanId_SuratDinasMasuk, " +
        "A.CCTujuanId_SuratDinasMasuk, A.TanggalDikirimKeDireksi, A.Kirim_MemoDinasId, A.TahunBuku, " +
        "A.NomorInput, A.StatusSuratDinas, A.PengirimSuratDinas FROM [SPDK_KanpusEF].[dbo].[eOffice_SuratDinasMasuk] A " +
        "JOIN [SPDK_KanpusEF].[dbo].[eOffice_DisposisiSuratDinasMasuk] B ON A.SuratDinasMasukId = B.SuratDinasMasukId " +
        "where month(B.TanggalDidisposisiOlehDireksi)=" + nBulan.toString() + " and year(B.TanggalDidisposisiOlehDireksi)=" + nTahun.toString() + " ";

    return {
        strClassView: "Ptpn8.Models.MemoOnline.View_SuratDinasMasuk",
        scriptSQL: aa,
        _menuId: menuId
    };
}

function ddlBulanOnSelect(e) {

}

function filterdetailReadKirimMemoDinas(a) {
    var aa = "declare @bulanRomawi varchar(50); set @bulanRomawi = 'I   II  III IV  V   VI  VII VIIIIX  X   XI  XII '; " +
        "select DISTINCT C.NomorAgenda_SuratMasuk,C.StatusDistribusi,C.Kirim_MemoDinasId, StatusTindakLanjutMemoDinas=case when (select count(*) from [dbo].[eOffice_TindakLanjut_MemoDinas] D where D.Terima_MemoDinasId=A.Terima_MemoDinasId and D.StatusTindakLanjutMemoDinas like 'SELESAI')>0 then 'SELESAI' else 'BELUM SELESAI' end, " +
        "'M/'+B.Nomenklatur+'/'+cast(C.Nomor_MemoDinas as varchar(5))+'/'+substring(@bulanRomawi,(month(C.Tanggal_MemoDinas)*4)-3,4)+'/'+rtrim(cast(year(C.Tanggal_MemoDinas) as varchar(4))) as Nomor_MemoDinasView, " +
        "C.Tanggal_MemoDinas, C.Perihal_MemoDinas, (select top 1 NamaUnit from [ReferensiEF].[dbo].[Unit] where OrgId=C.TujuanId_MemoDinas) as NamaTujuan_MemoDinas, C.StatusMemoDinas, C.Lampiran_MemoDinas, " +
        "C.Isi_MemoDinasFinal, [SPDK_KanpusEF].[dbo].[func_ConcatCCTujuan](C.CCTujuanId_MemoDinas) as CCNamaTujuan_MemoDinas " +
        "from [SPDK_KanpusEF].[dbo].[eOffice_Kirim_MemoDinas] C " +
        "join [SPDK_KanpusEF].[dbo].[eOffice_Terima_MemoDinas] A on A.Kirim_MemoDinasId=C.Kirim_MemoDinasId " +
        "join [ReferensiEF].[dbo].[Unit] B on C.UnitId=B.OrgId " +
        "where [SuratDinasMasukId]='" + a + "'";

    return {
        strClassView: "Ptpn8.Models.MemoOnline.ViewKirim_MemoDinas",
        scriptSQL: aa,
        _menuId: menuId
    };
}

function filterdetailReadTerimaMemoDinas(a) {
    return {
        strClassView: "Ptpn8.Models.MemoOnline.ViewTerima_MemoDinas",
        scriptSQL: "select A.Terima_MemoDinasId,A.Kirim_MemoDinasId,A.UnitId,A.NomorTerima_MemoDinas,A.TanggalTerima_MemoDinas," +
            "A.Penerima_MemoDinas, A.TanggalDisposisi_MemoDinas, A.PembuatDisposisi_MemoDinas, A.TujuanDisposisi_MemoDinas,A.IsiDisposisi_MemoDinas," +
            "A.PenjelasanDisposisi_MemoDinas, A.StatusTerimaMemoDinas, A.TujuanIdDisposisi_MemoDinas, B.NamaUnit from [SPDK_KanpusEF].[dbo].[eOffice_Terima_MemoDinas] A " +
            "join[ReferensiEF].[dbo].[Unit] B on A.UnitId=B.OrgId where [Kirim_MemoDinasId]='" + a + "'",
        _menuId: menuId
    };
}

function filterdetailReadTLMemoDinas(a) {
    return {
        strClassView: "Ptpn8.Models.MemoOnline.ViewTindakLanjut_MemoDinas",
        scriptSQL: "select A.TindakLanjut_MemoDinasId,A.Terima_MemoDinasId,A.UnitId,A.TanggalTerimaDisposisi_MemoDinas,A.PembuatTindakLanjut_MemoDinas," +
            "A.PenjelasanTindakLanjut_MemoDinas,A.StatusTindakLanjutMemoDinas,A.PICTL_MemoDinas,A.TanggalTL_MemoDinas,A.KeteranganSelesai_MemoDinas,A.TanggalSelesai_MemoDinas, A.BuktiSelesai_MemoDinas, " +
            "B.NamaUnit from [SPDK_KanpusEF].[dbo].[eOffice_TindakLanjut_MemoDinas] A " +
            "join[ReferensiEF].[dbo].[Unit] B on A.UnitId=B.OrgId where [Terima_MemoDinasId]='" + a + "'",
        _menuId: menuId
    };
}

function readSuratMasuk() {
    ambilSuratMasuk($('#_TahunAgenda_SuratMasuk').text(), $('#_NomorAgenda_SuratMasuk').text()).done(function (dta) {
        if (dta.length > 0) {
            $('#_Nomor_SuratMasuk').text(dta[0][5]);
            $('#_Pengirim_SuratMasuk').text(dta[0][7]);
            $('#_Perihal_SuratMasuk').text(dta[0][9]);
            $('#_Tanggal_SuratMasuk').text(dta[0][4]);
            $('#img1').removeAttr('src');
            $('#imgdisposisi').removeAttr('src');
            d = new Date();
            $('#img1').attr('src', '/Content/Images/View/' + $('#namaUser').val() + '_img1.jpg?' + d.getTime());
            $('#imgdisposisi').attr('src', '/Content/Images/View/' + $('#namaUser').val() + '_imgdisposisi.jpg?' + d.getTime());
            if (true) {

                var _X = dta[0][15].split(',');
                var _Y = dta[0][14].split(',');
                _X.push(_Y[0]);

                $('#_SifatSurat').data('kendoDropDownList').value(dta[0][16]);

                // DIRUT
                if (_X.join().toUpperCase().search('C6DFDB6D-056E-45CC-9D07-A2207FF5FCC3') > -1) {
                    $("#fieldlistDitujukanA").prop('checked', true).prop("disabled", true);
                }
                // DIROP
                if (_X.join().toUpperCase().search('BDD778FE-BE35-42D7-A891-4B2BE7126CC7') > -1) {
                    $("#fieldlistDitujukanB").prop('checked', true).prop("disabled", true);
                }
                // DIRMA
                if (_X.join().toUpperCase().search('D89C3607-5739-4620-80AD-6259A5216907') > -1) {
                    $("#fieldlistDitujukanC").prop('checked', true).prop("disabled", true);
                }
                // DIRKOM
                if (_X.join().toUpperCase().search('24BDBB95-0CD5-463E-9ABC-A4B03AB70A1A') > -1) {
                    $("#fieldlistDitujukanD").prop('checked', true).prop("disabled", true);
                }

                $("#fieldlistDitujukanA").prop("disabled", true);
                $("#fieldlistDitujukanB").prop('disabled', true);
                $("#fieldlistDitujukanC").prop('disabled', true);
                $("#fieldlistDitujukanD").prop('disabled', true);


                var arrDitujukan = [];
                for (var i = 22; i > 0; i--) {
                    if (typeof ($("#fieldlistDitujukan" + i.toString()).val()) != "undefined") {
                        $("#fieldlistDitujukan" + i.toString()).prop("disabled", true);
                    }
                }

                var tujuanDisposisi = dta[0][17];
                if (tujuanDisposisi.toUpperCase().search("C7A94AC2-6148-400D-9235-A5DE1837A55F") > -1) {
                    $("#fieldlistDitujukan1").prop('checked', true).prop("disabled", true);
                }
                else {
                    $('#lbl1Ditujukan1').addClass('unChk');
                    $('#lbl2Ditujukan1').addClass('unChk');
                }

                if (tujuanDisposisi.toUpperCase().search("580A7BA1-31E5-4337-ABB2-24A07F5E9778") > -1) {
                    $("#fieldlistDitujukan2").prop('checked', true).prop("disabled", true);
                }
                else {
                    $('#lbl1Ditujukan2').addClass('unChk');
                    $('#lbl2Ditujukan2').addClass('unChk');
                }

                if (tujuanDisposisi.toUpperCase().search("8FC50B2C-2A83-42CD-9C3C-0A14B6C9CD5A") > -1) {
                    $("#fieldlistDitujukan3").prop('checked', true).prop("disabled", true);
                }
                else {
                    $('#lbl1Ditujukan3').addClass('unChk');
                    $('#lbl2Ditujukan3').addClass('unChk');
                }

                if (tujuanDisposisi.toUpperCase().search("72928006-34F6-46D7-B668-4CA588DF714F") > -1) {
                    $("#fieldlistDitujukan4").prop('checked', true).prop("disabled", true);
                }
                else {
                    $('#lbl1Ditujukan4').addClass('unChk');
                    $('#lbl2Ditujukan4').addClass('unChk');
                }

                if (tujuanDisposisi.toUpperCase().search("78D31B7B-DA29-4A49-B77B-861EE5458D6B") > -1) {
                    $("#fieldlistDitujukan5").prop('checked', true).prop("disabled", true);
                }
                else {
                    $('#lbl1Ditujukan5').addClass('unChk');
                    $('#lbl2Ditujukan5').addClass('unChk');
                }

                if (tujuanDisposisi.toUpperCase().search("A492B23A-E11F-4650-BC19-A48713AFEFB6") > -1) {
                    $("#fieldlistDitujukan6").prop('checked', true).prop("disabled", true);
                }
                else {
                    $('#lbl1Ditujukan6').addClass('unChk');
                    $('#lbl2Ditujukan6').addClass('unChk');
                }

                if (tujuanDisposisi.toUpperCase().search("977EF1BF-D05F-4680-9080-58093FBE7D95") > -1) {
                    $("#fieldlistDitujukan7").prop('checked', true).prop("disabled", true);
                }
                else {
                    $('#lbl1Ditujukan7').addClass('unChk');
                    $('#lbl2Ditujukan7').addClass('unChk');
                }

                if (tujuanDisposisi.toUpperCase().search("30D4C2DB-8BB4-4EE6-884D-6FFBEF181D11") > -1) {
                    $("#fieldlistDitujukan8").prop('checked', true).prop("disabled", true);
                }
                else {
                    $('#lbl1Ditujukan8').addClass('unChk');
                    $('#lbl2Ditujukan8').addClass('unChk');
                }

                if (tujuanDisposisi.toUpperCase().search("BBEEFC43-26E5-4F66-9DE3-D523A5462855") > -1) {
                    $("#fieldlistDitujukan9").prop('checked', true).prop("disabled", true);
                }
                else {
                    $('#lbl1Ditujukan9').addClass('unChk');
                    $('#lbl2Ditujukan9').addClass('unChk');
                }

                if (tujuanDisposisi.toUpperCase().search("0AAFC5BE-96ED-4213-AEB4-6B8808404878") > -1) {
                    $("#fieldlistDitujukan10").prop('checked', true).prop("disabled", true);
                }
                else {
                    $('#lbl1Ditujukan10').addClass('unChk');
                    $('#lbl2Ditujukan10').addClass('unChk');
                }

                if (tujuanDisposisi.toUpperCase().search("F0D332DB-817A-479B-A229-FF31BBBB3872") > -1) {
                    $("#fieldlistDitujukan11").prop('checked', true).prop("disabled", true);
                }
                else {
                    $('#lbl1Ditujukan11').addClass('unChk');
                    $('#lbl2Ditujukan11').addClass('unChk');
                }

                if (tujuanDisposisi.toUpperCase().search("2FD211DD-610D-4EB7-B209-F4D1ED24D1CB") > -1) {
                    $("#fieldlistDitujukan12").prop('checked', true).prop("disabled", true);
                }
                else {
                    $('#lbl1Ditujukan12').addClass('unChk');
                    $('#lbl2Ditujukan12').addClass('unChk');
                }

                if (tujuanDisposisi.toUpperCase().search("A17E11A6-81AB-4EAB-BD3F-936833EFA40B") > -1) {
                    $("#fieldlistDitujukan13").prop('checked', true).prop("disabled", true);
                }
                else {
                    $('#lbl1Ditujukan13').addClass('unChk');
                    $('#lbl2Ditujukan13').addClass('unChk');
                }

                if (tujuanDisposisi.toUpperCase().search("FD73DF57-2F13-4A4D-980F-BEB92EBED1FC") > -1) {
                    $("#fieldlistDitujukan14").prop('checked', true).prop("disabled", true);
                }
                else {
                    $('#lbl1Ditujukan14').addClass('unChk');
                    $('#lbl2Ditujukan14').addClass('unChk');
                }

                if (tujuanDisposisi.toUpperCase().search("E89C016D-D5F2-460F-8E88-225695B257B7") > -1) {
                    $("#fieldlistDitujukan15").prop('checked', true).prop("disabled", true);
                }
                else {
                    $('#lbl1Ditujukan15').addClass('unChk');
                    $('#lbl2Ditujukan15').addClass('unChk');
                }

                if (tujuanDisposisi.toUpperCase().search("DD9B064B-6A3C-43EE-A3DC-8CF8E100A4BF") > -1) {
                    $("#fieldlistDitujukan16").prop('checked', true).prop("disabled", true);
                }
                else {
                    $('#lbl1Ditujukan16').addClass('unChk');
                    $('#lbl2Ditujukan16').addClass('unChk');
                }

                if (tujuanDisposisi.toUpperCase().search("147031BD-D0E2-4730-87B8-F12E2E1EE1FC") > -1) {
                    $("#fieldlistDitujukan17").prop('checked', true).prop("disabled", true);
                }
                else {
                    $('#lbl1Ditujukan17').addClass('unChk');
                    $('#lbl2Ditujukan17').addClass('unChk');
                }

                if (tujuanDisposisi.toUpperCase().search("329AAF85-2087-4F41-B4F6-304C70A178C0") > -1) {
                    $("#fieldlistDitujukan18").prop('checked', true).prop("disabled", true);
                }
                else {
                    $('#lbl1Ditujukan18').addClass('unChk');
                    $('#lbl2Ditujukan18').addClass('unChk');
                }

                if (tujuanDisposisi.toUpperCase().search("60A6881F-B09B-440D-8742-778D83A455C1") > -1) {
                    $("#fieldlistDitujukan19").prop('checked', true).prop("disabled", true);
                }
                else {
                    $('#lbl1Ditujukan19').addClass('unChk');
                    $('#lbl2Ditujukan19').addClass('unChk');
                }

                if (tujuanDisposisi.toUpperCase().search("E8B60A6F-C658-44A4-9486-DFEF5EADE092") > -1) {
                    $("#fieldlistDitujukan20").prop('checked', true).prop("disabled", true);
                }
                else {
                    $('#lbl1Ditujukan20').addClass('unChk');
                    $('#lbl2Ditujukan20').addClass('unChk');
                }

                if (tujuanDisposisi.toUpperCase().search("E7F2EB45-A6FF-4FBD-9006-5FB34F7AFDC1") > -1) {
                    $("#fieldlistDitujukan21").prop('checked', true).prop("disabled", true);
                }
                else {
                    $('#lbl1Ditujukan21').addClass('unChk');
                    $('#lbl2Ditujukan21').addClass('unChk');
                }

                if (tujuanDisposisi.toUpperCase().search("C4F3C41D-AAE2-44BD-8C8A-A22DFD50BA66") > -1) {
                    $("#fieldlistDitujukan22").prop('checked', true).prop("disabled", true);
                }
                else {
                    $('#lbl1Ditujukan22').addClass('unChk');
                    $('#lbl2Ditujukan22').addClass('unChk');
                }


                for (var i = 1; i < 15; i++) {
                    $("#fieldlistDisposisi" + i.toString()).prop('checked', false).prop("disabled", true);
                }
                arrDisposisi = dta[0][18].split(",");
                for (var i = 1; i < 15; i++) {
                    if (jQuery.inArray($('#lblDisposisi' + i.toString()).text(), arrDisposisi) > -1) {
                        $("#fieldlistDisposisi" + i.toString()).prop('checked', true).prop("disabled", true);
                    }
                    else {
                        $('#lblDisposisi' + i.toString()).addClass("unChk");
                    }
                }
                $("#penjelasan").val(dta[0][19]).prop("disabled", true);
                $('#tgldisposisi').data('kendoDateTimePicker').value(dta[0][20]);
            }
        }
    });
}

function ambilSuratMasuk(thn, nmr) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            //scriptSQL: "SET DATEFORMAT DMY; SELECT TOP 1 [thn_agenda],[no_agenda],[tgl_terima],[penerima],[tgl_surat],[no_surat],[kel_pengirim],[pengirim],[kel_perihal],[perihal],[img1],[imgdisposisi] FROM [Sekper].[dbo].[agenda_surat_masuk] WHERE thn_agenda=" + thn + " and no_agenda=" + nmr,
            scriptSQL: "SET DATEFORMAT DMY; SELECT TOP 1 [thn_agenda],[no_agenda],[tgl_terima],[penerima],[tgl_surat],[no_surat],[kel_pengirim],[pengirim],[kel_perihal],[perihal],[img1],[imgdisposisi], " +
            " B.SuratDinasMasukId, B.StatusSuratDinas, B.TujuanId_SuratDinasMasuk,B.CCTujuanId_SuratDinasMasuk, C.Sifat_SuratDinasMasuk, C.TujuanDisposisi_SuratDinasMasuk, C.IsiDisposisi_SuratDinasMasuk, C.PenjelasanDisposisi_SuratDinasMasuk,C.TanggalDidisposisiOlehDireksi FROM [Sekper].[dbo].[agenda_surat_masuk] A " +
            " JOIN [SPDK_KanpusEF].[dbo].[eOffice_SuratDinasMasuk] B ON B.TahunAgenda=A.thn_agenda AND B.NoAgenda=A.no_agenda " +
            " JOIN [SPDK_KanpusEF].[dbo].[eOffice_DisposisiSuratDinasMasuk] C ON C.SuratDinasMasukId=B.SuratDinasMasukId AND C.DireksiId=B.TujuanId_SuratDinasMasuk" +
            " WHERE thn_agenda=" + thn + " and no_agenda=" + nmr + " AND (SELECT count(*) FROM [SPDK_KanpusEF].[dbo].[eOffice_SuratDinasMasuk] WHERE TahunAgenda = " + thn + " and NoAgenda = " + nmr + ")>= 1",


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
        var unitid = dataItem.get("");
        var r = row.find(".k-grid-BuatSuratPengantar");

        if (dataItem.get("StatusSuratDinas") == "SUDIN-DIREKSI") {
            r.text('Belum Disposisi Direksi');
            r.addClass('disabledbutton');
            r.attr("style", "color: white; background-color: red;")
        }
        else if (dataItem.get("StatusSuratDinas") == "SUDIN-KIRIM") {
            r.text('Sudah Terkirim ke Divisi/Unit');
            r.addClass('disabledbutton');
            r.attr("style", "color: white; background-color: red;")
        }
        else {
            row.find(".k-grid-BuatSuratPengantar").removeClass('disabledbutton');
        }
    }
}

function btnBatalDisposisiOnClick(e) {
    e.preventDefault();
    wndDisposisiSudin.close();
}


function openWindowBuktiSelesai(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    var Isi = data.BuktiSelesai_MemoDinas;
    var unitId = '';
    dataROW = data;
    wndBuktiSelesai.open().center();

    $('#_Lampiran_BuktiSelesai').text(data.BuktiSelesai_MemoDinas);

    var listBuktiSelesai_MemoDinas = data.BuktiSelesai_MemoDinas;
    if (listBuktiSelesai_MemoDinas == null || listBuktiSelesai_MemoDinas == '') {
        listBuktiSelesai_MemoDinas = [];
    }
    else {
        listBuktiSelesai_MemoDinas = listBuktiSelesai_MemoDinas.split(',');
    }
    $("#daftarFileBuktiSelesai").html("")
    for (var i = 0; i < listBuktiSelesai_MemoDinas.length; i++) {
        unitId = listBuktiSelesai_MemoDinas[i].substr(0, data.UnitId.toString().length)
        listBuktiSelesai_MemoDinas[i] = listBuktiSelesai_MemoDinas[i].substr(data.UnitId.toString().length + 1, 100);
        renderNamaFileBukti(listBuktiSelesai_MemoDinas[i], unitId);
    }

    $("#OkBuktiSelesai").click(function () {
        wndBuktiSelesai.close();
    });
}

function renderNamaFileBukti(nmFile, unitId) {
    var extension = "";
    var i = nmFile.lastIndexOf('.');
    if (i > 0)
        extension = nmFile.substring(i + 1).toLowerCase().substring(0, 3);
    else
        extension = "";
    var namaFile = unitId + "_" + nmFile;
    $("<p><div class='daftarfile'><table><tr><td><img src='/Content/Images/FileTypeIcon/png/" + extension + ".png'/></td></tr><tr><td>[" + nmFile + "]</td></tr><tr><td><button type='button' onClick='btnViewBuktiMemoOnClick(\"" + namaFile + "\")'><span class='k-font-icon k-i-pencil'></span>Lihat</button></td></tr></table></div></p>").appendTo($("#daftarFileBuktiSelesai"));
}


function btnViewBuktiMemoOnClick(nmFile) {
    //var file = $('#UnitId').val() + '_' + nmFile;
    var newwindow = window.open("/BuktiSelesaiMemo/" + nmFile, "window2", "");
}

function grdOnDataBoundTL(e) {
    var rows = e.sender.tbody.children();

    for (var j = 0; j < rows.length; j++) {
        var row = $(rows[j]);
        var dataItem = e.sender.dataItem(row);
        var unitid = dataItem.get("");

        if (dataItem.get("StatusTindakLanjutMemoDinas") == "SELESAI") {
            row.addClass("sudahSelesai");
        }
        else {
            row.addClass("belumSelesai");
        }
    }
}

function ubahStatusNotif(nmuser, id) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "exec [SPDK_KanpusEF]..[UbahNotifJadiTerbaca] '" + nmuser + "','" + menuId + "','" + id + "'",
            //"update [SPDK_KanpusEF].[dbo].[eOffice_NotifikasiSudinModin] set StatusNotifikasi='TERBACA' where NamaUser='" + nmuser + "' and MenuId='" + menuId + "'",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });

}