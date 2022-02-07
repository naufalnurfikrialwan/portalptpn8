using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    [Table("PK_Invoice")]
    public class Invoice
    {
        public Guid InvoiceId { get; set; }
        public Guid InputKerjasamaKebunId { get; set; }
        public Guid UploadPerjanjianKerjasamaId { get; set; }
        public Guid VerifikasiPerjanjianKerjasamaId { get; set; }
        public Decimal TahunBuku { get; set; }
    }
}