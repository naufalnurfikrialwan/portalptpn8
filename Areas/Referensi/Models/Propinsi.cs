using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("Propinsi")]
    public class Propinsi
    {
        public Guid PropinsiId { get; set; }
        public Guid NegaraId { get; set; }
        public string Nama { get; set; }
        public String Nomenklatur { get; set; }
        public String Pulau { get; set; }
        public String IbuKota { get; set; }
        public Decimal Populasi { get; set; }
        public Decimal Luas { get; set; }
        public DbGeometry Peta { get; set; }
        public List<Kota> kota { get; set; }
    }
}