namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_Areal
    {
        public Guid Ref_ArealID { get; set; }

        [StringLength(2)]
        public string kodekebun { get; set; }

        [StringLength(4)]
        public string kodebudidaya { get; set; }

        [StringLength(2)]
        public string kodeafdeling { get; set; }

        [StringLength(4)]
        public string kodeblok { get; set; }

        [StringLength(50)]
        public string namablok { get; set; }

        [StringLength(50)]
        public string status { get; set; }

        [StringLength(100)]
        public string tahuntanam { get; set; }

        public decimal? luas { get; set; }

        [StringLength(50)]
        public string kodekloon { get; set; }

        [StringLength(50)]
        public string netral { get; set; }

        public double? jmlpohon { get; set; }

        public double? tinggipohon { get; set; }

        public double? topologi { get; set; }

        [StringLength(50)]
        public string exareal { get; set; }

        [StringLength(2)]
        public string KodeTopografi { get; set; }

        [StringLength(2)]
        public string KodeKetinggian { get; set; }

        public double? mdpl { get; set; }

        public DateTime? tanggal { get; set; }

        public double? jmlPatok { get; set; }
    }
}
