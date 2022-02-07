using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("Alokasi")]
    public class Alokasi
    {
        public Guid AlokasiId { get; set; }
        public string NamaAlokasi { get; set; }
    }
}