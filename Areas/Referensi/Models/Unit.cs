using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("Unit")]
    public class Unit
    {
        public Unit()
        {
            UnitId = Guid.NewGuid();
            OrgId = Guid.Empty;
            NamaUnit = "";
        }
        public Guid UnitId { get; set; }
        public Guid OrgId { get; set; }
        public string NamaUnit { get; set; }
        public string Nomenklatur { get; set; }
        public Guid IndukOrgId { get; set; }
        public string KodeKebun { get; set; }
        public string KodeAfdeling { get; set; }
        public DateTime TanggalKadaluarsa { get; set; } = new DateTime(1996, 3, 11);
    }
}