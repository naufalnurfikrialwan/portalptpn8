using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("Kemasan")]
    public class Kemasan
    {
        [Required(ErrorMessage ="Kemasan Id Harus Diisi")]
        public Guid KemasanId { get; set; }

        [Required(ErrorMessage = "Nama Kemasan Harus Diisi")]
        public String Nama { get; set; }

    }
}