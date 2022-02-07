var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var kerjasama_DokumenId, kerjasama_Dokumen_MinatId, kerjasama_Dokumen_LegalId, kerjasama_Dokumen_BASurveiId, kerjasama_Dokumen_MOMId, kerjasama_Dokumen_SaranPendapatId, kerjasama_Dokumen_MemoKajianId, kerjasama_Dokumen_PetaId, kerjasama_Dokumen_MoUId, kerjasama_Dokumen_RisalahRakorId, kerjasama_Dokumen_IjinDireksiId, kerjasama_Dokumen_CRSAId, kerjasama_Dokumen_PKSId, kerjasama_Dokumen_InvoiceId, kerjasama_Dokumen_BuktiPembayaranId;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var bagianId, proyekId, nominal;
var rowNumber = 0;
var menuId = '6bdf3693-40b1-4f52-82d6-a9d6bd4c0c18';

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
    disableHeader();

    wnd = $("#modalWindow").kendoWindow({
        title: "Konfirmasi",
        modal: true,
        visible: false,
        resizable: false,
        width: 300
    }).data("kendoWindow");

    uploadFile = $("#uploadFile").kendoWindow({
        title: "Upload File",
        modal: true,
        visible: false,
        resizable: false,
        width: 500,
        height: 300
    }).data("kendoWindow");

    uploadFile2 = $("#uploadFile2").kendoWindow({
        title: "Upload File",
        modal: true,
        visible: false,
        resizable: false,
        width: 500,
        height: 300
    }).data("kendoWindow");

    uploadFile3 = $("#uploadFile3").kendoWindow({
        title: "Upload File",
        modal: true,
        visible: false,
        resizable: false,
        width: 500,
        height: 300
    }).data("kendoWindow");

    uploadFile4 = $("#uploadFile4").kendoWindow({
        title: "Upload File",
        modal: true,
        visible: false,
        resizable: false,
        width: 500,
        height: 300
    }).data("kendoWindow");

    uploadFile5 = $("#uploadFile5").kendoWindow({
        title: "Upload File",
        modal: true,
        visible: false,
        resizable: false,
        width: 500,
        height: 300
    }).data("kendoWindow");

    uploadFile6 = $("#uploadFile6").kendoWindow({
        title: "Upload File",
        modal: true,
        visible: false,
        resizable: false,
        width: 500,
        height: 300
    }).data("kendoWindow");

    uploadFile7 = $("#uploadFile7").kendoWindow({
        title: "Upload File",
        modal: true,
        visible: false,
        resizable: false,
        width: 500,
        height: 300
    }).data("kendoWindow");

    uploadFile8 = $("#uploadFile8").kendoWindow({
        title: "Upload File",
        modal: true,
        visible: false,
        resizable: false,
        width: 500,
        height: 300
    }).data("kendoWindow");

    uploadFile81 = $("#uploadFile81").kendoWindow({
        title: "Upload File",
        modal: true,
        visible: false,
        resizable: false,
        width: 500,
        height: 300
    }).data("kendoWindow");

    uploadFile9 = $("#uploadFile9").kendoWindow({
        title: "Upload File",
        modal: true,
        visible: false,
        resizable: false,
        width: 500,
        height: 300
    }).data("kendoWindow");

    uploadFile10 = $("#uploadFile10").kendoWindow({
        title: "Upload File",
        modal: true,
        visible: false,
        resizable: false,
        width: 500,
        height: 300
    }).data("kendoWindow");

    uploadFile11 = $("#uploadFile11").kendoWindow({
        title: "Upload File",
        modal: true,
        visible: false,
        resizable: false,
        width: 500,
        height: 300
    }).data("kendoWindow");

    uploadFile12 = $("#uploadFile12").kendoWindow({
        title: "Upload File",
        modal: true,
        visible: false,
        resizable: false,
        width: 500,
        height: 300
    }).data("kendoWindow");

    uploadFile13 = $("#uploadFile13").kendoWindow({
        title: "Upload File",
        modal: true,
        visible: false,
        resizable: false,
        width: 500,
        height: 300
    }).data("kendoWindow");

    uploadFile14 = $("#uploadFile14").kendoWindow({
        title: "Upload File",
        modal: true,
        visible: false,
        resizable: false,
        width: 500,
        height: 300
    }).data("kendoWindow");

    uploadFile15 = $("#uploadFile15").kendoWindow({
        title: "Upload File",
        modal: true,
        visible: false,
        resizable: false,
        width: 500,
        height: 300
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

    pdf = $("#pdfWindow").kendoWindow({
        title: "PDF Viewer",
        modal: true,
        visible: false,
        resizable: false,
        width: 900,
        height: 550
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
    $("#grdDetail14").kendoValidator({
        rules: { // custom rules
            chopvalidation: function (input) {
                if (input.attr("name") == "Invoice_NilaiKompensasi" || input.attr("name") == "Invoice_PPN10" || input.attr("name") == "Invoice_PBB" || input.attr("name") == "Invoice_PPH" || input.attr("name") == "Invoice_GantiRugiTanaman" || input.attr("name") == "Invoice_Revenue_ProfitSharing" ) {
                    var grid = $('#grdDetail14').data('kendoGrid');
                    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_InvoiceId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_InvoiceId) : grid.dataSource.get(kerjasama_Dokumen_InvoiceId);
                    dataItem.Invoice_Total = dataItem.Invoice_NilaiKompensasi + dataItem.Invoice_PPN10 + dataItem.Invoice_PBB + dataItem.Invoice_PPH + dataItem.Invoice_GantiRugiTanaman + dataItem.Invoice_Revenue_ProfitSharing;
                    $('#Invoice_Total').val(dataItem.Invoice_NilaiKompensasi + dataItem.Invoice_PPN10 + dataItem.Invoice_PBB + dataItem.Invoice_PPH + dataItem.Invoice_GantiRugiTanaman + dataItem.Invoice_Revenue_ProfitSharing);
                    grid.refresh();

                }
                else if (input.attr("name") == "BuktiBayar_NilaiKompensasi" || input.attr("name") == "BuktiBayar_PPN10" || input.attr("name") == "BuktiBayar_PBB" || input.attr("name") == "BuktiBayar_PPH" || input.attr("name") == "BuktiBayar_GantiRugiTanaman" || input.attr("name") == "BuktiBayar_Revenue_ProfitSharing") {
                    var grid = $('#grdDetail14').data('kendoGrid');
                    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_InvoiceId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_InvoiceId) : grid.dataSource.get(kerjasama_Dokumen_InvoiceId);
                    dataItem.BuktiBayar_Total = dataItem.BuktiBayar_NilaiKompensasi + dataItem.BuktiBayar_PPN10 + dataItem.BuktiBayar_PBB + dataItem.BuktiBayar_PPH + dataItem.BuktiBayar_GantiRugiTanaman + dataItem.BuktiBayar_Revenue_ProfitSharing;
                    $('#BuktiBayar_Total').val(dataItem.BuktiBayar_NilaiKompensasi + dataItem.BuktiBayar_PPN10 + dataItem.BuktiBayar_PBB + dataItem.BuktiBayar_PPH + dataItem.BuktiBayar_GantiRugiTanaman + dataItem.BuktiBayar_Revenue_ProfitSharing);
                    grid.refresh();

                } else {
                    return true;
                };
            }
        }
    });
    $("#grdDetail15").kendoValidator({
        rules: { // custom rules
            chopvalidation: function (input) {
                if (input.attr("name") == "NilaiPembayaran" && input.val() != "") {
                    var grid = $('#grdDetail15').data('kendoGrid');
                    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_BuktiPembayaranId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_BuktiPembayaranId) : grid.dataSource.get(kerjasama_Dokumen_BuktiPembayaranId);
                    dataItem.Nominal = nominal;
                    dataItem.SelisihPembayaran = nominal - dataItem.NilaiPembayaran;
                } else {
                    return true;
                };
            }
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
                    if (data) {
                        kerjasama_DokumenId = $('#Kerjasama_DokumenId').val();
                        $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                            disableHeader();
                            $('#btnDeleteHeader').removeClass('disabledbutton');
                            $('#btnPrintHeader').removeClass('disabledbutton');
                            $('#btnDeleteHeader').attr('disabled', false);
                            $('#btnPrintHeader').attr('disabled', false);
                            $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                            $('#tabstrip').removeClass('disabledbutton');
                            //minat
                            var grid = $("#grdDetail").data("kendoGrid");
                            grid.dataSource.read({
                                id: $('#Kerjasama_DokumenId').val()
                            }).done(function () {
                                var currentData = grid.dataSource.data();
                                for (var i = 0; i < currentData.length; i++) {
                                    currentData[i].dirty = true;
                                }
                            });
                            //tanggapan
                            var grid2 = $("#grdDetail2").data("kendoGrid");
                            grid2.dataSource.read({
                                id: $('#Kerjasama_DokumenId').val()
                            }).done(function () {
                                var currentData = grid2.dataSource.data();
                                for (var i = 0; i < currentData.length; i++) {
                                    currentData[i].dirty = true;
                                }
                            });
                            //legal
                            var grid3 = $("#grdDetail3").data("kendoGrid");
                            grid3.dataSource.read({
                                id: $('#Kerjasama_DokumenId').val()
                            }).done(function () {
                                var currentData = grid3.dataSource.data();
                                for (var i = 0; i < currentData.length; i++) {
                                    currentData[i].dirty = true;
                                }
                                });
                            //BA survei
                            var grid4 = $("#grdDetail4").data("kendoGrid");
                            grid4.dataSource.read({
                                id: $('#Kerjasama_DokumenId').val()
                            }).done(function () {
                                var currentData = grid4.dataSource.data();
                                for (var i = 0; i < currentData.length; i++) {
                                    currentData[i].dirty = true;
                                }
                                });
                            //MoM
                            var grid5 = $("#grdDetail5").data("kendoGrid");
                            grid5.dataSource.read({
                                id: $('#Kerjasama_DokumenId').val()
                            }).done(function () {
                                var currentData = grid5.dataSource.data();
                                for (var i = 0; i < currentData.length; i++) {
                                    currentData[i].dirty = true;
                                }
                                });
                            //Saran Pendapat
                            var grid6 = $("#grdDetail6").data("kendoGrid");
                            grid6.dataSource.read({
                                id: $('#Kerjasama_DokumenId').val()
                            }).done(function () {
                                var currentData = grid6.dataSource.data();
                                for (var i = 0; i < currentData.length; i++) {
                                    currentData[i].dirty = true;
                                }
                                });
                            //Memo Kajian
                            var grid7 = $("#grdDetail7").data("kendoGrid");
                            grid7.dataSource.read({
                                id: $('#Kerjasama_DokumenId').val()
                            }).done(function () {
                                var currentData = grid7.dataSource.data();
                                for (var i = 0; i < currentData.length; i++) {
                                    currentData[i].dirty = true;
                                }
                                });
                            //Peta
                            var grid8 = $("#grdDetail8").data("kendoGrid");
                            grid8.dataSource.read({
                                id: $('#Kerjasama_DokumenId').val()
                            }).done(function () {
                                var currentData = grid8.dataSource.data();
                                for (var i = 0; i < currentData.length; i++) {
                                    currentData[i].dirty = true;
                                }
                                });
                            //PPT Rekor
                            var grid9 = $("#grdDetail9").data("kendoGrid");
                            grid9.dataSource.read({
                                id: $('#Kerjasama_DokumenId').val()
                            }).done(function () {
                                var currentData = grid9.dataSource.data();
                                for (var i = 0; i < currentData.length; i++) {
                                    currentData[i].dirty = true;
                                }
                                });
                            //Risalah Rakor
                            //var grid10 = $("#grdDetail10").data("kendoGrid");
                            //grid10.dataSource.read({
                            //    id: $('#Kerjasama_DokumenId').val()
                            //}).done(function () {
                            //    var currentData = grid10.dataSource.data();
                            //    for (var i = 0; i < currentData.length; i++) {
                            //        currentData[i].dirty = true;
                            //    }
                            //    });
                            //Ijin Direksi
                            var grid11 = $("#grdDetail11").data("kendoGrid");
                            grid11.dataSource.read({
                                id: $('#Kerjasama_DokumenId').val()
                            }).done(function () {
                                var currentData = grid11.dataSource.data();
                                for (var i = 0; i < currentData.length; i++) {
                                    currentData[i].dirty = true;
                                }
                                });
                            //CRSA
                            var grid12 = $("#grdDetail12").data("kendoGrid");
                            grid12.dataSource.read({
                                id: $('#Kerjasama_DokumenId').val()
                            }).done(function () {
                                var currentData = grid12.dataSource.data();
                                for (var i = 0; i < currentData.length; i++) {
                                    currentData[i].dirty = true;
                                }
                                });
                            //PKS
                            //var grid13 = $("#grdDetail13").data("kendoGrid");
                            //grid13.dataSource.read({
                            //    id: $('#Kerjasama_DokumenId').val()
                            //}).done(function () {
                            //    var currentData = grid13.dataSource.data();
                            //    for (var i = 0; i < currentData.length; i++) {
                            //        currentData[i].dirty = true;
                            //    }
                            //    });
                            //Invoice
                            var grid14 = $("#grdDetail14").data("kendoGrid");
                            grid14.dataSource.read({
                                id: $('#Kerjasama_DokumenId').val()
                            }).done(function () {
                                var currentData = grid14.dataSource.data();
                                for (var i = 0; i < currentData.length; i++) {
                                    currentData[i].dirty = true;
                                }
                                });
                            //Invoice
                        });
                    }
                    else {
                        openErrWindow();
                        $('#tabstrip').addClass('disabledbutton');
                        gridDestroy();
                    }
                })
                    .fail(function (x) {
                        alert("Error! Hubungi Bagian TI");
                    });
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
        $('#grdDetail').data('kendoGrid').removeRow(row);
        gridStatus = 'sudah-diapa-apain';

    });
    $("#noDetail").click(function () {
        wndDetail.close();
    });
}

