using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("TipeKamar")]
    public class TipeKamar
    {
        public Guid TipeKamarId { get; set; }
        public Guid KebunId { get; set; }
        public string JenisKamar { get; set; }
        public string Fasilitas { get; set; }
        [UIHint("ddlJenisPeruntukan")]
        public string JenisPeruntukan { get; set; }
        public decimal Harga { get; set; }
        public int JumlahKamar { get; set; }
        public int JmlOrangPerKamar { get; set; }
        public decimal HargaWeekend { get; set; }
        [NotMapped]
        public string FileFoto { get; set; } = "";
        [UIHint("imgUpload")]
        public byte[] Foto { get; set; } = { };
    }
}