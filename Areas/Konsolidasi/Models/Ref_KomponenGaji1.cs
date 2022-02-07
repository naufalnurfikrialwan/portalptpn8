namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_KomponenGaji1
    {
        [StringLength(255)]
        public string GOLBARU { get; set; }

        [StringLength(50)]
        public string GOL { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(2)]
        public string KDGOL { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string MK { get; set; }

        public double? GPO { get; set; }

        public double? T_TETAP { get; set; }

        public double? B_PERSH { get; set; }

        public double? B_PRIB { get; set; }

        public double? A_PERSH { get; set; }

        public double? A_PRIB { get; set; }

        public double? PIP_PERS { get; set; }

        public double? PIP_PRIB { get; set; }
    }
}
