using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ptpn8.Areas.Referensi.Models
{
    // Mandor Besar atau setingkat Mandor Besar
    [Table("MandorBesar")]
    public class MandorBesar
    {
        public Guid MandorBesarId { get; set; }
        public Guid AfdelingId { get; set; }

        [NotMapped]
        public Afdeling Afdeling { get; set; }

        [Required]
        [MaxLength(100)]
        public String Nomenklatur { get; set; }
        [Required]
        [MaxLength(500)]
        public String Nama { get; set; }

        public List<Mandor> DaftarMandor { get; set; }

    }
}