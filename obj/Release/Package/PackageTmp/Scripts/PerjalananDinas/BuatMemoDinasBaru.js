var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var kirim_MemoDinasId
var menuId = '0DEDEAED-9A05-4FA0-9B21-B9C61B3E5F52';

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


    $("#radio_1, #radio_2").change(function () {
        if ($("#radio_1").is(":checked")) {
            $('#yaMemoDistribusi').show();
            $('#bukanMemoDistribusi').hide();
        }
        else if ($("#radio_2").is(":checked")) {
            $('#yaMemoDistribusi').hide();
            $('#bukanMemoDistribusi').show();
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
                simpanHeader().done(function (data) {
                    //if (headerBaru) {
                    //    tambahNomorBaru().done(function () {
                    //        InitHeader();
                    //    });
                    //}
                    //else
                    //    InitHeader();

                    if (data) {
                        InitHeader();
                        //kirim_MemoDinasId = $('#Kirim_MemoDinasId').val();
                        $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                            disableHeader();
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
                })
                .fail(function (x) {
                    alert("Error! Hubungi Bagian TI");
                });
            } else {
                $(".k-button-icontext").addClass("k-state-disabled").removeClass("k-grid-add"); // edited
            }
        }
    });

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
            _menuId: menuId,
            _SQLLanjut: "exec [SPDK_KanpusEF].[dbo].[UbahStatusMemoDariBaruJadiSiapDraft] '" + $('#Kirim_MemoDinasId').val() + "', " + $('#TahunBuku').val() +
                        ", '" + $('#SubUnitId').val() + "', '" + $('#NamaTujuan_MemoDinas').val() + "', '" + $('#Perihal_MemoDinas').val() + "'"
        },
        type: 'POST',
        datatype: 'json'
    });
}

function tambahNomorBaru() {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "exec [SPDK_KanpusEF].[dbo].[UbahStatusMemoDariBaruJadiSiapDraft] '" + $('#Kirim_MemoDinasId').val() + "', " + $('#TahunBuku').val() +
                ", '" + $('#SubUnitId').val() + "', '" + $('#NamaTujuan_MemoDinas').val() + "', '" + $('#Perihal_MemoDinas').val()+"'",
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

    if (typeof (_NomorInputView.CCTujuanId_MemoDinas) == "string") {
        if (_NomorInputView.CCTujuanId_MemoDinas == "") {
            _NomorInputView.CCTujuanId_MemoDinas = [];
        }
        else {
            _NomorInputView.CCTujuanId_MemoDinas = _NomorInputView.CCTujuanId_MemoDinas.split(",");
        }
    }
    cekData(_NomorInputView.NomorInputView);
    ModelToView(_NomorInputView);
    enableHeader();
    var CCTujuanId = $('#CCTujuanId_MemoDinas').data('kendoMultiSelect');
    CCTujuanId.dataSource.read().done(function (data) {
        CCTujuanId.value(_NomorInputView.CCTujuanId_MemoDinas);
    });

}

