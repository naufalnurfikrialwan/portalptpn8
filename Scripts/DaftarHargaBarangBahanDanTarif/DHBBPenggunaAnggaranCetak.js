var evaluasiDBBId, err, bulan, tahun;
var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var detailBaru = false;
var userName;
var model;
var headerBaru, detailBaru;
var gridStatus, kebunId;
var menuId = '47E1488D-95B0-4D0F-A851-24336D3555E1';

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
	cekNamaBagian().done(function (data) {
		kebunId = data[0][0];
	}).fail(function (x) {
		alert('Error! Hubungi Bagian TI');
	});
});

angular.module("__header", ["kendo.directives"])
        .controller("MyCtrl", function ($scope) {
            $scope.validate = function (event) {
                event.preventDefault();
                if ($scope.validator.validate()) {
                    //bulan = $('#BulanTerima').data('kendoDropDownList').value();
                    //tahun = $('#TahunTerima').val();
                    var grd = $('#grdDetail').data('kendoGrid');
                    //grd.dataSource.read([]);
					cekNamaBagian().done(function (data) {
						kebunId = data[0][0];
						InsertGridFrom().done(function (dta) {
							grd.dataSource.data(dta);
							grd.refresh();
						}).fail(function (x) {
							alert('Error! Hubungi Bagian TI');
						});
					}).fail(function (x) {
						alert('Error! Hubungi Bagian TI');
					});

                 } else {
                    // not valid
                 }
              }
         });

//angular.module("__header", ["kendo.directives"])
//        .controller("MyCtrl", function ($scope) {
//            $scope.validate = function (event) {
//                event.preventDefault();
//                if ($scope.validator.validate()) {
//                    kodeBarang = $('#KodeBarang').val();
//                    var grd = $('#grdDetail').data('kendoGrid');
//                    grd.dataSource.read([]);
//                    InsertGridFrom().done(function (dta) {
//                        grd.dataSource.data(dta);
//                        grd.refresh();
//                    }).fail(function (x) {
//                        alert('Error! Hubungi Bagian TI');
//                    });

//                } else {
//                    // not valid
//                }
//            }
//        });



