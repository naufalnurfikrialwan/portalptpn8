var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var kebun;
var hdr_AlokasiId, mainBudidayaId, subBudidayaId, dtl_AlokasiId;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var rowNumber = 0;
var menuId = 'd7c3d0c1-7045-4b8c-a648-b9066486ab07';

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
	    var h2 = parseInt($(".hdr").css("height")) + parseInt($("._headerteh").css("height"));
	    var h3 = parseInt($(".k-grid-toolbar").css("height"));
	    var h4 = parseInt($(".k-grid-header-wrap").css("height"));
	    var h5 = parseInt($(".__footer").css("height")) + 20;
	    //$("#grdDetail").css("height", window.screen.height - (h1 + h2 + h3 + h4 + h5));

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
                if (input.attr("name") == "Chop" &&  input.val() != "") {
                    cekNoChop($('#Chop').val()).done(function (data) {
                        if (data.length > 0) {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_AlokasiId)) == "undefined" ? grid.dataSource.getByUid(dtl_AlokasiId) : grid.dataSource.get(dtl_AlokasiId);
                            dataItem.GradeId = data[0][0];
                            $('#GradeId').val(data[0][0]);
                            $('#NamaGrade').val(data[0][1]);
                            dataItem.QtySacks = data[0][2];
                            $('#QtySacks').val(data[0][2]);
                            dataItem.Gross = data[0][3];
                            $('#Gross').val(data[0][3]);
                            dataItem.Tare = data[0][4];
                            $('#Tare').val(data[0][4]);
                            dataItem.SatuanId = data[0][5];
                            $('#SatuanId').val(data[0][5]);
                            $('#NamaSatuan').val(data[0][6]);
                            dataItem.DTL_ProductSampleId = data[0][7];
                            $('#DTL_ProductSampleId').val(data[0][7]);
                            dataItem.AlokasiId = data[0][8];
                            dataItem.BuyerId = data[0][9];
                            dataItem.BrokerId = data[0][10];
                        }
                        else {                          
                            $('#errMsg').text('No Chop Tidak Lolos atau Belum Diuji atau Sudah Dialokasikan!');
                            openErrWindow();
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_AlokasiId)) == "undefined" ? grid.dataSource.getByUid(dtl_AlokasiId) : grid.dataSource.get(dtl_AlokasiId);
                            dataItem.Chop = '';
                            return false;                               
                        }
                    });
                }
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


function cekNoChop(chop) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_AlokasiId)) == "undefined" ? grid.dataSource.getByUid(dtl_AlokasiId) : grid.dataSource.get(dtl_AlokasiId);
    var tahunProduksi = '2017';
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select A.GradeId, B.Nama As NamaGrade, A.QtySacks, A.Gross, A.Tare, A.SatuanId, C.Nama as NamaSatuan, A.DTL_ProductSampleId, '9C353F00-78E9-4868-A42A-1C32F4880D02' as AlokasiId, '00000000-0000-0000-0000-000000000000' as BuyerId, A.BrokerId from [SPDK_KanpusEF].[dbo].[DTL_ProductSample] as A " +
                " join [ReferensiEF].[dbo].[Grade] as B on A.GradeId=B.GradeId " +
                " join [ReferensiEF].[dbo].[Satuan] as C on A.SatuanId=C.SatuanId " +
                " where A.SubBudidayaId='" + dataItem.SubBudidayaId + "' and A.MerkId='" + dataItem.MerkId + "' and A.Chop='" + chop + "' and A.TahunProduksi='" + dataItem.TahunProduksi + "' and A.LolosUji=1 and A.SudahAlokasi=0",
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
                            hdr_AlokasiId = $('#HDR_AlokasiId').val();
                            $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                                disableHeader();
                                $('#btnDeleteHeader').removeClass('disabledbutton');
                                $('#btnPrintHeader').removeClass('disabledbutton');
                                $('#btnDeleteHeader').attr('disabled', false);
                                $('#btnPrintHeader').attr('disabled', false);
                                $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                                $('#grdDetail').removeClass('disabledbutton');
                                $("#grdDetail").data("kendoGrid").dataSource.read({
                                    id: $('#HDR_AlokasiId').val()
                                });
                            });
                        }
                        else {
                            openErrWindow();
                            $('#_detail').addClass('disabledbutton');
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
    $('#HDR_AlokasiId').val(_NomorInputView.HDR_AlokasiId);
    $('#TahunBuku').val(_NomorInputView.TahunBuku);
    $('#NomorInput').val(_NomorInputView.NomorInput);
    $('#NomorInputView').val(_NomorInputView.NomorInputView);
    $('#No_Alokasi').val(_NomorInputView.No_Alokasi);
    $('#Tanggal_Alokasi').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.Tanggal_Alokasi.substr(6))));
    var tglVerKaur = new Date(parseInt(_NomorInputView.TglVerKaur.substr(6)));
    $('#TglVerKaur').val(tglVerKaur.getDate() + "/" + (tglVerKaur.getMonth() + 1) + "/" + tglVerKaur.getFullYear());
    $('#UserName').val(_NomorInputView.UserName);
    $('#MainBudidayaId').val(_NomorInputView.MainBudidayaId);
    $('#NamaMainBudidaya').val(_NomorInputView.NamaMainBudidaya);
    $('#StatusAlokasi').val(_NomorInputView.StatusAlokasi);
    $('#BuyerId').val(_NomorInputView.BuyerId);
    $('#NamaBuyer').val(_NomorInputView.NamaBuyer);
    $('#Alamat').val(_NomorInputView.Alamat);
    $('#Kota').val(_NomorInputView.Kota);
    $('#Propinsi').val(_NomorInputView.Propinsi);
    $('#No_Katalog').val(_NomorInputView.No_Katalog);
    $('#Tanggal_Auction').val(_NomorInputView.Tanggal_Auction);
}

