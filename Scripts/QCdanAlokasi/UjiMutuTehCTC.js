var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var kebun;
var hdr_UjiMutuId, mainBudidayaId, subBudidayaId, dtl_UjiMutuId;
var listProductSampleId, listNo_ProductSample;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var rowNumber = 0;
var menuId = '38f9b43a-af7e-4549-abb9-e44e11c4f9e8';

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
            validation: function (input) {
                if ((input.attr("name") == "Score_Appearance" || input.attr("name") == "Score_Infusion" || input.attr("name") == "Score_Liquor") && input.val() != "") {
                    var grid = $('#grdDetail').data('kendoGrid');
                    var dataItem = typeof (grid.dataSource.get(dtl_UjiMutuId)) == "undefined" ? grid.dataSource.getByUid(dtl_UjiMutuId) : grid.dataSource.get(dtl_UjiMutuId);
                    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");

                    $('#jmlAppearance').text(kendo.toString(rekapAppearance(), 'n2'));
                    $('#jmlInfusion').text(kendo.toString(rekapInfusion(), 'n2'));
                    $('#jmlLiquor').text(kendo.toString(rekapLiquor(), 'n2'));

                    dataItem.Score_Total = hitungJumlah(dtl_UjiMutuId);
                    dataItem.Nilai_Huruf = hitungNilaiHuruf(dtl_UjiMutuId);
                    dataItem.KriteriaPenerimaan = hitungKriteriaPenerimaan(dtl_UjiMutuId);

                    row[0].cells[21].textContent = kendo.toString(dataItem.Score_Total, 'n2');
                    row[0].cells[22].textContent = dataItem.Nilai_Huruf;
                    row[0].cells[23].textContent = dataItem.KriteriaPenerimaan;
                    if (dataItem.KriteriaPenerimaan == "Diterima") {
                        // hijau
                        row[0].cells[23].style.color = "white";
                        row[0].cells[23].style.backgroundColor = "green";
                    }
                    else if (dataItem.KriteriaPenerimaan == "Dipertimbangkan") {
                        // kuning
                        row[0].cells[23].style.color = "black";
                        row[0].cells[23].style.backgroundColor = "yellow";
                    }
                    else {
                        // merah
                        row[0].cells[23].style.color = "white";
                        row[0].cells[23].style.backgroundColor = "red";

                    }

                    //18,19,20

                    return true;
                }
                else {
                    return true;
                };
            }
        }
    });
    //////// sampe sini

});

function renderNumber(data) {
    var no = ++rowNumber;
    data.NoBaris = no;
    return no;
}

$(window).on("beforeunload", function () {
    return "Please don't leave me!";
});

$(window).on("unload", function () {
    hapusPenggunaPortalYangAktif();
});

