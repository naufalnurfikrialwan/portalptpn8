using System.Data.Entity;

namespace Ptpn8.Areas.SDM.Models
{
    public class SDMDbContext : DbContext
    {
        public SDMDbContext()
            : base("name=csSDM")
        {
        }

        public DbSet<DIK> DIK { get; set; }
        public DbSet<SDM01> SDM01 { get; set; }
        public DbSet<SDM08> SDM08 { get; set; }
    }
}