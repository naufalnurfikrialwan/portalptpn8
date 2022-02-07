namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GajiTaksasi")]
    public partial class GajiTaksasi
    {
        public Guid GajiTaksasiID { get; set; }

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

        public double? hslpanenTBS { get; set; }

        public double? hslpanen { get; set; }

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
}
