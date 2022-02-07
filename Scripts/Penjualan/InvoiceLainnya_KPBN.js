var wndLeaveGrid, wnd, wndDetail, prtInvoice, prtPackingList, err, del;
var vessel;
var hdr_InvoiceId, mainBudidayaId, subBudidayaId, dtl_InvoiceId;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var rowNumber = 0;
var menuId = 'bf821e6f-2dd8-9b83-706e-d68142249975';

function resetRowNumber(e) {
    rowNumber = 0;
    var jmlHarga = 0;
    var jmlGross = 0;
    var grid = $('#grdDetail').data('kendoGrid');
    var datasource = grid.dataSource.data();
    for (var i = 0; i < datasource.length; i++) {
        var model = datasource[i];
        model.JumlahHarga = hitungJumlah(model.DTL_InvoiceId);
        $('#jml' + model.DTL_InvoiceId).text(kendo.toString(model.JumlahHarga, 'n2'));
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
	var h1 = parseInt($(".content-header").css("height"));
	    var h2 = parseInt($(".hdr").css("height")) + parseInt($("._headerlainnya").css("height"));
	    var h3 = parseInt($(".k-grid-toolbar").css("height"));
	    var h4 = parseInt($(".k-grid-header-wrap").css("height"));
	    var h5 = parseInt($(".__footer").css("height")) + 20;
	    if ((window.screen.height - (h1 + h2 + h3 + h4 + h5))>=400)
        $("#grdDetail").css("height", window.screen.height - (h1 + h2 + h3 + h4 + h5));
    else
        $("#grdDetail").css("height", 500);
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

    prtInvoice = $("#printWindowInvoice").kendoWindow({
        title: "Print",
        modal: true,
        visible: false,
        resizable: false,
        width: 800,
        height: 500
    }).data("kendoWindow");

    prtPackingList = $("#printWindowPackingList").kendoWindow({
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
                    if (data[0]) {
                        hdr_InvoiceId = $('#HDR_InvoiceId').val();
                        $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                            disableHeader();
                            $('#btnDeleteHeader').removeClass('disabledbutton');
                            $('#btnPrintInvoice').removeClass('disabledbutton');
                            $('#btnPrintPackingList').removeClass('disabledbutton');
                            $('#btnDeleteHeader').attr('disabled', false);
                            $('#btnPrintInvoice').attr('disabled', false);
                            $('#btnPrintPackingList').attr('disabled', false);
                            $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                            $('#grdDetail').removeClass('disabledbutton');
                            getDataFrom().done(function (data) {
                                if (data == 0) {
                                    InsertGridFrom().done(function (data) {
                                        $("#grdDetail").data("kendoGrid").dataSource.read({ id: $('#HDR_InvoiceId').val() });
                                    }).fail(function (x) {
                                        alert('Error! Hubungi Bagian TI');
                                    });
                                }
                                else {
                                    $("#grdDetail").data("kendoGrid").dataSource.read({ id: $('#HDR_InvoiceId').val() });
                                }
                            }).fail(function (x) {
                                alert('Error! Hubungi Bagian TI');
                            });
                        });

                    }
                    else {
                        openErrWindow(data[1]);
                        $('#grdDetail').addClass('disabledbutton');
                        gridDestroy();
                    }
                }).fail(function (x) {
                    alert("Error! Hubungi Bagian TI");
                });
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
}

function getDataFrom() {
    // Hitung Jumlah Record di Detail
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_Invoice]" +
                " where HDR_InvoiceId='" + $("#HDR_InvoiceId").val() + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}
