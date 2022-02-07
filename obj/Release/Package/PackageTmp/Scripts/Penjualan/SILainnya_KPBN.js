var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var vessel;
var hdr_ShippingInstructionId, mainBudidayaId, subBudidayaId, dtl_ShippingInstructionId;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var gridStatus;
var rowNumber = 0;
var menuId = 'c276fe44-1e9a-5cab-549f-6d8eccd54886';

function resetRowNumber(e) {
    rowNumber = 0;
    var jmlHarga = 0;
    var jmlGross = 0;
    var grid = $('#grdDetail').data('kendoGrid');
    var datasource = grid.dataSource.data();
    for (var i = 0; i < datasource.length; i++) {
        var model = datasource[i];
        model.JumlahHarga = hitungJumlah(model.DTL_ShippingInstructionId);
        $('#jml' + model.DTL_ShippingInstructionId).text(kendo.toString(model.JumlahHarga, 'n2'));
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
            chopvalidation: function (input) {
                if (input.attr("name") == "Chop" && input.val() != "") {
                    input.attr("data-chopvalidation-msg", "Chop Sudah Ada!")
                    cekNoChop(input).done(function (r) {
                        if (r == 0) {
                            return true;
                        } else {
                            for (var i = 0; i < r.length; i++) {
                                if (r[i][0] != dtl_ShippingInstructionId) {
                                    $('#errMsg').text('No Chop Sudah Digunakan!');
                                    openErrWindow();
                                    var grid = $('#grdDetail').data('kendoGrid');
                                    var dataItem = typeof (grid.dataSource.get(dtl_ShippingInstructionId)) == "undefined" ? grid.dataSource.getByUid(dtl_ShippingInstructionId) : grid.dataSource.get(dtl_ShippingInstructionId);
                                    dataItem.Chop = '';
                                    return false;
                                }
                            }
                            return true;
                        }
                    })
                    .fail(function (x) {
                        return false;
                    });
                } else if (input.attr("name") == "NamaSubBudidaya" && input.val() != "") {
                    cekNamaSubBudidaya(input.val()).done(function (data) {
                        if (data.length > 0) {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_ShippingInstructionId)) == "undefined" ? grid.dataSource.getByUid(dtl_ShippingInstructionId) : grid.dataSource.get(dtl_ShippingInstructionId);
                            dataItem.SubBudidayaId = data[0][0];
                            subBudidayaId = data[0][0];
                        }
                    });
                } else if (input.attr("name") == "NamaMerk" && input.val() != "") {
                    cekNamaMerk(input.val()).done(function (data) {
                        if (data.length > 0) {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_ShippingInstructionId)) == "undefined" ? grid.dataSource.getByUid(dtl_ShippingInstructionId) : grid.dataSource.get(dtl_ShippingInstructionId);
                            dataItem.MerkId = data[0][0];
                        }
                    });
                } else if (input.attr("name") == "NamaGrade" && input.val() != "") {
                    cekNamaGrade(input.val()).done(function (data) {
                        if (data.length > 0) {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_ShippingInstructionId)) == "undefined" ? grid.dataSource.getByUid(dtl_ShippingInstructionId) : grid.dataSource.get(dtl_ShippingInstructionId);
                            dataItem.GradeId = data[0][0];
                            if (dataItem.QtySacks == 0) {
                                dataItem.QtySacks = data[0][1];
                                dataItem.Gross = (data[0][1] * data[0][2]) + data[0][3];
                                dataItem.Tare = data[0][3];
                                $('#QtySacks').val(data[0][1]);
                            }
                        }
                    });
                } else if (input.attr("name") == "NamaSatuan" && input.val() != "") {
                    cekNamaSatuan(input.val()).done(function (data) {
                        if (data.length > 0) {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_ShippingInstructionId)) == "undefined" ? grid.dataSource.getByUid(dtl_ShippingInstructionId) : grid.dataSource.get(dtl_ShippingInstructionId);
                            dataItem.SatuanId = data[0][0];
                        }
                    });
                } else if (input.attr("name") == "QtySacks" && input.val() != "") {
                    ambilStandarGrade().done(function (r) {
                        if (r == 0) {
                            return true;
                        } else {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_ShippingInstructionId)) == "undefined" ? grid.dataSource.getByUid(dtl_ShippingInstructionId) : grid.dataSource.get(dtl_ShippingInstructionId);
                            dataItem.Tare = (input.val() / r[0][0]) * r[0][2];
                            dataItem.Gross = (r[0][1] * input.val()) + dataItem.Tare;
                            return true;
                        }
                    })
                    .fail(function (x) {
                        return false;
                    });
                } else {
                    return true;
                };
            }
        }
    });
    //////// sampe sini

});