function getDataFrom() {
    // Hitung Jumlah Record di Detail
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "Select COUNT(*) as JmlRecord from SPDK_KanpusEF.dbo.CetakDHBB as A " +
                       "join SPDK_KanpusEF.dbo.HDR_TerimaDHBB as B " +
                       "on A.NomorPenerimaan=B.NomorPenerimaan " +
                       "where A.NomorPenerimaan=B.NomorPenerimaan ",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function InsertGridFrom() {
    var grd = $('#grdDetail').data('kendoGrid');
    var url = '/Input/GetDataFrom';
    //var bulan = $('#BulanTerima').val();
    //var tahun = $('#TahunTerima').val();

    return $.ajax({
        url: url,
        //contentType: 'application/json; charset=utf-8',
        data: {
            strClassView: "Ptpn8.DaftarHargaBarangBahanDanTarif.Models.CetakDHBB",
            scriptSQL: 
				"Select NEWID()  as EvaluasiDHBBId,B.NomorPenerimaan,A.NamaBarang, A.NamaSatuan, A.Spesifikasi, A.Merk, A.HargaDasar, (A.HargaDasar * (1 + (A.Overhead))) as Harga,B.KelompokBarang, A.SudahTerima, cast('" + kebunId + "' as uniqueidentifier) as KebunId " +
                       "from SPDK_KanpusEF.dbo.DTL_TerimaDHBB as A " +
                       "inner join SPDK_KanpusEF.dbo.HDR_TerimaDHBB as B on A.HDR_TerimaDHBBId = B.HDR_TerimaDHBBId " +
                       "where A.NamaBarang like '%" + $('#aucNamaBarang').val() + "%'",

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
        model.EvaluasiDHBBId= model.uid;
    }
    evaluasiDHBBId = model.EvaluasiDHBBId;

    $(e.container).find("input[name='NomorPenerimaan']").prop('disabled', true);
    $(e.container).find("input[name='NamaBarang']").prop('disabled', true);
    $(e.container).find("input[name='NamaSatuan']").prop('disabled', true);
    $(e.container).find("input[name='Spesifikasi']").prop('disabled', true);
    $(e.container).find("input[name='Merk']").prop('disabled', true);
    $(e.container).find("input[name='HargaDasar']").prop('disabled', true);
    $(e.container).find("input[name='Harga']").prop('disabled', true);
    $(e.container).find("input[name='KelompokBarang']").prop('disabled', true);

    gridStatus = 'sudah-diapa-apain';
}

function grdOnDataBinding(e) {
}

//function sendData() {
//	var grid = $("#grdDetail").data("kendoGrid");
//	var currentData = grid.dataSource.data();
//	var stFail = 0;

//	for (var i = 0; i < currentData.length; i++) {
//		if (currentData[i].isNew()) {
//			SimpanData('baru', currentData[i]).done(function (dta) {
//			}).fail(function () { stSfail = 1; alert("Ada kesalahan pada data ..."); });
//		} else if (currentData[i].dirty) {
//			SimpanData('edit', currentData[i]).done(function (dta) {
//			}).fail(function () { stSfail = 1; alert("Ada kesalahan pada data ..."); });
//		}
//	}

//	//this records are deleted
//	var deletedRecords = [];
//	for (var i = 0; i < grid.dataSource._destroyed.length; i++) {
//		SimpanData('hapus', grid.dataSource._destroyed[i]).done(function (dta) {
//		}).fail(function () { stSfail = 1; alert("Ada kesalahan pada data ..."); });
//	}

//	if (stFail == 0) {
//		alert("Data Sudah Tersimpan...");
//	}
//}

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
		}
	}

	//this records are deleted
	var deletedRecords = [];
	var spdeleted = [];
	for (var i = 0; i < grid.dataSource._destroyed.length; i++) {
		deletedRecords.push(JSON.stringify(grid.dataSource._destroyed[i]));
	}

	var data = {};
	$.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }), parameterMap({ new: newRecords }), parameterMap({ mnid: menuId }), parameterMap({ spupdated: spupdated }), parameterMap({ spdeleted: spdeleted }));
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
    return {
        _menuId: menuId
    };
}