function InsertGridFrom() {
    var grd = $('#grdDetail').data('kendoGrid');
    var url = '/Input/GetDataFrom';
    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            strClassView: "Ptpn8.Penjualan.Models.View_DTLInvoice",
            scriptSQL: "set dateformat DMY INSERT INTO [SPDK_KanpusEF].[dbo].[DTL_Invoice] SELECT newid() as DTL_InvoiceId, cast('" + $('#HDR_InvoiceId').val() + "' as uniqueidentifier) as HDR_InvoiceId," +
                " A.SubBudidayaId, A.LineNumber, A.MerkId, A.Chop, A.QtySacks, " +
                " A.SatuanId, A.GradeId, A.Gross, A.Tare, A.Price, A.No_SC, A.Shipping_Mark, A.DTL_ShippingInstructionId, A.KK, A.KA, A.ALB,A.TahunProduksi  " +
                " from [SPDK_KanpusEF].[dbo].[DTL_ShippingInstruction] as A" +
                " where HDR_ShippingInstructionId='" + $("#HDR_ShippingInstructionId").val() + "'",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function ModelToView(_NomorInputView) {
    $('#HDR_InvoiceId').val(_NomorInputView.HDR_InvoiceId);
    $('#TahunBuku').val(_NomorInputView.TahunBuku);
    //$('#MainBudidayaId').val(_NomorInputView.MainBudidayaId);
    //$('#NamaMainBudidaya').val(_NomorInputView.NamaMainBudidaya);
    $('#NomorInput').val(_NomorInputView.NomorInput);
    $('#NomorInputView').val(_NomorInputView.NomorInputView);
    $('#No_Invoice').val(_NomorInputView.No_Invoice);
    $('#Tanggal_Invoice').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.Tanggal_Invoice.substr(6))));
    $('#HDR_ShippingInstructionId').val(_NomorInputView.HDR_ShippingInstructionId);
    $('#No_SI').val(_NomorInputView.No_SI);
    var tanggal_SI = new Date(parseInt(_NomorInputView.Tanggal_SI.substr(6)));
    $('#Tanggal_SI').val(tanggal_SI.getDate() + "/" + (tanggal_SI.getMonth() + 1) + "/" + tanggal_SI.getFullYear());
    $('#Kota_Destination').val(_NomorInputView.Kota_Destination);
    $('#Negara_Destination').val(_NomorInputView.Negara_Destination);
    $('#perMS').val(_NomorInputView.perMS);
    $('#Connecting_Vessel').val(_NomorInputView.Connecting_Vessel);
    $('#Consignee1').val(_NomorInputView.Consignee1);
    $('#Consignee2').val(_NomorInputView.Consignee2);
    $('#Consignee3').val(_NomorInputView.Consignee3);
    $('#Consignee4').val(_NomorInputView.Consignee4);
    $('#Consignee5').val(_NomorInputView.Consignee5);
    $('#EORI_Number').val(_NomorInputView.EORI_Number);
    $('#No_PO').val(_NomorInputView.No_PO);
    $('#BrokerId').val(_NomorInputView.BrokerId);
    $('#NamaBroker').val(_NomorInputView.NamaBroker);
    $('#No_BL').val(_NomorInputView.No_BL);
    $('#No_ContainerSeal').val(_NomorInputView.No_ContainerSeal);
    $('#UserName').val(_NomorInputView.UserName);
    $('#Pejabat').val(_NomorInputView.Pejabat);
    $('#VerKaur').val(_NomorInputView.VerKaur);
    $('#VerKabag').val(_NomorInputView.VerKabag);
    var tglVerKaur = new Date(parseInt(_NomorInputView.TglVerKaur.substr(6)));
    var tglVerKabag = new Date(parseInt(_NomorInputView.TglVerKabag.substr(6)));
    $('#TglVerKaur').val(tglVerKaur.getDate() + "/" + (tglVerKaur.getMonth() + 1) + "/" + tglVerKaur.getFullYear());
    $('#TglVerKabag').val(tglVerKabag.getDate() + "/" + (tglVerKabag.getMonth() + 1) + "/" + tglVerKabag.getFullYear());
    $('#UserNameKaur').val(_NomorInputView.UserNameKaur);
    $('#UserNameKabag').val(_NomorInputView.UserNameKabag);
    $('#No_SuratTugas').val(_NomorInputView.No_SuratTugas);
    $('#Notes').val(_NomorInputView.Notes);
	$('#OceanFreightCost').data('kendoNumericTextBox').value(_NomorInputView.OceanFreightCost);
	$('#InsuranceCost').data('kendoNumericTextBox').value(_NomorInputView.InsuranceCost);
	$('#StatusPenjualan').val(_NomorInputView.StatusPenjualan);

}

