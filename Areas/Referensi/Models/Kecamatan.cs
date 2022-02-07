using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("Kecamatan")]
    public class Kecamatan
    {
        public Guid KecamatanId { get; set; }
        public Guid KotaId { get; set; }
        public string Nama { get; set; }
        public String Nomenklatur { get; set; }
        public String KodePos { get; set; }
        public DbGeometry peta { get; set; }
        public List<Kelurahan> kelurahan { get; set; }
    }
}