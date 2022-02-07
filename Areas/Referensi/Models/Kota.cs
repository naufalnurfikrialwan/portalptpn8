using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("Kota")]
    public class Kota
    {
        public Guid KotaId { get; set; }
        public string Nama { get; set; }
        public String Nomenklatur { get; set; }
        public String PusatPemerintahan { get; set; }
        public DbGeometry peta { get; set; }
        public Guid PropinsiId { get; set; }
        public List<Kecamatan> kecamatan { get; set; }

    }
}