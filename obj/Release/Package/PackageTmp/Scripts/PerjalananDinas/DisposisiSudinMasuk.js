var terimaSuratDinasId, err;
var detailBaru = false;
var userName;
var menuId = 'FADEF16D-5826-40F1-A902-DB4A8B5752A0';
var dataROW;

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

//$(function () {
//    hubSuratDinas = $.connection.suratDinasHub;
//    $.connection.hub.start().done(function () {
//        // sudah terhubung ke hub

//    });

//    hubSuratDinas.client.disposisiSuratDinas = function (data) {
//        var grd = $("#grdDetail").data('kendoGrid');
//        grd.dataSource.read();
//    };
//});

$(document).ready(function () {
    err = $("#errWindow").kendoWindow({
        title: "Error Data",
        modal: true,
        visible: false,
        resizable: false,
        width: 300
    }).data("kendoWindow");

    wndDisposisiSudin = $("#wDisposisiSuratDinas").kendoWindow({
        title: "Disposisi Surat Dinas",
        modal: true,
        visible: false,
        resizable: true
    }).data("kendoWindow");

    wndIsiSudin = $("#wIsiSuratDinas").kendoWindow({
        title: "Isi Surat Dinas",
        modal: true,
        visible: false,
        resizable: true,
        width: 600,
        height: 600
    }).data("kendoWindow");

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

function getUserName() {
    var url = '/Input/GetUserName';
    return $.ajax({
        url: url,
        data: {},
        type: 'GET',
        datatype: 'json'
    })
}

function grdOnDataBound(e) {
    $("#grdDetail").find(".k-icon.k-i-collapse").trigger("click");
}

function openErrWindow(e) {
    err.open().center();
    $("#ok").click(function () {
        err.close();
    });
}

function filterdetailRead(e) {
    return { _menuId: menuId };
}

function openWindowDisposisiSudin(e) {
    e.preventDefault();

    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var __data = grid.dataItem(row);
    dataROW = __data;
    var _X = dataROW.CCTujuanId_SuratDinasMasuk.split(',');
    var _Y = dataROW.TujuanId_SuratDinasMasuk.split(',');
    _X.push(_Y[0]);

    wndDisposisiSudin.open().center();

    $('#_SifatSurat').data('kendoDropDownList').value(dataROW.Sifat_SuratDinasMasuk)

    // DIRUT
    if (_X.join().toUpperCase().search('C6DFDB6D-056E-45CC-9D07-A2207FF5FCC3') > -1) {
        $("#fieldlistDitujukanA").prop('checked', true).prop("disabled", true);
    }
    // DIROP
    if (_X.join().toUpperCase().search('BDD778FE-BE35-42D7-A891-4B2BE7126CC7') > -1) {
        $("#fieldlistDitujukanB").prop('checked', true);
    }
    // DIRMA
    if (_X.join().toUpperCase().search('D89C3607-5739-4620-80AD-6259A5216907') > -1) {
        $("#fieldlistDitujukanC").prop('checked', true);
    }
    // DIRKOM
    if (_X.join().toUpperCase().search('24BDBB95-0CD5-463E-9ABC-A4B03AB70A1A') > -1) {
        $("#fieldlistDitujukanD").prop('checked', true);
    }

    $("#fieldlistDitujukanA").prop("disabled", true);
    $("#fieldlistDitujukanB").prop('disabled', true);
    $("#fieldlistDitujukanC").prop('disabled', true);
    $("#fieldlistDitujukanD").prop('disabled', true);

    var tujuanDisposisi = dataROW.TujuanDisposisi_SuratDinasMasuk;
    if (tujuanDisposisi.toUpperCase().search("C7A94AC2-6148-400D-9235-A5DE1837A55F") > -1) {
        $("#fieldlistDitujukan1").prop('checked', true);
    }
    if (tujuanDisposisi.toUpperCase().search("580A7BA1-31E5-4337-ABB2-24A07F5E9778") > -1) {
        $("#fieldlistDitujukan2").prop('checked', true);
    }
    if (tujuanDisposisi.toUpperCase().search("8FC50B2C-2A83-42CD-9C3C-0A14B6C9CD5A") > -1) {
        $("#fieldlistDitujukan3").prop('checked', true);
    }
    if (tujuanDisposisi.toUpperCase().search("72928006-34F6-46D7-B668-4CA588DF714F") > -1) {
        $("#fieldlistDitujukan4").prop('checked', true);
    }
    if (tujuanDisposisi.toUpperCase().search("78D31B7B-DA29-4A49-B77B-861EE5458D6B") > -1) {
        $("#fieldlistDitujukan5").prop('checked', true);
    }
    if (tujuanDisposisi.toUpperCase().search("A492B23A-E11F-4650-BC19-A48713AFEFB6") > -1) {
        $("#fieldlistDitujukan6").prop('checked', true);
    }
    if (tujuanDisposisi.toUpperCase().search("977EF1BF-D05F-4680-9080-58093FBE7D95") > -1) {
        $("#fieldlistDitujukan7").prop('checked', true);
    }
    if (tujuanDisposisi.toUpperCase().search("30D4C2DB-8BB4-4EE6-884D-6FFBEF181D11") > -1) {
        $("#fieldlistDitujukan8").prop('checked', true);
    }
    if (tujuanDisposisi.toUpperCase().search("BBEEFC43-26E5-4F66-9DE3-D523A5462855") > -1) {
        $("#fieldlistDitujukan9").prop('checked', true);
    }
    if (tujuanDisposisi.toUpperCase().search("0AAFC5BE-96ED-4213-AEB4-6B8808404878") > -1) {
        $("#fieldlistDitujukan10").prop('checked', true);
    }
    if (tujuanDisposisi.toUpperCase().search("F0D332DB-817A-479B-A229-FF31BBBB3872") > -1) {
        $("#fieldlistDitujukan11").prop('checked', true);
    }
    if (tujuanDisposisi.toUpperCase().search("2FD211DD-610D-4EB7-B209-F4D1ED24D1CB") > -1) {
        $("#fieldlistDitujukan12").prop('checked', true);
    }
    if (tujuanDisposisi.toUpperCase().search("A17E11A6-81AB-4EAB-BD3F-936833EFA40B") > -1) {
        $("#fieldlistDitujukan13").prop('checked', true);
    }
    if (tujuanDisposisi.toUpperCase().search("FD73DF57-2F13-4A4D-980F-BEB92EBED1FC") > -1) {
        $("#fieldlistDitujukan14").prop('checked', true);
    }
    if (tujuanDisposisi.toUpperCase().search("E89C016D-D5F2-460F-8E88-225695B257B7") > -1) {
        $("#fieldlistDitujukan15").prop('checked', true);
    }
    if (tujuanDisposisi.toUpperCase().search("DD9B064B-6A3C-43EE-A3DC-8CF8E100A4BF") > -1) {
        $("#fieldlistDitujukan16").prop('checked', true);
    }
    if (tujuanDisposisi.toUpperCase().search("147031BD-D0E2-4730-87B8-F12E2E1EE1FC") > -1) {
        $("#fieldlistDitujukan17").prop('checked', true);
    }
    if (tujuanDisposisi.toUpperCase().search("329AAF85-2087-4F41-B4F6-304C70A178C0") > -1) {
        $("#fieldlistDitujukan18").prop('checked', true);
    }
    if (tujuanDisposisi.toUpperCase().search("60A6881F-B09B-440D-8742-778D83A455C1") > -1) {
        $("#fieldlistDitujukan19").prop('checked', true);
    }
    if (tujuanDisposisi.toUpperCase().search("E8B60A6F-C658-44A4-9486-DFEF5EADE092") > -1) {
        $("#fieldlistDitujukan20").prop('checked', true);
    }
    if (tujuanDisposisi.toUpperCase().search("E7F2EB45-A6FF-4FBD-9006-5FB34F7AFDC1") > -1) {
        $("#fieldlistDitujukan21").prop('checked', true);
    }
    if (tujuanDisposisi.toUpperCase().search("C4F3C41D-AAE2-44BD-8C8A-A22DFD50BA66") > -1) {
        $("#fieldlistDitujukan22").prop('checked', true);
    }
    if (tujuanDisposisi.toUpperCase().search("8D99BFAC-B86B-4E7F-826E-EEC778AD2C68") > -1) {
        $("#fieldlistDitujukan23").prop('checked', true);
    }

    //var arrDitujukan = [];
    //var arrarrDisposisi = [];
    //for (var i = 22; i > 0; i--) {
    //    if (typeof ($("#fieldlistDitujukan" + i.toString()).val()) != "undefined") {
    //        $("#fieldlistDitujukan" + i.toString()).prop('checked', false);
    //    }
    //}

    //arrDitujukan = __data.TujuanDisposisi_SuratDinasMasuk.split(",");
    //for (var i = 20; i > 0; i--) {
    //    if (typeof ($("#fieldlistDitujukan" + i.toString()).val()) != "undefined") {
    //        if (jQuery.inArray($($("#fieldlistDitujukan" + i.toString()).prop('labels')).text(), arrDitujukan) > -1) {
    //            $("#fieldlistDitujukan" + i.toString()).prop('checked', true);
    //        }
    //        else {
    //        }
    //    }
    //}

    for (var i = 1; i <= 15; i++) {
        $("#fieldlistDisposisi" + i.toString()).prop('checked', false)
    }
    arrDisposisi = __data.IsiDisposisi_SuratDinasMasuk.split(",");
    for (var i = 1; i <= 15; i++) {
        if (jQuery.inArray($($("#fieldlistDisposisi" + i.toString()).prop('labels')).text(), arrDisposisi) > -1) {
            $("#fieldlistDisposisi" + i.toString()).prop('checked', true)
        }
        else {
        }
    }
    $("#penjelasan").val(__data.PenjelasanDisposisi_SuratDinasMasuk);
    $('#tgldisposisi').data('kendoDatePicker').value(__data.TanggalDidisposisiOlehDireksi);

}
    
function openWindowIsiSudin(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    dataROW = data;
    readSuratMasuk(data.TahunAgenda, data.NoAgenda);
    wndIsiSudin.open().center();

    var _X = dataROW.CCTujuanId_SuratDinasMasuk.split(',');
    var _Y = dataROW.TujuanId_SuratDinasMasuk.split(',');
    _X.push(_Y[0]);

    $('#_SifatSurat').data('kendoDropDownList').value(dataROW.Sifat_SuratDinasMasuk)

    // DIRUT
    if (_X.join().toUpperCase().search('C6DFDB6D-056E-45CC-9D07-A2207FF5FCC3') > -1)
        $("#fieldlistDitujukanA").prop('checked', true);
    else
        $("#fieldlistDitujukanA").prop('checked', false);

    // DIROP
    if (_X.join().toUpperCase().search('BDD778FE-BE35-42D7-A891-4B2BE7126CC7') > -1)
        $("#fieldlistDitujukanB").prop('checked', true);
    else
        $("#fieldlistDitujukanB").prop('checked', false);

    // DIRMA
    if (_X.join().toUpperCase().search('D89C3607-5739-4620-80AD-6259A5216907') > -1)
        $("#fieldlistDitujukanC").prop('checked', true);
    else
        $("#fieldlistDitujukanC").prop('checked', false);

    // DIRKOM
    if (_X.join().toUpperCase().search('24BDBB95-0CD5-463E-9ABC-A4B03AB70A1A') > -1)
        $("#fieldlistDitujukanD").prop('checked', true);
    else
        $("#fieldlistDitujukanD").prop('checked', false);

    $("#fieldlistDitujukanA").prop("disabled", false);
    $("#fieldlistDitujukanB").prop('disabled', false);
    $("#fieldlistDitujukanC").prop('disabled', false);
    $("#fieldlistDitujukanD").prop('disabled', false);

    if ($('#UnitId').val().toUpperCase() == 'C6DFDB6D-056E-45CC-9D07-A2207FF5FCC3') //dirut
    {
        $("#fieldlistDitujukanA").prop("disabled", true);
        $("#fieldlistDitujukanA").prop("checked", false);
    }
    if ($('#UnitId').val().toUpperCase()  == 'BDD778FE-BE35-42D7-A891-4B2BE7126CC7') //dirop
    {
        $("#fieldlistDitujukanB").prop("disabled", true);
        $("#fieldlistDitujukanB").prop("checked", false);
    }

    if ($('#UnitId').val().toUpperCase()  == 'D89C3607-5739-4620-80AD-6259A5216907') //dirma
    {
        $("#fieldlistDitujukanC").prop("disabled", true);
        $("#fieldlistDitujukanC").prop("checked", false);
    }

    if ($('#UnitId').val().toUpperCase() == '24BDBB95-0CD5-463E-9ABC-A4B03AB70A1A') //dirkom
    {
        $("#fieldlistDitujukanD").prop("disabled", true);
        $("#fieldlistDitujukanD").prop("checked", false);
    }



    var tujuanDisposisi = dataROW.TujuanDisposisi_SuratDinasMasuk;
    if (tujuanDisposisi.toUpperCase().search("C7A94AC2-6148-400D-9235-A5DE1837A55F") > -1) 
        $("#fieldlistDitujukan1").prop('checked', true);
    else
        $("#fieldlistDitujukan1").prop('checked', false);
    if (tujuanDisposisi.toUpperCase().search("580A7BA1-31E5-4337-ABB2-24A07F5E9778") > -1) 
        $("#fieldlistDitujukan2").prop('checked', true);
    else
        $("#fieldlistDitujukan2").prop('checked', false);
    if (tujuanDisposisi.toUpperCase().search("8FC50B2C-2A83-42CD-9C3C-0A14B6C9CD5A") > -1) 
        $("#fieldlistDitujukan3").prop('checked', true);
    else
        $("#fieldlistDitujukan3").prop('checked', false);
    if (tujuanDisposisi.toUpperCase().search("72928006-34F6-46D7-B668-4CA588DF714F") > -1) 
        $("#fieldlistDitujukan4").prop('checked', true);
    else
        $("#fieldlistDitujukan4").prop('checked', false);
    if (tujuanDisposisi.toUpperCase().search("78D31B7B-DA29-4A49-B77B-861EE5458D6B") > -1) 
        $("#fieldlistDitujukan5").prop('checked', true);
    else
        $("#fieldlistDitujukan5").prop('checked', false);
    
    if (tujuanDisposisi.toUpperCase().search("A492B23A-E11F-4650-BC19-A48713AFEFB6") > -1) 
        $("#fieldlistDitujukan6").prop('checked', true);
    else
        $("#fieldlistDitujukan6").prop('checked', false);
    
    if (tujuanDisposisi.toUpperCase().search("977EF1BF-D05F-4680-9080-58093FBE7D95") > -1) 
        $("#fieldlistDitujukan7").prop('checked', true);
    else
        $("#fieldlistDitujukan7").prop('checked', false);

    if (tujuanDisposisi.toUpperCase().search("30D4C2DB-8BB4-4EE6-884D-6FFBEF181D11") > -1) 
        $("#fieldlistDitujukan8").prop('checked', true);
    else
        $("#fieldlistDitujukan8").prop('checked', false);

    if (tujuanDisposisi.toUpperCase().search("BBEEFC43-26E5-4F66-9DE3-D523A5462855") > -1) 
        $("#fieldlistDitujukan9").prop('checked', true);
    else
        $("#fieldlistDitujukan9").prop('checked', false);

    if (tujuanDisposisi.toUpperCase().search("0AAFC5BE-96ED-4213-AEB4-6B8808404878") > -1) 
        $("#fieldlistDitujukan10").prop('checked', true);
    else
        $("#fieldlistDitujukan10").prop('checked', false);

    if (tujuanDisposisi.toUpperCase().search("F0D332DB-817A-479B-A229-FF31BBBB3872") > -1) 
        $("#fieldlistDitujukan11").prop('checked', true);
    else
        $("#fieldlistDitujukan11").prop('checked', false);

    if (tujuanDisposisi.toUpperCase().search("2FD211DD-610D-4EB7-B209-F4D1ED24D1CB") > -1) 
        $("#fieldlistDitujukan12").prop('checked', true);
    else
        $("#fieldlistDitujukan12").prop('checked', false);

    if (tujuanDisposisi.toUpperCase().search("A17E11A6-81AB-4EAB-BD3F-936833EFA40B") > -1) 
        $("#fieldlistDitujukan13").prop('checked', true);
    else
        $("#fieldlistDitujukan13").prop('checked', false);

    if (tujuanDisposisi.toUpperCase().search("FD73DF57-2F13-4A4D-980F-BEB92EBED1FC") > -1) 
        $("#fieldlistDitujukan14").prop('checked', true);
    else
        $("#fieldlistDitujukan14").prop('checked', false);
    
    if (tujuanDisposisi.toUpperCase().search("E89C016D-D5F2-460F-8E88-225695B257B7") > -1) 
        $("#fieldlistDitujukan15").prop('checked', true);
    else
        $("#fieldlistDitujukan15").prop('checked', false);

    if (tujuanDisposisi.toUpperCase().search("DD9B064B-6A3C-43EE-A3DC-8CF8E100A4BF") > -1) 
        $("#fieldlistDitujukan16").prop('checked', true);
    else
        $("#fieldlistDitujukan16").prop('checked', false);

    if (tujuanDisposisi.toUpperCase().search("147031BD-D0E2-4730-87B8-F12E2E1EE1FC") > -1) 
        $("#fieldlistDitujukan17").prop('checked', true);
    else
        $("#fieldlistDitujukan17").prop('checked', false);

    if (tujuanDisposisi.toUpperCase().search("329AAF85-2087-4F41-B4F6-304C70A178C0") > -1) 
        $("#fieldlistDitujukan18").prop('checked', true);
    else
        $("#fieldlistDitujukan18").prop('checked', false);

    if (tujuanDisposisi.toUpperCase().search("60A6881F-B09B-440D-8742-778D83A455C1") > -1) 
        $("#fieldlistDitujukan19").prop('checked', true);
    else
        $("#fieldlistDitujukan19").prop('checked', false);

    if (tujuanDisposisi.toUpperCase().search("E8B60A6F-C658-44A4-9486-DFEF5EADE092") > -1) 
        $("#fieldlistDitujukan20").prop('checked', true);
    else
        $("#fieldlistDitujukan20").prop('checked', false);

    if (tujuanDisposisi.toUpperCase().search("E7F2EB45-A6FF-4FBD-9006-5FB34F7AFDC1") > -1) 
        $("#fieldlistDitujukan21").prop('checked', true);
    else
        $("#fieldlistDitujukan21").prop('checked', false);

    if (tujuanDisposisi.toUpperCase().search("C4F3C41D-AAE2-44BD-8C8A-A22DFD50BA66") > -1) 
        $("#fieldlistDitujukan22").prop('checked', true);
    else
        $("#fieldlistDitujukan22").prop('checked', false);

    if (tujuanDisposisi.toUpperCase().search("8D99BFAC-B86B-4E7F-826E-EEC778AD2C68") > -1) 
        $("#fieldlistDitujukan23").prop('checked', true);
    else
        $("#fieldlistDitujukan23").prop('checked', false);

    for (var i = 1; i <= 15; i++) {
        $("#fieldlistDisposisi" + i.toString()).prop('checked', false)
    }
    arrDisposisi = data.IsiDisposisi_SuratDinasMasuk.split(",");
    for (var i = 1; i <= 15; i++) {
        if (jQuery.inArray($($("#fieldlistDisposisi" + i.toString()).prop('labels')).text(), arrDisposisi) > -1) {
            $("#fieldlistDisposisi" + i.toString()).prop('checked', true)
        }
        else {
        }
    }
    $("#penjelasan").val(dataROW.PenjelasanDisposisi_SuratDinasMasuk);
    $('#tgldisposisi').data('kendoDatePicker').value(dataROW.TanggalDidisposisiOlehDireksi);
}

function btnDisposisiOnClick(e) {
    e.preventDefault();
    var grid = $("#grdDetail").data("kendoGrid");
    var __data
    if (typeof (grid._current) == "undefined")
        __data = dataROW;
    else
        __data = grid.dataItem(grid._current.closest("tr"));

    var disposisiValidDireksi = false;
    if ($("#fieldlistDitujukanA").prop("checked") == true || $("#fieldlistDitujukanB").prop("checked") == true || $("#fieldlistDitujukanD").prop("checked") == true) {
        disposisiValidDireksi = true;
    }
    else {
        disposisiValidDireksi = false;
    }


    var arrDitujukan = [];
    for (var i = 25; i > 0; i--) {
        if (typeof ($("#fieldlistDitujukan" + i.toString()).val()) != "undefined") {
            if ($("#fieldlistDitujukan" + i.toString()).prop('checked') == true) {
                arrDitujukan.push($("#fieldlistDitujukan" + i.toString()).prop('value'));
            }
            else {
            }
        }
    }
    
    var arrDisposisi = [];
    for (var i = 1; i <= 15; i++) {
        if ($("#fieldlistDisposisi" + i.toString()).prop('checked') == true) {
            arrDisposisi.push($($("#fieldlistDisposisi" + i.toString()).prop('labels')).text());
        }
        else {
        }
    }

    if ( (disposisiValidDireksi == true && arrDisposisi.length > 0) || (arrDitujukan.length > 0 && arrDisposisi.length > 0) ) {
        simpanDisposisiSuratDinas(__data.DisposisiSuratDinasMasukId, __data.SuratDinasMasukId, $('#_SifatSurat').data('kendoDropDownList').value(), arrDitujukan.toString(), arrDisposisi.toString(), $("#penjelasan").val(),
            $("#tgldisposisi").val(), $("#namaUser").val(), $('#UnitId').val()).done(function (_data) {
                if (_data == 'false') {
                    alert("Gagal!");
                }
                else {
                    __data.TujuanDisposisi_MemoDinas = arrDitujukan.toString();
                    __data.IsiDisposisi_MemoDinas = arrDisposisi.toString();
                    __data.PenjelasanDisposisi_MemoDinas = $("#penjelasan").val();
                    __data.TanggalDisposisi_MemoDinas = $("#tgldisposisi").val();
                }
                grid.dataSource.read();
                wndIsiSudin.close();
            });
    }
    else {
        alert("Gagal Kirim, Tujuan Disposisi atau Disposisi belum dipilih ... !");
    }
}

function simpanDisposisiSuratDinas(id, sudinid, sifatsurat, tujuan, isidisposisi, penjelasan, tgldisposisi, username, unitid) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "exec [SPDK_KanpusEF]..[UbahNotifJadiTerbaca] '" + username + "','" + menuId + "','" + sudinid + "'; exec [dbo].[UbahStatusSudinDariDireksiJadiDisposisi] '" + id + "', '" + sudinid + "', '" + sifatsurat + "', '" + tujuan + "', '" + isidisposisi + "', '" + penjelasan + "', '" + tgldisposisi + "', '" + username + "', '" + unitid + "'",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function btnBatalDisposisiOnClick(e) {
    e.preventDefault();
    wndIsiSudin.close();
}

function readSuratMasuk(thn,no) {
    ambilSuratMasuk(thn,no).done(function (dta) {
        if (dta.length > 0) {
            $('#_Nomor_SuratMasuk').text(dta[0][5]);
            $('#_Pengirim_SuratMasuk').text(dta[0][7]);
            $('#_Perihal_SuratMasuk').text(dta[0][9]);
            $('#_Tanggal_SuratMasuk').text(dta[0][4]);
            $('#img1').removeAttr('src');
            $('#img2').removeAttr('src');
            $('#img3').removeAttr('src');
            $('#img4').removeAttr('src');
            $('#img5').removeAttr('src');
            d = new Date();
            if (dta[0][10]) $('#img1').attr('src', '/Content/Images/View/' + $('#namaUser').val() + '_img1.jpg?' + d.getTime());
            if (dta[0][11]) $('#img2').attr('src', '/Content/Images/View/' + $('#namaUser').val() + '_img2.jpg?' + d.getTime());
            if (dta[0][12]) $('#img3').attr('src', '/Content/Images/View/' + $('#namaUser').val() + '_img3.jpg?' + d.getTime());
            if (dta[0][13]) $('#img4').attr('src', '/Content/Images/View/' + $('#namaUser').val() + '_img4.jpg?' + d.getTime());
            if (dta[0][14]) $('#img5').attr('src', '/Content/Images/View/' + $('#namaUser').val() + '_img5.jpg?' + d.getTime());

            var listLampiranSuratDinas = dta[0][16];
            if (listLampiranSuratDinas == null || listLampiranSuratDinas == '') {
                listLampiranSuratDinas = [];
            }
            else {
                listLampiranSuratDinas = listLampiranSuratDinas.split(',');
            }

            $("#daftarFile").html("")
            for (var i = 0; i < listLampiranSuratDinas.length; i++) {
                listLampiranSuratDinas[i] = listLampiranSuratDinas[i].substr(dta[0][17].length + 1, 100);
                renderNamaFile(listLampiranSuratDinas[i]);
            }
            $('#Lampiran_SuratDinas').val(listLampiranSuratDinas.toString());
        }
    });
}

function ambilSuratMasuk(thn, nmr) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "SET DATEFORMAT DMY; SELECT TOP 1 [thn_agenda],[no_agenda],[tgl_terima],[penerima]," +
            "[tgl_surat],[no_surat],[kel_pengirim],[pengirim],[kel_perihal],[perihal],[img1]," +
            "img2,img3,img4,img5,[imgdisposisi],B.Lampiran_SuratDinas,B.UnitId FROM [Sekper].[dbo].[agenda_surat_masuk] A " +
            "JOIN SPDK_KanpusEF..eOffice_SuratDinasMasuk B ON A.thn_agenda=B.TahunAgenda and A.no_agenda=B.NoAgenda " +
            "WHERE thn_agenda=" + thn + " and no_agenda=" + nmr,
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function filterDisposisiSuratDinasMasuk(e) {
    var _scriptSQL = "SELECT A.DisposisiSuratDinasMasukId AS DisposisiSuratDinasMasukId, A.SuratDinasMasukId AS SuratDinasMasukId, " +
        "B.TujuanId_SuratDinasMasuk AS TujuanId_SuratDinasMasuk, B.CCTujuanId_SuratDinasMasuk AS CCTujuanId_SuratDinasMasuk, B.TahunAgenda as TahunAgenda, " +
        "B.NoAgenda as NoAgenda, A.Sifat_SuratDinasMasuk AS Sifat_SuratDinasMasuk, C.pengirim AS Pengirim_SuratMasuk, C.perihal AS Perihal_SuratMasuk, " +
        "C.tgl_surat AS Tanggal_SuratMasuk, A.TujuanDisposisi_SuratDinasMasuk AS TujuanDisposisi_SuratDinasMasuk, A.IsiDisposisi_SuratDinasMasuk AS " +
        "IsiDisposisi_SuratDinasMasuk, A.PenjelasanDisposisi_SuratDinasMasuk AS PenjelasanDisposisi_SuratDinasMasuk, " +
        "A.TanggalDidisposisiOlehDireksi AS TanggalDidisposisiOlehDireksi, A.PembuatDisposisi AS PembuatDisposisi " +
        "FROM [SPDK_KanpusEF].[dbo].[eOffice_DisposisiSuratDinasMasuk] A JOIN[SPDK_KanpusEF].[dbo].[eOffice_SuratDinasMasuk] B " +
        "ON A.SuratDinasMasukId = B.SuratDinasMasukId JOIN [Sekper].[dbo].[agenda_surat_masuk] C ON B.TahunAgenda = C.thn_agenda AND B.NoAgenda = " +
        "C.no_agenda WHERE A.TujuanDisposisi_SuratDinasMasuk='' AND A.DireksiId = '" + $('#UnitId').val() + "'";
    return {
        strClassView: 'Ptpn8.Models.MemoOnline.View_DisposisiSuratDinasMasuk',
        scriptSQL: _scriptSQL,
        _menuId: menuId
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


function renderNamaFile(nmFile) {
    var extension = "";
    var i = nmFile.lastIndexOf('.');
    if (i > 0)
        extension = nmFile.substring(i + 1).toLowerCase().substring(0, 3);
    else
        extension = "";
    $("<p><div class='daftarfile'><table><tr><td><img src='/Content/Images/FileTypeIcon/png/" + extension + ".png'/></td></tr><tr><td>[" + nmFile + "]</td></tr><tr><td><button type='button' onClick='btnViewOnClick(\"" + nmFile + "\")'><span class='k-font-icon k-i-justify-clear'></span>View</button></td></tr></table></div></p>").appendTo($("#daftarFile"));
}

function btnViewOnClick(nmFile) {
    var file = 'C7A94AC2-6148-400D-9235-A5DE1837A55F_' + nmFile;
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

