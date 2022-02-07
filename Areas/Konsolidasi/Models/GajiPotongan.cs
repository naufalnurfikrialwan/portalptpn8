namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GajiPotongan")]
    public partial class GajiPotongan
    {
        public Guid GajiPotonganID { get; set; }

        [StringLength(50)]
        public string kodeunit { get; set; }

        public DateTime? tanggal { get; set; }

        [StringLength(50)]
        public string kodeafd { get; set; }

        [StringLength(50)]
        public string kodeafd1 { get; set; }

        [StringLength(50)]
        public string kodebud { get; set; }

        [StringLength(50)]
        public string kodebud1 { get; set; }

        [StringLength(50)]
        public string sts { get; set; }

        [StringLength(50)]
        public string regmdr { get; set; }

        [StringLength(50)]
        public string kodetrans { get; set; }

        [StringLength(50)]
        public string jnsptg { get; set; }

        [StringLength(50)]
        public string register { get; set; }

        [StringLength(50)]
        public string nama { get; set; }

        public double? nilai { get; set; }

        public double? potongan { get; set; }

        [StringLength(50)]
        public string keterangan { get; set; }
    }
}
