namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GajiSPKHonor")]
    public partial class GajiSPKHonor
    {
        public Guid GajiSPKHonorID { get; set; }

        [StringLength(50)]
        public string kodeunit { get; set; }

        [StringLength(3)]
        public string kodetrans { get; set; }

        public DateTime? tanggal { get; set; }

        [StringLength(50)]
        public string NoSPK { get; set; }

        [StringLength(3)]
        public string nourut { get; set; }

        [StringLength(50)]
        public string kodeafd { get; set; }

        [StringLength(50)]
        public string kodebud { get; set; }

        [StringLength(50)]
        public string nama { get; set; }

        [StringLength(1)]
        public string kdjnsupah { get; set; }

        [StringLength(50)]
        public string norek { get; set; }

        [StringLength(50)]
        public string netral { get; set; }

        [StringLength(50)]
        public string kdblok { get; set; }

        [StringLength(50)]
        public string thntnm { get; set; }

        public double? jmlhadir { get; set; }

        public double? hrgsatuan { get; set; }

        public double? lain2 { get; set; }

        public double? potongan { get; set; }

        public double? pph { get; set; }

        public double? jmlhasil { get; set; }

        public double? jmlupah { get; set; }
    }
}
