using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Ptpn8.UserManagement.Models
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("csIdentity", throwIfV1Schema: false)
        {
        }
        public DbSet<ApplicationUserRole> ApplicationUserRole { get; set; }
        public DbSet<ApplicationRole> ApplicationRole { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string NoIndukKaryawan { get; set; }
        public string Nama { get; set; }
        public string Jabatan { get; set; }
        public string Telepon { get; set; }
        public Guid LokasiKerjaId { get; set; }
        public Guid PosisiLokasiKerjaId { get; set; }
        public bool IsApprove { get; set; }
        public DateTime TanggalAksesTerakhir { get; set; }
        [NotMapped]
        public string FileFoto { get; set; }
        [UIHint("imgUpload")]
        public byte[] Foto { get; set; }
        public bool StatusNoIndukKaryawan { get; set; }
        public bool StatusNama { get; set; }
        public bool StatusJabatan { get; set; }
        public bool StatusLokasiKerja { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationUserRole : IdentityUserRole
    {
        [NotMapped]
        [UIHint("aucRoleName")]
        public string RoleName { get; set; }
        public string Description { get; set; }
        public Guid Idd { get; set; } = Guid.NewGuid();

    }

    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
        
    }
   
}