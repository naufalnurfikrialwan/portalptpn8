using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    [Table("PK_VerifikasiPermohonan")]
    public class VerifikasiPermohonan
    {
        public Guid VerifikasiPermohonanId { get; set; }
        public Guid InputKerjasamaKebunId { get; set; }
        public Guid KebunId { get; set; }
        public Decimal TahunBuku { get; set; }
        public Boolean Verifikasi { get; set; }
        public String Keterangan { get; set; }
        public String FileDikirim { get; set; }
        public Boolean FileDiterimaOperasional { get; set; }
        public Boolean FileDiterimaAkuntansi { get; set; }

    }
}