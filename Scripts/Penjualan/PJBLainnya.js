    var wndLeaveGrid,wnd,wndDetail,prt,err,del;
    var buyer;
    var hdr_PJBId, mainBudidayaId, subBudidayaId, dtl_PJBId;
    var _NomorInputView;
    var model;
    var headerBaru, detailBaru;
    var rowNumber = 0;
    var menuId = 'e83f22b2-4aef-42e4-83b9-f555754d1381';

    function resetRowNumber(e) {
        rowNumber = 0;
        var jmlHarga = 0;
        var jmlGross = 0;
        var grid = $('#grdDetail').data('kendoGrid');
        var datasource = grid.dataSource.data();

        var status = $("#No_PJB").data("kendoAutoComplete").value();
        for (var i = 0; i < datasource.length; i++) {
            var model = datasource[i];
                model.JumlahHarga = hitungJumlah(model.DTL_PJBId);
                $('#jml' + model.DTL_PJBId).text(kendo.toString(model.JumlahHarga, 'n2'));
            
            jmlHarga = jmlHarga + model.JumlahHarga;
            jmlGross = jmlGross + model.Netto;
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
	    var h2 = parseInt($(".hdr").css("height")) + parseInt($("._headerlainnya").css("height"));
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
					}
					else if (input.attr("name") == "TahunProduksi" && input.val() == "") {
					    $('#errMsg').text('Tahun Produksi Harus Diisi!');
					    openErrWindow();
					    return false;
					}
					 else if(input.attr("name") == "NamaSubBudidaya" && input.val() != "") {
						cekNamaSubBudidaya(input.val()).done(function(data){
							if(data.length>0)
							{
								var grid = $('#grdDetail').data('kendoGrid');
                                var dataItem = typeof (grid.dataSource.get(dtl_PJBId)) == "undefined" ? grid.dataSource.getByUid(dtl_PJBId) : grid.dataSource.get(dtl_PJBId);
                                dataItem.SubBudidayaId = data[0][0];
								subBudidayaId=data[0][0];
							}
						});
					} else if(input.attr("name") == "NamaMerk" && input.val() != "") {
						cekNamaMerk(input.val()).done(function(data){
							if(data.length>0)
							{
								var grid = $('#grdDetail').data('kendoGrid');
                                var dataItem = typeof (grid.dataSource.get(dtl_PJBId)) == "undefined" ? grid.dataSource.getByUid(dtl_PJBId) : grid.dataSource.get(dtl_PJBId);
                                dataItem.MerkId = data[0][0];
							}
						});
					} else if(input.attr("name") == "NamaGrade" && input.val() != "") {
						cekNamaGrade(input.val()).done(function(data){
							if(data.length>0)
							{
								var grid = $('#grdDetail').data('kendoGrid');
                                var dataItem = typeof (grid.dataSource.get(dtl_PJBId)) == "undefined" ? grid.dataSource.getByUid(dtl_PJBId) : grid.dataSource.get(dtl_PJBId);
                                dataItem.GradeId = data[0][0];
								
							}
						});
					} else if(input.attr("name") == "NamaSatuan" && input.val() != "") {
						cekNamaSatuan(input.val()).done(function(data){
							if(data.length>0)
							{
								var grid = $('#grdDetail').data('kendoGrid');
                                var dataItem = typeof (grid.dataSource.get(dtl_PJBId)) == "undefined" ? grid.dataSource.getByUid(dtl_PJBId) : grid.dataSource.get(dtl_PJBId);
                                dataItem.SatuanId = data[0][0];
							}
						});
					} else if (input.attr("name") == "QtySacks" && input.val() != "") {
                        ambilStandarGrade().done(function (r) {
                            if (r == 0) {
                                return true;
                            } else {
                                var grid = $('#grdDetail').data('kendoGrid');
                                var dataItem = typeof (grid.dataSource.get(dtl_PJBId)) == "undefined" ? grid.dataSource.getByUid(dtl_PJBId) : grid.dataSource.get(dtl_PJBId);
                                if (dataItem.QtySacks > 0) {
                                    dataItem.Netto = (dataItem.QtySacks * r[0][2]);
                                }
                                return true;
                            }
                        })
                        .fail(function (x) {
                            return false;
                        });
					}
					//else if (input.attr("name") == "JumlahHarga" && input.val() != "" && $("#No_PO").data("kendoAutoComplete").value().toLowerCase().indexOf("kpb") > -1) {

					//}
					else {
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
	
    angular.module("__header", ["kendo.directives"])
        .controller("MyCtrl", function ($scope) {
            $scope.validate = function (event) {
                event.preventDefault();
                if ($scope.validator.validate()) {
                    simpanHeader().done(function (data) {
                        if (data) {
                            hdr_PJBId = $('#HDR_PJBId').val();
                            $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function(){
								disableHeader();
								$('#btnDeleteHeader').removeClass('disabledbutton');
								$('#btnPrintHeader').removeClass('disabledbutton');
								$('#btnDeleteHeader').attr('disabled', false);
								$('#btnPrintHeader').attr('disabled', false);
								$(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
								$('#grdDetail').removeClass('disabledbutton');
								$("#grdDetail").data("kendoGrid").dataSource.read({
									id: $('#HDR_PJBId').val()
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

    function ModelToView(_NomorInputView)
    {
        $('#HDR_PJBId').val(_NomorInputView.HDR_PJBId);
        $('#TahunBuku').val(_NomorInputView.TahunBuku);
        $('#NomorInput').val(_NomorInputView.NomorInput);
        $('#NomorInputView').val(_NomorInputView.NomorInputView);
        $('#No_PJB').val(_NomorInputView.No_PJB);
        $('#Tanggal_PJB').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.Tanggal_PJB.substr(6))));        
        $('#BuyerId').val(_NomorInputView.BuyerId);
        $('#NamaBuyer').val(_NomorInputView.NamaBuyer);
        $('#Alamat').val(_NomorInputView.Alamat);
        $('#Kota').val(_NomorInputView.Kota);
        $('#Propinsi').val(_NomorInputView.Propinsi);
        $('#UserName').val(_NomorInputView.UserName);
        //$('#MainBudidayaId').val(_NomorInputView.MainBudidayaId);
        //$('#NamaMainBudidaya').val(_NomorInputView.NamaMainBudidaya);
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
        $('#KenaPpn').data('kendoDropDownList').value(_NomorInputView.KenaPpn ? "1" : "0");
        $('#KenaMaterai').data('kendoDropDownList').value(_NomorInputView.KenaMaterai ? "1" : "0");
        $('#QtyMaterai').val(_NomorInputView.QtyMaterai);
        $('#NilaiMaterai').val(_NomorInputView.NilaiMaterai);
        $('#Pengepakan').val(_NomorInputView.Pengepakan);
        $('#Pembayaran').val(_NomorInputView.Pembayaran);
        $('#Penyerahan').val(_NomorInputView.Penyerahan);
        $('#LamaHari').val(_NomorInputView.LamaHari);
        $('#Syarat').val(_NomorInputView.Syarat);
    }

    function ViewToModel()
    {
        var mdl = {
            HDR_PJBId: $('#HDR_PJBId').val(),
            TahunBuku: $('#TahunBuku').val(),
            NomorInput: $('#NomorInput').val(),
            NomorInputView: Array(5 - String($('#NomorInput').val()).length + 1).join('0') + $('#NomorInput').val() + ' - ' + $('#No_PJB').val().toUpperCase(),
            No_PJB: $('#No_PJB').val().toUpperCase(),
            Tanggal_PJB: $('#Tanggal_PJB').val(),
            BuyerId: $('#BuyerId').val(),
            NamaBuyer: $('#NamaBuyer').val().toUpperCase(),
            Alamat: $('#Alamat').val(),
            Kota: $('#Kota').val(),
            Propinsi: $('#Propinsi').val(),
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
            KenaPpn: $('#KenaPpn').data('kendoDropDownList').value() == "1" ? "true" : "false",
            KenaMaterai: $('#KenaMaterai').data('kendoDropDownList').value() == "1" ? "true" : "false",
            QtyMaterai: $('#QtyMaterai').val(),
            NilaiMaterai: $('#NilaiMaterai').val(),
            Pengepakan: $('#Pengepakan').val(),
            Pembayaran: $('#Pembayaran').val(),
            Penyerahan: $('#Penyerahan').val(),
            LamaHari: $('#LamaHari').val(),
            Syarat: $('#Syarat').val()
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
                                hdr_PJBId = $('#HDR_PJBId').val();
                                $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function(){
									disableHeader();
									$('#btnDeleteHeader').removeClass('disabledbutton');
									$('#btnPrintHeader').removeClass('disabledbutton');
									$('#btnDeleteHeader').attr('disabled', false);
									$('#btnPrintHeader').attr('disabled', false);
									$(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
									$('#grdDetail').removeClass('disabledbutton');
									$("#grdDetail").data("kendoGrid").dataSource.read({
										id: $('#HDR_PJBId').val()
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
                scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_PJB]" +
                    " where HDR_PJBId='" + $("#HDR_PJBId").val() + "'",
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
                scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[HDR_PO]" +
                    " where patindex('%" + $("#HDR_PJBId").val() + "%',ListPJBId)>0",
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
                    " where DTL_PJBId='" + id + "'",
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
			if(typeof(e.item)!='undefined') 
				_NomorInputView = this.dataItem(e.item);
			else
				_NomorInputView = e.sender.dataItem();
            ModelToView(_NomorInputView);
            hdr_PJBId = _NomorInputView.HDR_PJBId;
            mainBudidayaId = _NomorInputView.MainBudidayaId;
            cekData(_NomorInputView.NomorInputView);
			$('#No_PJB').focus();
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

    function btnPrintHeaderOnClick() {
        var viewer = $("#reportViewer").data("telerik_ReportViewer");
        viewer.reportSource({
            report: viewer.reportSource().report,
            parameters: { HDR_PJBId: $('#HDR_PJBId').val() }
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

    function filterNomorInput(e)
    {
        var mdl = JSON.stringify(ViewToModel());
        return { _model: mdl, _menuId:menuId };
    }

    function filterBudidaya(e) {
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

            mainBudidayaIdOnChange(e).done(function (data) {
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


    function mainBudidayaIdOnChange(e)
    {
        var url = '/Input/hapusContext';
        return $.ajax({
            url: url,
            type: 'POST',
            datatype: 'json',
            data: {
                tahunBuku: $('#TahunBuku').val(),
                _menuId: menuId,
                mainBudidayaId: $('#MainBudidayaId').val()
            },
        });
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
    }

    function buyerOnSelect(e) {
        var url = '/Input/GetDataFrom';
        return $.ajax({
            url: url,
            //contentType: 'application/json; charset=utf-8',
            data: {
                strClassView: "Ptpn8.Penjualan.Models.View_HDRPJB",
                scriptSQL: "SELECT DISTINCT Nama, rtrim(Alamat)+', '+rtrim(alamat1)+' '+rtrim(alamat2) as Alamat, kota, propinsi,npwp from [ReferensiEF].[dbo].[Buyer] as A" +
                            " where BuyerId='" + $('#BuyerId').val() + "'",
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
            model.DTL_PJBId = model.uid;
            model.HDR_PJBId = hdr_PJBId;
        }
        dtl_PJBId = model.DTL_PJBId;
        mainBudidayaId = $('#MainBudidayaId').val();
        gridStatus = 'sudah-diapa-apain';

        $('#jmlGross').text(kendo.toString(rekapGross(), 'n2'));
		$('#jmlHarga').text(kendo.toString(rekapJumlahHarga(), 'n2'));
		$('#jmlTare').text(kendo.toString(rekapTare(), 'n2'));
		$('#jmlQtySacks').text(kendo.toString(rekapQtySacks(), 'n2'));
		    model.JumlahHarga = hitungJumlah(dtl_PJBId);
		    $('#jml' + model.dtl_PJBId).text(kendo.toString(model.JumlahHarga, 'n2'));
    }

    function rekapGross() {

        var grid = $('#grdDetail').data('kendoGrid');
        var newValue = 0;
        for (var i = 0; i < grid.dataItems().length; i++)
        {
            newValue += grid.dataItems()[i].Netto;
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
            newValue = dataItem.Price * (dataItem.Netto);
        
        return newValue;
    }

    function hitungHargaSatuan(id) {
        var grid = $('#grdDetail').data('kendoGrid');
        var dataItem = typeof (grid.dataSource.get(id)) == "undefined" ? grid.dataSource.getByUid(id) : grid.dataSource.get(id);
        var newValue = 0.00;
        if (dataItem.MataUang == "IDR")
            newValue = dataItem.JumlahHarga / (dataItem.Gross - dataItem.Tare);
        else
        {
            var jmlHargaDalamUSCentD = (dataItem.JumlahHarga / dataItem.Kurs) * 100;
            var rnum = jmlHargaDalamUSCentD / (dataItem.Gross - dataItem.Tare);
            newValue = rnum;
        }
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

    function aucNamaSubBudidayaOnSelect(e) {
        var ddlItem = this.dataItem(e.item);
        var grid = $('#grdDetail').data('kendoGrid');
        var dataItem = typeof(grid.dataSource.get(dtl_PJBId))=="undefined" ? grid.dataSource.getByUid(dtl_PJBId) : grid.dataSource.get(dtl_PJBId);
        var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
        var data = grid.dataItem(row);
        data.SubBudidayaId = ddlItem.SubBudidayaId;
        subBudidayaId = ddlItem.SubBudidayaId;
    }

    function aucNamaSubBudidayaOnDataBound(e) {

    }

    function filterMerk(e) {
        var grid = $('#grdDetail').data('kendoGrid');
        var dataItem = typeof (grid.dataSource.get(dtl_PJBId)) == "undefined" ? grid.dataSource.getByUid(dtl_PJBId) : grid.dataSource.get(dtl_PJBId);
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
        var dataItem = typeof(grid.dataSource.get(dtl_PJBId))=="undefined" ? grid.dataSource.getByUid(dtl_PJBId) : grid.dataSource.get(dtl_PJBId);
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
        var dataItem = typeof(grid.dataSource.get(dtl_PJBId))=="undefined" ? grid.dataSource.getByUid(dtl_PJBId) : grid.dataSource.get(dtl_PJBId);
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
        var dataItem = typeof (grid.dataSource.get(dtl_PJBId)) == "undefined" ? grid.dataSource.getByUid(dtl_PJBId) : grid.dataSource.get(dtl_PJBId);
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
        var dataItem = typeof(grid.dataSource.get(dtl_PJBId))=="undefined" ? grid.dataSource.getByUid(dtl_PJBId) : grid.dataSource.get(dtl_PJBId);
        var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
        var data = grid.dataItem(row);
        data.GradeId = ddlItem.GradeId;
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
    function cekNoChop(input)    
    {
        var grid = $('#grdDetail').data('kendoGrid');
        var dataItem = typeof (grid.dataSource.get(dtl_PJBId)) == "undefined" ? grid.dataSource.getByUid(dtl_PJBId) : grid.dataSource.get(dtl_PJBId);
        var items = grid.dataItems();
        var arr = [,];
        for (var i = 0; i < items.length; i++)
        {
            if (items[i].Chop == input.val() && items[i].DTL_PJBId != dtl_PJBId 
                && items[i].Chop.toLowerCase() != "na" && items[i].MerkId==dataItem.MerkId) {
                arr.push([items[i].DTL_PJBId,1]);
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
                    " select DTL_PJBId as Id, count(*) as Cnt from [SPDK_KanpusEF].[dbo].[DTL_PJB]" +
                    " where MerkId='" + dataItem.MerkId + "'" +
                    " and SubBudidayaId='" + dataItem.SubBudidayaId + "'" +
                    " and Chop='" + input.val() + "' and lower(Chop)!='na' and TahunProduksi='"+dataItem.TahunProduksi+"' group by DTL_PJBId",
                _menuId: menuId
            },
            type: 'GET',
            datatype: 'json'
        });
    }

    function ambilStandarGrade() {
        var grid = $('#grdDetail').data('kendoGrid');
        var dataItem = typeof (grid.dataSource.get(dtl_PJBId)) == "undefined" ? grid.dataSource.getByUid(dtl_PJBId) : grid.dataSource.get(dtl_PJBId);
        return $.ajax({
            url: '/Input/ExecSQL',
            data: {
                scriptSQL:
                    " select GradeId,JumlahSatuan,Standar_BeratPerSatuan,Tara from [ReferensiEF].[dbo].[Grade]" +
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
			PeriksaConstraintSeluruhDetail().done(function(data){
				if(data>0)
				{
					openDelDetailWindow();
				}
				else
				{
					var grid = $('#grdDetail').data('kendoGrid');
					var row=grid.tbody.find("tr:first");
					do {
						$('#grdDetail').data('kendoGrid').removeRow(row);
						row=grid.tbody.find("tr:first");
					}
					while (grid.tbody.contents().length>0)
				}
				gridStatus = 'sudah-diapa-apain';
			}).fail(function() {
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
		for(var i=0;i<data.length;i++)
		{
			listId.push("'"+data[i].DTL_PJBId.toString()+"'");
		}
		var strId = listId.toString();
		
        var url = '/Input/ExecSQL';
        return $.ajax({
            url: url,
            data: {
                scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_PO]" +
                    " where DTL_PJBId in (" + strId + ")",
                _menuId: menuId
            },
            type: 'GET',
            datatype: 'json'
        });
    }
    	
    function filterdetailRead(e)	
    {
        return { _menuId: menuId };
    }

    function filterdetailCreate(e) {
        return { _menuId: menuId };
    }

    