function ViewToModel() {
    var mdl = {
        HDR_InvoiceId: $('#HDR_InvoiceId').val(),
        TahunBuku: $('#TahunBuku').val(),
        MainBudidayaId: $('#MainBudidayaId').val(),
        NamaMainBudidaya: $('#NamaMainBudidaya').val().toUpperCase(),
        NomorInput: $('#NomorInput').val(),
        NomorInputView: Array(5 - String($('#NomorInput').val()).length + 1).join('0') + $('#NomorInput').val() + ' - ' + $('#No_Invoice').val().toUpperCase(),
        No_Invoice: $('#No_Invoice').val().toUpperCase(),
        Tanggal_Invoice: $('#Tanggal_Invoice').val(),
        HDR_ShippingInstructionId: $('#HDR_ShippingInstructionId').val(),
        No_SI: $('#No_SI').val().toUpperCase(),
        Tanggal_SI: $('#Tanggal_SI').val(),
        Kota_Destination: $('#Kota_Destination').val().toUpperCase(),
        Negara_Destination: $('#Negara_Destination').val().toUpperCase(),
        perMS: $('#perMS').val().toUpperCase(),
        Connecting_Vessel: $('#Connecting_Vessel').val().toUpperCase(),
        Consignee1: $('#Consignee1').val().toUpperCase(),
        Consignee2: $('#Consignee2').val().toUpperCase(),
        Consignee3: $('#Consignee3').val().toUpperCase(),
        Consignee4: $('#Consignee4').val().toUpperCase(),
        Consignee5: $('#Consignee5').val().toUpperCase(),
        EORI_Number: $('#EORI_Number').val().toUpperCase(),
        No_PO: $('#No_PO').val().toUpperCase(),
        BrokerId: $('#BrokerId').val().toUpperCase(),
        NamaBroker: $('#NamaBroker').val().toUpperCase(),
        No_BL: $('#No_BL').val().toUpperCase(),
        No_ContainerSeal: $('#No_ContainerSeal').val(),
        UserName: $('#UserName').val(),
        Pejabat: $('#Pejabat').val(),
        VerKaur: $('#VerKaur').val(),
        VerKabag: $('#VerKabag').val(),
        TglVerKaur: $('#TglVerKaur').val(),
        TglVerKabag: $('#TglVerKabag').val(),
        UserNameKaur: $('#UserNameKaur').val(),
        UserNameKabag: $('#UserNameKabag').val(),
        No_SuratTugas: $('#No_SuratTugas').val(),
        Notes: $('#Notes').val(),
		OceanFreightCost:$('#OceanFreightCost').val(),
		InsuranceCost:$('#InsuranceCost').val(),
		StatusPenjualan:$('#StatusPenjualan').val()
    };
    return mdl;
}

