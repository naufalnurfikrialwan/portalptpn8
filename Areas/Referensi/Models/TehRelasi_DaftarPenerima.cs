using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("TehRelasi_DaftarPenerima")]
    public class TehRelasi_DaftarPenerima
    {
        public Guid TehRelasi_DaftarPenerimaId { get; set; }
        public string Nama { get; set; }
        public string Perusahaan { get; set; }
        public string Alamat { get; set; }
        public string Telepon { get; set; }
        public string Email { get; set; }
        public string Keterangan { get; set; }
    }
}