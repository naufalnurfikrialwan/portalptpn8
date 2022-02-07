using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    [Table("PK_InputPicKerjasama")]
    public class InputPicKerjasama
    {
        public Guid InputPicKerjasamaId { get; set; }
        public Guid InputKerjasamaKebunId { get; set; }
        public Guid nik { get; set; }
        public Decimal TahunBuku { get; set; }
    }
}