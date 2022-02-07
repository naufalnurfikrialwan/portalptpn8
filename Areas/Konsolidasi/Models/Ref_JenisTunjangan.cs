namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_JenisTunjangan
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string JnsTunjangan { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string kdjabatan { get; set; }

        [StringLength(50)]
        public string nmjabatan { get; set; }
    }
}
