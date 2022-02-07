using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Areal.Models
{
    public class DataView
    {
        public Guid Id { get; set; }
        public string Wilayah { get; set; }
        public string Nama { get; set; }
        public string Kota { get; set; }
        public string Propinsi { get; set; }
        public string DaftarAfdeling { get; set; }
        public string DaftarBudidaya { get; set; }
        public string DaftarSKHGU { get; set; }
        public string DaftarStatusAreal { get; set; }
        public DateTime TanggalBerlaku { get; set; }
        public DateTime TanggalBerakhir { get; set; }
        public int SisaWaktu { get; set; }
        public decimal LuasAreal { get; set; }

    }

    public class DataViewKebun
    {
        public Guid Id { get; set; }
        public string Wilayah { get; set; }
        public string Nama { get; set; }
        public List<string> DaftarBudidaya { get; set; }
        public List<string> DaftarSKHGU { get; set; }
        public List<string> DaftarTahunTanam { get; set; }
        public List<string> DaftarStatusAreal { get; set; }
        public List<string> TanggalBerlaku { get; set; }
        public List<string> TanggalBerakhir { get; set; }
        public List<int> SisaWaktu { get; set; }
        public string sDaftarBudidaya { get; set; }
        public string sDaftarSKHGU { get; set; }
        public string sDaftarTahunTanam { get; set; }
        public string sDaftarStatusAreal { get; set; }
        public string sTanggalBerlaku { get; set; }
        public string sTanggalBerakhir { get; set; }
        public string sSisaWaktu { get; set; }

        public decimal TM { get; set; }
        public decimal TBM { get; set; }
        public decimal TTI { get; set; }
        public decimal TTAD { get; set; }
        public decimal Pesemaian { get; set; }
        public decimal Entrys { get; set; }
        public decimal Reboisasi { get; set; }
        public decimal Monocrop { get; set; }
        public decimal Intercrop { get; set; }
        public decimal TPJ { get; set; }
        public decimal Emplasemen { get; set; }
        public decimal Jalan { get; set; }
        public decimal Fasos { get; set; }
        public decimal Marginal { get; set; }
        public decimal CadanganMurni { get; set; }
        public decimal CadanganPokok { get; set; }
        public decimal Agrowisata { get; set; }
        public decimal KerjasamaBisnis { get; set; }
        public decimal PinjamPakai { get; set; }
        public decimal Okupasi { get; set; }
        public decimal LuasAreal { get; set; }
    }

    public class DataViewAfdeling
    {
        public Guid Id { get; set; }
        public string Kebun { get; set; }
        public Guid KebunId { get; set; }
        public string Nama { get; set; }
        public List<string> DaftarBudidaya { get; set; }
        public List<string> DaftarSKHGU { get; set; }
        public List<string> DaftarTahunTanam { get; set; }
        public List<string> DaftarStatusAreal { get; set; }
        public List<string> TanggalBerlaku { get; set; }
        public List<string> TanggalBerakhir { get; set; }
        public List<int> SisaWaktu { get; set; }

        public string sDaftarBudidaya { get; set; }
        public string sDaftarSKHGU { get; set; }
        public string sDaftarTahunTanam { get; set; }
        public string sDaftarStatusAreal { get; set; }
        public string sTanggalBerlaku { get; set; }
        public string sTanggalBerakhir { get; set; }
        public string sSisaWaktu { get; set; }

        public decimal TM { get; set; }
        public decimal TBM { get; set; }
        public decimal TTI { get; set; }
        public decimal TTAD { get; set; }
        public decimal Pesemaian { get; set; }
        public decimal Entrys { get; set; }
        public decimal Reboisasi { get; set; }
        public decimal Monocrop { get; set; }
        public decimal Intercrop { get; set; }
        public decimal TPJ { get; set; }
        public decimal Emplasemen { get; set; }
        public decimal Jalan { get; set; }
        public decimal Fasos { get; set; }
        public decimal Marginal { get; set; }
        public decimal CadanganMurni { get; set; }
        public decimal CadanganPokok { get; set; }
        public decimal Agrowisata { get; set; }
        public decimal KerjasamaBisnis { get; set; }
        public decimal PinjamPakai { get; set; }
        public decimal Okupasi { get; set; }
        public decimal LuasAreal { get; set; }
    }

    public class DataViewBlok
    {
        public Guid Id { get; set; }
        public string Afdeling { get; set; }
        public Guid AfdelingId { get; set; }
        public string Nama { get; set; }
        public string DaftarBudidaya { get; set; }
        public string DaftarTahunTanam { get; set; }
        public string DaftarSKHGU { get; set; }
        public string TanggalBerlaku { get; set; }
        public string TanggalBerakhir { get; set; }
        public int SisaWaktu { get; set; }
        public string DaftarStatusAreal { get; set; }
        public decimal LuasAreal { get; set; }
    }
    
    public class DataKegiatanKebun
    {
        public Guid Id { get; set; }
        public string NamaKebun { get; set; }
        public decimal LuasAreal { get; set; }
        public string TahunTanam { get; set; }
        public string NamaWilayah { get; set; }
        public string NamaBudidaya { get; set; }

        public decimal RealisasiJumlahKaryawanTetap { get; set; }
        public decimal RealisasiJumlahKaryawanLepas { get; set; }
        public int RealisasiJumlahHKKerjaKaryawanTetap { get; set; }
        public int RealisasiJumlahHKKerjaKaryawanLepas { get; set; }
        public int RealisasiJumlahHKTidakKerja { get; set; }
        public int RealisasiJumlahHKMangkir { get; set; }
        public double? RealisasiHasilKerjaKaryawanTetap { get; set; }
        public double? RealisasiHasilKerjaKaryawanLepas { get; set; }
        public double? RealisasiHasilKerjaLumpKaryawanTetap { get; set; }
        public double? RealisasiHasilKerjaLumpKaryawanLepas { get; set; }
        public double? RealisasiJelajahKaryawanTetap { get; set; }
        public double? RealisasiJelajahKaryawanLepas { get; set; }
        public double? RealisasiJumlahUpahKerjaKaryawanTetap { get; set; }
        public double? RealisasiJumlahUpahKerjaKaryawanLepas { get; set; }
        public double? RealisasiJumlahUpahSosial { get; set; }
        public double? RealisasiJumlahUpahKaryawanTetap { get; set; }
        public double? RKAPJumlahKaryawanTetap { get; set; }
        public double? RKAPJumlahKaryawanLepas { get; set; }
        public double? RKAPJumlahHKKerjaKaryawanTetap { get; set; }
        public double? RKAPJumlahHKKerjaKaryawanLepas { get; set; }
        public double? RKAPJumlahHKTidakKerja { get; set; }
        public double? RKAPJumlahHKMangkir { get; set; }
        public double? RKAPHasilKerjaKaryawanTetap { get; set; }
        public double? RKAPHasilKerjaKaryawanLepas { get; set; }
        public double? RKAPJelajahKaryawanTetap { get; set; }
        public double? RKAPJelajahKaryawanLepas { get; set; }
        public double? RKAPJumlahUpahKerjaKaryawanTetap { get; set; }
        public double? RKAPJumlahUpahKerjaKaryawanLepas { get; set; }
        public double? RKAPJumlahUpahSosial { get; set; }
        public double? RKAPJumlahUpahKaryawanTetap { get; set; }
        public double? PKBJumlahKaryawanTetap { get; set; }
        public double? PKBJumlahKaryawanLepas { get; set; }
        public double? PKBJumlahHKKerjaKaryawanTetap { get; set; }
        public double? PKBJumlahHKKerjaKaryawanLepas { get; set; }
        public double? PKBJumlahHKTidakKerja { get; set; }
        public double? PKBJumlahHKMangkir { get; set; }
        public double? PKBHasilKerjaKaryawanTetap { get; set; }
        public double? PKBHasilKerjaKaryawanLepas { get; set; }
        public double? PKBJelajahKaryawanTetap { get; set; }
        public double? PKBJelajahKaryawanLepas { get; set; }
        public double? PKBJumlahUpahKerjaKaryawanTetap { get; set; }
        public double? PKBJumlahUpahKerjaKaryawanLepas { get; set; }
        public double? PKBJumlahUpahSosial { get; set; }
        public double? PKBJumlahUpahKaryawanTetap { get; set; }
        public decimal RealisasiJumlahKaryawan { get; set; }
        public int RealisasiJumlahHKKerja { get; set; }
        public double? RealisasiHasilKerja { get; set; }
        public double? RealisasiJumlahUpahKerja { get; set; }
        public double? RealisasiBiayaLain { get; set; }
        public double? RealisasiTotalBiaya { get; set; }

        public double? RKAPJumlahKaryawan { get; set; }
        public double? RKAPJumlahHKKerja { get; set; }
        public double? RKAPHasilKerja { get; set; }
        public double? RKAPJumlahUpahKerja { get; set; }

        public double? PKBJumlahKaryawan { get; set; }
        public double? PKBJumlahHKKerja { get; set; }
        public double? PKBHasilKerja { get; set; }
        public double? PKBJumlahUpahKerja { get; set; }

        public decimal RealisasiSBIJumlahKaryawan { get; set; }
        public int RealisasiSBIJumlahHKKerja { get; set; }
        public double? RealisasiSBIHasilKerja { get; set; }
        public double? RealisasiSBIJumlahUpahKerja { get; set; }
        public double? RealisasiSBIBiayaLain { get; set; }
        public double? RealisasiSBITotalBiaya { get; set; }

        public double? RKAPSBIJumlahKaryawan { get; set; }
        public double? RKAPSBIJumlahHKKerja { get; set; }
        public double? RKAPSBIHasilKerja { get; set; }
        public double? RKAPSBIJumlahUpahKerja { get; set; }

        public double? PKBSBIJumlahKaryawan { get; set; }
        public double? PKBSBIJumlahHKKerja { get; set; }
        public double? PKBSBIHasilKerja { get; set; }
        public double? PKBSBIJumlahUpahKerja { get; set; }

    }

    public class DataKegiatanAfdeling
    {
        public Guid Id { get; set; }
        public string NamaAfdeling { get; set; }
        public decimal LuasAreal { get; set; }
        public string TahunTanam { get; set; }
        public Guid KebunId { get; set; }
        public string NamaKebun { get; set; }
        public string NamaBudidaya { get; set; }
        public decimal RealisasiJumlahKaryawanTetap { get; set; }
        public decimal RealisasiJumlahKaryawanLepas { get; set; }
        public int RealisasiJumlahHKKerjaKaryawanTetap { get; set; }
        public int RealisasiJumlahHKKerjaKaryawanLepas { get; set; }
        public int RealisasiJumlahHKTidakKerja { get; set; }
        public int RealisasiJumlahHKMangkir { get; set; }
        public double? RealisasiHasilKerjaKaryawanTetap { get; set; }
        public double? RealisasiHasilKerjaKaryawanLepas { get; set; }
        public double? RealisasiHasilKerjaLumpKaryawanTetap { get; set; }
        public double? RealisasiHasilKerjaLumpKaryawanLepas { get; set; }
        public double? RealisasiJumlahUpahKerjaKaryawanTetap { get; set; }
        public double? RealisasiJumlahUpahKerjaKaryawanLepas { get; set; }
        public double? RealisasiJumlahUpahSosial { get; set; }
        public double? RealisasiJumlahUpahKaryawanTetap { get; set; }

        public double? RKAPJumlahKaryawanTetap { get; set; }
        public double? RKAPJumlahKaryawanLepas { get; set; }
        public double? RKAPJumlahHKKerjaKaryawanTetap { get; set; }
        public double? RKAPJumlahHKKerjaKaryawanLepas { get; set; }
        public double? RKAPJumlahHKTidakKerja { get; set; }
        public double? RKAPJumlahHKMangkir { get; set; }
        public double? RKAPHasilKerjaKaryawanTetap { get; set; }
        public double? RKAPHasilKerjaKaryawanLepas { get; set; }
        public double? RKAPJumlahUpahKerjaKaryawanTetap { get; set; }
        public double? RKAPJumlahUpahKerjaKaryawanLepas { get; set; }
        public double? RKAPJumlahUpahSosial { get; set; }
        public double? RKAPJumlahUpahKaryawanTetap { get; set; }

        public double? PKBJumlahKaryawanTetap { get; set; }
        public double? PKBJumlahKaryawanLepas { get; set; }
        public double? PKBJumlahHKKerjaKaryawanTetap { get; set; }
        public double? PKBJumlahHKKerjaKaryawanLepas { get; set; }
        public double? PKBJumlahHKTidakKerja { get; set; }
        public double? PKBJumlahHKMangkir { get; set; }

        public double? PKBHasilKerjaKaryawanTetap { get; set; }
        public double? PKBHasilKerjaKaryawanLepas { get; set; }
        public double? PKBJumlahUpahKerjaKaryawanTetap { get; set; }
        public double? PKBJumlahUpahKerjaKaryawanLepas { get; set; }
        public double? PKBJumlahUpahSosial { get; set; }
        public double? PKBJumlahUpahKaryawanTetap { get; set; }

        public decimal RealisasiJumlahKaryawan { get; set; }
        public int RealisasiJumlahHKKerja { get; set; }
        public double? RealisasiHasilKerja { get; set; }
        public double? RealisasiJumlahUpahKerja { get; set; }
        public double? RealisasiBiayaLain { get; set; }
        public double? RealisasiTotalBiaya { get; set; }

        public double? RKAPJumlahKaryawan { get; set; }
        public double? RKAPJumlahHKKerja { get; set; }
        public double? RKAPHasilKerja { get; set; }
        public double? RKAPJumlahUpahKerja { get; set; }

        public double? PKBJumlahKaryawan { get; set; }
        public double? PKBJumlahHKKerja { get; set; }
        public double? PKBHasilKerja { get; set; }
        public double? PKBJumlahUpahKerja { get; set; }

        public decimal RealisasiSBIJumlahKaryawan { get; set; }
        public int RealisasiSBIJumlahHKKerja { get; set; }
        public double? RealisasiSBIHasilKerja { get; set; }
        public double? RealisasiSBIJumlahUpahKerja { get; set; }
        public double? RealisasiSBIBiayaLain { get; set; }
        public double? RealisasiSBITotalBiaya { get; set; }

        public double? RKAPSBIJumlahKaryawan { get; set; }
        public double? RKAPSBIJumlahHKKerja { get; set; }
        public double? RKAPSBIHasilKerja { get; set; }
        public double? RKAPSBIJumlahUpahKerja { get; set; }

        public double? PKBSBIJumlahKaryawan { get; set; }
        public double? PKBSBIJumlahHKKerja { get; set; }
        public double? PKBSBIHasilKerja { get; set; }
        public double? PKBSBIJumlahUpahKerja { get; set; }

        public double? RealisasiJelajahKaryawanTetap { get; set; }
        public double? RealisasiJelajahKaryawanLepas { get; set; }
        public double? RKAPJelajahKaryawanTetap { get; set; }
        public double? RKAPJelajahKaryawanLepas { get; set; }
        public double? PKBJelajahKaryawanTetap { get; set; }
        public double? PKBJelajahKaryawanLepas { get; set; }

    }

    public class DataKegiatanBlok
    {
        public Guid Id { get; set; }
        public string NamaBlok { get; set; }
        public decimal LuasAreal { get; set; }
        public string TahunTanam { get; set; }
        public Guid AfdelingId { get; set; }
        public string NamaAfdeling { get; set; }
        public string NamaBudidaya { get; set; }
        public decimal RealisasiJumlahKaryawanTetap { get; set; }
        public decimal RealisasiJumlahKaryawanLepas { get; set; }
        public int RealisasiJumlahHKKerjaKaryawanTetap { get; set; }
        public int RealisasiJumlahHKKerjaKaryawanLepas { get; set; }
        public int RealisasiJumlahHKTidakKerja { get; set; }
        public int RealisasiJumlahHKMangkir { get; set; }
        public double? RealisasiHasilKerjaKaryawanTetap { get; set; }
        public double? RealisasiHasilKerjaKaryawanLepas { get; set; }
        public double? RealisasiHasilKerjaLumpKaryawanTetap { get; set; }
        public double? RealisasiHasilKerjaLumpKaryawanLepas { get; set; }
        public double? RealisasiJumlahUpahKerjaKaryawanTetap { get; set; }
        public double? RealisasiJumlahUpahKerjaKaryawanLepas { get; set; }
        public double? RealisasiJumlahUpahSosial { get; set; }
        public double? RealisasiJumlahUpahKaryawanTetap { get; set; }

        public double? RKAPJumlahKaryawanTetap { get; set; }
        public double? RKAPJumlahKaryawanLepas { get; set; }
        public double? RKAPJumlahHKKerjaKaryawanTetap { get; set; }
        public double? RKAPJumlahHKKerjaKaryawanLepas { get; set; }
        public double? RKAPJumlahHKTidakKerja { get; set; }
        public double? RKAPJumlahHKMangkir { get; set; }
        public double? RKAPHasilKerjaKaryawanTetap { get; set; }
        public double? RKAPHasilKerjaKaryawanLepas { get; set; }
        public double? RKAPJumlahUpahKerjaKaryawanTetap { get; set; }
        public double? RKAPJumlahUpahKerjaKaryawanLepas { get; set; }
        public double? RKAPJumlahUpahSosial { get; set; }
        public double? RKAPJumlahUpahKaryawanTetap { get; set; }

        public double? PKBJumlahKaryawanTetap { get; set; }
        public double? PKBJumlahKaryawanLepas { get; set; }
        public double? PKBJumlahHKKerjaKaryawanTetap { get; set; }
        public double? PKBJumlahHKKerjaKaryawanLepas { get; set; }
        public double? PKBJumlahHKTidakKerja { get; set; }
        public double? PKBJumlahHKMangkir { get; set; }

        public double? PKBHasilKerjaKaryawanTetap { get; set; }
        public double? PKBHasilKerjaKaryawanLepas { get; set; }
        public double? PKBJumlahUpahKerjaKaryawanTetap { get; set; }
        public double? PKBJumlahUpahKerjaKaryawanLepas { get; set; }
        public double? PKBJumlahUpahSosial { get; set; }
        public double? PKBJumlahUpahKaryawanTetap { get; set; }

        public decimal RealisasiJumlahKaryawan { get; set; }
        public int RealisasiJumlahHKKerja { get; set; }
        public double? RealisasiHasilKerja { get; set; }
        public double? RealisasiJumlahUpahKerja { get; set; }
        public double? RealisasiBiayaLain { get; set; }
        public double? RealisasiTotalBiaya { get; set; }

        public double? RKAPJumlahKaryawan { get; set; }
        public double? RKAPJumlahHKKerja { get; set; }
        public double? RKAPHasilKerja { get; set; }
        public double? RKAPJumlahUpahKerja { get; set; }

        public double? PKBJumlahKaryawan { get; set; }
        public double? PKBJumlahHKKerja { get; set; }
        public double? PKBHasilKerja { get; set; }
        public double? PKBJumlahUpahKerja { get; set; }

        public decimal RealisasiSBIJumlahKaryawan { get; set; }
        public int RealisasiSBIJumlahHKKerja { get; set; }
        public double? RealisasiSBIHasilKerja { get; set; }
        public double? RealisasiSBIJumlahUpahKerja { get; set; }
        public double? RealisasiSBIBiayaLain { get; set; }
        public double? RealisasiSBITotalBiaya { get; set; }

        public double? RKAPSBIJumlahKaryawan { get; set; }
        public double? RKAPSBIJumlahHKKerja { get; set; }
        public double? RKAPSBIHasilKerja { get; set; }
        public double? RKAPSBIJumlahUpahKerja { get; set; }

        public double? PKBSBIJumlahKaryawan { get; set; }
        public double? PKBSBIJumlahHKKerja { get; set; }
        public double? PKBSBIHasilKerja { get; set; }
        public double? PKBSBIJumlahUpahKerja { get; set; }
        public double? RealisasiJelajahKaryawanTetap { get; set; }
        public double? RealisasiJelajahKaryawanLepas { get; set; }
        public double? RKAPJelajahKaryawanTetap { get; set; }
        public double? RKAPJelajahKaryawanLepas { get; set; }
        public double? PKBJelajahKaryawanTetap { get; set; }
        public double? PKBJelajahKaryawanLepas { get; set; }

    }

    public class BiayaProduksiperBudidaya
    {
        public string KodeKebun { get; set; }
        public string NamaKebun { get; set; }
        public string NamaWilayah { get; set; }
        public Guid KebunId { get; set; }
        public string KodeBudidaya { get; set; }
        public string NamaBudidaya { get; set; }
        public decimal LuasAreal { get; set; }
        public double? ProduksiSendiriMurni_BI { get; set; }
        public double? ProduksiLancuranTBM_BI { get; set; }
        public double? ProduksiPihak3_BI { get; set; }
        public double? ProduksiPembelianAntarKebun_BI { get; set; }
        public double? ProduksiPenjualanAntarKebun_BI { get; set; }
        public double? JumlahProduksi_BI { get; set; }
        public double? BiayaPemeliharaanTM_BI { get; set; }
        public double? BiayaPanen_BI { get; set; }
        public double? BiayaPengangkutan_BI { get; set; }
        public double? BiayaPenyusutanTanaman_BI { get; set; }
        public double? BiayaPenyusutanNonTanaman_BI { get; set; }
        public double? BiayaPengolahan_BI { get; set; }
        public double? BiayaPembelianHasilTanaman_BI { get; set; }
        public double? BiayaPenjualanHasilTanaman_BI { get; set; }
        public double? JumlahBiayaProduksi_BI { get; set; }
        public double? ProduksiSendiriMurni_SBI { get; set; }
        public double? ProduksiLancuranTBM_SBI { get; set; }
        public double? ProduksiPihak3_SBI { get; set; }
        public double? ProduksiPembelianAntarKebun_SBI { get; set; }
        public double? ProduksiPenjualanAntarKebun_SBI { get; set; }
        public double? JumlahProduksi_SBI { get; set; }
        public double? BiayaPemeliharaanTM_SBI { get; set; }
        public double? BiayaPanen_SBI { get; set; }
        public double? BiayaPengangkutan_SBI { get; set; }
        public double? BiayaPenyusutanTanaman_SBI { get; set; }
        public double? BiayaPenyusutanNonTanaman_SBI { get; set; }
        public double? BiayaPengolahan_SBI { get; set; }
        public double? BiayaPembelianHasilTanaman_SBI { get; set; }
        public double? BiayaPenjualanHasilTanaman_SBI { get; set; }
        public double? JumlahBiayaProduksi_SBI { get; set; }
        public double? ProduksiSendiriMurni_RBI { get; set; }
        public double? ProduksiLancuranTBM_RBI { get; set; }
        public double? ProduksiPihak3_RBI { get; set; }
        public double? ProduksiPembelianAntarKebun_RBI { get; set; }
        public double? ProduksiPenjualanAntarKebun_RBI { get; set; }
        public double? JumlahProduksi_RBI { get; set; }
        public double? BiayaPemeliharaanTM_RBI { get; set; }
        public double? BiayaPanen_RBI { get; set; }
        public double? BiayaPengangkutan_RBI { get; set; }
        public double? BiayaPenyusutanTanaman_RBI { get; set; }
        public double? BiayaPenyusutanNonTanaman_RBI { get; set; }
        public double? BiayaPengolahan_RBI { get; set; }
        public double? BiayaPembelianHasilTanaman_RBI { get; set; }
        public double? BiayaPenjualanHasilTanaman_RBI { get; set; }
        public double? JumlahBiayaProduksi_RBI { get; set; }
        public double? ProduksiSendiriMurni_RSBI { get; set; }
        public double? ProduksiLancuranTBM_RSBI { get; set; }
        public double? ProduksiPihak3_RSBI { get; set; }
        public double? ProduksiPembelianAntarKebun_RSBI { get; set; }
        public double? ProduksiPenjualanAntarKebun_RSBI { get; set; }
        public double? JumlahProduksi_RSBI { get; set; }
        public double? BiayaPemeliharaanTM_RSBI { get; set; }
        public double? BiayaPanen_RSBI { get; set; }
        public double? BiayaPengangkutan_RSBI { get; set; }
        public double? BiayaPenyusutanTanaman_RSBI { get; set; }
        public double? BiayaPenyusutanNonTanaman_RSBI { get; set; }
        public double? BiayaPengolahan_RSBI { get; set; }
        public double? BiayaPembelianHasilTanaman_RSBI { get; set; }
        public double? BiayaPenjualanHasilTanaman_RSBI { get; set; }
        public double? JumlahBiayaProduksi_RSBI { get; set; }

        public double? BiayaStaf_BI { get; set; }
        public double? BiayaStaf_SBI { get; set; }
        public double? BiayaStaf_RBI { get; set; }
        public double? BiayaStaf_RSBI { get; set; }

    }

    public class BiayaProduksiperJenis
    {
        public string KodeKebun { get; set; }
        public string NamaKebun { get; set; }
        public string NamaWilayah { get; set; }
        public Guid KebunId { get; set; }
        public string KodeJenisBudidaya { get; set; }
        public string NamaJenisBudidaya { get; set; }
        public decimal LuasAreal { get; set; }
        public double? ProduksiSendiriMurni_BI { get; set; }
        public double? ProduksiLancuranTBM_BI { get; set; }
        public double? ProduksiPihak3_BI { get; set; }
        public double? ProduksiPembelianAntarKebun_BI { get; set; }
        public double? ProduksiPenjualanAntarKebun_BI { get; set; }
        public double? JumlahProduksi_BI { get; set; }
        public double? BiayaPemeliharaanTM_BI { get; set; }
        public double? BiayaPanen_BI { get; set; }
        public double? BiayaPengangkutan_BI { get; set; }
        public double? BiayaPenyusutanTanaman_BI { get; set; }
        public double? BiayaPenyusutanNonTanaman_BI { get; set; }
        public double? BiayaPengolahan_BI { get; set; }
        public double? BiayaPembelianHasilTanaman_BI { get; set; }
        public double? BiayaPenjualanHasilTanaman_BI { get; set; }
        public double? JumlahBiayaProduksi_BI { get; set; }
        public double? ProduksiSendiriMurni_SBI { get; set; }
        public double? ProduksiLancuranTBM_SBI { get; set; }
        public double? ProduksiPihak3_SBI { get; set; }
        public double? ProduksiPembelianAntarKebun_SBI { get; set; }
        public double? ProduksiPenjualanAntarKebun_SBI { get; set; }
        public double? JumlahProduksi_SBI { get; set; }
        public double? BiayaPemeliharaanTM_SBI { get; set; }
        public double? BiayaPanen_SBI { get; set; }
        public double? BiayaPengangkutan_SBI { get; set; }
        public double? BiayaPenyusutanTanaman_SBI { get; set; }
        public double? BiayaPenyusutanNonTanaman_SBI { get; set; }
        public double? BiayaPengolahan_SBI { get; set; }
        public double? BiayaPembelianHasilTanaman_SBI { get; set; }
        public double? BiayaPenjualanHasilTanaman_SBI { get; set; }
        public double? JumlahBiayaProduksi_SBI { get; set; }
        public double? ProduksiSendiriMurni_RBI { get; set; }
        public double? ProduksiLancuranTBM_RBI { get; set; }
        public double? ProduksiPihak3_RBI { get; set; }
        public double? ProduksiPembelianAntarKebun_RBI { get; set; }
        public double? ProduksiPenjualanAntarKebun_RBI { get; set; }
        public double? JumlahProduksi_RBI { get; set; }
        public double? BiayaPemeliharaanTM_RBI { get; set; }
        public double? BiayaPanen_RBI { get; set; }
        public double? BiayaPengangkutan_RBI { get; set; }
        public double? BiayaPenyusutanTanaman_RBI { get; set; }
        public double? BiayaPenyusutanNonTanaman_RBI { get; set; }
        public double? BiayaPengolahan_RBI { get; set; }
        public double? BiayaPembelianHasilTanaman_RBI { get; set; }
        public double? BiayaPenjualanHasilTanaman_RBI { get; set; }
        public double? JumlahBiayaProduksi_RBI { get; set; }
        public double? ProduksiSendiriMurni_RSBI { get; set; }
        public double? ProduksiLancuranTBM_RSBI { get; set; }
        public double? ProduksiPihak3_RSBI { get; set; }
        public double? ProduksiPembelianAntarKebun_RSBI { get; set; }
        public double? ProduksiPenjualanAntarKebun_RSBI { get; set; }
        public double? JumlahProduksi_RSBI { get; set; }
        public double? BiayaPemeliharaanTM_RSBI { get; set; }
        public double? BiayaPanen_RSBI { get; set; }
        public double? BiayaPengangkutan_RSBI { get; set; }
        public double? BiayaPenyusutanTanaman_RSBI { get; set; }
        public double? BiayaPenyusutanNonTanaman_RSBI { get; set; }
        public double? BiayaPengolahan_RSBI { get; set; }
        public double? BiayaPembelianHasilTanaman_RSBI { get; set; }
        public double? BiayaPenjualanHasilTanaman_RSBI { get; set; }
        public double? JumlahBiayaProduksi_RSBI { get; set; }
    }

    public class ChartStatusAreal
    {
        public string StatusAreal { get; set; }
        public decimal LuasAreal { get; set; }
    }

    public class ChartStatusArealPerBudidaya
    {
        public string NamaBudidaya { get; set; }
        public string StatusAreal { get; set; }
        public decimal LuasAreal { get; set; }
    }

    public class ChartStatusArealPerKebun
    {
        public string NamaKebun { get; set; }
        public string NamaBudidaya { get; set; }
        public string StatusAreal { get; set; }
        public decimal LuasAreal { get; set; }

    }

    public class DaftarKebun
    {
        public int TALUNSANTOSA { get; set; }
        public int PURBASARI { get; set; }
        public int CISARUNI { get; set; }
        public int BOJONGDATAR { get; set; }
        public int CIBUNGUR { get; set; }
        public int CIKASO { get; set; }
        public int CIKASUNGKA { get; set; }
        public int SUKAMAJU { get; set; }
        public int AGROWISATA { get; set; }
        public int GOALPARA { get; set; }
        public int PASIRBADAK { get; set; }
        public int CIKUMPAY { get; set; }
        public int CIKUPA { get; set; }
        public int RANCABALI { get; set; }
        public int PASIRNANGKA { get; set; }
        public int BUKITUNGGUL { get; set; }
        public int BATULAWANG { get; set; }
        public int CISALAKBARU { get; set; }
        public int TAMBAKSARI { get; set; }
        public int JALUPANG { get; set; }
        public int CIATER { get; set; }
        public int RANCABOLANG { get; set; }
        public int PANGLEJAR { get; set; }
        public int PANYAIRAN { get; set; }
        public int SINUMBRA { get; set; }
        public int PARAKANSALAK { get; set; }
        public int WANGUNREJA { get; set; }
        public int KERTAMANAH { get; set; }
        public int GUNUNGMAS { get; set; }
        public int KERTAJAYA { get; set; }
        public int INDUSTRI_HILIR_TEH { get; set; }
        public int PANGLEJAR_PANGHEOTAN { get; set; }
        public int MIRAMARE { get; set; }
        public int DAYEUHMANGGUNG { get; set; }
        public int BAGJANAGARA { get; set; }
        public int AGRABINTA { get; set; }
        public int GEDEH { get; set; }
        public int SEDEP { get; set; }
        public int BUNISARI_LENDRA { get; set; }
        public int MONTAYA { get; set; }
        public int PASIRMALANG { get; set; }
        public int MALABAR { get; set; }
        public int PAPANDAYAN { get; set; }
        public int INDUSTRI_HILIR_HORTIKULTUR { get; set; }
        public int TALUNSANTOSA_RKAP { get; set; }
        public int PURBASARI_RKAP { get; set; }
        public int CISARUNI_RKAP { get; set; }
        public int BOJONGDATAR_RKAP { get; set; }
        public int CIBUNGUR_RKAP { get; set; }
        public int CIKASO_RKAP { get; set; }
        public int CIKASUNGKA_RKAP { get; set; }
        public int SUKAMAJU_RKAP { get; set; }
        public int AGROWISATA_RKAP { get; set; }
        public int GOALPARA_RKAP { get; set; }
        public int PASIRBADAK_RKAP { get; set; }
        public int CIKUMPAY_RKAP { get; set; }
        public int CIKUPA_RKAP { get; set; }
        public int RANCABALI_RKAP { get; set; }
        public int PASIRNANGKA_RKAP { get; set; }
        public int BUKITUNGGUL_RKAP { get; set; }
        public int BATULAWANG_RKAP { get; set; }
        public int CISALAKBARU_RKAP { get; set; }
        public int TAMBAKSARI_RKAP { get; set; }
        public int JALUPANG_RKAP { get; set; }
        public int CIATER_RKAP { get; set; }
        public int RANCABOLANG_RKAP { get; set; }
        public int PANGLEJAR_RKAP { get; set; }
        public int PANYAIRAN_RKAP { get; set; }
        public int SINUMBRA_RKAP { get; set; }
        public int PARAKANSALAK_RKAP { get; set; }
        public int WANGUNREJA_RKAP { get; set; }
        public int KERTAMANAH_RKAP { get; set; }
        public int GUNUNGMAS_RKAP { get; set; }
        public int KERTAJAYA_RKAP { get; set; }
        public int INDUSTRI_HILIR_TEH_RKAP { get; set; }
        public int PANGLEJAR_PANGHEOTAN_RKAP { get; set; }
        public int MIRAMARE_RKAP { get; set; }
        public int DAYEUHMANGGUNG_RKAP { get; set; }
        public int BAGJANAGARA_RKAP { get; set; }
        public int AGRABINTA_RKAP { get; set; }
        public int GEDEH_RKAP { get; set; }
        public int SEDEP_RKAP { get; set; }
        public int BUNISARI_LENDRA_RKAP { get; set; }
        public int MONTAYA_RKAP { get; set; }
        public int PASIRMALANG_RKAP { get; set; }
        public int MALABAR_RKAP { get; set; }
        public int PAPANDAYAN_RKAP { get; set; }
        public int INDUSTRI_HILIR_HORTIKULTUR_RKAP { get; set; }

    }
    public class DaftarBudidaya
    {
        public int Sawit { get; set; }
        public int Gutta_Perca { get; set; }
        public int Alpukat { get; set; }
        public int Jambu_Air { get; set; }
        public int Sirsak { get; set; }
        public int KKE { get; set; }
        public int Teh { get; set; }
        public int Durian { get; set; }
        public int JambuBatu { get; set; }
        public int Lainlain { get; set; }
        public int Kelapa { get; set; }
        public int Kakao { get; set; }
        public int BuahNaga { get; set; }
        public int Kina { get; set; }
        public int Kopi { get; set; }
        public int Agrowisata { get; set; }
        public int Jeruk { get; set; }
        public int Pisang { get; set; }
        public int Nanas { get; set; }
        public int Salak { get; set; }
        public int Karet { get; set; }
        public int Belimbing { get; set; }
        public int Manggis { get; set; }
        public int Pepaya { get; set; }
        public int Lengkeng { get; set; }
        public int Sawit_RKAP { get; set; }
        public int Gutta_Perca_RKAP { get; set; }
        public int Alpukat_RKAP { get; set; }
        public int Jambu_Air_RKAP { get; set; }
        public int Sirsak_RKAP { get; set; }
        public int KKE_RKAP { get; set; }
        public int Teh_RKAP { get; set; }
        public int Durian_RKAP { get; set; }
        public int JambuBatu_RKAP { get; set; }
        public int Lainlain_RKAP { get; set; }
        public int Kelapa_RKAP { get; set; }
        public int Kakao_RKAP { get; set; }
        public int BuahNaga_RKAP { get; set; }
        public int Kina_RKAP { get; set; }
        public int Kopi_RKAP { get; set; }
        public int Agrowisata_RKAP { get; set; }
        public int Jeruk_RKAP { get; set; }
        public int Pisang_RKAP { get; set; }
        public int Nanas_RKAP { get; set; }
        public int Salak_RKAP { get; set; }
        public int Karet_RKAP { get; set; }
        public int Belimbing_RKAP { get; set; }
        public int Manggis_RKAP { get; set; }
        public int Pepaya_RKAP { get; set; }
        public int Lengkeng_RKAP { get; set; }
    }
    public class ChartProduksiPerBudidaya : DaftarBudidaya
    {
        public int Bulan { get; set; }
        public int Tahun { get; set; }
        public string Kebun { get; set; }
        public string Budidaya { get; set; }
        public int RealisasiJumlahProduksi { get; set; }
        public int RKAPJumlahProduksi { get; set; }
        public float? RealisasiBiayaProduksi { get; set; }
        public float? RKAPBiayaProduksi { get; set; }

    }

    public class ChartProduksiPerKebun : DaftarKebun
    {
        public int Bulan { get; set; }
        public int Tahun { get; set; }
        public string Budidaya { get; set; }

    }

    
}