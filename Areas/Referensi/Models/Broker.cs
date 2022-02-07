using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("Broker")]
    public class Broker
    {
        [Required]
        public Guid BrokerId { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "Nama Broker Harus Diisi")]
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
        public string KodePos { get; set; } = "";
        public string NPWP { get; set; } = "";
        public string Telepon { get; set; } = "";
        public string Faksimili { get; set; } = "";
        public string Email { get; set; } = "";
        public string Portal { get; set; } = "";
        public string kd_brok { get; set; } = "";
    }
}