function openWindow2(e) {

    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
        $('#grdDetail2').data('kendoGrid').removeRow(row);
        gridStatus = 'sudah-diapa-apain';

    });
    $("#noDetail").click(function () {
        wndDetail.close();
    });
}

function openWindow3(e) {

    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
        $('#grdDetail3').data('kendoGrid').removeRow(row);
        gridStatus = 'sudah-diapa-apain';

    });
    $("#noDetail").click(function () {
        wndDetail.close();
    });
}

function openWindow4(e) {

    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
        $('#grdDetail4').data('kendoGrid').removeRow(row);
        gridStatus = 'sudah-diapa-apain';

    });
    $("#noDetail").click(function () {
        wndDetail.close();
    });
}

function openWindow5(e) {

    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
        $('#grdDetail5').data('kendoGrid').removeRow(row);
        gridStatus = 'sudah-diapa-apain';

    });
    $("#noDetail").click(function () {
        wndDetail.close();
    });
}

function openWindow6(e) {

    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
        $('#grdDetail6').data('kendoGrid').removeRow(row);
        gridStatus = 'sudah-diapa-apain';

    });
    $("#noDetail").click(function () {
        wndDetail.close();
    });
}

function openWindow7(e) {

    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
        $('#grdDetail7').data('kendoGrid').removeRow(row);
        gridStatus = 'sudah-diapa-apain';

    });
    $("#noDetail").click(function () {
        wndDetail.close();
    });
}

function openWindow8(e) {

    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
        $('#grdDetail8').data('kendoGrid').removeRow(row);
        gridStatus = 'sudah-diapa-apain';

    });
    $("#noDetail").click(function () {
        wndDetail.close();
    });
}

function openWindow9(e) {

    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
        $('#grdDetail9').data('kendoGrid').removeRow(row);
        gridStatus = 'sudah-diapa-apain';

    });
    $("#noDetail").click(function () {
        wndDetail.close();
    });
}

function openWindow10(e) {

    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
        $('#grdDetail10').data('kendoGrid').removeRow(row);
        gridStatus = 'sudah-diapa-apain';

    });
    $("#noDetail").click(function () {
        wndDetail.close();
    });
}

function openWindow11(e) {

    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
        $('#grdDetail11').data('kendoGrid').removeRow(row);
        gridStatus = 'sudah-diapa-apain';

    });
    $("#noDetail").click(function () {
        wndDetail.close();
    });
}

function openWindow12(e) {

    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
        $('#grdDetail12').data('kendoGrid').removeRow(row);
        gridStatus = 'sudah-diapa-apain';

    });
    $("#noDetail").click(function () {
        wndDetail.close();
    });
}

function openWindow13(e) {

    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
        $('#grdDetail13').data('kendoGrid').removeRow(row);
        gridStatus = 'sudah-diapa-apain';

    });
    $("#noDetail").click(function () {
        wndDetail.close();
    });
}

function openWindow14(e) {

    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
        $('#grdDetail14').data('kendoGrid').removeRow(row);
        gridStatus = 'sudah-diapa-apain';

    });
    $("#noDetail").click(function () {
        wndDetail.close();
    });
}

function openWindow15(e) {

    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
        $('#grdDetail15').data('kendoGrid').removeRow(row);
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
            _menuId: menuId,
            _SQLLanjut: "exec [KerjasamaEF].[dbo].[Kerjasama_IsiRincianLegalitas] '" + $('#ListProyekId').val() + "', '" + $('#Kerjasama_DokumenId').val() +
                "', '" + $('#BagianId').val() + "'"
        },
        type: 'POST',
        datatype: 'json'
    });
}