function cekData(nomorInputView) {
    if (nomorInputView.indexOf("Data Baru") > -1)   // Data Baru
    {
        _NomorInputView.StatusMemoDinas = 'BARU';
        execAmbilNomorBaru().done(function (dta) {
            $('#Nomor_MemoDinas').val(dta);
            $('#Nomor_MemoDinas').data('kendoNumericTextBox').focus();
            $('#StatusMemoDinas').val('BARU');
            
            $('#Nomor_MemoDinasView').focus();

            headerBaru = true;
            enableHeader();
            $('#btnSubmitHeader').removeClass('disabledbutton');
            $('#btnDeleteHeader').removeClass('disabledbutton');
            $('#btnPrintHeader').removeClass('disabledbutton');
            $('#btnSubmitHeader').attr('disabled', false);
            $('#btnDeleteHeader').attr('disabled', true);
            $('#btnPrintHeader').attr('disabled', true);
        });
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

function execAmbilNomorBaru() {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select top 1 NomorBaru_Keluar from [SPDK_KanpusEF].[dbo].[eOffice_Nomor_MemoDinas] where TahunBuku='" + $('#TahunBuku').val() + "' and UnitId='" + $('#UnitId').val() + "'",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });

}

function filterNomorInput(e) {
    var mdl = JSON.stringify(ViewToModel());

    var arrFilter = [];
    var userName = $('#UserName').val();

    arrFilter.push('pembuat_memodinas');
    arrFilter.push(userName);

    return { _model: mdl, _menuId: menuId, _filter: arrFilter };
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
    $('#TujuanId_MemoDinas').val(tujuan.OrgId);
    $('#NamaTujuan_MemoDinas').val(tujuan.NamaUnit);
    $('#NomorBalasan_MemoDinas').data('kendoAutoComplete').dataSource.read();
}

function filterTujuan(e) {
    //return {
    //    value: $("#NamaTujuan_MemoDinas").val()
    //};

    var _scriptSQL = "SELECT * FROM [ReferensiEF].[dbo].[Unit] where (NamaUnit like 'div:%' or NamaUnit like 'bod:%' or NamaUnit like 'gm:%' or NamaUnit like 'kebun:%' or NamaUnit like 'sel%') and NamaUnit like '%" + $("#NamaTujuan_MemoDinas").val() + "%'"; 
        //(IndukOrgId in ('3FC61718-BA8D-4A2B-B851-B07E10D4520A', '78A299AF-9733-412B-835D-083953E67E0D')  OR IndukOrgId in (SELECT[DireksiId] FROM [ReferensiEF].[dbo].[Direksi]) OR IndukOrgId IN (SELECT WILAYAHID FROM [ReferensiEF].[dbo].[Wilayah])) and NamaUnit not like '%Bidang%' and NamaUnit like '%" + $("#NamaTujuan_MemoDinas").val() + "%'"; 
        ////"SELECT 'DC50DF9D-39E4-4019-99C8-96D06CE83620' as UnitId, 'C8BC663E-8407-4DEC-AADA-8F02C2639F82' as OrgId, 'SELURUH' as NamaUnit, 'XXX.1' as Nomenklatur, '3FC61718-BA8D-4A2B-B851-B07E10D4520A' as IndukOrgId UNION " +
        ////"SELECT '8D6C65B6-6092-4FD5-885C-4AD33656C334' as UnitId, '7AE5024F-D3BD-45B0-ABD6-193E8B0581BD' as OrgId, 'SELURUH DIVISI' as NamaUnit, 'XXX.2' as Nomenklatur, '3FC61718-BA8D-4A2B-B851-B07E10D4520A' as IndukOrgId UNION " +
        ////"SELECT 'E27ADBFC-FA5A-44D5-8CD1-D3F14274C9A8' as UnitId, 'C6671335-3459-401F-9016-E4B7946E980F' as OrgId, 'SELURUH UNIT KOMODITI' as NamaUnit, 'XXX.3' as Nomenklatur, '3FC61718-BA8D-4A2B-B851-B07E10D4520A' as IndukOrgId UNION " +
        ////"SELECT '36219A44-1F49-4D1B-9A39-17268574AD78' as UnitId, 'B78F2F8A-EC14-4617-932B-0C4FBC5184AA' as OrgId, 'SELURUH KEBUN/UNIT' as NamaUnit, 'XXX.4' as Nomenklatur, '3FC61718-BA8D-4A2B-B851-B07E10D4520A' as IndukOrgId Order by Nomenklatur"; 

    return {
        strClassView: 'Ptpn8.Areas.Referensi.Models.Unit',
        scriptSQL: _scriptSQL,
        _menuId: menuId
    }
}

function filterTujuancc(e) {
    var ccTujuanId = $('#CCTujuanId_MemoDinas').data('kendoMultiSelect');
    var str = ccTujuanId.input.val() == "" || ccTujuanId.input.val() == "Pilih cc Tujuan" ? "XXX" : ccTujuanId.input.val();
    var listId = [];
    for (var i = 0; i < _NomorInputView.CCTujuanId_MemoDinas.length; i++) {
        listId.push("'" + _NomorInputView.CCTujuanId_MemoDinas[i].toString() + "'");
    }
    var _scriptSQL = "";

    if (headerBaru == true) {
        _scriptSQL = "select DISTINCT A.OrgId, A.NamaUnit from [ReferensiEF].[dbo].[Unit] as A " + (str != "" && str != "Pilih cc Tujuan" ? "where (NamaUnit like 'div:%' or NamaUnit like 'bod:%' or NamaUnit like 'gm:%' or NamaUnit like 'kebun:%' or NamaUnit like 'sel%') and A.NamaUnit like '%" + str + "%'" : "");
        return {
            strClassView: 'Ptpn8.Areas.Referensi.Models.Unit',
            scriptSQL: _scriptSQL,
            _menuId: menuId
        }
    }
    else {
        if(listId.length>0) 
            _scriptSQL = "select DISTINCT A.OrgId, A.NamaUnit from [ReferensiEF].[dbo].[Unit] as A " + "where OrgId in (" + listId.toString() + ")" + (str != "" && str != "Pilih cc Tujuan" ? " or ((NamaUnit like 'div:%' or NamaUnit like 'bod:%' or NamaUnit like 'gm:%' or NamaUnit like 'kebun:%' or NamaUnit like 'sel%') and A.NamaUnit like '%" + str + "%')" : "");
        else
            _scriptSQL = "select DISTINCT A.OrgId, A.NamaUnit from [ReferensiEF].[dbo].[Unit] as A " + (str != "" && str != "Pilih cc Tujuan" ? " where (NamaUnit like 'div:%' or NamaUnit like 'bod:%' or NamaUnit like 'gm:%' or NamaUnit like 'kebun:%' or NamaUnit like 'sel%') and A.NamaUnit like '%" + str + "%'" : "");

        return {
            strClassView: 'Ptpn8.Areas.Referensi.Models.Unit',
            scriptSQL: _scriptSQL,
            _menuId: menuId
        }
    }
}

function filterCCTujuan(e) {
    var listId = [];
    for (var i = 0; i < _NomorInputView.CCTujuanId_MemoDinas.length; i++) {
        listId.push("'" + _NomorInputView.CCTujuanId_MemoDinas[i].toString() + "'");
    }
    return {
        value: $("#NamaTujuan_MemoDinas").val()
    };

}

function CCTujuanId_MemoDinasOnSelect(e) {
    var ccTujuan = e.sender.dataItems();
    var arr = [];
    if (ccTujuan.length > 0)
    {
        for (var i = 0; i < ccTujuan.length; i++) {
            arr.push(ccTujuan[i].NamaUnit);
        }
        $('#CCNamaTujuan_MemoDinas').val(arr.toString());
    }
    else
    {
        $('#CCNamaTujuan_MemoDinas').val('');
    }
}

function btnUploadOnClick(e) {

}

function Nomor_MemoDinasOnChange(e) {
    nomor_MemoDinasOnChange().done(function(data){
        $('#Nomor_MemoDinasView').val(data);
    });
}

function nomor_MemoDinasOnChange(e) {
    var url = '/Input/ExecSQL';
    var unitId = $('#UnitId').val();
    var tgl = $('#Tanggal_MemoDinas').val();
    var no = $('#Nomor_MemoDinas').val();
    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            scriptSQL: 
                "set dateformat dmy; declare @unitId uniqueidentifier; declare @tanggal datetime; declare @nomemo int; set @unitId='"+unitId+"';" +
                "set @tanggal='"+tgl+"'; set @nomemo="+no+"; declare @bulanromawi varchar(100); set @bulanromawi='I   II  III IV  V   VI  VII VIIIIX  X   XI  XII ';" +
                "declare @nomenklatur varchar(100); set @nomenklatur=(select top 1 nomenklatur from [ReferensiEF].[dbo].[unit] where unit.OrgId=@unitId)" +
                "select 'M/'+@nomenklatur+'/'+cast(@nomemo as varchar(4))+'/'+rtrim(SUBSTRING(@bulanromawi,((month(@tanggal)-1)*4)+1,4))+'/'+cast(year(@tanggal) as varchar(4))",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function onSuccess(e) {
    var fileLampiran = $('#Lampiran_MemoDinas').val().split(',');
    if ($('#Lampiran_MemoDinas').val() == "") fileLampiran = [];
    fileLampiran.push(e.files[0].name)
    $('#Lampiran_MemoDinas').val(fileLampiran.toString());
    renderNamaFile(e.files[0].name);
}

function UploadSuccess(e) {
    var fileLampiran = $('#Lampiran_MemoDinas').val().split(',');
    if ($('#Lampiran_MemoDinas').val() == "") fileLampiran = [];
    fileLampiran.push(e.files[0].name)
    $('#Lampiran_MemoDinas').val(fileLampiran.toString());
}

function ModelToView(_NomorInputView) {
    $('#Kirim_MemoDinasId').val(_NomorInputView.Kirim_MemoDinasId);
    $('#TahunBuku').data('kendoNumericTextBox').value(_NomorInputView.TahunBuku);
    $('#UnitId').val(_NomorInputView.UnitId);
    $('#SubUnitId').val(_NomorInputView.SubUnitId);
    $('#NamaUnit').val(_NomorInputView.NamaUnit);
    $('#NomorInput').val(_NomorInputView.NomorInput);
    $('#NomorKonsep').val(_NomorInputView.NomorKonsep);
    $('#NomorInputView').val(_NomorInputView.NomorInputView);
    $('#Nomor_MemoDinas').data('kendoNumericTextBox').value(_NomorInputView.Nomor_MemoDinas);
    $('#Nomor_MemoDinasView').val(_NomorInputView.Nomor_MemoDinasView);
    $('#Tanggal_MemoDinas').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.Tanggal_MemoDinas.substr(6))));
    $('#Perihal_MemoDinas').val(_NomorInputView.Perihal_MemoDinas);
    $('#TujuanId_MemoDinas').val(_NomorInputView.TujuanId_MemoDinas);
    $('#NamaTujuan_MemoDinas').val(_NomorInputView.NamaTujuan_MemoDinas);
    $('#CCNamaTujuan_MemoDinas').val(_NomorInputView.CCNamaTujuan_MemoDinas);
    $('#SuratDinasMasukId').val(_NomorInputView.SuratDinasMasukId);

    if (_NomorInputView.SifatSurat_MemoDinas == "RAHASIA") {
        $('#__editor1').css("display", "none");
        $('#__editor2').css("display", "none");
        $('#__editor3').css("display", "none");
    }

    if (_NomorInputView.StatusMemoDinas == 'BARU') {
        stringDecode(_NomorInputView.Isi_MemoDinasBaru).done(function (dta) {
            $('#Isi_MemoDinasBaru').data('kendoEditor').value(dta);
            $('#Isi_MemoDinasDraft').val(_NomorInputView.Isi_MemoDinasDraft);
            $('#Isi_MemoDinasFinal').val(_NomorInputView.Isi_MemoDinasFinal);
            if (_NomorInputView.SifatSurat_MemoDinas != "RAHASIA") {
                $('#__editor1').css("display", "block");
                $('#__editor2').css("display", "none");
                $('#__editor3').css("display", "none");
            }
        });
    }
    else if (_NomorInputView.StatusMemoDinas == 'DRAFT') {
        stringDecode(_NomorInputView.Isi_MemoDinasDraft).done(function (dta) {
            $('#Isi_MemoDinasDraft').data('kendoEditor').value(dta);
            $('#Isi_MemoDinasBaru').val(_NomorInputView.Isi_MemoDinasBaru);
            $('#Isi_MemoDinasFinal').val(_NomorInputView.Isi_MemoDinasFinal);
            if (_NomorInputView.SifatSurat_MemoDinas != "RAHASIA") {
                $('#__editor1').css("display", "none");
                $('#__editor2').css("display", "block");
                $('#__editor3').css("display", "none");
            }
        });
    }
    else {
        stringDecode(_NomorInputView.Isi_MemoDinasFinal).done(function (dta) {
            $('#Isi_MemoDinasFinal').data('kendoEditor').value(dta);
            $('#Isi_MemoDinasBaru').val(_NomorInputView.Isi_MemoDinasBaru);
            $('#Isi_MemoDinasDraft').val(_NomorInputView.Isi_MemoDinasDraft);
            if (_NomorInputView.SifatSurat_MemoDinas != "RAHASIA") {
                $('#__editor1').css("display", "none");
                $('#__editor2').css("display", "none");
                $('#__editor3').css("display", "block");
            }
        });
    }

    var listLampiranMemoDinas = _NomorInputView.Lampiran_MemoDinas;
    if (listLampiranMemoDinas == null || listLampiranMemoDinas == '') {
        listLampiranMemoDinas = [];
    }
    else {
        listLampiranMemoDinas = listLampiranMemoDinas.split(',');
    }

    $("#daftarFile").html("")
    for (var i = 0; i < listLampiranMemoDinas.length; i++) {
        listLampiranMemoDinas[i] = listLampiranMemoDinas[i].substr(_NomorInputView.UnitId.toString().length + 1, 100);
        renderNamaFile(listLampiranMemoDinas[i]);
    }
    $('#Lampiran_MemoDinas').val(listLampiranMemoDinas.toString())
    $('#Pembuat_MemoDinas').val(_NomorInputView.Pembuat_MemoDinas);
    $('#TanggalDibuat_MemoDinas').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.TanggalDibuat_MemoDinas.substr(6))));
    $('#StatusMemoDinas').val(_NomorInputView.StatusMemoDinas);

    $('#PenandaTangan_MemoDinas').val(_NomorInputView.PenandaTangan_MemoDinas);
    $('#TanggalDitandatangan_MemoDinas').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.TanggalDitandatangan_MemoDinas.substr(6))));
    $('#Pemeriksa_MemoDinas').val(_NomorInputView.Pemeriksa_MemoDinas);
    $('#TanggalDiperiksa_MemoDinas').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.TanggalDiperiksa_MemoDinas.substr(6))));
    $('#SifatSurat_MemoDinas').val(_NomorInputView.SifatSurat_MemoDinas);
    $('#Balasan_MemoDinasId').val(_NomorInputView.Balasan_MemoDinasId);
    $('#NomorBalasan_MemoDinas').val(_NomorInputView.NomorBalasan_MemoDinas);

    var tgl = new Date(parseInt(_NomorInputView.Tanggal_MemoDinas.substr(6)));
    $('#QRMemoDinas').data('kendoQRCode').value("No Memo: "+_NomorInputView.Nomor_MemoDinasView + "; Kepada: "+_NomorInputView.NamaTujuan_MemoDinas + "; Dari: "+_NomorInputView.NamaUnit + ";" + "Perihal: "+_NomorInputView.Perihal_MemoDinas.toString().toUpperCase() + "; Tanggal: " +
        tgl.getDate().toString() + '/' + tgl.getMonth().toString() + '/' + tgl.getFullYear().toString());

    if (_NomorInputView.StatusDistribusi == true) {
        $('#radio_1').attr('checked', 'checked');
        $('#yaMemoDistribusi').show();
        $('#bukanMemoDistribusi').hide();
        $('#TahunAgenda_SuratMasuk').data('kendoNumericTextBox').value(_NomorInputView.TahunAgenda_SuratMasuk);
        $('#NomorAgenda_SuratMasuk').data('kendoNumericTextBox').value(_NomorInputView.NomorAgenda_SuratMasuk);
        readSuratMasuk();
    }
    else {
        $('#radio_2').attr('checked', 'checked');
        $('#yaMemoDistribusi').hide();
        $('#bukanMemoDistribusi').show();
    }
}


