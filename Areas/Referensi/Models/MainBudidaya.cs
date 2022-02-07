using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models
{
    [Table("MainBudidaya")]
    public class MainBudidaya
    {
        public MainBudidaya()
        {
            MainBudidayaId = Guid.NewGuid();
            Nama = "";
            kd_bud = "";
            FileFoto = "";
            Warna = "";
        }

    [Required(ErrorMessage = "Id Budidaya Pokok Harus Diisi")]
        public Guid MainBudidayaId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Nama Budidaya Pokok Harus Diisi")]
        public String Nama { get; set; } = "";
        [Required(ErrorMessage = "kd_bud Harus Diisi")]
        public String kd_bud { get; set; } = "";

        public List<SubBudidaya> SubBudidaya { get; set; }

        [NotMapped]
        public String FileFoto { get; set; } = "";
        [UIHint("imgUpload")]
        public byte[] Foto { get; set; } = { };

        //public List<BuyerBudidaya> DaftarBuyer { get; set; }
        //public List<Tanaman.Kloon> DaftarKloon { get; set; }
        public string Warna { get; set; } = "";
    }
}