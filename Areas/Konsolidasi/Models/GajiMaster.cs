namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GajiMaster")]
    public partial class GajiMaster
    {
        [Key]
        [Column(Order = 0)]
        public Guid GajiMasterID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string kdunit { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime tanggal { get; set; }

        [StringLength(2)]
        public string kodeafd { get; set; }

        [StringLength(50)]
        public string regmdr { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string register { get; set; }

        [StringLength(50)]
        public string sts { get; set; }

        public double? PDP { get; set; }

        public double? pdpth { get; set; }

        public double? Jab { get; set; }

        public double? Pen { get; set; }

        public double? Diri { get; set; }

        public double? GajiPokok { get; set; }

        public double? TTetap { get; set; }

        public double? TKhusus { get; set; }

        public double? TSansos { get; set; }

        public double? TStruktural { get; set; }

        public double? TJabatan { get; set; }

        public double? TJamSostek { get; set; }

        public double? TDapenbun { get; set; }

        public double? TPPIP { get; set; }

        public double? TKomunikasi { get; set; }

        public double? TPeralihan { get; set; }

        public double? TTransportasi { get; set; }

        public double? UpahHarian { get; set; }

        public double? UpahBorong { get; set; }

        public double? UpahRecovery { get; set; }

        public double? TPPH21 { get; set; }

        public double? Rapel { get; set; }

        public double? Premi { get; set; }

        public double? PJamsostek { get; set; }

        public double? PDapenbun { get; set; }

        public double? PPPIP { get; set; }

        public double? PPPH21 { get; set; }

        public double? PKebun { get; set; }

        public double? PKoperasi { get; set; }

        public double? JmlHasil { get; set; }

        public double? Kerja { get; set; }

        public double? Borong { get; set; }

        public double? Recovery { get; set; }

        public double? Libur { get; set; }

        public double? Sakit { get; set; }

        public double? PermisiDibayar { get; set; }

        public double? CutiHamil { get; set; }

        public double? CutiTahunan { get; set; }

        public double? IjinTdkDibayar { get; set; }

        public double? Mangkir { get; set; }

        public double? CutiPanjang { get; set; }

        public double? CutiMBT { get; set; }

        public double? TBPJS { get; set; }

        public double? PBPJS { get; set; }
    }
}
