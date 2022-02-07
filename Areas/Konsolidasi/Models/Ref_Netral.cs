namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_Netral
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(2)]
        public string KodeUnit { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(3)]
        public string Norek { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string Nomor { get; set; }

        [StringLength(50)]
        public string Uraian { get; set; }

        [StringLength(4)]
        public string Tahun { get; set; }

        public bool? status { get; set; }

        [StringLength(2)]
        public string BBM { get; set; }

        [StringLength(2)]
        public string Pemilik { get; set; }

        [StringLength(50)]
        public string Pengemudi { get; set; }

        [StringLength(50)]
        public string NoChasis { get; set; }

        [StringLength(50)]
        public string NoMesin { get; set; }

        [StringLength(50)]
        public string NoBPKB { get; set; }

        [StringLength(50)]
        public string Tonage { get; set; }

        [StringLength(50)]
        public string UkuranBan { get; set; }

        public DateTime? TglBeli { get; set; }

        [StringLength(50)]
        public string Voltage { get; set; }

        [StringLength(50)]
        public string BebanRekening { get; set; }
    }
}
