var terima_MemoDinasId, err;
var detailBaru = false;
var userName;
var menuId = '7B3CFF23-ED11-4E45-8C82-B36853DA4B11';
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

$(function () {
    hubMemoDinas = $.connection.memoDinasHub;
    $.connection.hub.start().done(function () {
        // sudah terhubung ke hub

    });

    hubMemoDinas.client.disposisiMemoDinas = function (data) {
        var grd = $("#grdDetail").data('kendoGrid');
        grd.dataSource.read();
    };
});

$(document).ready(function () {
    err = $("#errWindow").kendoWindow({
        title: "Error Data",
        modal: true,
        visible: false,
        resizable: false,
        width: 300
    }).data("kendoWindow");

    wndDisposisiMemo = $("#wDisposisiMemo").kendoWindow({
        title: "Disposisi Memo",
        modal: true,
        visible: false,
        resizable: true,
        width: 400,
        height: 260
    }).data("kendoWindow");

    wndIsiMemo = $("#wIsiMemo").kendoWindow({
        title: "Disposisi Memo",
        modal: true,
        visible: false,
        resizable: true,
        width: 600,
        height: 600
    }).data("kendoWindow");

    wndDistribusiMemo = $("#wDistribusiMemo").kendoWindow({
        title: "Distribusi Memo",
        modal: true,
        visible: false,
        resizable: false,
        width: 300,
        height: 50
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

function grdOnChange(e) {
}

function grdOnEdit(e) {
    var model = e.model;
    uid = model.uid;
    this.expandRow(this.tbody.find("tr[data-uid='" + uid + "']"));
    if (model.isNew()) {
        model.id = model.uid;
        e.container.find("input[name=terima_MemoDinasId]").val(model.id).change();
    }
    terima_MemoDinasId = model.id;

    getUserName().done(function (data) {
        if (data) {
            userName = data;
        }
        else {
        }
    }).fail(function (x) {
        alert('Error! Hubungi Bagian TI');
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

function filterGridUpdate(e) {
    var mdl = e.models;
    var arr = [];
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
            $('#btnSelesaiSimpanDetail').click();
            grid.dataSource.read([]);
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

function openErrWindow(e) {
    err.open().center();
    $("#ok").click(function () {
        err.close();
    });
}

function _sendData(data) {
    return $.ajax({
        url: "/Input/UpdateCreateDelete",
        data: data,
        type: "POST"
    })
}
function filterdetailRead(e) {
    return { _menuId: menuId };
}

function filterdetailCreate(e) {
    return { _menuId: menuId };
}

function openWindowDisposisiMemo(e) {
    e.preventDefault();

    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var __data = grid.dataItem(row);

    dataROW = __data;

    //var grid = $("#grdDetail").data("kendoGrid");
    //var __data = grid.dataItem(grid._current.closest("tr"))

    $('#_TanggalNoPenerimaan').text(__data.TanggalTerima_MemoDinas.getDate().toString() + '/' + __data.TanggalTerima_MemoDinas.getMonth().toString() + '/' +
        __data.TanggalTerima_MemoDinas.getFullYear().toString() + ' ['+__data.NomorTerima_MemoDinas.toString()+']');
    $('#_SifatSurat').text(__data.SifatSurat_MemoDinas.toString());

    var arrDitujukan = [];
    var arrarrDisposisi = [];
    for (var i = 23; i > 0; i--) {
        if (typeof ($("#fieldlistDitujukan" + i.toString()).val()) != "undefined") {
            $("#fieldlistDitujukan" + i.toString()).prop('checked', false);
        }
    }
    arrDitujukan = __data.TujuanDisposisi_MemoDinas.split(",");
    for (var i = 23; i > 0; i--) {
        if (typeof ($("#fieldlistDitujukan" + i.toString()).val()) != "undefined") {
            if (jQuery.inArray($($("#fieldlistDitujukan" + i.toString()).prop('labels')).text(), arrDitujukan) > -1) {
                $("#fieldlistDitujukan" + i.toString()).prop('checked', true);
            }
            else {
            }
        }
    }

    for (var i = 1; i < 15; i++) {
        $("#fieldlistDisposisi" + i.toString()).prop('checked', false)
    }
    arrDisposisi = __data.IsiDisposisi_MemoDinas.split(",");
    for (var i = 1; i < 15; i++) {
        if (jQuery.inArray($($("#fieldlistDisposisi" + i.toString()).prop('labels')).text(), arrDisposisi) > -1) {
            $("#fieldlistDisposisi" + i.toString()).prop('checked',true)
        }
        else {
        }
    }
    $("#penjelasan").val(__data.PenjelasanDisposisi_MemoDinas);
    wndDisposisiMemo.open().center();
}

function simpanDisposisiMemo(id, idkirim, tgldisposisi, isidisposisi,
    ditujukan, penjelasan, unitid, username, direksiid, nomemo)
{

    var txtMemoDisposisi = "";
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "exec [SPDK_KanpusEF]..[UbahNotifJadiTerbaca] '" + username + "','" + menuId + "','" + idkirim + "';" +
                "exec [dbo].[UbahStatusMemoDariTerimaJadiBacaDireksiX] '" + id + "', '" + idkirim + "', '" + tgldisposisi + "', '" + isidisposisi + "', '" +
                ditujukan + "','" + penjelasan + "', '" + unitid + "', '" + direksiid + "','"+nomemo+"','"+username+"'",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function openWindowDistribusiMemo(e) {
    e.preventDefault();
    wndDistribusiMemo.open().center();
} 

function distribusiMemo(id) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "exec [dbo].[UbahStatusMemoDariBacaJadiDistribusi] '" + id + "'",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });

}

function openWindowIsiMemo(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    var Isi = data.Isi_MemoDinasFinal;
    dataROW = data;

    wndIsiMemo.open().center();

    stringDecode(data.Isi_MemoDinasFinal).done(function (data) {
        $('#isiMemo').data('kendoEditor').value(data);
        $('#isiMemo').data('kendoEditor').body.setAttribute('contenteditable', false);
    });

    $('#_NoMemo').text(data.Nomor_MemoDinasView);
    $('#_Kepada').text(data.NamaTujuan_MemoDinas);
    $('#_Dari').text(data.NamaPengirim);
    $('#_Perihal').text(data.Perihal_MemoDinas.toString().toUpperCase());
    $('#_Tanggal').text(data.Tanggal_MemoDinas.getDate().toString() + '/' + data.Tanggal_MemoDinas.getMonth().toString() + '/' + data.Tanggal_MemoDinas.getFullYear().toString());
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

    //var tgl = data.Tanggal_MemoDinas;
    //$('#QRMemoDinas').data('kendoQRCode').value("No Memo: " + data.Nomor_MemoDinasView + "; Kepada: " + data.NamaTujuan_MemoDinas + "; Dari: " + data.NamaUnit + ";" + "Perihal: " + data.Perihal_MemoDinas.toString().toUpperCase() + "; Tanggal: " +
    //    tgl.getDate().toString() + '/' + tgl.getMonth().toString() + '/' + tgl.getFullYear().toString());

    //if (data.StatusDistribusi == true) {
    //    $('#_TahunAgenda_SuratMasuk').text(data.TahunAgenda_SuratMasuk);
    //    $('#_NomorAgenda_SuratMasuk').text(data.NomorAgenda_SuratMasuk);
    //    readSuratMasuk();
    //    $('#yaMemoDistribusi').show();
    //}
    //else {
    //    $('#_TahunAgenda_SuratMasuk').text('');
    //    $('#_NomorAgenda_SuratMasuk').text('');
    //    $('#yaMemoDistribusi').hide();
    //}

    //$("#Ok").click(function () {
    //    wndIsiMemo.close();
    //});

    var _X = dataROW.TujuanDisposisi_MemoDinas.split(',');

    
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

    if (dataROW.UnitId.toUpperCase() == 'C6DFDB6D-056E-45CC-9D07-A2207FF5FCC3') //dirut
    {
        $("#fieldlistDitujukanA").prop("disabled", true);
        $("#fieldlistDitujukanA").prop("checked", false);

    }
    if (dataROW.UnitId.toUpperCase() == 'BDD778FE-BE35-42D7-A891-4B2BE7126CC7') //dirop
    {
        $("#fieldlistDitujukanB").prop("disabled", true);
        $("#fieldlistDitujukanB").prop("checked", false);
    }

    if (dataROW.UnitId.toUpperCase() == 'D89C3607-5739-4620-80AD-6259A5216907') //dirma
    {
        $("#fieldlistDitujukanC").prop("disabled", true);
        $("#fieldlistDitujukanC").prop("checked", false);
    }

    if (dataROW.UnitId.toUpperCase() == '24BDBB95-0CD5-463E-9ABC-A4B03AB70A1A') //dirkom
    {
        $("#fieldlistDitujukanD").prop("disabled", true);
        $("#fieldlistDitujukanD").prop("checked", false);
    }


    var tujuanDisposisi = dataROW.TujuanDisposisi_MemoDinas;
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
    arrDisposisi = dataROW.IsiDisposisi_MemoDinas.split(",");
    for (var i = 1; i <= 15; i++) {
        if (jQuery.inArray($($("#fieldlistDisposisi" + i.toString()).prop('labels')).text(), arrDisposisi) > -1) {
            $("#fieldlistDisposisi" + i.toString()).prop('checked', true)
        }
        else {
        }
    }
    $("#penjelasan").val(dataROW.PenjelasanDisposisi_MemoDinas);
    $('#tgldisposisi').data('kendoDatePicker').value(dataROW.TanggalDisposisi_MemoDinas);
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
    var grid = $("#grdDetail").data("kendoGrid");
    var __data;
    if (typeof (grid._current) == "undefined")
        __data = dataROW;
    else
        __data = grid.dataItem(grid._current.closest("tr"));

    var file = __data.PengirimId + '_' + nmFile;
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

function btnDisposisiOnClick(e) {
    e.preventDefault();
    var grid = $("#grdDetail").data("kendoGrid");
    var __data;

    if (typeof(grid._current) == "undefined")
        __data= dataROW;
    else
        __data = grid.dataItem(grid._current.closest("tr"));

    var arrDitujukan = [];
    if ($("#fieldlistDitujukanA").prop('checked') == true) arrDitujukan.push($("#fieldlistDitujukanA").val());
    if ($("#fieldlistDitujukanB").prop('checked') == true) arrDitujukan.push($("#fieldlistDitujukanB").val());
    if ($("#fieldlistDitujukanC").prop('checked') == true) arrDitujukan.push($("#fieldlistDitujukanC").val());
    if ($("#fieldlistDitujukanD").prop('checked') == true) arrDitujukan.push($("#fieldlistDitujukanD").val());

    var disposisiValidDireksi = false;
    if ($("#fieldlistDitujukanA").prop("checked") == true || $("#fieldlistDitujukanB").prop("checked") == true || $("#fieldlistDitujukanD").prop("checked") == true) {
        disposisiValidDireksi = true;
    }
    else {
        disposisiValidDireksi = false;
    }

    for (var i = 25; i > 0; i--) {
        if (typeof ($("#fieldlistDitujukan" + i.toString()).val()) != "undefined") {
            if ($("#fieldlistDitujukan" + i.toString()).prop('checked') == true) {
                arrDitujukan.push( $("#fieldlistDitujukan" + i.toString()).val());
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

    if ((disposisiValidDireksi == true && arrDisposisi.length > 0) || (arrDitujukan.length > 0 && arrDisposisi.length > 0)) {
        simpanDisposisiMemo(__data.Terima_MemoDinasId,
            __data.Kirim_MemoDinasId,
            $("#tgldisposisi").val().substring(0, 10),
            arrDisposisi.toString(),
            arrDitujukan.toString(),
            $("#penjelasan").val(),
            __data.PengirimId,
            $("#namaUser").val(),
            __data.UnitId,
            __data.Nomor_MemoDinasView).done(function (_data) {

                //simpanDisposisiMemo(__data.Terima_MemoDinasId, arrDitujukan.toString(), arrDisposisi.toString(), $("#penjelasan").val(),
                //    $("#tgldisposisi").val().substring(0,10), $("#namaUser").val(), __data.UnitId, __data.Kirim_MemoDinasId).done(function (_data) {
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
                wndIsiMemo.close();
            });
    }
    else {
        alert("Gagal Kirim, Tujuan Disposisi atau Disposisi tidak dipilih ... !");
    }

}

function btnBatalDisposisiOnClick(e) {
    e.preventDefault();
    wndIsiMemo.close();
}


function btnDistribusiOnClick(e) {
    var grid = $("#grdDetail").data("kendoGrid");
    var __data = dataROW; //grid.dataItem(grid._current.closest("tr"))
    distribusiMemo(__data.Terima_MemoDinasId).done(function (dta) {
        if (dta == 'true') {
            hubMemoDinas.server.terimaMemoDinas('7B8B79FA-04E7-40A3-A7B5-D1DE6BCA2B83');
            wndDistribusiMemo.close();
            grid.dataSource.read();
        }
        else {
        }
    });
}

function btnBatalDistribusiOnClick(e) {
    wndDistribusiMemo.close();
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
        }
    });
}

function ambilSuratMasuk(thn, nmr) {
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

function ubahStatusNotif(nmuser) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "update [SPDK_KanpusEF].[dbo].[eOffice_NotifikasiSudinModin] set StatusNotifikasi='TERBACA' where NamaUser='" + nmuser + "' and MenuId='" + menuId + "'",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });

}