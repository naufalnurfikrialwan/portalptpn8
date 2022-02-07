var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var buyer;
var hdr_FakturId, dtl_FakturId;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var rowNumber = 0;
var menuId = '1d1d7f17-0ede-3d3f-58a2-0a349b0494d7';
var dataDetail;


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
                        gridStatus = 'belum-ngapa-ngapain';
                        hdr_FakturId = $('#headerFakturId').val();
                        $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                            disableHeader();
                            $('#btnDeleteHeader').removeClass('disabledbutton');
                            $('#btnPrintHeader').removeClass('disabledbutton');
                            $('#btnDeleteHeader').attr('disabled', false);
                            $('#btnPrintHeader').attr('disabled', false);
                            $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                            $('#_detail').removeClass('disabledbutton');
                            getDataFrom().done(function (data) {
                                if (data[0][0] > 0) {
                                    var grid = $("#grdDetail").data("kendoGrid");
                                    grid.dataSource.read({
                                        id: $('#headerFakturId').val()
                                    }).done(function () {
                                        var currentData = grid.dataSource.data();
                                        for (var i = 0; i < currentData.length; i++) {
                                            currentData[i].dirty = true;
                                        }
                                    });
                                }
                                else
                                {
                                    if ($('#IsiXML').val() == "") {
                                        readXML().done(function (data) {
                                            InsertGridFrom(data).done(function () {
                                                $("#grdDetail").data("kendoGrid").dataSource.read({ id: $('#headerFakturId').val() });
                                            }).fail(function (x) {
                                                alert('Error! Hubungi Bagian TI');
                                            });
                                        });
                                    }
                                    else {
                                        readIsiXML().done(function (data) {
                                            InsertGridFrom(data).done(function () {
                                                $("#grdDetail").data("kendoGrid").dataSource.read({ id: $('#headerFakturId').val() });
                                            }).fail(function (x) {
                                                alert('Error! Hubungi Bagian TI');
                                            });
                                        });
                                    }
                                }
                            }).fail(function (x) {
                                alert('Error! Hubungi Bagian TI');
                            });
                        });

                    }
                    else {
                        gridStatus = 'belum-ngapa-ngapain';
                        openErrWindow();
                        $('#_detail').addClass('disabledbutton');
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_FakturPajakMasukan]" +
                " where headerFakturId='" + $("#headerFakturId").val() + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function ModelToView(_NomorInputView) {
    $('#headerFakturId').val(_NomorInputView.headerFakturId);
    $('#TahunBuku').val(_NomorInputView.TahunBuku); 
    $('#NomorInput').val(_NomorInputView.NomorInput);
    $('#NomorInputView').val(_NomorInputView.NomorInputView);
    $('#nomorFaktur').val(_NomorInputView.nomorFaktur); 
    $('#kdJenisTransaksi').val(_NomorInputView.kdJenisTransaksi); 
    $('#fgPengganti').val(_NomorInputView.fgPengganti); 
    var tglFaktur = new Date(parseInt(_NomorInputView.tanggalFaktur.substr(6)));
    $('#tanggalFaktur').val(tglFaktur.getDate() + "/" + (tglFaktur.getMonth() + 1) + "/" + tglFaktur.getFullYear());
    $('#npwpPenjual').val(_NomorInputView.npwpPenjual); 
    $('#namaPenjual').val(_NomorInputView.namaPenjual); 
    $('#alamatPenjual').val(_NomorInputView.alamatPenjual); 
    $('#npwpLawanTransaksi').val(_NomorInputView.npwpLawanTransaksi); 
    $('#namaLawanTransaksi').val(_NomorInputView.namaLawanTransaksi); 
    $('#alamatLawanTransaksi').val(_NomorInputView.alamatLawanTransaksi);
    $('#jumlahDpp').val(_NomorInputView.jumlahDpp); 
    $('#jumlahPpn').val(_NomorInputView.jumlahPpn); 
    $('#jumlahPpnBm').val(_NomorInputView.jumlahPpnBm);
    $('#statusApproval').val(_NomorInputView.statusApproval);
    $('#statusFaktur').val(_NomorInputView.statusFaktur); 
    $('#url').val(_NomorInputView.url); 
    $('#eksporToCSV').val(_NomorInputView.eksporToCSV); 
    $('#eksporToExcel').val(_NomorInputView.eksporToExcel);
    $('#BulanMasaPajak').data('kendoDropDownList').value(_NomorInputView.BulanMasaPajak);  
    $('#TahunMasaPajak').val(_NomorInputView.TahunMasaPajak);
    $('#DapatDiKredit').data('kendoDropDownList').value(_NomorInputView.DapatDiKredit);
    $('#NPWP').val(_NomorInputView.NPWP);
    $('#UserName').val(_NomorInputView.UserName); 
    $('#KebunId').val(_NomorInputView.KebunId);
    $('#Referensi').val(_NomorInputView.Referensi);
    $('#NamaKebun').val(_NomorInputView.NamaKebun);
    var tglScan = new Date(parseInt(_NomorInputView.TanggalScan.substr(6)));
    $('#TanggalScan').val(tglScan.getDate() + "/" + (tglScan.getMonth() + 1) + "/" + tglScan.getFullYear());
    $('#IsiXML').val(_NomorInputView.IsiXML);

}

