namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AktivaTransaksi")]
    public partial class AktivaTransaksi
    {
        public Guid AktivaTransaksiID { get; set; }

        [StringLength(2)]
        public string kodeunit { get; set; }

        public DateTime? tanggal { get; set; }

        [StringLength(3)]
        public string nourut { get; set; }

        [StringLength(50)]
        public string noinput { get; set; }

        [StringLength(3)]
        public string kelompok { get; set; }

        [StringLength(50)]
        public string noaktiva { get; set; }

        [StringLength(50)]
        public string noaktivab { get; set; }

        public double? tarifpct { get; set; }

        [StringLength(100)]
        public string nmaktiva { get; set; }

        [StringLength(2)]
        public string kdafd { get; set; }

        [StringLength(2)]
        public string kdbud { get; set; }

        [StringLength(9)]
        public string norek { get; set; }

        [StringLength(9)]
        public string norekl { get; set; }

        public DateTime? tglbeli { get; set; }

        [StringLength(50)]
        public string nobukti { get; set; }

        public DateTime? tglbukti { get; set; }

        [StringLength(50)]
        public string leveransir { get; set; }

        public double? jmlunit { get; set; }

        public double? jmlunit1 { get; set; }

        [StringLength(50)]
        public string satuan { get; set; }

        public double? nilaiperolehan { get; set; }

        public decimal? nilaipenyusutan { get; set; }

        public decimal? nilaipenyusutan_thl { get; set; }

        public decimal? nilaipenyusutan_sbl { get; set; }

        public decimal? nilaikoreksipenyusutan { get; set; }

        [StringLength(2)]
        public string kelompokaset { get; set; }

        [StringLength(4)]
        public string thntnm { get; set; }

        public bool? statedit { get; set; }
    }
}
