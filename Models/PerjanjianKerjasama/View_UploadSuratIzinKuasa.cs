using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    
    public class View_UploadSuratIzinKuasa
    {
        public Guid UploadSuratIzinKuasaId { get; set; } = Guid.NewGuid();
        public Guid InputKerjasamaKebunId { get; set; } = Guid.Empty;
        public Decimal TahunBuku { get; set; } = 0;
        public String NomorInputView { get; set; } = "";
        public String File_SuratKuasa { get; set; } = "";
        public String File_SuratIzin { get; set; } = "";
        public String Keterangan { get; set; } = "";
    }
}