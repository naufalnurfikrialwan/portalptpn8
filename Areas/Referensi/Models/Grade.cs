using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("Grade")]
    public class Grade
    {
        public Grade()
        {
        }

        [Required(ErrorMessage = "Grade Id Harus Diisi")]
        public Guid GradeId { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "Id Sub Budidaya Harus Diisi")]
        public Guid SubBudidayaId { get; set; } = Guid.Empty;
        //[NotMapped]
        //public Ptpn8.Areas.Referensi.Models.SubBudidaya SubBudidaya { get; set; }
        [Required(ErrorMessage = "Nama Grade Harus Diisi")]
        public string Nama { get; set; } = "";
        public string Uraian { get; set; } = "";
        public string kd_grade { get; set; } = "";
        public string KelasGrade { get; set; } = "";
        public string Group { get; set; } = "";
        public int Standar_BeratPerSatuan { get; set; } = 0;
        [Required(ErrorMessage = "Satuan Harus Diisi")]
        public Guid SatuanId { get; set; } = Guid.Empty;
        [Required(ErrorMessage = "Kemasan Harus Diisi")]
        public Guid KemasanId { get; set; } = Guid.Empty;
        public string Merk { get; set; } = "";
        [Required(ErrorMessage = "Jumlah Satuan Harus Diisi")]
        public int JumlahSatuan { get; set; } = 0;
        public string Ukuran { get; set; } = "";
        //public List<Varian> Varian { get; set; }
        [NotMapped]
        public string FileFoto { get; set; } = "";
        [UIHint("imgUpload")]
        public byte[] Foto { get; set; } = { };
        //[UIHint("ddlKemasan")]
        //[NotMapped]
        //public Ptpn8.Areas.Referensi.Models.Kemasan Kemasan { get; set; }
        //[UIHint("ddlSatuan")]
        //[NotMapped]
        //public Ptpn8.Areas.Referensi.Models.Satuan Satuan { get; set; }

        [UIHint("aucNamaSatuan")]
        [NotMapped]
        public string NamaSatuan { get; set; }

        [NotMapped]
        public string Text { get; set; } = "";
        public string kd_jnsprod { get; set; } = "";
        public int Tara { get; set; } = 0;
        public string KodeKPBN { get; set; } = "";
    }
}