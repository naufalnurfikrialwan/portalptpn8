using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("SubBudidaya")]
    public class SubBudidaya
    {
        [Required(ErrorMessage = "Id Sub Budidaya Harus Diisi")]
        public Guid SubBudidayaId { get; set; }

        [Required(ErrorMessage = "Id Budidaya Harus Diisi")]
        public Guid MainBudidayaId { get; set; }

        [NotMapped]
        public MainBudidaya Budidaya { get; set; }

        [Required(ErrorMessage = "Nama Sub Budidaya Harus Diisi")]
        public String Nama { get; set; }
        [Required(ErrorMessage = "kd_jnsprod Harus Diisi")]
        public String kd_jnsprod { get; set; }

        //public List<Grade> Grade { get; set; }

        public String Merk { get; set; }

        [NotMapped]
        public String FileFoto { get; set; }
        [UIHint("imgUpload")]
        public byte[] Foto { get; set; }
    }
}