function ModelToView(_NomorInputView) {
    $('#Kerjasama_DokumenId').val(_NomorInputView.Kerjasama_DokumenId);
    $('#BagianId').val(_NomorInputView.BagianId);
    $('#TahunBuku').val(_NomorInputView.TahunBuku);
    $('#NomorInput').val(_NomorInputView.NomorInput);
    $('#NomorInputView').val(_NomorInputView.NomorInputView);
    $('#NomorPerjanjian').val(_NomorInputView.NomorPerjanjian);
    $('#TanggalPenandatangananPerjanjian').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.TanggalPenandatangananPerjanjian.substr(6))));
    $('#StatusPerjanjian').data('kendoDropDownList').value(_NomorInputView.StatusPerjanjian);
    $('#BentukKerjasama').data('kendoDropDownList').value(_NomorInputView.BentukKerjasama);
    $('#Bidang').data('kendoDropDownList').value(_NomorInputView.Bidang);
    $('#Perihal').val(_NomorInputView.Perihal);
    $('#JangkaWaktu').val(_NomorInputView.JangkaWaktu);
    $('#NamaMitra').val(_NomorInputView.NamaMitra);
    $('#LokasiPerjanjian').val(_NomorInputView.LokasiPerjanjian);
    $('#JangkaWaktuKerjasama').val(_NomorInputView.JangkaWaktuKerjasama);
    $('#TanggalMulai').val(_NomorInputView.TanggalMulai);
    $('#TanggalBerakhir').val(_NomorInputView.TanggalBerakhir);
    $('#NilaiNominal').val(_NomorInputView.NilaiNominal);
    $('#BentukKompensasi').val(_NomorInputView.BentukKompensasi);
    $('#PIC').val(_NomorInputView.PIC);
    $('#EstimasiBenefit').val(_NomorInputView.EstimasiBenefit);
    $('#UserName').val(_NomorInputView.UserName);
}

function ViewToModel() {
    var mdl = {
        Kerjasama_DokumenId: $('#Kerjasama_DokumenId').val(),
        BagianId: $('#BagianId').val(),
        TahunBuku: $('#TahunBuku').val(),
        NomorInput: $('#NomorInput').val(),
        NomorInputView: Array(5 - String($('#NomorInput').val()).length + 1).join('0') + $('#NomorInput').val() + ' - ' + $('#NomorPerjanjian').val().toUpperCase(),
        NomorPerjanjian: $('#NamaMitra').val(),
        TanggalPenandatangananPerjanjian: $('#TanggalPenandatangananPerjanjian').val(),
        StatusPerjanjian: $('#StatusPerjanjian').val(),
        BentukKerjasama: $('#BentukKerjasama').val(),
        Bidang: $('#Bidang').val(),
        Perihal: $('#Perihal').val(),
        JangkaWaktu: $('#JangkaWaktu').val(),
        NamaMitra: $('#NamaMitra').val(),
        LokasiPerjanjian: $('#LokasiPerjanjian').val(),
        JangkaWaktuKerjasama: $('#JangkaWaktuKerjasama').val(),
        TanggalMulai: $('#TanggalMulai').val(),
        TanggalBerakhir: $('#TanggalBerakhir').val(),
        NilaiNominal: $('#NilaiNominal').val(),
        BentukKompensasi: $('#BentukKompensasi').val(),
        PIC: $('#PIC').val(),
        EstimasiBenefit: $('#EstimasiBenefit').val(),
        StatusOtoritas: $('#StatusOtoritas').val(),
        PasswordFile: $('#PasswordFile').val(),
        Verifikasi: $('#Verifikasi').val(),
        UserName: $('#UserName').val()
    };
    return mdl;
}

function hapusHeader() {
    wnd.close();
    PeriksaConstraintHeader().done(function (data) {
        if (data == 0) {
            ConfirmedHapusHeader().done(function (data) {
                if (data) {
                    kerjasama_DokumenId = $('#Kerjasama_DokumenId').val();
                    //$('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                    disableHeader();
                    $('#btnDeleteHeader').removeClass('disabledbutton');
                    $('#btnPrintHeader').removeClass('disabledbutton');
                    $('#btnDeleteHeader').attr('disabled', false);
                    $('#btnPrintHeader').attr('disabled', false);
                    $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                    $('#tabstrip').removeClass('disabledbutton');
                    $("#grdDetail").data("kendoGrid").dataSource.read({
                        id: $('#Kerjasama_DokumenId').val()
                    });
                    //});
                }
                else {
                    openErrWindow();
                    $('#tabstrip').addClass('disabledbutton');
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_Remise]" +
                " where HDR_RemiseId='" + $("#HDR_RemiseId").val() + "'",
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
        cekData(_NomorInputView.NomorInputView);
        ModelToView(_NomorInputView);
        kerjasama_DokumenId = _NomorInputView.Kerjasama_DokumenId;
        
        $('#NomorPerjanjian').focus();
    }
}

function NomorInputViewOnDataBound(e) {
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
        parameters: { HDR_RemiseId: $('#HDR_RemiseId').val(), KebunId: $('#KebunId').val(), Bulan: $('#BulanRemise').val(), Tahun: $('#TahunRemise').val() }
    });
    viewer.refreshReport();
    prt.open().center();
}

function disableHeader() {
    //$("._key").find('span').addClass('disabledbutton');
    //$("._key").addClass('disabledbutton');
    //$("._nonkey").find('span').addClass('disabledbutton');
    //$("._nonkey").addClass('disabledbutton');

    //$('#btnSubmitHeader').addClass('disabledbutton');
    //$('#btnDeleteHeader').addClass('disabledbutton');
    //$('#btnPrintHeader').addClass('disabledbutton');
    //$('#tabstrip').addClass('disabledbutton');
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
        $('#tabstrip').addClass('disabledbutton');
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
        $('#tabstrip').addClass('disabledbutton');
        gridDestroy();
    }
}

function filterNomorInput(e) {
    var mdl = JSON.stringify(ViewToModel());
    var arrFilter = [];
    //var userName = $('#UserName').val();
    //var bagianId = $('#BagianId').val();
    //arrFilter.push('UserName');
    //arrFilter.push(userName);

    //arrFilter.push('BagianId');
    //arrFilter.push(bagianId);
    return { _model: mdl, _menuId: menuId, _filter: arrFilter };
}

function filterKebun() {
    return {
        key: "NamaKebun",
        value: $("#NamaKebun").val()
    };
}

function NamaKebunOnSelect(e) {
    var kebun = this.dataItem(e.item);
    if (typeof (kebun) != 'undefined') {
        $('#KebunId').val(kebun.KebunId);
        $('#NamaKebun').val(kebun.Nama);
        kebunId = kebun.KebunId;
    }

}
// Minat

function grdOnChange(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    var model = e.model;
}

function grdOnEdit(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.Kerjasama_Dokumen_MinatId = model.uid;
        model.Kerjasama_DokumenId = kerjasama_DokumenId;
    }

    kerjasama_Dokumen_MinatId = model.Kerjasama_Dokumen_MinatId;
    gridStatus = 'sudah-diapa-apain';
}

function grdOnBound(e) {

}

// Tanggapan

function grdOnChange2(e) {
    var grid = $('#grdDetail2').data('kendoGrid');
    var model = e.model;
}

function grdOnEdit2(e) {
    var grid = $('#grdDetail2').data('kendoGrid');
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.Kerjasama_Dokumen_TanggapanId = model.uid;
        model.Kerjasama_DokumenId = kerjasama_DokumenId;
    }

    kerjasama_Dokumen_TanggapanId = model.Kerjasama_Dokumen_TanggapanId;
    gridStatus = 'sudah-diapa-apain';
}

function grdOnBound2(e) {

}

// Legal

function grdOnChange3(e) {
    var grid = $('#grdDetail3').data('kendoGrid');
    var model = e.model;
}

function grdOnEdit3(e) {
    var grid = $('#grdDetail3').data('kendoGrid');
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.Kerjasama_Dokumen_LegalId = model.uid;
        model.Kerjasama_DokumenId = kerjasama_DokumenId;
    }

    kerjasama_Dokumen_LegalId = model.Kerjasama_Dokumen_LegalId;
    gridStatus = 'sudah-diapa-apain';
}

function grdOnBound3(e) {

}

// BA Survei

function grdOnChange4(e) {
    var grid = $('#grdDetail4').data('kendoGrid');
    var model = e.model;
}

function grdOnEdit4(e) {
    var grid = $('#grdDetail4').data('kendoGrid');
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.Kerjasama_Dokumen_BASurveiId = model.uid;
        model.Kerjasama_DokumenId = kerjasama_DokumenId;
    }

    kerjasama_Dokumen_BASurveiId = model.Kerjasama_Dokumen_BASurveiId;
    gridStatus = 'sudah-diapa-apain';
}

