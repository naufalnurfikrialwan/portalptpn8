using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    [Table("PerjanjianKerjasama_Dokumen")]
    public class PerjanjianKerjasama_Dokumen
    {
        public Guid PerjanjianKerjasama_DokumenId { get; set; }
        public Guid BagianId { get; set; }
        public string NomorPerjanjian { get; set; }
        public DateTime TanggalPenandatangananPerjanjian { get; set; }
        public string BentukKerjasama { get; set; }
        public string Perihal { get; set; }
        public string JenisPerjanjian { get; set; }
        public string NamaMitra { get; set; }
        public string LokasiPerjanjian { get; set; }
        public string JangkaWaktuKerjasama { get; set; }
        public DateTime TanggalMulai { get; set; }
        public DateTime TanggalBerakhir { get; set; }
        public decimal NilaiNominal { get; set; }
        public string BentukKompensasi { get; set; }
        public string PIC { get; set; }
        public string Keterangan { get; set; }
        public string StatusOtoritas { get; set; }
        public string File { get; set; }
        public string FileSHP { get; set; }
        public string PasswordFile { get; set; }
        public bool Verifikasi { get; set; }
    }


}

