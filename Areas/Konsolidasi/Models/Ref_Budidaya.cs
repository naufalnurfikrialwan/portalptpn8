namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_Budidaya
    {
        [Key]
        [Column(Order = 0)]
        public Guid Ref_BudidayaID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string KodeUnit { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(4)]
        public string kd_bud { get; set; }

        [StringLength(50)]
        public string nm_bud { get; set; }

        public bool? inti { get; set; }
    }
}
