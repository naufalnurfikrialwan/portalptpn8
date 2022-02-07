using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ptpn8.UserManagement.Models
{
    public class ReportAccessHistory
    {
        public string NamaAplikasi { get; set; } = "";
        [DataType(DataType.DateTime)]
        public DateTime TanggalAkses { get; set; }
        public string UserName { get; set; } = "";
        public string LinkText { get; set; } = "";
        public string NamaUnit { get; set; } = "";
    }
}