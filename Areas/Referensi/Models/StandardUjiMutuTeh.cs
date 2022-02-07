using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ptpn8.Areas.Referensi.Models
{
    //[Table("StandardUjiMutuTeh")]
    public class StandardUjiMutuTeh
    {
        [Required]
        public Guid StandardUjiMutuTehId { get; set; }
        [Required]
        public Guid MainBudidayaId { get; set; }

        [NotMapped]
        [Required]
        public string NamaMainBudidaya { get; set; }
        [Required]
        public Guid SubBudidayaId { get; set; }
        [NotMapped]
        [Required]
        public string NamaSubBudidaya { get; set; }
        [Required]
        public string KelasGrade { get; set; }          // Broken Grade, Small Grade
        [Required]
        public string KelompokDataran { get; set; }     // High Grown, Medium Grown, Low Grown
        [Required]
        public string Kriteria { get; set; }            // Appearance, Liquor dam Infusion
        [Required]
        public string Karakteristik { get; set; }       // Appearance: Warna, Karataan, Kebrsihan, dan Bentuk & Ukuran 
                                                        // Liquor: Warna Air, Kekuatan, dan Aroma
                                                        // Infusion: Warna, dan Kerataan
        [Required]
        public string Uraian { get; set; }
        [Required]
        public int ScoreAwal { get; set; }
        [Required]
        public int ScoreAkhir { get; set; }
        [Required]
        public string Penilaian { get; set; }           // Appearance: Well Made, Good, Fair Made, Unsatisfactory, Bad
                                                        // Liquor: Very Good, Good, Fairly Good, Unsatisfactory, Bad
                                                        // Infusion: Very Good, Good, Fairly Good, Unsatisfactory, Bad
        [Required]
        public string Penilaian_Huruf { get; set; }     // A,B,C,D,E

    }
}