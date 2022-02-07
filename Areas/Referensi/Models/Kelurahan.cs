using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("Kelurahan")]
    public class Kelurahan
    {
        public Guid KelurahanId { get; set; }
        public Guid KecamatanId { get; set; }
        public String Nama { get; set; }
        public String Nomenklatur { get; set; }
        public DbGeometry peta { get; set; }
    }
}