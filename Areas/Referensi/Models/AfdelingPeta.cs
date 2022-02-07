using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("AfdelingPeta")]
    public class AfdelingPeta
    {
        public Guid AfdelingPetaId { get; set; }
        public Guid AfdelingId { get; set; }
        [NotMapped]
        public Afdeling Afdeling { get; set; }
        public DbGeometry Peta { get; set; }
        public string kd_afd { get; set; }
    }
}