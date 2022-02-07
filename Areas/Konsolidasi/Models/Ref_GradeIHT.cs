namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_GradeIHT
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(2)]
        public string kd_bud { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string kd_jnsprod { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(4)]
        public string kd_grade { get; set; }

        [StringLength(50)]
        public string uraian { get; set; }

        [StringLength(2)]
        public string kd_satuan { get; set; }

        [StringLength(50)]
        public string satuan { get; set; }
    }
}
