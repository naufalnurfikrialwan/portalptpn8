var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var hdr_RemiseId, kebunId, barangId, namaBarang, norek, dtl_RemiseId, rekeningId;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var rowNumber = 0;
var menuId = '99931797-0c3d-2633-0de7-f04285976de1';
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
                if (input.attr("name") == "JumlahOrang" || input.attr("name") == "JumlahMakanPagi" ||
                    input.attr("name") == "JumlahMakanSiang" || input.attr("name") == "JumlahMakanMalam" || input.attr("name") == "JumlahSnack") {


                    var grd = $('#grdDetail').data('kendoGrid');
                    var dataItem = typeof (grd.dataSource.get(dtl_RemiseId)) == "undefined" ? grd.dataSource.getByUid(dtl_RemiseId) : grd.dataSource.get(dtl_RemiseId);
                    var taripMakan = parseFloat($('#TaripMakan').val());
                    var taripSnack = parseFloat($('#TaripSnack').val());
                    var jmlOrang = parseFloat(dataItem["JumlahOrang"]);
                    //var volume = parseFloat(dataItem["Volume"]);
                    //var dptMakanPagi = dataItem["DapatMakanPagi"];
                    //var dptMakanSiang = dataItem["DapatMakanSiang"];
                    //var dptMakanMalam = dataItem["DapatMakanMalam"];
                    //var dptSnack = dataItem["DapatSnack"];

                    var jmlMakanPagi = dataItem["JumlahMakanPagi"];
                    var jmlMakanSiang = dataItem["JumlahMakanSiang"];
                    var jmlMakanMalam = dataItem["JumlahMakanMalam"];
                    var jmlSnack = dataItem["JumlahSnack"];


                    dataItem.dirty = true;
                    dataItem["JumlahOrang_Disetujui"] = jmlOrang;
                    //dataItem["Volume_Disetujui"] = volume;

                    //dataItem["DapatMakanPagi_Disetujui"] = dataItem["DapatMakanPagi"];
                    //dataItem["DapatMakanSiang_Disetujui"] = dataItem["DapatMakanSiang"];
                    //dataItem["DapatMakanMalam_Disetujui"] = dataItem["DapatMakanMalam"];
                    //dataItem["DapatSnack_Disetujui"] = dataItem["DapatSnack"];
                    //dataItem["JumlahBiaya"] = (jmlOrang * volume * (dptMakanPagi ? taripMakan : 0)) +
                    //    (jmlOrang * volume * (dptMakanSiang ? taripMakan : 0)) +
                    //    (jmlOrang * volume * (dptMakanMalam ? taripMakan : 0)) +
                    //    (jmlOrang * volume * (dptSnack ? taripSnack : 0));
                    //dataItem["JumlahBiaya_Disetujui"] = dataItem["JumlahBiaya"];

                    dataItem["JumlahMakanPagi_Disetujui"] = dataItem["JumlahMakanPagi"];
                    dataItem["JumlahMakanSiang_Disetujui"] = dataItem["JumlahMakanSiang"];
                    dataItem["JumlahMakanMalam_Disetujui"] = dataItem["JumlahMakanMalam"];
                    dataItem["JumlahSnack_Disetujui"] = dataItem["JumlahSnack"];
                    dataItem["JumlahBiaya"] = (jmlOrang * jmlMakanPagi * taripMakan) +
                        (jmlOrang * jmlMakanSiang * taripMakan) +
                        (jmlOrang * jmlMakanMalam * taripMakan) +
                        (jmlOrang * jmlSnack * taripSnack);
                    dataItem["JumlahBiaya_Disetujui"] = dataItem["JumlahBiaya"];

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
                        hdr_RemiseId = $('#HDR_RemiseBiayaTamuId').val();
                        disableHeader();
                        $('#btnDeleteHeader').removeClass('disabledbutton');
                        $('#btnPrintHeader').removeClass('disabledbutton');
                        $('#btnDeleteHeader').attr('disabled', false);
                        $('#btnPrintHeader').attr('disabled', false);
                        $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                        $('#grdDetail').removeClass('disabledbutton');

                        var grid = $("#grdDetail").data("kendoGrid");
                        grid.dataSource.read({id: $('#HDR_RemiseBiayaTamuId').val()});
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
    $('#HDR_RemiseBiayaTamuId').val(_NomorInputView.HDR_RemiseBiayaTamuId);
    $('#TahunBuku').val(_NomorInputView.TahunBuku);
    $('#BulanRemise').val(_NomorInputView.BulanRemise);
    $('#TahunRemise').val(_NomorInputView.TahunRemise);
    $('#KebunId').val(_NomorInputView.KebunId);
    $('#NamaKebun').val(_NomorInputView.NamaKebun);
    $('#NomorInput').val(_NomorInputView.NomorInput);
    $('#NomorInputView').val(_NomorInputView.NomorInputView);
    $('#TanggalInput').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.TanggalInput.substr(6))));
    $('#UserName').val(_NomorInputView.UserName);
    $('#TaripMakan').data('kendoNumericTextBox').value(_NomorInputView.TaripMakan);
    $('#TaripSnack').data('kendoNumericTextBox').value(_NomorInputView.TaripSnack);
    $('#StatusFinal').val('BELUM FINAL');
}

