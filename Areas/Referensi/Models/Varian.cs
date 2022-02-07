using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("Varian")]
    public class Varian
    {
        public Varian()
        { }

        [Required(ErrorMessage = "Varian Id Harus Diisi")]
        public Guid VarianId { get; set; }

        [Required(ErrorMessage = "Grade Id Harus Diisi")]
        public Guid GradeId { get; set; }

        [NotMapped]
        public Grade Grade { get; set; }

        [Required(ErrorMessage = "Nama Varian Harus Diisi")]
        public String Nama { get; set; }
        public Guid SatuanId { get; set; }
        public Guid KemasanId { get; set; }

        public String Merk { get; set; }

        [Required(ErrorMessage = "Jumlah Satuan Harus Diisi")]
        public int JumlahSatuan { get; set; }

        public String Ukuran { get; set; }

        [UIHint("imgUpload")]

        public byte[] Foto { get; set; }
    }
}