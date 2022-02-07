var alokasiUrdalId, err, berangkat, berangkatdasar;
var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var detailBaru = false;
var userName;
var menuId = '494A31E7-E417-4FF1-8183-FABE9358AE99';
var model;
var headerBaru, detailBaru;
var gridStatus;
var reg, golongan, jenisWilayah, statuskmdanmenginap, jumlahhari, nomoralokasi, hariAwal, hariAkhir, dapatPenginapan;


$('.k-window.titlebar').css('style', 'display: none');
$(".k-button-icontext").addClass("k-state-disabled").removeClass("k-grid-add");

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
                    berangkatdasar = $('#TanggalAwal').data('kendoDatePicker').value();
                    berangkat = new Date(berangkatdasar),
                        month = '' + (berangkat.getMonth() + 1),
                        day = '' + berangkat.getDate(),
                        year = berangkat.getFullYear();
                    if (month.length < 2) month = '0' + month;
                    if (day.length < 2) day = '0' + day;
                    berangkat= [year, month, day].join('-');

                    $('#grdDetail').data('kendoGrid').dataSource.read();
                    getDataFrom().done(function (data) {
                        if (data >= 0) {
                            InsertGridFrom().done(function (data) {
                                $("#grdDetail").data("kendoGrid").dataSource.read({ id: $('#AlokasiUrdalId').val() });
                            }).fail(function (x) {
                                alert('Error! Hubungi Bagian TI');
                            });
                        }
                        else {
                            var grid = $("#grdDetail").data("kendoGrid");
                            grid.dataSource.read({
                                id: $('#AlokasiUrdalId').val()
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
                } else {
                    // not valid
                }
            }
        });

$(function () {
    var bpd = $.connection.bPDHub;
    $.connection.hub.start().done(function () {
    });

    bpd.client.klikButtonProses = function () {
        $('#btnSubmitHeader').click();
    }
});

function getDataFrom() {
    // Hitung Jumlah Record di Detail
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[AlokasiUrdal] as A" +
                " join [SPDK_KanpusEF].[dbo].[HDR_PermintaanPerjalananDinasKanpus] as B on A.NomorPermintaan=B.NomorPermintaan" +
                " where A.NomorPermintaan=B.NomorPermintaan and B.Berangkat ='"+berangkat+"'",
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
            strClassView: "Ptpn8.PerjalananDinas.Models.AlokasiUrdal",
            scriptSQL: "INSERT INTO [SPDK_KanpusEF].[dbo].[AlokasiUrdal] (AlokasiUrdalId,NomorPermintaan,Berangkat,Kembali,JenisTujuan,Tujuan,KendaraanDinas,NamaDriver,NopolKendaraan,NomorAlokasi,TanggalAlokasi,NamaPimpinan,Anggota, TanggalPermintaan,StatusPerjalananDinas,JumlahHari,Keterangan, UangBensin, DapatBiayaPenginapan, CatatanKhusus)" +
                       " SELECT distinct B.HDR_PermintaanPerjalananDinasKanpusId as AlokasiUrdalId, B.NomorPermintaan, B.Berangkat, B.Kembali, B.JenisTujuan,(case when (B.Tujuan != '') then B.Tujuan else B.TujuanDalamWilayah end) as Tujuan,B.KendaraanDinas,'','','',B.TanggalPermintaan, " +
                       " ( " +
                       " SELECT top 1 AAA.Nama FROM [SPDK_KanpusEF].[dbo].[DTL_PermintaanPerjalananDinasKanpus] AS AAA " +
                       " join [SPDK_KanpusEF].[dbo].[HDR_PermintaanPerjalananDinasKanpus] AS BBB " +
                       " ON AAA.HDR_PermintaanPerjalananDinasKanpusId=BBB.HDR_PermintaanPerjalananDinasKanpusId " +
                       " wHERE BBB.NomorPermintaan=B.NomorPermintaan AND BBB.Berangkat=B.Berangkat AND BBB.Kembali=B.Kembali AND BBB.JenisTujuan=B.JenisTujuan AND BBB.Tujuan=B.Tujuan  " +
                       " AND BBB.KendaraanDinas=B.KendaraanDinas AND BBB.TanggalPermintaan=B.TanggalPermintaan " +
                       " order by AAA.Jabatan, AAA.Golongan desc) as NamaPimpinan, " +
                       " ( " +
                       " SELECT count(*) as JumlahAnggota FROM [SPDK_KanpusEF].[dbo].[DTL_PermintaanPerjalananDinasKanpus] AS AAA " +
                       " join [SPDK_KanpusEF].[dbo].[HDR_PermintaanPerjalananDinasKanpus] AS BBB " +
                       " ON AAA.HDR_PermintaanPerjalananDinasKanpusId=BBB.HDR_PermintaanPerjalananDinasKanpusId " +
                       " WHERE BBB.NomorPermintaan=B.NomorPermintaan AND BBB.Berangkat=B.Berangkat AND BBB.Kembali=B.Kembali AND BBB.JenisTujuan=B.JenisTujuan AND BBB.Tujuan=B.Tujuan " +
                       " AND BBB.KendaraanDinas=B.KendaraanDinas AND BBB.TanggalPermintaan=B.TanggalPermintaan " +
                       " )-1 as Anggota, B.TanggalPermintaan, B.StatusKMdanMenginap, B.JumlahHari, B.Keterangan, 0, B.DapatBiayaPenginapan, B.CatatanKhusus " +
                       " from [SPDK_KanpusEF].[dbo].[DTL_PermintaanPerjalananDinasKanpus] as A join [SPDK_KanpusEF].[dbo].[HDR_PermintaanPerjalananDinasKanpus] as B on A.HDR_PermintaanPerjalananDinasKanpusId=B.HDR_PermintaanPerjalananDinasKanpusId where B.Berangkat ='" + $("#TanggalAwal").val() + "' and VerKaur=1 and A.JumlahBPD <> 0 and B.NomorPermintaan not in(select nomorpermintaan from [SPDK_KanpusEF].[dbo].[AlokasiUrdal]) and B.HDR_PermintaanPerjalananDinasKanpusId not in (select AlokasiUrdalId from [SPDK_KanpusEF].[dbo].[AlokasiUrdal])",
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}


$(window).on("beforeunload", function () {
    return "Please don't leave me!";
});

$(window).on("unload", function () {
    hapusPenggunaPortalYangAktif();
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

function disableHeader() {
    $("._key").find('span').addClass('disabledbutton');
    $("._key").addClass('disabledbutton');
    $("._nonkey").find('span').addClass('disabledbutton');
    $("._nonkey").addClass('disabledbutton');
}

function enableHeader() {
    $("._key").find('span').removeClass('disabledbutton');
    $("._key").removeClass('disabledbutton');
    $("._nonkey").find('span').removeClass('disabledbutton');
    $("._nonkey").removeClass('disabledbutton');
    $('#btnSubmitHeader').removeClass('disabledbutton');
}


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


/* Grid Buyer */

function grdOnChange(e) {

}

function grdOnEdit(e) {
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.AlokasiUrdalId = model.uid;
    }
    alokasiUrdalId = model.AlokasiUrdalId;
    jenisWilayah = model.JenisTujuan;
    statuskmdanmenginap = model.StatusPerjalananDinas;
    nomoralokasi = model.NomorAlokasi;
    dapatPenginapan = model.DapatBiayaPenginapan;
    jumlahhari = model.JumlahHari;

    $(e.container).find("input[name='NomorPermintaan']").prop('disabled', true);
    $(e.container).find("input[name='TanggalPermintaan']").prop('disabled', true);
    $(e.container).find("input[name='Berangkat']").prop('disabled', true);
    $(e.container).find("input[name='Kembali']").prop('disabled', true);
    $(e.container).find("input[name='JenisTujuan']").prop('disabled', true);
    $(e.container).find("input[name='Tujuan']").prop('disabled', true);
    $(e.container).find("input[name='KendaraanDinas']").prop('disabled', true);
    $(e.container).find("input[name='NamaPimpinan']").prop('disabled', true);
    $(e.container).find("input[name='Anggota']").prop('disabled', true);
    
    gridStatus = 'sudah-diapa-apain';
}

function grdOnDataBinding(e) {
}

function sendData() {
    var grid = $("#grdDetail").data("kendoGrid"),
        parameterMap = grid.dataSource.transport.parameterMap;

    //get the new and the updated records
    var currentData = grid.dataSource.data();
    var updatedRecords = [];
    var newRecords = [];
    var spupdated = [];
    var spnew = [];

    for (var i = 0; i < currentData.length; i++) {
        if (currentData[i].isNew()) {
            //this record is new
            newRecords.push(JSON.stringify(currentData[i]));
        } else if (currentData[i].dirty) {
            updatedRecords.push(JSON.stringify(currentData[i]));
            spupdated.push("EXEC InsertPengemudi '" + alokasiUrdalId + "','" + reg + "','" + jenisWilayah + "','" + statuskmdanmenginap + "'," + jumlahhari + ",'" + nomoralokasi + "',"+dapatPenginapan);
            spupdated.push("Update [SPDK_KanpusEF].[dbo].[DTL_PermintaanPerjalananDinasKanpus] set UangLain=C.UangBensin from [SPDK_KanpusEF].[dbo].[DTL_PermintaanPerjalananDinasKanpus] as A join [SPDK_KanpusEF].[dbo].[HDR_PermintaanPerjalananDinasKanpus] as B on A.HDR_PermintaanPerjalananDinasKanpusId=B.HDR_PermintaanPerjalananDinasKanpusId join [SPDK_KanpusEF].[dbo].[AlokasiUrdal] as C on B.HDR_PermintaanPerjalananDinasKanpusId=C.AlokasiUrdalId Where A.Nama=C.NamaPimpinan and B.NomorPermintaan=C.NomorPermintaan");
            spupdated.push("Update [SPDK_KanpusEF].[dbo].[DTL_PermintaanPerjalananDinasKanpus] set JumlahBpd=UangSaku+UangMakan+UangCucian+UangTransport+UangLain+UangPenginapan");
        }
    }

    //this records are deleted
    var deletedRecords = [];
    var spdeleted = [];
    for (var i = 0; i < grid.dataSource._destroyed.length; i++) {
        deletedRecords.push(JSON.stringify(grid.dataSource._destroyed[i]));
        spdeleted.push("Delete [SPDK_KanpusEF].[dbo].[AlokasiPengemudi] where AlokasiPengemudiId='" + alokasiUrdalId + "'");
    }

    var data = {};
    $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }), parameterMap({ new: newRecords }), parameterMap({ mnid: menuId }),parameterMap({ spupdated: spupdated }),parameterMap({ spdeleted: spdeleted }));
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

