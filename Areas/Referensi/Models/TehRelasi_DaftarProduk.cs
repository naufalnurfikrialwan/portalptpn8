using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("TehRelasi_DaftarProduk")]
    public class TehRelasi_DaftarProduk
    {
        public Guid TehRelasi_DaftarProdukId { get; set; }
        public string Nama { get; set; }
        public string Spesifikasi { get; set; }
        public decimal Harga { get; set; }
        public string Keterangan { get; set; }
        [NotMapped]
        public string FileFoto { get; set; } = "";
        [UIHint("imgUpload")]
        public byte[] Foto { get; set; } = { };
    }
}