function hapusHeader() {
    wnd.close();
    PeriksaConstraintHeader().done(function (data) {
        if (data == 0) {
            ConfirmedHapusHeader().done(function (data) {
                if (data) {
                    hdr_InvoiceId = $('#HDR_InvoiceId').val();
                    $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                        disableHeader();
                        $('#btnDeleteHeader').removeClass('disabledbutton');
                        $('#btnPrintHeader').removeClass('disabledbutton');
                        $('#btnDeleteHeader').attr('disabled', false);
                        $('#btnPrintHeader').attr('disabled', false);
                        $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                        $('#grdDetail').removeClass('disabledbutton');
                        $("#grdDetail").data("kendoGrid").dataSource.read({
                            id: $('#HDR_InvoiceId').val()
                        });
                    });
                }
                else {
                    openErrWindow();
                    $('#grdDetail').addClass('disabledbutton');
                    gridDestroy();
                }

            }).fail(function (x) {
                alert('Error! Hubungi Bagin TI');
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_Invoice]" +
                " where HDR_InvoiceId='" + $("#HDR_InvoiceId").val() + "'",
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
        hdr_InvoiceId = _NomorInputView.HDR_InvoiceId;
        mainBudidayaId = _NomorInputView.MainBudidayaId;
        cekData(_NomorInputView.NomorInputView);
        $('#No_SI').focus();
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

function btnPrintInvoiceOnClick() {
    var viewerInvoice = $("#reportViewerInvoice").data("telerik_ReportViewer");
    viewerInvoice.reportSource({
        report: viewerInvoice.reportSource().report,
        parameters: { HDR_InvoiceId: $('#HDR_InvoiceId').val() }
    });
    viewerInvoice.refreshReport();
    prtInvoice.open().center();
}

function btnPrintPackingListOnClick() {
    var viewerPackingList = $("#reportViewerPackingList").data("telerik_ReportViewer");
    viewerPackingList.reportSource({
        report: viewerPackingList.reportSource().report,
        parameters: { HDR_InvoiceId: $('#HDR_InvoiceId').val() }
    });
    viewerPackingList.refreshReport();
    prtPackingList.open().center();
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
    $('#btnPrintInvoice').addClass('disabledbutton');
    $('#btnPrintPackingList').addClass('disabledbutton');
    $('#grdDetail').addClass('disabledbutton');
}

function enableHeader() {
    $("._key").find('span').removeClass('disabledbutton');
    $("._key").removeClass('disabledbutton');
    $("._nonkey").find('span').removeClass('disabledbutton');
    $("._nonkey").removeClass('disabledbutton');
    $('#btnSubmitHeader').removeClass('disabledbutton');
    $('#btnDeleteHeader').removeClass('disabledbutton');
    $('#btnPrintInvoice').removeClass('disabledbutton');
    $('#btnPrintPackingList').removeClass('disabledbutton');
}

function cekData(nomorInputView) {
    if (nomorInputView.indexOf("Data Baru") > -1)   // Data Baru
    {
        headerBaru = true;
        enableHeader();
        $('#btnSubmitHeader').removeClass('disabledbutton');
        $('#btnDeleteHeader').removeClass('disabledbutton');
        $('#btnPrintInvoice').removeClass('disabledbutton');
        $('#btnPrintPackingList').removeClass('disabledbutton');
        $('#btnSubmitHeader').attr('disabled', false);
        $('#btnDeleteHeader').attr('disabled', true);
        $('#btnPrintInvoice').attr('disabled', true);
        $('#btnPrintPackingList').attr('disabled', true);
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
        $('#btnPrintInvoice').removeClass('disabledbutton');
        $('#btnPrintPackingList').removeClass('disabledbutton');
        $('#btnSubmitHeader').attr('disabled', false);
        $('#btnDeleteHeader').attr('disabled', false);
        $('#btnPrintInvoice').attr('disabled', false);
        $('#btnPrintPackingList').attr('disabled', false);
        $('#grdDetail').addClass('disabledbutton');
        gridDestroy();
    }
}

function NamaVesselOnSelect(e) {
    vessel = this.dataItem(e.item);
    $('#NamaVessel').val(vessel.Nama);
}

function NamaVesselOnDataBound(e) {
}

function filterNomorInput(e) {
    var mdl = JSON.stringify(ViewToModel());
    return {
        _model: mdl,
        _menuId: menuId
    };
}

function filterNo_SI(e) {
    return {
        strClassView: "Ptpn8.Penjualan.Models.View_HDRShippingInstruction",
        scriptSQL: "set dateformat dmy SELECT DISTINCT A.HDR_ShippingInstructionId, A.No_SI, A.Tanggal_SI, A.Kota_Destination, A.Negara_Destination, " +
            "A.Consignee1, A.Consignee2, A.Consignee3, A.Consignee4, A.Consignee5, A.BrokerId, B.Nama as NamaBroker, A.No_PO, A.Pejabat, A.Notes, A.OceanFreightCost, A.InsuranceCost" +
            " from [SPDK_KanpusEF].[dbo].[HDR_ShippingInstruction] as A left join [ReferensiEF].[dbo].[Broker] as B " +
            " on A.BrokerId=B.BrokerId inner join [SPDK_KanpusEF].[dbo].[DTL_ShippingInstruction] as C on A.HDR_ShippingInstructionId=C.HDR_ShippingInstructionId" +
            " where TahunBuku=" + $("#TahunBuku").val() + " and MainBudidayaId='" + $("#MainBudidayaId").val() + "' and No_SI LIKE '%" + $("#No_SI").val() + "%'" +
            " and (C.SudahInvoice=0 or A.HDR_ShippingInstructionId='"+$('#HDR_ShippingInstructionId').val()+"')  and A.VerKaur=1",
        _menuId: menuId
    };
}

function filterVessel() {
    return { value: $("#NamaVessel").val() };
}

function filterConsignee() {
    return { value: $("#Consignee1").val() };
}

function filterNotifyAddress() {
    return { value: $("#NotifyAddress1").val() };
}

function filterKota_Destination() {
    return {
        key: "Kota_Destination",
        value: $("#Kota_Destination").val(),
        _menuId: menuId
    };
}

function filterNegara_Destination() {
    return {
        key: "Negara_Destination",
        value: $("#Negara_Destination").val(),
        _menuId: menuId
    };
}

function filterBroker() {
    return { value: $("#NamaBroker").val() };
}

function NamaBrokerOnSelect(e) {
    var broker = this.dataItem(e.item);
    $('#BrokerId').val(broker.BrokerId);
}

function Consignee1OnSelect(e) {
    var consignee = this.dataItem(e.item);
    $('#Consignee1').val(consignee.Nama);
    $('#Consignee2').val(consignee.Alamat);
    $('#Consignee3').val(consignee.Alamat1);
    $('#Consignee4').val(consignee.Kota + ', ' + consignee.Propinsi + '-' + consignee.Negara);
    $('#Consignee5').val('Telp: ' + consignee.Telepon + ', Fax: ' + consignee.Faksimili);
}


function No_SIOnChange(e) {
    if (typeof (e.item) != 'undefined')
        _NomorInputView = this.dataItem(e.item);
    else
        _NomorInputView = e.sender.dataItem();
    $('#No_SI').val(_NomorInputView.No_SI);
    var tanggal_SI = new Date(parseInt(_NomorInputView.Tanggal_SI.substr(6)));
    $('#Tanggal_SI').val(tanggal_SI.getDate() + "/" + (tanggal_SI.getMonth() + 1) + "/" + tanggal_SI.getFullYear())
    $('#HDR_ShippingInstructionId').val(_NomorInputView.HDR_ShippingInstructionId);
    $('#Kota_Destination').val(_NomorInputView.Kota_Destination);
    $('#Negara_Destination').val(_NomorInputView.Negara_Destination);
    $('#Consignee1').val(_NomorInputView.Consignee1);
    $('#Consignee2').val(_NomorInputView.Consignee2);
    $('#Consignee3').val(_NomorInputView.Consignee3);
    $('#Consignee4').val(_NomorInputView.Consignee4);
    $('#Consignee5').val(_NomorInputView.Consignee5);
    $('#BrokerId').val(_NomorInputView.BrokerId);
    $('#NamaBroker').val(_NomorInputView.NamaBroker);
    $('#No_PO').val(_NomorInputView.No_PO);
    $('#Pejabat').val(_NomorInputView.Pejabat);
    $('#Notes').val(_NomorInputView.Notes);
	$('#OceanFreightCost').data('kendoNumericTextBox').value(_NomorInputView.OceanFreightCost);
	$('#InsuranceCost').data('kendoNumericTextBox').value(_NomorInputView.InsuranceCost);
}

function No_SIOnSelect(e) {
    var _NomorInputView = this.dataItem(e.item)
    $('#No_SI').val(_NomorInputView.No_SI);
    $('#Tanggal_SI').val(_NomorInputView.Tanggal_SI);
    $('#HDR_ShippingInstructionId').val(_NomorInputView.HDR_ShippingInstructionId);
    $('#Kota_Destination').val(_NomorInputView.Kota_Destination);
    $('#Negara_Destination').val(_NomorInputView.Negara_Destination);
    $('#Consignee1').val(_NomorInputView.Consignee1);
    $('#Consignee2').val(_NomorInputView.Consignee2);
    $('#Consignee3').val(_NomorInputView.Consignee3);
    $('#Consignee4').val(_NomorInputView.Consignee4);
    $('#Consignee5').val(_NomorInputView.Consignee5);
    $('#BrokerId').val(_NomorInputView.BrokerId);
    $('#NamaBroker').val(_NomorInputView.NamaBroker);
    $('#No_PO').val(_NomorInputView.No_PO);
    $('#Pejabat').val(_NomorInputView.Pejabat);
    $('#Notes').val(_NomorInputView.Notes);
	$('#OceanFreightCost').data('kendoNumericTextBox').value(_NomorInputView.OceanFreightCost);
	$('#InsuranceCost').data('kendoNumericTextBox').value(_NomorInputView.InsuranceCost);
}

function No_SIOnDataBound(e) {
}

// Detail

function grdOnChange(e) {
}

function grdOnEdit(e) {
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.DTL_InvoiceId = model.uid;
        model.HDR_InvoiceId = hdr_InvoiceId;
    }
    dtl_InvoiceId = model.DTL_InvoiceId;
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
    var newValue = 0;
    newValue = (dataItem.Price/100) * (dataItem.Gross-dataItem.Tare);
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
    var dataItem = typeof (grid.dataSource.get(dtl_InvoiceId)) == "undefined" ? grid.dataSource.getByUid(dtl_InvoiceId) : grid.dataSource.get(dtl_InvoiceId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.SubBudidayaId = ddlItem.SubBudidayaId;
    subBudidayaId = ddlItem.SubBudidayaId;
}

function aucNamaSubBudidayaOnDataBound(e) {

}

function filterMerk(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_InvoiceId)) == "undefined" ? grid.dataSource.getByUid(dtl_InvoiceId) : grid.dataSource.get(dtl_InvoiceId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_InvoiceId)) == "undefined" ? grid.dataSource.getByUid(dtl_InvoiceId) : grid.dataSource.get(dtl_InvoiceId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.MerkId = ddlItem.KebunBudidayaId;
}

function aucNamaMerkOnDataBound(e) {

}

function filterSatuan(e) {
    return {
        value: $("#NamaSatuan").val()
    };
}

function aucNamaSatuanOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_InvoiceId)) == "undefined" ? grid.dataSource.getByUid(dtl_InvoiceId) : grid.dataSource.get(dtl_InvoiceId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.SatuanId = ddlItem.SatuanId;
}

