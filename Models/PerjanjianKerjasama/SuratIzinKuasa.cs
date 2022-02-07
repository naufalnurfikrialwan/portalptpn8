using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    [Table("PK_SuratIzinKuasa")]
    public class SuratIzinKuasa
    {
        public Guid SuratIzinKuasaId { get; set; }
        public Guid InputKerjasamaKebunId { get; set; }
        public Guid UploadSuratIzinKuasaId { get; set; }
        public Decimal TahunBuku { get; set; }
    }
}