$(window).on("beforeunload", function () {
	return "Please don't leave me!";
});

$(window).on("unload", function () {
    hapusPenggunaPortalYangAktif();
});


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
                        hdr_ShippingInstructionId = $('#HDR_ShippingInstructionId').val();
                        $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                            disableHeader();
                            $('#btnDeleteHeader').removeClass('disabledbutton');
                            $('#btnPrintHeader').removeClass('disabledbutton');
                            $('#btnDeleteHeader').attr('disabled', false);
                            $('#btnPrintHeader').attr('disabled', false);
                            $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                            $('#grdDetail').removeClass('disabledbutton');

                            $("#grdDetail").data("kendoGrid").dataSource.read({
                                id: $('#HDR_ShippingInstructionId').val()
                            });
                        });
                    }
                    else {
                        openErrWindow();
                        $('#grdDetail').addClass('disabledbutton');
                        gridDestroy();
                    }

                }).fail(function (x) {
                    alert("Error! Hubungi Bagian TI");
                });  //edited
            } else {
                $(".k-button-icontext").addClass("k-state-disabled").removeClass("k-grid-add"); // edited
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
        PeriksaConstraintDetail(row).done(function (data) {
            if (data == 0) {
                $('#grdDetail').data('kendoGrid').removeRow(row);
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
    $('#HDR_ShippingInstructionId').val(_NomorInputView.HDR_ShippingInstructionId);
    $('#TahunBuku').val(_NomorInputView.TahunBuku);
    $('#NomorInput').val(_NomorInputView.NomorInput);
    $('#NomorInputView').val(_NomorInputView.NomorInputView);
    $('#No_SI').val(_NomorInputView.No_SI);
    $('#Tanggal_SI').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.Tanggal_SI.substr(6))));
    $('#NamaVessel').val(_NomorInputView.NamaVessel);
    $('#Kota_Loading').val(_NomorInputView.Kota_Loading);
    $('#Kota_Destination').val(_NomorInputView.Kota_Destination);
    $('#Negara_Destination').val(_NomorInputView.Negara_Destination);
    $('#JumlahContainer').val(_NomorInputView.JumlahContainer);
    $('#UraianContainer').data('kendoDropDownList').value(_NomorInputView.UraianContainer);
    $('#SatuanContainer').data('kendoDropDownList').value(_NomorInputView.SatuanContainer);
    $('#ETD').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.ETD.substr(6))));
    $('#ETA').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.ETA.substr(6))));
    $('#Consignee1').val(_NomorInputView.Consignee1);
    $('#Consignee2').val(_NomorInputView.Consignee2);
    $('#Consignee3').val(_NomorInputView.Consignee3);
    $('#Consignee4').val(_NomorInputView.Consignee4);
    $('#Consignee5').val(_NomorInputView.Consignee5);
    $('#NotifyAddress1').val(_NomorInputView.NotifyAddress1);
    $('#NotifyAddress2').val(_NomorInputView.NotifyAddress2);
    $('#NotifyAddress3').val(_NomorInputView.NotifyAddress3);
    $('#NotifyAddress4').val(_NomorInputView.NotifyAddress4);
    $('#NotifyAddress5').val(_NomorInputView.NotifyAddress5);
    $('#No_PO').val(_NomorInputView.No_PO);
    $('#BrokerId').val(_NomorInputView.BrokerId);
    $('#NamaBroker').val(_NomorInputView.NamaBroker);
    $('#Remarks_No_LC').val(_NomorInputView.Remarks_No_LC);
    $('#Remarks_Bank').val(_NomorInputView.Remarks_Bank);
    $('#Remarks_Shipment').val(_NomorInputView.Remarks_Shipment);
    $('#WeightList_Sets').val(_NomorInputView.WeightList_Sets);
    $('#WeightList_Copies').val(_NomorInputView.WeightList_Copies);
    $('#Certificate_Sets').val(_NomorInputView.Certificate_Sets);
    $('#Certificate_Copies').val(_NomorInputView.Certificate_Copies);
    $('#BL_Sets').val(_NomorInputView.BL_Sets);
    $('#BL_Signed').val(_NomorInputView.BL_Signed);
    $('#BL_Copies').val(_NomorInputView.BL_Copies);
    $('#Enclosures_Sets').val(_NomorInputView.Enclosures_Sets);
    $('#UserName').val(_NomorInputView.UserName);
    //$('#MainBudidayaId').val(_NomorInputView.MainBudidayaId);
    //$('#NamaMainBudidaya').val(_NomorInputView.NamaMainBudidaya);
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
	$('#SudahKirimKeBGR').val(_NomorInputView.SudahKirimKeBGR);
	$('#ProsesStuffing').data('kendoDropDownList').value(_NomorInputView.ProsesStuffing);
	$('#StatusPenjualan').val(_NomorInputView.StatusPenjualan);
}

