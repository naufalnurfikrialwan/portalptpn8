var wndLeaveGrid, wnd, wndDetail, prt, err, del;
var hdr_PermintaanPerjalananDinasKebunId, kebunId, dtl_PermintaanPerjalananDinasKebunId, reg, berangkat;
var _NomorInputView;
var model;
var headerBaru, detailBaru;
var rowNumber = 0;
var menuId = '1bdd62f8-d9a0-4b5d-89b7-8cc73310e1e3';
var tahunPermintaan, bulanPermintaan, jarak;
var statusKMdanMenginap, jumlahHari2;
var jenistujuan2;
var lokasiKerjaAsalId;
var lokasiKerjaTujuanId,namatujuan;

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

    ceknik = $("#ceknikWindow").kendoWindow({
        title: "Cari NIK",
        modal: true,
        visible: false,
        resizable: false,
        width: 700,
        height: 450
    }).data("kendoWindow");

    cekStatusKM = $("#cekStatusKM").kendoWindow({
        title: "Warning",
        modal: true,
        visible: false,
        resizable: false,
        width: 700,
        height: 250
    }).data("kendoWindow");

    cekkm = $("#cekkmWindow").kendoWindow({
        title: "Cek Kilometer",
        modal: true,
        visible: false,
        resizable: false,
        width: 500,
        height: 300
    }).data("kendoWindow");

    gridStatus = 'belum-ngapa-ngapain';


    // copykan ke semua view 22/09/16 16:40
    var golongan;
    $("#grdDetail").kendoValidator({
        rules: { // custom rules
            validation: function (input) {
                if (input.attr("name") == "NIK" || input.attr("name") == "Nama" || input.attr("name") == "Golongan" || input.attr("name") == "Jabatan") {
                    cekNIK($('#NIK').val()).done(function (data) {
                        if (data.length > 0) {
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_PermintaanPerjalananDinasKebunId)) == "undefined" ? grid.dataSource.getByUid(dtl_PermintaanPerjalananDinasKebunId) : grid.dataSource.get(dtl_PermintaanPerjalananDinasKebunId);
                            dataItem.Nama = data[0][0];
                            $('#Nama').val(data[0][0]);
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
                            dataItem.Golongan = golongan + "  " + data[0][2];
                            $('#Golongan').val(golongan + "  " + data[0][2]);
                            dataItem.Jabatan = data[0][3];
                            $('#Jabatan').val(data[0][3]);


                            var golongan1 = golongan;
                            var jabatan = dataItem["Jabatan"];
                            var jabatan1;

                            var jenisTujuan = jenistujuan2;

                            if (jenisTujuan == "Dalam Wilayah")
                            { var kmdanmenginap = statusKMdanMenginap; }
                            else if (jenisTujuan == "Luar Wilayah < 100 km")
                            {
                                if (jumlahHari2 == 0){ var kmdanmenginap = "Tdk Menginap < 100 km"; }
                                else if (jumlahHari2 > 0) { var kmdanmenginap = "Menginap < 100 km"; }
                            }
                            else if (jenisTujuan == "Luar Wilayah > 100 km")
                            {
                                if (jumlahHari2 == 0) { var kmdanmenginap = "Tdk Menginap > 100 km"; }
                                else if (jumlahHari2 > 0) { var kmdanmenginap = "Menginap > 100 km"; }
                            }
                            else if (jenisTujuan == "Jakarta PP")
                            { var kmdanmenginap = "Jakarta"; }
                            else if (jenisTujuan == "Jakarta Nginap")
                            { var kmdanmenginap = "Jakarta"; }

                            var dapatBiayaPenginapan = $('#DapatBiayaPenginapan').val();
                            var kendaraanDinas = $('#KendaraanDinas').val();
                            var jumlahMakan = $('#JumlahMakan').val();

                            var hariAkhir = $('#Kembali').val();
                            var hariAwal = $('#Berangkat').val();

                            var hariAkhirPisah = hariAkhir.split('/');
                            var hariAwalPisah = hariAwal.split('/');

                            var objek_tgl = new Date();

                            var pisahTanggalAkhir = objek_tgl.setFullYear(parseInt(hariAkhirPisah[2]), parseInt(hariAkhirPisah[1] - 1), parseInt(hariAkhirPisah[0]));
                            var pisahTanggalAwal = objek_tgl.setFullYear(parseInt(hariAwalPisah[2]), parseInt(hariAwalPisah[1] - 1), parseInt(hariAwalPisah[0]));

                            var jumlahHari;
                            var dayberangkat = parseInt(hariAwalPisah[0]);
                            var daypulang = parseInt(hariAkhirPisah[0]);
                            var monthberangkat = parseInt(hariAwalPisah[1]);
                            var monthpulang = parseInt(hariAkhirPisah[1]);

                            if ((dayberangkat == daypulang) && (monthberangkat == monthpulang)) {
                                jumlahHari = 0;
                            }
                            else {
                                jumlahHari = (((((pisahTanggalAkhir - pisahTanggalAwal) / 1000) / 60) / 60) / 24);
                            }

                            if (jabatan == "DIREKTUR UTAMA") { jabatan1 = "Dirut" }
                            else if (jabatan == "KOMISARIS UTAMA") { jabatan1 = "Dirut" }
                            else if (jabatan == "DIREKTUR MANAJEMEN ASET") { jabatan1 = "Direksi" }
                            else if (jabatan == "DIREKTUR KOMERSIL") { jabatan1 = "Direksi" }
                            else if (jabatan == "DIREKTUR OPERASIONAL") { jabatan1 = "Direksi" }
                            else if (jabatan == "KOMISARIS") { jabatan1 = "Direksi" }
                            else if (jabatan.toLowerCase().indexOf("kepala bagian") > -1) { jabatan1 = "PJP" }
                            else if (jabatan.toLowerCase().indexOf("administratur") > -1) { jabatan1 = "PJP" }
                            else if (jabatan.toLowerCase().indexOf("kepala divisi") > -1) { jabatan1 = "PJP" }
                            else if (jabatan.toLowerCase().indexOf("kadiv") > -1) { jabatan1 = "PJP" }
                            else if (jabatan == "MANAJER") { jabatan1 = "PJP" }
			                else if (jabatan.toLowerCase().indexOf("manajer") > -1) { jabatan1 = "PJP" }
                            else if (jabatan.toLowerCase().indexOf("manager") > -1) { jabatan1 = "PJP" }
                            else if (jabatan == "ADMINISTRATUR") { jabatan1 = "PJP" }
                            else if (jabatan == "Pjs.MANAJER") { jabatan1 = "PJP" }
                            else if (jabatan == "MANAJER AGROWISATA") { jabatan1 = "PJP" }
                            else if (jabatan == "MANAJER INDUSTRI HILIR TEH") { jabatan1 = "PJP" }
                            else if (jabatan == "MANAJER ANEKA USAHA") { jabatan1 = "PJP" }
                            else if (jabatan.toLowerCase().indexOf("Staf Setara Manajer") > -1) { jabatan1 = "PJP" }
                            else if (jabatan.toLowerCase().indexOf("kasubag") > -1) { jabatan1 = "Kaur" }
                            else if (jabatan.toLowerCase().indexOf("kasubdiv") > -1) { jabatan1 = "Kaur" }
                            else if (jabatan.toLowerCase().indexOf("kepala sub divisi") > -1) { jabatan1 = "Kaur" }
                            else if (jabatan.toLowerCase().indexOf("kepala subdivisi") > -1) { jabatan1 = "Kaur" }
                            else if (jabatan.toLowerCase().indexOf("kepala bidang") > -1) { jabatan1 = "Kaur" }
                            else if (jabatan.toLowerCase().indexOf("asisten manajer") > -1) { jabatan1 = "Kaur" }
                            else if (jabatan.toLowerCase().indexOf("pymt.kepala tanaman") > -1) { jabatan1 = "Kaur" }
                            else if (jabatan.toLowerCase().indexOf("asisten kepala") > -1) { jabatan1 = "Kaur" }
                            else if (jabatan.toLowerCase().indexOf("asisten keuangan") > -1) { jabatan1 = "Kaur" }
                            else if (jabatan.toLowerCase().indexOf("masinis") > -1) { jabatan1 = "Kaur" }

                            if (golongan1 == "IA" || golongan1 == "IB" || golongan1 == "IC" || golongan1 == "ID") {
                                //if (jabatan.toLowerCase().indexOf("karyawan") > -1 || jabatan.toLowerCase().indexOf("kary") > -1 || jabatan.toLowerCase().indexOf("jtu") > -1 || jabatan.toLowerCase().indexOf("mandor") > -1 || jabatan.toLowerCase().indexOf("tabin") > -1 || jabatan == "")
                                { jabatan1 = "Gol_I" }
                            }
                            else if (golongan1 == "IIA" || golongan1 == "IIB" || golongan1 == "IIC" || golongan1 == "IID") {
                                //if (jabatan.toLowerCase().indexOf("karyawan") > -1 || jabatan.toLowerCase().indexOf("kary") > -1 || jabatan.toLowerCase().indexOf("jtu") > -1 || jabatan.toLowerCase().indexOf("mandor") > -1 || jabatan.toLowerCase().indexOf("tabin") > -1 || jabatan.toLowerCase().indexOf("asisten administrasi") > -1)
                                { jabatan1 = "Gol_II" }
                            }
                            else if (golongan1 == "IIIA" || golongan1 == "IIIB" || golongan1 == "IIIC" || golongan1 == "IIID") {
                                if (jabatan.toLowerCase().indexOf("staf") > -1 || jabatan.toLowerCase().indexOf("admin") > -1 || jabatan.toLowerCase().indexOf("asisten tanaman") > -1 || jabatan.toLowerCase().indexOf("asisten sdm") > -1 || jabatan.toLowerCase().indexOf("asisten personalia kebun") > -1 || jabatan.toLowerCase().indexOf("krani") > -1 || jabatan.toLowerCase().indexOf("asisten afdeling") > -1 || jabatan.toLowerCase().indexOf("asisten sub pengolahan") > -1 || jabatan.toLowerCase().indexOf("asisten pengolahan") > -1 || jabatan.toLowerCase().indexOf("asisten administrasi") > -1 || jabatan.toLowerCase().indexOf("asisten tata usaha") > -1 || jabatan.toLowerCase().indexOf("asisten teknik") > -1 || jabatan.toLowerCase().indexOf("kepala pengolahan") > -1 || jabatan.toLowerCase().indexOf("kepala pengembangan") > -1 || jabatan.toLowerCase().indexOf("kepala administrasi") > -1) { jabatan1 = "Gol_III" }
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
                            if (jenisTujuan == "Dalam Wilayah") {
                                if (dapatBiayaPenginapan == 0) {
                                    if (kmdanmenginap == "Menginap < 50 km") {
                                        kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                        kondisi2 = "uraianbiaya='Makan Pagi'";
                                        kondisi3 = "uraianbiaya='Biaya Cucian'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahMakan + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (kmdanmenginap == "Menginap < 100 km") {
                                        kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                        kondisi2 = "uraianbiaya='Makan Pagi'";
                                        kondisi3 = "uraianbiaya='Biaya Cucian'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahMakan + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (kmdanmenginap == "Menginap 100-200 km") {
                                        kondisi1 = "uraianbiaya='Uang Saku 100-200 km'";
                                        kondisi2 = "uraianbiaya='Makan Pagi'";
                                        kondisi3 = "uraianbiaya='Biaya Cucian'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahMakan + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (kmdanmenginap == "Menginap > 200 km") {
                                        kondisi1 = "uraianbiaya='Uang Saku > 200 km'";
                                        kondisi2 = "uraianbiaya='Makan Pagi'";
                                        kondisi3 = "uraianbiaya='Biaya Cucian'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahMakan + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (kmdanmenginap == "Tdk Menginap < 50 km") {
                                        kondisi1 = "uraianbiaya='Uang Saku < 100 km'"
                                        kondisi2 = "uraianbiaya='Makan Pagi'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahMakan + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (kmdanmenginap == "Tdk Menginap < 100 km") {
                                        kondisi1 = "uraianbiaya='Uang Saku < 100 km'"
                                        kondisi2 = "uraianbiaya='Makan Pagi'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahMakan + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (kmdanmenginap == "Tdk Menginap 100-200 km") {
                                        kondisi1 = "uraianbiaya='Uang Saku 100-200 km'"
                                        kondisi2 = "uraianbiaya='Makan Pagi'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahMakan + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (kmdanmenginap == "Tdk Menginap > 200 km") {
                                        kondisi1 = "uraianbiaya='Uang Saku > 200 km'"
                                        kondisi2 = "uraianbiaya='Makan Pagi'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahMakan + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                    }
                                }
                                else if (dapatBiayaPenginapan == 1) {
                                    if (kmdanmenginap == "Menginap < 50 km") {
                                        kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                        kondisi2 = "uraianbiaya='Makan Pagi'";
                                        kondisi3 = "uraianbiaya='Biaya Cucian'";
                                        kondisi4 = "uraianbiaya='Biaya Penginapan'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahMakan + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi3 + ")  group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 0 * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (kmdanmenginap == "Menginap < 100 km") {
                                        kondisi1 = "uraianbiaya='Uang Saku < 100 km'";
                                        kondisi2 = "uraianbiaya='Makan Pagi'";
                                        kondisi3 = "uraianbiaya='Biaya Cucian'";
                                        kondisi4 = "uraianbiaya='Biaya Penginapan'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahMakan + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi3 + ")  group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * 0 * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (kmdanmenginap == "Menginap 100-200 km") {
                                        kondisi1 = "uraianbiaya='Uang Saku 100-200 km'";
                                        kondisi2 = "uraianbiaya='Makan Pagi'";
                                        kondisi3 = "uraianbiaya='Biaya Cucian'";
                                        kondisi4 = "uraianbiaya='Biaya Penginapan'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ")  * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahMakan + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi3 + ")  group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * 0 * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (kmdanmenginap == "Menginap > 200 km") {
                                        kondisi1 = "uraianbiaya='Uang Saku > 200 km'";
                                        kondisi2 = "uraianbiaya='Makan Pagi'";
                                        kondisi3 = "uraianbiaya='Biaya Cucian'";
                                        kondisi4 = "uraianbiaya='Biaya Penginapan'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ")  * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahMakan + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi3 + ")  group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * 0 * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (kmdanmenginap == "Tdk Menginap < 50 km") {
                                        kondisi1 = "uraianbiaya='Uang Saku < 100 km'"
                                        kondisi2 = "uraianbiaya='Makan Pagi'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahMakan + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (kmdanmenginap == "Tdk Menginap < 100 km") {
                                        kondisi1 = "uraianbiaya='Uang Saku < 100 km'"
                                        kondisi2 = "uraianbiaya='Makan Pagi'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahMakan + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (kmdanmenginap == "Tdk Menginap 100-200 km") {
                                        kondisi1 = "uraianbiaya='Uang Saku 100-200 km'"
                                        kondisi2 = "uraianbiaya='Makan Pagi'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahMakan + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (kmdanmenginap == "Tdk Menginap > 200 km") {
                                        kondisi1 = "uraianbiaya='Uang Saku > 200 km'"
                                        kondisi2 = "uraianbiaya='Makan Pagi'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Dalam Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahMakan + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                    }
                                }
                            }
                            else if (jenisTujuan == 'Luar Wilayah < 100 km') {
                                if (kendaraanDinas == 0) {
                                    if (kmdanmenginap == "Menginap < 100 km" || kmdanmenginap == "Menginap 100-200 km" || kmdanmenginap == "Menginap > 200 km") {
                                        if (dapatBiayaPenginapan == 0) {
                                            kondisi1 = "uraianbiaya='Uang Saku'";
                                            kondisi2 = "uraianbiaya='Uang Makan'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            kondisi4 = "uraianbiaya='Biaya Transport Lokal'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi3 + ")  group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                        else if (dapatBiayaPenginapan == 1) {
                                            kondisi1 = "uraianbiaya='Uang Saku'";
                                            kondisi2 = "uraianbiaya='Uang Makan'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            kondisi4 = "uraianbiaya='Biaya Transport Lokal'";
                                            kondisi5 = "uraianbiaya='Biaya Penginapan'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi3 + ")  group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi4 + ")  group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 0 * (" + jumlahHari + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi5 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                    }
                                    else {
                                        kondisi1 = "uraianbiaya='Uang Saku'"
                                        kondisi2 = "uraianbiaya='Uang Makan'"
                                        kondisi3 = "uraianbiaya='Biaya Transport Lokal'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi3 + ") group by uraianbiaya order by uraianbiaya";
                                    }
                                }
                                else if (kendaraanDinas == 1) {
                                    if (kmdanmenginap == "Menginap < 100 km" || kmdanmenginap == "Menginap 100-200 km" || kmdanmenginap == "Menginap > 200 km") {
                                        if (dapatBiayaPenginapan == 0) {
                                            kondisi1 = "uraianbiaya='Uang Saku'";
                                            kondisi2 = "uraianbiaya='Uang Makan'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                        else if (dapatBiayaPenginapan == 1) {
                                            kondisi1 = "uraianbiaya='Uang Saku'";
                                            kondisi2 = "uraianbiaya='Uang Makan'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            kondisi5 = "uraianbiaya='Biaya Penginapan'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi3 + ")  group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 0 * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi5 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                    }
                                    else {
                                        kondisi1 = "uraianbiaya='Uang Saku'"
                                        kondisi2 = "uraianbiaya='Uang Makan'"
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
               
                                    }
                                }
                            }
                            else if (jenisTujuan == 'Luar Wilayah > 100 km') {
                                if (kendaraanDinas == 0) {
                                    if (kmdanmenginap == "Menginap < 100 km" || kmdanmenginap == "Menginap 100-200 km" || kmdanmenginap == "Menginap > 100 km") {
                                        if (dapatBiayaPenginapan == 0) {
                                            kondisi1 = "uraianbiaya='Uang Saku'";
                                            kondisi2 = "uraianbiaya='Uang Makan'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            kondisi4 = "uraianbiaya='Biaya Transport Lokal'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi3 + ")  group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                        else if (dapatBiayaPenginapan == 1) {
                                            kondisi1 = "uraianbiaya='Uang Saku'";
                                            kondisi2 = "uraianbiaya='Uang Makan'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            kondisi4 = "uraianbiaya='Biaya Transport Lokal'";
                                            kondisi5 = "uraianbiaya='Biaya Penginapan'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi3 + ")  group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi4 + ")  group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") * 0 * (" + jumlahHari + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi5 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                    }
                                    else {
                                        kondisi1 = "uraianbiaya='Uang Saku'"
                                        kondisi2 = "uraianbiaya='Uang Makan'"
                                        kondisi3 = "uraianbiaya='Biaya Transport Lokal'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi3 + ") group by uraianbiaya order by uraianbiaya";
                                    }
                                }
                                else if (kendaraanDinas == 1) {
                                    if (kmdanmenginap == "Menginap < 100 km" || kmdanmenginap == "Menginap 100-200 km" || kmdanmenginap == "Menginap > 100 km") {
                                        if (dapatBiayaPenginapan == 0) {
                                            kondisi1 = "uraianbiaya='Uang Saku'";
                                            kondisi2 = "uraianbiaya='Uang Makan'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                        else if (dapatBiayaPenginapan == 1) {
                                            kondisi1 = "uraianbiaya='Uang Saku'";
                                            kondisi2 = "uraianbiaya='Uang Makan'";
                                            kondisi3 = "uraianbiaya='Biaya Cucian'";
                                            kondisi5 = "uraianbiaya='Biaya Penginapan'";
                                            cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi3 + ")  group by uraianbiaya" +
                                                " union select uraianbiaya,sum(" + jabatan1 + ") * 0 * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi5 + ")  group by uraianbiaya order by uraianbiaya";
                                        }
                                    }
                                    else {
                                        kondisi1 = "uraianbiaya='Uang Saku'"
                                        kondisi2 = "uraianbiaya='Uang Makan'"
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi1 + ") group by uraianbiaya" +
                                            " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='Luar Wilayah' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";

                                    }
                                }
                            }
                            else if (jenisTujuan == 'Jakarta PP') {
                                if (kendaraanDinas == 1) {
                                    kondisi1 = "uraianbiaya='Uang Saku'";
                                    kondisi2 = "uraianbiaya='Uang Makan'";
                                    cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                    " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya order by uraianbiaya";
                                }
                                else {
                                    kondisi1 = "uraianbiaya='Uang Saku'";
                                    kondisi2 = "uraianbiaya='Uang Makan'";
                                    kondisi3 = "uraianbiaya='Biaya Transport Lokal'";
                                    cSQL = "select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                    " union select uraianbiaya,sum(" + jabatan1 + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                    " union select uraianbiaya,sum(" + jabatan1 + ") * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                }
                            }
                            else if (jenisTujuan == 'Jakarta Nginap') {
                                if (kendaraanDinas == 1) {
                                    if (dapatBiayaPenginapan == 1) {
                                        kondisi1 = "uraianbiaya='Uang Saku'";
                                        kondisi2 = "uraianbiaya='Uang Makan'";
                                        kondisi3 = "uraianbiaya='Biaya Cucian'";
                                        kondisi4 = "uraianbiaya='Biaya Penginapan'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * 0 * (" + jumlahHari + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (dapatBiayaPenginapan == 0) {
                                        kondisi1 = "uraianbiaya='Uang Saku'";
                                        kondisi2 = "uraianbiaya='Uang Makan'";
                                        kondisi3 = "uraianbiaya='Biaya Cucian'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ")  group by uraianbiaya order by uraianbiaya";
                                    }
                                }
                                else {
                                    if (dapatBiayaPenginapan == 0) {
                                        kondisi1 = "uraianbiaya='Uang Saku'";
                                        kondisi2 = "uraianbiaya='Uang Makan'";
                                        kondisi3 = "uraianbiaya='Biaya Cucian'";
                                        kondisi4 = "uraianbiaya='Biaya Transport Lokal'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi4 + ")  group by uraianbiaya order by uraianbiaya";
                                    }
                                    else if (dapatBiayaPenginapan == 1) {
                                        kondisi1 = "uraianbiaya='Uang Saku'";
                                        kondisi2 = "uraianbiaya='Uang Makan'";
                                        kondisi3 = "uraianbiaya='Biaya Cucian'";
                                        kondisi4 = "uraianbiaya='Biaya Transport Lokal'";
                                        kondisi5 = "uraianbiaya='Biaya Penginapan'";
                                        cSQL = "select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi1 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi2 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + ") from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi3 + ") group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * (" + jumlahHari + "+1) * 0 from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi4 + ")  group by uraianbiaya" +
                                        " union select uraianbiaya,sum(" + jabatan1 + ") * 0 * " + jumlahHari + " from [ReferensiEF].[dbo].[BPD] where StatusWilayah='" + jenisTujuan + "' and (" + kondisi5 + ")  group by uraianbiaya order by uraianbiaya";
                                    }
                                }
                            }


                            HitungTaripBPD(cSQL).done(function (data) {
                                if (jenisTujuan == 'Luar Wilayah > 100 km' || jenisTujuan == 'Luar Wilayah < 100 km') {
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
										if (kendaraanDinas == 1)
                                        {
                                            dataItem.dirty = true;
											dataItem["UangCucian"] = parseFloat(data[0][1]);
											dataItem["UangPenginapan"] = parseFloat(data[1][1]);
											dataItem["UangMakan"] = parseFloat(data[2][1]);
											dataItem["UangSaku"] = parseFloat(data[3][1]);                                    
											dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]) + parseFloat(data[3][1]);
											dataItem["UangTransport"] = 0;

                                        }
                                        else if (kendaraanDinas == 0)
                                        {
                                            dataItem.dirty = true;
											dataItem["UangCucian"] = parseFloat(data[0][1]);
											dataItem["UangTransport"] = parseFloat(data[1][1]);
											dataItem["UangMakan"] = parseFloat(data[2][1]);
											dataItem["UangSaku"] = parseFloat(data[3][1]);                                    
											dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]) + parseFloat(data[3][1]);
											dataItem["UangPenginapan"] = 0;
					
                                        }
                                    }
                                    else if (data.length == 3) {
                                        if (kendaraanDinas == 1)
										{
											dataItem.dirty = true;
											dataItem["UangCucian"] = parseFloat(data[0][1]);
											dataItem["UangMakan"] = parseFloat(data[1][1]);
											dataItem["UangSaku"] = parseFloat(data[2][1]);
											dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]);
											dataItem["UangTransport"] = 0;
											dataItem["UangPenginapan"] = 0;
										}
										else if (kendaraanDinas == 0)
										{
											dataItem.dirty = true;
											dataItem["UangTransport"] = parseFloat(data[0][1]);
											dataItem["UangMakan"] = parseFloat(data[1][1]);
											dataItem["UangSaku"] = parseFloat(data[2][1]);
											dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]);
											dataItem["UangCucian"] = 0;
											dataItem["UangPenginapan"] = 0;
										}
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
                                        dataItem["UangCucian"] = 0;
                                        dataItem["UangTransport"] = 0;
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
                                        if (kendaraanDinas == 1) {
                                            dataItem.dirty = true;
                                            dataItem["UangCucian"] = parseFloat(data[0][1]);
                                            dataItem["UangPenginapan"] = parseFloat(data[1][1]);
                                            dataItem["UangMakan"] = parseFloat(data[2][1]);
                                            dataItem["UangSaku"] = parseFloat(data[3][1]);
                                            dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]) + parseFloat(data[3][1]);
                                            dataItem["UangTransport"] = 0;
                                        }
                                        else if (kendaraanDinas == 0) {
                                            dataItem.dirty = true;
                                            dataItem["UangCucian"] = parseFloat(data[0][1]);
                                            dataItem["UangTransport"] = parseFloat(data[1][1]);
                                            dataItem["UangMakan"] = parseFloat(data[2][1]);
                                            dataItem["UangSaku"] = parseFloat(data[3][1]);
                                            dataItem["JumlahBPD"] = parseFloat(data[0][1]) + parseFloat(data[1][1]) + parseFloat(data[2][1]) + parseFloat(data[3][1]);
                                            dataItem["UangPenginapan"] = 0;
                                        }
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
                                else if (jenisTujuan == "Dalam Wilayah") {
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
                            $('#errMsg').text('NIK belum terdaftar di bagian/unit/kebun anda/Pengemudi tidak diperkenankan diform ini!');
                            openErrWindow();
                            var grid = $('#grdDetail').data('kendoGrid');
                            var dataItem = typeof (grid.dataSource.get(dtl_PermintaanPerjalananDinasKebunId)) == "undefined" ? grid.dataSource.getByUid(dtl_PermintaanPerjalananDinasKebunId) : grid.dataSource.get(dtl_PermintaanPerjalananDinasKebunId);
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

function btnCariKMHeaderOnClick(e) {
    cekkm.open().center();
}

function cekNIKPopUp(e) {

    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    ceknik.open().center();

}

function opencekStatusKM(e) {
    cekStatusKM.open().center();
    $("#sayamengerti").click(function () {
        cekStatusKM.close();
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
    var kd_kbn = $('#Kd_Kbn').val();
    var grid = $('#grdDetail').data('kendoGrid');
    var dataItem = typeof (grid.dataSource.get(dtl_PermintaanPerjalananDinasKebunId)) == "undefined" ? grid.dataSource.getByUid(dtl_PermintaanPerjalananDinasKebunId) : grid.dataSource.get(dtl_PermintaanPerjalananDinasKebunId);

    var tanggalPermintaan = $('#TanggalPermintaan').val();
    var tanggalPermintaanPisah = tanggalPermintaan.split('/');
    var objek_tgl = new Date();

    tahunPermintaan = tanggalPermintaanPisah[2];
    var tahunPermintaanPisah = tahunPermintaan.split(' ');
    tahunPermintaan = tahunPermintaanPisah[0];
    bulanPermintaan = tanggalPermintaanPisah[1];

    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select top 1 A.Nama, A.Gol, A.MK, A.Nama_Jab from [SPDK_KanpusEF].[dbo].[Ref_Dik] as A " +
                " join [ReferensiEF].[dbo].[Kebun] as B on A.kd_kbn=B.kd_kbn" +
                " where B.KebunId='" + kebunId + "' and A.reg='" + dataItem.NIK + "' and Year(A.Tanggal)=" + tahunPermintaan + " and Month(A.Tanggal)=" + bulanPermintaan + " and (A.Nama_Jab !='Pengemudi' or A.Nama_Jab !='Supir' or A.Nama_Jab !='Sopir')" +
                " union select top 1 A.Nama, '' as Gol, '' as MK, ''  as Nama_Jab from [SPDK_KanpusEF].[dbo].[Ref_DikKLM] as A " +
                " join [ReferensiEF].[dbo].[Kebun] as B on A.kd_kbn=B.kd_kbn" +
                " where (A.Nama_Jab !='Pengemudi' or A.Nama_Jab !='Supir' or A.Nama_Jab !='Sopir') and B.KebunId='" + kebunId + "' and A.reg='" + dataItem.NIK + "' and Year(A.Tanggal)=" + tahunPermintaan + " and Month(A.Tanggal)=" + bulanPermintaan,
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

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
                        //alert("Untuk Isi Penumpang Sekarang sudah bisa sekaligus lebih dari 1 orang dalam sekali simpan...");
                        hdr_PermintaanPerjalananDinasKebunId = $('#HDR_PermintaanPerjalananDinasKebunId').val();
                        tanggalPermintaan = $('#TanggalPermintaan').val();
                        kebunId = $('#KebunId').val();
                        $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                            disableHeader();
                            $('#btnDeleteHeader').removeClass('disabledbutton');
                            $('#btnPrintHeader').removeClass('disabledbutton');
                            $('#btnDeleteHeader').attr('disabled', false);
                            $('#btnPrintHeader').attr('disabled', false);
                            $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                            $('#grdDetail').removeClass('disabledbutton');
                            
                                                     
                            $("#grdDetail").data("kendoGrid").dataSource.read({
                                id: $('#HDR_PermintaanPerjalananDinasKebunId').val()
                            });
                        });
                    }
                    else {
                        gridStatus = 'belum-ngapa-ngapain';
                        openErrWindow();
                        $('#grdDetail').addClass('disabledbutton');
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

function getDataFrom() {
    // Hitung Jumlah Record di Detail
    var url = '/Input/ExecSQL';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select top 1 Jarak from [ReferensiEF].[dbo].[Jarak]" +
                " where AsalId='" + $("#AsalId").val() + "' and TujuanId ='" + $("#TujuanId").val() + "'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function ModelToView(_NomorInputView) {
    $('#HDR_PermintaanPerjalananDinasKebunId').val(_NomorInputView.HDR_PermintaanPerjalananDinasKebunId);
    $('#TahunBuku').val(_NomorInputView.TahunBuku);
    $('#KebunId').val(_NomorInputView.KebunId);
    $('#NamaKebun').val(_NomorInputView.NamaKebun);
    $('#NomorInput').val(_NomorInputView.NomorInput);
    $('#NomorInputView').val(_NomorInputView.NomorInputView);
    $('#NomorPermintaan').val(_NomorInputView.NomorPermintaan);
    $('#TanggalPermintaan').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.TanggalPermintaan.substr(6))));
    $('#Berangkat').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.Berangkat.substr(6))));
    $('#Kembali').data('kendoDateTimePicker').value(new Date(parseInt(_NomorInputView.Kembali.substr(6))));
    $('#JenisTujuan').data('kendoDropDownList').value(_NomorInputView.JenisTujuan);
    $('#Tujuan').val(_NomorInputView.Tujuan);
    $('#Keterangan').val(_NomorInputView.Keterangan);
    $('#KendaraanDinas').data('kendoDropDownList').value(_NomorInputView.KendaraanDinas ? "1" : "0");
    $('#NamaDriver').val(_NomorInputView.NamaDriver);
    $('#NopolKendaraan').val(_NomorInputView.NopolKendaraan);
    $('#UserName').val(_NomorInputView.UserName);
    $('#VerKaur').val(_NomorInputView.VerKaur);
    $('#VerKabag').val(_NomorInputView.VerKabag);
    var tglVerKaur = new Date(parseInt(_NomorInputView.TglVerKaur.substr(6)));
    var tglVerKabag = new Date(parseInt(_NomorInputView.TglVerKabag.substr(6)));
    $('#TglVerKaur').val(tglVerKaur.getDate() + "/" + (tglVerKaur.getMonth() + 1) + "/" + tglVerKaur.getFullYear());
    $('#TglVerKabag').val(tglVerKabag.getDate() + "/" + (tglVerKabag.getMonth() + 1) + "/" + tglVerKabag.getFullYear());
    $('#UserNameKaur').val(_NomorInputView.UserNameKaur);
    $('#UserNameKabag').val(_NomorInputView.UserNameKabag);
    $('#AsalId').val(_NomorInputView.AsalId);
    $('#AsalDalamWilayah').val(_NomorInputView.AsalDalamWilayah);
    $('#TujuanId').val(_NomorInputView.TujuanId);
    $('#TujuanDalamWilayah').data('kendoDropDownList').text(_NomorInputView.TujuanDalamWilayah);
    $('#StatusKMdanMenginap').val(_NomorInputView.StatusKMdanMenginap);
    $('#DapatBiayaPenginapan').data('kendoDropDownList').value(_NomorInputView.DapatBiayaPenginapan ? "1" : "0");
    $('#NIKDriver').val(_NomorInputView.NikDriver);
    $('#JumlahHari').val(_NomorInputView.JumlahHari);
    $('#JumlahMakan').val(_NomorInputView.JumlahMakan);
    $('#Jarak').val(_NomorInputView.Jarak);
}

