using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Areal.Models
{
    public class PetaView
    {
        public string Peta {get; set;}
        public Guid Id { get; set; }
        public string Nama { get; set; }
        public string Warna { get; set; }
        public Guid IdInduk { get; set; }
        public string KodeKebun { get; set; }
        public string KodeBudidaya { get; set; }
        public string TahunTanam { get; set; }
        public double? Value1 { get; set; }
        public double? Value2 { get; set; }
        public double? Value3 { get; set; }

    }
}