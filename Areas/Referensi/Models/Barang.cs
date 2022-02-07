using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("Barang")]
    public class Barang
    {
        public Guid BarangId { get; set; }
        public string Norek { get; set; }
        public string KodeBarang { get; set; }
        public string NamaBarang { get; set; }
        public string KodeSatuan { get; set; }
        public string NamaSatuan { get; set; }
        public string InitSatuan { get; set; }
        public Guid RekeningId { get; set; }
        public Guid SatuanId { get; set; }
    }
}