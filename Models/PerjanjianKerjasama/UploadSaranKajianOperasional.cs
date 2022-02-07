using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    [Table("PK_UploadSaranKajianOperasional")]
    public class UploadSaranKajianOperasional
    {
        public Guid UploadSaranKajianOperasionalId { get; set; }
        public Guid InputKerjasamaKebun_HDRId { get; set; }
        public DateTime TanggalUpload { get; set; }
        public String File_SaranKajianOperasional { get; set; }
        public String NamaFile_SaranKajianOperasional { get; set; }
    }
}