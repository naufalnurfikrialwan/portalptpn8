var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var buyer;
var hdr_TerimaDHBBId, barangId, namaBarang, norek, dtl_TerimaDHBBId, namaSatuan, kebunId;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var rowNumber = 0;
var menuId = 'E154394F-4A2A-43A9-919B-FC03B73D3F91';

function resetRowNumber(e) {
    rowNumber = 0;
    var harga = 0;
    var grid = $('#grdDetail').data('kendoGrid');
    var datasource = grid.dataSource.data();
    for (var i = 0; i < datasource.length; i++) {
        var model = datasource[i];
        model.Harga = hitungJumlah(model.DTL_TerimaDHBBId);
        $('#jml' + model.DTL_TerimaDHBBId).text(kendo.toString(model.Harga, 'n2'));
        harga = harga + model.Harga;
    }
    $('#Harga').text(kendo.toString(harga, 'n2'));
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

    //////// sampe sini
});

$(window).on("beforeunload", function () {
    return "Please don't leave me!";
});

$(window).on("unload", function () {
    hapusPenggunaPortalYangAktif();
});

function cekNamaSatuan(satuan) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select SatuanId from [ReferensiEF].[dbo].[Satuan]" +
                " where lower(Nama)='" + satuan.toLowerCase() + "'",
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
                        hdr_TerimaDHBBId = $('#HDR_TerimaDHBBId').val();
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
                                        $("#grdDetail").data("kendoGrid").dataSource.read({ id: $('#HDR_TerimaDHBBId').val() });
                                    }).fail(function (x) {
                                        alert('Error! Hubungi Bagian TI');
                                    });
                                }
                                else {
                                    var grid = $("#grdDetail").data("kendoGrid");
                                    grid.dataSource.read({
                                        id: $('#HDR_TerimaDHBBId').val()
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

function getDataFrom() {
    // Hitung Jumlah Record di Detail
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_TerimaDHBB]" +
                " where HDR_TerimaDHBBId='" + $("#HDR_TerimaDHBBId").val() + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function InsertGridFrom() {
    var grd = $('#grdDetail').data('kendoGrid');
    var url = '/Input/GetDataFrom';
  
    var bulan = $('#BulanTerima').val();
    var tahun = $('#TahunTerima').val();

    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            strClassView: "Ptpn8.DaftarHargaBarangBahanDanTarif.Models.View_DTLTerimaDHBB",
            scriptSQL:  "Insert Into SPDK_KanpusEF.dbo.DTL_TerimaDHBB  "+
                        "(DTL_TerimaDHBBId,HDR_TerimaDHBBId,DTL_UpdateDHBBId,TanggalBerlaku,TanggalSurvey,BarangId,RekeningId,NamaBarang,NamaSatuan,Spesifikasi,Merk,HargaDasar,Overhead,Harga,DataSurvey,VerBrosur,SudahTerima,NomorInfo) "+
                        "SELECT newid() as DTL_TerimaDHBBId, cast('" + $('#HDR_TerimaDHBBId').val() + "' as uniqueidentifier) as HDR_TerimaDHBBId, " +
                        "A.DTL_UpdateDHBBId, A.TanggalBerlaku, A.TanggalSurvey, A.BarangId,A.RekeningId,A.NamaBarang, A.NamaSatuan, A.Spesifikasi, A.Merk, A.HargaDasar, A.Overhead ,A.Harga, A.DataSurvey,A.VerBrosur, A.SudahTerima, A.NomorInfo " +
                        "FROM [SPDK_KanpusEF].[dbo].[DTL_UpdateDHBB] as A "+
                        "join [SPDK_KanpusEF].[dbo].[HDR_UpdateDHBB] as B "+
                        "on A.HDR_UpdateDHBBId = B.HDR_UpdateDHBBId "+
                        "where B.BulanSurvey=" + bulan + " and A.SudahTerima=0 and B.TahunSurvey =" + tahun,
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function ModelToView(_NomorInputView) {
    $('#HDR_TerimaDHBBId').val(_NomorInputView.HDR_TerimaDHBBId);
    $('#TahunBuku').val(_NomorInputView.TahunBuku);
    $('#KebunId').val(_NomorInputView.KebunId);
    $('#NomorInput').val(_NomorInputView.NomorInput);
    $('#NomorInputView').val(_NomorInputView.NomorInputView);
    $('#NomorPenerimaan').val(_NomorInputView.NomorPenerimaan);
    $('#TanggalPenerimaan').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.TanggalPenerimaan.substr(6))));
    $('#UserName').val(_NomorInputView.UserName);
    $('#Pejabat').val(_NomorInputView.Pejabat);
    $('#No_SuratTugas').val(_NomorInputView.No_SuratTugas);
    $('#VerKaur').val(_NomorInputView.VerKaur);
    $('#VerKabag').val(_NomorInputView.VerKabag);
    var tglVerKaur = new Date(parseInt(_NomorInputView.TglVerKaur.substr(6)));
    var tglVerKabag = new Date(parseInt(_NomorInputView.TglVerKabag.substr(6)));
    $('#TglVerKaur').val(tglVerKaur.getDate() + "/" + (tglVerKaur.getMonth() + 1) + "/" + tglVerKaur.getFullYear());
    $('#TglVerKabag').val(tglVerKabag.getDate() + "/" + (tglVerKabag.getMonth() + 1) + "/" + tglVerKabag.getFullYear());
    $('#UserNameKaur').val(_NomorInputView.UserNameKaur);
    $('#UserNameKabag').val(_NomorInputView.UserNameKabag);
    $('#BulanTerima').data('kendoDropDownList').value(_NomorInputView.BulanTerima);
    $('#TahunTerima').val(_NomorInputView.TahunTerima);
}

function ViewToModel() {
    var mdl = {
        HDR_TerimaDHBBId: $('#HDR_TerimaDHBBId').val(),
        TahunBuku: $('#TahunBuku').val(),
        NomorInput: $('#NomorInput').val(),
        NomorInputView: Array(5 - String($('#NomorInput').val()).length + 1).join('0') + $('#NomorInput').val() + ' - ' + $('#NomorPenerimaan').val().toUpperCase(),
        KebunId: $('#KebunId').val(),
        NomorPenerimaan: $('#NomorPenerimaan').val().toUpperCase(),
        TanggaPenerimaan: $('#TanggalPenerimaan').val(),
        UserName: $('#UserName').val(),
        Pejabat: $('#Pejabat').val().toUpperCase(),
        No_SuratTugas: $('#No_SuratTugas').val(),
        VerKaur: $('#VerKaur').val(),
        VerKabag: $('#VerKabag').val(),
        TglVerKaur: $('#TglVerKaur').val(),
        TglVerKabag: $('#TglVerKabag').val(),
        UserNameKaur: $('#UserNameKaur').val(),
        UserNameKabag: $('#UserNameKabag').val(),
        BulanTerima: $('#BulanTerima').data('kendoDropDownList').value(),
        TahunTerima: $('#TahunTerima').val().toUpperCase(),
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
                            hdr_TerimaDHBBId = $('#HDR_TerimaDHBBId').val();
                            $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                                disableHeader();
                                $('#btnDeleteHeader').removeClass('disabledbutton');
                                $('#btnPrintHeader').removeClass('disabledbutton');
                                $('#btnDeleteHeader').attr('disabled', false);
                                $('#btnPrintHeader').attr('disabled', false);
                                $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                                $('#_detail').removeClass('disabledbutton');
                                $("#grdDetail").data("kendoGrid").dataSource.read({
                                    id: $('#HDR_TerimaDHBBId').val()
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_TerimaDHBB]" +
                " where HDR_TerimaDHBBId='" + $("#HDR_TerimaDHBBId").val() + "'",
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[HDR_TerimaDHBB]" +
                " where patindex('%" + $("#HDR_TerimaDHBBId").val() + "%',List_Lokasi)>0",
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
        hdr_TerimaDHBBId = _NomorInputView.HDR_TerimaDHBBId;
        $('#NomorPenerimaan').focus();
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
        parameters: { HDR_TerimaDHBBId: $('#HDR_TerimaDHBBId').val() }
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
    $('#_detail').addClass('disabledbutton');
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

function NamaKebunOnSelect(e) {
    var kebun = this.dataItem(e.item);
    $('#KebunId').val(kebun.KebunId);
    $('#Lokasi').val(kebun.Nama);
    kebunId = kebun.KebunId;

}


function grdOnChange(e) {
}

function grdOnEdit(e) {
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.DTL_TerimaDHBBId = model.uid;
        model.HDR_TerimaDHBBId = hdr_TerimaDHBBId;
    }
    dtl_TerimaDHBBId = model.DTL_TerimaDHBBId;

    //$(e.container).find("input[name='TanggalBerlaku']").prop('disabled', true);
    //$(e.container).find("input[name='TanggalSurvey']").prop('disabled', true);
    //$(e.container).find("input[name='KodeBarang']").prop('disabled', true);
    //$(e.container).find("input[name='NamaSatuan']").prop('disabled', true);
    //$(e.container).find("input[name='Spesifikasi']").prop('disabled', true);
    //$(e.container).find("input[name='Merk']").prop('disabled', true);
    //$(e.container).find("input[name='HargaDasar']").prop('disabled', true);
    //$(e.container).find("input[name='DataSurvey']").prop('disabled', true);
    //$(e.container).find("input[name='VerBrosur']").prop('disabled', true);
    //$(e.container).find("input[name='SudahTerima']").prop('disabled', true);
    //$(e.container).find("input[name='NomorInfo']").prop('disabled', true);

    gridStatus = 'sudah-diapa-apain';
    //model.Harga = hitungJumlah(dtl_UpdateDHBBId);
    //$('#jml' + model.dtl_UpdateDHBBId).text(kendo.toString(model.Harga, 'n2'));
    //$('#jmlHarga').text(kendo.toString(rekapJumlahHarga(), 'n2'));
}

function hitungJumlah(id) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(id)) == "undefined" ? grid.dataSource.getByUid(id) : grid.dataSource.get(id);
    var newValue = 0;
    newValue = (((dataItem.HargaDasar * (1 + (dataItem.Overhead)))));
    return newValue;
}

function filterkebun(e) {
    return {
        kebunId: $('#KebunId').val(),
        value: $("#Lokasi").val()
    };
}

//function filterNamaBarang(e) {
//    return {
//        value: $("#NamaBarang").val()
//    };
//}
//function aucNamaBarangOnSelect(e) {
//    var ddlItem = this.dataItem(e.item);
//    var grid = $('#grdDetail').data('kendoGrid');
//    var dataItem = typeof (grid.dataSource.get(dtl_UpdateDHBBId)) == "undefined" ? grid.dataSource.getByUid(dtl_UpdateDHBBId) : grid.dataSource.get(dtl_UpdateDHBBId);
//    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
//    var data = grid.dataItem(row);
//    data.BarangId = ddlItem.BarangId;
//}

//function aucNamaBarangOnDataBound(e) {

//}

//function aucNorekOnSelect(e) {
//    var ddlItem = this.dataItem(e.item);
//    var grid = $('#grdDetail').data('kendoGrid');
//    var dataItem = typeof (grid.dataSource.get(dtl_UpdateDHBBId)) == "undefined" ? grid.dataSource.getByUid(dtl_UpdateDHBBId) : grid.dataSource.get(dtl_UpdateDHBBId);
//    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
//    var data = grid.dataItem(row);
//    data.RekeningId = ddlItem.RekeningId;
//    rekeningId = ddlItem.RekeningId;
//    if (ddlItem.Norek != "") {
//        data.NamaRekening = ddlItem.NamaRekening;
//    }
//}

//function aucNorekOnDataBound(e) {

//}

//function aucNamaSatuanOnSelect(e) {
//    var ddlItem = this.dataItem(e.item);
//    var grid = $('#grdDetail').data('kendoGrid');
//    var dataItem = typeof (grid.dataSource.get(dtl_UpdateDHBBId)) == "undefined" ? grid.dataSource.getByUid(dtl_UpdateDHBBId) : grid.dataSource.get(dtl_UpdateDHBBId);
//    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
//    var data = grid.dataItem(row);
//    data.SatuanId = ddlItem.SatuanId;
//}

//function aucNamaSatuanOnDataBound(e) {

//}



function BidangOnSelect(e) {
    var bidang = $('#Bidang').val();
}
function BulanTerimaOnSelect(e) {
    var bulanTerima = $('#BulanTerima ').val();
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

function rekapHarga() {

    var grid = $('#grdDetail').data('kendoGrid');
    var newValue = 0;
    for (var i = 0; i < grid.dataItems().length; i++) {
        newValue += grid.dataItems()[i].Harga;
    }
    return newValue;
}
function filterdetailRead(e) {
    return { _menuId: menuId };
}

function filterdetailCreate(e) {
    return { _menuId: menuId };
}