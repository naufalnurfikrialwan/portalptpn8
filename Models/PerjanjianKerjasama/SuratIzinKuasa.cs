using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    [Table("PK_HDRSuratIzinKuasa")]
    public class HDRSuratIzinKuasa
    {
        public Guid HDRSuratIzinKuasaId { get; set; }
        public Guid InputKerjasamaKebun_HDRId { get; set; }
        public Guid KebunId { get; set; }
        public Guid WilayahId { get; set; }
        public string NamaKebun { get; set; }
        public string NomorPermohonan { get; set; }
        public DateTime TanggalPermohonan { get; set; }
        public string NamaMitra { get; set; } = "";
        public string AlamatMitra { get; set; } = "";
        public string EmailMitra { get; set; } = "";
        public string NomorTelepon { get; set; } = "";
        public string UserName { get; set; }
    }
    [Table("PK_DTLSuratIzinKuasa")]
    public class DTLSuratIzinKuasa
    {
        public Guid DTLSuratIzinKuasaId { get; set; }
        public Guid HDRSuratIzinKuasaId { get; set; }
        public string NamaLampiran { get; set; }
        public string NomorLampiran { get; set; }
        public string NamaFile { get; set; }
        public DateTime TanggalUpload { get; set; }
    }
}