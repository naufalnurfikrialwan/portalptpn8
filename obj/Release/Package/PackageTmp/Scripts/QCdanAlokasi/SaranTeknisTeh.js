var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var kebun;
var hdr_SaranTeknisId, mainBudidayaId, subBudidayaId, dtl_SaranTeknisId;
var listUjiMutuId, listNo_UjiMutu;
var listKebunId, listNamaKebun;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var rowNumber = 0;
var menuId = '01d6bf6a-afe0-455c-89bf-39e653cea3ed';

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
});

$(window).on("beforeunload", function () {
    return "Please don't leave me!";
});

$(window).on("unload", function () {
    hapusPenggunaPortalYangAktif();
});

function renderNumber(data) {
    var no = ++rowNumber;
    data.NoBaris = no;
    return no;
}

function cekNamaSubBudidaya(namaSubBudidaya) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select SubBudidayaId from [ReferensiEF].[dbo].[SubBudidaya]" +
                " where lower(Nama)='" + namaSubBudidaya.toLowerCase() + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function cekNamaMerk(namaMerk) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select KebunBudidayaId from [ReferensiEF].[dbo].[KebunBudidaya]" +
                " where SubBudidayaId='" + subBudidayaId + "' and lower(Merk)='" + namaMerk.toLowerCase() + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function cekNamaGrade(namaGrade) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select GradeId,JumlahSatuan,Standar_BeratPerSatuan,Tara from [ReferensiEF].[dbo].[Grade]" +
                " where SubBudidayaId='" + subBudidayaId + "' and lower(Nama)='" + namaGrade.toLowerCase() + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function cekNamaSatuan(namaSatuan) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select SatuanId from [ReferensiEF].[dbo].[Satuan]" +
                " where lower(Nama)='" + namaSatuan.toLowerCase() + "'",
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
                        gridStatus = 'belum-ngapa-ngapain';
                        hdr_SaranTeknisId = $('#HDR_SaranTeknisId').val();
                        $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                            disableHeader();
                            $('#btnDeleteHeader').removeClass('disabledbutton');
                            $('#btnPrintHeader').removeClass('disabledbutton');
                            $('#btnDeleteHeader').attr('disabled', false);
                            $('#btnPrintHeader').attr('disabled', false);
                            $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                            $('#grdDetail').removeClass('disabledbutton');
                            getDataFrom().done(function (data) {
                                if (data == 0) {
                                    InsertGridFrom().done(function (data) {
                                        $("#grdDetail").data("kendoGrid").dataSource.read({ id: $('#HDR_SaranTeknisId').val() });
                                    }).fail(function (x) {
                                        alert('Error! Hubungi Bagian TI');
                                    });
                                }
                                else {
                                    var grid = $("#grdDetail").data("kendoGrid");
                                    grid.dataSource.read({
                                        id: $('#HDR_SaranTeknisId').val()
                                    }).done(function () {
                                        var currentData = grid.dataSource.data();
                                        for (var i = 0; i < currentData.length; i++) {
                                            currentData[i].dirty = true;
                                        }
                                    });
                                }
                            }).fail(function (x) {
                                alert('Error! Hubungi Bagian TI');
                            });
                        });

                    }
                    else {
                        gridStatus = 'belum-ngapa-ngapain';
                        openErrWindow();
                        $('#grdDetail').addClass('disabledbutton');
                        gridDestroy();
                    }

                })
                .fail(function (x) {
                    gridStatus = 'belum-ngapa-ngapain';
                    alert("Error! Hubungi Bagian TI");
                });  //edited
            } else {
                $(".k-button-icontext").addClass("k-state-disabled").removeClass("k-grid-add"); // edited
                gridStatus = 'belum-ngapa-ngapain';
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

        //PeriksaConstraintDetail(row).done(function (data) {
        //    if (data == 0) {
        //        $('#grdDetail').data('kendoGrid').removeRow(row);
        //        gridStatus = 'sudah-diapa-apain';
        //    }
        //    else {
        //        openDelDetailWindow();
        //    }
        //}).fail(function (x) {
        //    alert('Error! Hubungi Bagian TI');
        //});
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
    $('#HDR_SaranTeknisId').val(_NomorInputView.HDR_SaranTeknisId);
    $('#TahunBuku').val(_NomorInputView.TahunBuku);
    $('#NomorInput').val(_NomorInputView.NomorInput);
    $('#NomorInputView').val(_NomorInputView.NomorInputView);
    $('#No_SaranTeknis').val(_NomorInputView.No_SaranTeknis);
    $('#Tanggal_SaranTeknis').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.Tanggal_SaranTeknis.substr(6))));
    $('#ListKebunId').val(_NomorInputView.ListKebunId);
    $('#ListNamaKebun').val(_NomorInputView.ListNamaKebun);
    var tglVerKaur = new Date(parseInt(_NomorInputView.TglVerKaur.substr(6)));
    $('#TglVerKaur').val(tglVerKaur.getDate() + "/" + (tglVerKaur.getMonth() + 1) + "/" + tglVerKaur.getFullYear());
    $('#UserName').val(_NomorInputView.UserName);
    $('#MainBudidayaId').val(_NomorInputView.MainBudidayaId);
    $('#NamaMainBudidaya').val(_NomorInputView.NamaMainBudidaya);

    var lst = $('#ListUjiMutuId').data('kendoMultiSelect');
    lst.dataSource.read().done(function (data) {
        lst.value(_NomorInputView.ListUjiMutuId);
        $('#ListNo_UjiMutu').val(_NomorInputView.ListNo_UjiMutu);
    });

    var lst2 = $('#ListKebunId').data('kendoMultiSelect');
    lst2.dataSource.read().done(function (data) {
        lst2.value(_NomorInputView.ListKebunId);
        $('#ListNamaKebun').val(_NomorInputView.ListNamaKebun);
    });

}

