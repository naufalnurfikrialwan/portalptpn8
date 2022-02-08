using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    [Table("PK_KajiSaranOpset")]
    public class KajiSaranOpset
    {
        public Guid KajiSaranOpsetId { get; set; }
        public Guid InputKerjasamaKebunId { get; set; }
        public Guid UploadSaranKajianOperasionalId { get; set; }
        public Guid UploadSaranKajianAkuntansiId { get; set; }
        public Decimal TahunBuku { get; set; }
        public Decimal TestGit { get; set; }

    }
}