function ViewToModel() {
    var budidaya = "Teh";
    var statusAlokasi = "Free Sales Bandung (SC)";
    var mdl = {
        HDR_AlokasiId: $('#HDR_AlokasiId').val(),
        TahunBuku: $('#TahunBuku').val(),
        NomorInput: $('#NomorInput').val(),
        NomorInputView: Array(5 - String($('#NomorInput').val()).length + 1).join('0') + $('#NomorInput').val() + ' - ' + $('#No_Alokasi').val().toUpperCase(),
        No_Alokasi: $('#No_Alokasi').val().toUpperCase(),
        Tanggal_Alokasi: $('#Tanggal_Alokasi').val(),
        VerKaur: $('#VerKaur').val(),
        TglVerKaur: $('#TglVerKaur').val(),
        UserName: $('#UserName').val(),
        MainBudidayaId: $('#MainBudidayaId').val(),
        NamaMainBudidaya: budidaya,
        StatusAlokasi: statusAlokasi.toString(),
        BuyerId: $('#BuyerId').val(),
        NamaBuyer: $('#NamaBuyer').val().toUpperCase(),
        Alamat: $('#Alamat').val(),
        Kota: $('#Kota').val(),
        Propinsi: $('#Propinsi').val(),
        No_Katalog: $('#No_Katalog').val(),
        Tanggal_Auction: $('#Tanggal_Auction').val()
    };
    return mdl;
}

function hapusHeader() {
    wnd.close();
    PeriksaConstraintHeader().done(function (data) {
        if (data == 0) {
            ConfirmedHapusHeader().done(function (data) {
                if (data) {
                    hdr_AlokasiId = $('#HDR_AlokasiId').val();
                    $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                        disableHeader();
                        $('#btnDeleteHeader').removeClass('disabledbutton');
                        $('#btnPrintHeader').removeClass('disabledbutton');
                        $('#btnDeleteHeader').attr('disabled', false);
                        $('#btnPrintHeader').attr('disabled', false);
                        $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                        $('#_detail').removeClass('disabledbutton');
                        $("#grdDetail").data("kendoGrid").dataSource.read({
                            id: $('#HDR_AlokasiId').val()
                        });
                    });
                }
                else {
                    openErrWindow();
                    $('#_detail').addClass('disabledbutton');
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_Alokasi]" +
                " where HDR_AlokasiId='" + $("#HDR_AlokasiId").val() + "'",
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
        hdr_AlokasiId = _NomorInputView.HDR_AlokasiId;
        $('#No_Alokasi').focus();
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
        parameters: { HDR_AlokasiId: $('#HDR_AlokasiId').val() }
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
        $('#_detail').addClass('disabledbutton');
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
        $('#_detail').addClass('disabledbutton');
        gridDestroy();
    }
}

function filterNomorInput(e) {
    var mdl = JSON.stringify(ViewToModel());
    return { _model: mdl, _menuId: menuId };
}

function filterKebun() {
    return {
        kebunId: $('#KebunId').val(),
        value: $("#NamaKebun").val()
    };
}

function KebunOnSelect(e) {
    var kebun = this.dataItem(e.item);
    $('#KebunId').val(kebun.KebunId);
    $('#NamaKebun').val(kebun.Nama);
    kebunId = kebun.KebunId;
}

function filterBuyer() {
    return {
        value: $("#NamaBuyer").val(),
        mainBudidayaId: $("#MainBudidayaId").val()
    };
}

function BuyerOnSelect(e) {
    var buyer = this.dataItem(e.item);
    $('#BuyerId').val(buyer.BuyerId)
    $('#NamaBuyer').val(buyer.Nama);
    buyerOnSelect(e).done(function (data) {
        $('#Alamat').data('kendoAutoComplete').value(data[0].Alamat);
        $('#Kota').data('kendoAutoComplete').value(data[0].Kota);
        $('#Propinsi').data('kendoAutoComplete').value(data[0].Propinsi);
    }).fail(function (x) {
        alert('Error! Hubungi Bagian TI');
    });
}

function buyerOnSelect(e) {
    var url = '/Input/GetDataFrom';
    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            strClassView: "Ptpn8.Penjualan.Models.View_HDRAlokasi",
            scriptSQL: "SELECT DISTINCT Nama, rtrim(Alamat)+', '+rtrim(alamat1)+' '+rtrim(alamat2) as Alamat, kota, propinsi from [ReferensiEF].[dbo].[Buyer] as A" +
                        " where BuyerId='" + $('#BuyerId').val() + "'",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

// Detail

function grdOnChange(e) {
    $('#NamaMerk').text('ADUH');
}

function grdOnEdit(e) {
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.DTL_AlokasiId = model.uid;
        model.HDR_AlokasiId = hdr_AlokasiId;
    }
    dtl_AlokasiId = model.DTL_AlokasiId;
    mainBudidayaId = $('#MainBudidayaId').val();
    gridStatus = 'sudah-diapa-apain';
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
    var dataItem = typeof (grid.dataSource.get(dtl_AlokasiId)) == "undefined" ? grid.dataSource.getByUid(dtl_AlokasiId) : grid.dataSource.get(dtl_AlokasiId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_AlokasiId)) == "undefined" ? grid.dataSource.getByUid(dtl_AlokasiId) : grid.dataSource.get(dtl_AlokasiId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_AlokasiId)) == "undefined" ? grid.dataSource.getByUid(dtl_AlokasiId) : grid.dataSource.get(dtl_AlokasiId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_AlokasiId)) == "undefined" ? grid.dataSource.getByUid(dtl_AlokasiId) : grid.dataSource.get(dtl_AlokasiId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.SatuanId = ddlItem.SatuanId;
}

