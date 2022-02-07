using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Areal.Models
{
    [Table("StandardProduktivitasKaret")]
    public class StandardProduktivitasKaret
    {
        public Guid StandardProduktivitasKaretId { get; set; }
        public int Umur { get; set; }
        public int UmurSadap { get; set; }
        public decimal TonPerHa { get; set; }
        public decimal Rotasi { get; set; }
        public decimal PohonDisadapPerHanca { get; set; }
        public decimal PctLateks { get; set; }
        public decimal PctLump { get; set; }
        public decimal PctKKK { get; set; }

    }
}