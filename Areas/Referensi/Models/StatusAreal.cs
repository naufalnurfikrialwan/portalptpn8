using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("StatusAreal")]
    public class StatusAreal
    {
        public Guid StatusArealId { get; set; }
        public string Nama { get; set; }
        public string Deskripsi { get; set; }
        public string Warna { get; set; }

    }
}