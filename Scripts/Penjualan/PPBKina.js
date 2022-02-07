var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var dataFaktur;
var hdr_PPBId, mainBudidayaId, subBudidayaId, dtl_PPBId;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var rowNumber = 0;
var menuId = '4B2683DF-8242-40CA-9011-52B3B131DB82';
var kebunId;

function resetRowNumber(e) {
    rowNumber = 0;
    var jmlHarga = 0;
    var jmlGross = 0;
    var grid = $('#grdDetail').data('kendoGrid');
    var datasource = grid.dataSource.data();
    for (var i = 0; i < datasource.length; i++) {
        var model = datasource[i];
        model.JumlahHarga = hitungJumlah(model.DTL_PPBId);
        $('#jml' + model.DTL_PPBId).text(kendo.toString(model.JumlahHarga, 'n2'));
        jmlHarga = jmlHarga + model.JumlahHarga;
        jmlGross = jmlGross + model.Gross;
    }
    $('#jmlHarga').text(kendo.toString(jmlHarga, 'n2'));
    $('#jmlGross').text(kendo.toString(jmlGross, 'n2'));
}

function renderNumber(data) {
    var no = ++rowNumber;
    data.NoBaris = no;
    return no;
}

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
function hapusPenggunaPortalYangAktif()
{
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

angular.module("__header", ["kendo.directives"])
    .controller("MyCtrl", function ($scope) {
        $scope.validate = function (event) {
            event.preventDefault();
            if ($scope.validator.validate()) {
                simpanHeader().done(function (data) {
                    if (data) {
                        hdr_PPBId = $('#HDR_PPBId').val();
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
                                        $("#grdDetail").data("kendoGrid").dataSource.read({
                                            id: $('#HDR_PPBId').val()
                                        });
                                    }).fail(function (x) {
                                        alert('Error! Hubungi Bagian TI');
                                    });
                                }
                                else {
                                    $("#grdDetail").data("kendoGrid").dataSource.read({
                                        id: $('#HDR_PPBId').val()
                                    });
                                }
                            }).fail(function (x) {
                                alert('Error! Hubungi Bagian TI');
                            });
                        });

                    }
                    else {
                        openErrWindow();
                        $('#grdDetail').addClass('disabledbutton');
                        gridDestroy();
                    }

                }).fail(function () {
                    alert("Error! Hubungi Bagian TI");
                });  //edited
            } else {
                $(".k-button-icontext").addClass("k-state-disabled").removeClass("k-grid-add"); // edited
                gridDestroy();
            }
        }
    });

$(window).on("beforeunload", function () {
	return "Please don't leave me!";
});