function ViewToModel() {
    var listUjiMutuId = $('#ListUjiMutuId').val();
    if (listUjiMutuId == null) listUjiMutuId = "";
    var listNo_UjiMutu = $('#ListNo_UjiMutu').val();
    if (listNo_UjiMutu == null) listNo_UjiMutu = "";

    var listKebunId = $('#ListKebunId').val();
    if (listKebunId == null) listKebunId = "";
    var listNamaKebun = $('#ListNamaKebun').val();
    if (listNamaKebun == null) listNamaKebun = "";

    var mdl = {
        HDR_SaranTeknisId: $('#HDR_SaranTeknisId').val(),
        TahunBuku: $('#TahunBuku').val(),
        NomorInput: $('#NomorInput').val(),
        NomorInputView: Array(5 - String($('#NomorInput').val()).length + 1).join('0') + $('#NomorInput').val() + ' - ' + $('#No_SaranTeknis').val().toUpperCase(),
        No_SaranTeknis: $('#No_SaranTeknis').val().toUpperCase(),
        ListKebunId: listKebunId.toString(),
        ListNamaKebun: listNamaKebun.toString(),
        Tanggal_SaranTeknis: $('#Tanggal_SaranTeknis').val(),
        ListUjiMutuId: listUjiMutuId.toString(),
        ListNo_UjiMutu: listNo_UjiMutu.toString(),
        VerKaur: $('#VerKaur').val(),
        TglVerKaur: $('#TglVerKaur').val(),
        UserName: $('#UserName').val(),
        MainBudidayaId: $('#MainBudidayaId').val(),
        NamaMainBudidaya: $('#NamaMainBudidaya').val().toUpperCase(),
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
                            hdr_SaranTeknisId = $('#HDR_SaranTeknisId').val();
                            $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                                disableHeader();
                                $('#btnDeleteHeader').removeClass('disabledbutton');
                                $('#btnPrintHeader').removeClass('disabledbutton');
                                $('#btnDeleteHeader').attr('disabled', false);
                                $('#btnPrintHeader').attr('disabled', false);
                                $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                                $('#grdDetail').removeClass('disabledbutton');
                                $("#grdDetail").data("kendoGrid").dataSource.read({
                                    id: $('#HDR_SaranTeknisId').val()
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_SaranTeknis]" +
                " where HDR_SaranTeknisId='" + $("#HDR_SaranTeknisId").val() + "'",
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[HDR_Alokasi]" +
                " where patindex('%" + $("#HDR_AlokasiId").val() + "%',ListSaranTeknisId)>0",
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

        if (typeof (_NomorInputView.ListUjiMutuId) == "string" || typeof (_NomorInputView.ListKebunId) == "string") {
            if (_NomorInputView.ListUjiMutuId == "" || _NomorInputView.ListKebunId == "") {
                _NomorInputView.ListUjiMutuId = [];
                _NomorInputView.ListKebunId = [];
                _NomorInputView.ListNo_UjiMutu = [];
                _NomorInputView.ListNamaKebun = [];
            }
            else {
                _NomorInputView.ListUjiMutuId = _NomorInputView.ListUjiMutuId.split(",");
                _NomorInputView.ListNo_UjiMutu = _NomorInputView.ListNo_UjiMutu.split(",");
                _NomorInputView.ListKebunId = _NomorInputView.ListKebunId.split(",");
                _NomorInputView.ListNamaKebun = _NomorInputView.ListNamaKebun.split(",");
            }
        }

        cekData(_NomorInputView.NomorInputView);
        ModelToView(_NomorInputView);
        hdr_SaranTeknisId = _NomorInputView.HDR_SaranTeknisId;
        $('#No_SaranTeknis').focus();
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
        parameters: { HDR_SaranTeknisId: $('#HDR_SaranTeknisId').val() }
    });
    viewer.refreshReport();
    prt.open().center();
}

