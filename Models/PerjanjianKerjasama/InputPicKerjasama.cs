using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    [Table("PK_InputPICKerjasama")]
    public class InputPicKerjasama
    {
        public Guid InputPicKerjasamaId { get; set; }
        public Guid InputKerjasamaKebun_HDRId { get; set; }
        public DateTime TanggalAlokasiPIC { get; set; }
        public DateTime TanggalPermohonan { get; set; }
        public string NIK { get; set; }
        public string Nama { get; set; }
        public string UserName { get; set; }
    }
}