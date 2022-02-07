using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("BPD")]
    public class BPD
    {
        public Guid BPDId { get; set; }
        public string StatusWilayah { get; set; }
        public string UraianBiaya { get; set; } 
        public decimal Gol_I { get; set; }
        public decimal Gol_II { get; set; }
        public decimal Gol_III { get; set; }
        public decimal Gol_IV { get; set; }
        public decimal Kaur { get; set; }
        public decimal PJP { get; set; }
        public decimal Direksi { get; set; }
        public decimal Dirut { get; set; }
        
    }
}