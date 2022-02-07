using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    //Bagian di Kantor Pusat
    [Table("Bagian")]
    public class Bagian
    {
        public Guid BagianId { get; set; }
        public Guid DireksiId { get; set; }
        [NotMapped]
        public Direksi Direksi { get; set; }

        [Required]
        [MaxLength(100)]
        public String Nomenklatur { get; set; }
        [Required]
        [MaxLength(500)]
        public String Nama { get; set; }

        public String kd_kbn { get; set; }
        [MaxLength(100)]
        public String Telepon { get; set; }
        [MaxLength(100)]
        public String Faksimili { get; set; }
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        public List<Urusan> DaftarUrusan { get; set; }
        public DateTime TanggalKadaluarsa { get; set; } = new DateTime(1996, 3, 11);
    }
}