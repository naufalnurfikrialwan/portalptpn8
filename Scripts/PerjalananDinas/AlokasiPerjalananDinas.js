var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var hdr_AlokasiPerjalananDinasId, dtl_AlokasiPerjalananDinasId, listPermintaanPerjalananDinasKanpusId, lst;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var rowNumber = 0;
var menuId = '494A31E7-E417-4FF1-8183-FABE9358AE19';

function resetRowNumber(e) {
    rowNumber = 0;
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
                if ((input.attr("name") == "UangLain") && input.val() != "") {
                    var grid = $('#grdDetail').data('kendoGrid');
                    var dataItem = typeof (grid.dataSource.get(dtl_AlokasiPerjalananDinasId)) == "undefined" ? grid.dataSource.getByUid(dtl_AlokasiPerjalananDinasId) : grid.dataSource.get(dtl_AlokasiPerjalananDinasId);
                    dataItem.JumlahBPD = dataItem.UangLain + dataItem.JumlahBPD;
                }
            }
        }
    })
    //////// sampe sini
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
                    gridStatus = 'belum-ngapa-ngapain';
                    hdr_AlokasiPerjalananDinasId = $('#HDR_AlokasiPerjalananDinasId').val();
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
                                    $("#grdDetail").data("kendoGrid").dataSource.read({ id: $('#HDR_AlokasiPerjalananDinasId').val() });
                                }).fail(function (x) {
                                    alert('Error! Hubungi Bagian TI');
                                });
                            }
                            else {
                                var grid = $("#grdDetail").data("kendoGrid");
                                grid.dataSource.read({
                                    id: $('#HDR_AlokasiPerjalananDinasId').val()
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
    $('#HDR_AlokasiPerjalananDinasId').val(_NomorInputView.HDR_AlokasiPerjalananDinasId);
    $('#TahunBuku').val(_NomorInputView.TahunBuku);
    $('#NomorInput').val(_NomorInputView.NomorInput);
    $('#NomorInputView').val(_NomorInputView.NomorInputView);
    $('#NomorAlokasi').val(_NomorInputView.NomorAlokasi);
    $('#TanggalAlokasi').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.TanggalAlokasi.substr(6))));
    $('#Berangkat').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.Berangkat.substr(6))));
    $('#Kembali').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.Kembali.substr(6))));
    $('#JenisTujuan').data('kendoDropDownList').value(_NomorInputView.JenisTujuan);
    $('#Tujuan').val(_NomorInputView.Tujuan);
    $('#Keterangan').val(_NomorInputView.Keterangan);
    $('#KendaraanDinas').data('kendoDropDownList').value(_NomorInputView.KendaraanDinas ? "1" : "0");
    $('#NamaDriver').val(_NomorInputView.NamaDriver);
    $('#NopolKendaraan').val(_NomorInputView.NopolKendaraan);
    $('#UserName').val(_NomorInputView.UserName);
    $('#VerKaur').val(_NomorInputView.VerKaur);
    $('#VerKabag').val(_NomorInputView.VerKabag);
    var tglVerKaur = new Date(parseInt(_NomorInputView.TglVerKaur.substr(6)));
    var tglVerKabag = new Date(parseInt(_NomorInputView.TglVerKabag.substr(6)));
    $('#TglVerKaur').val(tglVerKaur.getDate() + "/" + (tglVerKaur.getMonth() + 1) + "/" + tglVerKaur.getFullYear());
    $('#TglVerKabag').val(tglVerKabag.getDate() + "/" + (tglVerKabag.getMonth() + 1) + "/" + tglVerKabag.getFullYear());
    $('#UserNameKaur').val(_NomorInputView.UserNameKaur);
    $('#UserNameKabag').val(_NomorInputView.UserNameKabag);
    $('#StatusPerjalananDinas').val(_NomorInputView.StatusPerjalananDinas);
    lst = $('#ListPermintaanPerjalananDinasKanpusId').data('kendoMultiSelect');
    lst.dataSource.read().done(function (data) {
        lst.value(_NomorInputView.ListPermintaanPerjalananDinasKanpusId);
        $('#ListNomorPermintaan').val(_NomorInputView.ListNomorPermintaan);
        $('#ListTanggalPermintaan').val(_NomorInputView.ListTanggalPermintaan);
    });
}