function grdOnBound4(e) {

}

// MoM

function grdOnChange5(e) {
    var grid = $('#grdDetail5').data('kendoGrid');
    var model = e.model;
}

function grdOnEdit5(e) {
    var grid = $('#grdDetail5').data('kendoGrid');
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.Kerjasama_Dokumen_MOMId = model.uid;
        model.Kerjasama_DokumenId = kerjasama_DokumenId;
    }

    kerjasama_Dokumen_MOMId = model.Kerjasama_Dokumen_MOMId;
    gridStatus = 'sudah-diapa-apain';
}

function grdOnBound5(e) {

}

// Saran Pendapat

function grdOnChange6(e) {
    var grid = $('#grdDetail6').data('kendoGrid');
    var model = e.model;
}

function grdOnEdit6(e) {
    var grid = $('#grdDetail6').data('kendoGrid');
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.Kerjasama_Dokumen_SaranPendapatId = model.uid;
        model.Kerjasama_DokumenId = kerjasama_DokumenId;
    }

    kerjasama_Dokumen_SaranPendapatId = model.Kerjasama_Dokumen_SaranPendapatId;
    gridStatus = 'sudah-diapa-apain';
}

function grdOnBound6(e) {

}

// Memo Kajian

function grdOnChange7(e) {
    var grid = $('#grdDetail7').data('kendoGrid');
    var model = e.model;
}

function grdOnEdit7(e) {
    var grid = $('#grdDetail7').data('kendoGrid');
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.Kerjasama_Dokumen_MemoKajianId = model.uid;
        model.Kerjasama_DokumenId = kerjasama_DokumenId;
    }

    kerjasama_Dokumen_MemoKajianId = model.Kerjasama_Dokumen_MemoKajianId;
    gridStatus = 'sudah-diapa-apain';
}

function grdOnBound7(e) {

}

// Peta

function grdOnChange8(e) {
    var grid = $('#grdDetail8').data('kendoGrid');
    var model = e.model;
}

function grdOnEdit8(e) {
    var grid = $('#grdDetail8').data('kendoGrid');
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.Kerjasama_Dokumen_PetaId = model.uid;
        model.Kerjasama_DokumenId = kerjasama_DokumenId;
    }

    kerjasama_Dokumen_PetaId = model.Kerjasama_Dokumen_PetaId;
    gridStatus = 'sudah-diapa-apain';
}

function grdOnBound8(e) {

}

// PPT Rakor

function grdOnChange9(e) {
    var grid = $('#grdDetail9').data('kendoGrid');
    var model = e.model;
}

function grdOnEdit9(e) {
    var grid = $('#grdDetail9').data('kendoGrid');
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.Kerjasama_Dokumen_MoUId = model.uid;
        model.Kerjasama_DokumenId = kerjasama_DokumenId;
    }

    kerjasama_Dokumen_MoUId = model.Kerjasama_Dokumen_MoUId;
    gridStatus = 'sudah-diapa-apain';
}

function grdOnBound9(e) {

}

// Risalah Rakor

//function grdOnChange10(e) {
//    var grid = $('#grdDetail10').data('kendoGrid');
//    var model = e.model;
//}

//function grdOnEdit10(e) {
//    var grid = $('#grdDetail10').data('kendoGrid');
//    var model = e.model;
//    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
//    if (model.isNew()) {
//        model.Kerjasama_Dokumen_RisalahRakorId = model.uid;
//        model.Kerjasama_DokumenId = kerjasama_DokumenId;
//    }

//    kerjasama_Dokumen_RisalahRakorId = model.Kerjasama_Dokumen_RisalahRakorId;
//    gridStatus = 'sudah-diapa-apain';
//}

//function grdOnBound10(e) {

//}
// Ijin Direksi

function grdOnChange11(e) {
    var grid = $('#grdDetail11').data('kendoGrid');
    var model = e.model;
}

function grdOnEdit11(e) {
    var grid = $('#grdDetail11').data('kendoGrid');
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.Kerjasama_Dokumen_IjinDireksiId = model.uid;
        model.Kerjasama_DokumenId = kerjasama_DokumenId;
    }

    kerjasama_Dokumen_IjinDireksiId = model.Kerjasama_Dokumen_IjinDireksiId;
    gridStatus = 'sudah-diapa-apain';
}

function grdOnBound11(e) {

}

// CRSA

function grdOnChange12(e) {
    var grid = $('#grdDetail12').data('kendoGrid');
    var model = e.model;
}

function grdOnEdit12(e) {
    var grid = $('#grdDetail12').data('kendoGrid');
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.Kerjasama_Dokumen_CRSAId = model.uid;
        model.Kerjasama_DokumenId = kerjasama_DokumenId;
    }

    kerjasama_Dokumen_CRSAId = model.Kerjasama_Dokumen_CRSAId;
    gridStatus = 'sudah-diapa-apain';
}

function grdOnBound12(e) {

}

// PKS

function grdOnChange13(e) {
    var grid = $('#grdDetail13').data('kendoGrid');
    var model = e.model;
}

function grdOnEdit13(e) {
    var grid = $('#grdDetail13').data('kendoGrid');
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.Kerjasama_Dokumen_PKSId = model.uid;
        model.Kerjasama_DokumenId = kerjasama_DokumenId;
    }

    kerjasama_Dokumen_PKSId = model.Kerjasama_Dokumen_PKSId;
    gridStatus = 'sudah-diapa-apain';
}

function grdOnBound13(e) {

}

// Invoice

function grdOnChange14(e) {
    var grid = $('#grdDetail14').data('kendoGrid');
    var model = e.model;
}

function grdOnEdit14(e) {
    var grid = $('#grdDetail14').data('kendoGrid');
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.Kerjasama_Dokumen_InvoiceId = model.uid;
        model.Kerjasama_DokumenId = kerjasama_DokumenId;
    }

    kerjasama_Dokumen_InvoiceId = model.Kerjasama_Dokumen_InvoiceId;
    gridStatus = 'sudah-diapa-apain';
}

function grdOnBound14(e) {

}

// Bukti Pembayaran

function grdOnChange15(e) {
    var grid = $('#grdDetail15').data('kendoGrid');
    var model = e.model;
}

function grdOnEdit15(e) {
    var grid = $('#grdDetail15').data('kendoGrid');
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.Kerjasama_Dokumen_BuktiPembayaranId = model.uid;
        model.Kerjasama_DokumenId = kerjasama_DokumenId;
    }

    kerjasama_Dokumen_BuktiPembayaranId = model.Kerjasama_Dokumen_BuktiPembayaranId;
    gridStatus = 'sudah-diapa-apain';
}

function grdOnBound15(e) {

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
        tahunBukuOnChange().done(function (data) {
            if (data) {
                //$('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                disableHeader();
                $(".k-button-icontext").addClass("k-state-disabled").removeClass("k-grid-add"); // edited
                gridDestroy();
                //});
            }
            else {
                alert("Error ! Hubungi Bagian TI");
            }

        }).fail(function (x) {
            alert("Error! Hubungi Bagian TI");
        });

    }
}

