namespace Ptpn8.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _27012022 : DbMigration
    {
        public override void Up()
        {
            
            CreateTable(
                "dbo.PK_InputKerjasamaKebun",
                c => new
                    {
                        InputKerjasamaKebunId = c.Guid(nullable: false),
                        KebunId = c.Guid(nullable: false),
                        MitraId = c.Guid(nullable: false),
                        TahunBuku = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                .PrimaryKey(t => t.InputKerjasamaKebunId);
            
            
        }
        
        public override void Down()
        {
            
        }
    }
}
