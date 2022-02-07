using System.Data.Entity;


namespace Ptpn8.Areas.Areal.Models
{
    public class ArealdbContext : DbContext
    {
        public ArealdbContext()
            : base("name=csAreal")
        {

        }

        public DbSet<Project> Project { get; set; }
        public DbSet<ProjectBlok> ProjectBlok { get; set; }
        public DbSet<ProjectProgress> ProjectProgress { get; set; }
        public DbSet<StandardProduktivitasSawit> StandardProduktivitasSawit { get; set; }
        public DbSet<StandardProduktivitasKaret> StandardProduktivitasKaret { get; set; }
        public DbSet<StandardProduktivitasTeh> StandardProduktivitasTeh { get; set; }
        public object LegendPetaKegiatan { get; internal set; }
    }
}