    var wndLeaveGrid,wnd,wndDetail,prt,err,del;
    var dataFaktur;
    var hdr_SPAId, mainBudidayaId, subBudidayaId, dtl_SPAId;
    var _NomorInputView;
    var model;
    var headerBaru, detailBaru;
    var rowNumber = 0;
	var menuId='DFA34EF8-DAE1-4810-ADA8-9D95E5ECF1EB';

function resetRowNumber(e) {
    rowNumber = 0;
    var jmlHarga = 0;
    var jmlGross = 0;
    var grid = $('#grdDetail').data('kendoGrid');
    var datasource = grid.dataSource.data();
    for (var i = 0; i < datasource.length; i++) {
        var model = datasource[i];
        model.JumlahHarga = hitungJumlah(model.DTL_SPAId);
        $('#jml' + model.DTL_SPAId).text(kendo.toString(model.JumlahHarga, 'n2'));
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
					if (input.attr("name") == "Chop" && input.val() == "") {
						$('#errMsg').text('No Chop Harus Diisi!');
                        openErrWindow();
						return false;
					} else if (input.attr("name") == "Chop" && input.val() == "") {
						$('#errMsg').text('No Chop Harus Diisi!');
                        openErrWindow();
						return false;
					} else if (input.attr("name") == "Chop" && input.val() != "") {
                        input.attr("data-chopvalidation-msg", "Chop Sudah Ada!")
                        cekNoChop(input).done(function (r) {
                            if (r == 0) {
                                return true;
                            } else {
                                for (var i = 0; i < r.length; i++) {
                                    if (r[i][0] != dtl_SPAId) {
                                        $('#errMsg').text('No Chop Sudah Digunakan!');
                                        openErrWindow();
                                        var grid = $('#grdDetail').data('kendoGrid');
                                        var dataItem = typeof (grid.dataSource.get(dtl_SPAId)) == "undefined" ? grid.dataSource.getByUid(dtl_SPAId) : grid.dataSource.get(dtl_SPAId);
                                        dataItem.Chop = '';
                                        return false;
                                    }
                                }
                                return true;
                            }
                        })
                        .fail(function (x) {
                            return false;
                        });
                    } else if(input.attr("name") == "NamaBuyer" && input.val() != "") {
						cekNamaBuyer(input.val()).done(function(data){
							if(data.length>0)
							{
								var grid = $('#grdDetail').data('kendoGrid');
                                var dataItem = typeof (grid.dataSource.get(dtl_SPAId)) == "undefined" ? grid.dataSource.getByUid(dtl_SPAId) : grid.dataSource.get(dtl_SPAId);
                                dataItem.BuyerId = data[0][0];
							}
						});
					} else if(input.attr("name") == "NamaSubBudidaya" && input.val() != "") {
						cekNamaSubBudidaya(input.val()).done(function(data){
							if(data.length>0)
							{
								var grid = $('#grdDetail').data('kendoGrid');
                                var dataItem = typeof (grid.dataSource.get(dtl_SPAId)) == "undefined" ? grid.dataSource.getByUid(dtl_SPAId) : grid.dataSource.get(dtl_SPAId);
                                dataItem.SubBudidayaId = data[0][0];
								subBudidayaId=data[0][0];
							}
						});
					} else if(input.attr("name") == "NamaMerk" && input.val() != "") {
						cekNamaMerk(input.val()).done(function(data){
							if(data.length>0)
							{
								var grid = $('#grdDetail').data('kendoGrid');
                                var dataItem = typeof (grid.dataSource.get(dtl_SPAId)) == "undefined" ? grid.dataSource.getByUid(dtl_SPAId) : grid.dataSource.get(dtl_SPAId);
                                dataItem.MerkId = data[0][0];
							}
						});
					} else if(input.attr("name") == "NamaGrade" && input.val() != "") {
						cekNamaGrade(input.val()).done(function(data){
							if(data.length>0)
							{
								var grid = $('#grdDetail').data('kendoGrid');
                                var dataItem = typeof (grid.dataSource.get(dtl_SPAId)) == "undefined" ? grid.dataSource.getByUid(dtl_SPAId) : grid.dataSource.get(dtl_SPAId);
                                dataItem.GradeId = data[0][0];
								if (dataItem.QtySacks == 0) {
									dataItem.QtySacks = data[0][1];
									dataItem.Gross = (data[0][1] * data[0][2]) + data[0][3];
									dataItem.Tare = data[0][3];
									$('#QtySacks').val(data[0][1]);
								}
							}
						});
					} else if(input.attr("name") == "NamaSatuan" && input.val() != "") {
						cekNamaSatuan(input.val()).done(function(data){
							if(data.length>0)
							{
								var grid = $('#grdDetail').data('kendoGrid');
                                var dataItem = typeof (grid.dataSource.get(dtl_SPAId)) == "undefined" ? grid.dataSource.getByUid(dtl_SPAId) : grid.dataSource.get(dtl_SPAId);
                                dataItem.SatuanId = data[0][0];
							}
						});
					} else if (input.attr("name") == "QtySacks" && input.val() != "") {
                        ambilStandarGrade().done(function (r) {
                            if (r == 0) {
                                return true;
                            } else {
                                var grid = $('#grdDetail').data('kendoGrid');
                                var dataItem = typeof (grid.dataSource.get(dtl_SPAId)) == "undefined" ? grid.dataSource.getByUid(dtl_SPAId) : grid.dataSource.get(dtl_SPAId);
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

	
	function cekNamaBuyer(namaBuyer)	
	{
		var url = '/Input/ExecSQL';
        return $.ajax({
            url: url,
            data: {
                scriptSQL: "select BuyerId from [ReferensiEF].[dbo].[Buyer]" +
                    " where lower(Nama)='" + namaBuyer.toLowerCase() + "'",
                _menuId: menuId
            },
            type: 'GET',
            datatype: 'json'
        });
	}
	function cekNamaSubBudidaya(namaSubBudidaya)	
	{
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
	
	function cekNamaMerk(namaMerk)	
	{
		var url = '/Input/ExecSQL';
        return $.ajax({
            url: url,
            data: {
                scriptSQL: "select KebunBudidayaId from [ReferensiEF].[dbo].[KebunBudidaya]" +
                    " where SubBudidayaId='"+subBudidayaId+"' and lower(Merk)='" + namaMerk.toLowerCase() + "'",
                _menuId: menuId
            },
            type: 'GET',
            datatype: 'json'
        });
	}

	function cekNamaGrade(namaGrade)	
	{
		var url = '/Input/ExecSQL';
        return $.ajax({
            url: url,
            data: {
                scriptSQL: "select GradeId,JumlahSatuan,Standar_BeratPerSatuan,Tara from [ReferensiEF].[dbo].[Grade]" +
                    " where SubBudidayaId='"+subBudidayaId+"' and lower(Nama)='" + namaGrade.toLowerCase() + "'",
                _menuId: menuId
            },
            type: 'GET',
            datatype: 'json'
        });
	}

	function cekNamaSatuan(namaSatuan)	
	{
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

    function ambilStandarGrade() {
        var grid = $('#grdDetail').data('kendoGrid');
        var dataItem = typeof (grid.dataSource.get(dtl_SPAId)) == "undefined" ? grid.dataSource.getByUid(dtl_SPAId) : grid.dataSource.get(dtl_SPAId);
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
	
	
    angular.module("__header", ["kendo.directives"])
        .controller("MyCtrl", function ($scope) {
            $scope.validate = function (event) {
                event.preventDefault();
                if ($scope.validator.validate()) {
                    simpanHeader().done(function (data) {
                        if (data) {
                            hdr_SPAId = $('#HDR_SPAId').val();
                            $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function(){
								disableHeader();
								$('#btnDeleteHeader').removeClass('disabledbutton');
								$('#btnPrintHeader').removeClass('disabledbutton');
								$('#btnDeleteHeader').attr('disabled', false);
								$('#btnPrintHeader').attr('disabled', false);
								$(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
								$('#grdDetail').removeClass('disabledbutton');
								$("#grdDetail").data("kendoGrid").dataSource.read({
									id: $('#HDR_SPAId').val()
								});
								GetDataFrom();
							});
                        }
                        else {
                            openErrWindow();
                            $('#grdDetail').addClass('disabledbutton');
                            gridDestroy();
                        }

                    }).fail(function (x) {
                        alert("Error! Hubungi Bagian TI");
                    });  //edited
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

    function GetDataFrom() {
        // Hitung Jumlah Record di Detail
        getDataFrom().done(function (data) {
            if (data == 0) {
                //InsertGridFrom();
            }
            else {
            }
        }).fail(function (x) {
            alert('Error! Hubungi Bagian TI');
        });

    }

    function getDataFrom() {
        var url = '/Input/ExecSQL';
        return $.ajax({
            url: url,
            data: {
                scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_SPA]" +
                    " where HDR_SPAId='" + $("#HDR_SPAId").val() + "'",
                _menuId: menuId
            },
            type: 'GET',
            datatype: 'json'
        });
    }

    function ModelToView(_NomorInputView)
    {
        $('#HDR_SPAId').val(_NomorInputView.HDR_SPAId);
        $('#TahunBuku').val(_NomorInputView.TahunBuku);
        $('#NomorInput').val(_NomorInputView.NomorInput);
        $('#NomorInputView').val(_NomorInputView.NomorInputView);
        $('#No_SPA').val(_NomorInputView.No_SPA);
        $('#Tanggal_SPA').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.Tanggal_SPA.substr(6))));
        $('#NamaPengangkut').val(_NomorInputView.NamaPengangkut);
        $('#AlamatPengangkut').val(_NomorInputView.AlamatPengangkut);
        $('#KotaPengangkut').val(_NomorInputView.KotaPengangkut);
        $('#PropinsiPengangkut').val(_NomorInputView.PropinsiPengangkut);
        $('#AlamatKirim').val(_NomorInputView.AlamatKirim);
        $('#KotaKirim').val(_NomorInputView.KotaKirim);
        $('#PropinsiKirim').val(_NomorInputView.PropinsiKirim);
        $('#ContactPerson').val(_NomorInputView.ContactPerson);
        $('#VerKaur').val(_NomorInputView.VerKaur);
        $('#VerKabag').val(_NomorInputView.VerKabag);
        $('#TglVerKaur').val(new Date(parseInt(_NomorInputView.TglVerKaur.substr(6))));
        $('#TglVerKabag').val(new Date(parseInt(_NomorInputView.TglVerKabag.substr(6))));
        $('#UserNameKaur').val(_NomorInputView.UserNameKaur);
        $('#UserNameKabag').val(_NomorInputView.UserNameKabag);
        $('#UserName').val(_NomorInputView.UserName);
        $('#Pejabat').val(_NomorInputView.Pejabat);
        $('#No_SuratTugas').val(_NomorInputView.No_SuratTugas);
		$('#MainBudidayaId').val(_NomorInputView.MainBudidayaId);
		$('#NamaMainBudidaya').val(_NomorInputView.NamaMainBudidaya);
		$('#SudahKirimKeBGR').val(_NomorInputView.SudahKirimKeBGR);
		$('#TglBarangDitunggu').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.TglBarangDitunggu.substr(6))));
    }

    function ViewToModel()
    {
        var listFakturId = $('#ListFakturId').val();
        if (listFakturId == null) listFakturId = "";
        var listNo_Faktur = $('#ListNo_Faktur').val();
        if (listNo_Faktur == null) listNo_Faktur = "";
        var mdl = {
            HDR_SPAId: $('#HDR_SPAId').val(),
            TahunBuku: $('#TahunBuku').val(),
            NomorInput: $('#NomorInput').val(),
            NomorInputView: Array(5 - String($('#NomorInput').val()).length + 1).join('0') + $('#NomorInput').val() + ' - ' + $('#No_SPA').val().toUpperCase(),
            No_SPA: $('#No_SPA').val().toUpperCase(),
            Tanggal_SPA: $('#Tanggal_SPA').val(),
            NamaPengangkut: $('#NamaPengangkut').val().toUpperCase(),
            AlamatPengangkut: $('#AlamatPengangkut').val(),
            KotaPengangkut: $('#KotaPengangkut').val(),
            PropinsiPengangkut: $('#PropinsiPengangkut').val(),
            MainBudidayaId: $('#MainBudidayaId').val(),
            NamaMainBudidaya: $('#NamaMainBudidaya').val().toUpperCase(),
            AlamatKirim: $('#AlamatKirim').val(),
            KotaKirim: $('#KotaKirim').val(),
            PropinsiKirim: $('#PropinsiKirim').val(),
            ContactPerson: $('#ContactPerson').val(),
            UserName: $('#UserName').val(),
            Pejabat: $('#Pejabat').val().toUpperCase(),
            VerKaur: $('#VerKaur').val(),
            VerKabag: $('#VerKabag').val(),
            TglVerKaur: $('#TglVerKaur').val(),
            TglVerKabag: $('#TglVerKabag').val(),
            UserNameKaur: $('#UserNameKaur').val(),
            UserNameKabag: $('#UserNameKabag').val(),
            No_SuratTugas: $('#No_SuratTugas').val(),
            SudahKirimKeBGR: $('#SudahKirimKeBGR').val(),
            TglBarangDitunggu: $('#TglBarangDitunggu').val()
        };
        return mdl;
    }

    function hapusHeader() {
        wnd.close();
        PeriksaConstraintHeader().done(function (data) {
            if (data == 0) {
                ConfirmedHapusHeader().done(function (data) {
                    if (data) {
                        hdr_SPAId = $('#HDR_SPAId').val();
                        $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function(){
							disableHeader();
							$('#btnDeleteHeader').removeClass('disabledbutton');
							$('#btnPrintHeader').removeClass('disabledbutton');
							$('#btnDeleteHeader').attr('disabled', false);
							$('#btnPrintHeader').attr('disabled', false);
							$(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
							$('#grdDetail').removeClass('disabledbutton');
							$("#grdDetail").data("kendoGrid").dataSource.read({
								id: $('#HDR_SPAId').val()
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
                scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_SPA]" +
                    " where HDR_SPAId='" + $("#HDR_SPAId").val() + "'",
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
			if(typeof(e.item)!='undefined') 
				_NomorInputView = this.dataItem(e.item);
			else
				_NomorInputView = e.sender.dataItem();
            cekData(_NomorInputView.NomorInputView);
            ModelToView(_NomorInputView);
            hdr_SPAId = _NomorInputView.HDR_SPAId;
            mainBudidayaId = _NomorInputView.MainBudidayaId;
			$('#No_SPA').focus();
        }
    }

    function NomorInputViewOnDataBound(e) {
        var items = this.dataItems();
        var adaDataBaru = false;
        for(var i=0;i<items.length;i++)
        {
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

    function btnPrintHeaderOnClick() {
        var viewer = $("#reportViewer").data("telerik_ReportViewer");
        viewer.reportSource({
            report: viewer.reportSource().report,
            parameters: { HDR_SPAId: $('#HDR_SPAId').val() }
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

    function NamaPengangkutOnSelect(e) {
        pengangkut = this.dataItem(e.item);
        $('#NamaPengangkut').val(pengangkut.NamaPengangkut);
        $('#AlamatPengangkut').val(pengangkut.AlamatPengangkut);
        $('#KotaPengangkut').val(pengangkut.KotaPengangkut);
        $('#PropinsiPengangkut').val(pengangkut.PropinsiPengangkut);
    }

    function NamaPengangkutOnDataBound(e) {
    }

    function filterNomorInput(e)
    {
        var mdl = JSON.stringify(ViewToModel());
        return { _model: mdl ,
                _menuId: menuId};
    }

    function filterBuyer() {
        return {
            value: $("#NamaBuyer").val(),
            mainBudidayaId: $("#MainBudidayaId").val()
        };
    }

    function filterBudidaya() {
        return {
            key: "NamaMainBudidaya",
            value: $("#NamaMainBudidaya").val()
        };
    }

    function BudidayaOnSelect(e) {
        if (gridStatus == 'sudah-nyoba-disimpan-tapi-model-masih-salah' ||
            gridStatus == 'sudah-diapa-apain') {
            openWindowLeaveGrid(e);
        }
        else {
            _NomorInputView = this.dataItem(e.item);
            mainBudidayaId = _NomorInputView.MainBudidayaId;
            $('#MainBudidayaId').val(_NomorInputView.MainBudidayaId);
        }
    }


    function filterPengangkut() {
        return {
            key: "NamaPengangkut",
            value: $("#NamaPengangkut").val(),
                _menuId: menuId };
    }
    function filterAlamatKirim() {
        return {
            key: "AlamatKirim",
            value: $("#AlamatKirim").val(),
                _menuId: menuId
        };
    }

    function filterKotaKirim() {
        return {
            key: "KotaKirim",
            value: $("#KotaKirim").val(),
                _menuId: menuId
        };
    }

    function filterPropinsiKirim() {
        return {
            key: "PropinsiKirim",
            value: $("#PropinsiKirim").val(),
                _menuId: menuId
        };
    }

    // Detail

    function grdOnChange(e) {
    }

    function grdOnEdit(e) {
        var model = e.model;
        this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
        if (model.isNew()) {
            model.DTL_SPAId = model.uid;
            model.HDR_SPAId = hdr_SPAId;
        }
        dtl_SPAId = model.DTL_SPAId;
        mainBudidayaId = $('#MainBudidayaId').val();
        gridStatus = 'sudah-diapa-apain';
		$('#jmlGross').text(kendo.toString(rekapGross(), 'n2'));
		$('#jmlTare').text(kendo.toString(rekapTare(), 'n2'));
		$('#jmlQtySacks').text(kendo.toString(rekapQtySacks(), 'n2'));
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
    var newValue = dataItem.Price * (dataItem.Gross-dataItem.Tare);
    return newValue;
}

    function grdOnDataBinding(e) {

    }

    function filterSubBudidaya(e) {
        return {
            mainBudidayaId: $('#MainBudidayaId').val(),
            value: $("#NamaSubBudidaya").val()
        };
    }

    aucNamaBuyerOnSelect

    function aucNamaBuyerOnSelect(e) {
        var ddlItem = this.dataItem(e.item);
        var grid = $('#grdDetail').data('kendoGrid');
        var dataItem = typeof(grid.dataSource.get(dtl_SPAId))=="undefined" ? grid.dataSource.getByUid(dtl_SPAId) : grid.dataSource.get(dtl_SPAId);
        var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
        var data = grid.dataItem(row);
        data.BuyerId = ddlItem.BuyerId;
    }

    function aucNamaBuyerOnDataBound(e) {

    }
    function aucNamaSubBudidayaOnSelect(e) {
        var ddlItem = this.dataItem(e.item);
        var grid = $('#grdDetail').data('kendoGrid');
        var dataItem = typeof(grid.dataSource.get(dtl_SPAId))=="undefined" ? grid.dataSource.getByUid(dtl_SPAId) : grid.dataSource.get(dtl_SPAId);
        var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
        var data = grid.dataItem(row);
        data.SubBudidayaId = ddlItem.SubBudidayaId;
        subBudidayaId = ddlItem.SubBudidayaId;
    }

    function aucNamaSubBudidayaOnDataBound(e) {

    }

    function filterMerk(e) {
        var grid = $('#grdDetail').data('kendoGrid');
        var dataItem = typeof (grid.dataSource.get(dtl_SPAId)) == "undefined" ? grid.dataSource.getByUid(dtl_SPAId) : grid.dataSource.get(dtl_SPAId);
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
        var dataItem = typeof(grid.dataSource.get(dtl_SPAId))=="undefined" ? grid.dataSource.getByUid(dtl_SPAId) : grid.dataSource.get(dtl_SPAId);
        var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
        var data = grid.dataItem(row);
        data.MerkId = ddlItem.KebunBudidayaId;
    }

    function aucNamaMerkOnDataBound(e) {

    }

    function filterKebun(e) {
        return {
            value: $("#NamaKebun").val()
        };
    }

    function filterSatuan(e) {
        return {
            value: $("#NamaSatuan").val()
        };
    }

    function aucNamaSatuanOnSelect(e) {
        var ddlItem = this.dataItem(e.item);
        var grid = $('#grdDetail').data('kendoGrid');
        var dataItem = typeof(grid.dataSource.get(dtl_SPAId))=="undefined" ? grid.dataSource.getByUid(dtl_SPAId) : grid.dataSource.get(dtl_SPAId);
        var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
        var data = grid.dataItem(row);
        data.SatuanId = ddlItem.SatuanId;
    }

    function aucNamaSatuanOnDataBound(e) {

    }

    function filterGridUpdate(e)
    {
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

    function filterGrade(e) {
        var grid = $('#grdDetail').data('kendoGrid');
        var dataItem = typeof (grid.dataSource.get(dtl_SPAId)) == "undefined" ? grid.dataSource.getByUid(dtl_SPAId) : grid.dataSource.get(dtl_SPAId);
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
        var dataItem = typeof(grid.dataSource.get(dtl_SPAId))=="undefined" ? grid.dataSource.getByUid(dtl_SPAId) : grid.dataSource.get(dtl_SPAId);
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

    function TahunBukuOnChange(e) {
        if (gridStatus == 'sudah-nyoba-disimpan-tapi-model-masih-salah' ||
            gridStatus == 'sudah-diapa-apain') {
            openWindowLeaveGrid(e);
        }
        else {

            $('#msgInputView').text('');
            tahunBukuOnChange(e).done(function (data) {
                if (data) {
                    $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function(){
						disableHeader();
						$(".k-button-icontext").addClass("k-state-disabled").removeClass("k-grid-add"); // edited
						gridDestroy();
					});
                }
                else {
                    alert('Error! Hubungi Bagian TI');
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
            data: { tahunBuku: $('#TahunBuku').val() ,
                _menuId: menuId}
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
        $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }), parameterMap({ new: newRecords }), parameterMap({mnid: menuId}));
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
        var dataItem = typeof (grid.dataSource.get(dtl_SPAId)) == "undefined" ? grid.dataSource.getByUid(dtl_SPAId) : grid.dataSource.get(dtl_SPAId);
        var items = grid.dataItems();
        var arr = [, ];
        for (var i = 0; i < items.length; i++) {
            if (items[i].Chop == input.val() && items[i].DTL_SPAId != dtl_SPAId
                && items[i].Chop.toLowerCase() != "na" && items[i].MerkId == dataItem.MerkId) {
                arr.push([items[i].DTL_SPAId, 1]);
                return $.ajax(arr);
            }
        }
        return $.ajax({
            url: '/Input/ExecSQL',
            data: {
                scriptSQL:
                    " select DTL_SPAId as Id, count(*) as Cnt from [SPDK_KanpusEF].[dbo].[DTL_SPA]" +
                    " where MerkId='" + dataItem.MerkId + "'" +
                    " and SubBudidayaId='" + dataItem.SubBudidayaId + "'" +
                    " and Chop='" + input.val() + "'  and lower(Chop)!='na' group by DTL_SPAId" ,
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
			var row=grid.tbody.find("tr:first");
			do {
				$('#grdDetail').data('kendoGrid').removeRow(row);
				row=grid.tbody.find("tr:first");
			}
			while (grid.tbody.contents().length>0)
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
