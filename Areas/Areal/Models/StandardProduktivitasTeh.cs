using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Areal.Models
{
    [Table("StandardProduktivitasTeh")]
    public class StandardProduktivitasTeh
    {
        public StandardProduktivitasTeh()
        {
            StandardProduktivitasTehId = Guid.NewGuid();
            Dataran = "";
            RotasiGunting = 0;
            RotasiMesin = 0;
            LuasJelajah = 0;
            KgPerHa = 0;
        }
        public Guid StandardProduktivitasTehId { get; set; }
        public string Dataran { get; set; }
        public decimal RotasiGunting { get; set; }
        public decimal RotasiMesin { get; set; }
        public decimal LuasJelajah { get; set; }
        public decimal KgPerHa { get; set; }
    }
}