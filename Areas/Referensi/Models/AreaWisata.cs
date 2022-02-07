using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("AreaWisata")]
    public class AreaWisata
    {
        public Guid AreaWisataId { get; set; }
        public Guid KebunId { get; set; }
        public string Nama { get; set; }
        public string Alamat { get; set; }
        public string Telepon { get; set; }
        public string Fasilitas { get; set; }
        public decimal HargaWeekend { get; set; }
        public decimal Harga { get; set; }
        [NotMapped]
        public string FileFoto { get; set; } = "";
        [UIHint("imgUpload")]
        public byte[] Foto { get; set; } = { };
    }
}