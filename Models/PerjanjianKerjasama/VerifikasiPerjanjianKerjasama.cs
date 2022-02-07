using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    [Table("PK_VerifikasiPerjanjianKerjasama")]
    public class VerifikasiPerjanjianKerjasama
    {
        public Guid VerifikasiPerjanjianKerjasamaId { get; set; }
        public Guid InputKerjasamaKebunId { get; set; }
        public Decimal TahunBuku { get; set; }
        public Boolean Verifikasi { get; set; }
        public String NomorInvoice { get; set; }
        public String PerihalInvoice { get; set; }
        public String TanggalInvoice { get; set; }
        public String Budidaya { get; set; }
        public String LuasAreal { get; set; }
        public String KelompokTani { get; set; }
        public Decimal NilaiKompensasi { get; set; }
        public String NilaiKompensasiTerbilang { get; set; }
        public Decimal PPN { get; set; }
        public Decimal PBB { get; set; }
        public String JatuhTempo { get; set; }
        public String Keterangan { get; set; }
    }
}