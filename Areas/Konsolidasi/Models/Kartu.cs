namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kartu")]
    public partial class Kartu
    {
        public Guid KartuID { get; set; }

        [StringLength(2)]
        public string KodeUnit { get; set; }

        public DateTime? Tanggal { get; set; }

        [StringLength(50)]
        public string NoBukti { get; set; }

        [StringLength(50)]
        public string NoInput { get; set; }

        [StringLength(4)]
        public string KodeBudidaya { get; set; }

        [StringLength(2)]
        public string KodeAfdeling { get; set; }

        [StringLength(9)]
        public string Norek { get; set; }

        [StringLength(8)]
        public string KodeBarang { get; set; }

        [StringLength(16)]
        public string Netral { get; set; }

        [StringLength(4)]
        public string kdblok { get; set; }

        [StringLength(1)]
        public string KodeUnsur { get; set; }

        public string Uraian { get; set; }

        public double? Jml_fisik { get; set; }

        public double? Debet { get; set; }

        public double? Kredit { get; set; }

        [StringLength(3)]
        public string Aplikasi { get; set; }
    }

    public class KartuperKebun
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

    public class KartuperAfdeling
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

    public class KartuperBlok
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