function resetRowNumber(e) {
    rowNumber = 0;
    var jmlNilai = 0;
    var jmlNilaiHuruf = '';
    var kriteriaPenerimaan = '';
    var jmlAppearance = 0;
    var jmlInfusion = 0;
    var jmlLiquor = 0;
    var grid = $('#grdDetail').data('kendoGrid');
    var datasource = grid.dataSource.data();
    for (var i = 0; i < datasource.length; i++) {
        var model = datasource[i];
        var row = grid.tbody.find("tr[data-uid='" + model.uid + "']");

        model.Score_Total = hitungJumlah(model.DTL_UjiMutuId);
        model.Nilai_Huruf = hitungNilaiHuruf(model.DTL_UjiMutuId);
        model.KriteriaPenerimaan = hitungKriteriaPenerimaan(model.DTL_UjiMutuId);
        $('#jml' + model.DTL_UjiMutuId).text(kendo.toString(model.Score_Total, 'n2'));
        $('#jmlHuruf' + model.DTL_UjiMutuId).text(model.Nilai_Huruf);
        $('#kriteriaPenerimaan' + model.DTL_UjiMutuId).text(model.KriteriaPenerimaan);
        jmlNilai = jmlNilai + model.Score_Total;
        jmlNilaiHuruf = model.Nilai_Huruf;
        jmlAppearance = jmlAppearance + model.Score_Appearance;
        jmlInfusion = jmlInfusion + model.Score_Infusion;
        jmlLiquor = jmlLiquor + model.Score_Liquor;
        kriteriaPenerimaan = model.KriteriaPenerimaan;

        if (model.KriteriaPenerimaan == "Diterima") {
            // hijau
            row[0].cells[23].style.color = "white";
            row[0].cells[23].style.backgroundColor = "green";
        }
        else if (model.KriteriaPenerimaan == "Dipertimbangkan") {
            // kuning
            row[0].cells[23].style.color = "black";
            row[0].cells[23].style.backgroundColor = "yellow";
        }
        else {
            // merah
            row[0].cells[23].style.color = "white";
            row[0].cells[23].style.backgroundColor = "red";

        }


    }
    $('#jmlNilai').text(kendo.toString(jmlNilai, 'n2'));
    $('#jmlNilaiHuruf').text(jmlNilaiHuruf);
    $('#kriteriaPenerimaan').text(kriteriaPenerimaan);
    $('#jmlAppearance').text(kendo.toString(jmlAppearance, 'n2'));
    $('#jmlInfusion').text(kendo.toString(jmlInfusion, 'n2'));
    $('#jmlLiquor').text(kendo.toString(jmlLiquor, 'n2'));
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
                        hdr_UjiMutuId = $('#HDR_UjiMutuId').val();
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
                                        $("#grdDetail").data("kendoGrid").dataSource.read({ id: $('#HDR_UjiMutuId').val() });
                                    }).fail(function (x) {
                                        alert('Error! Hubungi Bagian TI');
                                    });
                                }
                                else {
                                    var grid = $("#grdDetail").data("kendoGrid");
                                    grid.dataSource.read({
                                        id: $('#HDR_UjiMutuId').val()
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
    var row = $(e.currentTarget).closest("tr");
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = grid.dataSource.getByUid(row.data().uid);
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
        PeriksaConstraintDetail(dataItem).done(function (data) {
            if (data == 0) {
                grid.removeRow(row);
                gridStatus = 'sudah-diapa-apain';
            }
            else {
                openDelDetailWindow();
            }
        }).fail(function (x) {
            alert('Error! Hubungi Bagian TI');
        });
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
    $('#HDR_UjiMutuId').val(_NomorInputView.HDR_UjiMutuId);
    $('#TahunBuku').val(_NomorInputView.TahunBuku);
    $('#NomorInput').val(_NomorInputView.NomorInput);
    $('#NomorInputView').val(_NomorInputView.NomorInputView);
    $('#No_UjiMutu').val(_NomorInputView.No_UjiMutu);
    $('#Tanggal_Uji').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.Tanggal_Uji.substr(6))));
    $('#Tester').val(_NomorInputView.Tester);
    var tglVerKaur = new Date(parseInt(_NomorInputView.TglVerKaur.substr(6)));
    $('#TglVerKaur').val(tglVerKaur.getDate() + "/" + (tglVerKaur.getMonth() + 1) + "/" + tglVerKaur.getFullYear());
    $('#UserName').val(_NomorInputView.UserName);
    $('#MainBudidayaId').val(_NomorInputView.MainBudidayaId);
    $('#NamaMainBudidaya').val(_NomorInputView.NamaMainBudidaya);
    $('#SubBudidayaId').val(_NomorInputView.SubBudidayaId);
    $('#NamaSubBudidaya').val(_NomorInputView.NamaSubBudidaya);

    var lst = $('#ListProductSampleId').data('kendoMultiSelect');
    lst.dataSource.read().done(function (data) {
        lst.value(_NomorInputView.ListProductSampleId);
        $('#ListNo_ProductSample').val(_NomorInputView.ListNo_ProductSample);
    });

}

