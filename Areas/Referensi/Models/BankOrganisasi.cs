using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("BankOrganisasi")]
    public class BankOrganisasi
    {
        public BankOrganisasi()
        {

        }

        public Guid BankOrganisasiId { get; set; }
        public Guid OrganisasiId { get; set; }
        [MaxLength(100)]
        public string NamaPemilik { get; set; }
        public string NamaBank { get; set; }
        public string NomorAkunBank { get; set; }
        public string AlamatBank { get; set; }
        public string KotaBank { get; set; }
        public string PropinsiBank { get; set; }
        public string TeleponBank { get; set; }
        public string Norek { get; set; }
        public string Inisial { get; set; }
        public string MataUang { get; set; }
        public string Keterangan { get; set; }
    }
}