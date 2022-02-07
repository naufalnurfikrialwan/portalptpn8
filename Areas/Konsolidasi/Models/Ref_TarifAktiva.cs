namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_TarifAktiva
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(2)]
        public string kdbud { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string Norek { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string kelompokaset { get; set; }

        [StringLength(50)]
        public string Keterangan { get; set; }

        [StringLength(50)]
        public string Uraian { get; set; }

        public double? Tarif { get; set; }

        [StringLength(50)]
        public string Tahun { get; set; }
    }
}
