namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GajiAbsensi")]
    public partial class GajiAbsensi
    {
        public Guid GajiAbsensiID { get; set; }

        [StringLength(50)]
        public string kodeunit { get; set; }
        public DateTime? tanggal { get; set; }

        [StringLength(50)]
        public string kodeafd { get; set; }

        [StringLength(50)]
        public string kodeafd1 { get; set; }

        [StringLength(50)]
        public string kodetrans { get; set; }

        [StringLength(50)]
        public string kodebud { get; set; }

        [StringLength(50)]
        public string kodebud1 { get; set; }

        [StringLength(50)]
        public string regmdr { get; set; }

        [StringLength(50)]
        public string nama { get; set; }

        [StringLength(2)]
        public string sts { get; set; }

        [StringLength(50)]
        public string register { get; set; }

        [StringLength(1)]
        public string kodeabs { get; set; }

        [StringLength(50)]
        public string norek { get; set; }

        [StringLength(50)]
        public string netral { get; set; }

        [StringLength(50)]
        public string thntnm { get; set; }

        [StringLength(50)]
        public string tkesulitan { get; set; }

        [StringLength(50)]
        public string kdblok { get; set; }

        [StringLength(1)]
        public string kdjnsupah { get; set; }

        [StringLength(50)]
        public string kdgrade { get; set; }

        [StringLength(1)]
        public string kdpanen { get; set; }

        [StringLength(1)]
        public string stpikul { get; set; }

        [StringLength(50)]
        public string grup { get; set; }

        public double? jelajahHA { get; set; }

        public double? jmlkg { get; set; }

        public double? basic { get; set; }

        public double? hrgsatuan { get; set; }

        public double? hrglatex { get; set; }

        public double? hrglump { get; set; }

        public double? hslpanenTBS { get; set; }

        public double? hslpanen { get; set; }

        public double? hslpanenlump { get; set; }

        public double? hslpemel { get; set; }

        public double? hsllain2 { get; set; }

        [StringLength(50)]
        public string satuan { get; set; }

        [StringLength(1)]
        public string sbrd { get; set; }

        public double? nbrd { get; set; }

        public double? premipikul { get; set; }

        public double? selmkg { get; set; }

        public double? tpb { get; set; }

        public double? jmlupah { get; set; }

        public double? premi { get; set; }

        public double? jmlhadir { get; set; }

        public double? prekual { get; set; }

        public double? prekuan { get; set; }

        [StringLength(1)]
        public string kodekelas { get; set; }

        public bool? stprestasi { get; set; }

        public double? pct { get; set; }

        public double? CutiTahunan { get; set; }

        public double? CutiPanjang { get; set; }

        public double? CutiHamil { get; set; }

        public double? Sakit { get; set; }

        public double? Ijin { get; set; }

        public double? Mangkir { get; set; }

        [StringLength(50)]
        public string UserID { get; set; }

        public DateTime? TglInput { get; set; }
    }

    public class GajiAbsensiPerKebun
    {
        
        //public Guid Id { get; set; }
        public string KodeKebun { get; set; }
        //public string NamaKebun { get; set; }
        //public string NamaWilayah { get; set; }
        public int Bulan { get; set; }
        public int Tahun { get; set; }
        public string KodeBudidaya { get; set; }
        //public string NamaBudidaya { get; set; }
        //public Guid MainBudidayaId { get; set; }
        public string StatusKaryawan { get; set; }
        public string KodeAbsen { get; set; }
        public string KodeRekening { get; set; }
        //public string NamaRekening { get; set; }
        //public decimal LuasAreal { get; set; }
        //public string KodePanen { get; set; }
        public int RealisasiJumlahKaryawan { get; set; }
        public int RealisasiJumlahHK { get; set; }

        public double? RealisasiHasilKerja { get; set; }
        public double? RealisasiHasilKerjaLump { get; set; }
        public double? RealisasiHasilKerjaTandan { get; set; }
        public double? RealisasiJumlahUpah { get; set; }
        public double? RealisasiBiayaLain { get; set; }

        public double? RKAPJumlahKaryawan { get; set; }
        public double? RKAPJumlahHK { get; set; }
        public double? RKAPHasilKerja { get; set; }
        public double? RKAPJumlahUpah { get; set; }

        public double? PKBJumlahKaryawan { get; set; }
        public double? PKBJumlahHK { get; set; }
        public double? PKBHasilKerja { get; set; }
        public double? PKBJumlahUpah { get; set; }
        public double? RealisasiJelajah { get; set; }
        public double? RKAPJelajah { get; set; }
        public double? PKBJelajah { get; set; }
        public string GrupPanen { get; set; }
    }
    public class GajiAbsensiPerAfdeling
    {
        public Guid Id { get; set; }
        public string KodeAfdeling { get; set; }
        public string NamaAfdeling { get; set; }
        public string NamaKebun { get; set; }
        public Guid KebunId { get; set; }
        public int Bulan { get; set; }
        public int Tahun { get; set; }
        public string KodeBudidaya { get; set; }
        public string NamaBudidaya { get; set; }
        public Guid MainBudidayaId { get; set; }
        public string StatusKaryawan { get; set; }
        public string KodeAbsen { get; set; }
        public string KodeRekening { get; set; }
        public string NamaRekening { get; set; }
        //public string KodePanen { get; set; }
        public decimal LuasAreal { get; set; }
        public int RealisasiJumlahKaryawan { get; set; }
        public int RealisasiJumlahHK { get; set; }
        public double? RealisasiHasilKerja { get; set; }
        public double? RealisasiHasilKerjaLump { get; set; }
        public double? RealisasiHasilKerjaTandan { get; set; }
        public double? RealisasiJumlahUpah { get; set; }
        public double? RealisasiBiayaLain { get; set; }


        public double? RKAPJumlahKaryawan { get; set; }
        public double? RKAPJumlahHK { get; set; }
        public double? RKAPHasilKerja { get; set; }
        public double? RKAPJumlahUpah { get; set; }

        public double? PKBJumlahKaryawan { get; set; }
        public double? PKBJumlahHK { get; set; }
        public double? PKBHasilKerja { get; set; }
        public double? PKBJumlahUpah { get; set; }
        public double? RealisasiJelajah { get; set; }
        public double? RKAPJelajah { get; set; }
        public double? PKBJelajah { get; set; }
        public string GrupPanen { get; set; }
    }

    public class GajiAbsensiPerBlok
    {
        public GajiAbsensiPerBlok()
        {
            RealisasiJumlahKaryawan = 0;
            RealisasiJumlahHK = 0;
            RealisasiHasilKerja = 0;
            RealisasiHasilKerjaLump = 0;
            RealisasiHasilKerjaTandan = 0;
            RealisasiJumlahUpah = 0;
            RealisasiBiayaLain = 0;
            RKAPJumlahKaryawan = 0;
            RKAPJumlahHK = 0;
            RKAPHasilKerja = 0;
            RKAPJumlahUpah = 0;
            PKBJumlahKaryawan = 0;
            PKBJumlahHK = 0;
            PKBHasilKerja = 0;
            PKBJumlahUpah = 0;
            RealisasiJelajah = 0;
            RKAPJelajah = 0;
            PKBJelajah = 0;
        }
    public Guid Id { get; set; }
        public string KodeBlok { get; set; }
        public string NamaBlok { get; set; }
        public decimal LuasAreal { get; set; }
        public string TahunTanam { get; set; }
        public string NamaAfdeling { get; set; }
        public Guid AfdelingId { get; set; }
        public int Bulan { get; set; }
        public int Tahun { get; set; }
        public string KodeBudidaya { get; set; }
        public string NamaBudidaya { get; set; }
        public Guid MainBudidayaId { get; set; }
        public string StatusKaryawan { get; set; }
        public string KodeAbsen { get; set; }
        public string KodeRekening { get; set; }
        public string NamaRekening { get; set; }
        //public string KodePanen { get; set; }
        public int RealisasiJumlahKaryawan { get; set; }
        public int RealisasiJumlahHK { get; set; }
        public double? RealisasiHasilKerja { get; set; }
        public double? RealisasiHasilKerjaLump { get; set; }
        public double? RealisasiHasilKerjaTandan { get; set; }
        public double? RealisasiJumlahUpah { get; set; }
        public double? RealisasiBiayaLain { get; set; }
        public double? RKAPJumlahKaryawan { get; set; }
        public double? RKAPJumlahHK { get; set; }
        public double? RKAPHasilKerja { get; set; }
        public double? RKAPJumlahUpah { get; set; }
        public double? PKBJumlahKaryawan { get; set; }
        public double? PKBJumlahHK { get; set; }
        public double? PKBHasilKerja { get; set; }
        public double? PKBJumlahUpah { get; set; }
        public double? RealisasiJelajah { get; set; }
        public double? RKAPJelajah { get; set; }
        public double? PKBJelajah { get; set; }
        public string GrupPanen { get; set; }
    }
}
