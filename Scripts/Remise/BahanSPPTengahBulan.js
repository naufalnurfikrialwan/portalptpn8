var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var hdr_BahanSPPTengahBulanId, kebunId, barangId, namaBarang, norek, dtl_BahanSPPTengahBulanId, rekeningId;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var rowNumber = 0;
var menuId = '4ee16602-52d5-4253-bff7-98b99c9e84ad';
var IHTBukan = 'Bukan';

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

    $('#grdDetail').kendoValidator({
        rules: {
            validated: function (input) {
                if (input.attr("name") == "Bayar_Jumlah_BarangBahan" || input.attr("name") == "Bayar_Jumlah_BiayaUmum" ||
                    input.attr("name") == "Bayar_PembelianProduksi" || input.attr("name") == "Bayar_UangMuka" ||
                    input.attr("name") == "Bayar_ModalKerjaDiluarPKB") {

                    var namaField = input.attr("name");
                    var grd = $('#grdDetail').data('kendoGrid');
                    var ds = grd.dataSource;
                    var dataItem = typeof (grd.dataSource.get(dtl_BahanSPPTengahBulanId)) == "undefined" ? grd.dataSource.getByUid(dtl_BahanSPPTengahBulanId) : grd.dataSource.get(dtl_BahanSPPTengahBulanId);
                    var row = $('#grdDetail').find('tbody>tr[data-uid=' + dataItem.uid + ']');

                    dataItem[namaField] = input[0].checked;
                    var results = grd.dataSource.data();
                    var total_Dibayar = 0;
                    for (var i = 0; i < results.length; i++) {
                        var result = results[i];
                        result['Total_Dibayar'] = hitungJumlahDibayar(result);
                        total_Dibayar = total_Dibayar + parseFloat(result['Total_Dibayar']);
                    }
                    ds.aggregates().Total_Dibayar.sum = total_Dibayar;
                    grd.refresh();
                }
            }

        }


    });

    namaKebunOnChange().done(function (data) {
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

    $('#Bayar_Jumlah_BarangBahanSemua').click(function (e) {
        var grd = $('#grdDetail').data('kendoGrid');
        var ds = grd.dataSource;
        var $cb = $(this);
        var checked = $cb.is(':checked');
        var results = grd.dataSource.data();
        var total_Dibayar = 0;
        for (var i = 0; i < results.length; i++) {
            var result = results[i];
            result['Bayar_Jumlah_BarangBahan'] = checked;
            result['Total_Dibayar'] = hitungJumlahDibayar(result);
            total_Dibayar = total_Dibayar + parseFloat(result['Total_Dibayar']);
        }

        ds.aggregates().Total_Dibayar.sum = total_Dibayar;
        grd.refresh();

    });

    $('#Bayar_Jumlah_BiayaUmumSemua').click(function (e) {
        var grd = $('#grdDetail').data('kendoGrid');
        var ds = grd.dataSource;
        var $cb = $(this);
        var checked = $cb.is(':checked');
        var results = grd.dataSource.data();
        var total_Dibayar = 0;
        for (var i = 0; i < results.length; i++) {
            var result = results[i];
            result['Bayar_Jumlah_BiayaUmum'] = checked;
            result['Total_Dibayar'] = hitungJumlahDibayar(result);
            total_Dibayar = total_Dibayar + parseFloat(result['Total_Dibayar']);
        }
        ds.aggregates().Total_Dibayar.sum = total_Dibayar;
        grd.refresh();
    });

    $('#Bayar_PembelianProduksiSemua').click(function (e) {
        var grd = $('#grdDetail').data('kendoGrid');
        var ds = grd.dataSource;
        var $cb = $(this);
        var checked = $cb.is(':checked');
        var results = grd.dataSource.data();
        var total_Dibayar = 0;
        for (var i = 0; i < results.length; i++) {
            var result = results[i];
            result['Bayar_PembelianProduksi'] = checked;
            result['Total_Dibayar'] = hitungJumlahDibayar(result);
            total_Dibayar = total_Dibayar + parseFloat(result['Total_Dibayar']);
        }
        grd.refresh();
    });

    $('#Bayar_UangMukaSemua').click(function (e) {
        var grd = $('#grdDetail').data('kendoGrid');
        var ds = grd.dataSource;
        var $cb = $(this);
        var checked = $cb.is(':checked');
        var results = grd.dataSource.data();
        var total_Dibayar = 0;
        for (var i = 0; i < results.length; i++) {
            var result = results[i];
            result['Bayar_UangMuka'] = checked;
            result['Total_Dibayar'] = hitungJumlahDibayar(result);
            total_Dibayar = total_Dibayar + parseFloat(result['Total_Dibayar']);
        }
        ds.aggregates().Total_Dibayar.sum = total_Dibayar;
        grd.refresh();
    });

    $('#Bayar_ModalKerjaDiluarPKBSemua').click(function (e) {
        var grd = $('#grdDetail').data('kendoGrid');
        var ds = grd.dataSource;
        var $cb = $(this);
        var checked = $cb.is(':checked');
        var results = grd.dataSource.data();
        var total_Dibayar = 0;
        for (var i = 0; i < results.length; i++) {
            var result = results[i];
            result['Bayar_ModalKerjaDiluarPKB'] = checked;
            result['Total_Dibayar'] = hitungJumlahDibayar(result);
            total_Dibayar = total_Dibayar + parseFloat(result['Total_Dibayar']);
        }
        ds.aggregates().Total_Dibayar.sum = total_Dibayar;
        grd.refresh();
    });

});


