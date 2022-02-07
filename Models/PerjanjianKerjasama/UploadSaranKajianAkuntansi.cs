using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    [Table("PK_UploadSaranKajianAkuntansi")]
    public class UploadSaranKajianAkuntansi
    {
        public Guid UploadSaranKajianOperasionalId { get; set; }
        public Guid InputKerjasamaKebunId { get; set; }
        public Decimal TahunBuku { get; set; }
        public String File_SaranKajianAkuntansi { get; set; }
        public String Keterangan { get; set; }
    }
}