﻿using System;
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
        public Guid InputKerjasamaKebunId { get; set; } = Guid.Empty;
        public Guid nik { get; set; }= Guid.Empty;
        public Decimal TahunBuku { get; set; } = 0;
        public String NomorInputView { get; set; } = "";
        public String Nama { get; set; } = "";
    }
}