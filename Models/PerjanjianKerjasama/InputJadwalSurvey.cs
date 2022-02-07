using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    [Table("PK_InputJadwalSurvey")]
    public class InputJadwalSurvey
    {
        public Guid InputJadwalSurveyId { get; set; }
        public Guid InputKerjasamaKebunId { get; set; }
        public Guid KebunId { get; set; }
        public Decimal TahunBuku { get; set; }
        public String JadwalSurvey { get; set; }
    }
}