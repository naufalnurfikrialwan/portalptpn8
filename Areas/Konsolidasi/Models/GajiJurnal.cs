namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GajiJurnal")]
    public partial class GajiJurnal
    {
        public Guid GajiJurnalID { get; set; }

        public DateTime? tanggal { get; set; }

        [StringLength(50)]
        public string kodeunit { get; set; }

        [StringLength(50)]
        public string kodeafd { get; set; }

        [StringLength(50)]
        public string kodebud { get; set; }

        [StringLength(2)]
        public string grup { get; set; }

        [StringLength(50)]
        public string nobukti { get; set; }

        public string uraian { get; set; }

        [StringLength(9)]
        public string norek { get; set; }

        [StringLength(1)]
        public string DK { get; set; }

        public double? nilai { get; set; }

        [StringLength(50)]
        public string netral { get; set; }

        [StringLength(50)]
        public string thntnm { get; set; }

        public double? jmlfisik { get; set; }
    }
}