function hitungJumlahDibayar(dataItem) {
    var jumlahDibayar = 0;
    jumlahDibayar = jumlahDibayar +
        (dataItem["Bayar_Jumlah_BarangBahan"] == true ? parseFloat(dataItem["Jumlah_BarangBahan"]) : 0) +
        (dataItem["Bayar_Jumlah_BiayaUmum"] == true ? parseFloat(dataItem["Jumlah_BiayaUmum"]) : 0) +
        (dataItem["Bayar_PembelianProduksi"] == true ? parseFloat(dataItem["PembelianProduksi"]) : 0) +
        (dataItem["Bayar_UangMuka"] == true ? parseFloat(dataItem["UangMuka"]) : 0) +
        (dataItem["Bayar_ModalKerjaDiluarPKB"] == true ? parseFloat(dataItem["ModalKerjaDiluarPKB"]) : 0);

    return jumlahDibayar;
}

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
                        gridStatus = 'belum-ngapa-ngapain';
                        hdr_BahanSPPTengahBulanId = $('#HDR_BahanSPPTengahBulanId').val();
                        $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                            disableHeader();
                            $('#btnDeleteHeader').removeClass('disabledbutton');
                            $('#btnPrintHeader').removeClass('disabledbutton');
                            $('#btnDeleteHeader').attr('disabled', false);
                            $('#btnPrintHeader').attr('disabled', false);
                            $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                            $('#grdDetail').removeClass('disabledbutton');

                            getDataFrom3().done(function (results) {
                                if (results == 0) {
                                    InsertGridFrom().done(function (data) {
                                        $("#grdDetail").data("kendoGrid").dataSource.read({ id: $('#HDR_BahanSPPTengahBulanId').val() });
                                    }).fail(function (x) {
                                        alert('Error! Hubungi Bagian TI');
                                    });
                                }
                                else {
                                    var grid = $("#grdDetail").data("kendoGrid");
                                    grid.dataSource.read({
                                        id: $('#HDR_BahanSPPTengahBulanId').val()
                                    }).done(function () {
                                        var currentData = grid.dataSource.data();
                                        for (var i = 0; i < currentData.length; i++) {
                                            currentData[i].dirty = true;
                                        }
                                    });
                                }
                            });
                        });
                    }
                    else {
                        openErrWindow();
                        $('#grdDetail').addClass('disabledbutton');
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
    $('#HDR_BahanSPPTengahBulanId').val(_NomorInputView.HDR_BahanSPPTengahBulanId);
    $('#TahunBuku').val(_NomorInputView.TahunBuku);
    $('#BulanRemise').data('kendoDropDownList').value(_NomorInputView.BulanRemise);
    $('#TahunRemise').val(_NomorInputView.TahunRemise);
    $('#NomorInput').val(_NomorInputView.NomorInput);
    $('#NomorInputView').val(_NomorInputView.NomorInputView);
    $('#TanggalInput').val(new Date(parseInt(_NomorInputView.TanggalInput.substr(6))));
    $('#Keterangan').val(_NomorInputView.Keterangan);
    $('#NoSPP').val(_NomorInputView.NoSPP);
    $('#UserName').val(_NomorInputView.UserName);
}


