using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ptpn8.Areas.Referensi.Models
{
    //Direksi
    [Table("Direksi")]
    public class Direksi
    {
        public Guid DireksiId { get; set; }
        public Guid OrganisasiId { get; set; }
        //public Organisasi organisasi { get; set; }

        [Required]
        public String Nomenklatur { get; set; }

        [Required]
        public String Nama { get; set; }

        public List<Wilayah> DaftarWilayah { get; set; }
        public List<Bagian> DaftarBagian { get; set; }
        [NotMapped]
        public Organisasi Organisasi { get; set; }
        public DateTime TanggalKadaluarsa { get; set; } = new DateTime(1996, 3, 11);
    }
}