namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GajiPremi")]
    public partial class GajiPremi
    {
        public Guid GajiPremiID { get; set; }

        public DateTime? tanggal { get; set; }

        [StringLength(50)]
        public string kodeunit { get; set; }

        [StringLength(50)]
        public string kodetrans { get; set; }

        [StringLength(50)]
        public string kodeAfd { get; set; }

        [StringLength(50)]
        public string kodeAfd1 { get; set; }

        [StringLength(50)]
        public string kodebud { get; set; }

        [StringLength(50)]
        public string kodebud1 { get; set; }

        [StringLength(50)]
        public string jnsprm { get; set; }

        [StringLength(50)]
        public string sts { get; set; }

        [StringLength(50)]
        public string regmdr { get; set; }

        [StringLength(50)]
        public string register { get; set; }

        [StringLength(50)]
        public string nama { get; set; }

        [StringLength(50)]
        public string norek { get; set; }

        [StringLength(50)]
        public string netral { get; set; }

        [StringLength(50)]
        public string thntnm { get; set; }

        public double? jmlfisik { get; set; }

        public double? nilai { get; set; }
    }
}
