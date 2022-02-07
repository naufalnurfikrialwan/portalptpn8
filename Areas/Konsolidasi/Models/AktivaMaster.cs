namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AktivaMaster")]
    public partial class AktivaMaster
    {
        public Guid AktivaMasterID { get; set; }

        [StringLength(2)]
        public string kodeunit { get; set; }

        public DateTime? tgltran { get; set; }

        [StringLength(50)]
        public string nobukti { get; set; }

        [StringLength(50)]
        public string noaktiva { get; set; }

        [StringLength(2)]
        public string kdafd { get; set; }

        [StringLength(2)]
        public string kdbud { get; set; }

        [StringLength(3)]
        public string kelompok { get; set; }

        [StringLength(9)]
        public string norek { get; set; }

        public DateTime? tglbeli { get; set; }

        [StringLength(100)]
        public string nmaktiva { get; set; }

        [StringLength(2)]
        public string kelompokaset { get; set; }

        public double? tarifpct { get; set; }

        public double? jmlunit { get; set; }

        [StringLength(50)]
        public string satuan { get; set; }

        public double? sawalp { get; set; }

        public double? auditp { get; set; }

        public double? rcp { get; set; }

        public double? hapusp { get; set; }

        public double? rekp { get; set; }

        public double? kbnp { get; set; }

        public double? murnip { get; set; }

        public double? sawals { get; set; }

        public double? audits { get; set; }

        public double? rcs { get; set; }

        public double? hapuss { get; set; }

        public double? kbns { get; set; }

        public double? reks { get; set; }

        public double? murnis { get; set; }

        [StringLength(1)]
        public string stopr { get; set; }

        [StringLength(4)]
        public string thntnm { get; set; }

        public double? kondisi { get; set; }
    }
}
