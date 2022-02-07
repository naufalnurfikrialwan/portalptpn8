namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AkunRKAP")]
    public partial class AkunRKAP
    {
        public Guid AkunRKAPID { get; set; }

        [StringLength(2)]
        public string kodeunit { get; set; }

        public DateTime? tanggal { get; set; }

        [StringLength(2)]
        public string unsur { get; set; }

        [StringLength(2)]
        public string kodeafd { get; set; }

        [StringLength(2)]
        public string kodebud { get; set; }

        [StringLength(9)]
        public string norek { get; set; }

        [StringLength(50)]
        public string netral { get; set; }

        [StringLength(50)]
        public string kodebarang { get; set; }

        public double? qtyBrg_RKAP { get; set; }

        public double? qtyBrg_PMK { get; set; }

        [StringLength(1)]
        public string klsKary { get; set; }

        public double? jmlHK_RKAP { get; set; }

        public double? jmlHsl_RKAP { get; set; }

        public double? jmlKary_RKAP { get; set; }

        public double? nilai_RKAP { get; set; }

        public double? jmlHK_PMK { get; set; }

        public double? jmlHsl_PMK { get; set; }

        public double? jmlKary_PMK { get; set; }

        public double? nilai_PMK { get; set; }

        [StringLength(4)]
        public string kodeblok { get; set; }
        [StringLength(4)]
        public string thntnm { get; set; }
        public double? HaJelajah_RKAP { get; set; }
        public double? HaJelajah_PMK { get; set; }


    }

    public class RKAPperKebun
    {
        public int Tahun { get; set; }
        public int Bulan { get; set; }
        public string KodeKebun { get; set; }
        public Guid KebunId { get; set; }
        public string NamaKebun { get; set; }
        public string KodeBudidaya { get; set; }
        public Guid MainBudidayaId { get; set; }
        public string NamaBudidaya { get; set; }
        public string KodeUnsur { get; set; }
        public string StatusKaryawan { get; set; }
        public string KodeRekening { get; set; }
        public double? JumlahKaryawanRKAP { get; set; }
        public double? JumlahHKRKAP { get; set; }
        public double? JumlahHasilKerjaRKAP { get; set; }
        public double? NilaiRKAP { get; set; }
        public double? JumlahKaryawanPKB { get; set; }
        public double? JumlahHKPKB { get; set; }
        public double? JumlahHasilKerjaPKB { get; set; }
        public double? NilaiPKB { get; set; }

    }

    public class RKAPperAfdeling
    {
        public int Tahun { get; set; }
        public int Bulan { get; set; }
        public string KodeAfdeling { get; set; }
        public Guid AfdelingId { get; set; }
        public string NamaAfdeling { get; set; }
        public Guid KebunId { get; set; }
        public string KodeBudidaya { get; set; }
        public Guid MainBudidayaId { get; set; }
        public string NamaBudidaya { get; set; }
        public string KodeUnsur { get; set; }
        public string StatusKaryawan { get; set; }
        public string KodeRekening { get; set; }
        public double? JumlahKaryawanRKAP { get; set; }
        public double? JumlahHKRKAP { get; set; }
        public double? JumlahHasilKerjaRKAP { get; set; }
        public double? NilaiRKAP { get; set; }
        public double? JumlahKaryawanPKB { get; set; }
        public double? JumlahHKPKB { get; set; }
        public double? JumlahHasilKerjaPKB { get; set; }
        public double? NilaiPKB { get; set; }

    }

    public class RKAPperBlok
    {
        public int Tahun { get; set; }
        public int Bulan { get; set; }
        public string KodeBlok { get; set; }
        public Guid BlokId { get; set; }
        public string TahunTanam { get; set; }
        public string KodeAfdeling { get; set; }
        public Guid AfdelingId { get; set; }
        public string NamaAfdeling { get; set; }
        public Guid KebunId { get; set; }
        public string KodeBudidaya { get; set; }
        public Guid MainBudidayaId { get; set; }
        public string NamaBudidaya { get; set; }
        public string KodeUnsur { get; set; }
        public string StatusKaryawan { get; set; }
        public string KodeRekening { get; set; }
        public double? JumlahKaryawanRKAP { get; set; }
        public double? JumlahHKRKAP { get; set; }
        public double? JumlahHasilKerjaRKAP { get; set; }
        public double? NilaiRKAP { get; set; }
        public double? JumlahKaryawanPKB { get; set; }
        public double? JumlahHKPKB { get; set; }
        public double? JumlahHasilKerjaPKB { get; set; }
        public double? NilaiPKB { get; set; }

    }
}
