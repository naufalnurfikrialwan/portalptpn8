using System.Data.Entity;

namespace Ptpn8.Models
{
    public class Ptpn8dbContext : DbContext
    {
        public Ptpn8dbContext()
            : base("name=csPortalPTPN8")
        {

        }

        public DbSet<Ptpn8.Models.Aplikasi> Aplikasi { get; set; }
        public DbSet<Ptpn8.Models.Menu> Menu { get; set; }
        public DbSet<Ptpn8.Models.InisiasiInputProperty> InisiasiInputProperty { get; set; }
        public DbSet<Ptpn8.Models.KondisiCRUD> KondisiCRUD { get; set; }
        public DbSet<Ptpn8.Models.RiwayatAkses> RiwayatAkses { get; set; }
        //public DbSet<Ptpn8.Models.MenuTab> MenuTab { get; set; }

    }
}