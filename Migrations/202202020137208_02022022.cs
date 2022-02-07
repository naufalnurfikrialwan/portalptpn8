namespace Ptpn8.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _02022022 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PK_InputKerjasamaKebun", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PK_InputKerjasamaKebun", "UserName");
        }
    }
}
