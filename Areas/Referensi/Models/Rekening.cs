using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("Rekening")]
    public class Rekening
    {   
        [Required(ErrorMessage = "Rekening Id Harus Diisi")]
        public Guid RekeningId { get; set; }
        public string KodeUnit { get; set; }
//        [Required(ErrorMessage = "Kode Rekening Harus Diisi")]
//        public String KodeRekening { get; set; }
        public string Norek { get; set; }

        //[Required(ErrorMessage = "Nama Rekening Harus Diisi")]
        //public string Nama { get; set; }

        public string NamaRekening { get; set; }
        public bool Netral { get; set; }
        public bool vblok { get; set; }
        [UIHint("ddlSaldoNormal")]
//        [Required(ErrorMessage = "Saldo Normal Harus Diisi")]
        public string SaldoNormal { get; set; }

        [UIHint("ddlKelompokRekening")]
//        [Required(ErrorMessage = "Kelompok Rekening Harus Diisi")]
        public string KelompokRekening { get; set; }
        [NotMapped]
        public string Text { get; set; }
    }
}