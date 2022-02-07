using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    public class Customer_Agrowisata
    {
        public Guid Customer_AgrowisataId { get; set; }
        [Required]
        public string Nama { get; set; }
        [Required]
        [UIHint("ddlJenisIdentitas")]
        public string JenisIdentitas { get; set; }
        [Required]
        public string NoIdentitas { get; set; }
        [Required]
        [UIHint("ddlJenisKelamin")]
        public string JenisKelamin { get; set; }
        [Required]
        [UIHint("ddlKategoriCustomer")]
        public string KategoriCustomer { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Tgl_Lahir { get; set; } = DateTime.Now;
        [Required]
        public string Alamat { get; set; }
        [Required]
        public string Telepon { get; set; }
        [Required]
        public string Email { get; set; }
    }
}