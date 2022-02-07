namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AkunKomentar")]
    public partial class AkunKomentar
    {
        public Guid AkunKomentarID { get; set; }

        [StringLength(2)]
        public string kodeunit { get; set; }

        public DateTime? tanggal { get; set; }

        [StringLength(50)]
        public string kdtrans { get; set; }

        [StringLength(2)]
        public string kodebud { get; set; }

        [StringLength(50)]
        public string kodeolah { get; set; }

        [StringLength(50)]
        public string kodekom { get; set; }

        public string uraian { get; set; }
    }
}