$(window).on("unload", function () {
    hapusPenggunaPortalYangAktif();
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

function GetDataFrom() {
    // Hitung Jumlah Record di Detail
}

function getDataFrom() {
    // Hitung Jumlah Record di Detail
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_PPB]" +
                " where HDR_PPBId='" + $("#HDR_PPBId").val() + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function InsertGridFrom() {
    var grd = $('#grdDetail').data('kendoGrid');
    var url = '/Input/GetDataFrom';
    var lst = $('#ListFakturId').data('kendoMultiSelect').value();
    var str = "";
    for (var i = 0; i < lst.length; i++) {
        str = str + "'" + lst[i] + "'";
        if (i < lst.length - 1) str = str + ",";
    }

    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            strClassView: "Ptpn8.Penjualan.Models.View_DTLPPB",
            scriptSQL: "INSERT INTO [SPDK_KanpusEF].[dbo].[DTL_PPB] " +
                " SELECT newid() as DTL_PPBId, cast('" + $('#HDR_PPBId').val() + "' as uniqueidentifier) as HDR_PPBId," +
                " A.DTL_FakturId, A.SubBudidayaId, A.MerkId, A.Chop, A.QtySacks, " +
                " A.SatuanId, A.GradeId, A.Gross, A.Tare, A.Price, A.KK, A.KA, A.ALB,A.TahunProduksi, 0, '00000000-0000-0000-0000-000000000000' " +
                " from [SPDK_KanpusEF].[dbo].[DTL_Faktur] as A" +
                " inner join [ReferensiEF].[dbo].[KebunBudidaya] as B on A.MerkId=B.KebunBudidayaId" +
                " where HDR_FakturId IN (" + str + ") and B.KebunId='" + $('#KebunId').val() + "' and A.SudahPPB=0",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function ModelToView(_NomorInputView) {
    $('#HDR_PPBId').val(_NomorInputView.HDR_PPBId);
    $('#TahunBuku').val(_NomorInputView.TahunBuku);
    $('#NomorInput').val(_NomorInputView.NomorInput);
    $('#NomorInputView').val(_NomorInputView.NomorInputView);
    $('#No_PPB').val(_NomorInputView.No_PPB);
    $('#Tanggal_PPB').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.Tanggal_PPB.substr(6))));
    $('#KebunId').val(_NomorInputView.KebunId);
    $('#NamaKebun').val(_NomorInputView.NamaKebun);
    $('#BuyerId').val(_NomorInputView.BuyerId);
    $('#NamaBuyer').val(_NomorInputView.NamaBuyer);
    $('#MainBudidayaId').val(_NomorInputView.MainBudidayaId);
    $('#NamaMainBudidaya').val(_NomorInputView.NamaMainBudidaya);
    $('#AlamatKirim').val(_NomorInputView.AlamatKirim);
    $('#KotaKirim').val(_NomorInputView.KotaKirim);
    $('#PropinsiKirim').val(_NomorInputView.PropinsiKirim);
    $('#ContactPerson').val(_NomorInputView.ContactPerson);
    $('#VerKaur').val(_NomorInputView.VerKaur);
    $('#VerKabag').val(_NomorInputView.VerKabag);
    $('#TglVerKaur').val(new Date(parseInt(_NomorInputView.TglVerKaur.substr(6))));
    $('#TglVerKabag').val(new Date(parseInt(_NomorInputView.TglVerKabag.substr(6))));
    $('#UserNameKaur').val(_NomorInputView.UserNameKaur);
    $('#UserNameKabag').val(_NomorInputView.UserNameKabag);
    $('#No_SuratTugas').val(_NomorInputView.No_SuratTugas);

    kebunId = _NomorInputView.KebunId;
    var lst = $('#ListFakturId').data('kendoMultiSelect');
    lst.dataSource.read().done(function (data) {
        lst.value(_NomorInputView.ListFakturId);
        $('#ListNo_Faktur').val(_NomorInputView.ListNo_Faktur);
    });

    $('#Syarat1').val(_NomorInputView.Syarat1);
    $('#Syarat2').val(_NomorInputView.Syarat2);
    $('#Syarat3').val(_NomorInputView.Syarat3);
    $('#Catatan1').val(_NomorInputView.Catatan1);
    $('#Catatan2').val(_NomorInputView.Catatan2);
    $('#UserName').val(_NomorInputView.UserName);
    $('#Pembuat').val(_NomorInputView.Pembuat);
    $('#Pemeriksa').val(_NomorInputView.Pemeriksa);
    $('#Pejabat').val(_NomorInputView.Pejabat);
    $('#Catatan3').val(_NomorInputView.Catatan3);
    $('#Pengangkut1').val(_NomorInputView.Pengangkut1);
    $('#Pengangkut2').val(_NomorInputView.Pengangkut2);
    $('#Pengangkut3').val(_NomorInputView.Pengangkut3);
    $('#Pengangkut4').val(_NomorInputView.Pengangkut4);
    $('#Pengangkut5').val(_NomorInputView.Pengangkut5);
    $('#SudahKirimKeBGR').val(_NomorInputView.SudahKirimKeBGR);
}

