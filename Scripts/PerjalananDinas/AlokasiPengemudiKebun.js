var alokasiPengemudiKebunId, err, berangkat, berangkatdasar, month, year,day, date;
var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var detailBaru = false;
var userName;
var menuId = '3460eb27-8f99-4115-ba0f-f97de00d92b4';
var model;
var headerBaru, detailBaru;
var gridStatus;
var jumlahHari, jenisTujuan, statusKMdanMenginap, dapatPenginapan, jumlahMakan;
var jenisTujuan, jarak, asalId, tujuanId, tujuan, jenistujuan2, tujuandlmwilayah;



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

    ceknik = $("#ceknikWindow").kendoWindow({
        title: "Cari NIK",
        modal: true,
        visible: false,
        resizable: false,
        width: 700,
        height: 450
    }).data("kendoWindow");

    gridStatus = 'belum-ngapa-ngapain';
    var golongan;
    $("#grdDetail").kendoValidator({
        rules: { // custom rules
            chopvalidation: function (input) {
                if (input.attr("name") == "NomorAlokasi" && input.val() != "") {
                    cekNamaKebun(input.val()).done(function (data) {
                        if (data.length > 0) {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(alokasiPengemudiKebunId)) == "undefined" ? grid.dataSource.getByUid(alokasiPengemudiKebunId) : grid.dataSource.get(alokasiPengemudiKebunId);
                            dataItem.KebunId = data[0][0];
                            $('#KebunId').val(data[0][0]);
                            dataItem.AsalId = data[0][0];
                            $('#AsalId').val(data[0][0]);
                            dataItem.AsalDalamWilayah = data[0][1];
                            $('#AsalDalamWilayah').val(data[0][1]);
                            grid.refresh();
                        }
                    });
                } else {
                    return true;
                };
            }
        }
    });
    $("#grdDetail").kendoValidator({
        rules: { // custom rules
            validation: function (input) {
                if (input.attr("name") == "NIK" || input.attr("name") == "NamaDriver" || input.attr("name") == "Golongan" || input.attr("name") == "Jabatan") {
                    cekNIK($('#NIK').val()).done(function (data) {
                        if (data.length > 0) {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(alokasiPengemudiKebunId)) == "undefined" ? grid.dataSource.getByUid(alokasiPengemudiKebunId) : grid.dataSource.get(alokasiPengemudiKebunId);
                            dataItem.NamaDriver = data[0][0];
                            $('#NamaDriver').val(data[0][0]);
                            if (data[0][1] == 01)
                            { golongan = "IA" }
                            else if (data[0][1] == 02)
                            { golongan = "IB" }
                            else if (data[0][1] == 03)
                            { golongan = "IC" }
                            else if (data[0][1] == 04)
                            { golongan = "ID" }
                            else if (data[0][1] == 05)
                            { golongan = "IIA" }
                            else if (data[0][1] == 06)
                            { golongan = "IIB" }
                            else if (data[0][1] == 07)
                            { golongan = "IIC" }
                            else if (data[0][1] == 08)
                            { golongan = "IID" }
                            else if (data[0][1] == 09)
                            { golongan = "IIIA" }
                            else if (data[0][1] == 10)
                            { golongan = "IIIB" }
                            else if (data[0][1] == 11)
                            { golongan = "IIIC" }
                            else if (data[0][1] == 12)
                            { golongan = "IIID" }
                            else if (data[0][1] == 13)
                            { golongan = "IVA" }
                            else if (data[0][1] == 14)
                            { golongan = "IVB" }
                            else if (data[0][1] == 15)
                            { golongan = "IVC" }
                            else if (data[0][1] == 16)
                            { golongan = "IVD" }
                            else if (data[0][1] == '')
                            { golongan = "" }
                            dataItem.Golongan = golongan + " " + data[0][2];
                            $('#Golongan').val(golongan + " " + data[0][2]);
                            dataItem.Jabatan = data[0][3];
                            $('#Jabatan').val(data[0][3]);


                            var golongan1 = golongan;
                            var jabatan = dataItem["Jabatan"];
                            var jabatan1;

                            
                            var tujuandalamwilayah;

                            var kendaraanDinas = 1;
                            if (jenisTujuan == "Dalam Wilayah")
                            { var kmdanmenginap = statusKMdanMenginap; }
                            else if (jenisTujuan == "Luar Wilayah > 50 km")
                            {
                                if (jumlahHari == 0)
                                { kmdanmenginap = "Tdk Menginap > 50 km"; }
                                else if (jumlahHari > 0)
                                { kmdanmenginap = "Menginap > 50 km"; }
                            }   
                            else if (jenisTujuan == "Luar Wilayah < 50 km") {
                                if (jumlahHari == 0) { kmdanmenginap = "Tdk Menginap < 50 km"; }
                                else if (jumlahHari > 0) { kmdanmenginap = "Menginap < 50 km"; }
                            }
                            else if (jenisTujuan == "Jakarta PP")
                                { kmdanmenginap = "Jakarta"; }
                            else if (jenisTujuan == "Jakarta Nginap")
                                { kmdanmenginap = "Jakarta"; }

                            if (jabatan == "DIREKTUR UTAMA") { jabatan1 = "Dirut" }
                            else if (jabatan == "KOMISARIS UTAMA") { jabatan1 = "Dirut" }
                            else if (jabatan == "DIREKTUR MANAJEMEN ASET") { jabatan1 = "Direksi" }
                            else if (jabatan == "DIREKTUR KOMERSIL") { jabatan1 = "Direksi" }
                            else if (jabatan == "DIREKTUR OPERASIONAL") { jabatan1 = "Direksi" }
                            else if (jabatan == "KOMISARIS") { jabatan1 = "Direksi" }
                            else if (jabatan.toLowerCase().indexOf("kepala bagian") > -1) { jabatan1 = "PJP" }
                            else if (jabatan == "MANAJER") { jabatan1 = "PJP" }
                            else if (jabatan == "Pjs.MANAJER") { jabatan1 = "PJP" }
                            else if (jabatan == "MANAJER AGROWISATA") { jabatan1 = "PJP" }
                            else if (jabatan == "MANAJER INDUSTRI HILIR TEH") { jabatan1 = "PJP" }
                            else if (jabatan == "MANAJER ANEKA USAHA") { jabatan1 = "PJP" }
                            else if (jabatan.toLowerCase().indexOf("Staf Setara Manajer") > -1) { jabatan1 = "PJP" }
                            else if (jabatan.toLowerCase().indexOf("kasubag") > -1) { jabatan1 = "Kaur" }
                            else if (jabatan.toLowerCase().indexOf("kepala bidang") > -1) { jabatan1 = "Kaur" }
                            else if (jabatan.toLowerCase().indexOf("asisten manajer") > -1) { jabatan1 = "Kaur" }
                            else if (jabatan.toLowerCase().indexOf("asisten kepala") > -1) { jabatan1 = "Kaur" }

                            if (golongan1 == "IA" || golongan1 == "IB" || golongan1 == "IC" || golongan1 == "ID") {                             
                                jabatan1 = "Gol_I"
                            }
                            else if (golongan1 == "IIA" || golongan1 == "IIB" || golongan1 == "IIC" || golongan1 == "IID") {
                                jabatan1 = "Gol_II"
                            }
                            else if (golongan1 == "IIIA" || golongan1 == "IIIB" || golongan1 == "IIIC" || golongan1 == "IIID") {
                                if (jabatan.toLowerCase().indexOf("staf") > -1 || jabatan.toLowerCase().indexOf("asisten afdeling") > -1 || jabatan.toLowerCase().indexOf("asisten pengolahan") > -1) { jabatan1 = "Gol_III" }
                            }
                            else if (golongan1 == "IVA" || golongan1 == "IVB" || golongan1 == "IVC" || golongan1 == "IVD") {
                                if (jabatan.toLowerCase().indexOf("staf") > -1) { jabatan1 = "Gol_IV" }
                            }
                            else if (golongan1 == "" || jabatan == "") {
                                jabatan1 = "Gol_I"
                            }

                            var cSQL = "";
                            var kondisi1 = "";
                            var kondisi2 = "";
                            var kondisi3 = "";
                            var kondisi4 = "";
                            if (jenisTujuan == 'Dalam Wilayah') {
                                if (dapatPenginapan == 0) {
                                    if (kmdanmenginap == "Menginap < 50 km") {
                                        if (tujuandlmwilayah.toLowerCase().indexOf("bagian") > -1 || tujuandlmwilayah.toLowerCase().indexOf("sekretariat") > -1|| tujuandlmwilayah.toLowerCase().indexOf("satuan") > -1 || tujuandlmwilayah.toLowerCase().indexOf("wilayah") > -1) {
                                            kondisi1 = "uraianbiaya='Uang Saku < 50 km'";
                                            kondisi2 = "uraianbiaya='Uang Makan < 50 km'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                        else {
                                            kondisi1 = "uraianbiaya='Uang Saku < 50 km'";
                                            kondisi2 = "uraianbiaya='Uang Makan < 50 km'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 2 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                    }
                                    else if (kmdanmenginap == "Menginap > 50 km") {
                                        if (tujuandlmwilayah.toLowerCase().indexOf("bagian") > -1 || tujuandlmwilayah.toLowerCase().indexOf("sekretariat") > -1|| tujuandlmwilayah.toLowerCase().indexOf("satuan") > -1 || tujuandlmwilayah.toLowerCase().indexOf("wilayah") > -1) {
                                            kondisi1 = "uraianbiaya='Uang Saku > 50 km'";
                                            kondisi2 = "uraianbiaya='Uang Makan > 50 km'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                        else {
                                            kondisi1 = "uraianbiaya='Uang Saku > 50 km'";
                                            kondisi2 = "uraianbiaya='Uang Makan > 50 km'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 2 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                    }                                
                                    else if (kmdanmenginap == "Tdk Menginap < 50 km") {
                                        kondisi1 = "uraianbiaya='Uang Saku < 50 km'"
                                        kondisi2 = "uraianbiaya='Uang Makan < 50 km'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0  from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (kmdanmenginap == "Tdk Menginap > 50 km") {
                                        kondisi1 = "uraianbiaya='Uang Saku > 50 km'"
                                        kondisi2 = "uraianbiaya='Uang Makan > 50 km'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                    }                                    
                                }
                                else if (dapatPenginapan == 1) {
                                    if (kmdanmenginap == "Menginap < 50 km") {
                                        if (tujuandlmwilayah.toLowerCase().indexOf("bagian") > -1 || tujuandlmwilayah.toLowerCase().indexOf("sekretariat") > -1|| tujuandlmwilayah.toLowerCase().indexOf("satuan") > -1 || tujuandlmwilayah.toLowerCase().indexOf("wilayah") > -1) {
                                            kondisi1 = "uraianbiaya='Uang Saku < 50 km'";
                                            kondisi2 = "uraianbiaya='Uang Makan < 50 km'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            kondisi4 = "uraianbiaya='Biaya Penginapan'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                        else {
                                            kondisi1 = "uraianbiaya='Uang Saku < 50 km'";
                                            kondisi2 = "uraianbiaya='Uang Makan < 50 km'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            kondisi4 = "uraianbiaya='Biaya Penginapan'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 2 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                    }
                                    else if (kmdanmenginap == "Menginap > 50 km") {
                                        if (tujuandlmwilayah.toLowerCase().indexOf("bagian") > -1 || tujuandlmwilayah.toLowerCase().indexOf("sekretariat") > -1|| tujuandlmwilayah.toLowerCase().indexOf("satuan") > -1 || tujuandlmwilayah.toLowerCase().indexOf("wilayah") > -1) {
                                            kondisi1 = "uraianbiaya='Uang Saku > 50 km'";
                                            kondisi2 = "uraianbiaya='Uang Makan > 50 km'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            kondisi4 = "uraianbiaya='Biaya Penginapan'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                        else {
                                            kondisi1 = "uraianbiaya='Uang Saku > 50 km'";
                                            kondisi2 = "uraianbiaya='Uang Makan > 50 km'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            kondisi4 = "uraianbiaya='Biaya Penginapan'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 2 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                    }                 
                                    else if (kmdanmenginap == "Tdk Menginap < 50 km") {
                                        kondisi1 = "uraianbiaya='Uang Saku < 50 km'"
                                        kondisi2 = "uraianbiaya='Uang Makan < 50 km'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0  from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (kmdanmenginap == "Tdk Menginap > 50 km") {
                                        kondisi1 = "uraianbiaya='Uang Saku > 50 km'"
                                        kondisi2 = "uraianbiaya='Uang Makan > 50 km'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                    }
                                }                                
                            }
                            else if (jenisTujuan == 'Luar Wilayah < 50 km') {
                                if (kmdanmenginap == "Menginap < 50 km") {
                                    if (dapatPenginapan == 1) {
                                        kondisi1 = "uraianbiaya='Uang Saku < 50 km'";
                                        kondisi2 = "uraianbiaya='Uang Makan < 50 km'";
                                        kondisi3 = "uraianbiaya='Biaya Cucian'";
                                        kondisi4 = "uraianbiaya='Biaya Penginapan'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi3 + ")  group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (dapatPenginapan == 0) {
                                        kondisi1 = "uraianbiaya='Uang Saku < 50 km'";
                                        kondisi2 = "uraianbiaya='Uang Makan < 50 km'";
                                        kondisi3 = "uraianbiaya='Biaya Cucian'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
              
                                    }
                                }
                                else {
                                    kondisi1 = "uraianbiaya='Uang Saku < 50 km'"
                                    kondisi2 = "uraianbiaya='Uang Makan < 50 km'"
                              
                                    cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0  from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                    " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                }
                            }
                            else if (jenisTujuan == 'Luar Wilayah > 50 km') {
                                if (kmdanmenginap == "Menginap > 50 km") {
                                    if (dapatPenginapan == 1) {
                                        kondisi1 = "uraianbiaya='Uang Saku > 50 km'";
                                        kondisi2 = "uraianbiaya='Uang Makan > 50 km'";
                                        kondisi3 = "uraianbiaya='Biaya Cucian'";
                                        kondisi4 = "uraianbiaya='Biaya Penginapan'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi3 + ")  group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (dapatPenginapan == 0) {
                                        kondisi1 = "uraianbiaya='Uang Saku > 50 km'";
                                        kondisi2 = "uraianbiaya='Uang Makan > 50 km'";
                                        kondisi3 = "uraianbiaya='Biaya Cucian'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";

                                    }
                                }
                                else {
                                    kondisi1 = "uraianbiaya='Uang Saku > 50 km'"
                                    kondisi2 = "uraianbiaya='Uang Makan > 50 km'"

                                    cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                }
                            }
                            else if (jenisTujuan == 'Jakarta PP') {
                                if (kendaraanDinas == 1) {
                                    kondisi1 = "uraianbiaya='Uang Saku'";
                                    kondisi2 = "uraianbiaya='Uang Makan'";
                                    cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                    " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                }
                                else if (kendaraanDinas == 0) {
                                    kondisi1 = "uraianbiaya='Uang Saku'";
                                    kondisi2 = "uraianbiaya='Uang Makan'";
                                    kondisi3 = "uraianbiaya='Biaya Transport Lokal'";
                                    cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                    " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                    " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                }
                                else {
                                    kondisi1 = "uraianbiaya='Uang Saku'";
                                    kondisi2 = "uraianbiaya='Uang Makan'";
                                    cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                    " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                }
                            }
                            else if (jenisTujuan == 'Jakarta Nginap') {
                                if (kendaraanDinas == 1) {
                                    if (dapatPenginapan == 1) {
                                        kondisi1 = "uraianbiaya='Uang Saku'";
                                        kondisi2 = "uraianbiaya='Uang Makan'";
                                        kondisi3 = "uraianbiaya='Biaya Cucian'";
                                        kondisi4 = "uraianbiaya='Biaya Penginapan'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya " +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * 0 * (" + jumlahHari + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (dapatPenginapan == 0) {
                                        kondisi1 = "uraianbiaya='Uang Saku'";
                                        kondisi2 = "uraianbiaya='Uang Makan'";
                                        kondisi3 = "uraianbiaya='Biaya Cucian'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                    }
                                }
                                else {
                                    kondisi1 = "uraianbiaya='Uang Saku'";
                                    kondisi2 = "uraianbiaya='Uang Makan'";
                                    kondisi3 = "uraianbiaya='Biaya Cucian'";
                                    kondisi4 = "uraianbiaya='Biaya Penginapan'";
                                    kondisi5 = "uraianbiaya='Biaya Transport Lokal'";

                                    cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                    " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                    " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ") group by uraianbiaya" +
                                    " union select uraianbiaya,sum(" + jabatan1 + ") * 0 * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi4 + ")  group by uraianbiaya" +
                                    " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi5 + ")  group by uraianbiaya order by uraianbiaya";
                                }
                            }


                            HitungTaripBPD(cSQL).done(function (data) {
                                if (jenisTujuan == 'Luar Wilayah > 50 km' || jenisTujuan == 'Luar Wilayah < 50 km') {
                                    if (data.length == 5) {
                                        dataItem.dirty = true;
                                        dataItem["UangCucian"] = parseFloat(data[0][1]);
                                        dataItem["UangPenginapan"] = parseFloat(data[1][1]);
                                        dataItem["UangTransport"] = parseFloat(data[2][1]);
                                        dataItem["UangMakan"] = parseFloat(data[3][1]);
                                        dataItem["UangSaku"] = parseFloat(data[4][1]);
                                        dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]) + parseFloat(data[3][1]) + parseFloat(data[4][1]);
                                    }
                                    else if (data.length == 4) {
                                        dataItem.dirty = true;
                                        dataItem["UangCucian"] = parseFloat(data[0][1]);
                                        dataItem["UangPenginapan"] = parseFloat(data[1][1]);
                                        dataItem["UangMakan"] = parseFloat(data[2][1]);
                                        dataItem["UangSaku"] = parseFloat(data[3][1]);
                                        dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]) + parseFloat(data[3][1]);
                                        dataItem["UangTransport"] = 0;
                                    }
                                    else if (data.length == 3) {
                                        dataItem.dirty = true;
                                        dataItem["UangCucian"] = parseFloat(data[0][1]);
                                        dataItem["UangMakan"] = parseFloat(data[1][1]);
                                        dataItem["UangSaku"] = parseFloat(data[2][1]);
                                        dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]);
                                        dataItem["UangPenginapan"] = 0;
                                        dataItem["UangTransport"] = 0;
                                    }
                                    else if (data.length == 2) {
                                        dataItem.dirty = true;
                                        dataItem["UangMakan"] = parseFloat(data[0][1]);
                                        dataItem["UangSaku"] = parseFloat(data[1][1]);
                                        dataItem["UangCucian"] = 0;
                                        dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]);
                                        dataItem["UangTransport"] = 0;
                                        dataItem["UangPenginapan"] = 0;
                                    }
                                }
                                else if (jenisTujuan == 'Jakarta PP') {
                                    if (data.length == 3) {
                                        dataItem.dirty = true;
                                        dataItem["UangTransport"] = parseFloat(data[0][1]);
                                        dataItem["UangMakan"] = parseFloat(data[1][1]);
                                        dataItem["UangSaku"] = parseFloat(data[2][1]);
                                        dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]);
                                        dataItem["UangCucian"] = 0;
                                        dataItem["UangPenginapan"] = 0;
                                    }
                                    else {
                                        dataItem.dirty = true;
                                        dataItem["UangMakan"] = parseFloat(data[0][1]);
                                        dataItem["UangSaku"] = parseFloat(data[1][1]);
                                        dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]);
                                        dataItem["UangTransport"] = 0;
                                        dataItem["UangCucian"] = 0;
                                        dataItem["UangPenginapan"] = 0;
                                    }
                                }
                                else if (jenisTujuan == 'Jakarta Nginap') {
                                    if (data.length == 5) {
                                        dataItem.dirty = true;
                                        dataItem["UangCucian"] = parseFloat(data[0][1]);
                                        dataItem["UangPenginapan"] = parseFloat(data[1][1]);
                                        dataItem["UangTransport"] = parseFloat(data[2][1]);
                                        dataItem["UangMakan"] = parseFloat(data[3][1]);
                                        dataItem["UangSaku"] = parseFloat(data[4][1]);
                                        dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]) + parseFloat(data[3][1]) + parseFloat(data[4][1]);
                                    }
                                    else if (data.length == 4) {
                                        dataItem.dirty = true;
                                        dataItem["UangCucian"] = parseFloat(data[0][1]);
                                        dataItem["UangPenginapan"] = parseFloat(data[1][1]);
                                        dataItem["UangMakan"] = parseFloat(data[2][1]);
                                        dataItem["UangSaku"] = parseFloat(data[3][1]);
                                        dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]) + parseFloat(data[3][1]);
                                        dataItem["UangTransport"] = 0;
                                    }
                                    else {
                                        dataItem.dirty = true;
                                        dataItem["UangCucian"] = parseFloat(data[0][1]);
                                        dataItem["UangMakan"] = parseFloat(data[1][1]);
                                        dataItem["UangSaku"] = parseFloat(data[2][1]);
                                        dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]);
                                        dataItem["UangTransport"] = 0;
                                        dataItem["UangPenginapan"] = 0;
                                    }
                                }
                                else if (jenisTujuan == 'Dalam Wilayah') {
                                    if (data.length == 4) {
                                        dataItem.dirty = true;
                                        dataItem["UangCucian"] = parseFloat(data[0][1]);
                                        dataItem["UangPenginapan"] = parseFloat(data[1][1]);
                                        dataItem["UangMakan"] = parseFloat(data[2][1]);
                                        dataItem["UangSaku"] = parseFloat(data[3][1]);
                                        dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]) + parseFloat(data[3][1]);
                                        dataItem["UangTransport"] = 0;
                                    }
                                    else if (data.length == 3) {
                                        dataItem.dirty = true;
                                        dataItem["UangCucian"] = parseFloat(data[0][1]);
                                        dataItem["UangMakan"] = parseFloat(data[1][1]);
                                        dataItem["UangSaku"] = parseFloat(data[2][1]);
                                        dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]);
                                        dataItem["UangTransport"] = 0;
                                        dataItem["UangPenginapan"] = 0;
                                    }
                                    else if (data.length == 2) {
                                        dataItem.dirty = true;
                                        dataItem["UangMakan"] = parseFloat(data[0][1]);
                                        dataItem["UangSaku"] = parseFloat(data[1][1]);
                                        dataItem["UangCucian"] = 0;
                                        dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]);
                                        dataItem["UangTransport"] = 0;
                                        dataItem["UangPenginapan"] = 0;
                                    }
                                    else if (data.length == 1) {
                                        dataItem.dirty = true;
                                        dataItem["UangMakan"] = parseFloat(data[0][1]);
                                        dataItem["UangSaku"] = 0;
                                        dataItem["UangCucian"] = 0;
                                        dataItem["JumlahBPD"] = parseFloat(data[0][1]);
                                        dataItem["UangTransport"] = 0;
                                        dataItem["UangPenginapan"] = 0;
                                    }
                                    else {
                                        dataItem["UangMakan"] = 0;
                                        dataItem["UangSaku"] = 0;
                                        dataItem["UangCucian"] = 0;
                                        dataItem["JumlahBPD"] = 0;
                                        dataItem["UangTransport"] = 0;
                                        dataItem["UangPenginapan"] = 0;
                                    }
                                }
                                var model = grid.dataSource.data();
                                grid.refresh();
                            });
                        }
                        else {
                            $('#errMsg').text('NIK belum terdaftar di bagian/unit/kebun anda!');
                            openErrWindow();
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(alokasiPengemudiKebunId)) == "undefined" ? grid.dataSource.getByUid(alokasiPengemudiKebunId) : grid.dataSource.get(alokasiPengemudiKebunId);
                            
                            return false;
                        }
                    });
                }
            }
        }
    });
    //////// sampe sini
});

