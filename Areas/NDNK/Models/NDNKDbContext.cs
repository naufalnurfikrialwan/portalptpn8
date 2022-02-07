using System.Data.Entity;

namespace Ptpn8.Areas.NDNK.Models
{
    public class NDNKDbContext : DbContext
    {
        public NDNKDbContext()
            : base("name=csNDNK")
        {

        }

        public DbSet<StatusTahunBuku> StatusTahunBuku { get; set; }
        public DbSet<HDRKonsepKirimNota> HDRKonsepKirimNota { get; set; }
        public DbSet<DTLKonsepKirimNota> DTLKonsepKirimNota { get; set; }
    }
}