namespace Ptpn8.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _270120222 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PK_InputKerjasamaKebun", "NomorInput", c => c.Int(nullable: true));
            AddColumn("dbo.PK_InputKerjasamaKebun", "NomorPermohonan", c => c.String());
            AddColumn("dbo.PK_InputKerjasamaKebun", "TanggalPermohonan", c => c.DateTime(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PK_InputKerjasamaKebun", "TanggalPermohonan");
            DropColumn("dbo.PK_InputKerjasamaKebun", "NomorPermohonan");
            DropColumn("dbo.PK_InputKerjasamaKebun", "NomorInput");
        }
    }
}