function ViewToModel() {
    var listFakturId = $('#ListFakturId').val();
    if (listFakturId == null) listFakturId = "";
    var listNo_Faktur = $('#ListNo_Faktur').val();
    if (listNo_Faktur == null) listNo_Faktur = "";
    var mdl = {
        HDR_PPBId: $('#HDR_PPBId').val(),
        TahunBuku: $('#TahunBuku').val(),
        NomorInput: $('#NomorInput').val(),
        NomorInputView: Array(5 - String($('#NomorInput').val()).length + 1).join('0') + $('#NomorInput').val() + ' - ' + $('#No_PPB').val().toUpperCase(),
        No_PPB: $('#No_PPB').val().toUpperCase(),
        Tanggal_PPB: $('#Tanggal_PPB').val(),
        KebunId: $('#KebunId').val(),
        NamaKebun: $('#NamaKebun').val().toUpperCase(),
        BuyerId: $('#BuyerId').val(),
        NamaBuyer: $('#NamaBuyer').val().toUpperCase(),
        MainBudidayaId: $('#MainBudidayaId').val(),
        NamaMainBudidaya: $('#NamaMainBudidaya').val().toUpperCase(),
        AlamatKirim: $('#AlamatKirim').val(),
        KotaKirim: $('#KotaKirim').val(),
        PropinsiKirim: $('#PropinsiKirim').val(),
        ContactPerson: $('#ContactPerson').val(),
        ListFakturId: listFakturId.toString(),
        ListNo_Faktur: listNo_Faktur.toString(),
        Syarat1: $('#Syarat1').val().toUpperCase(),
        Syarat2: $('#Syarat2').val().toUpperCase(),
        Syarat3: $('#Syarat3').val().toUpperCase(),
        Catatan1: $('#Catatan1').val().toUpperCase(),
        Catatan2: $('#Catatan2').val().toUpperCase(),
        UserName: $('#UserName').val(),
        Pembuat: $('#Pembuat').val().toUpperCase(),
        Pemeriksa: $('#Pemeriksa').val().toUpperCase(),
        Pejabat: $('#Pejabat').val().toUpperCase(),
        VerKaur: $('#VerKaur').val(),
        VerKabag: $('#VerKabag').val(),
        TglVerKaur: $('#TglVerKaur').val(),
        TglVerKabag: $('#TglVerKabag').val(),
        UserNameKaur: $('#UserNameKaur').val(),
        UserNameKabag: $('#UserNameKabag').val(),
        No_SuratTugas: $('#No_SuratTugas').val(),
        Catatan3: $('#Catatan3').val().toUpperCase(),
        Pengangkut1: $('#Pengangkut1').val(),
        Pengangkut2: $('#Pengangkut2').val(),
        Pengangkut3: $('#Pengangkut3').val(),
        Pengangkut4: $('#Pengangkut4').val(),
        Pengangkut5: $('#Pengangkut5').val(),
        SudahKirimKeBGR: $('#SudahKirimKeBGR').val()
    };
    return mdl;
}

