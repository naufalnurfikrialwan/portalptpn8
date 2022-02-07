using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("Negara")]
    public class Negara
    {
        [Required(ErrorMessage = "Negara Id Harus Diisi")]
        public Guid NegaraId { get; set; }

        [Required(ErrorMessage = "Nama Negara Harus Diisi")]
        public String Nama { get; set; }

        [Required(ErrorMessage = "Inisial Negara Harus Diisi")]
        public String Inisial { get; set; }

        [NotMapped]
        public String FileFoto { get; set; }

        public byte[] Foto { get; set; }

        public List<Propinsi> DaftarPropinsi { get; set; }

    }
}