function ViewToModel() {
    var listProductSampleId = $('#ListProductSampleId').val();
    if (listProductSampleId == null) listProductSampleId = "";
    var listNo_ProductSample = $('#ListNo_ProductSample').val();
    if (listNo_ProductSample == null) listNo_ProductSample = "";
    var mdl = {
        HDR_UjiMutuId: $('#HDR_UjiMutuId').val(),
        TahunBuku: $('#TahunBuku').val(),
        NomorInput: $('#NomorInput').val(),
        NomorInputView: Array(5 - String($('#NomorInput').val()).length + 1).join('0') + $('#NomorInput').val() + ' - ' + $('#No_UjiMutu').val().toUpperCase(),
        No_UjiMutu: $('#No_UjiMutu').val().toUpperCase(),
        Tester: $('#Tester').val().toUpperCase(),
        Tanggal_Uji: $('#Tanggal_Uji').val(),
        ListProductSampleId: listProductSampleId.toString(),
        ListNo_ProductSample: listNo_ProductSample.toString(),
        VerKaur: $('#VerKaur').val(),
        TglVerKaur: $('#TglVerKaur').val(),
        UserName: $('#UserName').val(),
        MainBudidayaId: $('#MainBudidayaId').val(),
        NamaMainBudidaya: $('#NamaMainBudidaya').val().toUpperCase(),
        SubBudidayaId: $('#SubBudidayaId').val(),
        NamaSubBudidaya: $('#NamaSubBudidaya').val().toUpperCase()
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
                            hdr_UjiMutuId = $('#HDR_UjiMutuId').val();
                            $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                                disableHeader();
                                $('#btnDeleteHeader').removeClass('disabledbutton');
                                $('#btnPrintHeader').removeClass('disabledbutton');
                                $('#btnDeleteHeader').attr('disabled', false);
                                $('#btnPrintHeader').attr('disabled', false);
                                $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                                $('#grdDetail').removeClass('disabledbutton');
                                $("#grdDetail").data("kendoGrid").dataSource.read({
                                    id: $('#HDR_UjiMutuId').val()
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_UjiMutu]" +
                " where HDR_UjiMutuId='" + $("#HDR_UjiMutuId").val() + "'",
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[HDR_SaranTeknis]" +
                " where patindex('%" + $("#HDR_UjiMutuId").val() + "%',ListUjiMutuId)>0",
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

        if (typeof (_NomorInputView.ListProductSampleId) == "string") {
            if (_NomorInputView.ListProductSampleId == "") {
                _NomorInputView.ListProductSampleId = [];
                _NomorInputView.ListNo_ProductSample = [];
            }
            else {
                _NomorInputView.ListProductSampleId = _NomorInputView.ListProductSampleId.split(",");
                _NomorInputView.ListNo_ProductSample = _NomorInputView.ListNo_ProductSample.split(",");
            }
        }
        cekData(_NomorInputView.NomorInputView);
        ModelToView(_NomorInputView);
        hdr_UjiMutuId = _NomorInputView.HDR_UjiMutuId;
        $('#No_UjiMutu').focus();
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
        parameters: { HDR_UjiMutuId: $('#HDR_UjiMutuId').val() }
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_UjiMutu]" +
                " where HDR_UjiMutuId='" + $("#HDR_UjiMutuId").val() + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function InsertGridFrom() {
    var grd = $('#grdDetail').data('kendoGrid');
    var url = '/Input/GetDataFrom';
    var lst = $('#ListProductSampleId').data('kendoMultiSelect').value();
    var str = "";
    for (var i = 0; i < lst.length; i++) {
        str = str + "'" + lst[i] + "'";
        if (i < lst.length - 1) str = str + ",";
    }

    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            strClassView: "Ptpn8.QCdanAlokasi.Models.View_DTLUjiMutu",
            scriptSQL: "INSERT INTO [SPDK_KanpusEF].[dbo].[DTL_UjiMutu] (DTL_UjiMutuId, HDR_UjiMutuId, DTL_ProductSampleId, no_sp, tanggal_sp, chop, gross, Score_Appearance, Score_Infusion, Score_Liquor, Score_Total, " +
                " diterima, SubBudidayaId, MerkId, GradeId, SatuanId, BrokerId) " +
                " SELECT newid() as DTL_UjiMutuId, cast('" + $('#HDR_UjiMutuId').val() + "' as uniqueidentifier) as HDR_UjiMutuId," +
                " A.DTL_ProductSampleId, A.No_SP, A.Tanggal_SP, A.chop, A.gross, 0, 0, 0, 0, 0, A.SubBudidayaId, A.MerkId, A.GradeId, A.SatuanId, A.BrokerId " +
                " from [SPDK_KanpusEF].[dbo].[DTL_ProductSample] as A" +
                " where HDR_ProductSampleId IN (" + str + ") and A.SudahUji=0 and A.SubBudidayaId = '34D10803-E200-4C3B-86C2-6A4F8D2B394E'",
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
        model.DTL_UjiMutuId = model.uid;
        model.HDR_UjiMutuId = hdr_UjiMutuId;
    }
    dtl_UjiMutuId = model.DTL_UjiMutuId;
    mainBudidayaId = $('#MainBudidayaId').val();
    subBudidayaId = $('#SubBudidayaId').val();
    gridStatus = 'sudah-diapa-apain';

    $('#jmlGross').text(kendo.toString(rekapGross(), 'n2'));

    //$('#jmlNilai').text(kendo.toString(rekapNilai(), 'n2'));
    $('#jmlAppearance').text(kendo.toString(rekapAppearance(), 'n2'));
    $('#jmlInfusion').text(kendo.toString(rekapInfusion(), 'n2'));
    $('#jmlLiquor').text(kendo.toString(rekapLiquor(), 'n2'));

    model.Score_Total = hitungJumlah(dtl_UjiMutuId);
    model.Nilai_Huruf = hitungNilaiHuruf(dtl_UjiMutuId);
    model.KriteriaPenerimaan = hitungKriteriaPenerimaan(dtl_UjiMutuId);

    $('#jml' + model.dtl_UjiMutuId).text(kendo.toString(model.Score_Total, 'n2'));
    $('#jmlHuruf' + model.dtl_UjiMutuId).text(model.Nilai_Huruf);
    $('#kriteriaPenerimaan' + model.dtl_UjiMutuId).text(model.KriteriaPenerimaan);


}

function rekapGross() {

    var grid = $('#grdDetail').data('kendoGrid');
    var newValue = 0;
    for (var i = 0; i < grid.dataItems().length; i++) {
        newValue += grid.dataItems()[i].Gross;
    }
    return newValue;
}

function rekapAppearance() {

    var grid = $('#grdDetail').data('kendoGrid');
    var newValue = 0;
    for (var i = 0; i < grid.dataItems().length; i++) {
        newValue += grid.dataItems()[i].Score_Appearance;
    }
    return newValue;
}

function rekapInfusion() {

    var grid = $('#grdDetail').data('kendoGrid');
    var newValue = 0;
    for (var i = 0; i < grid.dataItems().length; i++) {
        newValue += grid.dataItems()[i].Score_Infusion;
    }
    return newValue;
}

function rekapLiquor() {
    var grid = $('#grdDetail').data('kendoGrid');
    var newValue = 0;
    for (var i = 0; i < grid.dataItems().length; i++) {
        newValue += grid.dataItems()[i].Score_Liquor;
    }
    return newValue;
}

function hitungJumlah(id) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(id)) == "undefined" ? grid.dataSource.getByUid(id) : grid.dataSource.get(id);
    var newValue = 0;
    newValue = dataItem.Score_Appearance + dataItem.Score_Infusion + dataItem.Score_Liquor;
    return newValue;
}

