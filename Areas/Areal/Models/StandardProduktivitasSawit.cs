using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Areal.Models
{
    [Table("StandardProduktivitasSawit")]
    public class StandardProduktivitasSawit
    {
        public StandardProduktivitasSawit()
        {
            StandardProduktivitasSawitId = Guid.NewGuid();
            Umur = 0;
            TonTBS_S1 = 0;
            RBT_S1 = 0;
            RJT_S1 = 0;
            TonTBS_S2 = 0;
            RBT_S2 = 0;
            RJT_S2 = 0;
            TonTBS_S3 = 0;
            RBT_S3 = 0;
            RJT_S3 = 0;
        }
        public Guid StandardProduktivitasSawitId { get; set; }
        public int Umur { get; set; }
        public decimal TonTBS_S1 { get; set; }
        public decimal RBT_S1 { get; set; }
        public decimal RJT_S1 { get; set; }
        public decimal TonTBS_S2 { get; set; }
        public decimal RBT_S2 { get; set; }
        public decimal RJT_S2 { get; set; }
        public decimal TonTBS_S3 { get; set; }
        public decimal RBT_S3 { get; set; }
        public decimal RJT_S3 { get; set; }
        public decimal Rotasi { get; set; }
        public decimal HKperHa { get; set; }
    }
}