function ViewToModel() {
    var mdl = {
        HDR_BahanSPPTengahBulanId: $('#HDR_BahanSPPTengahBulanId').val(),
        TahunBuku: $('#TahunBuku').val(),
        BulanRemise: $('#BulanRemise').val(),
        TahunRemise: $('#TahunRemise').val(),
        NoSPP: $('#NoSPP').val(),
        Keterangan: $('#Keterangan').val(),
        NomorInput: $('#NomorInput').val(),
        NomorInputView: $('#NomorInput').val(),
        TanggalInput: $('#TanggalInput').val(),
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
                    hdr_PermintaanBarangDariKebunId = $('#HDR_BahanSPPTengahBulanId').val();
                    //$('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                    disableHeader();
                    $('#btnDeleteHeader').removeClass('disabledbutton');
                    $('#btnPrintHeader').removeClass('disabledbutton');
                    $('#btnDeleteHeader').attr('disabled', false);
                    $('#btnPrintHeader').attr('disabled', false);
                    $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                    $('#grdDetail').removeClass('disabledbutton');
                    $("#grdDetail").data("kendoGrid").dataSource.read({
                        id: $('#HDR_BahanSPPTengahBulanId').val()
                    });
                    //});
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_BahanSPPTengahBulan]" +
                " where HDR_BahanSPPTengahBulanId='" + $("#HDR_BahanSPPTengahBulanId").val() + "'",
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
        hdr_BahanSPPTengahBulanId = _NomorInputView.HDR_BahanSPPTengahBulanId;
        $('#BulanRemise').focus();
    }
}

