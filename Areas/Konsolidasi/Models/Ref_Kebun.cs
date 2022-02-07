namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_Kebun
    {
        [Key]
        [StringLength(2)]
        public string KodeKebun { get; set; }

        [StringLength(50)]
        public string NamaKebun { get; set; }

        public bool? Status { get; set; }
    }
}
