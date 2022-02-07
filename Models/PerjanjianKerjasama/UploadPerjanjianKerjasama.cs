using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    [Table("PK_UploadPerjanjianKerjasama")]
    public class UploadPerjanjianKerjasama
    {
        public Guid UploadPerjanjianKerjasamaId { get; set; }
        public Guid InputKerjasamaKebunId { get; set; }
        public int TahunBuku { get; set; }
        public int NomorInput { get; set; }
        public String NomorPermohonan { get; set; }
        public String File_SuratPerjanjianKerjasama { get; set; }
        public String NomorSuratPerjanjianKerjasama { get; set; }
        public String Keterangan { get; set; }
    }
}