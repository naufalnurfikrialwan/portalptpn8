using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    
    public class View_InputPicKerjasama
    {
        public Guid InputPicKerjasamaId { get; set; } = Guid.NewGuid();
        public Guid InputKerjasamaKebun_HDRId { get; set; } = Guid.Empty;
        public Guid KebunId { get; set; } = Guid.Empty;
        [DataType(DataType.Date)]
        public DateTime TanggalAlokasiPIC { get; set; } = DateTime.Now;
        public string NIK { get; set; }= "";
        [UIHint("ddlPICKerjasama")]
        public string Nama { get; set; } = "";
        public string NamaKebun { get; set; } = "";
        public string NomorPermohonan { get; set; } = "";
        public DateTime TanggalPermohonan { get; set; } 
        public string NamaMitra { get; set; } = "";
        public string AlamatMitra { get; set; } = "";
        public string EmailMitra { get; set; } = "";
        public string NomorTelepon { get; set; } = "";
    }
}