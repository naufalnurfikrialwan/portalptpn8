var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var kebun;
var hdr_ProductSampleId, mainBudidayaId, subBudidayaId, dtl_ProductSampleId;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var rowNumber = 0;
var menuId = '17d2f2b6-c079-4bfb-81ad-0e4db7db63f4';

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


    // copykan ke semua view 22/09/16 16:40
    $("#grdDetail").kendoValidator({
        rules: { // custom rules
            validation: function (input) {
                if (input.attr("name") == "QRCode" && input.val() != "") {
                    var grid = $('#grdDetail').data('kendoGrid');
                    var dataItem = typeof (grid.dataSource.get(dtl_ProductSampleId)) == "undefined" ? grid.dataSource.getByUid(dtl_ProductSampleId) : grid.dataSource.get(dtl_ProductSampleId);
                    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
                    var arrQRCode = input.val().split("|");
                    dataItem.No_SP = arrQRCode[0];
                    dataItem.Tanggal_SP = arrQRCode[1].replace("-","/").replace("-","/");
                    dataItem.BulanProduksi = arrQRCode[2].split('-')[0];
                    dataItem.TahunProduksi = arrQRCode[2].split('-')[1];
                    dataItem.NamaMerk = arrQRCode[3];
                    dataItem.Chop = arrQRCode[4];
                    dataItem.NamaGrade = arrQRCode[5];
                    dataItem.QtySacks = arrQRCode[6];
                    dataItem.No_SC = arrQRCode[7];
                    dataItem.NamaBroker = arrQRCode[8];
                    dataItem.NamaSubBudidaya = arrQRCode[9];
                    dataItem.NamaSatuan = arrQRCode[10];
                    row[0].cells[8].textContent = dataItem.No_SP;
                    row[0].cells[9].textContent = dataItem.Tanggal_SP;
                    row[0].cells[10].textContent = dataItem.NamaSubBudidaya;
                    row[0].cells[11].textContent = dataItem.NamaMerk;
                    row[0].cells[12].textContent = dataItem.NamaGrade;
                    row[0].cells[13].textContent = dataItem.Chop;
                    row[0].cells[14].textContent = dataItem.BulanProduksi;
                    row[0].cells[15].textContent = dataItem.TahunProduksi;
                    row[0].cells[16].textContent = dataItem.QtySacks;
                    row[0].cells[17].textContent = dataItem.NamaSatuan;
                    row[0].cells[18].textContent = dataItem.Gross;
                    row[0].cells[19].textContent = dataItem.Tare;
                    row[0].cells[20].textContent = dataItem.NamaBroker;
                    return true;
                }
                else if (input.attr("name") == "Chop" && input.val() == "") {
                    $('#errMsg').text('No Chop Harus Diisi!');
                    openErrWindow();
                    return false;
                }
                else if (input.attr("name") == "NamaMerk" && input.val() == "") {
                    $('#errMsg').text('Merk/Kebun Harus Diisi!');
                    openErrWindow();
                    return false;
                }
                else if (input.attr("name") == "NamaSubBudidaya" && input.val() == "") {
                    $('#errMsg').text('Budidaya Harus Diisi!');
                    openErrWindow();
                    return false;
                }
                else if (input.attr("name") == "NamaGrade" && input.val() == "") {
                    $('#errMsg').text('Grade Harus Diisi!');
                    openErrWindow();
                    return false;
                }
                else if (input.attr("name") == "Chop" && input.val() != "") {
                    cekNoChop(input).done(function (r) {
                        if (r == 0) {
                            return true;
                        } else {
                            for (var i = 0; i < r.length; i++) {
                                if (r[i][0] != dtl_ProductSampleId) {
                                    var grid = $('#grdDetail').data('kendoGrid');
                                    var dataItem = typeof (grid.dataSource.get(dtl_ProductSampleId)) == "undefined" ?
                                        grid.dataSource.getByUid(dtl_ProductSampleId) : grid.dataSource.get(dtl_ProductSampleId);
                                    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
                                    dataItem.DTL_ProductSampleId = r[i][0];
                                    dataItem.id = r[i][0];
                                    row[0].style.color = "white";
                                    row[0].style.backgroundColor = "red";
                                    return true;
                                }
                            }
                            return true;
                        }
                    }).fail(function () {
                        return false;
                    });

                    //input.attr("data-chopvalidation-msg", "Chop Sudah Ada!")
                    //cekNoChop(input).done(function (r) {
                    //    if (r == 0) {
                    //        return true;
                    //    } else {
                    //        for (var i = 0; i < r.length; i++) {
                    //            if (r[i][0] != dtl_ProductSampleId) {
                    //                $('#errMsg').text('No Chop Sudah Digunakan!');
                    //                openErrWindow();
                    //                var grid = $('#grdDetail').data('kendoGrid');
                    //                var dataItem = typeof (grid.dataSource.get(dtl_ProductSampleId)) == "undefined" ?
                    //                    grid.dataSource.getByUid(dtl_ProductSampleId) : grid.dataSource.get(dtl_ProductSampleId);
                    //                dataItem.Chop = '';
                    //                return false;
                    //            }
                    //        }
                    //        return true;
                    //    }
                    //})
                    //.fail(function (x) {
                    //    return false;
                    //});
                }
                else if (input.attr("name") == "NamaSubBudidaya" && input.val() != "") {
                    cekNamaSubBudidaya(input.val()).done(function (data) {
                        if (data.length > 0) {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_ProductSampleId)) == "undefined" ? grid.dataSource.getByUid(dtl_ProductSampleId) : grid.dataSource.get(dtl_ProductSampleId);
                            dataItem.SubBudidayaId = data[0][0];
                            subBudidayaId = data[0][0];
                        }
                    });
                }
                else if (input.attr("name") == "NamaMerk" && input.val() != "") {
                    cekNamaMerk(input.val()).done(function (data) {
                        if (data.length > 0) {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_ProductSampleId)) == "undefined" ? grid.dataSource.getByUid(dtl_ProductSampleId) : grid.dataSource.get(dtl_ProductSampleId);
                            dataItem.MerkId = data[0][0];
                        }
                    });
                }
                else if (input.attr("name") == "NamaGrade" && input.val() != "") {
                    cekNamaGrade(input.val()).done(function (data) {
                        if (data.length > 0) {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_ProductSampleId)) == "undefined" ? grid.dataSource.getByUid(dtl_ProductSampleId) : grid.dataSource.get(dtl_ProductSampleId);
                            dataItem.GradeId = data[0][0];
                            if (dataItem.QtySacks == 0) {
                                dataItem.QtySacks = data[0][1];
                                dataItem.Gross = (data[0][1] * data[0][2]) + data[0][3];
                                dataItem.Tare = data[0][3];
                                $('#QtySacks').val(data[0][1]);
                            }
                        }
                    });
                }
                else if (input.attr("name") == "NamaSatuan" && input.val() != "") {
                    cekNamaSatuan(input.val()).done(function (data) {
                        if (data.length > 0) {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_ProductSampleId)) == "undefined" ? grid.dataSource.getByUid(dtl_ProductSampleId) : grid.dataSource.get(dtl_ProductSampleId);
                            dataItem.SatuanId = data[0][0];
                        }
                    });
                }
                else if (input.attr("name") == "NamaBroker" && input.val() != "") {
                    cekNamaBroker(input.val()).done(function (data) {
                        if (data.length > 0) {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_ProductSampleId)) == "undefined" ? grid.dataSource.getByUid(dtl_ProductSampleId) : grid.dataSource.get(dtl_ProductSampleId);
                            dataItem.BrokerId = data[0][0];
                        }
                    });
                }
                else if (input.attr("name") == "QtySacks" && input.val() != "") {
                    ambilStandarGrade().done(function (r) {
                        if (r == 0) {
                            return true;
                        } else {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_ProductSampleId)) == "undefined" ? grid.dataSource.getByUid(dtl_ProductSampleId) : grid.dataSource.get(dtl_ProductSampleId);
                            dataItem.Tare = (input.val() / r[0][0]) * r[0][2];
                            dataItem.Gross = (r[0][1] * input.val()) + dataItem.Tare;
                            return true;
                        }
                    })
                    .fail(function (x) {
                        return false;
                    });
                }
                else {
                    return true;
                };
            }
        }
    });
    //////// sampe sini
});

