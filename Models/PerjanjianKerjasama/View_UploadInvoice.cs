using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    public class View_HDRUploadInvoice
    {
        public Guid HDRUploadInvoiceId { get; set; } = Guid.NewGuid();
        public Guid InputKerjasamaKebun_HDRId { get; set; } = Guid.Empty;
        public Guid KebunId { get; set; } = Guid.Empty;
        public string NamaKebun { get; set; } = "";
        public string NomorPermohonan { get; set; } = "";
        public DateTime TanggalPermohonan { get; set; } = DateTime.Now;
        public string NamaMitra { get; set; } = "";
        public string AlamatMitra { get; set; } = "";
        public string EmailMitra { get; set; } = "";
        public string NomorTelepon { get; set; } = "";
        public string UserName { get; set; } = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(HttpContext.Current.User.Identity.Name).UserName;
    }

    public class View_DTLUploadInvoice
    {
        public Guid DTLUploadInvoiceId { get; set; } = Guid.NewGuid();
        public Guid HDRUploadInvoiceId { get; set; } = Guid.NewGuid();
        public String NomorInvoice { get; set; } = "";
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
    }
}