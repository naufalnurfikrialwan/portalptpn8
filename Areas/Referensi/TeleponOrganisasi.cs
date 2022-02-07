using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ptpn8.Areas.Referensi
{
    [Table("TeleponOrganisasi")]
    public class TeleponOrganisasi
    {
        public TeleponOrganisasi()
        {

        }

        public Guid TeleponOrganisasiId { get; set; }
        public Guid OrganisasiId { get; set; }
        [MaxLength(100)]
        [Required(ErrorMessage = "No Telepon Harus Diisi")]
        public String Telepon { get; set; }
    }

}