function hapusHeader() {
    wnd.close();
    PeriksaConstraintHeader().done(function (data) {
        if (data == 0) {
            ConfirmedHapusHeader().done(function (data) {
                if (data) {
                    hdr_PPBId = $('#HDR_PPBId').val();
                    $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                        disableHeader();
                        $('#btnDeleteHeader').removeClass('disabledbutton');
                        $('#btnPrintHeader').removeClass('disabledbutton');
                        $('#btnDeleteHeader').attr('disabled', false);
                        $('#btnPrintHeader').attr('disabled', false);
                        $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                        $('#grdDetail').removeClass('disabledbutton');
                        $("#grdDetail").data("kendoGrid").dataSource.read({
                            id: $('#HDR_PPBId').val()
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_PPB]" +
                " where HDR_PPBId='" + $("#HDR_PPBId").val() + "'",
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
        if (typeof (_NomorInputView.ListFakturId) == "string") {
            if (_NomorInputView.ListFakturId == "") {
                _NomorInputView.ListFakturId = [];
                _NomorInputView.ListNo_Faktur = [];
            }
            else {
                _NomorInputView.ListFakturId = _NomorInputView.ListFakturId.split(",");
                _NomorInputView.ListNo_Faktur = $.distinct(_NomorInputView.ListNo_Faktur.split(","));
            }
        }
        cekData(_NomorInputView.NomorInputView);
        ModelToView(_NomorInputView);
        hdr_PPBId = _NomorInputView.HDR_PPBId;
        mainBudidayaId = _NomorInputView.MainBudidayaId;
        $('#No_PPB').focus();
    }
}
$.extend({
    distinct: function (anArray) {
        var result = [];
        $.each(anArray, function (i, v) {
            if ($.inArray(v, result) == -1) result.push(v);
        });
        return result;
    }
});

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
            $('#msgInputView').text('Tidak dapat membuat data baru, dikarenakan sedang diedit oleh '+data);
        }).fail(function () {
            $('#msgInputView').text('Tidak dapat membuat data baru, dikarenakan sedang diedit oleh user lain.');
        });
    }
}
function cariUserYangLagiNgedit()
{
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
        parameters: { HDR_PPBId: $('#HDR_PPBId').val() }
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

function NamaBuyerOnSelect(e) {
    buyer = e.sender.dataItem();
    $('#NamaBuyer').val(buyer.Nama);
    $('#BuyerId').val(buyer.BuyerId);
    $('#ListFakturId').data('kendoMultiSelect').dataSource.read();

    //if(buyer.BuyerId==unilever)
    //{
    //	$('#KhususUnilever').css('display', 'inline-block');
    //}
    //else
    //{
    //	$('#KhususUnilever').css('display', 'none');
    //}
}

function NamaBuyerOnDataBound(e) {
}

function filterNomorInput(e) {
    var mdl = JSON.stringify(ViewToModel());
    return {
        _model: mdl,
        _menuId: menuId
    };
}

function filterBuyer() {
    return {
        value: $("#NamaBuyer").val(),
        mainBudidayaId: $("#MainBudidayaId").val()
    };
}
function filterCatatan() {
    return {
        key: "Catatan1",
        value: $("#Catatan1").val(),
        _menuId: menuId
    };
}

function filterSyarat() {
    return {
        key: "Syarat1",
        value: $("#Syarat1").val(),
        _menuId: menuId
    };
}

function filterFaktur() {
    var listId = [];
    for (var i = 0; i < _NomorInputView.ListFakturId.length; i++) {
        listId.push("'" + _NomorInputView.ListFakturId[i].toString() + "'");
    }

    if (headerBaru == true && kebunId != '00000000-0000-0000-0000-000000000000') {
        return {
            strClassView: 'Ptpn8.Penjualan.Models.HDR_Faktur',
            scriptSQL: "select DISTINCT A.* from [SPDK_KanpusEF].[dbo].[HDR_Faktur] as A" +
                " inner join [SPDK_KanpusEF].[dbo].[DTL_Faktur] as B on A.HDR_FakturId=B.HDR_FakturId" +
                " inner join [ReferensiEF].[dbo].[KebunBudidaya] as C on B.MerkId=C.KebunBudidayaId" +
                " where A.VerKaur=1 and A.TahunBuku=" + $('#TahunBuku').val() + " and A.BuyerId='" + $("#BuyerId").val() + "' and B.SudahPPB=0 and C.KebunId='" + $("#KebunId").val() + "'",
            _menuId: menuId
        }
    }
    else if (headerBaru == true && kebunId == '00000000-0000-0000-0000-000000000000') {
        return {
            strClassView: 'Ptpn8.Penjualan.Models.HDR_Faktur',
            scriptSQL: "select DISTINCT A.* from [SPDK_KanpusEF].[dbo].[HDR_Faktur] as A" +
                " inner join [SPDK_KanpusEF].[dbo].[DTL_Faktur] as B on A.HDR_FakturId=B.HDR_FakturId" +
                " inner join [ReferensiEF].[dbo].[KebunBudidaya] as C on B.MerkId=C.KebunBudidayaId" +
                " where A.VerKaur=1 and A.TahunBuku=" + $('#TahunBuku').val() + " and A.BuyerId='" + $("#BuyerId").val() + "' and B.SudahPPB=0",
            _menuId: menuId
        }
    }
    else if (headerBaru == false && kebunId != '00000000-0000-0000-0000-000000000000') {
        return {
            strClassView: 'Ptpn8.Penjualan.Models.HDR_Faktur',
            scriptSQL: "select DISTINCT A.* from [SPDK_KanpusEF].[dbo].[HDR_Faktur] as A" +
                " inner join [SPDK_KanpusEF].[dbo].[DTL_Faktur] as B on A.HDR_FakturId=B.HDR_FakturId" +
                " inner join [ReferensiEF].[dbo].[KebunBudidaya] as C on B.MerkId=C.KebunBudidayaId" +
                " where A.VerKaur=1 and A.TahunBuku=" + $('#TahunBuku').val() + " and A.BuyerId='" + $("#BuyerId").val() + "' and (B.SudahPPB=0 or A.HDR_FakturId in (" + listId + ")) and C.KebunId='" + $("#KebunId").val() + "'",
            _menuId: menuId
        }
    }
    else {
        return {
            strClassView: 'Ptpn8.Penjualan.Models.HDR_Faktur',
            scriptSQL: "select DISTINCT A.* from [SPDK_KanpusEF].[dbo].[HDR_Faktur] as A" +
                " inner join [SPDK_KanpusEF].[dbo].[DTL_Faktur] as B on A.HDR_FakturId=B.HDR_FakturId" +
                " inner join [ReferensiEF].[dbo].[KebunBudidaya] as C on B.MerkId=C.KebunBudidayaId" +
                " where A.VerKaur=1 and A.TahunBuku=" + $('#TahunBuku').val() + " and A.BuyerId='" + $("#BuyerId").val() + "' and (B.SudahPPB=0 or A.HDR_FakturId in (" + listId + "))",
            _menuId: menuId
        }

    }
}
function filterAlamatKirim() {
    return {
        key: "AlamatKirim",
        value: $("#AlamatKirim").val(),
        _menuId: menuId
    };
}

function filterKotaKirim() {
    return {
        key: "KotaKirim",
        value: $("#KotaKirim").val(),
        _menuId: menuId
    };
}

function filterPropinsiKirim() {
    return {
        key: "PropinsiKirim",
        value: $("#PropinsiKirim").val(),
        _menuId: menuId
    };
}

function filterKebun() {
    return {
        key: "NamaKebun",
        value: $("#NamaKebun").val()
    };
}

function NamaKebunOnSelect(e) {
    var kebun = this.dataItem(e.item);
    if (kebun != null) {
        $('#KebunId').val(kebun.KebunId);
        $('#NamaKebun').val(kebun.Nama);
        kebunId = kebun.KebunId;
    }
    else {
        $('#KebunId').val('00000000-0000-0000-0000-000000000000');
        kebunId = '00000000-0000-0000-0000-000000000000';
    }
    $('#ListFakturId').data('kendoMultiSelect').dataSource.read();
}

function ListFakturIdOnSelect(e) {
    var listFakturId = e.sender.dataItem();
    var arr = [];
    if ($('#ListFakturId').val() != null) {
        var lst = $('#ListFakturId').data('kendoMultiSelect');
        for (var i = 0; i < lst.dataItems().length; i++) {
            arr.push(lst.dataItems()[i].No_Faktur);
        }
    }
    arr.push(listFakturId.No_Faktur);
    $('#ListNo_Faktur').val(arr.toString());
}

function No_PPBOnSelect(e) {
    $('#ListFakturId').data('kendoMultiSelect').dataSource.read();
}

// Detail

function grdOnChange(e) {
}

function grdOnEdit(e) {
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.DTL_PPBId = model.uid;
        model.HDR_PPBId = hdr_PPBId;
    }
    dtl_PPBId = model.DTL_PPBId;
    mainBudidayaId = $('#MainBudidayaId').val();
    gridStatus = 'sudah-diapa-apain';
}
function rekapGross() {

    var grid = $('#grdDetail').data('kendoGrid');
    var newValue = 0;
    for (var i = 0; i < grid.dataItems().length; i++)
    {
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

function rekapJumlahHarga() {

    var grid = $('#grdDetail').data('kendoGrid');
    var newValue = 0;
    for (var i = 0; i < grid.dataItems().length; i++) {
        newValue += grid.dataItems()[i].JumlahHarga;
    }
    return newValue;
}

function hitungJumlah(id) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(id)) == "undefined" ? grid.dataSource.getByUid(id) : grid.dataSource.get(id);
    var newValue = dataItem.Price * (dataItem.Gross-dataItem.Tare);
    return newValue;
}

function grdOnDataBinding(e) {

}

function filterSubBudidaya(e) {
    return {
        mainBudidayaId: $('#MainBudidayaId').val(),
        value: $("#NamaSubBudidaya").val()
    };
}

function aucNamaSubBudidayaOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_PPBId)) == "undefined" ? grid.dataSource.getByUid(dtl_PPBId) : grid.dataSource.get(dtl_PPBId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.SubBudidayaId = ddlItem.SubBudidayaId;
    subBudidayaId = ddlItem.SubBudidayaId;
}

