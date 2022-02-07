using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Areal.Models
{
    public class HasilPanen
    {
        public HasilPanen()
        {
            HasilPanenId = Guid.NewGuid();
            Bulan = 0;
            Tahun = 0;
            KodeBlok = "";
            TahunTanam = "";
            KodeBudidaya = "";
            GroupMesin = "";
            LuasAreal = 0;
            KgLateks = 0;
            KgLump = 0;
            KgTBS = 0;
            KgBrondol = 0;
            JumlahTandan = 0;
            JumlahPohon = 0;
            KgPucuk = 0;
            PanenLainnya = 0;
            JumlahHK = 0;
            HaJelajah = 0;
            RpGaji = 0;
            RKAPKgLateks = 0;
            RKAPKgLump = 0;
            RKAPKgTBS = 0;
            RKAPKgBrondol = 0;
            RKAPJumlahTandan = 0;
            RKAPKgPucuk = 0;
            RKAPPanenLainnya = 0;
            RKAPJumlahHK = 0;
            RKAPHaJelajah = 0;
            RKAPRpGaji = 0;
            PKBKgLateks = 0;
            PKBKgLump = 0;
            PKBKgTBS = 0;
            PKBKgBrondol = 0;
            PKBJumlahTandan = 0;
            PKBKgPucuk = 0;
            PKBPanenLainnya = 0;
            PKBJumlahHK = 0;
            PKBHaJelajah = 0;
            PKBRpGaji = 0;
        }
        public Guid HasilPanenId { get; set; }
        public int Bulan { get; set; }
        public int Tahun { get; set; }
        public string KodeBlok { get; set; }
        public string TahunTanam { get; set; }
        public string KodeBudidaya { get; set; }
        public decimal LuasAreal { get; set; }
        public decimal KgLateks { get; set; }
        public decimal KgLump { get; set; }
        public decimal KgTBS { get; set; }
        public decimal KgBrondol { get; set; }
        public decimal JumlahTandan { get; set; }
        public decimal JumlahPohon { get; set; }
        public decimal KgPucuk { get; set; }
        public decimal PanenLainnya { get; set; }
        public int JumlahHK { get; set; }
        public decimal HaJelajah { get; set; }
        public decimal RpGaji { get; set; }
        public decimal RKAPKgLateks { get; set; }
        public decimal RKAPKgLump { get; set; }
        public decimal RKAPKgTBS { get; set; }
        public decimal RKAPKgBrondol { get; set; }
        public decimal RKAPJumlahTandan { get; set; }
        public decimal RKAPKgPucuk { get; set; }
        public decimal RKAPPanenLainnya { get; set; }
        public int RKAPJumlahHK { get; set; }
        public decimal RKAPHaJelajah { get; set; }
        public decimal RKAPRpGaji { get; set; }
        public decimal PKBKgLateks { get; set; }
        public decimal PKBKgLump { get; set; }
        public decimal PKBKgTBS { get; set; }
        public decimal PKBKgBrondol { get; set; }
        public decimal PKBJumlahTandan { get; set; }
        public decimal PKBKgPucuk { get; set; }
        public decimal PKBPanenLainnya { get; set; }
        public int PKBJumlahHK { get; set; }
        public decimal PKBHaJelajah { get; set; }
        public decimal PKBRpGaji { get; set; }
        public string GroupMesin { get; set; }

    }

    public class HasilPanenPerTanggal
    {
        public HasilPanenPerTanggal()
        {
            HasilPanenId = Guid.NewGuid();
            Tanggal = DateTime.Now;
            KodeBlok = "";
            TahunTanam = "";
            KodeBudidaya = "";
            GroupMesin = "";
            LuasAreal = 0;
            KgLateks = 0;
            KgLump = 0;
            KgTBS = 0;
            KgBrondol = 0;
            JumlahTandan = 0;
            JumlahPohon = 0;
            KgPucuk = 0;
            PanenLainnya = 0;
            JumlahHK = 0;
            HaJelajah = 0;
            RpGaji = 0;
            RKAPKgLateks = 0;
            RKAPKgLump = 0;
            RKAPKgTBS = 0;
            RKAPKgBrondol = 0;
            RKAPJumlahTandan = 0;
            RKAPKgPucuk = 0;
            RKAPPanenLainnya = 0;
            RKAPJumlahHK = 0;
            RKAPHaJelajah = 0;
            RKAPRpGaji = 0;
            PKBKgLateks = 0;
            PKBKgLump = 0;
            PKBKgTBS = 0;
            PKBKgBrondol = 0;
            PKBJumlahTandan = 0;
            PKBKgPucuk = 0;
            PKBPanenLainnya = 0;
            PKBJumlahHK = 0;
            PKBHaJelajah = 0;
            PKBRpGaji = 0;

        }
        public Guid HasilPanenId { get; set; }
        public DateTime Tanggal { get; set; }
        public string KodeBlok { get; set; }
        public string TahunTanam { get; set; }
        public string KodeBudidaya { get; set; }
        public decimal LuasAreal { get; set; }
        public decimal KgLateks { get; set; }
        public decimal KgLump { get; set; }
        public decimal KgTBS { get; set; }
        public decimal KgBrondol { get; set; }
        public decimal JumlahTandan { get; set; }
        public decimal JumlahPohon { get; set; }
        public decimal KgPucuk { get; set; }
        public decimal PanenLainnya { get; set; }
        public int JumlahHK { get; set; }
        public decimal HaJelajah { get; set; }
        public decimal RpGaji { get; set; }
        public string GroupMesin { get; set; }
        public decimal RKAPKgLateks { get; set; }
        public decimal RKAPKgLump { get; set; }
        public decimal RKAPKgTBS { get; set; }
        public decimal RKAPKgBrondol { get; set; }
        public decimal RKAPJumlahTandan { get; set; }
        public decimal RKAPKgPucuk { get; set; }
        public decimal RKAPPanenLainnya { get; set; }
        public int RKAPJumlahHK { get; set; }
        public decimal RKAPHaJelajah { get; set; }
        public decimal RKAPRpGaji { get; set; }
        public decimal PKBKgLateks { get; set; }
        public decimal PKBKgLump { get; set; }
        public decimal PKBKgTBS { get; set; }
        public decimal PKBKgBrondol { get; set; }
        public decimal PKBJumlahTandan { get; set; }
        public decimal PKBKgPucuk { get; set; }
        public decimal PKBPanenLainnya { get; set; }
        public int PKBJumlahHK { get; set; }
        public decimal PKBHaJelajah { get; set; }
        public decimal PKBRpGaji { get; set; }


    }


    public class ProduktivitasPerKebun
    {
        public ProduktivitasPerKebun()
        {
            LuasAreal = 0;
            LuasArealInduk = 0;
            ProtasKebun_BI = 0;
            ProtasPTPN8_BI = 0;
            ProtasKebun_RBI = 0;
            ProtasKebun_PBI = 0;
            ProtasKebun_BalaiBI = 0;
            ProtasPTPN8_RBI = 0;
            ProtasKebun_SBI = 0;
            ProtasPTPN8_SBI = 0;
            ProtasKebun_RSBI = 0;
            ProtasKebun_PSBI = 0;
            ProtasKebun_BalaiSBI = 0;
            ProtasPTPN8_RSBI = 0;
            Januari_Real = 0;
            Januari_RKAP = 0;
            Januari_PKB = 0;
            Januari_PTPN8 = 0;
            Februari_Real = 0;
            Februari_RKAP = 0;
            Februari_PKB = 0;
            Februari_PTPN8 = 0;
            Maret_Real = 0;
            Maret_RKAP = 0;
            Maret_PKB = 0;
            Maret_PTPN8 = 0;
            April_Real = 0;
            April_RKAP = 0;
            April_PKB = 0;
            April_PTPN8 = 0;
            Mei_Real = 0;
            Mei_RKAP = 0;
            Mei_PKB = 0;
            Mei_PTPN8 = 0;
            Juni_Real = 0;
            Juni_RKAP = 0;
            Juni_PKB = 0;
            Juni_PTPN8 = 0;
            Juli_Real = 0;
            Juli_RKAP = 0;
            Juli_PKB = 0;
            Juli_PTPN8 = 0;
            Agustus_Real = 0;
            Agustus_RKAP = 0;
            Agustus_PKB = 0;
            Agustus_PTPN8 = 0;
            September_Real = 0;
            September_RKAP = 0;
            September_PKB = 0;
            September_PTPN8 = 0;
            Oktober_Real = 0;
            Oktober_RKAP = 0;
            Oktober_PKB = 0;
            Oktober_PTPN8 = 0;
            November_Real = 0;
            November_RKAP = 0;
            November_PKB = 0;
            November_PTPN8 = 0;
            Desember_Real = 0;
            Desember_RKAP = 0;
            Desember_PKB = 0;
            Desember_PTPN8 = 0;

            MATime_Sem1 = 0;
            MATime_Sem2 = 0;
            MABelow_Sem1 = 0;
            MABelow_Sem2 = 0;
        }
        public Guid Id { get; set; }
        public int Bulan { get; set; }
        public int Tahun { get; set; }
        public string Kebun { get; set; }
        public string TahunTanam { get; set; }
        public string Wilayah { get; set; }
        public string Budidaya { get; set; }
        public decimal LuasAreal { get; set; }
        public decimal LuasArealInduk { get; set; }
        public double? ProtasKebun_BI { get; set; }
        public double? ProtasPTPN8_BI { get; set; }
        public double? ProtasKebun_RBI { get; set; }
        public double? ProtasKebun_PBI { get; set; }
        public double? ProtasKebun_BalaiBI { get; set; }
        public double? ProtasPTPN8_RBI { get; set; }
        public double? ProtasKebun_SBI { get; set; }
        public double? ProtasPTPN8_SBI { get; set; }
        public double? ProtasKebun_RSBI { get; set; }
        public double? ProtasKebun_PSBI { get; set; }
        public double? ProtasKebun_BalaiSBI { get; set; }
        public double? ProtasPTPN8_RSBI { get; set; }
        public double? Januari_Real { get; set; }
        public double? Januari_RKAP { get; set; }
        public double? Januari_PKB { get; set; }
        public double? Januari_PTPN8 { get; set; }
        public double? Februari_Real { get; set; }
        public double? Februari_RKAP { get; set; }
        public double? Februari_PKB { get; set; }
        public double? Februari_PTPN8 { get; set; }
        public double? Maret_Real { get; set; }
        public double? Maret_RKAP { get; set; }
        public double? Maret_PKB { get; set; }
        public double? Maret_PTPN8 { get; set; }
        public double? April_Real { get; set; }
        public double? April_RKAP { get; set; }
        public double? April_PKB { get; set; }
        public double? April_PTPN8 { get; set; }
        public double? Mei_Real { get; set; }
        public double? Mei_RKAP { get; set; }
        public double? Mei_PKB { get; set; }
        public double? Mei_PTPN8 { get; set; }
        public double? Juni_Real { get; set; }
        public double? Juni_RKAP { get; set; }
        public double? Juni_PKB { get; set; }
        public double? Juni_PTPN8 { get; set; }
        public double? Juli_Real { get; set; }
        public double? Juli_RKAP { get; set; }
        public double? Juli_PKB { get; set; }
        public double? Juli_PTPN8 { get; set; }
        public double? Agustus_Real { get; set; }
        public double? Agustus_RKAP { get; set; }
        public double? Agustus_PKB { get; set; }
        public double? Agustus_PTPN8 { get; set; }
        public double? September_Real { get; set; }
        public double? September_RKAP { get; set; }
        public double? September_PKB { get; set; }
        public double? September_PTPN8 { get; set; }
        public double? Oktober_Real { get; set; }
        public double? Oktober_RKAP { get; set; }
        public double? Oktober_PKB { get; set; }
        public double? Oktober_PTPN8 { get; set; }
        public double? November_Real { get; set; }
        public double? November_RKAP { get; set; }
        public double? November_PKB { get; set; }
        public double? November_PTPN8 { get; set; }
        public double? Desember_Real { get; set; }
        public double? Desember_RKAP { get; set; }
        public double? Desember_PKB { get; set; }
        public double? Desember_PTPN8 { get; set; }
        public double? MATime_Sem1 { get; set; }
        public double? MATime_Sem2 { get; set; }
        public double? MABelow_Sem1 { get; set; }
        public double? MABelow_Sem2 { get; set; }
        public string Warna_Sem1 { get; set; }
        public string Warna_Sem2 { get; set; }

    }

    public class ProduktivitasPerAfdeling
    {
        public ProduktivitasPerAfdeling()
        {
            LuasAreal = 0;
            LuasArealInduk = 0;
            ProtasAfdeling_BI = 0;
            ProtasKebun_BI = 0;
            ProtasAfdeling_RBI = 0;
            ProtasAfdeling_PBI = 0;
            ProtasAfdeling_BalaiBI = 0;
            ProtasKebun_RBI = 0;
            ProtasAfdeling_SBI = 0;
            ProtasKebun_SBI = 0;
            ProtasAfdeling_RSBI = 0;
            ProtasAfdeling_PSBI = 0;
            ProtasAfdeling_BalaiSBI = 0;
            ProtasKebun_RSBI = 0;
            Januari_Real = 0;
            Januari_RKAP = 0;
            Januari_PKB = 0;
            Januari_PTPN8 = 0;
            Februari_Real = 0;
            Februari_RKAP = 0;
            Februari_PKB = 0;
            Februari_PTPN8 = 0;
            Maret_Real = 0;
            Maret_RKAP = 0;
            Maret_PKB = 0;
            Maret_PTPN8 = 0;
            April_Real = 0;
            April_RKAP = 0;
            April_PKB = 0;
            April_PTPN8 = 0;
            Mei_Real = 0;
            Mei_RKAP = 0;
            Mei_PKB = 0;
            Mei_PTPN8 = 0;
            Juni_Real = 0;
            Juni_RKAP = 0;
            Juni_PKB = 0;
            Juni_PTPN8 = 0;
            Juli_Real = 0;
            Juli_RKAP = 0;
            Juli_PKB = 0;
            Juli_PTPN8 = 0;
            Agustus_Real = 0;
            Agustus_RKAP = 0;
            Agustus_PKB = 0;
            Agustus_PTPN8 = 0;
            September_Real = 0;
            September_RKAP = 0;
            September_PKB = 0;
            September_PTPN8 = 0;
            Oktober_Real = 0;
            Oktober_RKAP = 0;
            Oktober_PKB = 0;
            Oktober_PTPN8 = 0;
            November_Real = 0;
            November_RKAP = 0;
            November_PKB = 0;
            November_PTPN8 = 0;
            Desember_Real = 0;
            Desember_RKAP = 0;
            Desember_PKB = 0;
            Desember_PTPN8 = 0;

            MATime_Sem1 = 0;
            MATime_Sem2 = 0;
            MABelow_Sem1 = 0;
            MABelow_Sem2 = 0;

        }
        public Guid Id { get; set; }
        public int Bulan { get; set; }
        public int Tahun { get; set; }
        public string Afdeling { get; set; }
        public string TahunTanam { get; set; }
        public string Kebun { get; set; }
        public string Budidaya { get; set; }
        public decimal LuasAreal { get; set; }
        public decimal LuasArealInduk { get; set; }
        public double? ProtasAfdeling_BI { get; set; }
        public double? ProtasKebun_BI { get; set; }
        public double? ProtasAfdeling_RBI { get; set; }
        public double? ProtasAfdeling_PBI { get; set; }
        public double? ProtasAfdeling_BalaiBI { get; set; }
        public double? ProtasKebun_RBI { get; set; }
        public double? ProtasAfdeling_SBI { get; set; }
        public double? ProtasKebun_SBI { get; set; }
        public double? ProtasAfdeling_RSBI { get; set; }
        public double? ProtasAfdeling_PSBI { get; set; }
        public double? ProtasAfdeling_BalaiSBI { get; set; }
        public double? ProtasKebun_RSBI { get; set; }
        public double? Januari_Real { get; set; }
        public double? Januari_RKAP { get; set; }
        public double? Januari_PKB { get; set; }
        public double? Januari_PTPN8 { get; set; }
        public double? Februari_Real { get; set; }
        public double? Februari_RKAP { get; set; }
        public double? Februari_PKB { get; set; }
        public double? Februari_PTPN8 { get; set; }
        public double? Maret_Real { get; set; }
        public double? Maret_RKAP { get; set; }
        public double? Maret_PKB { get; set; }
        public double? Maret_PTPN8 { get; set; }
        public double? April_Real { get; set; }
        public double? April_RKAP { get; set; }
        public double? April_PKB { get; set; }
        public double? April_PTPN8 { get; set; }
        public double? Mei_Real { get; set; }
        public double? Mei_RKAP { get; set; }
        public double? Mei_PKB { get; set; }
        public double? Mei_PTPN8 { get; set; }
        public double? Juni_Real { get; set; }
        public double? Juni_RKAP { get; set; }
        public double? Juni_PKB { get; set; }
        public double? Juni_PTPN8 { get; set; }
        public double? Juli_Real { get; set; }
        public double? Juli_RKAP { get; set; }
        public double? Juli_PKB { get; set; }
        public double? Juli_PTPN8 { get; set; }
        public double? Agustus_Real { get; set; }
        public double? Agustus_RKAP { get; set; }
        public double? Agustus_PKB { get; set; }
        public double? Agustus_PTPN8 { get; set; }
        public double? September_Real { get; set; }
        public double? September_RKAP { get; set; }
        public double? September_PKB { get; set; }
        public double? September_PTPN8 { get; set; }
        public double? Oktober_Real { get; set; }
        public double? Oktober_RKAP { get; set; }
        public double? Oktober_PKB { get; set; }
        public double? Oktober_PTPN8 { get; set; }
        public double? November_Real { get; set; }
        public double? November_RKAP { get; set; }
        public double? November_PKB { get; set; }
        public double? November_PTPN8 { get; set; }
        public double? Desember_Real { get; set; }
        public double? Desember_RKAP { get; set; }
        public double? Desember_PKB { get; set; }
        public double? Desember_PTPN8 { get; set; }
        public double? MATime_Sem1 { get; set; }
        public double? MATime_Sem2 { get; set; }
        public double? MABelow_Sem1 { get; set; }
        public double? MABelow_Sem2 { get; set; }
        public string Warna_Sem1 { get; set; }
        public string Warna_Sem2 { get; set; }

    }

    public class ProduktivitasPerBlok
    {
        public ProduktivitasPerBlok()
        {
            LuasAreal = 0;
            LuasArealInduk = 0;
            ProtasBlok_BI = 0;
            ProtasAfdeling_BI = 0;
            ProtasBlok_RBI = 0;
            ProtasBlok_PBI = 0;
            ProtasBlok_BalaiBI = 0;
            ProtasAfdeling_RBI = 0;
            ProtasBlok_SBI = 0;
            ProtasAfdeling_SBI = 0;
            ProtasBlok_RSBI = 0;
            ProtasBlok_PSBI = 0;
            ProtasBlok_BalaiSBI = 0;
            ProtasAfdeling_RSBI = 0;
            Januari_Real = 0;
            Januari_RKAP = 0;
            Januari_PKB = 0;
            Januari_PTPN8 = 0;
            Februari_Real = 0;
            Februari_RKAP = 0;
            Februari_PKB = 0;
            Februari_PTPN8 = 0;
            Maret_Real = 0;
            Maret_RKAP = 0;
            Maret_PKB = 0;
            Maret_PTPN8 = 0;
            April_Real = 0;
            April_RKAP = 0;
            April_PKB = 0;
            April_PTPN8 = 0;
            Mei_Real = 0;
            Mei_RKAP = 0;
            Mei_PKB = 0;
            Mei_PTPN8 = 0;
            Juni_Real = 0;
            Juni_RKAP = 0;
            Juni_PKB = 0;
            Juni_PTPN8 = 0;
            Juli_Real = 0;
            Juli_RKAP = 0;
            Juli_PKB = 0;
            Juli_PTPN8 = 0;
            Agustus_Real = 0;
            Agustus_RKAP = 0;
            Agustus_PKB = 0;
            Agustus_PTPN8 = 0;
            September_Real = 0;
            September_RKAP = 0;
            September_PKB = 0;
            September_PTPN8 = 0;
            Oktober_Real = 0;
            Oktober_RKAP = 0;
            Oktober_PKB = 0;
            Oktober_PTPN8 = 0;
            November_Real = 0;
            November_RKAP = 0;
            November_PKB = 0;
            November_PTPN8 = 0;
            Desember_Real = 0;
            Desember_RKAP = 0;
            Desember_PKB = 0;
            Desember_PTPN8 = 0;

            MATime_Sem1 = 0;
            MATime_Sem2 = 0;
            MABelow_Sem1 = 0;
            MABelow_Sem2 = 0;

        }
        public Guid Id { get; set; }
        public int Bulan { get; set; }
        public int Tahun { get; set; }
        public string Blok { get; set; }
        public string TahunTanam { get; set; }
        public string Afdeling { get; set; }
        public string Budidaya { get; set; }
        public decimal LuasAreal { get; set; }
        public decimal LuasArealInduk { get; set; }
        public double? ProtasBlok_BI { get; set; }
        public double? ProtasAfdeling_BI { get; set; }
        public double? ProtasBlok_RBI { get; set; }
        public double? ProtasBlok_PBI { get; set; }
        public double? ProtasBlok_BalaiBI { get; set; }
        public double? ProtasAfdeling_RBI { get; set; }
        public double? ProtasBlok_SBI { get; set; }
        public double? ProtasAfdeling_SBI { get; set; }
        public double? ProtasBlok_RSBI { get; set; }
        public double? ProtasBlok_PSBI { get; set; }
        public double? ProtasBlok_BalaiSBI { get; set; }
        public double? ProtasAfdeling_RSBI { get; set; }
        public double? Januari_Real { get; set; }
        public double? Januari_RKAP { get; set; }
        public double? Januari_PKB { get; set; }
        public double? Januari_PTPN8 { get; set; }
        public double? Februari_Real { get; set; }
        public double? Februari_RKAP { get; set; }
        public double? Februari_PKB { get; set; }
        public double? Februari_PTPN8 { get; set; }
        public double? Maret_Real { get; set; }
        public double? Maret_RKAP { get; set; }
        public double? Maret_PKB { get; set; }
        public double? Maret_PTPN8 { get; set; }
        public double? April_Real { get; set; }
        public double? April_RKAP { get; set; }
        public double? April_PKB { get; set; }
        public double? April_PTPN8 { get; set; }
        public double? Mei_Real { get; set; }
        public double? Mei_RKAP { get; set; }
        public double? Mei_PKB { get; set; }
        public double? Mei_PTPN8 { get; set; }
        public double? Juni_Real { get; set; }
        public double? Juni_RKAP { get; set; }
        public double? Juni_PKB { get; set; }
        public double? Juni_PTPN8 { get; set; }
        public double? Juli_Real { get; set; }
        public double? Juli_RKAP { get; set; }
        public double? Juli_PKB { get; set; }
        public double? Juli_PTPN8 { get; set; }
        public double? Agustus_Real { get; set; }
        public double? Agustus_RKAP { get; set; }
        public double? Agustus_PKB { get; set; }
        public double? Agustus_PTPN8 { get; set; }
        public double? September_Real { get; set; }
        public double? September_RKAP { get; set; }
        public double? September_PKB { get; set; }
        public double? September_PTPN8 { get; set; }
        public double? Oktober_Real { get; set; }
        public double? Oktober_RKAP { get; set; }
        public double? Oktober_PKB { get; set; }
        public double? Oktober_PTPN8 { get; set; }
        public double? November_Real { get; set; }
        public double? November_RKAP { get; set; }
        public double? November_PKB { get; set; }
        public double? November_PTPN8 { get; set; }
        public double? Desember_Real { get; set; }
        public double? Desember_RKAP { get; set; }
        public double? Desember_PKB { get; set; }
        public double? Desember_PTPN8 { get; set; }
        public double? MATime_Sem1 { get; set; }
        public double? MATime_Sem2 { get; set; }
        public double? MABelow_Sem1 { get; set; }
        public double? MABelow_Sem2 { get; set; }
        public string Warna_Sem1 { get; set; }
        public string Warna_Sem2 { get; set; }
    }

    public class RotasiPanenPerBulan
    {
        public RotasiPanenPerBulan()
        {
            Bulan = DateTime.Now.Month;
            Tahun = DateTime.Now.Year;
            KodeLokasi = "";
            KodeBudidaya = "";
            NamaBudidaya = "";
            NamaLokasi = "";
            NamaInduk = "";
            TahunTanam = "";
            LuasAreal = 0;
            Bulan = DateTime.Now.Month;
            Tahun = DateTime.Now.Year;
            Warna = "";
            JumlahPohon = 0;
            JumlahRotasi = 0;
            TanggalPanen = "";
            TotalLuasJelajah =0;
            TotalJumlahPohon = 0;
            TotalJumlahTandan = 0;
            TotalKgTBS = 0;
            TotalKgBrondol = 0;
            TotalKgLateks = 0;
            TotalKgLump = 0;
            TotalKgPucuk = 0;
            TotalKgLainnya = 0;
            TotalJumlahHK = 0;
            TotalJumlahUpah = 0;
            T01LuasJelajah = 0;
            T01JumlahPohon = 0;
            T01JumlahTandan = 0;
            T01KgTBS = 0;
            T01KgBrondol = 0;
            T01KgLateks = 0;
            T01KgLump = 0;
            T01KgPucuk = 0;
            T01KgLainnya = 0;
            T01JumlahHK = 0;
            T01JumlahUpah = 0;

            T01LuasJelajah = 0;
            T01JumlahPohon =0;
            T01JumlahTandan =0;
            T01KgTBS =0;
            T01KgBrondol =0;
            T01KgLateks =0;
            T01KgLump =0;
            T01KgPucuk =0;
            T01KgLainnya =0;
            T01JumlahHK =0;
            T01JumlahUpah =0;
            T02LuasJelajah =0;
            T02JumlahPohon =0;
            T02JumlahTandan =0;
            T02KgTBS =0;
            T02KgBrondol =0;
            T02KgLateks =0;
            T02KgLump =0;
            T02KgPucuk =0;
            T02KgLainnya =0;
            T02JumlahHK =0;
            T02JumlahUpah =0;
            T03LuasJelajah =0;
            T03JumlahPohon =0;
            T03JumlahTandan =0;
            T03KgTBS =0;
            T03KgBrondol =0;
            T03KgLateks =0;
            T03KgLump =0;
            T03KgPucuk =0;
            T03KgLainnya =0;
            T03JumlahHK =0;
            T03JumlahUpah =0;
            T04LuasJelajah =0;
            T04JumlahPohon =0;
            T04JumlahTandan =0;
            T04KgTBS =0;
            T04KgBrondol =0;
            T04KgLateks =0;
            T04KgLump =0;
            T04KgPucuk =0;
            T04KgLainnya =0;
            T04JumlahHK =0;
            T04JumlahUpah =0;
            T05LuasJelajah =0;
            T05JumlahPohon =0;
            T05JumlahTandan =0;
            T05KgTBS =0;
            T05KgBrondol =0;
            T05KgLateks =0;
            T05KgLump =0;
            T05KgPucuk =0;
            T05KgLainnya =0;
            T05JumlahHK =0;
            T05JumlahUpah =0;
            T06LuasJelajah =0;
            T06JumlahPohon =0;
            T06JumlahTandan =0;
            T06KgTBS =0;
            T06KgBrondol =0;
            T06KgLateks =0;
            T06KgLump =0;
            T06KgPucuk =0;
            T06KgLainnya =0;
            T06JumlahHK =0;
            T06JumlahUpah =0;
            T07LuasJelajah =0;
            T07JumlahPohon =0;
            T07JumlahTandan =0;
            T07KgTBS =0;
            T07KgBrondol =0;
            T07KgLateks =0;
            T07KgLump =0;
            T07KgPucuk =0;
            T07KgLainnya =0;
            T07JumlahHK =0;
            T07JumlahUpah =0;
            T08LuasJelajah =0;
            T08JumlahPohon =0;
            T08JumlahTandan =0;
            T08KgTBS =0;
            T08KgBrondol =0;
            T08KgLateks =0;
            T08KgLump =0;
            T08KgPucuk =0;
            T08KgLainnya =0;
            T08JumlahHK =0;
            T08JumlahUpah =0;
            T09LuasJelajah =0;
            T09JumlahPohon =0;
            T09JumlahTandan =0;
            T09KgTBS =0;
            T09KgBrondol =0;
            T09KgLateks =0;
            T09KgLump =0;
            T09KgPucuk =0;
            T09KgLainnya =0;
            T09JumlahHK =0;
            T09JumlahUpah =0;
            T10LuasJelajah =0;
            T10JumlahPohon =0;
            T10JumlahTandan =0;
            T10KgTBS =0;
            T10KgBrondol =0;
            T10KgLateks =0;
            T10KgLump =0;
            T10KgPucuk =0;
            T10KgLainnya =0;
            T10JumlahHK =0;
            T10JumlahUpah =0;
            T11LuasJelajah =0;
            T11JumlahPohon =0;
            T11JumlahTandan =0;
            T11KgTBS =0;
            T11KgBrondol =0;
            T11KgLateks =0;
            T11KgLump =0;
            T11KgPucuk =0;
            T11KgLainnya =0;
            T11JumlahHK =0;
            T11JumlahUpah =0;
            T12LuasJelajah =0;
            T12JumlahPohon =0;
            T12JumlahTandan =0;
            T12KgTBS =0;
            T12KgBrondol =0;
            T12KgLateks =0;
            T12KgLump =0;
            T12KgPucuk =0;
            T12KgLainnya =0;
            T12JumlahHK =0;
            T12JumlahUpah =0;
            T13LuasJelajah =0;
            T13JumlahPohon =0;
            T13JumlahTandan =0;
            T13KgTBS =0;
            T13KgBrondol =0;
            T13KgLateks =0;
            T13KgLump =0;
            T13KgPucuk =0;
            T13KgLainnya =0;
            T13JumlahHK =0;
            T13JumlahUpah =0;
            T14LuasJelajah =0;
            T14JumlahPohon =0;
            T14JumlahTandan =0;
            T14KgTBS =0;
            T14KgBrondol =0;
            T14KgLateks =0;
            T14KgLump =0;
            T14KgPucuk =0;
            T14KgLainnya =0;
            T14JumlahHK =0;
            T14JumlahUpah =0;
            T15LuasJelajah =0;
            T15JumlahPohon =0;
            T15JumlahTandan =0;
            T15KgTBS =0;
            T15KgBrondol =0;
            T15KgLateks =0;
            T15KgLump =0;
            T15KgPucuk =0;
            T15KgLainnya =0;
            T15JumlahHK =0;
            T15JumlahUpah =0;
            T16LuasJelajah =0;
            T16JumlahPohon =0;
            T16JumlahTandan =0;
            T16KgTBS =0;
            T16KgBrondol =0;
            T16KgLateks =0;
            T16KgLump =0;
            T16KgPucuk =0;
            T16KgLainnya =0;
            T16JumlahHK =0;
            T16JumlahUpah =0;
            T17LuasJelajah =0;
            T17JumlahPohon =0;
            T17JumlahTandan =0;
            T17KgTBS =0;
            T17KgBrondol =0;
            T17KgLateks =0;
            T17KgLump =0;
            T17KgPucuk =0;
            T17KgLainnya =0;
            T17JumlahHK =0;
            T17JumlahUpah =0;
            T18LuasJelajah =0;
            T18JumlahPohon =0;
            T18JumlahTandan =0;
            T18KgTBS =0;
            T18KgBrondol =0;
            T18KgLateks =0;
            T18KgLump =0;
            T18KgPucuk =0;
            T18KgLainnya =0;
            T18JumlahHK =0;
            T18JumlahUpah =0;
            T19LuasJelajah =0;
            T19JumlahPohon =0;
            T19JumlahTandan =0;
            T19KgTBS =0;
            T19KgBrondol =0;
            T19KgLateks =0;
            T19KgLump =0;
            T19KgPucuk =0;
            T19KgLainnya =0;
            T19JumlahHK =0;
            T19JumlahUpah =0;
            T20LuasJelajah =0;
            T20JumlahPohon =0;
            T20JumlahTandan =0;
            T20KgTBS =0;
            T20KgBrondol =0;
            T20KgLateks =0;
            T20KgLump =0;
            T20KgPucuk =0;
            T20KgLainnya =0;
            T20JumlahHK =0;
            T20JumlahUpah =0;
            T21LuasJelajah =0;
            T21JumlahPohon =0;
            T21JumlahTandan =0;
            T21KgTBS =0;
            T21KgBrondol =0;
            T21KgLateks =0;
            T21KgLump =0;
            T21KgPucuk =0;
            T21KgLainnya =0;
            T21JumlahHK =0;
            T21JumlahUpah =0;
            T22LuasJelajah =0;
            T22JumlahPohon =0;
            T22JumlahTandan =0;
            T22KgTBS =0;
            T22KgBrondol =0;
            T22KgLateks =0;
            T22KgLump =0;
            T22KgPucuk =0;
            T22KgLainnya =0;
            T22JumlahHK =0;
            T22JumlahUpah =0;
            T23LuasJelajah =0;
            T23JumlahPohon =0;
            T23JumlahTandan =0;
            T23KgTBS =0;
            T23KgBrondol =0;
            T23KgLateks =0;
            T23KgLump =0;
            T23KgPucuk =0;
            T23KgLainnya =0;
            T23JumlahHK =0;
            T23JumlahUpah =0;
            T24LuasJelajah =0;
            T24JumlahPohon =0;
            T24JumlahTandan =0;
            T24KgTBS =0;
            T24KgBrondol =0;
            T24KgLateks =0;
            T24KgLump =0;
            T24KgPucuk =0;
            T24KgLainnya =0;
            T24JumlahHK =0;
            T24JumlahUpah =0;
            T25LuasJelajah =0;
            T25JumlahPohon =0;
            T25JumlahTandan =0;
            T25KgTBS =0;
            T25KgBrondol =0;
            T25KgLateks =0;
            T25KgLump =0;
            T25KgPucuk =0;
            T25KgLainnya =0;
            T25JumlahHK =0;
            T25JumlahUpah =0;
            T26LuasJelajah =0;
            T26JumlahPohon =0;
            T26JumlahTandan =0;
            T26KgTBS =0;
            T26KgBrondol =0;
            T26KgLateks =0;
            T26KgLump =0;
            T26KgPucuk =0;
            T26KgLainnya =0;
            T26JumlahHK =0;
            T26JumlahUpah =0;
            T27LuasJelajah =0;
            T27JumlahPohon =0;
            T27JumlahTandan =0;
            T27KgTBS =0;
            T27KgBrondol =0;
            T27KgLateks =0;
            T27KgLump =0;
            T27KgPucuk =0;
            T27KgLainnya =0;
            T27JumlahHK =0;
            T27JumlahUpah =0;
            T28LuasJelajah =0;
            T28JumlahPohon =0;
            T28JumlahTandan =0;
            T28KgTBS =0;
            T28KgBrondol =0;
            T28KgLateks =0;
            T28KgLump =0;
            T28KgPucuk =0;
            T28KgLainnya =0;
            T28JumlahHK =0;
            T28JumlahUpah =0;
            T29LuasJelajah =0;
            T29JumlahPohon =0;
            T29JumlahTandan =0;
            T29KgTBS =0;
            T29KgBrondol =0;
            T29KgLateks =0;
            T29KgLump =0;
            T29KgPucuk =0;
            T29KgLainnya =0;
            T29JumlahHK =0;
            T29JumlahUpah =0;
            T30LuasJelajah =0;
            T30JumlahPohon =0;
            T30JumlahTandan =0;
            T30KgTBS =0;
            T30KgBrondol =0;
            T30KgLateks =0;
            T30KgLump =0;
            T30KgPucuk =0;
            T30KgLainnya =0;
            T30JumlahHK =0;
            T30JumlahUpah =0;
            T31LuasJelajah = 0;
            T31JumlahPohon = 0;
            T31JumlahTandan =0;
            T31KgTBS =0;
            T31KgBrondol =0;
            T31KgLateks =0;
            T31KgLump =0;
            T31KgPucuk =0;
            T31KgLainnya =0;
            T31JumlahHK =0;
            T31JumlahUpah =0;
    }
        public Guid Id { get; set; }
        public string KodeLokasi { get; set; }
        public string KodeBudidaya { get; set; }
        public string NamaBudidaya { get; set; }
        public string NamaLokasi { get; set; }
        public string NamaInduk { get; set; }
        public string TahunTanam { get; set; }
        public decimal LuasAreal { get; set; }
        public decimal JumlahPohon { get; set; }
        public decimal JumlahRotasi { get; set; }
        public string TanggalPanen { get; set; }
        public int Bulan { get; set; }
        public int Tahun { get; set; }
        public string Warna { get; set; }
        public decimal TotalLuasJelajah { get; set; }
        public decimal TotalJumlahPohon { get; set; }
        public decimal TotalJumlahTandan { get; set; }
        public decimal TotalKgTBS { get; set; }
        public decimal TotalKgBrondol { get; set; }
        public decimal TotalKgLateks { get; set; }
        public decimal TotalKgLump { get; set; }
        public decimal TotalKgPucuk { get; set; }
        public decimal TotalKgLainnya { get; set; }
        public int TotalJumlahHK { get; set; }
        public decimal TotalJumlahUpah { get; set; }
        public decimal T01LuasJelajah { get; set; }
        public decimal T01JumlahPohon { get; set; }
        public decimal T01JumlahTandan { get; set; }
        public decimal T01KgTBS { get; set; }
        public decimal T01KgBrondol { get; set; }
        public decimal T01KgLateks { get; set; }
        public decimal T01KgLump { get; set; }
        public decimal T01KgPucuk { get; set; }
        public decimal T01KgLainnya { get; set; }
        public int T01JumlahHK { get; set; }
        public decimal T01JumlahUpah { get; set; }
        public decimal T02LuasJelajah { get; set; }
        public decimal T02JumlahPohon { get; set; }
        public decimal T02JumlahTandan { get; set; }
        public decimal T02KgTBS { get; set; }
        public decimal T02KgBrondol { get; set; }
        public decimal T02KgLateks { get; set; }
        public decimal T02KgLump { get; set; }
        public decimal T02KgPucuk { get; set; }
        public decimal T02KgLainnya { get; set; }
        public int T02JumlahHK { get; set; }
        public decimal T02JumlahUpah { get; set; }
        public decimal T03LuasJelajah { get; set; }
        public decimal T03JumlahPohon { get; set; }
        public decimal T03JumlahTandan { get; set; }
        public decimal T03KgTBS { get; set; }
        public decimal T03KgBrondol { get; set; }
        public decimal T03KgLateks { get; set; }
        public decimal T03KgLump { get; set; }
        public decimal T03KgPucuk { get; set; }
        public decimal T03KgLainnya { get; set; }
        public int T03JumlahHK { get; set; }
        public decimal T03JumlahUpah { get; set; }
        public decimal T04LuasJelajah { get; set; }
        public decimal T04JumlahPohon { get; set; }
        public decimal T04JumlahTandan { get; set; }
        public decimal T04KgTBS { get; set; }
        public decimal T04KgBrondol { get; set; }
        public decimal T04KgLateks { get; set; }
        public decimal T04KgLump { get; set; }
        public decimal T04KgPucuk { get; set; }
        public decimal T04KgLainnya { get; set; }
        public int T04JumlahHK { get; set; }
        public decimal T04JumlahUpah { get; set; }
        public decimal T05LuasJelajah { get; set; }
        public decimal T05JumlahPohon { get; set; }
        public decimal T05JumlahTandan { get; set; }
        public decimal T05KgTBS { get; set; }
        public decimal T05KgBrondol { get; set; }
        public decimal T05KgLateks { get; set; }
        public decimal T05KgLump { get; set; }
        public decimal T05KgPucuk { get; set; }
        public decimal T05KgLainnya { get; set; }
        public int T05JumlahHK { get; set; }
        public decimal T05JumlahUpah { get; set; }
        public decimal T06LuasJelajah { get; set; }
        public decimal T06JumlahPohon { get; set; }
        public decimal T06JumlahTandan { get; set; }
        public decimal T06KgTBS { get; set; }
        public decimal T06KgBrondol { get; set; }
        public decimal T06KgLateks { get; set; }
        public decimal T06KgLump { get; set; }
        public decimal T06KgPucuk { get; set; }
        public decimal T06KgLainnya { get; set; }
        public int T06JumlahHK { get; set; }
        public decimal T06JumlahUpah { get; set; }
        public decimal T07LuasJelajah { get; set; }
        public decimal T07JumlahPohon { get; set; }
        public decimal T07JumlahTandan { get; set; }
        public decimal T07KgTBS { get; set; }
        public decimal T07KgBrondol { get; set; }
        public decimal T07KgLateks { get; set; }
        public decimal T07KgLump { get; set; }
        public decimal T07KgPucuk { get; set; }
        public decimal T07KgLainnya { get; set; }
        public int T07JumlahHK { get; set; }
        public decimal T07JumlahUpah { get; set; }
        public decimal T08LuasJelajah { get; set; }
        public decimal T08JumlahPohon { get; set; }
        public decimal T08JumlahTandan { get; set; }
        public decimal T08KgTBS { get; set; }
        public decimal T08KgBrondol { get; set; }
        public decimal T08KgLateks { get; set; }
        public decimal T08KgLump { get; set; }
        public decimal T08KgPucuk { get; set; }
        public decimal T08KgLainnya { get; set; }
        public int T08JumlahHK { get; set; }
        public decimal T08JumlahUpah { get; set; }
        public decimal T09LuasJelajah { get; set; }
        public decimal T09JumlahPohon { get; set; }
        public decimal T09JumlahTandan { get; set; }
        public decimal T09KgTBS { get; set; }
        public decimal T09KgBrondol { get; set; }
        public decimal T09KgLateks { get; set; }
        public decimal T09KgLump { get; set; }
        public decimal T09KgPucuk { get; set; }
        public decimal T09KgLainnya { get; set; }
        public int T09JumlahHK { get; set; }
        public decimal T09JumlahUpah { get; set; }
        public decimal T10LuasJelajah { get; set; }
        public decimal T10JumlahPohon { get; set; }
        public decimal T10JumlahTandan { get; set; }
        public decimal T10KgTBS { get; set; }
        public decimal T10KgBrondol { get; set; }
        public decimal T10KgLateks { get; set; }
        public decimal T10KgLump { get; set; }
        public decimal T10KgPucuk { get; set; }
        public decimal T10KgLainnya { get; set; }
        public int T10JumlahHK { get; set; }
        public decimal T10JumlahUpah { get; set; }
        public decimal T11LuasJelajah { get; set; }
        public decimal T11JumlahPohon { get; set; }
        public decimal T11JumlahTandan { get; set; }
        public decimal T11KgTBS { get; set; }
        public decimal T11KgBrondol { get; set; }
        public decimal T11KgLateks { get; set; }
        public decimal T11KgLump { get; set; }
        public decimal T11KgPucuk { get; set; }
        public decimal T11KgLainnya { get; set; }
        public int T11JumlahHK { get; set; }
        public decimal T11JumlahUpah { get; set; }
        public decimal T12LuasJelajah { get; set; }
        public decimal T12JumlahPohon { get; set; }
        public decimal T12JumlahTandan { get; set; }
        public decimal T12KgTBS { get; set; }
        public decimal T12KgBrondol { get; set; }
        public decimal T12KgLateks { get; set; }
        public decimal T12KgLump { get; set; }
        public decimal T12KgPucuk { get; set; }
        public decimal T12KgLainnya { get; set; }
        public int T12JumlahHK { get; set; }
        public decimal T12JumlahUpah { get; set; }
        public decimal T13LuasJelajah { get; set; }
        public decimal T13JumlahPohon { get; set; }
        public decimal T13JumlahTandan { get; set; }
        public decimal T13KgTBS { get; set; }
        public decimal T13KgBrondol { get; set; }
        public decimal T13KgLateks { get; set; }
        public decimal T13KgLump { get; set; }
        public decimal T13KgPucuk { get; set; }
        public decimal T13KgLainnya { get; set; }
        public int T13JumlahHK { get; set; }
        public decimal T13JumlahUpah { get; set; }
        public decimal T14LuasJelajah { get; set; }
        public decimal T14JumlahPohon { get; set; }
        public decimal T14JumlahTandan { get; set; }
        public decimal T14KgTBS { get; set; }
        public decimal T14KgBrondol { get; set; }
        public decimal T14KgLateks { get; set; }
        public decimal T14KgLump { get; set; }
        public decimal T14KgPucuk { get; set; }
        public decimal T14KgLainnya { get; set; }
        public int T14JumlahHK { get; set; }
        public decimal T14JumlahUpah { get; set; }
        public decimal T15LuasJelajah { get; set; }
        public decimal T15JumlahPohon { get; set; }
        public decimal T15JumlahTandan { get; set; }
        public decimal T15KgTBS { get; set; }
        public decimal T15KgBrondol { get; set; }
        public decimal T15KgLateks { get; set; }
        public decimal T15KgLump { get; set; }
        public decimal T15KgPucuk { get; set; }
        public decimal T15KgLainnya { get; set; }
        public int T15JumlahHK { get; set; }
        public decimal T15JumlahUpah { get; set; }
        public decimal T16LuasJelajah { get; set; }
        public decimal T16JumlahPohon { get; set; }
        public decimal T16JumlahTandan { get; set; }
        public decimal T16KgTBS { get; set; }
        public decimal T16KgBrondol { get; set; }
        public decimal T16KgLateks { get; set; }
        public decimal T16KgLump { get; set; }
        public decimal T16KgPucuk { get; set; }
        public decimal T16KgLainnya { get; set; }
        public int T16JumlahHK { get; set; }
        public decimal T16JumlahUpah { get; set; }
        public decimal T17LuasJelajah { get; set; }
        public decimal T17JumlahPohon { get; set; }
        public decimal T17JumlahTandan { get; set; }
        public decimal T17KgTBS { get; set; }
        public decimal T17KgBrondol { get; set; }
        public decimal T17KgLateks { get; set; }
        public decimal T17KgLump { get; set; }
        public decimal T17KgPucuk { get; set; }
        public decimal T17KgLainnya { get; set; }
        public int T17JumlahHK { get; set; }
        public decimal T17JumlahUpah { get; set; }
        public decimal T18LuasJelajah { get; set; }
        public decimal T18JumlahPohon { get; set; }
        public decimal T18JumlahTandan { get; set; }
        public decimal T18KgTBS { get; set; }
        public decimal T18KgBrondol { get; set; }
        public decimal T18KgLateks { get; set; }
        public decimal T18KgLump { get; set; }
        public decimal T18KgPucuk { get; set; }
        public decimal T18KgLainnya { get; set; }
        public int T18JumlahHK { get; set; }
        public decimal T18JumlahUpah { get; set; }
        public decimal T19LuasJelajah { get; set; }
        public decimal T19JumlahPohon { get; set; }
        public decimal T19JumlahTandan { get; set; }
        public decimal T19KgTBS { get; set; }
        public decimal T19KgBrondol { get; set; }
        public decimal T19KgLateks { get; set; }
        public decimal T19KgLump { get; set; }
        public decimal T19KgPucuk { get; set; }
        public decimal T19KgLainnya { get; set; }
        public int T19JumlahHK { get; set; }
        public decimal T19JumlahUpah { get; set; }
        public decimal T20LuasJelajah { get; set; }
        public decimal T20JumlahPohon { get; set; }
        public decimal T20JumlahTandan { get; set; }
        public decimal T20KgTBS { get; set; }
        public decimal T20KgBrondol { get; set; }
        public decimal T20KgLateks { get; set; }
        public decimal T20KgLump { get; set; }
        public decimal T20KgPucuk { get; set; }
        public decimal T20KgLainnya { get; set; }
        public int T20JumlahHK { get; set; }
        public decimal T20JumlahUpah { get; set; }
        public decimal T21LuasJelajah { get; set; }
        public decimal T21JumlahPohon { get; set; }
        public decimal T21JumlahTandan { get; set; }
        public decimal T21KgTBS { get; set; }
        public decimal T21KgBrondol { get; set; }
        public decimal T21KgLateks { get; set; }
        public decimal T21KgLump { get; set; }
        public decimal T21KgPucuk { get; set; }
        public decimal T21KgLainnya { get; set; }
        public int T21JumlahHK { get; set; }
        public decimal T21JumlahUpah { get; set; }
        public decimal T22LuasJelajah { get; set; }
        public decimal T22JumlahPohon { get; set; }
        public decimal T22JumlahTandan { get; set; }
        public decimal T22KgTBS { get; set; }
        public decimal T22KgBrondol { get; set; }
        public decimal T22KgLateks { get; set; }
        public decimal T22KgLump { get; set; }
        public decimal T22KgPucuk { get; set; }
        public decimal T22KgLainnya { get; set; }
        public int T22JumlahHK { get; set; }
        public decimal T22JumlahUpah { get; set; }
        public decimal T23LuasJelajah { get; set; }
        public decimal T23JumlahPohon { get; set; }
        public decimal T23JumlahTandan { get; set; }
        public decimal T23KgTBS { get; set; }
        public decimal T23KgBrondol { get; set; }
        public decimal T23KgLateks { get; set; }
        public decimal T23KgLump { get; set; }
        public decimal T23KgPucuk { get; set; }
        public decimal T23KgLainnya { get; set; }
        public int T23JumlahHK { get; set; }
        public decimal T23JumlahUpah { get; set; }
        public decimal T24LuasJelajah { get; set; }
        public decimal T24JumlahPohon { get; set; }
        public decimal T24JumlahTandan { get; set; }
        public decimal T24KgTBS { get; set; }
        public decimal T24KgBrondol { get; set; }
        public decimal T24KgLateks { get; set; }
        public decimal T24KgLump { get; set; }
        public decimal T24KgPucuk { get; set; }
        public decimal T24KgLainnya { get; set; }
        public int T24JumlahHK { get; set; }
        public decimal T24JumlahUpah { get; set; }
        public decimal T25LuasJelajah { get; set; }
        public decimal T25JumlahPohon { get; set; }
        public decimal T25JumlahTandan { get; set; }
        public decimal T25KgTBS { get; set; }
        public decimal T25KgBrondol { get; set; }
        public decimal T25KgLateks { get; set; }
        public decimal T25KgLump { get; set; }
        public decimal T25KgPucuk { get; set; }
        public decimal T25KgLainnya { get; set; }
        public int T25JumlahHK { get; set; }
        public decimal T25JumlahUpah { get; set; }
        public decimal T26LuasJelajah { get; set; }
        public decimal T26JumlahPohon { get; set; }
        public decimal T26JumlahTandan { get; set; }
        public decimal T26KgTBS { get; set; }
        public decimal T26KgBrondol { get; set; }
        public decimal T26KgLateks { get; set; }
        public decimal T26KgLump { get; set; }
        public decimal T26KgPucuk { get; set; }
        public decimal T26KgLainnya { get; set; }
        public int T26JumlahHK { get; set; }
        public decimal T26JumlahUpah { get; set; }
        public decimal T27LuasJelajah { get; set; }
        public decimal T27JumlahPohon { get; set; }
        public decimal T27JumlahTandan { get; set; }
        public decimal T27KgTBS { get; set; }
        public decimal T27KgBrondol { get; set; }
        public decimal T27KgLateks { get; set; }
        public decimal T27KgLump { get; set; }
        public decimal T27KgPucuk { get; set; }
        public decimal T27KgLainnya { get; set; }
        public int T27JumlahHK { get; set; }
        public decimal T27JumlahUpah { get; set; }
        public decimal T28LuasJelajah { get; set; }
        public decimal T28JumlahPohon { get; set; }
        public decimal T28JumlahTandan { get; set; }
        public decimal T28KgTBS { get; set; }
        public decimal T28KgBrondol { get; set; }
        public decimal T28KgLateks { get; set; }
        public decimal T28KgLump { get; set; }
        public decimal T28KgPucuk { get; set; }
        public decimal T28KgLainnya { get; set; }
        public int T28JumlahHK { get; set; }
        public decimal T28JumlahUpah { get; set; }
        public decimal T29LuasJelajah { get; set; }
        public decimal T29JumlahPohon { get; set; }
        public decimal T29JumlahTandan { get; set; }
        public decimal T29KgTBS { get; set; }
        public decimal T29KgBrondol { get; set; }
        public decimal T29KgLateks { get; set; }
        public decimal T29KgLump { get; set; }
        public decimal T29KgPucuk { get; set; }
        public decimal T29KgLainnya { get; set; }
        public int T29JumlahHK { get; set; }
        public decimal T29JumlahUpah { get; set; }
        public decimal T30LuasJelajah { get; set; }
        public decimal T30JumlahPohon { get; set; }
        public decimal T30JumlahTandan { get; set; }
        public decimal T30KgTBS { get; set; }
        public decimal T30KgBrondol { get; set; }
        public decimal T30KgLateks { get; set; }
        public decimal T30KgLump { get; set; }
        public decimal T30KgPucuk { get; set; }
        public decimal T30KgLainnya { get; set; }
        public int T30JumlahHK { get; set; }
        public decimal T30JumlahUpah { get; set; }
        public decimal T31LuasJelajah { get; set; }
        public decimal T31JumlahPohon { get; set; }
        public decimal T31JumlahTandan { get; set; }
        public decimal T31KgTBS { get; set; }
        public decimal T31KgBrondol { get; set; }
        public decimal T31KgLateks { get; set; }
        public decimal T31KgLump { get; set; }
        public decimal T31KgPucuk { get; set; }
        public decimal T31KgLainnya { get; set; }
        public int T31JumlahHK { get; set; }
        public decimal T31JumlahUpah { get; set; }
    }

    public class RotasiPanenPerTahun
    {
        public RotasiPanenPerTahun()
        {
            Tahun = DateTime.Now.Year;
            KodeLokasi = "";
            KodeBudidaya = "";
            NamaBudidaya = "";
            NamaLokasi = "";
            NamaInduk = "";
            TahunTanam = "";
            LuasAreal = 0;
            Warna = "";
            JumlahPohon = 0;
            JumlahRotasi = 0;
            TotalLuasJelajah = 0;
            TotalJumlahPohon = 0;
            TotalJumlahTandan = 0;
            TotalKgTBS = 0;
            TotalKgBrondol = 0;
            TotalKgLateks = 0;
            TotalKgLump = 0;
            TotalKgPucuk = 0;
            TotalKgLainnya = 0;
            TotalJumlahHK = 0;
            TotalJumlahUpah = 0;
            T01LuasJelajah = 0;
            T01JumlahPohon = 0;
            T01JumlahTandan = 0;
            T01KgTBS = 0;
            T01KgBrondol = 0;
            T01KgLateks = 0;
            T01KgLump = 0;
            T01KgPucuk = 0;
            T01KgLainnya = 0;
            T01JumlahHK = 0;
            T01JumlahUpah = 0;
            T02LuasJelajah = 0;
            T02JumlahPohon = 0;
            T02JumlahTandan = 0;
            T02KgTBS = 0;
            T02KgBrondol = 0;
            T02KgLateks = 0;
            T02KgLump = 0;
            T02KgPucuk = 0;
            T02KgLainnya = 0;
            T02JumlahHK = 0;
            T02JumlahUpah = 0;
            T03LuasJelajah = 0;
            T03JumlahPohon = 0;
            T03JumlahTandan = 0;
            T03KgTBS = 0;
            T03KgBrondol = 0;
            T03KgLateks = 0;
            T03KgLump = 0;
            T03KgPucuk = 0;
            T03KgLainnya = 0;
            T03JumlahHK = 0;
            T03JumlahUpah = 0;
            T04LuasJelajah = 0;
            T04JumlahPohon = 0;
            T04JumlahTandan = 0;
            T04KgTBS = 0;
            T04KgBrondol = 0;
            T04KgLateks = 0;
            T04KgLump = 0;
            T04KgPucuk = 0;
            T04KgLainnya = 0;
            T04JumlahHK = 0;
            T04JumlahUpah = 0;
            T05LuasJelajah = 0;
            T05JumlahPohon = 0;
            T05JumlahTandan = 0;
            T05KgTBS = 0;
            T05KgBrondol = 0;
            T05KgLateks = 0;
            T05KgLump = 0;
            T05KgPucuk = 0;
            T05KgLainnya = 0;
            T05JumlahHK = 0;
            T05JumlahUpah = 0;
            T06LuasJelajah = 0;
            T06JumlahPohon = 0;
            T06JumlahTandan = 0;
            T06KgTBS = 0;
            T06KgBrondol = 0;
            T06KgLateks = 0;
            T06KgLump = 0;
            T06KgPucuk = 0;
            T06KgLainnya = 0;
            T06JumlahHK = 0;
            T06JumlahUpah = 0;
            T07LuasJelajah = 0;
            T07JumlahPohon = 0;
            T07JumlahTandan = 0;
            T07KgTBS = 0;
            T07KgBrondol = 0;
            T07KgLateks = 0;
            T07KgLump = 0;
            T07KgPucuk = 0;
            T07KgLainnya = 0;
            T07JumlahHK = 0;
            T07JumlahUpah = 0;
            T08LuasJelajah = 0;
            T08JumlahPohon = 0;
            T08JumlahTandan = 0;
            T08KgTBS = 0;
            T08KgBrondol = 0;
            T08KgLateks = 0;
            T08KgLump = 0;
            T08KgPucuk = 0;
            T08KgLainnya = 0;
            T08JumlahHK = 0;
            T08JumlahUpah = 0;
            T09LuasJelajah = 0;
            T09JumlahPohon = 0;
            T09JumlahTandan = 0;
            T09KgTBS = 0;
            T09KgBrondol = 0;
            T09KgLateks = 0;
            T09KgLump = 0;
            T09KgPucuk = 0;
            T09KgLainnya = 0;
            T09JumlahHK = 0;
            T09JumlahUpah = 0;
            T10LuasJelajah = 0;
            T10JumlahPohon = 0;
            T10JumlahTandan = 0;
            T10KgTBS = 0;
            T10KgBrondol = 0;
            T10KgLateks = 0;
            T10KgLump = 0;
            T10KgPucuk = 0;
            T10KgLainnya = 0;
            T10JumlahHK = 0;
            T10JumlahUpah = 0;
            T11LuasJelajah = 0;
            T11JumlahPohon = 0;
            T11JumlahTandan = 0;
            T11KgTBS = 0;
            T11KgBrondol = 0;
            T11KgLateks = 0;
            T11KgLump = 0;
            T11KgPucuk = 0;
            T11KgLainnya = 0;
            T11JumlahHK = 0;
            T11JumlahUpah = 0;
            T12LuasJelajah = 0;
            T12JumlahPohon = 0;
            T12JumlahTandan = 0;
            T12KgTBS = 0;
            T12KgBrondol = 0;
            T12KgLateks = 0;
            T12KgLump = 0;
            T12KgPucuk = 0;
            T12KgLainnya = 0;
            T12JumlahHK = 0;
            T12JumlahUpah = 0;


        }
        public Guid Id { get; set; }
        public string KodeLokasi { get; set; }
        public string KodeBudidaya { get; set; }
        public string NamaBudidaya { get; set; }
        public string NamaLokasi { get; set; }
        public string NamaInduk { get; set; }
        public string TahunTanam { get; set; }
        public decimal LuasAreal { get; set; }
        public decimal JumlahPohon { get; set; }
        public decimal JumlahRotasi { get; set; }
        public int Tahun { get; set; }
        public string Warna { get; set; }
        public decimal TotalLuasJelajah { get; set; }
        public decimal TotalJumlahPohon { get; set; }
        public decimal TotalJumlahTandan { get; set; }
        public decimal TotalKgTBS { get; set; }
        public decimal TotalKgBrondol { get; set; }
        public decimal TotalKgLateks { get; set; }
        public decimal TotalKgLump { get; set; }
        public decimal TotalKgPucuk { get; set; }
        public decimal TotalKgLainnya { get; set; }
        public int TotalJumlahHK { get; set; }
        public decimal TotalJumlahUpah { get; set; }
        public decimal T01LuasJelajah { get; set; }
        public decimal T01JumlahPohon { get; set; }
        public decimal T01JumlahTandan { get; set; }
        public decimal T01KgTBS { get; set; }
        public decimal T01KgBrondol { get; set; }
        public decimal T01KgLateks { get; set; }
        public decimal T01KgLump { get; set; }
        public decimal T01KgPucuk { get; set; }
        public decimal T01KgLainnya { get; set; }
        public int T01JumlahHK { get; set; }
        public decimal T01JumlahUpah { get; set; }
        public decimal T02LuasJelajah { get; set; }
        public decimal T02JumlahPohon { get; set; }
        public decimal T02JumlahTandan { get; set; }
        public decimal T02KgTBS { get; set; }
        public decimal T02KgBrondol { get; set; }
        public decimal T02KgLateks { get; set; }
        public decimal T02KgLump { get; set; }
        public decimal T02KgPucuk { get; set; }
        public decimal T02KgLainnya { get; set; }
        public int T02JumlahHK { get; set; }
        public decimal T02JumlahUpah { get; set; }
        public decimal T03LuasJelajah { get; set; }
        public decimal T03JumlahPohon { get; set; }
        public decimal T03JumlahTandan { get; set; }
        public decimal T03KgTBS { get; set; }
        public decimal T03KgBrondol { get; set; }
        public decimal T03KgLateks { get; set; }
        public decimal T03KgLump { get; set; }
        public decimal T03KgPucuk { get; set; }
        public decimal T03KgLainnya { get; set; }
        public int T03JumlahHK { get; set; }
        public decimal T03JumlahUpah { get; set; }
        public decimal T04LuasJelajah { get; set; }
        public decimal T04JumlahPohon { get; set; }
        public decimal T04JumlahTandan { get; set; }
        public decimal T04KgTBS { get; set; }
        public decimal T04KgBrondol { get; set; }
        public decimal T04KgLateks { get; set; }
        public decimal T04KgLump { get; set; }
        public decimal T04KgPucuk { get; set; }
        public decimal T04KgLainnya { get; set; }
        public int T04JumlahHK { get; set; }
        public decimal T04JumlahUpah { get; set; }
        public decimal T05LuasJelajah { get; set; }
        public decimal T05JumlahPohon { get; set; }
        public decimal T05JumlahTandan { get; set; }
        public decimal T05KgTBS { get; set; }
        public decimal T05KgBrondol { get; set; }
        public decimal T05KgLateks { get; set; }
        public decimal T05KgLump { get; set; }
        public decimal T05KgPucuk { get; set; }
        public decimal T05KgLainnya { get; set; }
        public int T05JumlahHK { get; set; }
        public decimal T05JumlahUpah { get; set; }
        public decimal T06LuasJelajah { get; set; }
        public decimal T06JumlahPohon { get; set; }
        public decimal T06JumlahTandan { get; set; }
        public decimal T06KgTBS { get; set; }
        public decimal T06KgBrondol { get; set; }
        public decimal T06KgLateks { get; set; }
        public decimal T06KgLump { get; set; }
        public decimal T06KgPucuk { get; set; }
        public decimal T06KgLainnya { get; set; }
        public int T06JumlahHK { get; set; }
        public decimal T06JumlahUpah { get; set; }
        public decimal T07LuasJelajah { get; set; }
        public decimal T07JumlahPohon { get; set; }
        public decimal T07JumlahTandan { get; set; }
        public decimal T07KgTBS { get; set; }
        public decimal T07KgBrondol { get; set; }
        public decimal T07KgLateks { get; set; }
        public decimal T07KgLump { get; set; }
        public decimal T07KgPucuk { get; set; }
        public decimal T07KgLainnya { get; set; }
        public int T07JumlahHK { get; set; }
        public decimal T07JumlahUpah { get; set; }
        public decimal T08LuasJelajah { get; set; }
        public decimal T08JumlahPohon { get; set; }
        public decimal T08JumlahTandan { get; set; }
        public decimal T08KgTBS { get; set; }
        public decimal T08KgBrondol { get; set; }
        public decimal T08KgLateks { get; set; }
        public decimal T08KgLump { get; set; }
        public decimal T08KgPucuk { get; set; }
        public decimal T08KgLainnya { get; set; }
        public int T08JumlahHK { get; set; }
        public decimal T08JumlahUpah { get; set; }
        public decimal T09LuasJelajah { get; set; }
        public decimal T09JumlahPohon { get; set; }
        public decimal T09JumlahTandan { get; set; }
        public decimal T09KgTBS { get; set; }
        public decimal T09KgBrondol { get; set; }
        public decimal T09KgLateks { get; set; }
        public decimal T09KgLump { get; set; }
        public decimal T09KgPucuk { get; set; }
        public decimal T09KgLainnya { get; set; }
        public int T09JumlahHK { get; set; }
        public decimal T09JumlahUpah { get; set; }
        public decimal T10LuasJelajah { get; set; }
        public decimal T10JumlahPohon { get; set; }
        public decimal T10JumlahTandan { get; set; }
        public decimal T10KgTBS { get; set; }
        public decimal T10KgBrondol { get; set; }
        public decimal T10KgLateks { get; set; }
        public decimal T10KgLump { get; set; }
        public decimal T10KgPucuk { get; set; }
        public decimal T10KgLainnya { get; set; }
        public int T10JumlahHK { get; set; }
        public decimal T10JumlahUpah { get; set; }
        public decimal T11LuasJelajah { get; set; }
        public decimal T11JumlahPohon { get; set; }
        public decimal T11JumlahTandan { get; set; }
        public decimal T11KgTBS { get; set; }
        public decimal T11KgBrondol { get; set; }
        public decimal T11KgLateks { get; set; }
        public decimal T11KgLump { get; set; }
        public decimal T11KgPucuk { get; set; }
        public decimal T11KgLainnya { get; set; }
        public int T11JumlahHK { get; set; }
        public decimal T11JumlahUpah { get; set; }
        public decimal T12LuasJelajah { get; set; }
        public decimal T12JumlahPohon { get; set; }
        public decimal T12JumlahTandan { get; set; }
        public decimal T12KgTBS { get; set; }
        public decimal T12KgBrondol { get; set; }
        public decimal T12KgLateks { get; set; }
        public decimal T12KgLump { get; set; }
        public decimal T12KgPucuk { get; set; }
        public decimal T12KgLainnya { get; set; }
        public int T12JumlahHK { get; set; }
        public decimal T12JumlahUpah { get; set; }
    }


}