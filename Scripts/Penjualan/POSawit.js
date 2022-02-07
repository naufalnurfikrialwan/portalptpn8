var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var buyer;
var hdr_POId, mainBudidayaId, subBudidayaId, dtl_POId, listPJBId, lst;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var rowNumber = 0;
var menuId = '592AF94F-1A0D-443B-9657-4904B93ACF1E';

function resetRowNumber(e) {
    rowNumber = 0;
    var jmlHarga = 0;
    var jmlGross = 0;
    var grid = $('#grdDetail').data('kendoGrid');
    var datasource = grid.dataSource.data();
    for (var i = 0; i < datasource.length; i++) {
        var model = datasource[i];
        model.JumlahHarga = hitungJumlah(model.DTL_POId);
        $('#jml' + model.DTL_POId).text(kendo.toString(model.JumlahHarga, 'n2'));
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
	    var h2 = parseInt($(".hdr").css("height")) + parseInt($("._headerteh").css("height"));
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
                if (input.attr("name") == "TahunProduksi" && input.val() == "") {
                    $('#errMsg').text('Tahun Produksi Harus Diisi!');
                    openErrWindow();
                    return false;
                } 
                else if (input.attr("name") == "NamaSubBudidaya" && input.val() != "") {
                    cekNamaSubBudidaya(input.val()).done(function (data) {
                        if (data.length > 0) {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_POId)) == "undefined" ? grid.dataSource.getByUid(dtl_POId) : grid.dataSource.get(dtl_POId);
                            dataItem.SubBudidayaId = data[0][0];
                            subBudidayaId = data[0][0];
                        }
                    });
                } else if (input.attr("name") == "NamaMerk" && input.val() != "") {
                    cekNamaMerk(input.val()).done(function (data) {
                        if (data.length > 0) {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_POId)) == "undefined" ? grid.dataSource.getByUid(dtl_POId) : grid.dataSource.get(dtl_POId);
                            dataItem.MerkId = data[0][0];
                        }
                    });
                } else if (input.attr("name") == "NamaGrade" && input.val() != "") {
                    cekNamaGrade(input.val()).done(function (data) {
                        if (data.length > 0) {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_POId)) == "undefined" ? grid.dataSource.getByUid(dtl_POId) : grid.dataSource.get(dtl_POId);
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
                            var dataItem = typeof (grid.dataSource.get(dtl_POId)) == "undefined" ? grid.dataSource.getByUid(dtl_POId) : grid.dataSource.get(dtl_POId);
                            dataItem.SatuanId = data[0][0];
                        }
                    });
                } else if (input.attr("name") == "QtySacks" && input.val() != "") {
                    ambilStandarGrade().done(function (r) {
                        if (r == 0) {
                            return true;
                        } else {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_POId)) == "undefined" ? grid.dataSource.getByUid(dtl_POId) : grid.dataSource.get(dtl_POId);
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
                        gridStatus = 'belum-ngapa-ngapain';
                        hdr_POId = $('#HDR_POId').val();
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
                                        $("#grdDetail").data("kendoGrid").dataSource.read({ id: $('#HDR_POId').val() });
                                    }).fail(function (x) {
                                        alert('Error! Hubungi Bagian TI');
                                    });
                                }
                                else {
                                    var grid = $("#grdDetail").data("kendoGrid");
                                    grid.dataSource.read({
                                        id: $('#HDR_POId').val()
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
    $('#HDR_POId').val(_NomorInputView.HDR_POId);
    $('#TahunBuku').val(_NomorInputView.TahunBuku);
    $('#NomorInput').val(_NomorInputView.NomorInput);
    $('#NomorInputView').val(_NomorInputView.NomorInputView);
    $('#No_PO').val(_NomorInputView.No_PO);
    $('#Tanggal_PO').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.Tanggal_PO.substr(6))));
    $('#No_Referensi').val(_NomorInputView.No_Referensi);
    var tanggal_Referensi = new Date(parseInt(_NomorInputView.Tanggal_Referensi.substr(6)));
    $('#Tanggal_Referensi').val(tanggal_Referensi.getDate() + "/" + (tanggal_Referensi.getMonth() + 1) + "/" + tanggal_Referensi.getFullYear());
    $('#BuyerId').val(_NomorInputView.BuyerId);
    $('#NamaBuyer').val(_NomorInputView.NamaBuyer);
    $('#Alamat').val(_NomorInputView.Alamat);
    $('#Kota').val(_NomorInputView.Kota);
    $('#Propinsi').val(_NomorInputView.Propinsi);
    $('#KenaPpn').val(_NomorInputView.KenaPpn);
    $('#KenaMaterai').val(_NomorInputView.KenaMaterai);
    $('#Pembuat').val(_NomorInputView.Pembuat);
    $('#Pemeriksa').val(_NomorInputView.Pemeriksa);
    $('#UserName').val(_NomorInputView.UserName);
    $('#MainBudidayaId').val(_NomorInputView.MainBudidayaId);
    $('#NamaMainBudidaya').val(_NomorInputView.NamaMainBudidaya);
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
    $('#NPWP').val(_NomorInputView.NPWP);
    lst = $('#ListPJBId').data('kendoMultiSelect');
    lst.dataSource.read().done(function (data) {
        lst.value(_NomorInputView.ListPJBId);
        $('#ListNo_PJB').val(_NomorInputView.ListNo_PJB);
        $('#ListTanggal_PJB').val(_NomorInputView.ListTanggal_PJB);
    });
}

