namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SDM02
    {
        [Key]
        [StringLength(14)]
        public string NPP { get; set; }

        [StringLength(4)]
        public string NoUrut { get; set; }

        [StringLength(500)]
        public string Nama { get; set; }

        [StringLength(50)]
        public string Hubungan { get; set; }

        public DateTime? TglLahir { get; set; }

        [StringLength(200)]
        public string Kota { get; set; }

        [StringLength(200)]
        public string Propinsi { get; set; }

        [StringLength(200)]
        public string Negara { get; set; }

        [StringLength(50)]
        public string JenisKelamin { get; set; }

        [StringLength(2)]
        public string GolDarah { get; set; }

        [StringLength(50)]
        public string Agama { get; set; }

        [StringLength(50)]
        public string TKPendidikan { get; set; }

        [StringLength(50)]
        public string StatSipil { get; set; }

        [StringLength(50)]
        public string StatKerja { get; set; }

        public DateTime? TglNikah { get; set; }

        public DateTime? TglCerai { get; set; }

        [StringLength(1)]
        public string StatTanggung { get; set; }

        [StringLength(50)]
        public string UserId { get; set; }

        public DateTime? TglInput { get; set; }
    }
}