function ddlBudidayaOnSelect(e) {
    ddlBudidaya = this.dataItem(e.item);
}

function ddlBudidayaOnDataBound(e) {
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

function getDataFrom() {
    // Hitung Jumlah Record di Detail
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_SaranTeknis]" +
                " where HDR_SaranTeknisId='" + $("#HDR_SaranTeknisId").val() + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function InsertGridFrom() {
    var grd = $('#grdDetail').data('kendoGrid');
    var url = '/Input/GetDataFrom';
    var lst = $('#ListUjiMutuId').data('kendoMultiSelect').value();
    var str = "";
    for (var i = 0; i < lst.length; i++) {
        str = str + "'" + lst[i] + "'";
        if (i < lst.length - 1) str = str + ",";
    }

    var lst2 = $('#ListKebunId').data('kendoMultiSelect').value();
    var str2 = "";
    for (var i = 0; i < lst2.length; i++) {
        str2 = str2 + "'" + lst2[i] + "'";
        if (i < lst2.length - 1) str2 = str2 + ",";
    }

    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            strClassView: "Ptpn8.QCdanAlokasi.Models.View_DTLSaranTeknis",
            scriptSQL: "INSERT INTO [SPDK_KanpusEF].[dbo].[DTL_SaranTeknis]" +
                " SELECT newid() as DTL_SaranTeknisId, cast('" + $('#HDR_SaranTeknisId').val() + "' as uniqueidentifier) as HDR_SaranTeknisId," +
                " A.DTL_ProductSampleId, A.SubBudidayaId, A.MerkId, A.GradeId, A.SatuanId,  A.BrokerId, A.Chop, '', A.No_SP, A.Tanggal_SP " +
                " from [SPDK_KanpusEF].[dbo].[DTL_UjiMutu] as A" +
                " join [ReferensiEF].[dbo].[KebunBudidaya] as B on B.KebunBudidayaId=A.MerkId" +
                " where HDR_UjiMutuId IN (" + str + ") and A.Score_Total < 41 and KebunId IN (" + str2 + ")",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

// Detail

function grdOnChange(e) {
}

function grdOnEdit(e) {
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.DTL_SaranTeknisId = model.uid;
        model.HDR_SaranTeknisId = hdr_SaranTeknisId;
    }
    dtl_SaranTeknisId = model.DTL_SaranTeknisId;
    mainBudidayaId = $('#MainBudidayaId').val();
    gridStatus = 'sudah-diapa-apain';
    $('#jmlGross').text(kendo.toString(rekapGross(), 'n2'));
    $('#jmlTare').text(kendo.toString(rekapTare(), 'n2'));
    $('#jmlQtySacks').text(kendo.toString(rekapQtySacks(), 'n2'));
}

function rekapGross() {

    var grid = $('#grdDetail').data('kendoGrid');
    var newValue = 0;
    for (var i = 0; i < grid.dataItems().length; i++) {
        newValue += grid.dataItems()[i].Gross;
    }
    return newValue;
}

function rekapTare() {

    var grid = $('#grdDetail').data('kendoGrid');
    var newValue = 0;
    for (var i = 0; i < grid.dataItems().length; i++) {
        newValue += grid.dataItems()[i].Tare;
    }
    return newValue;
}

function rekapQtySacks() {
    var grid = $('#grdDetail').data('kendoGrid');
    var newValue = 0;
    for (var i = 0; i < grid.dataItems().length; i++) {
        newValue += grid.dataItems()[i].QtySacks;
    }
    return newValue;
}

function ListUjiMutuIdOnSelect(e) {
    var listUjiMutuId = e.sender.dataItem();
    var arr = [];
    var arr1 = [];
    if ($('#ListUjiMutuId').val() != null) {
        var lst = $('#ListUjiMutuId').data('kendoMultiSelect');
        for (var i = 0; i < lst.dataItems().length; i++) {
            arr.push(lst.dataItems()[i].No_UjiMutu);
        }
        $('#ListNo_UjiMutu').val(arr.toString());
    }
}

function filterUjiMutu() {
    var listId = [];
    for (var i = 0; i < _NomorInputView.ListUjiMutuId.length; i++) {
        listId.push("'" + _NomorInputView.ListUjiMutuId[i].toString() + "'");
    }

    if (headerBaru == true) {
        return {
            strClassView: 'Ptpn8.QCdanAlokasi.Models.View_HDRUjiMutu',
            scriptSQL: "select DISTINCT A.HDR_UjiMutuId, A.No_UjiMutu from [SPDK_KanpusEF].[dbo].[HDR_UjiMutu] as A" +
                " inner join [SPDK_KanpusEF].[dbo].[DTL_UjiMutu] as B on A.HDR_UjiMutuId=B.HDR_UjiMutuId" +
                " inner join [ReferensiEF].[dbo].[KebunBudidaya] as C on B.MerkId=C.KebunBudidayaId" +
                " where A.VerKaur=1 and B.SudahSaranTeknis=0 and B.Score_Total < 41  and A.TahunBuku=" + $('#TahunBuku').val(),
            _menuId: menuId
        }
    }
    else {
        return {
            strClassView: 'Ptpn8.QCdanAlokasi.Models.View_HDRUjiMutu',
            scriptSQL: "select DISTINCT A.HDR_UjiMutuId, A.No_UjiMutu from [SPDK_KanpusEF].[dbo].[HDR_UjiMutu] as A" +
                " inner join [SPDK_KanpusEF].[dbo].[DTL_UjiMutu] as B on A.HDR_UjiMutuId=B.HDR_UjiMutuId" +
                 " inner join [ReferensiEF].[dbo].[KebunBudidaya] as C on B.MerkId=C.KebunBudidayaId" +
                " where A.HDR_UjiMutuId in (" + listId + ")",
            _menuId: menuId
        }
    }
}

//kebun

function ListKebunIdOnSelect(e) {
    var listKebunId = e.sender.dataItem();
    var arr = [];
    var arr1 = [];
    if ($('#ListKebunId').val() != null) {
        var lst = $('#ListKebunId').data('kendoMultiSelect');
        for (var i = 0; i < lst.dataItems().length; i++) {
            arr.push(lst.dataItems()[i].Nama);
        }
        $('#ListNamaKebun').val(arr.toString());
    }
}

function filterKebun() {
    var listId = [];
    for (var i = 0; i < _NomorInputView.ListKebunId.length; i++) {
        listId.push("'" + _NomorInputView.ListKebunId[i].toString() + "'");
    }

    if (headerBaru == true) {
        return {
            strClassView: 'Ptpn8.Areas.Referensi.Models.Kebun',
            scriptSQL: "select DISTINCT A.KebunId, A.Nama from [ReferensiEF].[dbo].[Kebun] as A" +
                " inner join [ReferensiEF].[dbo].[KebunBudidaya] as B on A.KebunId=B.KebunId" +
                " inner join [SPDK_KanpusEF].[dbo].[DTL_UjiMutu] as C on B.KebunBudidayaId=C.MerkId" +
                " inner join [SPDK_KanpusEF].[dbo].[HDR_UjiMutu] as D on C.HDR_UjiMutuId=D.HDR_UjiMutuId" +
                " where D.VerKaur=1 and D.TahunBuku=" + $('#TahunBuku').val() + " and C.Score_Total < 41 and SudahSaranTeknis=0",
            _menuId: menuId
        }
    }
    else {
        return {
            strClassView: 'Ptpn8.Areas.Referensi.Models.Kebun',
            scriptSQL: "select DISTINCT A.KebunId, A.Nama from [ReferensiEF].[dbo].[Kebun] as A" +
                //" inner join [ReferensiEF].[dbo].[KebunBudidaya] as B on A.KebunId=B.KebunId" +
                //" inner join [SPDK_KanpusEF].[dbo].[DTL_UjiMutu] as C on B.KebunBudidayaId=C.MerkId" +
                //" inner join [SPDK_KanpusEF].[dbo].[HDR_UjiMutu] as D on C.HDR_UjiMutuId=D.HDR_UjiMutuId" +
                " where A.KebunId in (" + listId + ")",
            _menuId: menuId
        }
    }
}

function grdOnDataBinding(e) {

}

//autocomplete subbudidaya
function filterSubBudidaya(e) {
    return {
        mainBudidayaId: $('#MainBudidayaId').val(),
        value: $("#NamaSubBudidaya").val()
    };
}

function aucNamaSubBudidayaOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_SaranTeknisId)) == "undefined" ? grid.dataSource.getByUid(dtl_SaranTeknisId) : grid.dataSource.get(dtl_SaranTeknisId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.SubBudidayaId = ddlItem.SubBudidayaId;
    subBudidayaId = ddlItem.SubBudidayaId;
}

