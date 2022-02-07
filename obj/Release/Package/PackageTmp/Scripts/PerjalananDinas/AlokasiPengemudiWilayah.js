var alokasiPengemudiKebunId, err, berangkat, berangkatdasar, month, year, date;
var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var detailBaru = false;
var userName;
var menuId = '7d4d949c-f6d4-4ea1-b796-412e14d02346';
var model;
var headerBaru, detailBaru;
var gridStatus;
var jumlahHari, jenisTujuan, kmdanmenginap, dapatPenginapan, jumlahMakan;
var jenisTujuan, jarak, asalId, tujuanId, tujuan, tujuandalamwilayah;



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
                            if (data[0][1] == 01) { golongan = "IA" }
                            else if (data[0][1] == 02) { golongan = "IB" }
                            else if (data[0][1] == 03) { golongan = "IC" }
                            else if (data[0][1] == 04) { golongan = "ID" }
                            else if (data[0][1] == 05) { golongan = "IIA" }
                            else if (data[0][1] == 06) { golongan = "IIB" }
                            else if (data[0][1] == 07) { golongan = "IIC" }
                            else if (data[0][1] == 08) { golongan = "IID" }
                            else if (data[0][1] == 09) { golongan = "IIIA" }
                            else if (data[0][1] == 10) { golongan = "IIIB" }
                            else if (data[0][1] == 11) { golongan = "IIIC" }
                            else if (data[0][1] == 12) { golongan = "IIID" }
                            else if (data[0][1] == 13) { golongan = "IVA" }
                            else if (data[0][1] == 14) { golongan = "IVB" }
                            else if (data[0][1] == 15) { golongan = "IVC" }
                            else if (data[0][1] == 16) { golongan = "IVD" }
                            else if (data[0][1] == '') { golongan = "" }
                            dataItem.Golongan = golongan + " " + data[0][2];
                            $('#Golongan').val(golongan + " " + data[0][2]);
                            dataItem.Jabatan = data[0][3];
                            $('#Jabatan').val(data[0][3]);


                            var golongan1 = golongan;
                            var jabatan = dataItem["Jabatan"];
                            var jabatan1;


                            var kendaraanDinas = 1;
                            if (jenisTujuan == "Luar Wilayah > 100 km") {
                                if (jumlahHari == 0) { kmdanmenginap = "Tdk Menginap > 100 km"; }
                                else if (jumlahHari > 0) { kmdanmenginap = "Menginap > 100 km"; }
                            }
                            else if (jenisTujuan == "Luar Wilayah < 100 km") {
                                if (jumlahHari == 0) { kmdanmenginap = "Tdk Menginap < 100 km"; }
                                else if (jumlahHari > 0) { kmdanmenginap = "Menginap < 100 km"; }
                            }
                            else if (jenisTujuan == "Jakarta PP") { kmdanmenginap = "Jakarta"; }
                            else if (jenisTujuan == "Jakarta Nginap") { kmdanmenginap = "Jakarta"; }

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
                                if (jabatan.toLowerCase().indexOf("karyawan") > -1 || jabatan.toLowerCase().indexOf("kary") > -1 || jabatan.toLowerCase().indexOf("jtu") > -1 || jabatan.toLowerCase().indexOf("mandor") > -1 || jabatan.toLowerCase().indexOf("pengemudi") > -1 || jabatan == "") { jabatan1 = "Gol_I" }
                            }
                            else if (golongan1 == "IIA" || golongan1 == "IIB" || golongan1 == "IIC" || golongan1 == "IID") {
                                if (jabatan.toLowerCase().indexOf("karyawan") > -1 || jabatan.toLowerCase().indexOf("kary") > -1 || jabatan.toLowerCase().indexOf("jtu") > -1 || jabatan.toLowerCase().indexOf("mandor") > -1 || jabatan.toLowerCase().indexOf("pengemudi") > -1) { jabatan1 = "Gol_II" }
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
                                    if (kmdanmenginap == "Menginap Di Dalam Wilayah Kerja < 50 km") {
                                        if (jenisTujuan == "Dalam Wilayah") {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ")  * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ")*0 * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                        else {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ")*0 * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ")*0 * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                    }
                                    else if (kmdanmenginap == "Menginap Di Dalam Wilayah Kerja > 50 km") {
                                        if (jenisTujuan == "Dalam Wilayah") {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ")  * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") * 2 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ")*0 * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                        else {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ")*0 * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                    }
                                    else if (kmdanmenginap == "Menginap < 50 km") {
                                        if (jenisTujuan == "Dalam Wilayah") {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ")  * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") * 2 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ")*0 * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                        else {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ")*0 * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                    }
                                    else if (kmdanmenginap == "Menginap < 100 km") {
                                        if (jarak != null) {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ")  * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 2 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                        else {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                    }
                                    else if (kmdanmenginap == "Menginap 100-200 km") {
                                        if (jarak != null) {
                                            kondisi1 = "uraianbiaya='Uang Saku 100-200 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ")  * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 2  from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                        else {
                                            kondisi1 = "uraianbiaya='Uang Saku 100-200 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                    }
                                    else if (kmdanmenginap == "Menginap > 200 km") {
                                        if (jarak != null) {
                                            kondisi1 = "uraianbiaya='Uang Saku > 200 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ")  * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 2 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                        else {
                                            kondisi1 = "uraianbiaya='Uang Saku > 200 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                    }
                                    else if (kmdanmenginap == "Tdk Menginap Di Dalam Wilayah Kerja < 50 km") {
                                        if (jenisTujuan == "Dalam Wilayah") {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Malam' or uraianbiaya='Makan Siang'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") *0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ")*0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                        }
                                        else {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Malam' or uraianbiaya='Makan Siang'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ")*0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ")*0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                        }

                                    }
                                    else if (kmdanmenginap == "Tdk Menginap Di Dalam Wilayah Kerja > 50 km") {
                                        if (jenisTujuan == "Dalam Wilayah") {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Malam' or uraianbiaya='Makan Siang'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") *0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                        }
                                        else {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Malam' or uraianbiaya='Makan Siang'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ")*0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                        }

                                    }
                                    else if (kmdanmenginap == "Tdk Menginap < 50 km") {
                                        if (jenisTujuan == "Dalam Wilayah") {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Malam' or uraianbiaya='Makan Siang'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") *0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                        }
                                        else {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Malam' or uraianbiaya='Makan Siang'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ")*0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                        }

                                    }
                                    else if (kmdanmenginap == "Tdk Menginap < 100 km") {
                                        
                                    kondisi1 = "uraianbiaya='Uang Saku < 100 km'"
                                    kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Malam' or uraianbiaya='Makan Siang'";
                                    cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";                             
                                    }
                                    else if (kmdanmenginap == "Tdk Menginap 100-200 km") {
                                            kondisi1 = "uraianbiaya='Uang Saku 100-200 km'"
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Malam'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (kmdanmenginap == "Tdk Menginap > 200 km") {
                                            kondisi1 = "uraianbiaya='Uang Saku > 200 km'"
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Malam' or uraianbiaya='Makan Siang'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                    }
                                }
                                else if (dapatPenginapan == 1) {
                                    if (kmdanmenginap == "Menginap Di Dalam Wilayah Kerja < 50 km") {
                                        if (jenisTujuan == "Dalam Wilayah") {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ")  * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ")*0 * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                        else {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ")*0 * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ")*0 * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                    }
                                    else if (kmdanmenginap == "Menginap Di Dalam Wilayah Kerja > 50 km") {
                                        if (jenisTujuan == "Dalam Wilayah") {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ")  * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") * 2 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ")*0 * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                        else {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ")*0 * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                    }
                                    else if (kmdanmenginap == "Menginap < 50 km") {
                                        if (jenisTujuan == "Dalam Wilayah") {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ")  * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") * 2 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ")*0 * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                        else {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ")*0 * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                    }
                                    else if (kmdanmenginap == "Menginap < 100 km") {
                                        if (jarak != null) {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            kondisi4 = "uraianbiaya='Biaya Penginapan'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ")  * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 2 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                        else {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            kondisi4 = "uraianbiaya='Biaya Penginapan'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                    }
                                    else if (kmdanmenginap == "Menginap 100-200 km") {
                                        if (jarak != null) {
                                            kondisi1 = "uraianbiaya='Uang Saku 100-200 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            kondisi4 = "uraianbiaya='Biaya Penginapan'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ")  * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 2  from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                        else {
                                            kondisi1 = "uraianbiaya='Uang Saku 100-200 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            kondisi4 = "uraianbiaya='Biaya Penginapan'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                    }
                                    else if (kmdanmenginap == "Menginap > 200 km") {
                                        if (jarak != null) {
                                            kondisi1 = "uraianbiaya='Uang Saku > 200 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            kondisi4 = "uraianbiaya='Biaya Penginapan'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ")  * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 2  from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                        else {
                                            kondisi1 = "uraianbiaya='Uang Saku > 200 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Siang' or uraianbiaya='Makan Malam'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            kondisi4 = "uraianbiaya='Biaya Penginapan'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                    }
                                    else if (kmdanmenginap == "Tdk Menginap Di Dalam Wilayah Kerja < 50 km") {
                                        if (jenisTujuan == "Dalam Wilayah") {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Malam' or uraianbiaya='Makan Siang'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") *0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ")*0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                        }
                                        else {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Malam' or uraianbiaya='Makan Siang'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ")*0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ")*0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                        }

                                    }
                                    else if (kmdanmenginap == "Tdk Menginap Di Dalam Wilayah Kerja > 50 km") {
                                        if (jenisTujuan == "Dalam Wilayah") {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Malam' or uraianbiaya='Makan Siang'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") *0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                        }
                                        else {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Malam' or uraianbiaya='Makan Siang'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ")*0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                        }

                                    }
                                    else if (kmdanmenginap == "Tdk Menginap < 50 km") {
                                        if (jenisTujuan == "Dalam Wilayah") {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Malam' or uraianbiaya='Makan Siang'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") *0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                        }
                                        else {
                                            kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                            kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Malam' or uraianbiaya='Makan Siang'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ")*0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                        }

                                    }
                                    else if (kmdanmenginap == "Tdk Menginap < 100 km") {
                                        kondisi1 = "uraianbiaya='Uang Saku < 100 km'"
                                        kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Malam' or uraianbiaya='Makan Siang'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (kmdanmenginap == "Tdk Menginap 100-200 km") {                                        
                                        kondisi1 = "uraianbiaya='Uang Saku 100-200 km'"
                                        kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Malam' or uraianbiaya='Makan Siang'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";                                      
                                    }
                                    else if (kmdanmenginap == "Tdk Menginap > 200 km") {
                                        kondisi1 = "uraianbiaya='Uang Saku > 200 km'"
                                        kondisi2 = "uraianbiaya='Makan Pagi' or uraianbiaya='Makan Malam' or uraianbiaya='Makan Siang'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                    }
                                }



                            }
                            else if (jenisTujuan == 'Luar Wilayah > 100 km') {
                                if (kmdanmenginap == "Menginap < 100 km" || kmdanmenginap == "Menginap 100-200 km" || kmdanmenginap == "Menginap > 100 km") {
                                    if (dapatPenginapan == 1) {
                                        kondisi1 = "uraianbiaya='Uang Saku'";
                                        kondisi2 = "uraianbiaya='Uang Makan'";
                                        kondisi3 = "uraianbiaya='Biaya Cucian'";
                                        kondisi4 = "uraianbiaya='Biaya Penginapan'";
                                        kondisi5 = "uraianbiaya='Biaya Transport Lokal'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi3 + ")  group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi4 + ")  group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi5 + ")  group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (dapatPenginapan == 0) {
                                        kondisi1 = "uraianbiaya='Uang Saku'";
                                        kondisi2 = "uraianbiaya='Uang Makan'";
                                        kondisi3 = "uraianbiaya='Biaya Cucian'";
                                        kondisi4 = "uraianbiaya='Biaya Transport Lokal'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi3 + ")  group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                    }
                                }
                                else {
                                    kondisi1 = "uraianbiaya='Uang Saku'"
                                    kondisi2 = "uraianbiaya='Uang Makan'"
                                    kondisi3 = "uraianbiaya='Biaya Transport Lokal'";
                                    cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi3 + ") group by uraianbiaya order by uraianbiaya";
                                }
                            }
                            else if (jenisTujuan == 'Luar Wilayah < 100 km') {
                                if (kmdanmenginap == "Menginap < 100 km" || kmdanmenginap == "Menginap 100-200 km" || kmdanmenginap == "Menginap > 200 km") {
                                    if (dapatPenginapan == 1) {
                                        kondisi1 = "uraianbiaya='Uang Saku'";
                                        kondisi2 = "uraianbiaya='Uang Makan'";
                                        kondisi3 = "uraianbiaya='Biaya Cucian'";
                                        kondisi4 = "uraianbiaya='Biaya Penginapan'";
                                        kondisi5 = "uraianbiaya='Biaya Transport Lokal'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi3 + ")  group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi4 + ")  group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi5 + ")  group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (dapatPenginapan == 0) {
                                        kondisi1 = "uraianbiaya='Uang Saku'";
                                        kondisi2 = "uraianbiaya='Uang Makan'";
                                        kondisi3 = "uraianbiaya='Biaya Cucian'";
                                        kondisi4 = "uraianbiaya='Biaya Transport Lokal'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi3 + ")  group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                    }
                                }
                                else {
                                    kondisi1 = "uraianbiaya='Uang Saku'"
                                    kondisi2 = "uraianbiaya='Uang Makan'"
                                    kondisi3 = "uraianbiaya='Biaya Transport Lokal'";
                                    cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi3 + ") group by uraianbiaya order by uraianbiaya";
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
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
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
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi4 + ")  group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi5 + ")  group by uraianbiaya order by uraianbiaya";
                                }
                            }


                            HitungTaripBPD(cSQL).done(function (data) {
                                if (jenisTujuan == 'Luar Wilayah < 100 km' || jenisTujuan == 'Luar Wilayah > 100 km') {
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
                                        dataItem["UangTransport"] = parseFloat(data[1][1]);
                                        dataItem["UangMakan"] = parseFloat(data[2][1]);
                                        dataItem["UangSaku"] = parseFloat(data[3][1]);
                                        dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]) + parseFloat(data[3][1]);
                                        dataItem["UangPenginapan"] = 0;
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
                                else if (jenisTujuan == 'Dalam Wilayah' || jenisTujuan == "Dalam Wilayah Kerja PTPN VIII" || jenisTujuan == "Dalam Wilayah Prov. Jabar Banten") {
                                    if (data.length == 6) {
                                        dataItem.dirty = true;
                                        dataItem["UangCucian"] = parseFloat(data[0][1]);
                                        dataItem["UangPenginapan"] = parseFloat(data[1][1]);
                                        dataItem["UangMakan"] = parseFloat(data[2][1]) + parseFloat(data[3][1]) + parseFloat(data[4][1]);
                                        dataItem["UangSaku"] = parseFloat(data[5][1]);
                                        dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]) + parseFloat(data[3][1]) + parseFloat(data[4][1]) + parseFloat(data[5][1]);
                                        dataItem["UangTransport"] = 0;

                                    }
                                    else if (data.length == 5) {
                                        if (dapatPenginapan == 0)
                                        {
                                            dataItem.dirty = true;
                                            dataItem["UangCucian"] = parseFloat(data[0][1]);
                                            //dataItem["UangPenginapan"] = parseFloat(data[1][1]);
                                            dataItem["UangMakan"] = parseFloat(data[1][1]) + parseFloat(data[2][1]) + parseFloat(data[3][1]);
                                            dataItem["UangSaku"] = parseFloat(data[4][1]);
                                            //dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]) + parseFloat(data[3][1]) + parseFloat(data[4][1]);
                                            dataItem["JumlahBPD"] = parseFloat(data[0][1]) + 0 + parseFloat(data[2][1]) + parseFloat(data[3][1]) + parseFloat(data[4][1]);
                                            dataItem["UangPenginapan"] = 0;
                                            dataItem["UangTransport"] = 0;
                                            dataItem["UangLain"] = 0;
                                        }
                                        else if (dapatPenginapan == 1) {
                                            dataItem.dirty = true;
                                            dataItem["UangCucian"] = parseFloat(data[0][1]);
                                            //dataItem["UangPenginapan"] = parseFloat(data[1][1]);
                                            dataItem["UangPenginapan"] = 0;
                                            dataItem["UangMakan"] = parseFloat(data[1][1]) + parseFloat(data[2][1]) + +parseFloat(data[3][1]);
                                            dataItem["UangSaku"] = parseFloat(data[4][1]);
                                            //dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]) + parseFloat(data[3][1]) + parseFloat(data[4][1]);
                                            dataItem["JumlahBPD"] = parseFloat(data[0][1]) + 0 + parseFloat(data[2][1]) + parseFloat(data[3][1]) + parseFloat(data[4][1]);
                                            dataItem["UangTransport"] = 0;
                                        }
                                        else {
                                            dataItem.dirty = true;
                                            dataItem["UangCucian"] = parseFloat(data[0][1]);
                                            //dataItem["UangPenginapan"] = parseFloat(data[1][1]);
                                            dataItem["UangMakan"] = parseFloat(data[1][1]) + parseFloat(data[2][1]) + parseFloat(data[3][1]);
                                            dataItem["UangSaku"] = parseFloat(data[4][1]);
                                            //dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]) + parseFloat(data[3][1]) + parseFloat(data[4][1]);
                                            dataItem["JumlahBPD"] = parseFloat(data[0][1]) + 0 + parseFloat(data[2][1]) + parseFloat(data[3][1]) + parseFloat(data[4][1]);
                                            dataItem["UangPenginapan"] = 0;
                                            dataItem["UangTransport"] = 0;
                                        }

                                    }
                                    else if (data.length == 4) {
                                        if (jarak != null) {
                                            dataItem.dirty = true;
                                            dataItem["UangCucian"] = 0 ;
                                            dataItem["UangMakan"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]);
                                            dataItem["UangSaku"] = parseFloat(data[3][1]);
                                            dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]) + parseFloat(data[3][1]);
                                            dataItem["UangTransport"] = 0;
                                            dataItem["UangPenginapan"] = 0;
                                        }
                                        else {
                                            dataItem.dirty = true;
                                            dataItem["UangMakan"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]);
                                            dataItem["UangSaku"] = parseFloat(data[3][1]);
                                            dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]) + parseFloat(data[3][1]);
                                            dataItem["UangCucian"] = 0;
                                            dataItem["UangTransport"] = 0;
                                            dataItem["UangPenginapan"] = 0;

                                        }
                                    }
                                    else if (data.length == 3) {
                                        dataItem.dirty = true;
                                        dataItem["UangMakan"] = parseFloat(data[0][1]) + parseFloat(data[1][1]);
                                        dataItem["UangSaku"] = parseFloat(data[2][1]);
                                        dataItem["UangCucian"] = 0;
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
                            dataItem.Chop = '';
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

function cekNamaKebun(namaKebun) {
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select WilayahId from [ReferensiEF].[dbo].[Wilayah]" +
                " where WilayahId=(select posisilokasikerjaid from [Identity].[dbo].[AspNetUsers] where UserName ='" + usrName + "' )",
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
                " join [ReferensiEF].[dbo].[Wilayah] as B on A.kd_kbn=(Select kd_kbn from [ReferensiEF].dbo.Wilayah where WilayahId = (select posisilokasikerjaid from [Identity].[dbo].[AspNetUsers] where UserName ='" + usrName + "' )) " +
                " where A.reg='" + dataItem.NIK + "' and Year(A.Tanggal)=" + year + " and Month(A.Tanggal)=" + month + "" +
                " union select top 1 A.Nama, '' as Gol, '' as MK, '' as Nama_Jab from [SPDK_KanpusEF].[dbo].[Ref_DikKLM] as A " +
                " join [ReferensiEF].[dbo].[Wilayah] as B on A.kd_kbn=(Select kd_kbn from [ReferensiEF].dbo.Wilayah where WilayahId = (select posisilokasikerjaid from [Identity].[dbo].[AspNetUsers] where UserName ='" + usrName + "' )) " +
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
    kmdanmenginap = model.KMdanMenginap;
    dapatPenginapan = model.DapatPenginapan;
    jumlahMakan = model.JumlahMakan;

    var berangkat = model.Berangkat;
    var kembali = model.Kembali;
    var dateStringBerangkat = berangkat.getUTCFullYear() + "/" + (berangkat.getUTCMonth() + 1) + "/" + (berangkat.getUTCDate() + 1)
    var dateStringKembali = kembali.getUTCFullYear() + "/" + (kembali.getUTCMonth() + 1) + "/" + (kembali.getUTCDate() + 1)

    var hariAkhirPisah = dateStringKembali.split('/');
    var hariAwalPisah = dateStringBerangkat.split('/');
    var objek_tgl = new Date();
    var pisahTanggalAkhir = objek_tgl.setFullYear(parseInt(hariAkhirPisah[0]), parseInt(hariAkhirPisah[1] - 1), parseInt(hariAkhirPisah[2]));
    var pisahTanggalAwal = objek_tgl.setFullYear(parseInt(hariAwalPisah[0]), parseInt(hariAwalPisah[1] - 1), parseInt(hariAwalPisah[2]));

    jumlahHari = (((((pisahTanggalAkhir - pisahTanggalAwal) / 1000) / 60) / 60) / 24);

    if (jarak < 51 && jumlahHari == 0 && DalamWilayahKerja == 1) { kmdanmenginap = "Tdk Menginap Di Dalam Wilayah Kerja < 50 km"; }
    else if (jarak < 51 && jumlahHari > 0 && DalamWilayahKerja == 1) { kmdanmenginap = "Menginap Di Dalam Wilayah Kerja < 50 km"; }
    else if (jarak > 50 && jumlahHari == 0 && DalamWilayahKerja == 1) { kmdanmenginap = "Tdk Menginap Di Dalam Wilayah Kerja > 50 km"; }
    else if (jarak > 50 && jumlahHari > 0 && DalamWilayahKerja == 1) { kmdanmenginap = "Menginap Di Dalam Wilayah Kerja > 50 km"; }
    else if (jarak < 51 && jumlahHari == 0) { kmdanmenginap = "Tdk Menginap < 50 km"; }
    else if (jarak < 51 && jumlahHari > 0) { kmdanmenginap = "Menginap < 50 km"; }
    else if (jarak < 100 && jumlahHari == 0)
    { kmdanmenginap = "Tdk Menginap < 100 km" }
    else if (jarak > 99 && jarak < 200 && jumlahHari == 0)
    { kmdanmenginap = "Tdk Menginap 100-200 km" }
    else if (jarak > 199 && jumlahHari == 0)
    { kmdanmenginap = "Tdk Menginap > 200 km" }
    else if (jarak < 100 && jumlahHari > 0)
    { kmdanmenginap = "Menginap < 100 km" }
    else if (jarak > 99 && jarak < 200 && jumlahHari > 0)
    { kmdanmenginap = "Menginap 100-200 km" }
    else if (jarak > 199 && jumlahHari > 0)
    { kmdanmenginap = "Menginap > 200 km" }
    else if (jarak == null && jumlahHari > 0)
    { kmdanmenginap = "Menginap < 100 km" }
    else if (jarak == null && jumlahHari == 0)
    { kmdanmenginap = "Tdk Menginap < 100 km" }
    else if (jarak == null && jumlahHari == 0 && jenisTujuan == "Luar Wilayah < 100 km") { kmdanmenginap = "Tdk Menginap < 100 km" }
    else if (jarak == null && jumlahHari > 0 && jenisTujuan == "Luar Wilayah < 100 km") { kmdanmenginap = "Menginap < 100 km" }
    else if (jarak == null && jumlahHari == 0 && jenisTujuan == "Luar Wilayah > 100 km") { kmdanmenginap = "Tdk Menginap > 100 km" }
    else if (jarak == null && jumlahHari > 0 && jenisTujuan == "Luar Wilayah > 100 km") { kmdanmenginap = "Menginap > 100 km" }
    else if (tujuan == "Jakarta" && (jumlahHari == 0 || jumlahHari > 0))
    { kmdanmenginap = "Jakarta" }

    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(alokasiPengemudiKebunId)) == "undefined" ? grid.dataSource.getByUid(alokasiPengemudiKebunId) : grid.dataSource.get(alokasiPengemudiKebunId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.KMdanMenginap = kmdanmenginap;

    $(e.container).find("input[name='UangSaku']").prop('disabled', true);
    $(e.container).find("input[name='UangMakan']").prop('disabled', true);
    $(e.container).find("input[name='UangCucian']").prop('disabled', true);
    $(e.container).find("input[name='UangTransport']").prop('disabled', true);
    $(e.container).find("input[name='JumlahBPD']").prop('disabled', true);
    if (jenisTujuan == "Dalam Wilayah") {
        $(e.container).find("input[name='Tujuan']").prop('disabled', true);
    }
    else if (jenisTujuan == "Luar Wilayah") {
        $(e.container).find("input[name='TujuanDalamWilayah']").prop('disabled', true);
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
        id: berangkat,
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
    var jenisTujuan = $('#JenisTujuan').val();
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

function filterAsal() {
    return {
        //id: IdBudidaya,
        value: $('#AsalDalamWilayah').val()
    };
}

function aucLokasiKerjaAsalOnSelect(e) {
    var ddlItem = this.dataItem(e.item);
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(alokasiPengemudiKebunId)) == "undefined" ? grid.dataSource.getByUid(alokasiPengemudiKebunId) : grid.dataSource.get(alokasiPengemudiKebunId);
    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
    var data = grid.dataItem(row);
    data.AsalId = ddlItem.LokasiKerjaId;
    asalId = data.AsalId;
    data.AsalDalamWilayah = ddlItem.Nama;
    grid.refresh();
}

function aucLokasiKerjaAsalOnDataBound(e) {

}

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
    grid.refresh();
    tujuandalamwilayah = data.TujuanDalamWilayah;
    getDataFrom().done(function (data) {
        if (data != null) {
            jarak = data[0][0];
            if (jarak < 51) {
                CekWilayahKerja().done(function (data) {
                    if (data != 0) {
                        DalamWilayahKerja = 1;
                    }
                    else if (data == 0) {
                        DalamWilayahKerja = 0;
                    }
                })
            }
            else if (jarak > 50) {
                CekWilayahKerja().done(function (data) {
                    if (data != 0) {
                        DalamWilayahKerja = 1;
                    }
                    else if (data == 0) {
                        DalamWilayahKerja = 0;
                    }
                })
            }
        }
    })
}

function CekWilayahKerja() {
    // Hitung Jumlah Record di Detail
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select count(WilayahId) as Data from [ReferensiEF].[dbo].[Kebun]" +
                " where WilayahId='" + asalId + "' and KebunId ='" + tujuanId + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
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

function aucLokasiKerjaTujuanOnDataBound(e) {

}