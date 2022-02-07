using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("FaksimiliOrganisasi")]
    public class FaksimiliOrganisasi
    {
        public FaksimiliOrganisasi()
        {

        }

        public Guid FaksimiliOrganisasiId { get; set; }
        public Guid OrganisasiId { get; set; }
        [MaxLength(100)]
        public String Faksimili { get; set; }
    }
}