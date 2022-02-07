namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_RekeningInvestasi
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(3)]
        public string KRBB { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(9)]
        public string Norek { get; set; }

        [StringLength(50)]
        public string NamaRekening { get; set; }
    }
}