function tahunBukuOnChange() {
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

function namaKebunOnChange() {
    var url = '/Input/hapusContextNomorInput';
    return $.ajax({
        url: url,
        type: 'POST',
        datatype: 'json',
        data: {
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

function sendData2() {
    var grid = $("#grdDetail2").data("kendoGrid"),
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
    $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }),
        parameterMap({ new: newRecords }), parameterMap({ mnid: menuId }),
        parameterMap({ classdv: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_Tanggapan" })
    );
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

function sendData3() {
    var grid = $("#grdDetail3").data("kendoGrid"),
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
    $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }),
        parameterMap({ new: newRecords }), parameterMap({ mnid: menuId }),
        parameterMap({ classdv: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_Legal" })
    );
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

function sendData4() {
    var grid = $("#grdDetail4").data("kendoGrid"),
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
    $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }),
        parameterMap({ new: newRecords }), parameterMap({ mnid: menuId }),
        parameterMap({ classdv: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_BASurvei" })
    );
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

function sendData5() {
    var grid = $("#grdDetail5").data("kendoGrid"),
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
    $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }),
        parameterMap({ new: newRecords }), parameterMap({ mnid: menuId }),
        parameterMap({ classdv: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_MOM" })
    );
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

function sendData6() {
    var grid = $("#grdDetail6").data("kendoGrid"),
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
    $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }),
        parameterMap({ new: newRecords }), parameterMap({ mnid: menuId }),
        parameterMap({ classdv: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_SaranPendapat" })
    );
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

function sendData7() {
    var grid = $("#grdDetail7").data("kendoGrid"),
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
    $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }),
        parameterMap({ new: newRecords }), parameterMap({ mnid: menuId }),
        parameterMap({ classdv: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_MemoKajian" })
    );
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

function sendData8() {
    var grid = $("#grdDetail8").data("kendoGrid"),
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
    $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }),
        parameterMap({ new: newRecords }), parameterMap({ mnid: menuId }),
        parameterMap({ classdv: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_Peta" })
    );
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

function sendData9() {
    var grid = $("#grdDetail9").data("kendoGrid"),
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
    $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }),
        parameterMap({ new: newRecords }), parameterMap({ mnid: menuId }),
        parameterMap({ classdv: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_MoU" })
    );
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

function sendData10() {
    var grid = $("#grdDetail10").data("kendoGrid"),
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
    $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }),
        parameterMap({ new: newRecords }), parameterMap({ mnid: menuId }),
        parameterMap({ classdv: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_RisalahRakor" })
    );
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

function sendData11() {
    var grid = $("#grdDetail11").data("kendoGrid"),
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
    $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }),
        parameterMap({ new: newRecords }), parameterMap({ mnid: menuId }),
        parameterMap({ classdv: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_IjinDireksi" })
    );
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

function sendData12() {
    var grid = $("#grdDetail12").data("kendoGrid"),
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
    $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }),
        parameterMap({ new: newRecords }), parameterMap({ mnid: menuId }),
        parameterMap({ classdv: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_CRSA" })
    );
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

function sendData13() {
    var grid = $("#grdDetail13").data("kendoGrid"),
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
    $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }),
        parameterMap({ new: newRecords }), parameterMap({ mnid: menuId }),
        parameterMap({ classdv: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_PKS" })
    );
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

function sendData14() {
    var grid = $("#grdDetail14").data("kendoGrid"),
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
    $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }),
        parameterMap({ new: newRecords }), parameterMap({ mnid: menuId }),
        parameterMap({ classdv: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_Invoice" })
    );
    _sendData(data).done(function (msg) {
        if (msg == "Success") {
            grid.dataSource.read([]);
            //disableHeader();
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

function sendData15() {
    var grid = $("#grdDetail15").data("kendoGrid"),
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
    $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }),
        parameterMap({ new: newRecords }), parameterMap({ mnid: menuId }),
        parameterMap({ classdv: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_BuktiPembayaran" })
    );
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

////// sampe sini
function hapusSeluruh(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
        //openDelDetailWindow();
        var grid = $('#grdDetail').data('kendoGrid');
        grid.dataSource.page(1);
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

function filterdetailRead2(e) {
    return { _menuId: menuId, _classDetailView:"Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_Tanggapan" };
}

function filterdetailRead3(e) {
    return { _menuId: menuId, _classDetailView: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_Legal" };
}

function filterdetailRead4(e) {
    return { _menuId: menuId, _classDetailView: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_BASurvei" };
}

function filterdetailRead5(e) {
    return { _menuId: menuId, _classDetailView: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_MOM" };
}

function filterdetailRead6(e) {
    return { _menuId: menuId, _classDetailView: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_SaranPendapat" };
}

function filterdetailRead7(e) {
    return { _menuId: menuId, _classDetailView: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_MemoKajian" };
}

function filterdetailRead8(e) {
    return { _menuId: menuId, _classDetailView: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_Peta" };
}

function filterdetailRead9(e) {
    return { _menuId: menuId, _classDetailView: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_MoU" };
}

function filterdetailRead10(e) {
    return { _menuId: menuId, _classDetailView: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_RisalahRakor" };
}

function filterdetailRead11(e) {
    return { _menuId: menuId, _classDetailView: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_IjinDireksi" };
}

function filterdetailRead12(e) {
    return { _menuId: menuId, _classDetailView: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_CRSA" };
}

function filterdetailRead13(e) {
    return { _menuId: menuId, _classDetailView: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_PKS" };
}

function filterdetailRead14(e) {
    return { _menuId: menuId, _classDetailView: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_Invoice" };
}

function filterdetailRead15(e) {
    return { _menuId: menuId, _classDetailView: "Ptpn8.Kerjasama.Models.ViewKerjasama_Dokumen_BuktiPembayaran" };
}

function filterdetailCreate(e) {
    return { _menuId: menuId };
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


//Minat
function uplOnSuccess(e) {
    uploadFile.close();
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_MinatId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_MinatId) : grid.dataSource.get(kerjasama_Dokumen_MinatId);
    //dataItem.NamaFilePDF_Minat = e.files[0].name;
    dataItem.NamaFilePDF_Minat = kerjasama_DokumenId+ "_" +kerjasama_Dokumen_MinatId + ".pdf";
    dataItem.dirty = true;
    grid.refresh();
}


function onUpload(e) {
    var namafile;
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_MinatId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_MinatId) : grid.dataSource.get(kerjasama_Dokumen_MinatId);
    namafile = dataItem.Kerjasama_DokumenId + "_" + dataItem.Kerjasama_Dokumen_MinatId + ".pdf"
    e.data = {
        namaFile: namafile
    }
}

function uploadData(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    kerjasama_Dokumen_MinatId = data.Kerjasama_Dokumen_MinatId;
    uploadFile.open().center();
}

//Tanggapan

function uplOnSuccess2(e) {
    uploadFile2.close();
    var grid = $('#grdDetail2').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_TanggapanId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_TanggapanId) : grid.dataSource.get(kerjasama_Dokumen_TanggapanId);
    //dataItem.NamaFilePDF_Minat = e.files[0].name;
    dataItem.NamaFilePDF_Tanggapan = kerjasama_DokumenId + "_" + kerjasama_Dokumen_TanggapanId + ".pdf";
    dataItem.dirty = true;
    grid.refresh();
}


function onUpload2(e) {
    var namafile;
    var grid = $('#grdDetail2').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_TanggapanId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_TanggapanId) : grid.dataSource.get(kerjasama_Dokumen_TanggapanId);
    namafile = dataItem.Kerjasama_DokumenId + "_" + dataItem.Kerjasama_Dokumen_TanggapanId + ".pdf"
    e.data = {
        namaFile: namafile
    }
}

function uploadData2(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    kerjasama_Dokumen_TanggapanId = data.Kerjasama_Dokumen_TanggapanId;
    uploadFile2.open().center();
}

//Legal
function uplOnSuccess3(e) {
    uploadFile3.close();
    var grid = $('#grdDetail3').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_LegalId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_LegalId) : grid.dataSource.get(kerjasama_Dokumen_LegalId);
    //dataItem.NamaFilePDF_Minat = e.files[0].name;
    dataItem.NamaFilePDF_Legal = kerjasama_DokumenId + "_" + kerjasama_Dokumen_LegalId + ".pdf";
    dataItem.dirty = true;
    grid.refresh();
}

function onUpload3(e) {
    var namafile;
    var grid = $('#grdDetail3').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_LegalId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_LegalId) : grid.dataSource.get(kerjasama_Dokumen_LegalId);
    namafile = dataItem.Kerjasama_DokumenId + "_" + dataItem.Kerjasama_Dokumen_LegalId + ".pdf"
    e.data = {
        namaFile: namafile
    }
}

function uploadData3(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    kerjasama_Dokumen_LegalId = data.Kerjasama_Dokumen_LegalId;
    uploadFile3.open().center();
}

//BASurvei
function uplOnSuccess4(e) {
    uploadFile4.close();
    var grid = $('#grdDetail4').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_BASurveiId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_BASurveiId) : grid.dataSource.get(kerjasama_Dokumen_BASurveiId);
    //dataItem.NamaFilePDF_Minat = e.files[0].name;
    dataItem.NamaFilePDF_BASurvei = kerjasama_DokumenId + "_" + kerjasama_Dokumen_BASurveiId + ".pdf";
    dataItem.dirty = true;
    grid.refresh();
}

function onUpload4(e) {
    var namafile;
    var grid = $('#grdDetail4').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_BASurveiId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_BASurveiId) : grid.dataSource.get(kerjasama_Dokumen_BASurveiId);
    namafile = dataItem.Kerjasama_DokumenId + "_" + dataItem.Kerjasama_Dokumen_BASurveiId + ".pdf"
    e.data = {
        namaFile: namafile
    }
}

function uploadData4(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    kerjasama_Dokumen_BASurveiId = data.Kerjasama_Dokumen_BASurveiId;
    uploadFile4.open().center();
}

//MOM
function uplOnSuccess5(e) {
    uploadFile5.close();
    var grid = $('#grdDetail5').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_MOMId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_MOMId) : grid.dataSource.get(kerjasama_Dokumen_MOMId);
    //dataItem.NamaFilePDF_Minat = e.files[0].name;
    dataItem.NamaFilePDF_MOM = kerjasama_DokumenId + "_" + kerjasama_Dokumen_MOMId + ".pdf";
    dataItem.dirty = true;
    grid.refresh();
}

function onUpload5(e) {
    var namafile;
    var grid = $('#grdDetail5').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_MOMId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_MOMId) : grid.dataSource.get(kerjasama_Dokumen_MOMId);
    namafile = dataItem.Kerjasama_DokumenId + "_" + dataItem.Kerjasama_Dokumen_MOMId + ".pdf"
    e.data = {
        namaFile: namafile
    }
}

function uploadData5(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    kerjasama_Dokumen_MOMId = data.Kerjasama_Dokumen_MOMId;
    uploadFile5.open().center();
}

//SaranPendapat
function uplOnSuccess6(e) {
    uploadFile6.close();
    var grid = $('#grdDetail6').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_SaranPendapatId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_SaranPendapatId) : grid.dataSource.get(kerjasama_Dokumen_SaranPendapatId);
    //dataItem.NamaFilePDF_Minat = e.files[0].name;
    dataItem.NamaFilePDF_SaranPendapat = kerjasama_DokumenId + "_" + kerjasama_Dokumen_SaranPendapatId + ".pdf";
    dataItem.dirty = true;
    grid.refresh();
}

function onUpload6(e) {
    var namafile;
    var grid = $('#grdDetail6').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_SaranPendapatId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_SaranPendapatId) : grid.dataSource.get(kerjasama_Dokumen_SaranPendapatId);
    namafile = dataItem.Kerjasama_DokumenId + "_" + dataItem.Kerjasama_Dokumen_SaranPendapatId + ".pdf"
    e.data = {
        namaFile: namafile
    }
}

function uploadData6(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    kerjasama_Dokumen_SaranPendapatId = data.Kerjasama_Dokumen_SaranPendapatId;
    uploadFile6.open().center();
}

//MemoKajian
function uplOnSuccess7(e) {
    uploadFile7.close();
    var grid = $('#grdDetail7').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_MemoKajianId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_MemoKajianId) : grid.dataSource.get(kerjasama_Dokumen_MemoKajianId);
    //dataItem.NamaFilePDF_Minat = e.files[0].name;
    dataItem.NamaFilePDF_MemoKajian = kerjasama_DokumenId + "_" + kerjasama_Dokumen_MemoKajianId + ".pdf";
    dataItem.dirty = true;
    grid.refresh();
}

function onUpload7(e) {
    var namafile;
    var grid = $('#grdDetail7').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_MemoKajianId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_MemoKajianId) : grid.dataSource.get(kerjasama_Dokumen_MemoKajianId);
    namafile = dataItem.Kerjasama_DokumenId + "_" + dataItem.Kerjasama_Dokumen_MemoKajianId + ".pdf"
    e.data = {
        namaFile: namafile
    }
}

function uploadData7(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    kerjasama_Dokumen_MemoKajianId = data.Kerjasama_Dokumen_MemoKajianId;
    uploadFile7.open().center();
}

//Peta
function uplOnSuccess8(e) {
    uploadFile8.close();
    var grid = $('#grdDetail8').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_PetaId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_PetaId) : grid.dataSource.get(kerjasama_Dokumen_PetaId);
    //dataItem.NamaFilePDF_Minat = e.files[0].name;
    dataItem.NamaFileSHP_Peta = kerjasama_DokumenId + "_" + kerjasama_Dokumen_PetaId + ".shp";
    dataItem.dirty = true;
    grid.refresh();
}

function onUpload8(e) {
    var namafile;
    var grid = $('#grdDetail8').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_PetaId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_PetaId) : grid.dataSource.get(kerjasama_Dokumen_PetaId);
    namafile = dataItem.Kerjasama_DokumenId + "_" + dataItem.Kerjasama_Dokumen_PetaId + ".shp"
    e.data = {
        namaFile: namafile
    }
}

function uploadData8(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    kerjasama_Dokumen_PetaId = data.Kerjasama_Dokumen_PetaId;
    uploadFile8.open().center();
}
//PetaPDF
function uplOnSuccess81(e) {
    uploadFile81.close();
    var grid = $('#grdDetail8').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_PetaId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_PetaId) : grid.dataSource.get(kerjasama_Dokumen_PetaId);
    //dataItem.NamaFilePDF_Minat = e.files[0].name;
    dataItem.NamaFileSHP_Peta = kerjasama_DokumenId + "_" + kerjasama_Dokumen_PetaId + ".pdf";
    dataItem.dirty = true;
    grid.refresh();
}

function onUpload81(e) {
    var namafile;
    var grid = $('#grdDetail8').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_PetaId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_PetaId) : grid.dataSource.get(kerjasama_Dokumen_PetaId);
    namafile = dataItem.Kerjasama_DokumenId + "_" + dataItem.Kerjasama_Dokumen_PetaId + ".pdf"
    e.data = {
        namaFile: namafile
    }
}

function uploadData81(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    kerjasama_Dokumen_PetaId = data.Kerjasama_Dokumen_PetaId;
    uploadFile81.open().center();
}

//PPT Rakor
function uplOnSuccess9(e) {
    uploadFile9.close();
    var grid = $('#grdDetail9').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_MoUId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_MoUId) : grid.dataSource.get(kerjasama_Dokumen_MoUId);
    //dataItem.NamaFilePDF_Minat = e.files[0].name;
    dataItem.NamaFilePDF_MoU = kerjasama_DokumenId + "_" + kerjasama_Dokumen_MoUId + ".pdf";
    dataItem.dirty = true;
    grid.refresh();
}

function onUpload9(e) {
    var namafile;
    var grid = $('#grdDetail9').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_MoUId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_MoUId) : grid.dataSource.get(kerjasama_Dokumen_MoUId);
    namafile = dataItem.Kerjasama_DokumenId + "_" + dataItem.Kerjasama_Dokumen_MoUId + ".pdf"
    e.data = {
        namaFile: namafile
    }
}

function uploadData9(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    kerjasama_Dokumen_MoUId = data.Kerjasama_Dokumen_MoUId;
    uploadFile9.open().center();
}

//RisalahRakor
function uplOnSuccess10(e) {
    uploadFile10.close();
    var grid = $('#grdDetail10').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_RisalahRakorId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_RisalahRakorId) : grid.dataSource.get(kerjasama_Dokumen_RisalahRakorId);
    //dataItem.NamaFilePDF_Minat = e.files[0].name;
    dataItem.NamaFilePDF_RisalahRakor = kerjasama_DokumenId + "_" + kerjasama_Dokumen_RisalahRakorId + ".pdf";
    dataItem.dirty = true;
    grid.refresh();
}

function onUpload10(e) {
    var namafile;
    var grid = $('#grdDetail10').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_RisalahRakorId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_RisalahRakorId) : grid.dataSource.get(kerjasama_Dokumen_RisalahRakorId);
    namafile = dataItem.Kerjasama_DokumenId + "_" + dataItem.Kerjasama_Dokumen_RisalahRakorId + ".pdf"
    e.data = {
        namaFile: namafile
    }
}