function aucNamaSubBudidayaOnDataBound(e) {

}

//autocomplete Merk Kebun
function filterMerk(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_SaranTeknisId)) == "undefined" ? grid.dataSource.getByUid(dtl_SaranTeknisId) : grid.dataSource.get(dtl_SaranTeknisId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    var subBudidayaId = data.SubBudidayaId;
    return {
        value: $("#NamaMerk").val(),
        subBudidayaId: subBudidayaId
    };
}

function aucNamaMerkOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_SaranTeknisId)) == "undefined" ? grid.dataSource.getByUid(dtl_SaranTeknisId) : grid.dataSource.get(dtl_SaranTeknisId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.MerkId = ddlItem.KebunBudidayaId;
}

function aucNamaMerkOnDataBound(e) {

}

//autocomplete Satuan
function filterSatuan(e) {
    return {
        value: $("#NamaSatuan").val()
    };
}

function aucNamaSatuanOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_SaranTeknisId)) == "undefined" ? grid.dataSource.getByUid(dtl_SaranTeknisId) : grid.dataSource.get(dtl_SaranTeknisId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.SatuanId = ddlItem.SatuanId;
}

function aucNamaSatuanOnDataBound(e) {

}

//autocomplete Grade
function filterGrade(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_SaranTeknisId)) == "undefined" ? grid.dataSource.getByUid(dtl_SaranTeknisId) : grid.dataSource.get(dtl_SaranTeknisId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    var subBudidayaId = data.SubBudidayaId;
    return {
        value: $("#NamaGrade").val(),
        subBudidayaId: subBudidayaId
    };
}

function aucNamaGradeOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_SaranTeknisId)) == "undefined" ? grid.dataSource.getByUid(dtl_SaranTeknisId) : grid.dataSource.get(dtl_SaranTeknisId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.GradeId = ddlItem.GradeId;

    if (ddlItem.JumlahSatuan > 0) {
        var qtySacks = data.QtySacks;
        var kgTara = ddlItem.Tara;
        if (qtySacks == 0) {
            data.QtySacks = ddlItem.JumlahSatuan;
            data.Gross = (ddlItem.JumlahSatuan * ddlItem.Standar_BeratPerSatuan) + kgTara;
            data.Tare = kgTara;
        }
    }
}

function aucNamaGradeOnDataBound(e) {

}

//autocomplete Broker
function filterBroker(e) {
    return {
        value: $("#NamaBroker").val()
    };
}

function aucNamaBrokerOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_SaranTeknisId)) == "undefined" ? grid.dataSource.getByUid(dtl_SaranTeknisId) : grid.dataSource.get(dtl_SaranTeknisId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.BrokerId = ddlItem.BrokerId;
}

function aucNamaBrokerOnDataBound(e) {

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

function ambilStandarGrade() {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_SaranTeknisId)) == "undefined" ? grid.dataSource.getByUid(dtl_SaranTeknisId) : grid.dataSource.get(dtl_SaranTeknisId);
    return $.ajax({
        url: '/Input/ExecSQL',
        data: {
            scriptSQL:
                " select JumlahSatuan,Standar_BeratPerSatuan,Tara from [ReferensiEF].[dbo].[Grade]" +
                " where GradeId='" + dataItem.GradeId + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}
//// sampe sini
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

function filterTester() {
    return {
        key: "Tester",
        value: $("#Tester").val(),
        _menuId: menuId
    };
}

//function PeriksaConstraintDetail(row) {
//    // periksa apakah punya data di detail dan punya data di invoice
//    // kalo ya, batalkan hapus header
//    var id = row.find("td:first").html()
//    var url = '/Input/ExecSQL';
//    return $.ajax({
//        url: url,
//        data: {
//            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_Alokasi]" +
//                " where DTL_AlokasiId='" + id + "'",
//            _menuId: menuId
//        },
//        type: 'GET',
//        datatype: 'json'
//    });
//}