function filterdetailRead(e) {
    return {id : berangkat,
        _menuId: menuId
    };
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

function filterGridDestroy(e) {
    return {
        _model: JSON.stringify(e),
        _menuId: menuId
    };
}

function btnPrintHeaderOnClick(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");    

    var data = grid.dataItem(row);
    var datalokasiurdalid = data.AlokasiUrdalId;

    var viewer = $("#reportViewer").data("telerik_ReportViewer");
    viewer.reportSource({
        report: viewer.reportSource().report,
        parameters: { AlokasiUrdalId: datalokasiurdalid }
    });
    viewer.refreshReport();
    prt.open().center();
}

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

function filterPengemudi(e) {
    return {
        strClassView: 'Ptpn8.UploadDataHarian.Models.ref_dik',
        scriptSQL: "select distinct NAMA, REG, GOL from [SPDK_KanpusEF].[dbo].[Ref_Dik] where NAMA_JAB like '%pengemudi%' and KD_KBN = 00 and MBT= 'K'",
        _menuId: menuId
    }
}

function aucPengemudiOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(alokasiUrdalId)) == "undefined" ? grid.dataSource.getByUid(alokasiUrdalId) : grid.dataSource.get(alokasiUrdalId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.NamaDriver = ddlItem.NAMA;
    reg = ddlItem.REG;
 
}

function aucPengemudiOnDataBound(e) {

}

function ddlAlokasiKendaraanOnChange(e) {
    var alokasiKendaraan = $('#AlokasiKendaraan').val();
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(alokasiUrdalId)) == "undefined" ? grid.dataSource.getByUid(alokasiUrdalId) : grid.dataSource.get(alokasiUrdalId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
}