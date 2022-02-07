namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("xyz")]
    public partial class xyz
    {
        [Key]
        [StringLength(4)]
        public string kd_bud { get; set; }

        [StringLength(50)]
        public string nm_bud { get; set; }
    }
}