function ViewToModel() {
    var mdl = {
        HDR_ShippingInstructionId: $('#HDR_ShippingInstructionId').val(),
        TahunBuku: $('#TahunBuku').val(),
        NomorInput: $('#NomorInput').val(),
        NomorInputView: Array(5 - String($('#NomorInput').val()).length + 1).join('0') + $('#NomorInput').val() + ' - ' + $('#No_SI').val().toUpperCase(),
        No_SI: $('#No_SI').val().toUpperCase(),
        Tanggal_SI: $('#Tanggal_SI').val(),
        NamaVessel: $('#NamaVessel').val().toUpperCase(),
        Kota_Loading: $('#Kota_Loading').val().toUpperCase(),
        Kota_Destination: $('#Kota_Destination').val().toUpperCase(),
        Negara_Destination: $('#Negara_Destination').val().toUpperCase(),
        JumlahContainer: $('#JumlahContainer').val(),
        UraianContainer: $('#UraianContainer').data('kendoDropDownList').value(),
        SatuanContainer: $('#SatuanContainer').data('kendoDropDownList').value(),
        ETD: $('#ETD').val(),
        ETA: $('#ETA').val(),
        Consignee1: $('#Consignee1').val().toUpperCase(),
        Consignee2: $('#Consignee2').val(),
        Consignee3: $('#Consignee3').val(),
        Consignee4: $('#Consignee4').val(),
        Consignee5: $('#Consignee5').val(),
        NotifyAddress1: $('#NotifyAddress1').val().toUpperCase(),
        NotifyAddress2: $('#NotifyAddress2').val(),
        NotifyAddress3: $('#NotifyAddress3').val(),
        NotifyAddress4: $('#NotifyAddress4').val(),
        NotifyAddress5: $('#NotifyAddress5').val(),
        No_PO: $('#No_PO').val(),
        BrokerId: $('#BrokerId').val(),
        NamaBroker: $('#NamaBroker').val().toUpperCase(),
        Remarks_No_LC: $('#Remarks_No_LC').val().toUpperCase(),
        Remarks_Bank: $('#Remarks_Bank').val().toUpperCase(),
        Remarks_Shipment: $('#Remarks_Shipment').val().toUpperCase(),
        WeightList_Sets: $('#WeightList_Sets').val(),
        WeightList_Copies: $('#WeightList_Copies').val(),
        Certificate_Sets: $('#Certificate_Sets').val(),
        Certificate_Copies: $('#Certificate_Copies').val(),
        BL_Sets: $('#BL_Sets').val(),
        BL_Signed: $('#BL_Signed').val(),
        BL_Copies: $('#BL_Copies').val(),
        Enclosures_Sets: $('#Enclosures_Sets').val(),
        UserName: $('#UserName').val(),
        MainBudidayaId: $('#MainBudidayaId').val(),
        NamaMainBudidaya: $('#NamaMainBudidaya').val().toUpperCase(),
        Pejabat: $('#Pejabat').val().toUpperCase(),
        VerKaur: $('#VerKaur').val(),
        VerKabag: $('#VerKabag').val(),
        TglVerKaur: $('#TglVerKaur').val(),
        TglVerKabag: $('#TglVerKabag').val(),
        UserNameKaur: $('#UserNameKaur').val(),
        UserNameKabag: $('#UserNameKabag').val(),
        No_SuratTugas: $('#No_SuratTugas').val(),
        Notes: $('#Notes').val(),
		OceanFreightCost:$('#OceanFreightCost').val(),
		InsuranceCost: $('#InsuranceCost').val(),
		SudahKirimKeBGR: $('#SudahKirimKeBGR').val(),
		ProsesStuffing: $('#ProsesStuffing').data('kendoDropDownList').value(),
        StatusPenjualan: $('#StatusPenjualan').val()
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
                            hdr_ShippingInstructionId = $('#HDR_ShippingInstructionId').val();
                            $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                                disableHeader();
                                $('#btnDeleteHeader').removeClass('disabledbutton');
                                $('#btnPrintHeader').removeClass('disabledbutton');
                                $('#btnDeleteHeader').attr('disabled', false);
                                $('#btnPrintHeader').attr('disabled', false);
                                $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                                $('#grdDetail').removeClass('disabledbutton');
                                $("#grdDetail").data("kendoGrid").dataSource.read({
                                    id: $('#HDR_ShippingInstructionId').val()
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_ShippingInstruction]" +
                " where HDR_ShippingInstructionId='" + $("#HDR_ShippingInstructionId").val() + "'",
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[HDR_Invoice]" +
                " where HDR_ShippingInstructionId='" + $("#HDR_ShippingInstructionId").val() + "'",
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

function PeriksaConstraintDetail(row) {
    // periksa apakah punya data di detail dan punya data di invoice
    // kalo ya, batalkan hapus header
    var id = row.find("td:first").html()
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_Invoice]" +
                " where DTL_ShippingInstructionId='" + id + "'",
            _menuId: menuId
        },
        type: 'GET',
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
        hdr_ShippingInstructionId = _NomorInputView.HDR_ShippingInstructionId;
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
        parameters: { HDR_ShippingInstructionId: $('#HDR_ShippingInstructionId').val() }
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

function filterVessel() {
    return {
        key: "NamaVessel",
        value: $("#NamaVessel").val(),
        _menuId: menuId
    };
}

function filterConsignee() {
    return {
        key: "Consignee1",
        value: $("#Consignee1").val(),
        _menuId: menuId
    };
}

function filterNotifyAddress() {
    return {
        key: "NotifyAddress1",
        value: $("#NotifyAddress1").val(),
        _menuId: menuId
    };
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
    return {
        key: "NamaBroker",
        value: $("#NamaBroker").val(),
        _menuId: menuId
    };
}

function Consignee1OnSelect(e) {
    var consignee = this.dataItem(e.item);
    $('#Consignee1').val(consignee.Consignee1);
    $('#Consignee2').val(consignee.Consignee2);
    $('#Consignee3').val(consignee.Consignee3);
    $('#Consignee4').val(consignee.Consignee4);
    $('#Consignee5').val(consignee.Consignee5);
}

function NotifyAddress1OnSelect(e) {
    var consignee = this.dataItem(e.item);
    $('#NotifyAddress2').val(consignee.NotifyAddress2);
    $('#NotifyAddress3').val(consignee.NotifyAddress3);
    $('#NotifyAddress4').val(consignee.NotifyAddress4);
    $('#NotifyAddress5').val(consignee.NotifyAddress5);
}

function NamaBrokerOnSelect(e) {
    var broker = this.dataItem(e.item);
    $('#BrokerId').val(broker.BrokerId);
}

function NamaBrokerOnDataBound(e) {
    if ($('#NamaBroker').val() == '') $('#BrokerId').val("00000000-0000-0000-0000-000000000000");
}

// Detail

function grdOnChange(e) {
}

function grdOnEdit(e) {
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.DTL_ShippingInstructionId = model.uid;
        model.HDR_ShippingInstructionId = hdr_ShippingInstructionId;
    }
    dtl_ShippingInstructionId = model.DTL_ShippingInstructionId;
    mainBudidayaId = $('#MainBudidayaId').val();
    gridStatus = 'sudah-diapa-apain';
	
    $('#jmlGross').text(kendo.toString(rekapGross(), 'n2'));
	$('#jmlHarga').text(kendo.toString(rekapJumlahHarga(), 'n2'));
	model.JumlahHarga = hitungJumlah(dtl_ShippingInstructionId);
	$('#jml' + model.dtl_ShippingInstructionId).text(kendo.toString(model.JumlahHarga, 'n2'));
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
    var dataItem = typeof (grid.dataSource.get(dtl_ShippingInstructionId)) == "undefined" ? grid.dataSource.getByUid(dtl_ShippingInstructionId) : grid.dataSource.get(dtl_ShippingInstructionId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.SubBudidayaId = ddlItem.SubBudidayaId;
    subBudidayaId = ddlItem.SubBudidayaId;
}

function aucNamaSubBudidayaOnDataBound(e) {

}

function filterMerk(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_ShippingInstructionId)) == "undefined" ? grid.dataSource.getByUid(dtl_ShippingInstructionId) : grid.dataSource.get(dtl_ShippingInstructionId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_ShippingInstructionId)) == "undefined" ? grid.dataSource.getByUid(dtl_ShippingInstructionId) : grid.dataSource.get(dtl_ShippingInstructionId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_ShippingInstructionId)) == "undefined" ? grid.dataSource.getByUid(dtl_ShippingInstructionId) : grid.dataSource.get(dtl_ShippingInstructionId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_ShippingInstructionId)) == "undefined" ? grid.dataSource.getByUid(dtl_ShippingInstructionId) : grid.dataSource.get(dtl_ShippingInstructionId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_ShippingInstructionId)) == "undefined" ? grid.dataSource.getByUid(dtl_ShippingInstructionId) : grid.dataSource.get(dtl_ShippingInstructionId);
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
                alert('Error! Hubungi Bagian TI');
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

