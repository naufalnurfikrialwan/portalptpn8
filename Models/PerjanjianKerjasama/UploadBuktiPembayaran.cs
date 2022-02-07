using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    [Table("PK_UploadBuktiPembayaran")]
    public class UploadBuktiPembayaran
    {
        public Guid UploadBuktiPembayaranId { get; set; }
        public Guid InputKerjasamaKebunId { get; set; }
        public Decimal TahunBuku { get; set; }
        public String File_BuktiPembayaran { get; set; }
        public String Keterangan { get; set; }
    }
}