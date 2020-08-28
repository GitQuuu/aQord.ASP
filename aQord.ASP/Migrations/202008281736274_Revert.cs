namespace aQord.ASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Revert : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Schematics", "Hours_HoursId", "dbo.Hours");
            DropIndex("dbo.Schematics", new[] { "Hours_HoursId" });
            DropColumn("dbo.Schematics", "Hours_HoursId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schematics", "Hours_HoursId", c => c.Int());
            CreateIndex("dbo.Schematics", "Hours_HoursId");
            AddForeignKey("dbo.Schematics", "Hours_HoursId", "dbo.Hours", "HoursId");
        }
    }
}
