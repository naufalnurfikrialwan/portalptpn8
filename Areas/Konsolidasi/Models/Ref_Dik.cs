namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_Dik
    {
        public Guid Ref_DikID { get; set; }

        public DateTime? TANGGAL { get; set; }

        [StringLength(255)]
        public string REG { get; set; }

        [StringLength(255)]
        public string NAMA { get; set; }

        [StringLength(255)]
        public string NM_PGL { get; set; }

        [StringLength(255)]
        public string GLR_DPN { get; set; }

        [StringLength(255)]
        public string GLR_BLK { get; set; }

        [StringLength(255)]
        public string TPT_LAHIR { get; set; }

        public DateTime? TGL_LAHIR { get; set; }

        [StringLength(255)]
        public string KELAMIN { get; set; }

        [StringLength(255)]
        public string GOL_DARAH { get; set; }

        [StringLength(255)]
        public string AGAMA { get; set; }

        [StringLength(255)]
        public string ALAMAT { get; set; }

        [StringLength(255)]
        public string KOTA { get; set; }

        [StringLength(255)]
        public string TINGGAL { get; set; }

        [StringLength(255)]
        public string SIPIL { get; set; }

        [StringLength(255)]
        public string STAT_IS { get; set; }

        public DateTime? TGL_NIKAH { get; set; }

        public DateTime? TGL_CERAI { get; set; }

        public double? KANDUNG { get; set; }

        public double? ANGKAT { get; set; }

        public double? TANGGUNG { get; set; }

        [StringLength(255)]
        public string TGG_PPH { get; set; }

        [StringLength(255)]
        public string KD_PEND { get; set; }

        public DateTime? TGL_SK { get; set; }

        [StringLength(255)]
        public string NO_SK { get; set; }

        [StringLength(50)]
        public string KD_KELAS { get; set; }

        public DateTime? KLS_TMT { get; set; }

        [StringLength(255)]
        public string KLS_SK { get; set; }

        [StringLength(2)]
        public string GOL { get; set; }

        [StringLength(2)]
        public string MK { get; set; }

        public DateTime? GOL_TMT { get; set; }

        [StringLength(255)]
        public string GOL_SK { get; set; }

        [StringLength(255)]
        public string GPO { get; set; }

        [StringLength(255)]
        public string KD_KBN { get; set; }

        [StringLength(255)]
        public string KD_AFD { get; set; }

        [StringLength(255)]
        public string KD_JAB { get; set; }

        [StringLength(255)]
        public string NAMA_JAB { get; set; }

        [StringLength(255)]
        public string KD_BUD { get; set; }

        public DateTime? JAB_TMT { get; set; }

        [StringLength(255)]
        public string JAB_SK { get; set; }

        [StringLength(255)]
        public string ASTEK { get; set; }

        [StringLength(255)]
        public string TASPEN { get; set; }

        [StringLength(1)]
        public string SANSOS { get; set; }

        [StringLength(255)]
        public string STAT_DAP { get; set; }

        [StringLength(1)]
        public string MBT { get; set; }
    }
}
