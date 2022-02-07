using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("Vessel")]
    public class Vessel
    {
        public Guid VesselId { get; set; }
        public string Nama { get; set; }
        public string Alamat1 { get; set; }
        public string Alamat2 { get; set; }
        public string Kota { get; set; }
        public string KodePos { get; set; }
        public string Negara { get; set; }
        public string NoTelepon { get; set; }
        public string NoFaksimili { get; set; }
        public string KodeRekening { get; set; }
    }
}