function hitungNilaiHuruf(id) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(id)) == "undefined" ? grid.dataSource.getByUid(id) : grid.dataSource.get(id);
    var newValue = '';
    if (dataItem.Score_Total > 80 && dataItem.Score_Total < 101) {
        newValue = 'A';
    }
    else if (dataItem.Score_Total > 60 && dataItem.Score_Total < 81) {
        newValue = 'B';
    }
    else if (dataItem.Score_Total > 40 && dataItem.Score_Total < 61) {
        newValue = 'C';
    }
    else if (dataItem.Score_Total > 20 && dataItem.Score_Total < 41) {
        newValue = 'D';
    }
    else newValue = 'E'
    return newValue;
}

function hitungKriteriaPenerimaan(id) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(id)) == "undefined" ? grid.dataSource.getByUid(id) : grid.dataSource.get(id);
    var newValue = '';
    if (dataItem.Nilai_Huruf == 'A') {
        newValue = 'Diterima';
    }
    else if (dataItem.Nilai_Huruf == 'B') {
        newValue = 'Diterima';
    }
    else if (dataItem.Nilai_Huruf == 'C') {
        newValue = 'Dipertimbangkan';
    }
    else if (dataItem.Nilai_Huruf == 'D') {
        newValue = 'Ditolak';
    }
    else newValue = 'Ditolak'
    return newValue;
}

function rekapAverageScoreTotal() {

    var grid = $('#grdDetail').data('kendoGrid');
    var newValue = 0;
    for (var i = 0; i < grid.dataItems().length; i++) {
        newValue += grid.dataItems()[i].Score_Total;
    }
    return newValue;
}

function ListProductSampleIdOnSelect(e) {
    var listProductSampleId = e.sender.dataItem();
    var arr = [];
    var arr1 = [];
    if ($('#ListProductSampleId').val() != null) {
        var lst = $('#ListProductSampleId').data('kendoMultiSelect');
        for (var i = 0; i < lst.dataItems().length; i++) {
            arr.push(lst.dataItems()[i].No_ProductSample);
        }
        $('#ListNo_ProductSample').val(arr.toString());
    }
}

