namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_LM
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(2)]
        public string kodeunit { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(3)]
        public string kode { get; set; }

        [StringLength(50)]
        public string nmMenu { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string kdgrup { get; set; }

        [StringLength(100)]
        public string nmgrup { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string urut { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string norek { get; set; }

        [StringLength(100)]
        public string uraian { get; set; }

        [StringLength(50)]
        public string tampil { get; set; }
    }
}