function ViewToModel() {
    listPJBId = $('#ListPJBId').val();
    if (listPJBId == null) listPJBId = "";
    var listNo_PJB = $('#ListNo_PJB').val();
    if (listNo_PJB == null) listNo_PJB = "";
    var listTanggal_PJB = $('#ListTanggal_PJB').val();
    if (listTanggal_PJB == null) listTanggal_PJB = "";
    var mdl = {
        HDR_POId: $('#HDR_POId').val(),
        TahunBuku: $('#TahunBuku').val(),
        NomorInput: $('#NomorInput').val(),
        NomorInputView: Array(5 - String($('#NomorInput').val()).length + 1).join('0') + $('#NomorInput').val() + ' - ' + $('#No_PO').val().toUpperCase(),
        No_PO: $('#No_PO').val().toUpperCase(),
        Tanggal_PO: $('#Tanggal_PO').val(),
        No_Referensi: $('#No_Referensi').val().toUpperCase(),
        Tanggal_Referensi: $('#Tanggal_Referensi').val(),
        BuyerId: $('#BuyerId').val(),
        NamaBuyer: $('#NamaBuyer').val().toUpperCase(),
        Alamat: $('#Alamat').val(),
        Kota: $('#Kota').val(),
        Propinsi: $('#Propinsi').val(),
        KenaPpn: $('#KenaPpn').val(),
        KenaMaterai: $('#KenaMaterai').val(),
        Pembuat: $('#Pembuat').val(),
        Pemeriksa: $('#Pemeriksa').val(),
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
        NPWP: $('#NPWP').val(),
        ListPJBId: listPJBId.toString(),
        ListNo_PJB: listNo_PJB.toString(),
        ListTanggal_PJB: listTanggal_PJB.toString()
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
                            hdr_POId = $('#HDR_POId').val();
                            $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                                disableHeader();
                                $('#btnDeleteHeader').removeClass('disabledbutton');
                                $('#btnPrintHeader').removeClass('disabledbutton');
                                $('#btnDeleteHeader').attr('disabled', false);
                                $('#btnPrintHeader').attr('disabled', false);
                                $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                                $('#grdDetail').removeClass('disabledbutton');
                                $("#grdDetail").data("kendoGrid").dataSource.read({
                                    id: $('#HDR_POId').val()
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_PO]" +
                " where HDR_POId='" + $("#HDR_POId").val() + "'",
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[HDR_Faktur]" +
                " where patindex('%" + $("#HDR_POId").val() + "%',ListPOId)>0",
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_Faktur]" +
                " where DTL_POId='" + id + "'",
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

        if (typeof (_NomorInputView.ListPJBId) == "string") {
            if (_NomorInputView.ListPJBId == "") {
                _NomorInputView.ListPJBId = [];
                _NomorInputView.ListNo_PJB = [];
                _NomorInputView.ListTanggal_PJB = [];
            }
            else {
                _NomorInputView.ListPJBId = _NomorInputView.ListPJBId.split(",");
                _NomorInputView.ListNo_PJB = _NomorInputView.ListNo_PJB.split(",");
                _NomorInputView.ListTanggal_PJB = _NomorInputView.ListTanggal_PJB.split(",");
            }
        }
        ModelToView(_NomorInputView);
        hdr_POId = _NomorInputView.HDR_POId;
        mainBudidayaId = _NomorInputView.MainBudidayaId;
        cekData(_NomorInputView.NomorInputView);
        $('#No_PO').focus();
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

function btnPrintHeaderOnClick() {
    var viewer = $("#reportViewer").data("telerik_ReportViewer");
    viewer.reportSource({
        report: viewer.reportSource().report,
        parameters: { HDR_POId: $('#HDR_POId').val() }
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

function BuyerOnSelect(e) {
    var buyer = this.dataItem(e.item);
    $('#BuyerId').val(buyer.BuyerId)
    $('#NamaBuyer').val(buyer.Nama);
    buyerOnSelect(e).done(function (data) {
        $('#Alamat').data('kendoAutoComplete').value(data[0].Alamat);
        $('#Kota').data('kendoAutoComplete').value(data[0].Kota);
        $('#Propinsi').data('kendoAutoComplete').value(data[0].Propinsi);
        $('#NPWP').val(data[0].NPWP);
    }).fail(function (x) {
        alert('Error! Hubungi Bagian TI');
    });
    $('#ListPJBId').data('kendoMultiSelect').dataSource.read();
}

function buyerOnSelect(e) {

    var url = '/Input/GetDataFrom';
    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            strClassView: "Ptpn8.Penjualan.Models.View_HDRPO",
            scriptSQL: "SELECT DISTINCT Nama, rtrim(Alamat)+', '+rtrim(alamat1)+' '+rtrim(alamat2) as Alamat, kota, propinsi,npwp from [ReferensiEF].[dbo].[Buyer] as A" +
                        " where BuyerId='" + $('#BuyerId').val() + "'",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function filterPJB() {
    if (headerBaru == true) {
        var listId = [];
        for (var i = 0; i < _NomorInputView.ListPJBId.length; i++) {
            listId.push("'" + _NomorInputView.ListPJBId[i].toString() + "'");
        }

        var tahunIni, tahunLalu;
        tahunIni = parseInt($('#TahunBuku').val());
        tahunLalu = tahunIni - 1;

        return {
            strClassView: 'Ptpn8.Penjualan.Models.View_HDRPJB',
            scriptSQL: "select DISTINCT A.HDR_PJBId, A.No_PJB, A.Tanggal_PJB, A.Pejabat from [SPDK_KanpusEF].[dbo].[HDR_PJB] as A" +
                " inner join [SPDK_KanpusEF].[dbo].[DTL_PJB] as B on A.HDR_PJBId=B.HDR_PJBId" +
                " where A.VerKaur=1 and A.BuyerId='" + $('#BuyerId').val() + "' and  B.SudahPO=0 and A.MainBudidayaId='" + $("#MainBudidayaId").val() +
                "' and (A.TahunBuku=" + String(tahunIni) + " or A.TahunBuku=" + String(tahunLalu) + ")",
            _menuId: menuId
        }

    }
    else {
        if (_NomorInputView.ListPJBId != "") {
            var listId = [];
            for (var i = 0; i < _NomorInputView.ListPJBId.length; i++) {
                listId.push("'" + _NomorInputView.ListPJBId[i].toString() + "'");
            }

            var tahunIni, tahunLalu;
            tahunIni = parseInt($('#TahunBuku').val());
            tahunLalu = tahunIni - 1;

            return {
                strClassView: 'Ptpn8.Penjualan.Models.View_HDRPJB',
                scriptSQL: "select DISTINCT A.HDR_PJBId, A.No_PJB, A.Tanggal_PJB, A.Pejabat  from [SPDK_KanpusEF].[dbo].[HDR_PJB] as A" +
                    " inner join [SPDK_KanpusEF].[dbo].[DTL_PJB] as B on A.HDR_PJBId=B.HDR_PJBId" +
                    " where A.VerKaur=1 and A.BuyerId='" + $('#BuyerId').val() + "' and A.MainBudidayaId='" + $("#MainBudidayaId").val() +
                    "' and (A.TahunBuku=" + String(tahunIni) + " or A.TahunBuku=" + String(tahunLalu) + ")",
                _menuId: menuId
            }
        }
        else { return "" }
    }
}


function ListPJBIdOnSelect(e) {
    var listPOId = e.sender.dataItem();
    var arr = [];
    var arr1 = [];
    if ($('#ListPJBId').val() != null) {
        $('#Pejabat').val(listPOId.Pejabat);
        var lst = $('#ListPJBId').data('kendoMultiSelect');
        for (var i = 0; i < lst.dataItems().length; i++) {
            arr.push(lst.dataItems()[i].No_PJB);
            var tglPJB = new Date(parseInt(lst.dataItems()[i].Tanggal_PJB.substr(6)));
            arr1.push(tglPJB.getDate() + "/" + (tglPJB.getMonth() + 1) + "/" + tglPJB.getFullYear());
            $('#Tanggal_PJB').val(tglPJB.getDate() + "/" + (tglPJB.getMonth() + 1) + "/" + tglPJB.getFullYear());
        }
        $('#ListNo_PJB').val(arr.toString());
        $('#ListTanggal_PJB').val(arr1.toString());
    }
}

function getDataFrom() {
    // Hitung Jumlah Record di Detail
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_Faktur]" +
                " where HDR_FakturId='" + $("#HDR_FakturId").val() + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function InsertGridFrom() {
    var grd = $('#grdDetail').data('kendoGrid');
    var url = '/Input/GetDataFrom';
    var lst = $('#ListPJBId').data('kendoMultiSelect').value();
    var str = "";
    for (var i = 0; i < lst.length; i++) {
        str = str + "'" + lst[i] + "'";
        if (i < lst.length - 1) str = str + ",";
    }

    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            strClassView: "Ptpn8.Penjualan.Models.View_DTLPO",
            scriptSQL: "INSERT INTO [SPDK_KanpusEF].[dbo].[DTL_PO] (DTL_POId, HDR_POId, SubBudidayaId, MerkId, QtySacks, SatuanId, GradeId, DTL_PJBId, Gross, Tare, Kurs, Price, PriceIDR, SudahFaktur, KK, KA, ALB, TahunProduksi, JumlahHarga)" +
                " SELECT newid() as DTL_POId, cast('" + $('#HDR_POId').val() + "' as uniqueidentifier) as HDR_POId," +
                " A.SubBudidayaId, A.MerkId, A.QtySacks, " +
                " A.SatuanId, A.GradeId, A.DTL_PJBId, 0, 0, 0, 0, 0, 0, 0 , 0, 0 , 0, 0" +
                " from [SPDK_KanpusEF].[dbo].[DTL_PJB] as A" +
                " where HDR_PJBId IN (" + str + ") and A.SudahPO=0",
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
        model.DTL_POId = model.uid;
        model.HDR_POId = hdr_POId;
    }
    dtl_POId = model.DTL_POId;
    mainBudidayaId = $('#MainBudidayaId').val();
    gridStatus = 'sudah-diapa-apain';
    if (model.MataUang == "IDR") {
        $(e.container).find('input[name="Kurs"]').attr("readonly", true);
        $(e.container).find('input[name="Price"]').attr("readonly", true);
        $(e.container).find('input[name="PriceIDR"]').attr("readonly", false);
        model.Kurs = 0;
        model.Price = 0;
    }
    else {
        $(e.container).find('input[name="Kurs"]').attr("readonly", false);
        $(e.container).find('input[name="Price"]').attr("readonly", false);
        $(e.container).find('input[name="PriceIDR"]').attr("readonly", true);
        model.PriceIDR = 0;
    }
    $('#jmlGross').text(kendo.toString(rekapGross(), 'n2'));
    $('#jmlHarga').text(kendo.toString(rekapJumlahHarga(), 'n2'));
    $('#jmlTare').text(kendo.toString(rekapTare(), 'n2'));
    $('#jmlQtySacks').text(kendo.toString(rekapQtySacks(), 'n2'));
    model.JumlahHarga = hitungJumlah(dtl_POId);
    $('#jml' + model.dtl_POId).text(kendo.toString(model.JumlahHarga, 'n2'));
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
    if (dataItem.MataUang == "IDR")
        newValue = dataItem.PriceIDR * (dataItem.Gross-dataItem.Tare);
    else
        newValue = dataItem.Kurs * (dataItem.Price / 100) * (dataItem.Gross-dataItem.Tare);
    return newValue;
}

function grdOnDataBound(e) {
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
    var dataItem = typeof (grid.dataSource.get(dtl_POId)) == "undefined" ? grid.dataSource.getByUid(dtl_POId) : grid.dataSource.get(dtl_POId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.SubBudidayaId = ddlItem.SubBudidayaId;
    subBudidayaId = ddlItem.SubBudidayaId;
}

function aucNamaSubBudidayaOnDataBound(e) {

}

function filterMerk(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_POId)) == "undefined" ? grid.dataSource.getByUid(dtl_POId) : grid.dataSource.get(dtl_POId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_POId)) == "undefined" ? grid.dataSource.getByUid(dtl_POId) : grid.dataSource.get(dtl_POId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_POId)) == "undefined" ? grid.dataSource.getByUid(dtl_POId) : grid.dataSource.get(dtl_POId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.SatuanId = ddlItem.SatuanId;
}

function aucNamaSatuanOnDataBound(e) {

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

function filterGrade(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_POId)) == "undefined" ? grid.dataSource.getByUid(dtl_POId) : grid.dataSource.get(dtl_POId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_POId)) == "undefined" ? grid.dataSource.getByUid(dtl_POId) : grid.dataSource.get(dtl_POId);
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


function ddlMataUangOnChange(e) {
    var matauang = $('#MataUang').val();
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_POId)) == "undefined" ? grid.dataSource.getByUid(dtl_POId) : grid.dataSource.get(dtl_POId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);

    if (matauang == "IDR") {
        $(data).find('input[name="Kurs"]').attr("readonly", true);
        $(data).find('input[name="Price"]').attr("readonly", true);
        $(data).find('input[name="PriceIDR"]').attr("readonly", false);
        data.Kurs = 0;
        data.Price = 0;
    }
    else {
        $(data).find('input[name="Kurs"]').attr("readonly", false);
        $(data).find('input[name="Price"]').attr("readonly", false);
        $(data).find('input[name="PriceIDR"]').attr("readonly", true);
        data.PriceIDR = 0;
    }
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

// copykan ke semua view
function cekNoChop(input) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_POId)) == "undefined" ? grid.dataSource.getByUid(dtl_POId) : grid.dataSource.get(dtl_POId);
    var items = grid.dataItems();
    var arr = [, ];
    for (var i = 0; i < items.length; i++) {
        if (items[i].Chop == input.val() && items[i].DTL_POId != dtl_POId
            && items[i].Chop.toLowerCase() != "na" && items[i].MerkId == dataItem.MerkId) {
            arr.push([items[i].DTL_POId, 1]);
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

function ambilStandarGrade() {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_POId)) == "undefined" ? grid.dataSource.getByUid(dtl_POId) : grid.dataSource.get(dtl_POId);
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
        listId.push("'" + data[i].DTL_POId.toString() + "'");
    }
    var strId = listId.toString();

    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_Faktur]" +
                " where DTL_POId in (" + strId + ")",
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

