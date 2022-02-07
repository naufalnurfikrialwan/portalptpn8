namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_ResolusiLayar
    {
        [Key]
        [StringLength(50)]
        public string Resolusi { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        [StringLength(50)]
        public string Description { get; set; }
    }
}