function filterdetailCreate(e) {
    return { _menuId: menuId };
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


function btnPrintHeaderOnClick() {
    var viewer = $("#reportViewer").data("telerik_ReportViewer");


    var grid = $('#grdDetail').data('kendoGrid');
    var ds = grid.dataSource;
    var flt = '';//'"01505031","01501164","01501138"';
    var arrFlt = [];
    for (var i = 0; i < ds.data().length; i++) {
        if (ds.data()[i].SudahTerima == true) {
            arrFlt.push("'" + ds.data()[i].bulan + "'", tahun +"'");
        }
    }
    flt = arrFlt.join();

    viewer.reportSource({
        report: viewer.reportSource().report,
        parameters:
		{
			KebunId: kebunId
            }
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

function cekNamaBagian() {
	var url = '/Input/ExecSQL';
	return $.ajax({
		url: url,
		data: {
			scriptSQL: "select OrgId as KebunId from [ReferensiEF].[dbo].[Unit]" +
				" where orgId=(select posisilokasikerjaid from [Identity].[dbo].[AspNetUsers] where UserName ='" + usrName + "' )",
			_menuId: menuId
		},
		type: 'GET',
		datatype: 'json'
	});
}

//function filterKodeBarang() {
//    return {
//        //id: IdBudidaya,
//        value: $('#KodeBarang').val()
//    };
//}
//function aucKodeBarangOnSelect(e) {
//    //var kodeBarang = this.dataItem(e.item);
//    //$('#KodeBarang').val(kodeBarang.KodeBarang);
//}


//function rekapBiaya() {

//    var grid = $('#grdDetail').data('kendoGrid');
//    var newValue = 0;
//    for (var i = 0; i < grid.dataItems().length; i++) {
//        newValue += grid.dataItems()[i].Harga;
//    }
//    return newValue;
//}

//"select NEWID() as RekapDHBBId, A.TanggalBerlaku, A.NamaBarang,A.NamaSatuan,A.Spesifikasi, A.Merk, A.HargaDasar,sum(((A.HargaDasar * (1 + (A.Overhead))))) as Harga, A.BarangId, A.SudahTerima, B.NomorDaftarHargaBarangBahan " +
//        "from SPDK_KanpusEF.dbo.DTL_UpdateDHBB as A " +
//        "join SPDK_KanpusEF.dbo.HDR_UpdateDHBB as B " +
//        "on A.HDR_UpdateDHBBId = B.HDR_UpdateDHBBId " +
//        "Where A.TanggaBerlaku='" + $("#TanggalBerlaku").val() + "' " +
//        //--where substring(A.KodeBarang,1,"+lenkdbrg+")='"+kdbrg+"' 
//        "group by A.TanggalBerlaku,A.NamaBarang,A.NamaSatuan,A.Spesifikasi, A.Merk, A.BarangId, A.SudahTerima, A.HargaDasar, B.NomorDaftarHargaBarangBahan " +
//        "order by A.NamaBarang ",


//QUERY SEBELUMNYA
//"select NEWID() as RekapDHBBId, C.NamaBarang,A.KodeBarang,A.NamaSatuan,A.Spesifikasi, A.Merk, A.HargaDasar,sum(((A.HargaDasar * (1 + (A.Overhead))))) as Harga, A.BarangId, A.SudahTerima, B.NomorDaftarHargaBarangBahan " +
//"from SPDK_KanpusEF.dbo.DTL_UpdateDHBB as A " +
//"join SPDK_KanpusEF.dbo.HDR_UpdateDHBB as B " +
//"on A.HDR_UpdateDHBBId = B.HDR_UpdateDHBBId " +
//"join [ReferensiEF].dbo.Barang as C " +
//"on C.BarangId=A.BarangId " +
//"where substring(A.KodeBarang,1,"+lenkdbrg+")='"+kdbrg+"' " +
//"group by C.NamaBarang,A.KodeBarang,A.NamaSatuan,A.Spesifikasi, A.Merk, A.BarangId, A.SudahTerima, A.HargaDasar, B.NomorDaftarHargaBarangBahan " +
//"order by A.KodeBarang ",

//function SimpanData(status, dta) {

//	var url = '/Input/ExecSQL';

//	var EvaluasiDHBBId = evaluasidhbbid;
//	var NomorPenerimaan = dta.NomorPenerimaan;
//	var NamaBarang = dta.NamaBarang;
//	var NamaSatuan = dta.NamaSatuan;
//	var Spesifikasi = dta.Spesifikasi;
//	var Merk = dta.Merk;
//	var HargaDasar = dta.HargaDasar;
//	var Harga = dta.Harga;
//	var SudahTerima = dta.SudahTerima;
//	var KelompokBarang = dta.KelompokBarang;
//	var KebunId = dta.KebunId;
//	var NamaKebun = dta.NamaKebun;

//	if (status == 'baru') {
//		return $.ajax({
//			url: url,
//			data: {
//				scriptSQL: "exec dbo.EvaluasiDBB '" + EvaluasiDHBBId.toString() +
//					"','" + NomorPenerimaan.toString() + "','" + NamaBarang + "','" + NamaSatuan + "', '" + Spesifikasi + "', '" +
//					Merk + "','" + HargaDasar + "','" + Harga + "','" + SudahTerima + "','" + KelompokBarang + "','" + kebunId.toString() + "','" + NamaKebun +"'",
//				_menuId: menuId
//			},
//			type: 'POST',
//			datatype: 'json'
//		});`
//}