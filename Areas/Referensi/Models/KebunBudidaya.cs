using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("KebunBudidaya")]
    public class KebunBudidaya
    {
        public Guid KebunBudidayaId { get; set; } = Guid.NewGuid();
        public Guid KebunId { get; set; } = Guid.Empty;

        [NotMapped]
        [UIHint("ddlKebun")]
        public Kebun Kebun { get; set; }
        public Guid MainBudidayaId { get; set; } = Guid.Empty;
        [NotMapped]
        public MainBudidaya Budidaya { get; set; }
        public Guid SubBudidayaId { get; set; } = Guid.Empty;
        [NotMapped]
        [UIHint("aucNamaSubBudidaya")]
        public string NamaSubBudidaya { get; set; } = "";
        public string kd_merk { get; set; } = "";
        public string Merk { get; set; } = "";
        public int NoUrut { get; set; } = 0;
        public string KodeKPBN { get; set; } = "";

    }
}