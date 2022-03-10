using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    [Table("PK_HDRUploadInvoice")]
    public class HDRUploadInvoice
    {
        public Guid HDRUploadInvoiceId { get; set; }
        public Guid InputKerjasamaKebun_HDRId { get; set; }
        public Guid KebunId { get; set; }
        public string NamaKebun { get; set; }
        public string NomorPermohonan { get; set; }
        public DateTime TanggalPermohonan { get; set; }
        public string NamaMitra { get; set; } = "";
        public string AlamatMitra { get; set; } = "";
        public string EmailMitra { get; set; } = "";
        public string NomorTelepon { get; set; } = "";
        public string UserName { get; set; }

    }
    [Table("PK_DTLUploadInvoice")]
    public class DTLUploadInvoice
    {
        public Guid DTLUploadInvoiceId { get; set; }
        public Guid HDRUploadInvoiceId { get; set; }
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
    }
}