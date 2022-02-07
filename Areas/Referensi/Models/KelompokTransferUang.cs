using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("KelompokTransferUang")]
    public class KelompokTransferUang
    {
        public Guid KelompokTransferUangId { get; set; }
        public string NamaKelompok { get; set; }
    }
}