namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_AsalProd
    {
        [Key]
        [Column(Order = 0)]
        public Guid Ref_AsalProdID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string kdasal { get; set; }

        [StringLength(50)]
        public string uraian { get; set; }

        [StringLength(50)]
        public string rekBasah { get; set; }

        [StringLength(50)]
        public string rekJadi { get; set; }
    }
}
