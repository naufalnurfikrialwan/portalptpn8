namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GajiPremiPanen")]
    public partial class GajiPremiPanen
    {
        public Guid GajiPremiPanenID { get; set; }

        [StringLength(2)]
        public string kodeunit { get; set; }

        [StringLength(50)]
        public string kodetrans { get; set; }

        public DateTime? tanggal { get; set; }

        [StringLength(2)]
        public string kodebud { get; set; }

        [StringLength(2)]
        public string kodeafd { get; set; }

        [StringLength(50)]
        public string sts { get; set; }

        [StringLength(50)]
        public string regmdr { get; set; }

        [StringLength(50)]
        public string register { get; set; }

        [StringLength(50)]
        public string nama { get; set; }

        [StringLength(50)]
        public string norek { get; set; }

        [StringLength(50)]
        public string tkesulitan { get; set; }

        [StringLength(50)]
        public string kodepanen { get; set; }

        public double? jmlhasil { get; set; }

        public double? basic { get; set; }

        public double? target { get; set; }

        public double? rerata { get; set; }

        public double? PCTRendement { get; set; }

        public double? jmlkary { get; set; }

        public double? jmlhk { get; set; }

        [StringLength(50)]
        public string ndxmutu { get; set; }

        [StringLength(50)]
        public string klas { get; set; }

        public double? rbt { get; set; }

        public double? pkualitas { get; set; }

        public double? pkuantitas { get; set; }

        public double? denda { get; set; }

        public double? PCTpmandor { get; set; }

        public double? jmlpremi { get; set; }

        public double? pmandor { get; set; }
    }
}
