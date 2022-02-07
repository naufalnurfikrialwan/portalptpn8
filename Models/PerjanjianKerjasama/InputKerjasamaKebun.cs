using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    [Table("PK_InputKerjasamaKebun")]
    public class InputKerjasamaKebun_HDR
    {
        public Guid InputKerjasamaKebun_HDRId { get; set; }
        public Guid KebunId { get; set; }
        public Guid MitraId { get; set; }
        public int NomorInput { get; set; }
        public int TahunBuku { get; set; }
        public String NomorPermohonan { get; set; }
        public DateTime TanggalPermohonan { get; set; }
        public String NamaMitra { get; set; }
        public String AlamatMitra { get; set; }
        public String EmailMitra { get; set; }
        public String NomorTelepon { get; set; }
        public string UserName { get; set; }
        public List<InputKerjasamaKebun_DTL> InputKerjasamaKebun_DTL { get; set; }

    }

    [Table("PK_DTLInputKerjasamaKebun")]
    public class InputKerjasamaKebun_DTL
    {
        public Guid InputKerjasamaKebun_DTLId { get; set; }
        public Guid InputKerjasamaKebun_HDRId { get; set; }       
        public string Lampiran { get; set; }
        public string Nama_File { get; set; }      
    }
}