function ViewToModel() {
    var listId = $('#CCTujuanId_MemoDinas').val();
    if (listId == null) {
        listId = "";
    }
    else {
        listId = listId.toString();
    }

    var listTujuan = $('#CCNamaTujuan_MemoDinas').val();
    if (listTujuan == null) {
        listTujuan = "";
    }
    else {
        listTujuan = listTujuan.toString();
    }

    var listLampiranMemoDinas = $('#Lampiran_MemoDinas').val();
    if (listLampiranMemoDinas == null || listLampiranMemoDinas == '') {
        listLampiranMemoDinas = [];
    }
    else {
        listLampiranMemoDinas = listLampiranMemoDinas.split(',');
    }
    for (var i = 0; i < listLampiranMemoDinas.length; i++) {
        listLampiranMemoDinas[i] = $('#UnitId').val().toString() + '_' + listLampiranMemoDinas[i];
    }
    $('#Lampiran_MemoDinas').val(listLampiranMemoDinas.toString())

    if ($('#StatusMemoDinas').val() == 'BARU') {
        $('#Isi_MemoDinasDraft').val($('#Isi_MemoDinasBaru').val());
        $('#Isi_MemoDinasFinal').val($('#Isi_MemoDinasBaru').val());
    }

    var statusMemoDistribusi;
    if ($("#radio_1").is(":checked")) {
        statusMemoDistribusi = true;
    }
    else {
        statusMemoDistribusi = false;
    }

    var mdl = {
        Kirim_MemoDinasId: $('#Kirim_MemoDinasId').val(),
        TahunBuku: $('#TahunBuku').val(),
        UnitId: $('#UnitId').val(),
        SubUnitId: $('#SubUnitId').val(),
        NamaUnit: $('#NamaUnit').val(),
        NomorInput: $('#NomorInput').val(),
        NomorKonsep: $('#NomorKonsep').val(),
        NomorInputView: Array(5 - String($('#NomorInput').val()).length + 1).join('0') + $('#NomorInput').val() + ' - ' + $('#Nomor_MemoDinas').val().toUpperCase(),
        Nomor_MemoDinas: '1',
        Tanggal_MemoDinas: $('#Tanggal_MemoDinas').val(),
        Perihal_MemoDinas: $('#Perihal_MemoDinas').val(),
        TujuanId_MemoDinas: $('#TujuanId_MemoDinas').val(),
        NamaTujuan_MemoDinas: $('#NamaTujuan_MemoDinas').val(),
        CCTujuanId_MemoDinas: listId.toString(),
        CCNamaTujuan_MemoDinas: listTujuan.toString(),
        Lampiran_MemoDinas: $('#Lampiran_MemoDinas').val(),
        Pembuat_MemoDinas: $('#Pembuat_MemoDinas').val(),
        TanggalDibuat_MemoDinas: $('#TanggalDibuat_MemoDinas').val(),
        StatusMemoDinas: 'BARU',
        Isi_MemoDinasBaru: $('#Isi_MemoDinasBaru').val(),
        Isi_MemoDinasDraft: $('#Isi_MemoDinasDraft').val(),
        Isi_MemoDinasFinal: $('#Isi_MemoDinasFinal').val(),
        PenandaTangan_MemoDinas: $('#PenandaTangan_MemoDinas').val(),
        TanggalDitandatangan_MemoDinas: $('#TanggalDitandatangan_MemoDinas').val(),
        Pemeriksa_MemoDinas: $('#Pemeriksa_MemoDinas').val(),
        TanggalDiperiksa_MemoDinas: $('#TanggalDiperiksa_MemoDinas').val(),
        SifatSurat_MemoDinas: $('#SifatSurat_MemoDinas').val(),
        Balasan_MemoDinasId: $('#Balasan_MemoDinasId').val(),
        NomorBalasan_MemoDinas: $('#NomorBalasan_MemoDinas').val(),
        StatusDistribusi: statusMemoDistribusi,
        TahunAgenda_SuratMasuk: $('#TahunAgenda_SuratMasuk').val(),
        NomorAgenda_SuratMasuk: $('#NomorAgenda_SuratMasuk').val(),
        SuratDinasMasukId: $('#SuratDinasMasukId').val()
    };
    return mdl;
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

function btnHapusOnClick(nmFile) {
    hapusFile(nmFile).done(function (data) {
        var listLampiranMemoDinas = $('#Lampiran_MemoDinas').val();
        if (listLampiranMemoDinas == null || listLampiranMemoDinas == '') {
            listLampiranMemoDinas = [];
        }
        else {
            listLampiranMemoDinas = listLampiranMemoDinas.split(',');
        }
        
        var index = listLampiranMemoDinas.indexOf(nmFile);
        if (index !== -1) listLampiranMemoDinas.splice(index, 1);
        $('#Lampiran_MemoDinas').val(listLampiranMemoDinas.toString())
        $("#daftarFile").html("")
        for (var i = 0; i < listLampiranMemoDinas.length; i++) {
            renderNamaFile(listLampiranMemoDinas[i]);
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
    $('#Kirim_MemoDinasId').val('');
    $('#UnitId').val('');
    $('#NamaUnit').val('');
    $('#NomorInput').val('');
    $('#NomorInputView').val('');
    $('#Nomor_MemoDinas').val(1);
    $('#Nomor_MemoDinasView').val('');
    $('#Tanggal_MemoDinas').val('');
    $('#Perihal_MemoDinas').val('');
    $('#TujuanId_MemoDinas').val('');
    $('#NamaTujuan_MemoDinas').val('');

    if (typeof ($('#CCTujuanId_MemoDinas').data('kendoMultiSelect')) == 'undefined') {
        $('#CCTujuanId_MemoDinas').val('');
    }
    else {
        $('#CCTujuanId_MemoDinas').data('kendoMultiSelect').value([]);
    }


    $('#__editor1').css("display", "none");
    $('#__editor2').css("display", "none");
    $('#__editor3').css("display", "none");

    $('#Lampiran_MemoDinas').val('');
    $("#daftarFile").html("");


}

function btnViewOnClick(nmFile) {
    var file = $('#UnitId').val() + '_' + nmFile;
    var newwindow = window.open("/LampiranMemo/" + file, "window2", "");
}

function disableHeader() {
    $("._key").find('span').addClass('disabledbutton');
    $("._key").addClass('disabledbutton');
    $("._nonkey").find('span').addClass('disabledbutton');
    $("._nonkey").addClass('disabledbutton');
    $('#btnSubmitHeader').addClass('disabledbutton');
    $('#btnDeleteHeader').addClass('disabledbutton');
    $('#btnPrintHeader').addClass('disabledbutton');
    if (typeof ($('#Isi_MemoDinasBaru').data('kendoEditor')) != 'undefined')
        $($('#Isi_MemoDinasBaru').data().kendoEditor.body).attr('contenteditable', false);
}

function enableHeader() {
    if ($('#StatusMemoDinas').val() == 'BARU') {
        $("._key").find('span').removeClass('disabledbutton');
        $("._key").removeClass('disabledbutton');
        $("._nonkey").find('span').removeClass('disabledbutton');
        $("._nonkey").removeClass('disabledbutton');
        $('#btnSubmitHeader').removeClass('disabledbutton');
        $('#btnDeleteHeader').removeClass('disabledbutton');
        $('#btnPrintHeader').removeClass('disabledbutton');
        $($('#Isi_MemoDinasBaru').data().kendoEditor.body).attr('contenteditable', true);
    }
    else {
        disableHeader();
    }
}

function filterNomorBalasan(e) {

    var str = "declare @bulanromawi varchar(100); set @bulanromawi='I   II  III IV  V   VI  VII VIIIIX  X   XI  XII '; " + 
        "SELECT DISTINCT D.NamaUnit, C.UnitId, C.Kirim_MemoDinasId, C.Nomor_MemoDinas, 'M/' + D.Nomenklatur + '/' + ltrim(rtrim(cast(C.Nomor_MemoDinas as varchar(5)))) + '/' + rtrim(SUBSTRING(@bulanromawi, ((month(C.Tanggal_MemoDinas) - 1) * 4) + 1, 4)) +'/' + cast(year(C.Tanggal_MemoDinas) as varchar(4)) AS Nomor_MemoDinasView, C.Tanggal_MemoDinas, C.Perihal_MemoDinas " +
        "FROM[SPDK_KanpusEF].[dbo].[eOffice_TindakLanjut_MemoDinas] A " +
        "JOIN[SPDK_KanpusEF].[dbo].[eOffice_Terima_MemoDinas] B ON A.Terima_MemoDinasId = B.Terima_MemoDinasId " +
        "JOIN[SPDK_KanpusEF].[dbo].[eOffice_Kirim_MemoDinas] C ON B.Kirim_MemoDinasId = C.Kirim_MemoDinasId " +
        "JOIN[ReferensiEF].[dbo].[Unit] D ON C.UnitId = D.OrgId " +
        "WHERE B.UnitId = '" + $('#UnitId').val() + "' and C.UnitId='" + $('#TujuanId_MemoDinas').val()+"' and A.StatusTindakLanjutMemoDinas != ''";
    return {
         strClassView: 'Ptpn8.Models.MemoOnline.ViewKirim_MemoDinas',
        scriptSQL: str,
        _menuId: menuId
    };
}

function NomorBalasanOnSelect(e) {
    var noMemo = this.dataItem(e.item);
    $('#Balasan_MemoDinasId').val(noMemo.Kirim_MemoDinasId);
}

function readSuratMasuk() {
    ambilSuratMasuk($('#TahunAgenda_SuratMasuk').data('kendoNumericTextBox').value(),
        $('#NomorAgenda_SuratMasuk').data('kendoNumericTextBox').value(),
        $('#TujuanId_MemoDinas').val()
    ).done(function (dta) {
        if (dta.length > 0) {
            $('#Nomor_SuratMasuk').val(dta[0][5]);
            $('#Pengirim_SuratMasuk').val(dta[0][7]);
            $('#Perihal_SuratMasuk').val(dta[0][9]);
            $('#Tanggal_SuratMasuk').data('kendoDateTimePicker').value(new Date(dta[0][4]));
            $('#img1').removeAttr('src');
            $('#imgdisposisi').removeAttr('src');
            $('#SuratDinasMasukId').val(dta[0][12]);
            d = new Date();
            $('#img1').attr('src', '/Content/Images/View/' + $('#UserName').val() + '_img1.jpg?'+d.getTime());
            $('#imgdisposisi').attr('src', '/Content/Images/View/' + $('#UserName').val() + '_imgdisposisi.jpg?' + d.getTime());

            var editor;
            editor = $('#Isi_MemoDinasBaru').data('kendoEditor');
            if ((headerBaru || editor.value() =="") && $("#radio_1").is(":checked") ) {
                $('#Perihal_MemoDinas').val('Pengantar Surat Dinas dari: ' + dta[0][7] + ' Nomor: ' + dta[0][5]);
                editor.value("<p>Menindaklanjuti surat dari " + dta[0][7] + " nomor " + dta[0][5] + " tanggal " + dta[0][4].substr(0,10) + " perihal " + dta[0][9] +
                    ", bersama ini disampaikan untuk di tindaklanjuti sesuai dengan disposisi direksi di atas. </p>" +
                    "<p>Atas kerja sama yang baik, diucapkan terima kasih.</p>");
            }


            if (dta[0][13] == 'SUDIN-DISPOSISI') {

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
        else {
            $('#Nomor_SuratMasuk').val('');
            $('#Pengirim_SuratMasuk').val('');
            $('#Perihal_SuratMasuk').val('');
            $('#Tanggal_SuratMasuk').data('kendoDateTimePicker').value(new Date('01/01/1900'));
            $('#img1').attr('src', '');
            $('#imgdisposisi').attr('src', '');
            var editor;
            editor = $('#Isi_MemoDinasBaru').data('kendoEditor');
            editor.value("");
        }
    });
}

function ambilSuratMasuk(thn,nmr,id) {
//scriptSQL: "SET DATEFORMAT DMY; SELECT TOP 1 [thn_agenda],[no_agenda],[tgl_terima],[penerima],[tgl_surat],[no_surat],[kel_pengirim],[pengirim],[kel_perihal],[perihal],[img1],[imgdisposisi] FROM [Sekper].[dbo].[agenda_surat_masuk] WHERE thn_agenda=" + thn + " and no_agenda=" + nmr,
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "SET DATEFORMAT DMY; SELECT TOP 1 [thn_agenda],[no_agenda],[tgl_terima],[penerima],[tgl_surat],[no_surat],[kel_pengirim],[pengirim],[kel_perihal],[perihal],[img1],[imgdisposisi], " +
                " B.SuratDinasMasukId, B.StatusSuratDinas, B.TujuanId_SuratDinasMasuk,B.CCTujuanId_SuratDinasMasuk, C.Sifat_SuratDinasMasuk, C.TujuanDisposisi_SuratDinasMasuk, C.IsiDisposisi_SuratDinasMasuk, C.PenjelasanDisposisi_SuratDinasMasuk,C.TanggalDidisposisiOlehDireksi FROM [Sekper].[dbo].[agenda_surat_masuk] A " +
                " JOIN [SPDK_KanpusEF].[dbo].[eOffice_SuratDinasMasuk] B ON B.TahunAgenda=A.thn_agenda AND B.NoAgenda=A.no_agenda " +
                " JOIN [SPDK_KanpusEF].[dbo].[eOffice_DisposisiSuratDinasMasuk] C ON C.SuratDinasMasukId=B.SuratDinasMasukId AND C.DireksiId=B.TujuanId_SuratDinasMasuk" +
            " WHERE thn_agenda=" + thn + " and no_agenda=" + nmr + " AND (SELECT count(*) FROM [SPDK_KanpusEF].[dbo].[eOffice_SuratDinasMasuk] WHERE TahunAgenda = " + thn + " and NoAgenda = " + nmr + " and StatusSuratDinas = 'SUDIN-DISPOSISI' and C.TujuanDisposisi_SuratDinasMasuk like '%"+id+"%')>= 1", 
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
