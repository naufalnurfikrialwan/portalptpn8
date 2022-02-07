using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("Satuan")]
    public class Satuan
    {
        [Required(ErrorMessage = "Satuan Id Harus Diisi")]
        public Guid SatuanId { get; set; }

        [Required(ErrorMessage = "Nama Satuan Harus Diisi")]
        public String Nama { get; set; }

        public String kd_sat { get; set; }
    }
}