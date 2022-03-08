﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    
    public class View_CekDokumenPermohonan
    {
        public Guid CekDokumenPermohonanId { get; set; } = Guid.NewGuid();
        public Guid InputKerjasamaKebun_HDRId { get; set; } = Guid.Empty;
        public Guid BagianId { get; set; } = Guid.Empty;
        public string NomorKajian { get; set; } = "";
        public string File_Kajian { get; set; } = "";
        public Guid KebunId { get; set; } = Guid.Empty;
        public string NamaKebun { get; set; } = "";
        [DataType(DataType.Date)]
        public DateTime TanggalUploadKajian { get; set; } = DateTime.Now;
        public string NomorPermohonan { get; set; } = "";
        [DataType(DataType.Date)]
        public DateTime TanggalPermohonan { get; set; } = DateTime.Now;
        public string NamaMitra { get; set; } = "";
        public string AlamatMitra { get; set; } = "";
        public string EmailMitra { get; set; } = "";
        public string NomorTelepon { get; set; } = "";
        public string UserName { get; set; } = "";
        public string StatusDokumen { get; set; } = "";
    }
}