function ViewToModel() {
    var mdl = {
        HDR_RemiseBiayaTamuId: $('#HDR_RemiseBiayaTamuId').val(),
        TahunBuku: $('#TahunBuku').val(),
        BulanRemise: $('#BulanRemise').val(),
        TahunRemise: $('#TahunRemise').val(),
        KebunId: $('#KebunId').val(),
        NamaKebun: $('#NamaKebun').val().toUpperCase(),
        NomorInput: $('#NomorInput').val(),
        NomorInputView: $('#NomorInput').val(),
        TanggalInput: $('#TanggalInput').val(),
        UserName: $('#UserName').val(),
        TaripMakan: $('#TaripMakan').val(),
        TaripSnack: $('#TaripSnack').val(),
        StatusFinal: $('#StatusFinal').val()
    };
    return mdl;
}

function hapusHeader() {
    wnd.close();
    PeriksaConstraintHeader().done(function (data) {
        if (data == 0) {
            ConfirmedHapusHeader().done(function (data) {
                if (data) {
                    hdr_PermintaanBarangDariKebunId = $('#HDR_RemiseBiayaTamuId').val();
                    //$('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                        disableHeader();
                        $('#btnDeleteHeader').removeClass('disabledbutton');
                        $('#btnPrintHeader').removeClass('disabledbutton');
                        $('#btnDeleteHeader').attr('disabled', false);
                        $('#btnPrintHeader').attr('disabled', false);
                        $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                        $('#grdDetail').removeClass('disabledbutton');
                        $("#grdDetail").data("kendoGrid").dataSource.read({
                            id: $('#HDR_RemiseBiayaTamuId').val()
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

function HitungTaripBPD(cSQL)
{
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {

            scriptSQL: cSQL,
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function PeriksaConstraintHeader() {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_RemiseBiayaTamu]" +
                " where HDR_RemiseBiayaTamuId='" + $("#HDR_RemiseBiayaTamuId").val() + "'",
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
        var dataItem = this.dataItem(e.item);
        var noInput = parseInt(dataItem.Value.substring(0, 5));
        $('#NomorInput').val(noInput);
        $('#BulanRemise').val(noInput);
        $('#TahunRemise').val($('#TahunBuku').val());
        
        getDataFrom4().done(function (results) {
            if (results.length) {
                _NomorInputView = results[0];
                _NomorInputView.NomorInputView = dataItem.Text;
                ModelToView(_NomorInputView);
                headerBaru = false;
            }
            else
            {
                _NomorInputView = ViewToModel();
                _NomorInputView.NomorInputView = dataItem.Text;
                headerBaru = true;
            }
            hdr_RemiseId = _NomorInputView.HDR_RemiseBiayaTamuId;
            kebunId = _NomorInputView.KebunId;
            if (_NomorInputView.StatusFinal == "BELUM FINAL") {
                cekData(_NomorInputView.NomorInputView);
            }
        });

    }
}

function getDataFrom4() {
    // Hitung Jumlah Record di Detail
    var url = '/Input/GetDataFrom';
    return $.ajax({
        url: url,
        data: {
            strClassView: "Ptpn8.Remise.Models.View_HDRRemiseBiayaTamu",
            scriptSQL: "select A.*, B.Nama as NamaKebun, '' as NomorInputView from [SPDK_KanpusEF].[dbo].[HDR_RemiseBiayaTamu] A " +
                "join [ReferensiEF].[dbo].[Kebun] B on A.KebunId=B.KebunId " +
                "where BulanRemise="+$('#BulanRemise').val()+" and TahunRemise="+$('#TahunRemise').val()+" and A.KebunId='"+$('#KebunId').val()+"' and NomorInput="+$('#NomorInput').val(),
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
        parameters: { HDR_RemiseBiayaTamuId: $('#HDR_RemiseBiayaTamuId').val() }
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
    if (headerBaru)   // Data Baru
    {
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

}// Detail

function grdOnChange(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    var model = e.model;
}

function grdOnEdit(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.DTL_RemiseBiayaTamuId = model.uid;
        model.HDR_RemiseBiayaTamuId = hdr_RemiseId;
    }
    $(e.container).find("input[name='JumlahBPD']").prop('disabled', true);

    dtl_RemiseId = model.DTL_RemiseBiayaTamuId
    kebunId = $('#KebunId').val();
    gridStatus = 'sudah-diapa-apain';

    $('#jmlBiaya').text(kendo.toString(rekapJmlBiaya(), 'n2'));
}

function rekapJmlBiaya() {

    var grid = $('#grdDetail').data('kendoGrid');
    var newValue = 0;
    for (var i = 0; i < grid.dataItems().length; i++) {
        newValue += grid.dataItems()[i].JumlahBiaya;
    }
    return newValue;
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
            scriptSQL: "select count(*) as JmlRecord from  [SPDK_KanpusEF].[dbo].[DTL_RemiseBiayaTamu]" +
                " where HDR_RemiseBiayaTamuId='" + $("#HDR_RemiseBiayaTamuId").val() + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function grdOnBound(e) {

}

