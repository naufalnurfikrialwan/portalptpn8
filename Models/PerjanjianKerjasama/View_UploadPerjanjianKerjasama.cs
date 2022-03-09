using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    
    public class View_UploadPerjanjianKerjasama
    {
        public Guid UploadPerjanjianKerjasamaId { get; set; } = Guid.NewGuid();
        public Guid InputKerjasamaKebun_HDRId { get; set; } = Guid.Empty;
        public string NomorPerjanjianKerjasama { get; set; } = "";
        public string File_PerjanjianKerjasama { get; set; } = "";
        public string StatusDokumen { get; set; } = "";
        public Guid KebunId { get; set; } = Guid.Empty;
        public string NamaKebun { get; set; } = "";
        public DateTime TanggalUploadPerjanjianKerjasama { get; set; } = DateTime.Now;
        public string NomorPermohonan { get; set; } = "";
        public DateTime TanggalPermohonan { get; set; } = DateTime.Now;
        public string NamaMitra { get; set; } = "";
        public string AlamatMitra { get; set; } = "";
        public string EmailMitra { get; set; } = "";
        public string NomorTelepon { get; set; } = "";
        public string UserName { get; set; } = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(HttpContext.Current.User.Identity.Name).UserName;
    }
}