function ViewToModel() {
    var hariAkhir = $('#Kembali').val();
    var hariAwal = $('#Berangkat').val();

    var hariAkhirPisah = hariAkhir.split('/');
    var hariAwalPisah = hariAwal.split('/');
    var jenistujuan = $('#JenisTujuan').val();
    var tujuanDalamWilayah = $('#TujuanDalamWilayah').val();
    var tujuan = $('#Tujuan').val();
    var objek_tgl = new Date();

    var pisahTanggalAkhir = objek_tgl.setFullYear(parseInt(hariAkhirPisah[2]), parseInt(hariAkhirPisah[1] - 1), parseInt(hariAkhirPisah[0]));
    var pisahTanggalAwal = objek_tgl.setFullYear(parseInt(hariAwalPisah[2]), parseInt(hariAwalPisah[1] - 1), parseInt(hariAwalPisah[0]));

    jumlahHari2 = (((((pisahTanggalAkhir - pisahTanggalAwal) / 1000) / 60) / 60) / 24);

    jenistujuan2 = jenistujuan;
    if (jenistujuan == "Jakarta" && jumlahHari2 == 0)
    { jenistujuan2 = "Jakarta PP" }
    else if (jenistujuan == "Jakarta" && jumlahHari2 > 0)
    { jenistujuan2 = "Jakarta Nginap" }
    
    jarak = $('#Jarak').val();

    if (jarak < 51 && jumlahHari2 == 0)
        { statusKMdanMenginap = "Tdk Menginap < 50 km"; }
    else if (jarak < 51 && jumlahHari2 > 0)
        { statusKMdanMenginap = "Menginap < 50 km"; }
    else if (jarak > 50 && jarak < 100 && jumlahHari2 == 0)
        { statusKMdanMenginap = "Tdk Menginap < 100 km" }
    else if (jarak > 99 && jarak < 200 && jumlahHari2 == 0)
        { statusKMdanMenginap = "Tdk Menginap 100-200 km" }
    else if (jarak > 199 && jumlahHari2 == 0)
        { statusKMdanMenginap = "Tdk Menginap > 200 km" }
    else if (jarak > 50 && jarak < 100 && jumlahHari2 > 0)
        { statusKMdanMenginap = "Menginap < 100 km" }
    else if (jarak > 99 && jarak < 200 && jumlahHari2 > 0)
        { statusKMdanMenginap = "Menginap 100-200 km" }
    else if (jarak > 199 && jumlahHari2 > 0)
        { statusKMdanMenginap = "Menginap > 200 km" }
    else if (jarak == null && jumlahHari2 > 0 && namatujuan == "Lain-Lain < 100")
        { statusKMdanMenginap = "Menginap < 100 km" }
    else if (jarak == null && jumlahHari2 == 0 && namatujuan == "Lain-Lain < 100")
        { statusKMdanMenginap = "Tdk Menginap < 100 km" }
    else if (jarak == null && jumlahHari2 > 0 && namatujuan == "Lain-Lain > 200")
        { statusKMdanMenginap = "Menginap > 200 km" }
    else if (jarak == null && jumlahHari2 == 0 && namatujuan == "Lain-Lain > 200")
        { statusKMdanMenginap = "Tdk Menginap > 200 km" }
    else if (jarak == null && jumlahHari2 > 0 && namatujuan == "Lain-Lain 100-200")
        { statusKMdanMenginap = "Menginap 100-200 km" }
    else if (jarak == null && jumlahHari2 == 0 && namatujuan == "Lain-Lain 100-200")
        { statusKMdanMenginap = "Tdk Menginap 100-200 km" }
    else if (jarak == null && jumlahHari2 > 0 && jenistujuan == "Dalam Wilayah")
        { statusKMdanMenginap = "Menginap < 100 km" }
    else if (jarak == null && jumlahHari2 == 0 && jenistujuan == "Dalam Wilayah")
        { statusKMdanMenginap = "Tdk Menginap < 100 km" }
    else if (jarak == null && jumlahHari2 == 0 && jenistujuan == "Luar Wilayah < 100 km")
        { statusKMdanMenginap = "Tdk Menginap < 100 km" }
    else if (jarak == null && jumlahHari2 > 0 && jenistujuan == "Luar Wilayah < 100 km")
        { statusKMdanMenginap = "Menginap < 100 km" }
    else if (jarak == null && jumlahHari2 == 0 && jenistujuan == "Luar Wilayah > 100 km")
        { statusKMdanMenginap = "Tdk Menginap > 100 km" }
    else if (jarak == null && jumlahHari2 > 0 && jenistujuan == "Luar Wilayah > 100 km")
        { statusKMdanMenginap = "Menginap > 100 km" }
    else if (jenistujuan == "Jakarta" && (jumlahHari2 == 0 || jumlahHari2 > 0))
        { statusKMdanMenginap = "Jakarta" }


    var mdl = {
        HDR_PermintaanPerjalananDinasKebunId: $('#HDR_PermintaanPerjalananDinasKebunId').val(),
        TahunBuku: $('#TahunBuku').val(),
        KebunId: $('#KebunId').val(),
        NamaKebun: $('#NamaKebun').val().toUpperCase(),
        NomorInput: $('#NomorInput').val(),
        NomorInputView: Array(5 - String($('#NomorInput').val()).length + 1).join('0') + $('#NomorInput').val() + ' - ' + $('#NomorPermintaan').val().toUpperCase(),
        NomorPermintaan: $('#NomorPermintaan').val().toUpperCase(),
        TanggalPermintaan: $('#TanggalPermintaan').val(),
        Berangkat: $('#Berangkat').val(),
        Kembali: $('#Kembali').val(),
        JenisTujuan: $('#JenisTujuan').data('kendoDropDownList').value(),
        Tujuan: $('#Tujuan').val(),
        Keterangan: $('#Keterangan').val(),
        KendaraanDinas: $('#KendaraanDinas').data('kendoDropDownList').value() == "1" ? "true" : "false",
        NamaDriver: $('#NamaDriver').val(),
        NopolKendaraan: $('#NopolKendaraan').val(),
        UserName: $('#UserName').val(),
        VerKaur: $('#VerKaur').val(),
        VerKabag: $('#VerKabag').val(),
        TglVerKaur: $('#TglVerKaur').val(),
        TglVerKabag: $('#TglVerKabag').val(),
        UserNameKaur: $('#UserNameKaur').val(),
        UserNameKabag: $('#UserNameKabag').val(),
        AsalId: $('#AsalId').val(),
        AsalDalamWilayah: $('#AsalDalamWilayah').val(),
        TujuanId: $('#TujuanId').val(),
        TujuanDalamWilayah: namatujuan,
        StatusKMdanMenginap: statusKMdanMenginap,
        DapatBiayaPenginapan: $('#DapatBiayaPenginapan').data('kendoDropDownList').value() == "1" ? "true" : "false",
        NIKDriver: reg,
        JumlahHari: jumlahHari2,
        JumlahMakan: $('#JumlahMakan').val(),
        Jarak: jarak
    };
    return mdl;
}

