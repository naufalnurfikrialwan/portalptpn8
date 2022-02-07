using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("Blok")]
    public class Blok
    {
        public Guid BlokId { get; set; }
        public Guid AfdelingId { get; set; }
        [NotMapped]
        public Afdeling Afdeling { get; set; }
        public string kd_blok { get; set; }        
        public string Nama { get; set; }       
        public string TahunTanam { get; set; }
        public Decimal Elevasi { get; set; }
        public Guid JenisTanahId { get; set; }
        public Decimal pH { get; set; }
        public string Warna { get; set; }
        public byte[] Foto { get; set; }
        public List<BlokRealisasi> BlokRealisasi { get; set; }
        public List<BlokRKAP> BlokRKAP { get; set; }
        public List<BlokRJPP> BlokRJPP { get; set; }
        public DateTime TanggalKadaluarsa { get; set; } = new DateTime(1996, 3, 11);
    }
    [Table("BlokRealisasi")]
    public class BlokRealisasi
    {
        public Guid BlokRealisasiId { get; set; }
        public String TahunBuku { get; set; }
        public Guid BlokId { get; set; }
        [NotMapped]
        public Blok Blok { get; set; }
        public Guid MainBudidayaId { get; set; }
        public Decimal LuasAreal { get; set; }
        public Guid StatusArealId { get; set; }
        public string StatusAreal { get; set; }
        public int Populasi { get; set; }
        public Guid KloonId { get; set; }
        public Guid HGUId { get; set; }
        [NotMapped]
        public HGU HGU { get; set; }
        public DbGeometry Peta { get; set; }
        public string kd_blok { get; set; }
    }

    [Table("BlokRKAP")]
    public class BlokRKAP
    {
        public Guid BlokRKAPId { get; set; }
        public String TahunBuku { get; set; }
        public Guid BlokId { get; set; }
        public Guid MainBudidayaId { get; set; }
        public Decimal LuasAreal { get; set; }
        public Guid StatusArealId { get; set; }
        public string StatusAreal { get; set; }
        public int Populasi { get; set; }
        public Guid KloonId { get; set; }
        public Guid HGUId { get; set; }
        public DbGeometry Peta { get; set; }
    }
    [Table("BlokRJPP")]
    public class BlokRJPP
    {
        public Guid BlokRJPPId { get; set; }
        public String TahunBuku { get; set; }
        public Guid BlokId { get; set; }
        public Guid MainBudidayaId { get; set; }
        public Decimal LuasAreal { get; set; }
        public Guid StatusArealId { get; set; }
        public string StatusAreal { get; set; }
        public int Populasi { get; set; }
        public Guid KloonId { get; set; }
        public Guid HGUId { get; set; }
        public DbGeometry Peta { get; set; }
    }
}