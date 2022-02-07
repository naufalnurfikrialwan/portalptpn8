namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Eap")]
    public partial class Eap
    {
        public Guid EapID { get; set; }

        [StringLength(50)]
        public string kodeunit { get; set; }

        [StringLength(50)]
        public string kdtrans { get; set; }

        public DateTime? tanggal { get; set; }

        [StringLength(50)]
        public string noinput { get; set; }

        [StringLength(50)]
        public string nourut { get; set; }

        [StringLength(50)]
        public string nomor { get; set; }

        [StringLength(50)]
        public string pengemudi { get; set; }

        [StringLength(50)]
        public string pembantu { get; set; }

        [StringLength(50)]
        public string namapemakai { get; set; }

        [StringLength(50)]
        public string kdafd { get; set; }

        [StringLength(50)]
        public string kdbud { get; set; }

        [StringLength(50)]
        public string asal { get; set; }

        [StringLength(50)]
        public string tujuan { get; set; }

        [StringLength(50)]
        public string waktukeluar { get; set; }

        [StringLength(50)]
        public string waktumasuk { get; set; }

        public int? kmawal { get; set; }

        public int? kmakhir { get; set; }

        public int? jmlkm { get; set; }

        public double? jmlfisik { get; set; }

        [StringLength(50)]
        public string norek { get; set; }

        public double? nilai { get; set; }

        [StringLength(50)]
        public string netral { get; set; }

        public DateTime? tglmasuk { get; set; }

        public double? jmljam { get; set; }

        [StringLength(50)]
        public string kdblok { get; set; }

        [StringLength(4)]
        public string thntnm { get; set; }

        [StringLength(3)]
        public string krbb { get; set; }

        [StringLength(50)]
        public string UserID { get; set; }

        public DateTime? TglInput { get; set; }
    }
}