function getDataFrom4() {
    // Hitung Jumlah Record di Detail
    var url = '/Input/GetDataFrom';
    return $.ajax({
        url: url,
        data: {
            strClassView: "Ptpn8.Remise.Models.View_HDRBahanSPPTengahBulan",
            scriptSQL: "select A.* from [SPDK_KanpusEF].[dbo].[HDR_BahanSPPTengahBulanId] A where BulanRemise=" + $('#BulanRemise').val() + " and TahunRemise=" + $('#TahunRemise').val(),
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
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
        parameters: { HDR_BahanSPPTengahBulanId: $('#HDR_BahanSPPTengahBulanId').val() }
    });
    viewer.refreshReport();
    prt.open().center();
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
// Detail


function updateFooters(detailItems) {
    var grid = $('#grdDetail').data('kendoGrid');
    var groupFooterIndex = 0;
    var groupFooters = grid.tbody.children(".k-group-footer");

    function updateGroupFooters(items) {
        for (var idx = 0; idx < items.length; idx++) {
            var item = items[idx];
            if (item.hasSubgroups) {
                updateGroupFooters(item.items);
            }
            groupFooters.eq(groupFooterIndex).replaceWith(grid.groupFooterTemplate(item.aggregates));
            groupFooterIndex++;
        }
    }

    updateGroupFooters(detailItems);
    grid.footer.find(".k-footer-template").replaceWith(grid.footerTemplate(this.aggregates()));
}

function grdOnChange(e) {
    var grid = $('#grdDetail').data('kendoGrid');

}

function grdOnSave(e) {
}

function grdOnEdit(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.DTL_BahanSPPTengahBulanId = model.uid;
        model.HDR_BahanSPPTengahBulanId = hdr_BahanSPPTengahBulanId;
    }

    dtl_BahanSPPTengahBulanId = model.DTL_BahanSPPTengahBulanId
    kebunId = $('#KebunId').val();
    gridStatus = 'sudah-diapa-apain';

    $(e.container).find("input[name='BarangBahan_Investasi']").prop('disabled', true);
    $(e.container).find("input[name='BarangBahan_Rutin']").prop('disabled', true);
    $(e.container).find("input[name='BarangBahan_Pengepakan']").prop('disabled', true);
    $(e.container).find("input[name='BarangBahan_BBM']").prop('disabled', true);
    $(e.container).find("input[name='BarangBahan_BBP']").prop('disabled', true);
    $(e.container).find("input[name='BarangBahan_Lainnya']").prop('disabled', true);
    $(e.container).find("input[name='Jumlah_BarangBahan']").prop('disabled', true);
    $(e.container).find("input[name='BiayaUmumRekg403']").prop('disabled', true);
    $(e.container).find("input[name='BiayaUmumRekg404']").prop('disabled', true);
    $(e.container).find("input[name='BiayaUmumRekg405']").prop('disabled', true);
    $(e.container).find("input[name='BiayaUmumRekg406']").prop('disabled', true);
    $(e.container).find("input[name='BiayaUmumRekg40604']").prop('disabled', true);
    $(e.container).find("input[name='BiayaUmumRekg407']").prop('disabled', true);
    $(e.container).find("input[name='BiayaUmumRekg410']").prop('disabled', true);
    $(e.container).find("input[name='BiayaUmumRekg411']").prop('disabled', true);
    $(e.container).find("input[name='BiayaUmumRekg412']").prop('disabled', true);
    $(e.container).find("input[name='BiayaUmumRekg413']").prop('disabled', true);
    $(e.container).find("input[name='BiayaUmumRekg414']").prop('disabled', true);
    $(e.container).find("input[name='BiayaUmumRekg420']").prop('disabled', true);
    $(e.container).find("input[name='BiayaUmumRekg421']").prop('disabled', true);
    $(e.container).find("input[name='BiayaUmumRekg422']").prop('disabled', true);
    $(e.container).find("input[name='BiayaUmumRekg423']").prop('disabled', true);
    $(e.container).find("input[name='BiayaUmumRekg424']").prop('disabled', true);
    $(e.container).find("input[name='BiayaUmumRekg425']").prop('disabled', true);
    $(e.container).find("input[name='BiayaUmumRekg426']").prop('disabled', true);
    $(e.container).find("input[name='BiayaUmumRekg200']").prop('disabled', true);
    $(e.container).find("input[name='BiayaUmumRekg201']").prop('disabled', true);
    $(e.container).find("input[name='BiayaUmumRekg601603']").prop('disabled', true);
    $(e.container).find("input[name='BiayaUmumRekg640']").prop('disabled', true);
    $(e.container).find("input[name='Jumlah_BiayaUmum']").prop('disabled', true);
    $(e.container).find("input[name='PembelianProduksi']").prop('disabled', true);
    $(e.container).find("input[name='UangMuka']").prop('disabled', true);
    $(e.container).find("input[name='ModalKerjaDiluarPKB']").prop('disabled', true);
    $(e.container).find("input[name='Total_Dibayar']").prop('disabled', true);

}

function hitungJumlah(id) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(id)) == "undefined" ? grid.dataSource.getByUid(id) : grid.dataSource.get(id);
    var newValue = 0;
    newValue = dataItem.Qty_Kebutuhan * dataItem.HargaSatuan;
    return newValue;
}

function filterkebun(e) {
    return {
        kebunId: $('#KebunId').val(),
        value: $("#NamaKebun").val()
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
            _menuId: menuId,
            mainBudidayaId: '3D662350-146F-48B0-A181-5A840330C451'
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

function filterGridUpdate(e) {
    var mdl = e;
    var arr = [];
    for (var i = 0; i < mdl.length; i++) {
        arr.push(JSON.stringify(mdl[i]));
    }
    return {
        _model: arr,
        _menuId: menuId
    };
}

function getDataFrom3() {
    // Hitung Jumlah Record di Detail
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from  [SPDK_KanpusEF].[dbo].[DTL_BahanSPPTengahBulan]" +
                " where HDR_BahanSPPTengahBulanId='" + $("#HDR_BahanSPPTengahBulanId").val() + "'",
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
            strClassView: "Ptpn8.Remise.Models.View_DTLBahanSPPTengahBulan",
            scriptSQL: "exec [SPDK_KanpusEF].[dbo].[BahanSPPTengahBulan] " + $('#BulanRemise').val() + "," + $('#TahunRemise').val() + ",'" + $('#HDR_BahanSPPTengahBulanId').val() + "'",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function grdOnBound(e) {

}
