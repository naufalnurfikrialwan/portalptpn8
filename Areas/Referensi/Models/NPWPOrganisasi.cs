using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("NPWPOrganisasi")]
    public class NPWPOrganisasi
    {
        public NPWPOrganisasi()
        {

        }

        public Guid NPWPOrganisasiId { get; set; }
        public Guid OrganisasiId { get; set; }
        [MaxLength(100)]
        public String NPWP { get; set; }
    }
}