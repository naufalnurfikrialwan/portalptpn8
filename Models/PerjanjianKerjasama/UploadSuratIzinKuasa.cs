using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    [Table("PK_UploadSuratIzinKuasa")]
    public class UploadSuratIzinKuasa
    {
        public Guid UploadSuratIzinKuasaId { get; set; }
        public Guid InputKerjasamaKebunId { get; set; }
        public int TahunBuku { get; set; }
        public int NomorInput { get; set; }
        public String NomorPermohonan { get; set; }
        public String File_SuratKuasa { get; set; }
        public String File_SuratIzin { get; set; }
        public String Keterangan { get; set; }
    }
}