function ViewToModel() {
    var mdl = {
        headerFakturId : $('#headerFakturId').val(),
        TahunBuku : $('#TahunBuku').val(),
        NomorInput : $('#NomorInput').val(),
        NomorInputView : Array(5 - String($('#NomorInput').val()).length + 1).join('0') + $('#NomorInput').val() + ' - ' + $('#nomorFaktur').val().toUpperCase(),
        nomorFaktur : $('#nomorFaktur').val(),
        kdJenisTransaksi : $('#kdJenisTransaksi').val(),
        fgPengganti : $('#fgPengganti').val(),
        tanggalFaktur : $('#tanggalFaktur').val(),
        npwpPenjual : $('#npwpPenjual').val(),
        namaPenjual : $('#namaPenjual').val(),
        alamatPenjual : $('#alamatPenjual').val(),
        npwpLawanTransaksi : $('#npwpLawanTransaksi').val(),
        namaLawanTransaksi : $('#namaLawanTransaksi').val(),
        alamatLawanTransaksi : $('#alamatLawanTransaksi').val(),
        jumlahDpp : $('#jumlahDpp').val(),
        jumlahPpn : $('#jumlahPpn').val(),
        jumlahPpnBm : $('#jumlahPpnBm').val(),
        statusApproval : $('#statusApproval').val(),
        statusFaktur : $('#statusFaktur').val(),
        url : $('#url').val(),
        eksporToCSV : $('#eksporToCSV').val(),
        eksporToExcel : $('#eksporToExcel').val(),
        BulanMasaPajak : $('#BulanMasaPajak').val(),   
        TahunMasaPajak : $('#TahunMasaPajak').val(),
        DapatDiKredit : $('#DapatDiKredit').val(),
        NPWP : $('#NPWP').val(),
        UserName : $('#UserName').val(),
        KebunId: $('#KebunId').val(),
        NamaKebun: $('#NamaKebun').val(),
        TanggalScan: $('#TanggalScan').val(),
        Referensi: $('#Referensi').val(),
        IsiXML: $('#IsiXML').val()
    };
    return mdl;
}

