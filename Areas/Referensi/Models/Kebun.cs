using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ptpn8.Areas.Referensi.Models
{
    //Kebun dan Unit
    [Table("Kebun")]
    public class Kebun
    {
        public Kebun()
        {
            KebunId = Guid.NewGuid();
            WilayahId = Guid.Empty;
            Nomenklatur = "";
            Nama = "";
            Alamat = "";
            PropinsiId = Guid.Empty;
            KotaId = Guid.Empty;
            KecamatanId = Guid.Empty;
            KelurahanId = Guid.Empty;
            RW = "";
            RT = "";
            kd_kbn = "";
            NPWP = "";
            Telepon = "";
            Faksimili = "";
            Email = "";
            KodeRekening = "";
            Warna = "";
            Dataran = "";
        }
        public Guid KebunId { get; set; } = Guid.NewGuid();
        public Guid WilayahId { get; set; } = Guid.Empty;

        [NotMapped]
        public Wilayah Wilayah { get; set; }

        [Required]
        [MaxLength(100)]
        public String Nomenklatur { get; set; } = "";

        [Required]
        [MaxLength(500)]
        public String Nama { get; set; } = "";

        [Required]
        [MaxLength(500)]
        public String Alamat { get; set; } = "";

        [Required]
        public Guid PropinsiId { get; set; } = Guid.Empty;
        [NotMapped]
        [UIHint("ddlPropinsi")]
        public Propinsi Propinsi { get; set; }

        [Required]
        public Guid KotaId { get; set; } = Guid.Empty;

        [NotMapped]
        [UIHint("ddlKota")]
        public Kota Kota { get; set; }

        [Required]
        public Guid KecamatanId { get; set; } = Guid.Empty;

        [NotMapped]
        [UIHint("ddlKecamatan")]
        public Kecamatan Kecamatan { get; set; }
        [Required]
        public Guid KelurahanId { get; set; } = Guid.Empty;
        [NotMapped]
        [UIHint("ddlKelurahan")]
        public Kelurahan Kelurahan { get; set; }

        [Required]
        public String RW { get; set; } = "";

        [Required]
        public String RT { get; set; } = "";

        [Required]
        public String kd_kbn { get; set; } = "";
        [MaxLength(100)]
        public String NPWP { get; set; } = "";
        [MaxLength(100)]
        public String Telepon { get; set; } = "";
        [MaxLength(100)]
        public String Faksimili { get; set; } = "";
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; } = "";
        public byte[] FotoLokasi { get; set; } = { };

        public List<KebunPeta> DaftarPeta { get; set; }
        [NotMapped]
        public List<Afdeling> DaftarAfdeling { get; set; }
        [NotMapped]
        public List<KebunBudidaya> KebunBudidaya { get; set; }

        public string KodeRekening { get; set; } = "";
        public string Warna { get; set; } = "";

        public string Dataran { get; set; } = "";
        public DateTime TanggalKadaluarsa { get; set; } = new DateTime(1996, 3, 11);

    }

    [Table("KepTan")]
    public class KepTan
    {

        public Guid KepTanId { get; set; }
        public Guid KebunId { get; set; }
        [Required]
        [MaxLength(100)]
        public String Nomenklatur { get; set; } = "";

        [Required]
        [MaxLength(500)]
        public String Nama { get; set; } = "";
        public DateTime TanggalKadaluarsa { get; set; } = new DateTime(1996, 3, 11);
    }
}