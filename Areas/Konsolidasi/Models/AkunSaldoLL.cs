namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AkunSaldoLL")]
    public partial class AkunSaldoLL
    {
        public Guid AkunSaldoLLID { get; set; }

        [StringLength(2)]
        public string kodeunit { get; set; }

        public DateTime? tanggal { get; set; }

        [StringLength(9)]
        public string norek { get; set; }

        [StringLength(50)]
        public string netral { get; set; }

        [StringLength(3)]
        public string kdtrans { get; set; }

        [StringLength(2)]
        public string kodeafd { get; set; }

        [StringLength(2)]
        public string kodebud { get; set; }

        [StringLength(50)]
        public string kdblok { get; set; }

        [StringLength(4)]
        public string thntnm { get; set; }

        [StringLength(50)]
        public string kdselisihRK { get; set; }

        [StringLength(100)]
        public string uraian { get; set; }

        public double? rk { get; set; }

        public double? saldo_klas_a { get; set; }

        public double? saldo_klas_b { get; set; }

        public double? saldo_klas_c { get; set; }

        public double? saldo_bl_diseleksi { get; set; }

        public double? fisik_awal_tahun { get; set; }

        public double? biaya_awal_tahun { get; set; }

        public double? penambahan_bi { get; set; }

        public double? penambahan_sd_bl { get; set; }

        public double? ke_tti_bi { get; set; }

        public double? ke_tti_bl { get; set; }

        public double? ke_tbm_bi { get; set; }

        public double? ke_tbm_bl { get; set; }

        public double? ke_tm_bi { get; set; }

        public double? ke_tm_bl { get; set; }

        public double? ke_polybag_bi { get; set; }

        public double? ke_polybag_bl { get; set; }

        public double? dikirim_bi { get; set; }

        public double? dikirim_bl { get; set; }

        public double? dijual_bi { get; set; }

        public double? dijual_bl { get; set; }

        public double? mati_bi { get; set; }

        public double? mati_bl { get; set; }

        public double? HariKerja { get; set; }

        public double? BarangBahan { get; set; }

        public double? HasilKerja { get; set; }
    }
    
}
