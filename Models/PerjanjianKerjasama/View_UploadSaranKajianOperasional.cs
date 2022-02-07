using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    
    public class View_UploadSaranKajianOperasional
    {
        public Guid UploadSaranKajianOperasionalId { get; set; } = Guid.NewGuid();
        public Guid InputKerjasamaKebun_HDRId { get; set; } = Guid.Empty;
        [DataType(DataType.Date)]
        public DateTime TanggalUpload { get; set; } = DateTime.Now;
        public String File_SaranKajianOperasional { get; set; } = "";
        public String NamaFile_SaranKajianOperasional { get; set; } = "";
        public string NomorPermohonan { get; set; } = "";
        public DateTime TanggalPermohonan { get; set; }
        public string NamaMitra { get; set; } = "";
        public string AlamatMitra { get; set; } = "";
        public string EmailMitra { get; set; } = "";
    }
}