namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AkunMemorial")]
    public partial class AkunMemorial
    {
        public Guid AkunMemorialID { get; set; }

        [StringLength(2)]
        public string KodeUnit { get; set; }

        public DateTime? tanggal { get; set; }

        [StringLength(50)]
        public string NoInput { get; set; }

        [StringLength(50)]
        public string NoBukti { get; set; }

        [StringLength(50)]
        public string Keterangan { get; set; }

        [StringLength(4)]
        public string NoUrut { get; set; }

        [StringLength(2)]
        public string kodeafd { get; set; }

        [StringLength(2)]
        public string kodebud { get; set; }

        [StringLength(9)]
        public string norek { get; set; }

        [StringLength(50)]
        public string netral { get; set; }

        [StringLength(50)]
        public string kdblok { get; set; }

        [StringLength(50)]
        public string thntnm { get; set; }

        public string uraian { get; set; }

        [StringLength(50)]
        public string unsur { get; set; }

        [StringLength(1)]
        public string DK { get; set; }

        public double? nilai { get; set; }

        public double? JmlFisik { get; set; }

        [StringLength(50)]
        public string NamaUser { get; set; }

        public DateTime? TglInput { get; set; }
    }

    public class SaldoperKebun
    {
        public int Tahun { get; set; }
        public int Bulan { get; set; }
        public string KodeKebun { get; set; }
        public Guid KebunId { get; set; }
        public string NamaKebun { get; set; }
        public string KodeBudidaya { get; set; }
        public Guid MainBudidayaId { get; set; }
        public string NamaBudidaya { get; set; }
        public string KodeUnsur { get; set; }
        public string KodeRekening { get; set; }
        public double? JumlahFisik { get; set; }
        public double? Nilai { get; set; }

    }

    public class SaldoperAfdeling
    {
        public int Tahun { get; set; }
        public int Bulan { get; set; }
        public string KodeAfdeling { get; set; }
        public Guid AfdelingId { get; set; }
        public string NamaAfdeling { get; set; }
        public Guid KebunId { get; set; }
        public string KodeBudidaya { get; set; }
        public Guid MainBudidayaId { get; set; }
        public string NamaBudidaya { get; set; }
        public string KodeUnsur { get; set; }
        public string KodeRekening { get; set; }
        public double? JumlahFisik { get; set; }
        public double? Nilai { get; set; }

    }

    public class SaldoperBlok
    {
        public int Tahun { get; set; }
        public int Bulan { get; set; }
        public string KodeBlok { get; set; }
        public Guid BlokId { get; set; }
        public string NamaBlok { get; set; }
        public Guid AfdelingId { get; set; }
        public string KodeBudidaya { get; set; }
        public Guid MainBudidayaId { get; set; }
        public string NamaBudidaya { get; set; }
        public string KodeUnsur { get; set; }
        public string KodeRekening { get; set; }
        public double? JumlahFisik { get; set; }
        public double? Nilai { get; set; }

    }
}
