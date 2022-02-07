using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("KebunPeta")]
    public class KebunPeta
    {
        public Guid KebunPetaId { get; set; }
        public Guid KebunId { get; set; }
        [NotMapped]
        public Kebun Kebun { get; set; }
        public DbGeometry Peta { get; set; }
    }
}