function uploadData10(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    kerjasama_Dokumen_RisalahRakorId = data.Kerjasama_Dokumen_RisalahRakorId;
    uploadFile10.open().center();
}

//IjinDireksi
function uplOnSuccess11(e) {
    uploadFile11.close();
    var grid = $('#grdDetail11').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_IjinDireksiId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_IjinDireksiId) : grid.dataSource.get(kerjasama_Dokumen_IjinDireksiId);
    //dataItem.NamaFilePDF_Minat = e.files[0].name;
    dataItem.NamaFilePDF_IjinDireksi = kerjasama_DokumenId + "_" + kerjasama_Dokumen_IjinDireksiId + ".pdf";
    dataItem.dirty = true;
    grid.refresh();
}

function onUpload11(e) {
    var namafile;
    var grid = $('#grdDetail11').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_IjinDireksiId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_IjinDireksiId) : grid.dataSource.get(kerjasama_Dokumen_IjinDireksiId);
    namafile = dataItem.Kerjasama_DokumenId + "_" + dataItem.Kerjasama_Dokumen_IjinDireksiId + ".pdf"
    e.data = {
        namaFile: namafile
    }
}

function uploadData11(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    kerjasama_Dokumen_IjinDireksiId = data.Kerjasama_Dokumen_IjinDireksiId;
    uploadFile11.open().center();
}

//CRSA
function uplOnSuccess12(e) {
    uploadFile12.close();
    var grid = $('#grdDetail12').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_CRSAId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_CRSAId) : grid.dataSource.get(kerjasama_Dokumen_CRSAId);
    //dataItem.NamaFilePDF_Minat = e.files[0].name;
    dataItem.NamaFilePDF_CRSA = kerjasama_DokumenId + "_" + kerjasama_Dokumen_CRSAId + ".pdf";
    dataItem.dirty = true;
    grid.refresh();
}

