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
        public Guid InputKerjasamaKebunId { get; set; } = Guid.Empty;
        public int TahunBuku { get; set; } = 0;
        public int NomorInput { get; set; } = 0;
        public String NomorPermohonan { get; set; } = "";
        public String NomorSuratPerjanjianKerjasama { get; set; } = "";
        public String File_SuratPerjanjianKerjasama { get; set; } = "";
        public String Keterangan { get; set; } = "";
    }
}