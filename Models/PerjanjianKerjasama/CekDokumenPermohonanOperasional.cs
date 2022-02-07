using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    [Table("PK_CekDokumenPermohonanOperasional")]
    public class CekDokumenPermohonanOperasional
    {
        public Guid CekDokumenPermohonanOperasionalId { get; set; }
        public Guid InputKerjasamaKebunId { get; set; }
        public Guid VerifikasiPermohonanId { get; set; }
        public Decimal TahunBuku { get; set; }
        public String Keterangan { get; set; }
        public String NamaKebun { get; set; }
        public String NamaMitra { get; set; }
        public String AlamatMitra { get; set; }
        public String NomorTelepon { get; set; }
        public String File_SuratPermohonan { get; set; }
        public String File_Proposal { get; set; }
        public String File_DaftarNormatif { get; set; }
        public String File_SuratPengukuhan { get; set; }
        public String File_Simultan { get; set; }
        public String File_PengantarKebun { get; set; }
        public String File_SaranPendapatKebun { get; set; }
        public String File_Bapengukuran { get; set; }
    }
}