using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Ptpn8.Areas.Referensi.Models
{
    //Wilayah dan GM
    [Table("Wilayah")]
    public class Wilayah
    {
        public Guid WilayahId { get; set; }
        public Guid DireksiId { get; set; }

        [NotMapped]
        public Direksi Direksi { get; set; }

        [Required]
        [MaxLength(100)]
        public String Nomenklatur { get; set; }

        [Required]
        [MaxLength(500)]
        public String Nama { get; set; }

        [Required]
        [MaxLength(500)]
        public String Alamat { get; set; }

        [Required]
        public Guid PropinsiId { get; set; }
        [NotMapped]
        [UIHint("ddlPropinsi")]
        public Propinsi Propinsi { get; set; }

        [Required]

        public Guid KotaId { get; set; }
        [NotMapped]
        [UIHint("ddlKota")]
        public Kota Kota { get; set; }

        [Required]
        public Guid KecamatanId { get; set; }
        [NotMapped]
        [UIHint("ddlKecamatan")]
        public Kecamatan Kecamatan { get; set; }

        [Required]
        public Guid KelurahanId { get; set; }
        [NotMapped]
        [UIHint("ddlKelurahan")]
        public Kelurahan Kelurahan { get; set; }

        [Required]
        [MaxLength(100)]
        public String RW { get; set; }

        [Required]
        [MaxLength(100)]
        public String RT { get; set; }

        //public Referensi.Models.SDM.Propinsi propinsi { get; set; }

        [Required]
        public String kd_kbn { get; set; }

        [MaxLength(100)]
        public String NPWP { get; set; }

        [MaxLength(100)]
        public String Telepon { get; set; }

        [MaxLength(100)]
        public String Faksimili { get; set; }

        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        public DbGeometry Peta { get; set; }

        public List<Kebun> DaftarKebun { get; set; }
        public List<Bidang> DaftarBidang { get; set; }
        public DateTime TanggalKadaluarsa { get; set; } = new DateTime(1996, 3, 11);
    }
}