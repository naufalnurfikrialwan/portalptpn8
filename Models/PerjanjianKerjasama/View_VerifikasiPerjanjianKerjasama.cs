using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    
    public class View_VerifikasiPerjanjianKerjasama
    {
        public Guid VerifikasiPerjanjianKerjasamaId { get; set; } = Guid.NewGuid();
        public Guid InputKerjasamaKebunId { get; set; } = Guid.Empty;
        public Guid UploadPerjanjianKerjasamaId { get; set; } = Guid.Empty;
        public Decimal TahunBuku { get; set; } = 0;
        public String NomorInputView { get; set; } = "";
        public Boolean Verifikasi { get; set; } = false;
        public String NomorInvoice { get; set; } = "";
        public String NomorSuratPerjanjianKerjasama { get; set; } = "";
        public String PerihalInvoice { get; set; } = "";
        public String TanggalInvoice { get; set; } = "";
        public String Budidaya { get; set; } = "";
        public String LuasAreal { get; set; } = "";
        public String KelompokTani { get; set; } = "";
        public Decimal NilaiKompensasi { get; set; } = 0;
        public String NilaiKompensasiTerbilang { get; set; } = "";
        public Decimal PPN { get; set; } = 0;
        public Decimal PBB { get; set; } = 0;
        public String JatuhTempo { get; set; } = "";
        public String Keterangan { get; set; } = "";
        public String File_SuratPerjanjianKerjasama { get; set; } = "";
    }
}