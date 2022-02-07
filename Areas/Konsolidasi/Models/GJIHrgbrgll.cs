namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GJIHrgbrgll")]
    public partial class GJIHrgbrgll
    {
        public Guid GJIHrgbrgllID { get; set; }

        [StringLength(2)]
        public string kodeunit { get; set; }

        public DateTime? tanggal { get; set; }

        [StringLength(2)]
        public string kodeafd { get; set; }

        [StringLength(4)]
        public string thntnm { get; set; }

        public double? HrgLatex { get; set; }

        public double? HrgLump { get; set; }
    }
}
