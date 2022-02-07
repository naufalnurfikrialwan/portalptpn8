namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_JenisUpah
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(4)]
        public string kodebud { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(1)]
        public string kodeupah { get; set; }

        [StringLength(50)]
        public string uraian { get; set; }
    }
}
