using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Ptpn8.Areas.Referensi.Models.CRUD;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    
    
    public class View_LihatFileKajian
    {
        public Guid CekDokumenPermohonanId { get; set; } = Guid.NewGuid();
        public Guid InputKerjasamaKebun_HDRId { get; set; } = Guid.Empty;
        public Guid BagianId { get; set; } = Guid.Empty;
        public string NamaBagian { get; set; } = "";
        public string NomorKajian { get; set; } = "";
        public string File_Kajian { get; set; } = "";
        [DataType(DataType.Date)]
        public DateTime TanggalUploadKajian { get; set; } 

    }
}