using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ptpn8.Areas.Referensi.Models
{
    //Dewan Komisaris
    [Table("Komisaris")]
    public class Komisaris
    {
        public Guid KomisarisId { get; set; }
        public Guid OrganisasiId { get; set; }
        //public Organisasi organisasi { get; set; }

        [Required]
        public String Nomenklatur { get; set; }

        [Required]
        public String Nama { get; set; }
        public DateTime TanggalKadaluarsa { get; set; } = new DateTime(1996, 3, 11);
    }
}