function aucNamaSatuanOnDataBound(e) {

}

function filterGridCreate(e) {
    return {
        _model: JSON.stringify(e),
        _menuId: menuId
    };
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
    var dataItem = typeof (grid.dataSource.get(dtl_InvoiceId)) == "undefined" ? grid.dataSource.getByUid(dtl_InvoiceId) : grid.dataSource.get(dtl_InvoiceId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_InvoiceId)) == "undefined" ? grid.dataSource.getByUid(dtl_InvoiceId) : grid.dataSource.get(dtl_InvoiceId);
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
                alert("Error! Hubungi Bagian TI");
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

function filterBudidaya(e) {
    return {
        key: "NamaMainBudidaya",
        value: $("#NamaMainBudidaya").val()
    };
}

function BudidayaOnSelect(e) {
    if (gridStatus == 'sudah-nyoba-disimpan-tapi-model-masih-salah' ||
        gridStatus == 'sudah-diapa-apain') {
        openWindowLeaveGrid(e);
    }
    else {
        _NomorInputView = this.dataItem(e.item);
        mainBudidayaId = _NomorInputView.MainBudidayaId;
        $('#MainBudidayaId').val(_NomorInputView.MainBudidayaId);
    }
}
function filterdetailRead(e) {
    return { _menuId: menuId };
}

function filterdetailCreate(e) {
    return { _menuId: menuId };
}
