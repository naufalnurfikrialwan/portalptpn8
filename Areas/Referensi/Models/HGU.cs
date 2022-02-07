using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("HGU")]
    public class HGU
    {
        public Guid HGUId { get; set; }
        public DateTime TanggalTerbit { get; set; }
        public DateTime TanggalBerakhir { get; set; }
        public string No_SK { get; set; }
        public string No_Sertifikat { get; set; }
        public string No_PBT { get; set; }
        public DateTime Tanggal_PBT { get; set; }
        [NotMapped]
        public int SisaWaktu { get; set; }
    }
}