function onUpload12(e) {
    var namafile;
    var grid = $('#grdDetail12').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_CRSAId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_CRSAId) : grid.dataSource.get(kerjasama_Dokumen_CRSAId);
    namafile = dataItem.Kerjasama_DokumenId + "_" + dataItem.Kerjasama_Dokumen_CRSAId + ".pdf"
    e.data = {
        namaFile: namafile
    }
}

function uploadData12(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    kerjasama_Dokumen_CRSAId = data.Kerjasama_Dokumen_CRSAId;
    uploadFile12.open().center();
}

//PKS
function uplOnSuccess13(e) {
    uploadFile13.close();
    var grid = $('#grdDetail13').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_PKSId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_PKSId) : grid.dataSource.get(kerjasama_Dokumen_PKSId);
    //dataItem.NamaFilePDF_Minat = e.files[0].name;
    dataItem.NamaFilePDF_PKS = kerjasama_DokumenId + "_" + kerjasama_Dokumen_PKSId + ".pdf";
    dataItem.dirty = true;
    grid.refresh();
}

function onUpload13(e) {
    var namafile;
    var grid = $('#grdDetail13').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_PKSId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_PKSId) : grid.dataSource.get(kerjasama_Dokumen_PKSId);
    namafile = dataItem.Kerjasama_DokumenId + "_" + dataItem.Kerjasama_Dokumen_PKSId + ".pdf"
    e.data = {
        namaFile: namafile
    }
}

function uploadData13(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    kerjasama_Dokumen_PKSId = data.Kerjasama_Dokumen_PKSId;
    uploadFile13.open().center();
}

//Invoice
function uplOnSuccess14(e) {
    uploadFile14.close();
    var grid = $('#grdDetail14').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_InvoiceId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_InvoiceId) : grid.dataSource.get(kerjasama_Dokumen_InvoiceId);
    //dataItem.NamaFilePDF_Minat = e.files[0].name;
    dataItem.NamaFilePDF_Invoice = kerjasama_DokumenId + "_" + kerjasama_Dokumen_InvoiceId + ".pdf";
    dataItem.dirty = true;
    grid.refresh();
}

function onUpload14(e) {
    var namafile;
    var grid = $('#grdDetail14').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_InvoiceId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_InvoiceId) : grid.dataSource.get(kerjasama_Dokumen_InvoiceId);
    namafile = dataItem.Kerjasama_DokumenId + "_" + dataItem.Kerjasama_Dokumen_InvoiceId + ".pdf"
    e.data = {
        namaFile: namafile
    }
}

function uploadData14(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    kerjasama_Dokumen_InvoiceId = data.Kerjasama_Dokumen_InvoiceId;
    uploadFile14.open().center();
}

//BuktiPembayaran
function uplOnSuccess15(e) {
    uploadFile15.close();
    var grid = $('#grdDetail14').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_InvoiceId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_InvoiceId) : grid.dataSource.get(kerjasama_Dokumen_InvoiceId);
    //dataItem.NamaFilePDF_Minat = e.files[0].name;
    dataItem.NamaFilePDF_BuktiPembayaran = kerjasama_DokumenId + "_BuktiPembayaran_" + kerjasama_Dokumen_InvoiceId + ".pdf";
    dataItem.dirty = true;
    grid.refresh();
}

function onUpload15(e) {
    var namafile;
    var grid = $('#grdDetail14').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_InvoiceId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_InvoiceId) : grid.dataSource.get(kerjasama_Dokumen_InvoiceId);
    namafile = dataItem.Kerjasama_DokumenId + "_BuktiPembayaran_" + dataItem.Kerjasama_Dokumen_InvoiceId + ".pdf"
    e.data = {
        namaFile: namafile
    }
}

function uploadData15(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var data = grid.dataItem(row);
    kerjasama_Dokumen_BuktiPembayaranId = data.Kerjasama_Dokumen_InvoiceId;
    uploadFile15.open().center();
}

//Minat
function onButtonClick(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");

    var data = grid.dataItem(row);
    var file = data.NamaFilePDF_Minat;

    //window.location.href = "/Content/Images/Document/" + file; //pass the desired url for the view
    pdf.open().center();

    var options = {
        pdfOpenParams: {
            navpanes: 1,
            toolbar: 0,
            statusbar: 0,
            pagemode: 'none',
            pagemode: "none",
            page: 1,
            preventDefault: 1,
            zoom: "page-width",
            enableHandToolOnLoad: true
        } //,
        //forcePDFJS: true,
        //PDFJS_URL: "/PDF.js/web/viewer.html"
    }


    PDFObject.embed("/Files/Kerjasama/Minat/" + file, "#testPDFObject", options)

}

//Tanggapan
function onButtonClick2(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");

    var data = grid.dataItem(row);
    var file = data.NamaFilePDF_Tanggapan;

    //window.location.href = "/Content/Images/Document/" + file; //pass the desired url for the view
    pdf.open().center();

    var options = {
        pdfOpenParams: {
            navpanes: 1,
            toolbar: 0,
            statusbar: 0,
            pagemode: 'none',
            pagemode: "none",
            page: 1,
            preventDefault: 1,
            zoom: "page-width",
            enableHandToolOnLoad: true
        } //,
        //forcePDFJS: true,
        //PDFJS_URL: "/PDF.js/web/viewer.html"
    }


    PDFObject.embed("/Files/Kerjasama/Tanggapan/" + file, "#testPDFObject", options)

}

//Legal
function onButtonClick3(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");

    var data = grid.dataItem(row);
    var file = data.NamaFilePDF_Legal;

    //window.location.href = "/Content/Images/Document/" + file; //pass the desired url for the view
    pdf.open().center();

    var options = {
        pdfOpenParams: {
            navpanes: 1,
            toolbar: 0,
            statusbar: 0,
            pagemode: 'none',
            pagemode: "none",
            page: 1,
            preventDefault: 1,
            zoom: "page-width",
            enableHandToolOnLoad: true
        } //,
        //forcePDFJS: true,
        //PDFJS_URL: "/PDF.js/web/viewer.html"
    }


    PDFObject.embed("/Files/Kerjasama/Legal/" + file, "#testPDFObject", options)

}

//BASurvei
function onButtonClick4(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");

    var data = grid.dataItem(row);
    var file = data.NamaFilePDF_BASurvei;

    //window.location.href = "/Content/Images/Document/" + file; //pass the desired url for the view
    pdf.open().center();

    var options = {
        pdfOpenParams: {
            navpanes: 1,
            toolbar: 0,
            statusbar: 0,
            pagemode: 'none',
            pagemode: "none",
            page: 1,
            preventDefault: 1,
            zoom: "page-width",
            enableHandToolOnLoad: true
        } //,
        //forcePDFJS: true,
        //PDFJS_URL: "/PDF.js/web/viewer.html"
    }


    PDFObject.embed("/Files/Kerjasama/BASurvei/" + file, "#testPDFObject", options)

}

//MOM
function onButtonClick5(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");

    var data = grid.dataItem(row);
    var file = data.NamaFilePDF_MOM;

    //window.location.href = "/Content/Images/Document/" + file; //pass the desired url for the view
    pdf.open().center();

    var options = {
        pdfOpenParams: {
            navpanes: 1,
            toolbar: 0,
            statusbar: 0,
            pagemode: 'none',
            pagemode: "none",
            page: 1,
            preventDefault: 1,
            zoom: "page-width",
            enableHandToolOnLoad: true
        } //,
        //forcePDFJS: true,
        //PDFJS_URL: "/PDF.js/web/viewer.html"
    }


    PDFObject.embed("/Files/Kerjasama/MOM/" + file, "#testPDFObject", options)

}

//SaranPendapat
function onButtonClick6(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");

    var data = grid.dataItem(row);
    var file = data.NamaFilePDF_SaranPendapat;

    //window.location.href = "/Content/Images/Document/" + file; //pass the desired url for the view
    pdf.open().center();

    var options = {
        pdfOpenParams: {
            navpanes: 1,
            toolbar: 0,
            statusbar: 0,
            pagemode: 'none',
            pagemode: "none",
            page: 1,
            preventDefault: 1,
            zoom: "page-width",
            enableHandToolOnLoad: true
        } //,
        //forcePDFJS: true,
        //PDFJS_URL: "/PDF.js/web/viewer.html"
    }


    PDFObject.embed("/Files/Kerjasama/SaranPendapat/" + file, "#testPDFObject", options)

}

//MemoKajian
function onButtonClick7(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");

    var data = grid.dataItem(row);
    var file = data.NamaFilePDF_MemoKajian;

    //window.location.href = "/Content/Images/Document/" + file; //pass the desired url for the view
    pdf.open().center();

    var options = {
        pdfOpenParams: {
            navpanes: 1,
            toolbar: 0,
            statusbar: 0,
            pagemode: 'none',
            pagemode: "none",
            page: 1,
            preventDefault: 1,
            zoom: "page-width",
            enableHandToolOnLoad: true
        } //,
        //forcePDFJS: true,
        //PDFJS_URL: "/PDF.js/web/viewer.html"
    }


    PDFObject.embed("/Files/Kerjasama/MemoKajian/" + file, "#testPDFObject", options)

}

