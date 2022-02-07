namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_Afdeling
    {
        public Guid Ref_AfdelingID { get; set; }

        [StringLength(2)]
        public string kodekebun { get; set; }

        [StringLength(2)]
        public string kodeafdeling { get; set; }

        [StringLength(50)]
        public string namaafdeling { get; set; }
    }
}
