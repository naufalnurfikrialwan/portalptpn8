var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var mPP_RasioKebunId;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var rowNumber = 0;
var menuId = '70CE485D-1D2A-478A-8F43-0679F5F0D670';
var jumlah;

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

angular.module("__header", ["kendo.directives"])
    .controller("MyCtrl", function ($scope) {
        $scope.validate = function (event) {
            event.preventDefault();
            if ($scope.validator.validate()) {
                simpanHeader().done(function (data) {
                    if (data) {
                        mPP_RasioKebunId = $('#MPP_RasioKebunId').val();
                        $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                            disableHeader();
                            $('#btnDeleteHeader').removeClass('disabledbutton');
                            $('#btnDeleteHeader').attr('disabled', false);
                        });
                    }
                    else {
                        openErrWindow();
                    }
                })
                    .fail(function (x) {
                        alert("Error! Hubungi Bagian TI");
                    });
            } else {
                
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
    $('#MPP_RasioKebunId').val(_NomorInputView.MPP_RasioKebunId);
    $('#TahunBuku').val(_NomorInputView.TahunBuku);
    var unit = (_NomorInputView.UnitId);
    var unitString = unit.toString();
    $('#UnitId').data('kendoDropDownList').value(unitString);
    $('#TglVerKaur').val(_NomorInputView.TglVerKaur);
    $('#UserNameKaur').val(_NomorInputView.UserNameKaur);
    $('#VerKaur').val(_NomorInputView.VerKaur);
    $('#NomorInput').val(_NomorInputView.NomorInput);
    $('#NomorInputView').val(_NomorInputView.NomorInputView);
    $('#No_Register').val(_NomorInputView.No_Register);
    $('#Tanggal_Register').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.Tanggal_Register.substr(6))));
    $('#UserName').val(_NomorInputView.UserName);
    $('#HariKerjaEfektif').val(_NomorInputView.HariKerjaEfektif);
    $('#TipeKebun').data('kendoDropDownList').value(_NomorInputView.TipeKebun);
    $('#JumlahAfdeling').val(_NomorInputView.JumlahAfdeling);
    $('#JumlahPLTA').val(_NomorInputView.JumlahPLTA);
    $('#JumlahProduksiBasah').val(_NomorInputView.JumlahProduksiBasah);
    $('#LuasArealKebunEfektif').val(_NomorInputView.LuasArealKebunEfektif);
    $('#KompisisiGuntingPotongManual').val(_NomorInputView.KompisisiGuntingPotongManual);
    $('#KompisisiMesinPotong').val(_NomorInputView.KompisisiMesinPotong);
    $('#KompisisiPangkasManual').val(_NomorInputView.KompisisiPangkasManual);
    $('#KompisisiPangkasMesin').val(_NomorInputView.KompisisiPangkasMesin);
    $('#JumlahTitikPenjagaan').val(_NomorInputView.JumlahTitikPenjagaan);
    $('#JumlahTruk').val(_NomorInputView.JumlahTruk);
    $('#JumlahMobil').val(_NomorInputView.JumlahMobil);
}

function ViewToModel() {
    var mdl = {
        MPP_RasioKebunId: $('#MPP_RasioKebunId').val(),
        TahunBuku: $('#TahunBuku').val(),
        UnitId: $('#UnitId').val(),
        TglVerKaur: $('#TglVerKaur').val(),
        UserNameKaur: $('#UserNameKaur').val(),
        VerKaur: $('#VerKaur').val(),
        NomorInput: $('#NomorInput').val(),
        NomorInputView: Array(5 - String($('#NomorInput').val()).length + 1).join('0') + $('#NomorInput').val() + ' - ' + $('#No_Register').val().toUpperCase(),
        No_Register: $('#No_Register').val().toUpperCase(),
        Tanggal_Register: $('#Tanggal_Register').val(),
        UserName: $('#UserName').val(),
        HariKerjaEfektif: $('#HariKerjaEfektif').val(),
        TipeKebun: $('#TipeKebun').val(),
        JumlahAfdeling: $('#JumlahAfdeling').val(),
        JumlahPLTA: $('#JumlahPLTA').val(),
        JumlahProduksiBasah: $('#JumlahProduksiBasah').val(),
        LuasArealKebunEfektif: $('#LuasArealKebunEfektif').val(),
        KompisisiGuntingPotongManual: $('#KompisisiGuntingPotongManual').val(),
        KompisisiMesinPotong: $('#KompisisiMesinPotong').val(),
        KompisisiPangkasManual: $('#KompisisiPangkasManual').val(),
        KompisisiPangkasMesin: $('#KompisisiPangkasMesin').val(),
        JumlahTitikPenjagaan: $('#JumlahTitikPenjagaan').val(),
        JumlahTruk: $('#JumlahTruk').val(),
        JumlahMobil: $('#JumlahMobil').val(),
    };
    return mdl;
}

function hapusHeader() {
    wnd.close();
    ConfirmedHapusHeader().done(function (data) {
        if (data) {
            mPP_RasioKebunId = $('#MPP_RasioKebunId').val();
            $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                disableHeader();
                $('#btnDeleteHeader').removeClass('disabledbutton');
                $('#btnPrintHeader').removeClass('disabledbutton');
                $('#btnDeleteHeader').attr('disabled', false);
                $('#btnPrintHeader').attr('disabled', false);
                $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                $('#grdDetail').removeClass('disabledbutton');
            });
        }
        else {
            openErrWindow();
        }
    }).fail(function (x) {
        alert('Error! Hubungi Bagian TI');
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
        mPP_RasioKebunId = _NomorInputView.MPP_RasioKebunId;
        cekData(_NomorInputView.NomorInputView);
        $('#No_Register').focus();
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

function disableHeader() {
    $("._key").find('span').addClass('disabledbutton');
    $("._key").addClass('disabledbutton');
    $("._nonkey").find('span').addClass('disabledbutton');
    $("._nonkey").addClass('disabledbutton');

    $('#btnSubmitHeader').addClass('disabledbutton');
    $('#btnDeleteHeader').addClass('disabledbutton');
}

function enableHeader() {
    $("._key").find('span').removeClass('disabledbutton');
    $("._key").removeClass('disabledbutton');
    $("._nonkey").find('span').removeClass('disabledbutton');
    $("._nonkey").removeClass('disabledbutton');
    $('#btnSubmitHeader').removeClass('disabledbutton');
    $('#btnDeleteHeader').removeClass('disabledbutton');
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
    }
}

function filterNomorInput(e) {
    var mdl = JSON.stringify(ViewToModel());
    return { _model: mdl, _menuId: menuId };
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