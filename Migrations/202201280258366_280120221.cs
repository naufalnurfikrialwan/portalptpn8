namespace Ptpn8.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _280120221 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PK_InputKerjasamaKebun",
                c => new
                {
                    InputKerjasamaKebun_HDRId = c.Guid(nullable: false),
                    KebunId = c.Guid(nullable: true),
                    MitraId = c.Guid(nullable: true),
                    NomorInput = c.Int(nullable: true),
                    TahunBuku = c.Int(nullable: true),
                    NomorPermohonan = c.String(),
                    TanggalPermohonan = c.DateTime(nullable: false),
                    NamaMitra = c.String(),
                    AlamatMitra = c.String(),
                    EmailMitra = c.String(),
                    NomorTelepon = c.String(),
                    File_SuratPermohonan = c.String(),
                    File_Proposal = c.String(),
                    File_DaftarNormatif = c.String(),
                    File_SuratPernyataan = c.String(),
                    File_SuratPengukuhan = c.String(),
                    File_Simluhtan = c.String(),
                    File_PengantarKebun = c.String(),
                    File_SaranPendapatKebun = c.String(),
                    File_Bapengukuran = c.String(),
                })
                .PrimaryKey(t => t.InputKerjasamaKebun_HDRId);

        }

        public override void Down()
        {
        }
    }
}
