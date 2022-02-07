var perjanjianKerjasama_DokumenId, err;
var detailBaru = false;
var userName;
var menuId = '2D71A156-C93B-45EB-96CF-706663FAA951';

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
    var bpd = $.connection.bPDHub;
    $.connection.hub.start().done(function () {
    });

    $('#btnSelesaiSimpanDetail').click(function () {
        bpd.server.tambahData();
    });
});

$(document).ready(function () {
    err = $("#errWindow").kendoWindow({
        title: "Error Data",
        modal: true,
        visible: false,
        resizable: false,
        width: 300
    }).data("kendoWindow");
    pdf = $("#pdfWindow").kendoWindow({
        title: "PDF Viewer",
        modal: true,
        visible: false,
        resizable: false,
        width: 900,
        height: 550
    }).data("kendoWindow");

    wndHakAkses = $("#wndHakAkses").kendoWindow({
        title: "Hak Akses Untuk Unit",
        modal: true,
        visible: false,
        resizable: true,
        width: 600,
        height: 400
    }).data("kendoWindow");
});

$(window).on("beforeunload", function () {
    return "Please don't leave me!";
});

$(window).on("unload", function () {
    hapusPenggunaPortalYangAktif();
});

function onButtonClick(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");

    var data = grid.dataItem(row);
    var file = data.File;

    pdf.open().center();

    var options = {
        pdfOpenParams: {
            navpanes: 1,
            toolbar: 0,
            statusbar: 0,
            pagemode: 'none',
            pagemode: "none",
            page: 1,
            zoom: "page-width",

            enableHandToolOnLoad: true
        } //,
        //forcePDFJS: true,
        //PDFJS_URL: "/PDF.js/web/viewer.html"
    }


    PDFObject.embed("/Content/Images/Document/PerjanjianKerjasama/" + file, "#testPDFObject", options)
}

function openUnit(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    //var Isi = data.Isi_MemoDinasFinal;
    dataROW = data;

        var tujuanDisposisi = dataROW.StatusOtoritas;
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

    wndHakAkses.open().center();
}

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

function btnDisposisiOnClick(e) {
    e.preventDefault();
    var grid = $("#grdDetail").data("kendoGrid");
    var __data;

    if (typeof (grid._current) == "undefined")
        __data = dataROW;
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
                arrDitujukan.push($("#fieldlistDitujukan" + i.toString()).val());
            }
            else {
            }
        }
    }

    if ((disposisiValidDireksi == true && arrDitujukan.length > 0)) {
        simpanHakAkses(__data.PerjanjianKerjasama_DokumenId,
            arrDitujukan.toString()).done(function (_data) {

                //simpanDisposisiMemo(__data.Terima_MemoDinasId, arrDitujukan.toString(), arrDisposisi.toString(), $("#penjelasan").val(),
                //    $("#tgldisposisi").val().substring(0,10), $("#namaUser").val(), __data.UnitId, __data.Kirim_MemoDinasId).done(function (_data) {
                if (_data == 'false') {
                    alert("Gagal!");
                }
                else {
                    //__data.StatusOtoritas = arrDitujukan.toString();
                }
                grid.dataSource.read();
                wndHakAkses.close();
            });
    }
    else {
        alert("Gagal Simpan ... !");
    }

}

function simpanHakAkses(id, unitid) {

    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "exec [SPDK_KanpusEF]..[PerjanjianKerjasama_Verifikasi] '" + id + "','" + unitid + "'",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function btnBatalDisposisiOnClick(e) {
    e.preventDefault();
    wndHakAkses.close();
}


function grdOnChange(e) {
}

function grdOnEdit(e) {
    var model = e.model;
    uid = model.uid;
    this.expandRow(this.tbody.find("tr[data-uid='" + uid + "']"));
    if (model.isNew()) {
        model.id = model.uid;
        e.container.find("input[name=DokumenId]").val(model.id).change();
    }
    perjanjianKerjasama_DokumenId = model.id;

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
    var rows = e.sender.tbody.children();
    for (var j = 0; j < rows.length; j++) {
        var row = $(rows[j]);
        var dataItem = e.sender.dataItem(row);

        if (dataItem.get("Verifikasi") == 1) {
	    row.addClass("sudahDisetujui");
        }
        else {
            row.addClass("belumDisetujui");
        }
    }
}

function filterGridUpdate(e) {
    var mdl = e.models;
    var arr = [];
    for (var i = 0; i < mdl.length; i++) {
        var m = mdl[i];
        //if (m.VerKaur == true)
        //    m.UserNameKaur = userName;
        //else
        //    m.UserNameKaur = "";
        arr.push(JSON.stringify(m));
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

function ddlBentukKerjasamaOnChange(e) {
    var bentukKerjasama = $('#BentukKerjasama').val();
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(perjanjianKerjasama_DokumenId)) == "undefined" ? grid.dataSource.getByUid(perjanjianKerjasama_DokumenId) : grid.dataSource.get(perjanjianKerjasama_DokumenId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
}

function ddlStatusOtoritasOnChange(e) {
    var statusOtorisasi = $('#StatusOtorisasi').val();
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(perjanjianKerjasama_DokumenId)) == "undefined" ? grid.dataSource.getByUid(perjanjianKerjasama_DokumenId) : grid.dataSource.get(perjanjianKerjasama_DokumenId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
}