function ViewToModel() {
    var statusPerjalananDinas = "Utama";
    listPermintaanPerjalananDinasKanpusId = $('#ListPermintaanPerjalananDinasKanpusId').val();
    if (listPermintaanPerjalananDinasKanpusId == null) listPermintaanPerjalananDinasKanpusId = "";
    var listNomorPermintaan = $('#ListNomorPermintaan').val();
    if (listNomorPermintaan == null) listNomorPermintaan = "";
    var listTanggalPermintaan = $('#ListTanggalPermintaan').val();
    if (listTanggalPermintaan == null) listTanggalPermintaan = "";
    var mdl = {
        HDR_AlokasiPerjalananDinasId: $('#HDR_AlokasiPerjalananDinasId').val(),
        TahunBuku: $('#TahunBuku').val(),
        NomorInput: $('#NomorInput').val(),
        NomorInputView: Array(5 - String($('#NomorInput').val()).length + 1).join('0') + $('#NomorInput').val() + ' - ' + $('#NomorAlokasi').val().toUpperCase(),
        NomorAlokasi: $('#NomorAlokasi').val().toUpperCase(),
        TanggalAlokasi: $('#TanggalAlokasi').val(),
        Berangkat: $('#Berangkat').val(),
        Kembali: $('#Kembali').val(),
        JenisTujuan: $('#JenisTujuan').data('kendoDropDownList').value(),
        Tujuan: $('#Tujuan').val(),
        Keterangan: $('#Keterangan').val(),
        KendaraanDinas: $('#KendaraanDinas').data('kendoDropDownList').value() == "1" ? "true" : "false",
        NamaDriver: $('#NamaDriver').val(),
        NopolKendaraan: $('#NopolKendaraan').val(),
        UserName: $('#UserName').val(),
        VerKaur: $('#VerKaur').val(),
        VerKabag: $('#VerKabag').val(),
        TglVerKaur: $('#TglVerKaur').val(),
        TglVerKabag: $('#TglVerKabag').val(),
        UserNameKaur: $('#UserNameKaur').val(),
        UserNameKabag: $('#UserNameKabag').val(),
        ListPermintaanPerjalananDinasKanpusId: listPermintaanPerjalananDinasKanpusId.toString(),
        ListNomorPermintaan: listNomorPermintaan.toString(),
        ListTanggalPermintaan: listTanggalPermintaan.toString(),
        StatusPerjalananDinas: statusPerjalananDinas.toString()
    };
    return mdl;
}

