namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KasBank")]
    public partial class KasBank
    {
        public Guid KasBankID { get; set; }

        [StringLength(2)]
        public string KodeUnit { get; set; }

        [StringLength(2)]
        public string KT { get; set; }

        [StringLength(3)]
        public string Inisial { get; set; }

        [StringLength(50)]
        public string NoInput { get; set; }

        public DateTime? Tanggal { get; set; }

        [StringLength(50)]
        public string Nama { get; set; }

        [StringLength(50)]
        public string Alamat { get; set; }

        [StringLength(50)]
        public string Ket { get; set; }

        [StringLength(9)]
        public string RekKB { get; set; }

        [StringLength(3)]
        public string NoUrut { get; set; }

        [StringLength(4)]
        public string KdBud { get; set; }

        [StringLength(2)]
        public string kdAfd { get; set; }

        [StringLength(9)]
        public string Norek { get; set; }

        [StringLength(10)]
        public string Netral { get; set; }

        [StringLength(50)]
        public string Kdblok { get; set; }

        public string Uraian { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Nilai { get; set; }

        public double? Jml_Fisik { get; set; }

        [StringLength(4)]
        public string KdArus { get; set; }

        public DateTime? TglKom { get; set; }

        [StringLength(50)]
        public string namauser { get; set; }

        [StringLength(4)]
        public string thntnm { get; set; }
    }
}
