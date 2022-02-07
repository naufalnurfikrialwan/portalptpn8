using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    // Mandor atau yang setingkat
    [Table("Mandor")]
    public class Mandor
    {
        public Guid MandorId { get; set; }
        public Guid MandorBesarId { get; set; }

        [NotMapped]
        public MandorBesar MandorBesar { get; set; }

        [Required]
        [MaxLength(100)]
        public String Nomenklatur { get; set; }
        [Required]
        [MaxLength(500)]
        public String Nama { get; set; }
    }
}