// copykan ke semua view
function cekNoChop(input) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_ShippingInstructionId)) == "undefined" ? grid.dataSource.getByUid(dtl_ShippingInstructionId) : grid.dataSource.get(dtl_ShippingInstructionId);
    var items = grid.dataItems();
    var arr = [, ];
    for (var i = 0; i < items.length; i++) {
        if (items[i].Chop == input.val() && items[i].DTL_ShippingInstructionId != dtl_ShippingInstructionId
            && items[i].Chop.toLowerCase() != "na" && items[i].MerkId == dataItem.MerkId) {
            arr.push([items[i].DTL_ShippingInstructionId, 1]);
            return $.ajax(arr);
        }
    }
    return $.ajax({
        url: '/Input/ExecSQL',
        data: {
            scriptSQL:
                " select DTL_ShippingInstructionId as Id, count(*) as Cnt from [SPDK_KanpusEF].[dbo].[DTL_ShippingInstruction]" +
                " where MerkId='" + dataItem.MerkId + "'" +
                " and SubBudidayaId='" + dataItem.SubBudidayaId + "'" +
                " and Chop='" + input.val() + "'  and lower(Chop)!='na' and TahunProduksi='"+dataItem.TahunProduksi+"' group by DTL_ShippingInstructionId UNION" +
                " select DTL_POId as Id, count(*) as Cnt from [SPDK_KanpusEF].[dbo].[DTL_PO]" +
                " where MerkId='" + dataItem.MerkId + "'" +
                " and SubBudidayaId='" + dataItem.SubBudidayaId + "'" +
                " and Chop='" + input.val() + "' and lower(Chop)!='na' and TahunProduksi='"+dataItem.TahunProduksi+"' group by DTL_POId",
        _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

//// sampe sini

function ambilStandarGrade() {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_ShippingInstructionId)) == "undefined" ? grid.dataSource.getByUid(dtl_ShippingInstructionId) : grid.dataSource.get(dtl_ShippingInstructionId);
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
        listId.push("'" + data[i].DTL_ShippingInstructionId.toString() + "'");
    }
    var strId = listId.toString();

    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_Invoice]" +
                " where DTL_ShippingInstructionId in (" + strId + ")",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
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
