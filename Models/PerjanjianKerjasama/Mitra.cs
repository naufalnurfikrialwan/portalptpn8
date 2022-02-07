using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    [Table("PK_Mitra")]
    public class Mitra
    {
        public Guid MitraId { get; set; }
        public Guid KebunId { get; set; }
        public Decimal TahunBuku { get; set; }
        public String NamaMitra { get; set; }
        public String AlamatMitra { get; set; }
        public String NomorTelepon { get; set; }
        public String EmailMitra { get; set; }

    }
}