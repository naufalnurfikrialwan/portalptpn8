namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AkunProduksiHP")]
    public partial class AkunProduksiHP
    {
        public Guid AkunProduksiHPID { get; set; }

        [StringLength(2)]
        public string kodeunit { get; set; }

        public DateTime? tanggal { get; set; }

        [StringLength(2)]
        public string kodebud { get; set; }

        [StringLength(4)]
        public string kodeJenisOlah { get; set; }

        [StringLength(4)]
        public string kodegrade { get; set; }

        [StringLength(50)]
        public string kodevar { get; set; }

        [StringLength(50)]
        public string norek { get; set; }

        [StringLength(2)]
        public string kodekbn { get; set; }

        public double? bi { get; set; }

        public double? rbi { get; set; }

        public double? pbi { get; set; }
    }

    public class ProduksiperBudidaya
    {
        public int Tahun { get; set; }
        public int Bulan { get; set; }
        public string KodeKebun { get; set; }
        public string KodeBudidaya { get; set; }
        public string Variable { get; set; }
        public string KodeRekening { get; set; }
        public double? JumlahRealisasi { get; set; }
        public double? JumlahRKAP { get; set; }
        public double? JumlahPKB { get; set; }

    }

    public class ProduksiperJenis
    {
        public int Tahun { get; set; }
        public int Bulan { get; set; }
        public string KodeKebun { get; set; }
        public string KodeJenisBudidaya { get; set; }
        public string Variable { get; set; }
        public string KodeRekening { get; set; }
        public double? JumlahRealisasi { get; set; }
        public double? JumlahRKAP { get; set; }
        public double? JumlahPKB { get; set; }

    }
}
