using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ptpn8.Areas.Referensi.Models
{
    //Bagian di Kantor Pusat
    [Table("Urusan")]
    public class Urusan
    {
        public Guid UrusanId { get; set; }
        public Guid BagianId { get; set; }

        [NotMapped]
        public Bagian Bagian { get; set; }

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

        public DateTime TanggalKadaluarsa { get; set; } = new DateTime(1996, 3, 11);

    }
}