function aucNamaSatuanOnDataBound(e) {

}

//autocomplete Grade
function filterGrade(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_AlokasiId)) == "undefined" ? grid.dataSource.getByUid(dtl_AlokasiId) : grid.dataSource.get(dtl_AlokasiId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_AlokasiId)) == "undefined" ? grid.dataSource.getByUid(dtl_AlokasiId) : grid.dataSource.get(dtl_AlokasiId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_AlokasiId)) == "undefined" ? grid.dataSource.getByUid(dtl_AlokasiId) : grid.dataSource.get(dtl_AlokasiId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.BrokerId = ddlItem.BrokerId;
}

function aucNamaBrokerOnDataBound(e) {

}

//autocomplete Buyer
function filterBuyer(e) {
    return {
        value: $("#NamaBuyer").val()
    };
}

function aucNamaBuyerOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_AlokasiId)) == "undefined" ? grid.dataSource.getByUid(dtl_AlokasiId) : grid.dataSource.get(dtl_AlokasiId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.BuyerId = ddlItem.BuyerId;
}

function aucNamaBuyerOnDataBound(e) {

}

//autocomplete Alokasi
function filterAlokasi(e) {
    return {
        value: 'Auction PT. KPB'
    };
}

function aucNamaAlokasiOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_AlokasiId)) == "undefined" ? grid.dataSource.getByUid(dtl_AlokasiId) : grid.dataSource.get(dtl_AlokasiId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.AlokasiId = ddlItem.AlokasiId;
}

function aucNamaAlokasiOnDataBound(e) {

}

function aucNoPJBOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_AlokasiId)) == "undefined" ? grid.dataSource.getByUid(dtl_AlokasiId) : grid.dataSource.get(dtl_AlokasiId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.No_PJB = ddlItem.No_PJB;
    data.HDR_PJBId = ddlItem.HDR_PJBId;
}

function aucNoPJBOnDataBound(e) {

}

function filterPJB(e) {
    var tahunIni, tahunLalu;
    tahunIni = parseInt($('#TahunBuku').val());
    tahunLalu = tahunIni - 1;
    return {
        strClassView: 'Ptpn8.Penjualan.Models.View_HDRPJB',
        scriptSQL: "select DISTINCT A.No_PJB, A.HDR_PJBId from [SPDK_KanpusEF].[dbo].[HDR_PJB] as A" +
            " inner join [SPDK_KanpusEF].[dbo].[DTL_PJB] as B on A.HDR_PJBId=B.HDR_PJBId" +
            " where A.VerKaur=0 and A.BuyerId='" + $('#BuyerId').val() + "' and A.MainBudidayaId='" + $("#MainBudidayaId").val() + 
            "' and (A.TahunBuku=" + String(tahunIni) + " or A.TahunBuku=" + String(tahunLalu) + ")",
        _menuId: menuId
    }
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
    var dataItem = typeof (grid.dataSource.get(dtl_AlokasiId)) == "undefined" ? grid.dataSource.getByUid(dtl_AlokasiId) : grid.dataSource.get(dtl_AlokasiId);
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
