using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{

    public class View_HDRUploadSuratIzinKuasa
    {
        public Guid HDRUploadSuratIzinKuasaId { get; set; } = Guid.NewGuid();
        public Guid InputKerjasamaKebun_HDRId { get; set; } = Guid.Empty;
        public Guid KebunId { get; set; } = Guid.Empty;
        public Guid WilayahId { get; set; } = Guid.Empty;
        public string NamaKebun { get; set; } = "";
        public string NomorPermohonan { get; set; } = "";
        public DateTime TanggalPermohonan { get; set; } = DateTime.Now;
        public string NamaMitra { get; set; } = "";
        public string AlamatMitra { get; set; } = "";
        public string EmailMitra { get; set; } = "";
        public string NomorTelepon { get; set; } = "";
        public string UserName { get; set; } = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(HttpContext.Current.User.Identity.Name).UserName;
    }
    public class View_DTLUploadSuratIzinKuasa
    {
        public Guid DTLUploadSuratIzinKuasaId { get; set; } = Guid.NewGuid();
        public Guid HDRUploadSuratIzinKuasaId { get; set; } = Guid.Empty;
        public string NamaLampiran { get; set; } = "";
        public string NomorLampiran { get; set; } = "";
        public string NamaFile { get; set; } = "";
        public string StatusDokumen { get; set; } = "";
        public DateTime TanggalUpload { get; set; } = DateTime.Now;
    }
}