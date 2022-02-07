namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_NoBuktiJurnalGaji
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string kodepdp { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string nourut { get; set; }

        [StringLength(50)]
        public string namapdp { get; set; }

        [StringLength(50)]
        public string nobukti { get; set; }
    }
}