function filterProductSample() {
    var listId = [];
    for (var i = 0; i < _NomorInputView.ListProductSampleId.length; i++) {
        listId.push("'" + _NomorInputView.ListProductSampleId[i].toString() + "'");
    }

    if (headerBaru == true) {
        return {
            strClassView: 'Ptpn8.QCdanAlokasi.Models.View_HDRProductSample',
            scriptSQL: "select DISTINCT A.HDR_ProductSampleId, A.No_ProductSample from [SPDK_KanpusEF].[dbo].[HDR_ProductSample] as A" +
                " inner join [SPDK_KanpusEF].[dbo].[DTL_ProductSample] as B on A.HDR_ProductSampleId=B.HDR_ProductSampleId" +
                " where A.TahunBuku=" + $('#TahunBuku').val() + "and  B.SudahUji=0 and B.SubBudidayaId = '34D10803-E200-4C3B-86C2-6A4F8D2B394E'",
            _menuId: menuId
        }
    }
    else {
        return {
            strClassView: 'Ptpn8.QCdanAlokasi.Models.View_HDRProductSample',
            scriptSQL: "select DISTINCT A.HDR_ProductSampleId, A.No_ProductSample from [SPDK_KanpusEF].[dbo].[HDR_ProductSample] as A" +
                " inner join [SPDK_KanpusEF].[dbo].[DTL_ProductSample] as B on A.HDR_ProductSampleId=B.HDR_ProductSampleId" +
                " where A.HDR_ProductSampleId in (" + listId + ") and B.SubBudidayaId = '34D10803-E200-4C3B-86C2-6A4F8D2B394E' and A.TahunBuku=" + $('#TahunBuku').val(),
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
    var dataItem = typeof (grid.dataSource.get(dtl_UjiMutuId)) == "undefined" ? grid.dataSource.getByUid(dtl_UjiMutuId) : grid.dataSource.get(dtl_UjiMutuId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_UjiMutuId)) == "undefined" ? grid.dataSource.getByUid(dtl_UjiMutuId) : grid.dataSource.get(dtl_UjiMutuId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_UjiMutuId)) == "undefined" ? grid.dataSource.getByUid(dtl_UjiMutuId) : grid.dataSource.get(dtl_UjiMutuId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_UjiMutuId)) == "undefined" ? grid.dataSource.getByUid(dtl_UjiMutuId) : grid.dataSource.get(dtl_UjiMutuId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.SatuanId = ddlItem.SatuanId;
}

function aucNamaSatuanOnDataBound(e) {

}

//autocomplete Grade
function filterGrade(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_UjiMutuId)) == "undefined" ? grid.dataSource.getByUid(dtl_UjiMutuId) : grid.dataSource.get(dtl_UjiMutuId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_UjiMutuId)) == "undefined" ? grid.dataSource.getByUid(dtl_UjiMutuId) : grid.dataSource.get(dtl_UjiMutuId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_UjiMutuId)) == "undefined" ? grid.dataSource.getByUid(dtl_UjiMutuId) : grid.dataSource.get(dtl_UjiMutuId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_UjiMutuId)) == "undefined" ? grid.dataSource.getByUid(dtl_UjiMutuId) : grid.dataSource.get(dtl_UjiMutuId);
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
        PeriksaConstraintSeluruhDetail().done(function (data) {
            if (data > 0) {
                openDelDetailWindow();
            }
            else {
                var grid = $('#grdDetail').data('kendoGrid');
                var row = grid.tbody.find("tr:first");
                do {
                    $('#grdDetail').data('kendoGrid').removeRow(row);
                    row = grid.tbody.find("tr:first");
                }
                while (grid.tbody.contents().length > 0)
            }
            gridStatus = 'sudah-diapa-apain';
        }).fail(function () {
            alert('Error! Hubungi Bagian TI');
        });
    });
    $("#noDetail").click(function () {
        wndDetail.close();
    });
}

function PeriksaConstraintSeluruhDetail() {
    // periksa apakah punya data di detail dan punya data di invoice
    // kalo ya, batalkan hapus header

    var grid = $('#grdDetail').data('kendoGrid');
    var listId = [];
    var data = grid.dataSource._data;
    for (var i = 0; i < data.length; i++) {
        listId.push("'" + data[i].DTL_ProductSampleId.toString() + "'");
    }
    var strId = listId.toString();

    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_Alokasi]" +
                " where DTL_ProductSampleId in (" + strId + ")",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
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

function PeriksaConstraintDetail(dataItem) {
    // periksa apakah punya data di detail dan punya data di invoice
    // kalo ya, batalkan hapus header
    //var id = row.find("td:first").html()
    var id = dataItem.DTL_ProductSampleId;
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_SaranTeknis]" +
                " where DTL_ProductSampleId='" + id + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}