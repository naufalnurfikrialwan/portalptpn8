using System;
using System.Web;

namespace Ptpn8.Models
{
    public class PenggunaPortalYangAktif
    {
        public Guid PenggunaPortalYangAktifId { get; set; } = Guid.NewGuid();
        public string UserName { get; set; } = HttpContext.Current.User.Identity.Name;
        public Guid LokasiKerjaId { get; set; } = Guid.Empty;
        public Guid PosisiLokasiKerjaId { get; set; } = Guid.Empty;
        public string Nama { get; set; } = "";
        public DateTime Tanggal { get; set; } = DateTime.Now;
        public Guid AplikasiId { get; set; } = Guid.Empty;
        public string Aplikasi { get; set; } = "";
        public Guid MenuId { get; set; } = Guid.Empty;
        public string Menu { get; set; } = "";
        public bool StatusOwner { get; set; } = false;
        public bool StatusBaru { get; set; } = false;
        public string LokasiKerja { get; set; } = "";
        public int TahunBuku { get; set; } = DateTime.Now.Year;
    }
}