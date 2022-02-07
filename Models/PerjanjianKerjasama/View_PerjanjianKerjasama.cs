using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    public class ViewPerjanjianKerjasama_Dokumen
    {
        public Guid PerjanjianKerjasama_DokumenId { get; set; } = Guid.NewGuid();
        public Guid BagianId { get; set; } = Guid.Empty;
        public int NoBaris { get; set; } = 0;
        [Required]
        [Display(Name = "Message : Apabila lebih dari 1 Nomor Perjanjian, maka digunakan tanda pemisah ';'")]
        public string NomorPerjanjian { get; set; } = "";
        [DataType(DataType.Date)]
        public DateTime TanggalPenandatangananPerjanjian { get; set; } = DateTime.Now;
        [UIHint("ddlBentukKerjasama")]
        public string BentukKerjasama { get; set; } = "";
        [Required]
        [Display(Name = "Message : Diisi perihal kerjasama yang dijalin")]
        public string Perihal { get; set; } = "";
        [UIHint("ddlJenisPerjanjian")]
        public string JenisPerjanjian { get; set; } = "";
        [Required]
        [Display(Name = "Message : Diisi oleh Nama Mitra Perusahaan/Perorangan/Koperasi")]
        public string NamaMitra { get; set; } = "";
        [Display(Name = "Message : Diisi bilamana objek kerjasama merupakan aset PTPN VIII, kalau tidak ada dikosongkan sesuai dokumen kontrak")]
        public string LokasiPerjanjian { get; set; } = "";
        [UIHint("ddlJangkaWaktuKerjasama")]
        public string JangkaWaktuKerjasama { get; set; } = "";
        [DataType(DataType.Date)]
        public DateTime TanggalMulai { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        public DateTime TanggalBerakhir { get; set; } = DateTime.Now;
        [Display(Name = "Message : Diisi apabila sudah ada nilai rupiahnya, bila tidak ada/belum jelas kosongkan dengan memberikan penjelasan pada kolom keterangan")]
        public decimal NilaiNominal { get; set; } = 0;
        [Display(Name = "Message : Diisi sesuai Perjanjian")]
        public string BentukKompensasi { get; set; } = "";
        [Required]
        [Display(Name = "Message : Diisi oleh Nama Jabatan BOD-1")]
        public string PIC { get; set; } = "";
        public string Keterangan { get; set; } = "";
        public string StatusOtoritas { get; set; } = "";
        public string File { get; set; } = "";
        public string FileSHP { get; set; } = "";
        public string PasswordFile { get; set; } = "";
        public bool Verifikasi { get; set; } = false;
    }
}