function hapusHeader() {
    wnd.close();
    PeriksaConstraintHeader().done(function (data) {
        if (data == 0) {
            ConfirmedHapusHeader().done(function (data) {
                if (data) {
                    hdr_FakturId = $('#headerFakturId').val();
                    $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                        disableHeader();
                        $('#btnDeleteHeader').removeClass('disabledbutton');
                        $('#btnPrintHeader').removeClass('disabledbutton');
                        $('#btnDeleteHeader').attr('disabled', false);
                        $('#btnPrintHeader').attr('disabled', false);
                        $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                        $('#_detail').removeClass('disabledbutton');
                        $("#grdDetail").data("kendoGrid").dataSource.read({
                            id: $('#headerFakturId').val()
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_FakturPajakMasukan]" +
                " where headerFakturId='" + $("#headerFakturId").val() + "'",
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
        hdr_FakturId = _NomorInputView.headerFakturId;
        $('#url').focus();
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

function btnPrintHeaderOnClick() {
    var viewer = $("#reportViewer").data("telerik_ReportViewer");
    viewer.reportSource({
        report: viewer.reportSource().report,
        parameters: { headerFakturId: $('#headerFakturId').val() }
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

function btnGetUrlContentOnClick(e) {
    var url = $('#url').val();
    readXML().done(function (data) {
        var HDR = data.ModelHDR[0];
        HDR.url = url;
        HDR.TahunBuku = $('#TahunBuku').val();
        HDR.NomorInputView = $('#NomorInputView').val();
        HDR.NomorInput = $('#NomorInput').val();
        HDR.IsiXML = $('#IsiXML').val();
        ModelToView(HDR);
    });
}


function btnGetXMLContentOnClick(e) {
    readIsiXML().done(function (data) {
        var HDR = data.ModelHDR[0];
        HDR.url = $('#url').val();
        HDR.TahunBuku = $('#TahunBuku').val();
        HDR.NomorInputView = $('#NomorInputView').val();
        HDR.NomorInput = $('#NomorInput').val();
        HDR.IsiXML = $('#IsiXML').val();
        ModelToView(HDR);
    });
}



function readXML() {
    var url = '/Input/ReadXML';
    return $.ajax({
        url: url,
        type: 'POST',
        datatype: 'json',
        data: {
            url: $('#url').val(),
            strClassHeader: 'Ptpn8.eFakturPajak.Models.View_HeaderFaktur',
            strClassDetail: 'Ptpn8.eFakturPajak.Models.View_DetailFaktur',
            _menuId: menuId
        }
    });
}


function readIsiXML() {
    var url = '/Input/ReadIsiXML';
    return $.ajax({
        url: url,
        type: 'POST',
        datatype: 'json',
        data: {
            isiXML: $('#IsiXML').val(),
            strClassHeader: 'Ptpn8.eFakturPajak.Models.View_HeaderFaktur',
            strClassDetail: 'Ptpn8.eFakturPajak.Models.View_DetailFaktur',
            _menuId: menuId
        }
    });
}

// Detail


function InsertGridFrom(data)
{
    var url = $('#url').val();
    var strSQL = "";
    var DTL = data.ModelDTL;
    if (DTL && DTL.length > 0) {
        strSQL = "INSERT INTO [SPDK_KanpusEF].[dbo].[DTL_FakturPajakMasukan] (detailFakturId,headerFakturId,nama,hargaSatuan,jumlahBarang,hargaTotal,diskon,dpp,ppn,tarifPpnbm,ppnbm) values ";
        var arrContent = [];
        for (i = 0; i < DTL.length; i++) {
            arrContent[0] = "newid()";
            arrContent[1] = "'"+$('#headerFakturId').val()+"'";
            arrContent[2] = "'"+DTL[i].nama+"'";
            arrContent[3] = DTL[i].hargaSatuan.toString();
            arrContent[4] = DTL[i].jumlahBarang.toString();
            arrContent[5] = DTL[i].hargaTotal.toString();
            arrContent[6] = DTL[i].diskon.toString();
            arrContent[7] = DTL[i].dpp.toString();
            arrContent[8] = DTL[i].ppn.toString();
            arrContent[9] = DTL[i].tarifPpnbm.toString();
            arrContent[10] = DTL[i].ppnbm.toString();

            strSQL = strSQL + "(" + arrContent.join(', ') + ")";
            if(i < DTL.length - 1)
            {
                strSQL = strSQL + ",";
            }
        }
    }

    return $.ajax({
        url: '/Input/GetDataFrom',
        data: {
            strClassView: "Ptpn8.eFakturPajak.Models.View_DetailFaktur",
            scriptSQL: strSQL,
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function grdOnChange(e) {
}

function grdOnEdit(e) {
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.detailFakturId = model.uid;
        model.headerFakturId = hdr_FakturId;
    }
    dtl_FakturId = model.detailFakturId;
    gridStatus = 'sudah-diapa-apain';
}

function grdOnDataBinding(e) {

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
        listId.push("'" + data[i].detailFakturId.toString() + "'");
    }
    var strId = listId.toString();

    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_PPB]" +
                " where detailFakturId in (" + strId + ")",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function filterdetailRead(e) {
    return {_menuId: menuId
    };
}

function filterdetailCreate(e) {
    return { _menuId: menuId };
}

//function filterUnit() {
//    return {
//        value: $("#NamaKebun").val()
//    };
//}

function aucLokasiKerjaOnSelect(e) {
    var unit = this.dataItem(e.item);
    $('#KebunId').val(unit.LokasiKerjaId);
}

function filterLokasiKerja() {
    return {
        //id: IdBudidaya,
        value: $('#NamaKebun').val()
    };
}