function hapusHeader() {
    wnd.close();
    PeriksaConstraintHeader().done(function (data) {
        if (data == 0) {
            ConfirmedHapusHeader().done(function (data) {
                if (data) {
                    hdr_AlokasiPerjalananDinasId = $('#HDR_AlokasiPerjalananDinasId').val();
                    $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                        disableHeader();
                        $('#btnDeleteHeader').removeClass('disabledbutton');
                        $('#btnPrintHeader').removeClass('disabledbutton');
                        $('#btnDeleteHeader').attr('disabled', false);
                        $('#btnPrintHeader').attr('disabled', false);
                        $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                        $('#grdDetail').removeClass('disabledbutton');
                        $("#grdDetail").data("kendoGrid").dataSource.read({
                            id: $('#HDR_AlokasiPerjalananDinasId').val()
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_AlokasiPerjalananDinas]" +
                " where HDR_AlokasiPerjalananDinasId='" + $("#HDR_AlokasiPerjalananDinasId").val() + "'",
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

        if (typeof (_NomorInputView.ListPermintaanPerjalananDinasKanpusId) == "string") {
            if (_NomorInputView.ListPermintaanPerjalananDinasKanpusId == "") {
                _NomorInputView.ListPermintaanPerjalananDinasKanpusId = [];
                _NomorInputView.ListNomorPermintaan = [];
                _NomorInputView.ListTanggalPermintaan = [];
            }
            else {
                _NomorInputView.ListPermintaanPerjalananDinasKanpusId = _NomorInputView.ListPermintaanPerjalananDinasKanpusId.split(",");
                _NomorInputView.ListNomorPermintaan = _NomorInputView.ListNomorPermintaan.split(",");
                _NomorInputView.ListTanggalPermintaan = _NomorInputView.ListTanggalPermintaan.split(",");
            }
        }
        ModelToView(_NomorInputView);
        hdr_AlokasiPerjalananDinasId = _NomorInputView.HDR_AlokasiPerjalananDinasId;
        cekData(_NomorInputView.NomorInputView);
        $('#NomorAlokasi').focus();
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
        parameters: { HDR_AlokasiPerjalananDinasId: $('#HDR_AlokasiPerjalananDinasId').val() }
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

function filterSPD() {
    var berangkat,kembali,jenisTujuan, tujuan,kendaraanDinas;
        berangkat= $('#Berangkat').val();
        kembali = $('#Kembali').val();
        jenisTujuan= $('#JenisTujuan').data('kendoDropDownList').value();
        tujuan= $('#Tujuan').val();
        kendaraanDinas = $('#KendaraanDinas').data('kendoDropDownList').value();

    if (headerBaru == true) {
        var listId = [];
        for (var i = 0; i < _NomorInputView.ListPermintaanPerjalananDinasKanpusId.length; i++) {
            listId.push("'" + _NomorInputView.ListPermintaanPerjalananDinasKanpusId[i].toString() + "'");
        }       
        

        return {
            strClassView: 'Ptpn8.PerjalananDinas.Models.View_HDRPermintaanPerjalananDinasKanpus',
            scriptSQL: "select DISTINCT A.HDR_PermintaanPerjalananDinasKanpusId, A.NomorPermintaan, A.TanggalPermintaan from [SPDK_KanpusEF].[dbo].[HDR_PermintaanPerjalananDinasKanpus] as A" +
                " inner join [SPDK_KanpusEF].[dbo].[DTL_PermintaanPerjalananDinasKanpus] as B on A.HDR_PermintaanPerjalananDinasKanpusId=B.HDR_PermintaanPerjalananDinasKanpusId" +
                " where A.VerKaur=1 and B.SudahAlokasi=0 and A.Berangkat='"+berangkat+"' and A.Kembali='"+kembali+"' and A.jenisTujuan='"+jenisTujuan+"' and A.tujuan='"+tujuan+"' and A.KendaraanDinas='"+kendaraanDinas+"'",
            _menuId: menuId
        }

    }
    else {
        if (_NomorInputView.ListPermintaanPerjalananDinasKanpusId != "") {
            var listId = [];
            for (var i = 0; i < _NomorInputView.ListPermintaanPerjalananDinasKanpusId.length; i++) {
                listId.push("'" + _NomorInputView.ListPermintaanPerjalananDinasKanpusId[i].toString() + "'");
            }

            return {
                strClassView: 'Ptpn8.PerjalananDinas.Models.View_HDRPermintaanPerjalananDinasKanpus',
                scriptSQL: "select DISTINCT A.HDR_PermintaanPerjalananDinasKanpusId, A.NomorPermintaan, A.TanggalPermintaan from [SPDK_KanpusEF].[dbo].[HDR_PermintaanPerjalananDinasKanpus] as A" +
                    " inner join [SPDK_KanpusEF].[dbo].[DTL_PermintaanPerjalananDinasKanpus] as B on A.HDR_PermintaanPerjalananDinasKanpusId=B.HDR_PermintaanPerjalananDinasKanpusId" +
                    " where A.Berangkat='" + berangkat + "' and A.Kembali='" + kembali + "' and A.jenisTujuan='" + jenisTujuan + "' and A.tujuan='" + tujuan + "' and A.KendaraanDinas='" + kendaraanDinas + "'",
                _menuId: menuId
            }
        }
        else { return "" }
    }
}


function ListPermintaanPerjalananDinasKanpusIdOnSelect(e) {
    var listPermintaanPerjalananDinasKanpusId = e.sender.dataItem();
    var arr = [];
    var arr1 = [];
    if ($('#ListPermintaanPerjalananDinasKanpusId').val() != null) {
        var lst = $('#ListPermintaanPerjalananDinasKanpusId').data('kendoMultiSelect');
        for (var i = 0; i < lst.dataItems().length; i++) {
            arr.push(lst.dataItems()[i].NomorPermintaan);
            var tglPermintaan = new Date(parseInt(lst.dataItems()[i].TanggalPermintaan.substr(6)));
            arr1.push(tglPermintaan.getDate() + "/" + (tglPermintaan.getMonth() + 1) + "/" + tglPermintaan.getFullYear());
            $('#TanggalPermintaan').val(tglPermintaan.getDate() + "/" + (tglPermintaan.getMonth() + 1) + "/" + tglPermintaan.getFullYear());
        }
        $('#ListNomorPermintaan').val(arr.toString());
        $('#ListTanggalPermintaan').val(arr1.toString());
    }
}

function getDataFrom() {
    // Hitung Jumlah Record di Detail
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_AlokasiPerjalananDinas]" +
                " where HDR_AlokasiPerjalananDinasId='" + $("#HDR_AlokasiPerjalananDinasId").val() + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function InsertGridFrom() {
    var grd = $('#grdDetail').data('kendoGrid');
    var url = '/Input/GetDataFrom';
    var lst = $('#ListPermintaanPerjalananDinasKanpusId').data('kendoMultiSelect').value();
    var str = "";
    for (var i = 0; i < lst.length; i++) {
        str = str + "'" + lst[i] + "'";
        if (i < lst.length - 1) str = str + ",";
    }

    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            strClassView: "Ptpn8.PerjalananDinas.Models.View_DTLAlokasiPerjalananDinas",
            scriptSQL: "INSERT INTO [SPDK_KanpusEF].[dbo].[DTL_AlokasiPerjalananDinas] (DTL_AlokasiPerjalananDinasId, HDR_AlokasiPerjalananDinasId,NIK,Nama,Golongan,Jabatan,UangSaku,UangMakan,UangCucian,UangTransport,UangLain,JumlahBPD,DapatBiayaPenginapan)" +
                " SELECT newid() as DTL_AlokasiPerjalananDinasId, cast('" + $('#HDR_AlokasiPerjalananDinasId').val() + "' as uniqueidentifier) as HDR_AlokasiPerjalananDinasId," +
                " A.NIK, A.Nama, A.Golongan, A.Jabatan,A.UangSaku,A.UangMakan,A.UangCucian,A.UangTransport,0,A.JumlahBPD,B.DapatBiayaPenginapan " +
                " from [SPDK_KanpusEF].[dbo].[DTL_PermintaanPerjalananDinasKanpus] as A" +
                " join [SPDK_KanpusEF].[dbo].[HDR_PermintaanPerjalananDinasKanpus] as B on A.HDR_PermintaanPerjalananDinasKanpusId=B.HDR_PermintaanPerjalananDinasKanpusId" +
                " where A.HDR_PermintaanPerjalananDinasKanpusId IN (" + str + ") ",
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
        model.DTL_AlokasiPerjalananDinasId = model.uid;
        model.HDR_AlokasiPerjalananDinasId = hdr_AlokasiPerjalananDinasId;
    }
    dtl_AlokasiPerjalananDinasId = model.DTL_AlokasiPerjalananDinasId;
    gridStatus = 'sudah-diapa-apain';
}

function grdOnDataBinding(e) {

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

// copykan ke semua view

//// sampe sini
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

function JenisTujuanOnSelect(e) {
    var jenisTujuan = $('#JenisTujuan').val();
}

function KendaraanDinasOnSelect(e) {
    var kendaraanDinas = $('#KendaraanDinas').val();
}

function TujuanOnSelect(e) {
    var tujuan = $('#Tujuan').val();
    $('#ListPermintaanPerjalananDinasKanpusId').data('kendoMultiSelect').dataSource.read();
}