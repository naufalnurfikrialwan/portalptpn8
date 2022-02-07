using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    
    public class View_VerifikasiHasilSurvey
    {
        public Guid VerifikasiHasilSurveyId { get; set; } = Guid.NewGuid();
        public Guid InputKerjasamaKebunId { get; set; } = Guid.Empty;
        public Guid UploadHasilSurveyId { get; set; } = Guid.Empty;
        public Guid KebunId { get; set; } = Guid.Empty;
        public Decimal TahunBuku { get; set; } = 0;
        public String NomorInputView { get; set; } = "";
        public Boolean Verifikasi { get; set; }= false;
        public String Keterangan { get; set; } = "";
        public String NamaKebun { get; set; } = "";
        public String NamaMitra { get; set; } = "";
        public String AlamatMitra { get; set; } = "";
        public String NomorTelepon { get; set; } = "";
        public string File_SuratPermohonan { get; set; } = "";
        public string File_Proposal { get; set; } = "";
        public string File_DaftarNormatif { get; set; } = "";
        public string File_SuratPernyataan { get; set; } = "";
        public string File_SuratPengukuhan { get; set; } = "";
        public string File_Simultan { get; set; } = "";
        public string File_PengantarKebun { get; set; } = "";
        public string File_SaranPendapatKebun { get; set; } = "";
        public string File_Bapengukuran { get; set; } = "";
        public String File_BaNegosiasiKompensasi { get; set; } = "";
        public String File_MemoKajianDanNilaiKompensasi { get; set; } = "";
    }
}