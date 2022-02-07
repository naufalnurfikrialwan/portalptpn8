using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("Buyer")]
    public class Buyer
    {
        [Required]
        public Guid BuyerId { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "Nama Buyer Harus Diisi")]
        [MaxLength(500)]
        public string Nama { get; set; } = "";
        public string Inisial { get; set; } = "";
        [MaxLength(500)]
        public string Direktur { get; set; } = "";
        [NotMapped]
        public string FileFoto { get; set; } = "";
        [UIHint("imgUpload")]
        public byte[] Foto { get; set; } = { };
        [MaxLength(500)]
        public string Alamat { get; set; } = "";
        [MaxLength(500)]
        public string Alamat1 { get; set; } = "";
        [MaxLength(500)]
        public string Alamat2 { get; set; } = "";
        [MaxLength(500)]
        public string Kota { get; set; } = "";
        [MaxLength(500)]
        public string Propinsi { get; set; } = "";
        [MaxLength(500)]
        public string Negara { get; set; } = "";
        [MaxLength(100)]
        public string NPWP { get; set; } = "";
        public string Telepon { get; set; } = "";
        public string Faksimili { get; set; } = "";
        public string Email { get; set; } = "";
        public string Portal { get; set; } = "";
        public string kd_pemb { get; set; } = "";
        [NotMapped]
        public string DaftarBudidaya { get; set; } = "";
    }

    [Table("BuyerBudidaya")]
    public class BuyerBudidaya
    {
        [Required]
        public Guid BuyerBudidayaId { get; set; }
        [Required]
        public Guid MainBudidayaId { get; set; }
        [Required(ErrorMessage = "Buyer Id Harus Diisi")]
        public Guid BuyerId { get; set; }
        [NotMapped]
        [UIHint("ddlBuyer")]
        public Buyer Buyer { get; set; }
        [Required(ErrorMessage = "Rekening Harus Diisi")]
        public Guid RekeningId { get; set; }
        [NotMapped]
        [UIHint("ddlRekening")]
        [CRUD.KodeRekeningAda]
        public Rekening Rekening { get; set; }
        [NotMapped]
        [UIHint("ddlBudidaya")]
        public MainBudidaya Budidaya { get; set; }
    }
}
