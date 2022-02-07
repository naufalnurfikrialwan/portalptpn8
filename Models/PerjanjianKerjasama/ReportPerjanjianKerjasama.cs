using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    public class ReportPerjanjianKerjasama
    {
        public string NamaBagian { get; set; }
        public string NomorPerjanjian { get; set; }
        public string Perihal { get; set; }
        public string JenisPerjanjian { get; set; }
        public string NamaMitra { get; set; }
        public string LokasiPerjanjian { get; set; }
        public string JangkaWaktuKerjasama { get; set; }
        [DataType(DataType.Date)]
        public DateTime TanggalMulai { get; set; }
        [DataType(DataType.Date)]
        public DateTime TanggalBerakhir { get; set; }
        public decimal NilaiNominal { get; set; }
        public string BentukKompensasi { get; set; }
        public string PIC { get; set; }
        public string Keterangan { get; set; }
        public string File { get; set; }
    }
}