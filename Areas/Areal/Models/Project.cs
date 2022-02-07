using Ptpn8.Areas.Referensi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Areal.Models
{
    [Table("Project")]
    public class Project
    {
        public Guid ProjectId { get; set; }
        public Guid BagianId { get; set; }
        [NotMapped]
        public Bagian Bagian { get; set; }
        public string KodeProject { get; set; }
        public string Tanggal { get; set; }
        public string Deskripsi { get; set; }
        public string Mitra { get; set; }
        public List<ProjectBlok> DaftarBlok { get; set; }
        public List<ProjectProgress> DaftarProgress { get; set; }
    }

    [Table("ProjectProgress")]
    public class ProjectProgress
    {
        public Guid ProjectProgressId { get; set; }
        public Guid ProjectId { get; set; }
        public string NamaKegiatan { get; set; }
        public string UraianKegiatan { get; set; }
        public DateTime StartDate { get; set;}
        public DateTime FinishDate { get; set; }
        public string PIC { get; set; }
        public string Progress { get; set; }
    }

    [Table("ProjectBlok")]
    public class ProjectBlok
    {
        public Guid ProjectBlokId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid BlokId { get; set; }
        [NotMapped]
        public Blok Blok { get; set; }
    }
}