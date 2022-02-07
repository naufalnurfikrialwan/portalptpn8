namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_KodeLegalitas
    {
        [Key]
        [StringLength(3)]
        public string kode { get; set; }

        [StringLength(50)]
        public string uraian { get; set; }
    }
}
