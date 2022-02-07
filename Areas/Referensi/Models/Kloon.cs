using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("Kloon")]
    public class Kloon
    {
        public Guid KloonId { get; set; }
        public string Nama { get; set; }
        public decimal Protas { get; set; }
        public string Deskripsi { get; set; }
        public string FileFoto { get; set; }
        public byte[] Foto { get; set; }

    }
}