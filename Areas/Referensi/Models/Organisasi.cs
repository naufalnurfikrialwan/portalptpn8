using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("Organisasi")]
    public class Organisasi
    {
        public Organisasi()
        {

        }

        public Guid OrganisasiId { get; set; }

        [Required(ErrorMessage = "Nama Organisasi Harus Diisi")]
        [MaxLength(500)]
        public String Nama { get; set; }

        [NotMapped]
        public String FileLogo { get; set; }

        [UIHint("imgUpload")]
        public byte[] Logo { get; set; }

        [Required(ErrorMessage = "Alamat Organisasi Harus Diisi")]
        [MaxLength(500)]
        public String Alamat { get; set; }

        [Required(ErrorMessage = "Propinsi Harus Diisi")]

        public Guid NegaraId { get; set; }
        public Guid PropinsiId { get; set; }

        [Required(ErrorMessage = "Kota Harus Diisi")]

        public Guid KotaId { get; set; }

        [Required(ErrorMessage = "Kecamatan Harus Diisi")]

        public Guid KecamatanId { get; set; }

        [Required(ErrorMessage = "Kelurahan Harus Diisi")]

        public Guid KelurahanId { get; set; }

        [Required(ErrorMessage = "RW Harus Diisi")]
        [MaxLength(10)]
        public String RW { get; set; }

        [Required(ErrorMessage = "RT Organisasi Harus Diisi")]
        [MaxLength(10)]
        public String RT { get; set; }


        [NotMapped]
        [UIHint("ddlNegara")]
        public Negara Negara { get; set; }

        [NotMapped]
        [UIHint("ddlPropinsi")]
        public Propinsi Propinsi { get; set; }

        [NotMapped]
        [UIHint("ddlKota")]
        public Kota Kota { get; set; }

        [NotMapped]
        [UIHint("ddlKecamatan")]
        public Kecamatan Kecamatan { get; set; }

        [NotMapped]
        [UIHint("ddlKelurahan")]
        public Kelurahan Kelurahan { get; set; }
        public List<Komisaris> DaftarKomisaris { get; set; }
        public List<Direksi> DaftarDireksi { get; set; }
        public List<NPWPOrganisasi> DaftarNPWP { get; set; }
        public List<TeleponOrganisasi> DaftarTelepon { get; set; }
        public List<FaksimiliOrganisasi> DaftarFaksimili { get; set; }
        public List<EmailOrganisasi> DaftarEmail { get; set; }
        public List<PortalOrganisasi> DaftarPortal { get; set; }
        public List<BankOrganisasi> DaftarBank { get; set; }
        public DateTime TanggalKadaluarsa { get; set; } = new DateTime(1996, 3, 11);

    }
}