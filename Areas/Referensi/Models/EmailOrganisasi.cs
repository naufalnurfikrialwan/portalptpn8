using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("EmailOrganisasi")]
    public class EmailOrganisasi
    {
        public EmailOrganisasi()
        {
        }

        public Guid EmailOrganisasiId { get; set; }
        public Guid OrganisasiId { get; set; }

        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public String AlamatEmail { get; set; }
    }
}