function hapusHeader() {
    wnd.close();
    PeriksaConstraintHeader().done(function (data) {
        if (data == 0) {
            ConfirmedHapusHeader().done(function (data) {
                if (data) {
                    hdr_PermintaanPerjalananDinasKebunId = $('#HDR_PermintaanPerjalananDinasKebunId').val();
                    $('#NomorInputView').data('kendoDropDownList').dataSource.read().done(function () {
                        disableHeader();
                        $('#btnDeleteHeader').removeClass('disabledbutton');
                        $('#btnPrintHeader').removeClass('disabledbutton');
                        $('#btnDeleteHeader').attr('disabled', false);
                        $('#btnPrintHeader').attr('disabled', false);
                        $(".k-button-icontext").removeClass("k-state-disabled").addClass("k-grid-add");
                        $('#grdDetail').removeClass('disabledbutton');
                        $("#grdDetail").data("kendoGrid").dataSource.read({
                            id: $('#HDR_PermintaanPerjalananDinasKebunId').val()
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
            scriptSQL: "select count(*) as JmlRecord from [SPDK_KanpusEF].[dbo].[DTL_PermintaanPerjalananDinasKebun]" +
                " where HDR_PermintaanPerjalananDinasKebunId='" + $("#HDR_PermintaanPerjalananDinasKebunId").val() + "'",
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
        hdr_PermintaanPerjalananDinasKebunId = _NomorInputView.HDR_PermintaanPerjalananDinasKebunId;
        kebunId = _NomorInputView.KebunId;
        cekData(_NomorInputView.NomorInputView);
        $('#NomorPermintaan').focus();
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
        parameters: { HDR_PermintaanPerjalananDinasKebunId: $('#HDR_PermintaanPerjalananDinasKebunId').val() }
    });
    viewer.refreshReport();
    prt.open().center();
}

function disableHeader() {
    $("._key").find('span').addClass('disabledbutton');
    $("._key").addClass('disabledbutton');
    $("._nonkey").find('span').addClass('disabledbutton');
    $("._nonkey").addClass('disabledbutton');
    $("._nonkeydalamwilayah").addClass('disabledbutton');
    $("._nonkeyluarwilayah").addClass('disabledbutton');

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
    $("._nonkeydalamwilayah").removeClass('disabledbutton');
    $("._nonkeyluarwilayah").removeClass('disabledbutton');
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
        $("._nonkeydalamwilayah").removeClass('disabledbutton');
        $("._nonkeyluarwilayah").removeClass('disabledbutton');
        gridDestroy();
    }
}

function filterNomorInput(e) {
    var mdl = JSON.stringify(ViewToModel());
    return { _model: mdl, _menuId: '1bdd62f8-d9a0-4b5d-89b7-8cc73310e1e3', _top:200 };
}

function filterKebun() {
    return {
        key: "NamaKebun",
        value: $("#NamaKebun").val()
    };
}

function NamaKebunOnSelect(e) {
    var kebun = this.dataItem(e.item);
    $('#KebunId').val(kebun.KebunId);
    $('#NamaKebun').val(kebun.Nama);
    kebunId = kebun.KebunId;

}
// Detail

function grdOnChange(e) {
}

function grdOnEdit(e) {
    var model = e.model;
    this.expandRow(this.tbody.find("tr[data-uid='" + model.uid + "']"));
    if (model.isNew()) {
        model.DTL_PermintaanPerjalananDinasKebunId = model.uid;
        model.HDR_PermintaanPerjalananDinasKebunId = hdr_PermintaanPerjalananDinasKebunId;
    }
    dtl_PermintaanPerjalananDinasKebunId = model.DTL_PermintaanPerjalananDinasKebunId;
    kebunId = $('#KebunId').val();
    jenisKendaraan = $('#KendaraanDinas').val();
    gridStatus = 'sudah-diapa-apain';

    if (jenisKendaraan == 1){
        $(e.container).find("input[name='UangLain']").prop('disabled', true);
    }
    else
    {
        $(e.container).find("input[name='UangLain']").prop('disabled', false);
    }
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

//function sendData() {
//    var grid = $("#grdDetail").data("kendoGrid"),
//        parameterMap = grid.dataSource.transport.parameterMap;

//    //get the new and the updated records
//    var currentData = grid.dataSource.data();
//    var updatedRecords = [];
//    var newRecords = [];
//    var spupdated = [];
//    var spnew = [];

//    for (var i = 0; i < currentData.length; i++) {
//        if (currentData[i].isNew()) {
//            //this record is new
//            newRecords.push(JSON.stringify(currentData[i]));
//        } else if (currentData[i].dirty) {
//            updatedRecords.push(JSON.stringify(currentData[i]));
//            spupdated.push("Update [SPDK_KanpusEF].[dbo].[DTL_PermintaanPerjalananDinasKebun] set JumlahBpd=UangSaku+UangMakan+UangCucian+UangTransport+UangLain+UangPenginapan");
//        }
//    }

//    //this records are deleted
//    var deletedRecords = [];
//    for (var i = 0; i < grid.dataSource._destroyed.length; i++) {
//        deletedRecords.push(JSON.stringify(grid.dataSource._destroyed[i]));
//    }

//    var data = {};
//    $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }), parameterMap({ new: newRecords }), parameterMap({ mnid: menuId }), parameterMap({ spupdated: spupdated }));
//    _sendData(data).done(function (msg) {
//        if (msg == "Success") {
//            grid.dataSource.read([]);
//            disableHeader();
//            gridStatus = 'belum-ngapa-ngapain';
//        }
//        else {
//            $('#errMsg').text(msg);
//            gridStatus = 'sudah-nyoba-disimpan-tapi-model-masih-salah';
//            openErrWindow();
//        }
//    }).fail(function (x) {
//        $('#errMsg').text(msg);
//        openErrWindow();
//        grid.dataSource.read([]);
//        disableHeader();
//        gridStatus = 'sudah-nyoba-disimpan-tapi-gagal';
//    });
//}

//function _sendData(data) {
//    return $.ajax({
//        url: "/Input/UpdateCreateDelete",
//        data: data,
//        type: "POST"
//    })
//}

function sendData() {
    var grid = $("#grdDetail").data("kendoGrid");
    var currentData = grid.dataSource.data();
    var stFail = 0;

    for (var i = 0; i < currentData.length; i++) {
        if (currentData[i].isNew()) {
            SimpanData('baru', currentData[i]).done(function (dta) {
                grid.dataSource.read([]);
                disableHeader();
                gridStatus = 'belum-ngapa-ngapain';
            }).fail(function () { stSfail = 1; alert("Ada kesalahan pada data ..."); });
        } else if (currentData[i].dirty) {
            SimpanData('edit', currentData[i]).done(function (dta) {
                grid.dataSource.read([]);
                disableHeader();
                gridStatus = 'belum-ngapa-ngapain';
            }).fail(function () { stSfail = 1; alert("Ada kesalahan pada data ..."); });
        }
    }

    //this records are deleted
    var deletedRecords = [];
    for (var i = 0; i < grid.dataSource._destroyed.length; i++) {
        SimpanData('hapus', grid.dataSource._destroyed[i]).done(function (dta) {
            grid.dataSource.read([]);
            disableHeader();
            gridStatus = 'belum-ngapa-ngapain';
        }).fail(function () { stSfail = 1; alert("Ada kesalahan pada data ..."); });
    }

    //if (stFail == 0) {
    //    alert("Data Sudah Tersimpan...");
    //}
}

function SimpanData(status, dta) {

    var url = '/Input/ExecSQL';
    var DTL_PermintaanPerjalananDinasKebunId = dta.DTL_PermintaanPerjalananDinasKebunId;

    var HDR_PermintaanPerjalananDinasKebunId = hdr_PermintaanPerjalananDinasKebunId;
    var NIK = dta.NIK;
    var UangSaku = dta.UangSaku;
    var UangMakan = dta.UangMakan;
    var UangCucian = dta.UangCucian;
    var UangTransport = dta.UangTransport;
    var UangLain = dta.UangLain;
    var JumlahBPD = dta.JumlahBPD;
    var UangPenginapan = dta.UangPenginapan;

    if (status == 'baru') {
        return $.ajax({
            url: url,
            data: {
                scriptSQL: "exec dbo.PerjalananDinas_PengajuanKebun '" + DTL_PermintaanPerjalananDinasKebunId.toString() +
                    "','" + HDR_PermintaanPerjalananDinasKebunId.toString() + "','" + NIK + "','" +
                    UangSaku + "', '" + UangMakan + "', '" + UangCucian + "', '" + UangTransport + "', '" + UangLain + "', '" + JumlahBPD + "', '" + UangPenginapan + "'",
                _menuId: menuId
            },
            type: 'POST',
            datatype: 'json'
        });

    } else if (status == 'edit') {
        return $.ajax({
            url: url,
            data: {
                scriptSQL: "exec dbo.PerjalananDinas_PengajuanKebun_Edit '" + DTL_PermintaanPerjalananDinasKebunId.toString() +
                    "','" + HDR_PermintaanPerjalananDinasKebunId.toString() + "','" + NIK + "','" +
                    UangSaku + "', '" + UangMakan + "', '" + UangCucian + "', '" + UangTransport + "', '" + UangLain + "', '" + JumlahBPD + "', '" + UangPenginapan + "'",
                _menuId: menuId
            },
            type: 'POST',
            datatype: 'json'
        });

    } else {
        return $.ajax({
            url: url,
            data: {
                scriptSQL: "exec dbo.PerjalananDinas_PengajuanKebun_Del '" + DTL_PermintaanPerjalananDinasKebunId.toString() + "'",
                _menuId: menuId
            },
            type: 'POST',
            datatype: 'json'
        });

    }
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

function filterTujuan() {
    return {
        //id: IdBudidaya,
        value: $('#TujuanDalamWilayah').val()
    };
}

function LokasiTujuan(e) {
    
    if (typeof (e.item) != 'undefined')
        _tujuandlmwilayah = this.dataItem(e.item);
    else
        _tujuandlmwilayah = e.sender.dataItem();
    lokasiKerjaTujuanId = _tujuandlmwilayah.LokasiKerjaId;
    namatujuan = _tujuandlmwilayah.Nama;
    $('#TujuanId').val(lokasiKerjaTujuanId);
    $('#TujuanDalamWilayah').data('kendoDropDownList').value(_tujuandlmwilayah.namatujuan);
    if (namatujuan == 'Lain-Lain < 100')
    {
        $("._nonkeyluarwilayah").removeClass('disabledbutton');
        jarak = 99;
        $('#Jarak').val(jarak);
    }
    else if (namatujuan == 'Lain-Lain > 200') {
        $("._nonkeyluarwilayah").removeClass('disabledbutton');
        jarak = 201;
        $('#Jarak').val(jarak);
    }
    else if (namatujuan == 'Lain-Lain 100-200') {
        $("._nonkeyluarwilayah").removeClass('disabledbutton');
        jarak = 150;
        $('#Jarak').val(jarak);
    }
    else if (namatujuan != 'Lain-Lain') {
        $("._nonkeyluarwilayah").addClass('disabledbutton');
        getDataFrom().done(function (data) {
            if (data != null) {
                jarak = data[0][0];
                $('#Jarak').val(jarak);
            }
            else if (data == null) {
                jarak = 99;
                $('#Jarak').val(jarak);
                //opencekStatusKM();
            }
        })
    }
}

function filterdetailRead(e) {
    return { _menuId: menuId };
}

function filterdetailCreate(e) {
    return { _menuId: menuId };
}

function JenisTujuanOnSelect(e) {
    var jenisTujuan = $('#JenisTujuan').val();
    if (jenisTujuan == "Luar Wilayah > 100 km" || jenisTujuan == "Luar Wilayah < 100 km") {
        $("._nonkeydalamwilayah").addClass('disabledbutton');
        $("._nonkeyluarwilayah").removeClass('disabledbutton');
        $('#Tujuan').val("");
        $('#TujuanId').val('00000000-0000-0000-0000-000000000000');
        namatujuan = "";
        jarak = 0;
    }
    else if (jenisTujuan == "Jakarta") {
        $("._nonkeydalamwilayah").addClass('disabledbutton');
        $("._nonkeyluarwilayah").removeClass('disabledbutton');
        $('#Tujuan').val("Jakarta");
        $('#TujuanId').val('00000000-0000-0000-0000-000000000000');
        namatujuan = "";
        jarak = 0;
    }
    else if (jenisTujuan == "Dalam Wilayah") {
        $("._nonkeyluarwilayah").addClass('disabledbutton');
        $("._nonkeydalamwilayah").removeClass('disabledbutton');
        $('#Tujuan').val("");
    }
}

function KendaraanDinasOnSelect(e) {
    var kendaraanDinas = $('#KendaraanDinas').val();
}

function StatusKMdanMenginapOnSelect(e) {
    var statusKMdanMenginap = $('#StatusKMdanMenginap').val();
}

function DapatBiayaPenginapanOnSelect(e) {
    var dapatBiayaPenginapan = $('#DapatBiayaPenginapan').val();
}

//function JumlahMakanOnSelect(e) {
//    var jumlahMakan = $('#JumlahMakan').val();
//}

function filterPengemudi() {
    var tanggalPermintaan = $('#TanggalPermintaan').val();
    var tanggalPermintaanPisah = tanggalPermintaan.split('/');
    var objek_tgl = new Date();

    tahunPermintaan = tanggalPermintaanPisah[2];
    var tahunPermintaanPisah = tahunPermintaan.split(' ');
    tahunPermintaan = tahunPermintaanPisah[0];
    bulanPermintaan = tanggalPermintaanPisah[1];
    return {
        strClassView: 'Ptpn8.UploadDataHarian.Models.ref_dik',
        scriptSQL: "select distinct NAMA, REG from [SPDK_KanpusEF].[dbo].[Ref_Dik]" +
            " where (NAMA_JAB like '%pengemudi%' or NAMA_JAB like '%supir%' or NAMA_JAB like '%sopir%') and KD_KBN = '" + $('#Kd_Kbn').val() + "' and MBT= 'K' and Month(Tanggal) = '" + bulanPermintaan + "' and Year(Tanggal) = '" + tahunPermintaan + "'" +
            " union select distinct NAMA, REG from [SPDK_KanpusEF].[dbo].[Ref_DikKLM]" +
            " where (NAMA_JAB like '%pengemudi%' or NAMA_JAB like '%supir%' or NAMA_JAB like '%sopir%') and KD_KBN = '" + $('#Kd_Kbn').val() + "' and Month(Tanggal) = '" + bulanPermintaan + "' and Year(Tanggal) = " + tahunPermintaan,
        _menuId: menuId
    }
}

function PengemudiOnSelect(e) {
    var pengemudi = this.dataItem(e.item);
    reg = pengemudi.REG;
}

function TanggalPermintaanOnSelect(e) {
    $('#NamaDriver').data('kendoAutoComplete').dataSource.read();
}