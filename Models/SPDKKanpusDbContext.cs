
using Ptpn8.PerjanjianKerjasama.Models;
using System.Data.Entity;

namespace Ptpn8.Models
{
    public class SPDKKanpusDbContext : DbContext
    {
        public SPDKKanpusDbContext() : base("name=csSPDKKanpus")
        {

        }

        // Penjualan
       

        // PerjanjianKerjasama
        public DbSet<PerjanjianKerjasama_Dokumen> PerjanjianKerjasama_Dokumen { get; set; }
        public DbSet<InputKerjasamaKebun_HDR> InputKerjasamaKebun_HDR { get; set; }
        public DbSet<InputKerjasamaKebun_DTL> InputKerjasamaKebun_DTL { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
        }
    }
}