//Peta
function onButtonClick8(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");

    var data = grid.dataItem(row);
    var file = data.NamaFileSHP_Peta;

    //window.location.href = "/Content/Images/Document/" + file; //pass the desired url for the view
    pdf.open().center();

    var options = {
        pdfOpenParams: {
            navpanes: 1,
            toolbar: 0,
            statusbar: 0,
            pagemode: 'none',
            pagemode: "none",
            page: 1,
            preventDefault: 1,
            zoom: "page-width",
            enableHandToolOnLoad: true
        } //,
        //forcePDFJS: true,
        //PDFJS_URL: "/PDF.js/web/viewer.html"
    }


    PDFObject.embed("/Files/Kerjasama/Peta/" + file, "#testPDFObject", options)

}


function onButtonClick81(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");

    var data = grid.dataItem(row);
    var file = data.NamaFileSHP_PetaPDF;

    //window.location.href = "/Content/Images/Document/" + file; //pass the desired url for the view
    pdf.open().center();

    var options = {
        pdfOpenParams: {
            navpanes: 1,
            toolbar: 0,
            statusbar: 0,
            pagemode: 'none',
            pagemode: "none",
            page: 1,
            preventDefault: 1,
            zoom: "page-width",
            enableHandToolOnLoad: true
        } //,
        //forcePDFJS: true,
        //PDFJS_URL: "/PDF.js/web/viewer.html"
    }


    PDFObject.embed("/Files/Kerjasama/Peta/" + file, "#testPDFObject", options)

}
//MoU
function onButtonClick9(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");

    var data = grid.dataItem(row);
    var file = data.NamaFilePDF_MoU;

    //window.location.href = "/Content/Images/Document/" + file; //pass the desired url for the view
    pdf.open().center();

    var options = {
        pdfOpenParams: {
            navpanes: 1,
            toolbar: 0,
            statusbar: 0,
            pagemode: 'none',
            pagemode: "none",
            page: 1,
            preventDefault: 1,
            zoom: "page-width",
            enableHandToolOnLoad: true
        } //,
        //forcePDFJS: true,
        //PDFJS_URL: "/PDF.js/web/viewer.html"
    }


    PDFObject.embed("/Files/Kerjasama/MoU/" + file, "#testPDFObject", options)

}

//RisalahRakor
function onButtonClick10(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");

    var data = grid.dataItem(row);
    var file = data.NamaFilePDF_RisalahRakor;

    //window.location.href = "/Content/Images/Document/" + file; //pass the desired url for the view
    pdf.open().center();

    var options = {
        pdfOpenParams: {
            navpanes: 1,
            toolbar: 0,
            statusbar: 0,
            pagemode: 'none',
            pagemode: "none",
            page: 1,
            preventDefault: 1,
            zoom: "page-width",
            enableHandToolOnLoad: true
        } //,
        //forcePDFJS: true,
        //PDFJS_URL: "/PDF.js/web/viewer.html"
    }


    PDFObject.embed("/Files/Kerjasama/RisalahRakor/" + file, "#testPDFObject", options)

}

//IjinDireksi
function onButtonClick11(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");

    var data = grid.dataItem(row);
    var file = data.NamaFilePDF_IjinDireksi;

    //window.location.href = "/Content/Images/Document/" + file; //pass the desired url for the view
    pdf.open().center();

    var options = {
        pdfOpenParams: {
            navpanes: 1,
            toolbar: 0,
            statusbar: 0,
            pagemode: 'none',
            pagemode: "none",
            page: 1,
            preventDefault: 1,
            zoom: "page-width",
            enableHandToolOnLoad: true
        } //,
        //forcePDFJS: true,
        //PDFJS_URL: "/PDF.js/web/viewer.html"
    }


    PDFObject.embed("/Files/Kerjasama/IjinDireksi/" + file, "#testPDFObject", options)

}

//CRSA
function onButtonClick12(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");

    var data = grid.dataItem(row);
    var file = data.NamaFilePDF_CRSA;

    //window.location.href = "/Content/Images/Document/" + file; //pass the desired url for the view
    pdf.open().center();

    var options = {
        pdfOpenParams: {
            navpanes: 1,
            toolbar: 0,
            statusbar: 0,
            pagemode: 'none',
            pagemode: "none",
            page: 1,
            preventDefault: 1,
            zoom: "page-width",
            enableHandToolOnLoad: true
        } //,
        //forcePDFJS: true,
        //PDFJS_URL: "/PDF.js/web/viewer.html"
    }


    PDFObject.embed("/Files/Kerjasama/CRSA/" + file, "#testPDFObject", options)

}

//PKS
function onButtonClick13(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");

    var data = grid.dataItem(row);
    var file = data.NamaFilePDF_PKS;

    //window.location.href = "/Content/Images/Document/" + file; //pass the desired url for the view
    pdf.open().center();

    var options = {
        pdfOpenParams: {
            navpanes: 1,
            toolbar: 0,
            statusbar: 0,
            pagemode: 'none',
            pagemode: "none",
            page: 1,
            preventDefault: 1,
            zoom: "page-width",
            enableHandToolOnLoad: true
        } //,
        //forcePDFJS: true,
        //PDFJS_URL: "/PDF.js/web/viewer.html"
    }


    PDFObject.embed("/Files/Kerjasama/PKS/" + file, "#testPDFObject", options)

}

//Invoice
function onButtonClick14(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");

    var data = grid.dataItem(row);
    var file = data.NamaFilePDF_Invoice;

    //window.location.href = "/Content/Images/Document/" + file; //pass the desired url for the view
    pdf.open().center();

    var options = {
        pdfOpenParams: {
            navpanes: 1,
            toolbar: 0,
            statusbar: 0,
            pagemode: 'none',
            pagemode: "none",
            page: 1,
            preventDefault: 1,
            zoom: "page-width",
            enableHandToolOnLoad: true
        } //,
        //forcePDFJS: true,
        //PDFJS_URL: "/PDF.js/web/viewer.html"
    }


    PDFObject.embed("/Files/Kerjasama/Invoice/" + file, "#testPDFObject", options)

}

//BuktiPembayaran
function onButtonClick15(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");

    var data = grid.dataItem(row);
    var file = data.NamaFilePDF_BuktiPembayaran;

    //window.location.href = "/Content/Images/Document/" + file; //pass the desired url for the view
    pdf.open().center();

    var options = {
        pdfOpenParams: {
            navpanes: 1,
            toolbar: 0,
            statusbar: 0,
            pagemode: 'none',
            pagemode: "none",
            page: 1,
            preventDefault: 1,
            zoom: "page-width",
            enableHandToolOnLoad: true
        } //,
        //forcePDFJS: true,
        //PDFJS_URL: "/PDF.js/web/viewer.html"
    }


    PDFObject.embed("/Files/Kerjasama/BuktiPembayaran/" + file, "#testPDFObject", options)

}

function filterProyek() {
    return {
        strClassView: 'Ptpn8.Kerjasama.Models.ViewHDR_Proyek',
        scriptSQL: "select DISTINCT A.HDR_ProyekId, A.No_Register from [KerjasamaEF].[dbo].[Kerjasama_HDRProyek] as A" +
            " where A.BagianId='" + $("#BagianId").val() + "'",
        _menuId: menuId
    }
}

function ProyekOnSelect(e) {
    var proyek = this.dataItem(e.item);
    proyekId = proyek.HDR_ProyekId;
}

function filterNomorInvoice() {
    return {
        strClassView: 'Ptpn8.Kerjasama.Models.Kerjasama_Dokumen_Invoice',
        scriptSQL: "Select Kerjasama_Dokumen_InvoiceId, Nomor_Invoice, Nominal from KerjasamaEF.dbo.Kerjasama_Dokumen_Invoice where Kerjasama_DokumenId ='" + $('#Kerjasama_DokumenId').val() + "'",
        _menuId: menuId
    }
}

function aucNomorInvoiceOnDataBound(e) {

}

function aucNomorInvoiceOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail15').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(kerjasama_Dokumen_BuktiPembayaranId)) == "undefined" ? grid.dataSource.getByUid(kerjasama_Dokumen_BuktiPembayaranId) : grid.dataSource.get(kerjasama_Dokumen_BuktiPembayaranId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.Nomor_Invoice = ddlItem.Nomor_Invoice;
    data.Kerjasama_Dokumen_InvoiceId = ddlItem.Kerjasama_Dokumen_InvoiceId;
    data.Nominal = ddlItem.Nominal;
    nominal = ddlItem.Nominal;
}