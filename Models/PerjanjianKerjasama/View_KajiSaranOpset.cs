using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    
    public class View_KajiSaranOpset
    {
        public Guid KajiSaranOpsetId { get; set; } = Guid.NewGuid();
        public Guid InputKerjasamaKebunId { get; set; } = Guid.Empty;
        public Guid UploadSaranKajianOpersionalId { get; set; } = Guid.Empty;
        public Guid UploadSaranKajianAkuntansiId { get; set; } = Guid.Empty;
        public Decimal TahunBuku { get; set; } = 0;
        public String NomorInputView { get; set; } = "";
        public String KeteranganOperasional { get; set; } = "";
        public String KeteranganAkuntansi { get; set; } = "";
        public String File_SaranKajianOperasional { get; set; } = "";
        public String File_SaranKajianAkuntansi { get; set; } = "";
    }
}