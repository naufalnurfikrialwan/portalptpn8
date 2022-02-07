namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_Satuan
    {
        [Key]
        [StringLength(2)]
        public string KodeSatuan { get; set; }

        [StringLength(20)]
        public string NamaSatuan { get; set; }

        [StringLength(5)]
        public string Inisial { get; set; }
    }
}
