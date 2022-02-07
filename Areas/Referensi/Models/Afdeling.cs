using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ptpn8.Areas.Referensi.Models
{
    // Kepala Tanaman, Afdeling, Pengolahan/Pabrik, Administrasi, Teknik atau yang setingkat
    [Table("Afdeling")]
    public class Afdeling
    {
        public Guid AfdelingId { get; set; }
        public Guid KebunId { get; set; }

        [NotMapped]
        public Kebun Kebun { get; set; }

        [Required]
        [MaxLength(100)]
        public String Nomenklatur { get; set; }
        [Required]
        [MaxLength(500)]
        public String Nama { get; set; }

        [Required]
        public String kd_afd { get; set; }

        [Required]
        [MaxLength(100)]
        public String Telepon { get; set; }

        [MaxLength(100)]
        public String Faksimili { get; set; }

        public List<AfdelingPeta> DaftarPeta { get; set; }

        public byte[] FotoLokasi { get; set; }

        public List<MandorBesar> AnggotaMandorBesar { get; set; }
        public string Warna { get; set; }

        public DateTime TanggalKadaluarsa { get; set; } = new DateTime(1996, 3, 11);
    }
}