function aucNamaSubBudidayaOnDataBound(e) {

}

function filterMerk(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_PPBId)) == "undefined" ? grid.dataSource.getByUid(dtl_PPBId) : grid.dataSource.get(dtl_PPBId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_PPBId)) == "undefined" ? grid.dataSource.getByUid(dtl_PPBId) : grid.dataSource.get(dtl_PPBId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.MerkId = ddlItem.KebunBudidayaId;
}

function aucNamaMerkOnDataBound(e) {

}

function filterKebun(e) {
    return {
        value: $("#NamaKebun").val()
    };
}

function filterSatuan(e) {
    return {
        value: $("#NamaSatuan").val()
    };
}

function aucNamaSatuanOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_PPBId)) == "undefined" ? grid.dataSource.getByUid(dtl_PPBId) : grid.dataSource.get(dtl_PPBId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.SatuanId = ddlItem.SatuanId;
}

function aucNamaSatuanOnDataBound(e) {

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

function filterGrade(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_PPBId)) == "undefined" ? grid.dataSource.getByUid(dtl_PPBId) : grid.dataSource.get(dtl_PPBId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_PPBId)) == "undefined" ? grid.dataSource.getByUid(dtl_PPBId) : grid.dataSource.get(dtl_PPBId);
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
                alert("Error! Hubungi Bagian TI")
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

