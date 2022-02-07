namespace Ptpn8.Areas.Konsolidasi.Models
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class KonsolidasiDbContext : DbContext
    {
        public KonsolidasiDbContext()
            : base("name=csKonsol")
        {
            var objectContext = (this as IObjectContextAdapter).ObjectContext;

            // Sets the command timeout for all the commands
            objectContext.CommandTimeout = 1000;
        }

        public virtual DbSet<AktivaMaster> AktivaMaster { get; set; }
        public virtual DbSet<AktivaSaldo> AktivaSaldo { get; set; }
        public virtual DbSet<AktivaTransaksi> AktivaTransaksi { get; set; }
        public virtual DbSet<AkunKomentar> AkunKomentar { get; set; }
        public virtual DbSet<AkunMemorial> AkunMemorial { get; set; }
        public virtual DbSet<AkunProduksiHP> AkunProduksiHP { get; set; }
        public virtual DbSet<AkunRKAP> AkunRKAP { get; set; }
        public virtual DbSet<AkunSaldoLL> AkunSaldoLL { get; set; }
        public virtual DbSet<Eap> Eap { get; set; }
        public virtual DbSet<GajiAbsensi> GajiAbsensi { get; set; }
        public virtual DbSet<GajiJurnal> GajiJurnal { get; set; }
        public virtual DbSet<GajiKulir> GajiKulir { get; set; }
        public virtual DbSet<GajiMaster> GajiMaster { get; set; }
        public virtual DbSet<GajiPotongan> GajiPotongan { get; set; }
        public virtual DbSet<GajiPremi> GajiPremi { get; set; }
        public virtual DbSet<GajiPremiPanen> GajiPremiPanen { get; set; }
        public virtual DbSet<GajiSPKHonor> GajiSPKHonor { get; set; }
        public virtual DbSet<GajiTaksasi> GajiTaksasi { get; set; }
        public virtual DbSet<GJIHrgbrgll> GJIHrgbrgll { get; set; }
        public virtual DbSet<Kartu> Kartu { get; set; }
        public virtual DbSet<KasBank> KasBank { get; set; }
        public virtual DbSet<Ref_Afdeling> Ref_Afdeling { get; set; }
        public virtual DbSet<Ref_Areal> Ref_Areal { get; set; }
        public virtual DbSet<Ref_AsalProd> Ref_AsalProd { get; set; }
        public virtual DbSet<Ref_Barang> Ref_Barang { get; set; }
        public virtual DbSet<Ref_Basic> Ref_Basic { get; set; }
        public virtual DbSet<Ref_Budidaya> Ref_Budidaya { get; set; }
        public virtual DbSet<Ref_Dik> Ref_Dik { get; set; }
        public virtual DbSet<Ref_DikKLM> Ref_DikKLM { get; set; }
        public virtual DbSet<Ref_GradeIHT> Ref_GradeIHT { get; set; }
        public virtual DbSet<Ref_JenisPesemaian> Ref_JenisPesemaian { get; set; }
        public virtual DbSet<Ref_JenisTunjangan> Ref_JenisTunjangan { get; set; }
        public virtual DbSet<Ref_JenisUpah> Ref_JenisUpah { get; set; }
        public virtual DbSet<Ref_Kebun> Ref_Kebun { get; set; }
        public virtual DbSet<Ref_KelompokAset> Ref_KelompokAset { get; set; }
        public virtual DbSet<Ref_KelompokJurnalGaji> Ref_KelompokJurnalGaji { get; set; }
        public virtual DbSet<Ref_KodeArusKas> Ref_KodeArusKas { get; set; }
        public virtual DbSet<Ref_KodeKetinggianPohon> Ref_KodeKetinggianPohon { get; set; }
        public virtual DbSet<Ref_KodeLegalitas> Ref_KodeLegalitas { get; set; }
        public virtual DbSet<Ref_KodePanen> Ref_KodePanen { get; set; }
        public virtual DbSet<Ref_KomponenGaji1> Ref_KomponenGaji1 { get; set; }
        public virtual DbSet<Ref_LM> Ref_LM { get; set; }
        public virtual DbSet<Ref_Netral> Ref_Netral { get; set; }
        public virtual DbSet<Ref_NoBuktiJurnalGaji> Ref_NoBuktiJurnalGaji { get; set; }
        public virtual DbSet<Ref_RekeningInvestasi> Ref_RekeningInvestasi { get; set; }
        public virtual DbSet<Ref_ResolusiLayar> Ref_ResolusiLayar { get; set; }
        public virtual DbSet<Ref_Satuan> Ref_Satuan { get; set; }
        public virtual DbSet<Ref_SatuanHasil> Ref_SatuanHasil { get; set; }
        public virtual DbSet<Ref_Strook> Ref_Strook { get; set; }
        public virtual DbSet<Ref_TarifAktiva> Ref_TarifAktiva { get; set; }
        public virtual DbSet<Ref_UserID> Ref_UserID { get; set; }
        public virtual DbSet<SDM02> SDM02 { get; set; }
        public virtual DbSet<xyz> xyz { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AktivaMaster>()
                .Property(e => e.kodeunit)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaMaster>()
                .Property(e => e.nobukti)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaMaster>()
                .Property(e => e.noaktiva)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaMaster>()
                .Property(e => e.kdafd)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaMaster>()
                .Property(e => e.kdbud)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaMaster>()
                .Property(e => e.kelompok)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaMaster>()
                .Property(e => e.norek)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaMaster>()
                .Property(e => e.nmaktiva)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaMaster>()
                .Property(e => e.kelompokaset)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaMaster>()
                .Property(e => e.satuan)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaMaster>()
                .Property(e => e.stopr)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaMaster>()
                .Property(e => e.thntnm)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaSaldo>()
                .Property(e => e.kodeunit)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaSaldo>()
                .Property(e => e.noaktiva)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaSaldo>()
                .Property(e => e.kdafd)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaSaldo>()
                .Property(e => e.kdbud)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaSaldo>()
                .Property(e => e.kelompok)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaSaldo>()
                .Property(e => e.norek)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaSaldo>()
                .Property(e => e.nmaktiva)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaSaldo>()
                .Property(e => e.kelompokaset)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaSaldo>()
                .Property(e => e.satuan)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaSaldo>()
                .Property(e => e.nobukti)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaSaldo>()
                .Property(e => e.leveransir)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaSaldo>()
                .Property(e => e.stopr)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaTransaksi>()
                .Property(e => e.kodeunit)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaTransaksi>()
                .Property(e => e.nourut)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaTransaksi>()
                .Property(e => e.noinput)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaTransaksi>()
                .Property(e => e.kelompok)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaTransaksi>()
                .Property(e => e.noaktiva)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaTransaksi>()
                .Property(e => e.noaktivab)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaTransaksi>()
                .Property(e => e.nmaktiva)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaTransaksi>()
                .Property(e => e.kdafd)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaTransaksi>()
                .Property(e => e.kdbud)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaTransaksi>()
                .Property(e => e.norek)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaTransaksi>()
                .Property(e => e.norekl)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaTransaksi>()
                .Property(e => e.nobukti)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaTransaksi>()
                .Property(e => e.leveransir)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaTransaksi>()
                .Property(e => e.satuan)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaTransaksi>()
                .Property(e => e.nilaipenyusutan)
                .HasPrecision(18, 0);

            modelBuilder.Entity<AktivaTransaksi>()
                .Property(e => e.nilaipenyusutan_thl)
                .HasPrecision(18, 0);

            modelBuilder.Entity<AktivaTransaksi>()
                .Property(e => e.nilaipenyusutan_sbl)
                .HasPrecision(18, 0);

            modelBuilder.Entity<AktivaTransaksi>()
                .Property(e => e.nilaikoreksipenyusutan)
                .HasPrecision(18, 0);

            modelBuilder.Entity<AktivaTransaksi>()
                .Property(e => e.kelompokaset)
                .IsUnicode(false);

            modelBuilder.Entity<AktivaTransaksi>()
                .Property(e => e.thntnm)
                .IsUnicode(false);

            modelBuilder.Entity<AkunKomentar>()
                .Property(e => e.kodeunit)
                .IsUnicode(false);

            modelBuilder.Entity<AkunKomentar>()
                .Property(e => e.kdtrans)
                .IsUnicode(false);

            modelBuilder.Entity<AkunKomentar>()
                .Property(e => e.kodebud)
                .IsUnicode(false);

            modelBuilder.Entity<AkunKomentar>()
                .Property(e => e.kodeolah)
                .IsUnicode(false);

            modelBuilder.Entity<AkunKomentar>()
                .Property(e => e.kodekom)
                .IsUnicode(false);

            modelBuilder.Entity<AkunKomentar>()
                .Property(e => e.uraian)
                .IsUnicode(false);

            modelBuilder.Entity<AkunMemorial>()
                .Property(e => e.KodeUnit)
                .IsUnicode(false);

            modelBuilder.Entity<AkunMemorial>()
                .Property(e => e.NoInput)
                .IsUnicode(false);

            modelBuilder.Entity<AkunMemorial>()
                .Property(e => e.NoBukti)
                .IsUnicode(false);

            modelBuilder.Entity<AkunMemorial>()
                .Property(e => e.Keterangan)
                .IsUnicode(false);

            modelBuilder.Entity<AkunMemorial>()
                .Property(e => e.NoUrut)
                .IsUnicode(false);

            modelBuilder.Entity<AkunMemorial>()
                .Property(e => e.kodeafd)
                .IsUnicode(false);

            modelBuilder.Entity<AkunMemorial>()
                .Property(e => e.kodebud)
                .IsUnicode(false);

            modelBuilder.Entity<AkunMemorial>()
                .Property(e => e.norek)
                .IsUnicode(false);

            modelBuilder.Entity<AkunMemorial>()
                .Property(e => e.netral)
                .IsUnicode(false);

            modelBuilder.Entity<AkunMemorial>()
                .Property(e => e.kdblok)
                .IsUnicode(false);

            modelBuilder.Entity<AkunMemorial>()
                .Property(e => e.thntnm)
                .IsUnicode(false);

            modelBuilder.Entity<AkunMemorial>()
                .Property(e => e.uraian)
                .IsUnicode(false);

            modelBuilder.Entity<AkunMemorial>()
                .Property(e => e.unsur)
                .IsUnicode(false);

            modelBuilder.Entity<AkunMemorial>()
                .Property(e => e.DK)
                .IsUnicode(false);

            modelBuilder.Entity<AkunMemorial>()
                .Property(e => e.NamaUser)
                .IsUnicode(false);

            modelBuilder.Entity<AkunProduksiHP>()
                .Property(e => e.kodeunit)
                .IsUnicode(false);

            modelBuilder.Entity<AkunProduksiHP>()
                .Property(e => e.kodebud)
                .IsUnicode(false);

            modelBuilder.Entity<AkunProduksiHP>()
                .Property(e => e.kodeJenisOlah)
                .IsUnicode(false);

            modelBuilder.Entity<AkunProduksiHP>()
                .Property(e => e.kodegrade)
                .IsUnicode(false);

            modelBuilder.Entity<AkunProduksiHP>()
                .Property(e => e.kodevar)
                .IsUnicode(false);

            modelBuilder.Entity<AkunProduksiHP>()
                .Property(e => e.norek)
                .IsUnicode(false);

            modelBuilder.Entity<AkunProduksiHP>()
                .Property(e => e.kodekbn)
                .IsUnicode(false);

            modelBuilder.Entity<AkunRKAP>()
                .Property(e => e.kodeunit)
                .IsUnicode(false);

            modelBuilder.Entity<AkunRKAP>()
                .Property(e => e.unsur)
                .IsUnicode(false);

            modelBuilder.Entity<AkunRKAP>()
                .Property(e => e.kodeafd)
                .IsUnicode(false);

            modelBuilder.Entity<AkunRKAP>()
                .Property(e => e.kodebud)
                .IsUnicode(false);

            modelBuilder.Entity<AkunRKAP>()
                .Property(e => e.norek)
                .IsUnicode(false);

            modelBuilder.Entity<AkunRKAP>()
                .Property(e => e.netral)
                .IsUnicode(false);

            modelBuilder.Entity<AkunRKAP>()
                .Property(e => e.kodebarang)
                .IsUnicode(false);

            modelBuilder.Entity<AkunRKAP>()
                .Property(e => e.klsKary)
                .IsUnicode(false);

            modelBuilder.Entity<AkunSaldoLL>()
                .Property(e => e.kodeunit)
                .IsUnicode(false);

            modelBuilder.Entity<AkunSaldoLL>()
                .Property(e => e.norek)
                .IsUnicode(false);

            modelBuilder.Entity<AkunSaldoLL>()
                .Property(e => e.netral)
                .IsUnicode(false);

            modelBuilder.Entity<AkunSaldoLL>()
                .Property(e => e.kdtrans)
                .IsUnicode(false);

            modelBuilder.Entity<AkunSaldoLL>()
                .Property(e => e.kodeafd)
                .IsUnicode(false);

            modelBuilder.Entity<AkunSaldoLL>()
                .Property(e => e.kodebud)
                .IsUnicode(false);

            modelBuilder.Entity<AkunSaldoLL>()
                .Property(e => e.kdblok)
                .IsUnicode(false);

            modelBuilder.Entity<AkunSaldoLL>()
                .Property(e => e.thntnm)
                .IsUnicode(false);

            modelBuilder.Entity<AkunSaldoLL>()
                .Property(e => e.kdselisihRK)
                .IsUnicode(false);

            modelBuilder.Entity<AkunSaldoLL>()
                .Property(e => e.uraian)
                .IsUnicode(false);

            modelBuilder.Entity<Eap>()
                .Property(e => e.kodeunit)
                .IsUnicode(false);

            modelBuilder.Entity<Eap>()
                .Property(e => e.kdtrans)
                .IsUnicode(false);

            modelBuilder.Entity<Eap>()
                .Property(e => e.noinput)
                .IsUnicode(false);

            modelBuilder.Entity<Eap>()
                .Property(e => e.nourut)
                .IsUnicode(false);

            modelBuilder.Entity<Eap>()
                .Property(e => e.nomor)
                .IsUnicode(false);

            modelBuilder.Entity<Eap>()
                .Property(e => e.pengemudi)
                .IsUnicode(false);

            modelBuilder.Entity<Eap>()
                .Property(e => e.pembantu)
                .IsUnicode(false);

            modelBuilder.Entity<Eap>()
                .Property(e => e.namapemakai)
                .IsUnicode(false);

            modelBuilder.Entity<Eap>()
                .Property(e => e.kdafd)
                .IsUnicode(false);

            modelBuilder.Entity<Eap>()
                .Property(e => e.kdbud)
                .IsUnicode(false);

            modelBuilder.Entity<Eap>()
                .Property(e => e.asal)
                .IsUnicode(false);

            modelBuilder.Entity<Eap>()
                .Property(e => e.tujuan)
                .IsUnicode(false);

            modelBuilder.Entity<Eap>()
                .Property(e => e.waktukeluar)
                .IsUnicode(false);

            modelBuilder.Entity<Eap>()
                .Property(e => e.waktumasuk)
                .IsUnicode(false);

            modelBuilder.Entity<Eap>()
                .Property(e => e.norek)
                .IsUnicode(false);

            modelBuilder.Entity<Eap>()
                .Property(e => e.netral)
                .IsUnicode(false);

            modelBuilder.Entity<Eap>()
                .Property(e => e.kdblok)
                .IsUnicode(false);

            modelBuilder.Entity<Eap>()
                .Property(e => e.thntnm)
                .IsUnicode(false);

            modelBuilder.Entity<Eap>()
                .Property(e => e.krbb)
                .IsUnicode(false);

            modelBuilder.Entity<Eap>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.kodeunit)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.kodeafd)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.kodeafd1)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.kodetrans)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.kodebud)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.kodebud1)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.regmdr)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.nama)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.sts)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.register)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.kodeabs)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.norek)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.netral)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.thntnm)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.tkesulitan)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.kdblok)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.kdjnsupah)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.kdgrade)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.kdpanen)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.stpikul)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.grup)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.satuan)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.sbrd)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.kodekelas)
                .IsUnicode(false);

            modelBuilder.Entity<GajiAbsensi>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<GajiJurnal>()
                .Property(e => e.kodeunit)
                .IsUnicode(false);

            modelBuilder.Entity<GajiJurnal>()
                .Property(e => e.kodeafd)
                .IsUnicode(false);

            modelBuilder.Entity<GajiJurnal>()
                .Property(e => e.kodebud)
                .IsUnicode(false);

            modelBuilder.Entity<GajiJurnal>()
                .Property(e => e.grup)
                .IsUnicode(false);

            modelBuilder.Entity<GajiJurnal>()
                .Property(e => e.nobukti)
                .IsUnicode(false);

            modelBuilder.Entity<GajiJurnal>()
                .Property(e => e.uraian)
                .IsUnicode(false);

            modelBuilder.Entity<GajiJurnal>()
                .Property(e => e.norek)
                .IsUnicode(false);

            modelBuilder.Entity<GajiJurnal>()
                .Property(e => e.DK)
                .IsUnicode(false);

            modelBuilder.Entity<GajiJurnal>()
                .Property(e => e.netral)
                .IsUnicode(false);

            modelBuilder.Entity<GajiJurnal>()
                .Property(e => e.thntnm)
                .IsUnicode(false);

            modelBuilder.Entity<GajiKulir>()
                .Property(e => e.kodeunit)
                .IsUnicode(false);

            modelBuilder.Entity<GajiKulir>()
                .Property(e => e.kodetrans)
                .IsUnicode(false);

            modelBuilder.Entity<GajiKulir>()
                .Property(e => e.kodeafd)
                .IsUnicode(false);

            modelBuilder.Entity<GajiKulir>()
                .Property(e => e.kodebud)
                .IsUnicode(false);

            modelBuilder.Entity<GajiKulir>()
                .Property(e => e.regmdr)
                .IsUnicode(false);

            modelBuilder.Entity<GajiKulir>()
                .Property(e => e.register)
                .IsUnicode(false);

            modelBuilder.Entity<GajiKulir>()
                .Property(e => e.sts)
                .IsUnicode(false);

            modelBuilder.Entity<GajiKulir>()
                .Property(e => e.norek)
                .IsUnicode(false);

            modelBuilder.Entity<GajiKulir>()
                .Property(e => e.netral)
                .IsUnicode(false);

            modelBuilder.Entity<GajiKulir>()
                .Property(e => e.kdblok)
                .IsUnicode(false);

            modelBuilder.Entity<GajiKulir>()
                .Property(e => e.thntnm)
                .IsUnicode(false);

            modelBuilder.Entity<GajiKulir>()
                .Property(e => e.stb)
                .IsUnicode(false);

            modelBuilder.Entity<GajiMaster>()
                .Property(e => e.kdunit)
                .IsUnicode(false);

            modelBuilder.Entity<GajiMaster>()
                .Property(e => e.kodeafd)
                .IsUnicode(false);

            modelBuilder.Entity<GajiMaster>()
                .Property(e => e.regmdr)
                .IsUnicode(false);

            modelBuilder.Entity<GajiMaster>()
                .Property(e => e.register)
                .IsUnicode(false);

            modelBuilder.Entity<GajiMaster>()
                .Property(e => e.sts)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPotongan>()
                .Property(e => e.kodeunit)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPotongan>()
                .Property(e => e.kodeafd)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPotongan>()
                .Property(e => e.kodeafd1)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPotongan>()
                .Property(e => e.kodebud)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPotongan>()
                .Property(e => e.kodebud1)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPotongan>()
                .Property(e => e.sts)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPotongan>()
                .Property(e => e.regmdr)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPotongan>()
                .Property(e => e.kodetrans)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPotongan>()
                .Property(e => e.jnsptg)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPotongan>()
                .Property(e => e.register)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPotongan>()
                .Property(e => e.nama)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPotongan>()
                .Property(e => e.keterangan)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremi>()
                .Property(e => e.kodeunit)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremi>()
                .Property(e => e.kodetrans)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremi>()
                .Property(e => e.kodeAfd)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremi>()
                .Property(e => e.kodeAfd1)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremi>()
                .Property(e => e.kodebud)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremi>()
                .Property(e => e.kodebud1)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremi>()
                .Property(e => e.jnsprm)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremi>()
                .Property(e => e.sts)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremi>()
                .Property(e => e.regmdr)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremi>()
                .Property(e => e.register)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremi>()
                .Property(e => e.nama)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremi>()
                .Property(e => e.norek)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremi>()
                .Property(e => e.netral)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremi>()
                .Property(e => e.thntnm)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremiPanen>()
                .Property(e => e.kodeunit)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremiPanen>()
                .Property(e => e.kodetrans)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremiPanen>()
                .Property(e => e.kodebud)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremiPanen>()
                .Property(e => e.kodeafd)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremiPanen>()
                .Property(e => e.sts)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremiPanen>()
                .Property(e => e.regmdr)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremiPanen>()
                .Property(e => e.register)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremiPanen>()
                .Property(e => e.nama)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremiPanen>()
                .Property(e => e.norek)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremiPanen>()
                .Property(e => e.tkesulitan)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremiPanen>()
                .Property(e => e.kodepanen)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremiPanen>()
                .Property(e => e.ndxmutu)
                .IsUnicode(false);

            modelBuilder.Entity<GajiPremiPanen>()
                .Property(e => e.klas)
                .IsUnicode(false);

            modelBuilder.Entity<GajiSPKHonor>()
                .Property(e => e.kodeunit)
                .IsUnicode(false);

            modelBuilder.Entity<GajiSPKHonor>()
                .Property(e => e.kodetrans)
                .IsUnicode(false);

            modelBuilder.Entity<GajiSPKHonor>()
                .Property(e => e.NoSPK)
                .IsUnicode(false);

            modelBuilder.Entity<GajiSPKHonor>()
                .Property(e => e.nourut)
                .IsUnicode(false);

            modelBuilder.Entity<GajiSPKHonor>()
                .Property(e => e.kodeafd)
                .IsUnicode(false);

            modelBuilder.Entity<GajiSPKHonor>()
                .Property(e => e.kodebud)
                .IsUnicode(false);

            modelBuilder.Entity<GajiSPKHonor>()
                .Property(e => e.nama)
                .IsUnicode(false);

            modelBuilder.Entity<GajiSPKHonor>()
                .Property(e => e.kdjnsupah)
                .IsUnicode(false);

            modelBuilder.Entity<GajiSPKHonor>()
                .Property(e => e.norek)
                .IsUnicode(false);

            modelBuilder.Entity<GajiSPKHonor>()
                .Property(e => e.netral)
                .IsUnicode(false);

            modelBuilder.Entity<GajiSPKHonor>()
                .Property(e => e.kdblok)
                .IsUnicode(false);

            modelBuilder.Entity<GajiSPKHonor>()
                .Property(e => e.thntnm)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.kodeunit)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.kodeafd)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.kodeafd1)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.kodetrans)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.kodebud)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.kodebud1)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.regmdr)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.nama)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.sts)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.register)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.kodeabs)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.norek)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.netral)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.thntnm)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.tkesulitan)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.kdblok)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.kdjnsupah)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.kdgrade)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.kdpanen)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.stpikul)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.grup)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.satuan)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.sbrd)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.kodekelas)
                .IsUnicode(false);

            modelBuilder.Entity<GajiTaksasi>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<GJIHrgbrgll>()
                .Property(e => e.kodeunit)
                .IsUnicode(false);

            modelBuilder.Entity<GJIHrgbrgll>()
                .Property(e => e.kodeafd)
                .IsUnicode(false);

            modelBuilder.Entity<GJIHrgbrgll>()
                .Property(e => e.thntnm)
                .IsUnicode(false);

            modelBuilder.Entity<Kartu>()
                .Property(e => e.KodeUnit)
                .IsUnicode(false);

            modelBuilder.Entity<Kartu>()
                .Property(e => e.NoBukti)
                .IsUnicode(false);

            modelBuilder.Entity<Kartu>()
                .Property(e => e.NoInput)
                .IsUnicode(false);

            modelBuilder.Entity<Kartu>()
                .Property(e => e.KodeBudidaya)
                .IsUnicode(false);

            modelBuilder.Entity<Kartu>()
                .Property(e => e.KodeAfdeling)
                .IsUnicode(false);

            modelBuilder.Entity<Kartu>()
                .Property(e => e.Norek)
                .IsUnicode(false);

            modelBuilder.Entity<Kartu>()
                .Property(e => e.KodeBarang)
                .IsUnicode(false);

            modelBuilder.Entity<Kartu>()
                .Property(e => e.Netral)
                .IsUnicode(false);

            modelBuilder.Entity<Kartu>()
                .Property(e => e.kdblok)
                .IsUnicode(false);

            modelBuilder.Entity<Kartu>()
                .Property(e => e.KodeUnsur)
                .IsUnicode(false);

            modelBuilder.Entity<Kartu>()
                .Property(e => e.Uraian)
                .IsUnicode(false);

            modelBuilder.Entity<Kartu>()
                .Property(e => e.Aplikasi)
                .IsUnicode(false);

            modelBuilder.Entity<KasBank>()
                .Property(e => e.KodeUnit)
                .IsUnicode(false);

            modelBuilder.Entity<KasBank>()
                .Property(e => e.KT)
                .IsUnicode(false);

            modelBuilder.Entity<KasBank>()
                .Property(e => e.Inisial)
                .IsUnicode(false);

            modelBuilder.Entity<KasBank>()
                .Property(e => e.NoInput)
                .IsUnicode(false);

            modelBuilder.Entity<KasBank>()
                .Property(e => e.Nama)
                .IsUnicode(false);

            modelBuilder.Entity<KasBank>()
                .Property(e => e.Alamat)
                .IsUnicode(false);

            modelBuilder.Entity<KasBank>()
                .Property(e => e.Ket)
                .IsUnicode(false);

            modelBuilder.Entity<KasBank>()
                .Property(e => e.RekKB)
                .IsUnicode(false);

            modelBuilder.Entity<KasBank>()
                .Property(e => e.NoUrut)
                .IsUnicode(false);

            modelBuilder.Entity<KasBank>()
                .Property(e => e.KdBud)
                .IsUnicode(false);

            modelBuilder.Entity<KasBank>()
                .Property(e => e.kdAfd)
                .IsUnicode(false);

            modelBuilder.Entity<KasBank>()
                .Property(e => e.Norek)
                .IsUnicode(false);

            modelBuilder.Entity<KasBank>()
                .Property(e => e.Netral)
                .IsUnicode(false);

            modelBuilder.Entity<KasBank>()
                .Property(e => e.Kdblok)
                .IsUnicode(false);

            modelBuilder.Entity<KasBank>()
                .Property(e => e.Uraian)
                .IsUnicode(false);

            modelBuilder.Entity<KasBank>()
                .Property(e => e.Nilai)
                .HasPrecision(18, 0);

            modelBuilder.Entity<KasBank>()
                .Property(e => e.KdArus)
                .IsUnicode(false);

            modelBuilder.Entity<KasBank>()
                .Property(e => e.namauser)
                .IsUnicode(false);

            modelBuilder.Entity<KasBank>()
                .Property(e => e.thntnm)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Afdeling>()
                .Property(e => e.kodekebun)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Afdeling>()
                .Property(e => e.kodeafdeling)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Afdeling>()
                .Property(e => e.namaafdeling)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Areal>()
                .Property(e => e.kodekebun)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Areal>()
                .Property(e => e.kodebudidaya)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Areal>()
                .Property(e => e.kodeafdeling)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Areal>()
                .Property(e => e.kodeblok)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Areal>()
                .Property(e => e.namablok)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Areal>()
                .Property(e => e.status)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Areal>()
                .Property(e => e.tahuntanam)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Areal>()
                .Property(e => e.luas)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Ref_Areal>()
                .Property(e => e.kodekloon)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Areal>()
                .Property(e => e.netral)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Areal>()
                .Property(e => e.exareal)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Areal>()
                .Property(e => e.KodeTopografi)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Areal>()
                .Property(e => e.KodeKetinggian)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_AsalProd>()
                .Property(e => e.kdasal)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_AsalProd>()
                .Property(e => e.uraian)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_AsalProd>()
                .Property(e => e.rekBasah)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_AsalProd>()
                .Property(e => e.rekJadi)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Barang>()
                .Property(e => e.norek)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Barang>()
                .Property(e => e.KodeBarang)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Barang>()
                .Property(e => e.NamaBarang)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Barang>()
                .Property(e => e.KodeSatuan)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Barang>()
                .Property(e => e.NamaSatuan)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Barang>()
                .Property(e => e.InitSatuan)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Barang>()
                .Property(e => e.JenisBarang)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Basic>()
                .Property(e => e.kode)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Basic>()
                .Property(e => e.KodeUnit)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Basic>()
                .Property(e => e.kodeafd)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Basic>()
                .Property(e => e.kodebud)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Basic>()
                .Property(e => e.regmdr)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Basic>()
                .Property(e => e.register)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Basic>()
                .Property(e => e.nama)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Basic>()
                .Property(e => e.sts)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Basic>()
                .Property(e => e.kdblok)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Basic>()
                .Property(e => e.thntnm)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Basic>()
                .Property(e => e.kdpanen)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Basic>()
                .Property(e => e.tkesulitan)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Budidaya>()
                .Property(e => e.KodeUnit)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Budidaya>()
                .Property(e => e.kd_bud)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Budidaya>()
                .Property(e => e.nm_bud)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.REG)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.NAMA)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.NM_PGL)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.GLR_DPN)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.GLR_BLK)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.TPT_LAHIR)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.KELAMIN)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.GOL_DARAH)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.AGAMA)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.ALAMAT)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.KOTA)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.TINGGAL)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.SIPIL)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.STAT_IS)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.TGG_PPH)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.KD_PEND)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.NO_SK)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.KD_KELAS)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.KLS_SK)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.GOL)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.MK)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.GOL_SK)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.GPO)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.KD_KBN)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.KD_AFD)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.KD_JAB)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.NAMA_JAB)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.KD_BUD)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.JAB_SK)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.ASTEK)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.TASPEN)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.SANSOS)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.STAT_DAP)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Dik>()
                .Property(e => e.MBT)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.REG)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.NAMA)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.TPT_LAHIR)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.KELAMIN)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.GOL_DARAH)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.AGAMA)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.ALAMAT)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.KOTA)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.TINGGAL)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.SIPIL)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.KANDUNG)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.ANGKAT)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.TANGGUNG)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.KD_PEND)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.NO_SK)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.KD_KELAS)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.KLS_SK)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.GOL)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.MK)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.GOL_SK)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.KD_KBN)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.KD_AFD)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.KD_MB)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.KD_MDR)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.STATUS_JAB)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.KD_JAB)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.NAMA_JAB)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.KD_BUD)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.JAB_SK)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.ASTEK)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.TASPEN)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_DikKLM>()
                .Property(e => e.JUBILIUM)
                .IsUnicode(false);


            modelBuilder.Entity<Ref_GradeIHT>()
                .Property(e => e.kd_bud)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_GradeIHT>()
                .Property(e => e.kd_jnsprod)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_GradeIHT>()
                .Property(e => e.kd_grade)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_GradeIHT>()
                .Property(e => e.uraian)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_GradeIHT>()
                .Property(e => e.kd_satuan)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_GradeIHT>()
                .Property(e => e.satuan)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_JenisPesemaian>()
                .Property(e => e.norek)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_JenisPesemaian>()
                .Property(e => e.uraian)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_JenisTunjangan>()
                .Property(e => e.JnsTunjangan)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_JenisTunjangan>()
                .Property(e => e.kdjabatan)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_JenisTunjangan>()
                .Property(e => e.nmjabatan)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_JenisUpah>()
                .Property(e => e.kodebud)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_JenisUpah>()
                .Property(e => e.kodeupah)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_JenisUpah>()
                .Property(e => e.uraian)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Kebun>()
                .Property(e => e.KodeKebun)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Kebun>()
                .Property(e => e.NamaKebun)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_KelompokAset>()
                .Property(e => e.kelompokAset)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_KelompokAset>()
                .Property(e => e.uraian)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_KelompokJurnalGaji>()
                .Property(e => e.kode)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_KelompokJurnalGaji>()
                .Property(e => e.uraian)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_KelompokJurnalGaji>()
                .Property(e => e.namajurnal)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_KelompokJurnalGaji>()
                .Property(e => e.Filtering)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_KodeArusKas>()
                .Property(e => e.kd_arus)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_KodeArusKas>()
                .Property(e => e.uraian)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_KodeKetinggianPohon>()
                .Property(e => e.kode)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_KodeKetinggianPohon>()
                .Property(e => e.ketinggian)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_KodeLegalitas>()
                .Property(e => e.kode)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_KodeLegalitas>()
                .Property(e => e.uraian)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_KodePanen>()
                .Property(e => e.kodebudidaya)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_KodePanen>()
                .Property(e => e.kodepanen)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_KodePanen>()
                .Property(e => e.uraian)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_KomponenGaji1>()
                .Property(e => e.GOLBARU)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_KomponenGaji1>()
                .Property(e => e.GOL)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_KomponenGaji1>()
                .Property(e => e.KDGOL)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_KomponenGaji1>()
                .Property(e => e.MK)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_LM>()
                .Property(e => e.kodeunit)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_LM>()
                .Property(e => e.kode)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_LM>()
                .Property(e => e.nmMenu)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_LM>()
                .Property(e => e.kdgrup)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_LM>()
                .Property(e => e.nmgrup)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_LM>()
                .Property(e => e.urut)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_LM>()
                .Property(e => e.norek)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_LM>()
                .Property(e => e.uraian)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_LM>()
                .Property(e => e.tampil)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Netral>()
                .Property(e => e.KodeUnit)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Netral>()
                .Property(e => e.Norek)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Netral>()
                .Property(e => e.Nomor)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Netral>()
                .Property(e => e.Uraian)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Netral>()
                .Property(e => e.Tahun)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Netral>()
                .Property(e => e.BBM)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Netral>()
                .Property(e => e.Pemilik)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Netral>()
                .Property(e => e.Pengemudi)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Netral>()
                .Property(e => e.NoChasis)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Netral>()
                .Property(e => e.NoMesin)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Netral>()
                .Property(e => e.NoBPKB)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Netral>()
                .Property(e => e.Tonage)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Netral>()
                .Property(e => e.UkuranBan)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Netral>()
                .Property(e => e.Voltage)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Netral>()
                .Property(e => e.BebanRekening)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_NoBuktiJurnalGaji>()
                .Property(e => e.kodepdp)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_NoBuktiJurnalGaji>()
                .Property(e => e.nourut)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_NoBuktiJurnalGaji>()
                .Property(e => e.namapdp)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_NoBuktiJurnalGaji>()
                .Property(e => e.nobukti)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_RekeningInvestasi>()
                .Property(e => e.KRBB)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_RekeningInvestasi>()
                .Property(e => e.Norek)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_RekeningInvestasi>()
                .Property(e => e.NamaRekening)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_ResolusiLayar>()
                .Property(e => e.Resolusi)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_ResolusiLayar>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Satuan>()
                .Property(e => e.KodeSatuan)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Satuan>()
                .Property(e => e.NamaSatuan)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Satuan>()
                .Property(e => e.Inisial)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_SatuanHasil>()
                .Property(e => e.Satuan)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Strook>()
                .Property(e => e.Grup)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Strook>()
                .Property(e => e.urut)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Strook>()
                .Property(e => e.kode)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_Strook>()
                .Property(e => e.uraian)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_TarifAktiva>()
                .Property(e => e.kdbud)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_TarifAktiva>()
                .Property(e => e.Norek)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_TarifAktiva>()
                .Property(e => e.kelompokaset)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_TarifAktiva>()
                .Property(e => e.Keterangan)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_TarifAktiva>()
                .Property(e => e.Uraian)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_TarifAktiva>()
                .Property(e => e.Tahun)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_UserID>()
                .Property(e => e.userID)
                .IsUnicode(false);

            modelBuilder.Entity<Ref_UserID>()
                .Property(e => e.userPWD)
                .IsUnicode(false);

            modelBuilder.Entity<SDM02>()
                .Property(e => e.NPP)
                .IsUnicode(false);

            modelBuilder.Entity<SDM02>()
                .Property(e => e.NoUrut)
                .IsUnicode(false);

            modelBuilder.Entity<SDM02>()
                .Property(e => e.Nama)
                .IsUnicode(false);

            modelBuilder.Entity<SDM02>()
                .Property(e => e.Hubungan)
                .IsUnicode(false);

            modelBuilder.Entity<SDM02>()
                .Property(e => e.Kota)
                .IsUnicode(false);

            modelBuilder.Entity<SDM02>()
                .Property(e => e.Propinsi)
                .IsUnicode(false);

            modelBuilder.Entity<SDM02>()
                .Property(e => e.Negara)
                .IsUnicode(false);

            modelBuilder.Entity<SDM02>()
                .Property(e => e.JenisKelamin)
                .IsUnicode(false);

            modelBuilder.Entity<SDM02>()
                .Property(e => e.GolDarah)
                .IsUnicode(false);

            modelBuilder.Entity<SDM02>()
                .Property(e => e.Agama)
                .IsUnicode(false);

            modelBuilder.Entity<SDM02>()
                .Property(e => e.TKPendidikan)
                .IsUnicode(false);

            modelBuilder.Entity<SDM02>()
                .Property(e => e.StatSipil)
                .IsUnicode(false);

            modelBuilder.Entity<SDM02>()
                .Property(e => e.StatKerja)
                .IsUnicode(false);

            modelBuilder.Entity<SDM02>()
                .Property(e => e.StatTanggung)
                .IsUnicode(false);

            modelBuilder.Entity<SDM02>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<xyz>()
                .Property(e => e.kd_bud)
                .IsUnicode(false);

            modelBuilder.Entity<xyz>()
                .Property(e => e.nm_bud)
                .IsUnicode(false);
        }
    }
}