function cekNIKPopUp(e) {

    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    ceknik.open().center();

}

function getDataFrom() {
    // Hitung Jumlah Record di Detail
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select top 1 Jarak from [ReferensiEF].[dbo].[Jarak]" +
                " where AsalId='" + asalId + "' and TujuanId ='" + tujuanId + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function cekNamaKebun(namaKebun) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select KebunId, Nama from [ReferensiEF].[dbo].[Kebun]" +
                " where kebunId=(select posisilokasikerjaid from [Identity].[dbo].[AspNetUsers] where UserName ='" + usrName + "' )",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function HitungTaripBPD(cSQL) {
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

function cekNIK(nik) {
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(alokasiPengemudiKebunId)) == "undefined" ? grid.dataSource.getByUid(alokasiPengemudiKebunId) : grid.dataSource.get(alokasiPengemudiKebunId);

    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select top 1 A.Nama, A.Gol, A.MK, A.Nama_Jab from [SPDK_KanpusEF].[dbo].[Ref_Dik] as A " +
                " join [ReferensiEF].[dbo].[Kebun] as B on A.kd_kbn=(Select kd_kbn from [ReferensiEF].dbo.Kebun where KebunId = (select posisilokasikerjaid from [Identity].[dbo].[AspNetUsers] where UserName ='" + usrName + "' )) " +
                " where A.reg='" + dataItem.NIK + "' and Year(A.Tanggal)=" + year + " and Month(A.Tanggal)=" + month + "" +
                " union select top 1 A.Nama, '' as Gol, '' as MK, '' as Nama_Jab from [SPDK_KanpusEF].[dbo].[Ref_DikKLM] as A " +
                " join [ReferensiEF].[dbo].[Kebun] as B on A.kd_kbn=(Select kd_kbn from [ReferensiEF].dbo.Kebun where KebunId = (select posisilokasikerjaid from [Identity].[dbo].[AspNetUsers] where UserName ='" + usrName + "' )) " +
                " where A.reg='" + dataItem.NIK + "' and Year(A.Tanggal)=" + year + " and Month(A.Tanggal)=" + month + "",
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
                    berangkatdasar = $('#TanggalAwal').data('kendoDatePicker').value();
                    berangkat = new Date(berangkatdasar),
                        month = '' + (berangkat.getMonth() + 1),
                        day = '' + berangkat.getDate(),
                        year = berangkat.getFullYear();
                    if (month.length < 2) month = '0' + month;
                    if (day.length < 2) day = '0' + day;
                    berangkat = [year, month, day].join('-');

                    var grid = $("#grdDetail").data("kendoGrid");
                    grid.dataSource.read({
                        id: $('#AlokasiPengemudiKebunId').val()
                    }).done(function () {
                        var currentData = grid.dataSource.data();
                        for (var i = 0; i < currentData.length; i++) {
                            currentData[i].dirty = true;
                        }
                    });

                } else {
                    // not valid
                }
            }
        });

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
        model.AlokasiPengemudiKebunId = model.uid;
    }
    alokasiPengemudiKebunId = model.AlokasiPengemudiKebunId;
    jenisTujuan = model.JenisTujuan;

    dapatPenginapan = model.DapatPenginapan;
    jumlahMakan = model.JumlahMakan;
    asalId = model.AsalId;
    tujuanId = model.TujuanId;
    tujuan = model.Tujuan;
    tujuandlmwilayah = model.TujuanDalamWilayah;

    var berangkat = model.Berangkat;
    var kembali = model.Kembali;
    var dateStringBerangkat = new Date();
        dateStringBerangkat = berangkat.getFullYear() + "/" + (berangkat.getMonth() + 1) + "/" + (berangkat.getDate())
    var dateStringKembali = new Date();
        dateStringKembali = kembali.getFullYear() + "/" + (kembali.getMonth() + 1) + "/" + (kembali.getDate())

    var hariAkhirPisah = dateStringKembali.split('/');
    var hariAwalPisah = dateStringBerangkat.split('/');
    var objek_tgl = new Date();
    var pisahTanggalAkhir = objek_tgl.setFullYear(parseInt(hariAkhirPisah[0]), parseInt(hariAkhirPisah[1] - 1), parseInt(hariAkhirPisah[2]));
    var pisahTanggalAwal = objek_tgl.setFullYear(parseInt(hariAwalPisah[0]), parseInt(hariAwalPisah[1] - 1), parseInt(hariAwalPisah[2]));

    //var jumlahHari;
    var dayberangkat = parseInt(hariAwalPisah[2]);
    var daypulang = parseInt(hariAkhirPisah[2]);
    var monthberangkat = parseInt(hariAwalPisah[1]);
    var monthpulang = parseInt(hariAkhirPisah[1]);

    if ((dayberangkat == daypulang) && (monthberangkat == monthpulang)) {
        jumlahHari = 0;
    }
    else {
        jumlahHari = (((((pisahTanggalAkhir - pisahTanggalAwal) / 1000) / 60) / 60) / 24);
    }
    if (jenisTujuan == "Jakarta" && jumlahHari == 0)
    { jenisTujuan = "Jakarta PP" }
    else if (jenisTujuan == "Jakarta" && jumlahHari > 0)
    { jenisTujuan = "Jakarta Nginap" }

    if (jarak < 51 && jumlahHari == 0)
        { statusKMdanMenginap = "Tdk Menginap < 50 km"; }
    else if (jarak < 51 && jumlahHari > 0)
        { statusKMdanMenginap = "Menginap < 50 km"; }
    else if (jarak > 50 && jumlahHari == 0)
        { statusKMdanMenginap = "Tdk Menginap > 50 km" }
    else if (jarak > 50 && jumlahHari > 0)
        { statusKMdanMenginap = "Menginap < 50 km" }
    else if (jarak == null && jumlahHari > 0 && jenisTujuan == "Dalam Wilayah")
        { statusKMdanMenginap = "Menginap < 50 km" }
    else if (jarak == null && jumlahHari == 0 && jenisTujuan == "Dalam Wilayah")
        { statusKMdanMenginap = "Tdk Menginap < 50 km" }
    else if (jarak == null && jumlahHari == 0 && jenisTujuan == "Luar Wilayah < 50 km")
        { statusKMdanMenginap = "Tdk Menginap < 50 km" }
    else if (jarak == null && jumlahHari > 0 && jenisTujuan == "Luar Wilayah < 50 km")
        { statusKMdanMenginap = "Menginap < 50 km" }
    else if (jarak == null && jumlahHari > 0 && jenisTujuan == "Luar Wilayah > 50 km")
        { statusKMdanMenginap = "Menginap < 50 km" }
    else if (jarak == null && jumlahHari == 0 && jenisTujuan == "Luar Wilayah > 50 km")
        { statusKMdanMenginap = "Tdk Menginap < 50 km" }
   
    else if ((jenisTujuan == "Jakarta PP" || jenisTujuan == "Jakarta Nginap") && (jumlahHari == 0 || jumlahHari > 0))
        { statusKMdanMenginap = "Jakarta" }

    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(alokasiPengemudiKebunId)) == "undefined" ? grid.dataSource.getByUid(alokasiPengemudiKebunId) : grid.dataSource.get(alokasiPengemudiKebunId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.KMdanMenginap = statusKMdanMenginap;

    $(e.container).find("input[name='UangSaku']").prop('disabled', true);
    $(e.container).find("input[name='UangMakan']").prop('disabled', true);
    $(e.container).find("input[name='UangCucian']").prop('disabled', true);
    $(e.container).find("input[name='UangTransport']").prop('disabled', true);
    $(e.container).find("input[name='JumlahBPD']").prop('disabled', true);
    if (jenisTujuan == "Dalam Wilayah") {
        if (tujuandlmwilayah == "Lain-Lain < 50" || tujuandlmwilayah == "Lain-Lain > 50" || tujuandlmwilayah == "Lain-Lain 100-200")
        {
            $(e.container).find("input[name='Tujuan']").prop('disabled', false);
        }
        else if (tujuandlmwilayah != "Lain-Lain")
        {
            $(e.container).find("input[name='Tujuan']").prop('disabled', true);
            $(e.container).find("input[name='TujuanDalamWilayah']").prop('disabled', false);
        }
    }
    else if (jenisTujuan == "Luar Wilayah > 50 km" || jenisTujuan == "Luar Wilayah < 50 km") {
        $(e.container).find("input[name='Tujuan']").prop('disabled', false);
        //$(e.container).find("input[name='TujuanDalamWilayah']").prop('disabled', true);
        //e.container.find("input[name='TujuanDalamWilayah']").data("kendoDropDownList").enable(false);
    }
    else if (jenisTujuan == "Jakarta PP") {
        $(e.container).find("input[name='Tujuan']").prop('disabled', true);
        $(e.container).find("input[name='JumlahMakan']").prop('disabled', true);
        //$(e.container).find("input[name='TujuanDalamWilayah']").prop('disabled', true);
        //e.container.find("input[name='TujuanDalamWilayah']").data("kendoDropDownList").enable(false);
    }
    else if (jenisTujuan == "Jakarta Nginap") {
        $(e.container).find("input[name='Tujuan']").prop('disabled', true);
        $(e.container).find("input[name='JumlahMakan']").prop('disabled', true);
        //$(e.container).find("input[name='TujuanDalamWilayah']").prop('disabled', true);
        //e.container.find("input[name='TujuanDalamWilayah']").data("kendoDropDownList").enable(false);
    }
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

function filterdetailRead(e) {
    return {
        //id: berangkat,
        _menuId: menuId,
        _filter: ["Year(Berangkat)",""+year+"","Month(Berangkat)",""+month+"","Day(Berangkat)",""+day+""]
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
    var datalokasipengemudiKebunid = data.AlokasiPengemudiKebunId;

    var viewer = $("#reportViewer").data("telerik_ReportViewer");
    viewer.reportSource({
        report: viewer.reportSource().report,
        parameters: { AlokasiPengemudiKebunId: datalokasipengemudiKebunid }
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

function ddlJenisTujuanOnChange(e) {
    jenisTujuan = $('#JenisTujuan').val();
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(alokasiPengemudiKebunId)) == "undefined" ? grid.dataSource.getByUid(alokasiPengemudiKebunId) : grid.dataSource.get(alokasiPengemudiKebunId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);

    
}

function ddlKMdanMenginapOnChange(e) {
    var kmdanmenginap = $('#KMdanMenginap').val();
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(alokasiPengemudiKebunId)) == "undefined" ? grid.dataSource.getByUid(alokasiPengemudiKebunId) : grid.dataSource.get(alokasiPengemudiKebunId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
}

function ddlJumlahMakanOnChange(e) {
    var jumlahmakan = $('#JumlahMakan').val();
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(alokasiPengemudiKebunId)) == "undefined" ? grid.dataSource.getByUid(alokasiPengemudiKebunId) : grid.dataSource.get(alokasiPengemudiKebunId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
}

//function filterAsal() {
//    return {
//        //id: IdBudidaya,
//        value: $('#AsalDalamWilayah').val()
//    };
//}

//function aucLokasiKerjaAsalOnSelect(e) {
//    var ddlItem = this.dataItem(e.item);
//    var grid = $('#grdDetail').data('kendoGrid');
//    var dataItem = typeof (grid.dataSource.get(alokasiPengemudiKebunId)) == "undefined" ? grid.dataSource.getByUid(alokasiPengemudiKebunId) : grid.dataSource.get(alokasiPengemudiKebunId);
//    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
//    var data = grid.dataItem(row);
//    data.AsalId = ddlItem.LokasiKerjaId;
//    asalId = data.AsalId;
//    data.AsalDalamWilayah = ddlItem.Nama;
//    grid.refresh();
//}

//function aucLokasiKerjaAsalOnDataBound(e) {

//}

function filterTujuan() {
    return {
        //id: IdBudidaya,
        value: $('#TujuanDalamWilayah').val()
    };
}

function aucLokasiKerjaTujuanOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(alokasiPengemudiKebunId)) == "undefined" ? grid.dataSource.getByUid(alokasiPengemudiKebunId) : grid.dataSource.get(alokasiPengemudiKebunId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.TujuanId = ddlItem.LokasiKerjaId;
    tujuanId = data.TujuanId;
    data.TujuanDalamWilayah = ddlItem.Nama;
    var namatujuan = ddlItem.Nama;
    grid.refresh();
    if (namatujuan == 'Lain-Lain < 50')
    {
        jarak = 99;
        var grid = $('#grdDetail').data('kendoGrid');
        var dataItem = typeof (grid.dataSource.get(alokasiPengemudiKebunId)) == "undefined" ? grid.dataSource.getByUid(alokasiPengemudiKebunId) : grid.dataSource.get(alokasiPengemudiKebunId);
        dataItem.Jarak = jarak;
        $('#Jarak').val(jarak);
    }
    else if (namatujuan == 'Lain-Lain > 50') {
        jarak = 201;
        var grid = $('#grdDetail').data('kendoGrid');
        var dataItem = typeof (grid.dataSource.get(alokasiPengemudiKebunId)) == "undefined" ? grid.dataSource.getByUid(alokasiPengemudiKebunId) : grid.dataSource.get(alokasiPengemudiKebunId);
        dataItem.Jarak = jarak;
        $('#Jarak').val(jarak);
    }
    else if (namatujuan == 'Lain-Lain 100-200') {
        jarak = 150;
        var grid = $('#grdDetail').data('kendoGrid');
        var dataItem = typeof (grid.dataSource.get(alokasiPengemudiKebunId)) == "undefined" ? grid.dataSource.getByUid(alokasiPengemudiKebunId) : grid.dataSource.get(alokasiPengemudiKebunId);
        dataItem.Jarak = jarak;
        $('#Jarak').val(jarak);
    }
    else if (namatujuan != 'Lain-Lain') {
        getDataFrom().done(function (data) {
            if (data != null) {
                jarak = data[0][0];
                var grid = $('#grdDetail').data('kendoGrid');
                var dataItem = typeof (grid.dataSource.get(alokasiPengemudiKebunId)) == "undefined" ? grid.dataSource.getByUid(alokasiPengemudiKebunId) : grid.dataSource.get(alokasiPengemudiKebunId);
                dataItem.Jarak = data[0][0];
                $('#Jarak').val(data[0][0]);
            }
        })
    }
}

function aucLokasiKerjaTujuanOnDataBound(e) {

}