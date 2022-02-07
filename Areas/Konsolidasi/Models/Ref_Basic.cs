namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_Basic
    {
        public Guid Ref_BasicID { get; set; }

        public DateTime? tanggal { get; set; }

        [StringLength(50)]
        public string kode { get; set; }

        [StringLength(2)]
        public string KodeUnit { get; set; }

        [StringLength(2)]
        public string kodeafd { get; set; }

        [StringLength(2)]
        public string kodebud { get; set; }

        [StringLength(50)]
        public string regmdr { get; set; }

        [StringLength(50)]
        public string register { get; set; }

        [StringLength(50)]
        public string nama { get; set; }

        [StringLength(2)]
        public string sts { get; set; }

        [StringLength(50)]
        public string kdblok { get; set; }

        [StringLength(4)]
        public string thntnm { get; set; }

        [StringLength(1)]
        public string kdpanen { get; set; }

        public double? target { get; set; }

        public double? rkap { get; set; }

        [StringLength(50)]
        public string tkesulitan { get; set; }

        public double? basicyld { get; set; }

        public double? basicyldbrd { get; set; }

        public double? basicjlj { get; set; }

        public double? jmlhari { get; set; }

        public double? jmlorg { get; set; }

        public double? premibrd { get; set; }
    }
}