// copykan ke semua view
function cekNoChop(input) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_ProductSampleId)) == "undefined" ? grid.dataSource.getByUid(dtl_ProductSampleId) : grid.dataSource.get(dtl_ProductSampleId);
    var items = grid.dataItems();
    var arr = [, ];
    for (var i = 0; i < items.length; i++) {
        if (items[i].Chop == input.val() && items[i].DTL_ProductSampleId != dtl_ProductSampleId
            && items[i].Chop.toLowerCase() != "na" && items[i].MerkId == dataItem.MerkId) {
            arr.push([items[i].DTL_ProductSampleId, 1]);
            return $.ajax(arr);
        }
    }
    return $.ajax({
        url: '/Input/ExecSQL',
        data: {
            scriptSQL:
                " select DTL_ProductSampleId as Id, count(*) as Cnt from [SPDK_KanpusEF].[dbo].[DTL_ProductSample]" +
                " where MerkId='" + dataItem.MerkId + "'" +
                " and SubBudidayaId='" + dataItem.SubBudidayaId + "'" +
                " and Chop='" + input.val() + "'  and lower(Chop)!='na' and TahunProduksi='"+dataItem.TahunProduksi+"' group by DTL_ProductSampleId",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function renderNumber(data) {
    var no = ++rowNumber;
    data.NoBaris = no;
    return no;
}

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

function cekNamaBroker(namaBroker) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select BrokerId from [ReferensiEF].[dbo].[Broker]" +
                " where lower(Nama)='" + namaBroker.toLowerCase() + "'",
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
                        hdr_ProductSampleId = $('#HDR_ProductSampleId').val();
                        $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                            disableHeader();
                            $('#btnDeleteHeader').removeClass('disabledbutton');
                            $('#btnPrintHeader').removeClass('disabledbutton');
                            $('#btnDeleteHeader').attr('disabled', false);
                            $('#btnPrintHeader').attr('disabled', false);
                            $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                            $('#grdDetail').removeClass('disabledbutton');
                            $("#grdDetail").data("kendoGrid").dataSource.read({
                                id: $('#HDR_ProductSampleId').val()
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
    //var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = grid.dataSource.getByUid(row.data().uid);
    wndDetail.open().center();
    $("#yesDetail").click(function () {
        wndDetail.close();
        PeriksaConstraintDetail(dataItem).done(function (data) {
            if (data == 0) {
                grid.removeRow(row);
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
    $('#HDR_ProductSampleId').val(_NomorInputView.HDR_ProductSampleId);
    $('#TahunBuku').val(_NomorInputView.TahunBuku);
    $('#NomorInput').val(_NomorInputView.NomorInput);
    $('#NomorInputView').val(_NomorInputView.NomorInputView);
    $('#No_ProductSample').val(_NomorInputView.No_ProductSample);
    $('#Tanggal_ProductSample').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.Tanggal_ProductSample.substr(6))));
    $('#UserName').val(_NomorInputView.UserName);
    $('#MainBudidayaId').val(_NomorInputView.MainBudidayaId);
    $('#NamaMainBudidaya').val(_NomorInputView.NamaMainBudidaya);

}

function ViewToModel() {
    var budidaya = "Teh";
    var mdl = {
        HDR_ProductSampleId: $('#HDR_ProductSampleId').val(),
        TahunBuku: $('#TahunBuku').val(),
        NomorInput: $('#NomorInput').val(),
        NomorInputView: Array(5 - String($('#NomorInput').val()).length + 1).join('0') + $('#NomorInput').val() + ' - ' + $('#No_ProductSample').val().toUpperCase(),
        No_ProductSample: $('#No_ProductSample').val().toUpperCase(),
        Tanggal_ProductSample: $('#Tanggal_ProductSample').val(),
        UserName: $('#UserName').val(),
        MainBudidayaId: $('#MainBudidayaId').val(),
        NamaMainBudidaya: budidaya
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
                            hdr_ProductSampleId = $('#HDR_ProductSampleId').val();
                            $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                                disableHeader();
                                $('#btnDeleteHeader').removeClass('disabledbutton');
                                $('#btnPrintHeader').removeClass('disabledbutton');
                                $('#btnDeleteHeader').attr('disabled', false);
                                $('#btnPrintHeader').attr('disabled', false);
                                $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                                $('#grdDetail').removeClass('disabledbutton');
                                $("#grdDetail").data("kendoGrid").dataSource.read({
                                    id: $('#HDR_ProductSampleId').val()
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_ProductSample]" +
                " where HDR_ProductSampleId='" + $("#HDR_ProductSampleId").val() + "'",
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[HDR_UjiMutu]" +
                " where patindex('%" + $("#HDR_ProductSampleId").val() + "%',ListProductSampleId)>0",
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
        ModelToView(_NomorInputView);
        hdr_ProductSampleId = _NomorInputView.HDR_ProductSampleId;
        mainBudidayaId = _NomorInputView.MainBudidayaId;
        cekData(_NomorInputView.NomorInputView);
        $('#No_ProductSample').focus();
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
        parameters: { HDR_ProductSampleId: $('#HDR_ProductSampleId').val() }
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


// Detail

function grdOnChange(e) {
    $('#NamaMerk').text('ADUH');
}

function grdOnEdit(e) {
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.DTL_ProductSampleId = model.uid;
        model.HDR_ProductSampleId = hdr_ProductSampleId;
    }
    dtl_ProductSampleId = model.DTL_ProductSampleId;
    mainBudidayaId = $('#MainBudidayaId').val();
    gridStatus = 'sudah-diapa-apain';
    $('#jmlGross').text(kendo.toString(rekapGross(), 'n2'));
    $('#jmlTare').text(kendo.toString(rekapTare(), 'n2'));
    $('#jmlQtySacks').text(kendo.toString(rekapQtySacks(), 'n2'));
    $('#jmlChop').text(kendo.toString(rekapChop(), 'n2'));
}

function rekapGross() {

    var grid = $('#grdDetail').data('kendoGrid');
    var newValue = 0;
    for (var i = 0; i < grid.dataItems().length; i++) {
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

function rekapChop() {
    var grid = $('#grdDetail').data('kendoGrid');
    var newValue = 0;
    for (var i = 0; i < grid.dataItems().length; i++) {
        newValue = grid.dataItems().length;
    }
    return newValue;
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
    var dataItem = typeof (grid.dataSource.get(dtl_ProductSampleId)) == "undefined" ? grid.dataSource.getByUid(dtl_ProductSampleId) : grid.dataSource.get(dtl_ProductSampleId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_ProductSampleId)) == "undefined" ? grid.dataSource.getByUid(dtl_ProductSampleId) : grid.dataSource.get(dtl_ProductSampleId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_ProductSampleId)) == "undefined" ? grid.dataSource.getByUid(dtl_ProductSampleId) : grid.dataSource.get(dtl_ProductSampleId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_ProductSampleId)) == "undefined" ? grid.dataSource.getByUid(dtl_ProductSampleId) : grid.dataSource.get(dtl_ProductSampleId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.SatuanId = ddlItem.SatuanId;
}

function aucNamaSatuanOnDataBound(e) {

}

//autocomplete Grade
function filterGrade(e) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_ProductSampleId)) == "undefined" ? grid.dataSource.getByUid(dtl_ProductSampleId) : grid.dataSource.get(dtl_ProductSampleId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_ProductSampleId)) == "undefined" ? grid.dataSource.getByUid(dtl_ProductSampleId) : grid.dataSource.get(dtl_ProductSampleId);
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
    var dataItem = typeof (grid.dataSource.get(dtl_ProductSampleId)) == "undefined" ? grid.dataSource.getByUid(dtl_ProductSampleId) : grid.dataSource.get(dtl_ProductSampleId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.BrokerId = ddlItem.BrokerId;
}

function aucNamaBrokerOnDataBound(e) {

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
    var dataItem = typeof (grid.dataSource.get(dtl_ProductSampleId)) == "undefined" ? grid.dataSource.getByUid(dtl_ProductSampleId) : grid.dataSource.get(dtl_ProductSampleId);
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
        listId.push("'" + data[i].DTL_ProductSampleId.toString() + "'");
    }
    var strId = listId.toString();

    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_UjiMutu]" +
                " where DTL_ProductSampleId in (" + strId + ")",
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

function PeriksaConstraintDetail(dataItem) {
    // periksa apakah punya data di detail dan punya data di invoice
    // kalo ya, batalkan hapus header
    //var id = row.find("td:first").html()
    var id = dataItem.DTL_ProductSampleId;
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_UjiMutu]" +
                " where DTL_ProductSampleId='" + id + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}