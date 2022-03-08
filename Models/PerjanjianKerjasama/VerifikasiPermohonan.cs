using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    [Table("PK_VerifikasiPermohonan")]
    public class VerifikasiPermohonan
    {
        public Guid VerifikasiPermohonanId { get; set; }
        public Guid InputKerjasamaKebun_HDRId { get; set; }
        public Guid KebunId { get; set; }
        public bool Verifikasi { get; set; }
        public DateTime TanggalVerifikasi { get; set; }
        public String UserName { get; set; }
        public String NamaKebun { get; set; }
        public string NomorPermohonan { get; set; } = "";
        public DateTime TanggalPermohonan { get; set; }
        public string NamaMitra { get; set; } = "";
        public string AlamatMitra { get; set; } = "";
        public string EmailMitra { get; set; } = "";
        public string NomorTelepon { get; set; } = "";


    }
}