namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_UserID
    {
        [Key]
        [StringLength(50)]
        public string userID { get; set; }

        [StringLength(50)]
        public string userPWD { get; set; }
    }
}
