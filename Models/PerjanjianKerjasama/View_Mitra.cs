using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    public class View_Mitra
    {
        public Guid MitraId { get; set; } = Guid.NewGuid();
        public Guid KebunId { get; set; } = Guid.Empty;
        public Decimal TahunBuku { get; set; } = 0;
        public String NomorInputView { get; set; } = "";
        public String NamaKebun { get; set; } = "";
        public String NamaMitra { get; set; } = "";
        public String AlamatMitra { get; set; } = "";
        public String NomorTelepon { get; set; } = "";
        public String EmailMitra { get; set; } = "";

    }
}