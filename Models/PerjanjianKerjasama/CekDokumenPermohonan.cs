using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    [Table("PK_CekDokumenPermohonan")]
    public class CekDokumenPermohonan
    {
        public Guid CekDokumenPermohonanId { get; set; }
        public Guid InputKerjasamaKebun_HDRId { get; set; }
        public Guid BagianId { get; set; }
        public string NomorKajian { get; set; }
        public string File_Kajian { get; set; }
        public Guid KebunId { get; set; }
        public string NamaKebun { get; set; }
        public DateTime TanggalUploadKajian { get; set; }
        public string NomorPermohonan { get; set; }
        public DateTime TanggalPermohonan { get; set; }
        public string NamaMitra { get; set; } = "";
        public string AlamatMitra { get; set; } = "";
        public string EmailMitra { get; set; } = "";
        public string NomorTelepon { get; set; } = "";
        public string UserName { get; set; }
    }
}