function filterContactPerson() {
    return {
        key: "ContactPerson",
        value: $("#ContactPerson").val(),
        _menuId: menuId
    };
}

function filterPengangkut1() {
    return {
        key: "Pengangkut1",
        value: $("#Pengangkut1").val(),
        _menuId: menuId
    };
}

function filterPengangkut2() {
    return {
        key: "Pengangkut2",
        value: $("#Pengangkut2").val(),
        _menuId: menuId
    };
}

function filterPengangkut3() {
    return {
        key: "Pengangkut3",
        value: $("#Pengangkut3").val(),
        _menuId: menuId
    };
}

function filterPengangkut4() {
    return {
        key: "Pengangkut4",
        value: $("#Pengangkut4").val(),
        _menuId: menuId
    };
}

function filterPengangkut5() {
    return {
        key: "Pengangkut5",
        value: $("#Pengangkut5").val(),
        _menuId: menuId
    };
}

function filterCatatan1() {
    return {
        key: "Catatan1",
        value: $("#Catatan1").val(),
        _menuId: menuId
    };
}

function filterCatatan2() {
    return {
        key: "Catatan2",
        value: $("#Catatan2").val(),
        _menuId: menuId
    };
}

function filterCatatan3() {
    return {
        key: "Catatan3",
        value: $("#Catatan1").val(),
        _menuId: menuId
    };
}

