using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("PortalOrganisasi")]
    public class PortalOrganisasi
    {
        public PortalOrganisasi()
        {

        }

        public Guid PortalOrganisasiId { get; set; }
        public Guid OrganisasiId { get; set; }
        [MaxLength(300)]
        public String NamaPortal { get; set; }
    }
}