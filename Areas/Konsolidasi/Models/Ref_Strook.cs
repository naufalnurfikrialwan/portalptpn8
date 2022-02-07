namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_Strook
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(2)]
        public string Grup { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string urut { get; set; }

        [StringLength(50)]
        public string kode { get; set; }

        [StringLength(50)]
        public string uraian { get; set; }
    }
}
