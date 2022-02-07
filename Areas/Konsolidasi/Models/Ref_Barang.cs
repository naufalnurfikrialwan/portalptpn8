namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_Barang
    {
        public Guid Ref_BarangID { get; set; }

        [StringLength(9)]
        public string norek { get; set; }

        [StringLength(8)]
        public string KodeBarang { get; set; }

        [StringLength(50)]
        public string NamaBarang { get; set; }

        [StringLength(2)]
        public string KodeSatuan { get; set; }

        [StringLength(50)]
        public string NamaSatuan { get; set; }

        [StringLength(50)]
        public string InitSatuan { get; set; }

        [StringLength(1)]
        public string JenisBarang { get; set; }
    }
}
