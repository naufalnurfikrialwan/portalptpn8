using System.Data.Entity;

namespace Ptpn8.Areas.Referensi.Models
{
    public class ReferensiDbContext : DbContext
    {
        public ReferensiDbContext()
            : base("name=csReferensi")
        {

        }

        public DbSet<Organisasi> Organisasi { get; set; }
        public DbSet<NPWPOrganisasi> NPWPOrganisasi { get; set; }
        public DbSet<TeleponOrganisasi> TeleponOrganisasi { get; set; }
        public DbSet<FaksimiliOrganisasi> FaksimiliOrganisasi { get; set; }
        public DbSet<EmailOrganisasi> EmailOrganisasi { get; set; }
        public DbSet<PortalOrganisasi> PortalOrganisasi { get; set; }
        public DbSet<Komisaris> Komisaris { get; set; }
        public DbSet<Direksi> Direksi { get; set; }
        public DbSet<Wilayah> Wilayah { get; set; }
        public DbSet<Kebun> Kebun { get; set; }
        public DbSet<KepTan> KepTan { get; set; }
        public DbSet<Afdeling> Afdeling { get; set; }
        public DbSet<MandorBesar> MandorBesar { get; set; }
        public DbSet<Mandor> Mandor { get; set; }
        public DbSet<Bagian> Bagian { get; set; }
        public DbSet<Urusan> Urusan { get; set; }
        public DbSet<Bidang> Bidang { get; set; }
        public DbSet<Propinsi> Propinsi { get; set; }
        public DbSet<Kota> Kota { get; set; }
        public DbSet<Kecamatan> Kecamatan { get; set; }
        public DbSet<Kelurahan> Kelurahan { get; set; }
        public DbSet<MainBudidaya> MainBudidaya { get; set; }
        public DbSet<SubBudidaya> SubBudidaya { get; set; }
        public DbSet<Grade> Grade { get; set; }
        public DbSet<Varian> Varian { get; set; }
        public DbSet<Satuan> Satuan { get; set; }
        public DbSet<Kemasan> Kemasan { get; set; }
        public DbSet<Negara> Negara { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<BuyerBudidaya> BuyerBudidaya { get; set; }
        public DbSet<Rekening> Rekening { get; set; }
        public DbSet<KebunBudidaya> KebunBudidaya { get; set; }
        public DbSet<Blok> Blok { get; set; }
        public DbSet<BlokRealisasi> BlokRealisasi { get; set; }
        public DbSet<BlokRKAP> BlokRKAP { get; set; }
        public DbSet<BlokRJPP> BlokRJPP { get; set; }
        public DbSet<JenisTanah> JenisTanah { get; set; }
        public DbSet<StatusAreal> StatusAreal { get; set; }
        public DbSet<Kloon> Kloon { get; set; }
        public DbSet<HGU> HGU { get; set; }
        public DbSet<KebunPeta> KebunPeta { get; set; }
        public DbSet<AfdelingPeta> AfdelingPeta { get; set; }
        public DbSet<Broker> Broker { get; set; }
        public DbSet<Vessel> Vessel { get; set; }
        public DbSet<Barang> Barang { get; set; }
        public DbSet<Alokasi> Alokasi { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<KelompokTransferUang> KelompokTransferUang { get; set; }
        public DbSet<BankOrganisasi> BankOrganisasi { get; set; }
        public DbSet<BPD> BPD { get; set; }
        public DbSet<TipeKamar> TipeKamar { get; set; }
        public DbSet<AreaWisata> AreaWisata { get; set; }
        public DbSet<Customer_Agrowisata> Customer_Agrowisata { get; set; }
        public DbSet<TehRelasi_DaftarPenerima> TehRelasi_DaftarPenerima { get; set; }
        public DbSet<TehRelasi_DaftarProduk> TehRelasi_DaftarProduk { get; set; }
    }
}