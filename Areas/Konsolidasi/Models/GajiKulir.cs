namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GajiKulir")]
    public partial class GajiKulir
    {
        public Guid GajiKulirID { get; set; }

        [StringLength(50)]
        public string kodeunit { get; set; }

        public DateTime? tanggal { get; set; }

        [StringLength(3)]
        public string kodetrans { get; set; }

        [StringLength(50)]
        public string kodeafd { get; set; }

        [StringLength(50)]
        public string kodebud { get; set; }

        [StringLength(50)]
        public string regmdr { get; set; }

        [StringLength(50)]
        public string register { get; set; }

        [StringLength(50)]
        public string sts { get; set; }

        [StringLength(50)]
        public string norek { get; set; }

        [StringLength(50)]
        public string netral { get; set; }

        [StringLength(50)]
        public string kdblok { get; set; }

        [StringLength(50)]
        public string thntnm { get; set; }

        [StringLength(50)]
        public string stb { get; set; }

        public double? jmlhasil { get; set; }

        public double? nilai { get; set; }
    }
}
