namespace aQord.ASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HelpFromBanke : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Hours", "Day", c => c.String());
            AddColumn("dbo.Hours", "Schematics_Id", c => c.Int());
            CreateIndex("dbo.Hours", "Schematics_Id");
            AddForeignKey("dbo.Hours", "Schematics_Id", "dbo.Schematics", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Hours", "Schematics_Id", "dbo.Schematics");
            DropIndex("dbo.Hours", new[] { "Schematics_Id" });
            DropColumn("dbo.Hours", "Schematics_Id");
            DropColumn("dbo.Hours", "Day");
        }
    }
}
