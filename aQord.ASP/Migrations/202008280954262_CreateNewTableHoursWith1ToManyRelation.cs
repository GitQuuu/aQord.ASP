namespace aQord.ASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateNewTableHoursWith1ToManyRelation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hours",
                c => new
                    {
                        HoursId = c.Int(nullable: false, identity: true),
                        AkkordHours = c.Double(nullable: false),
                        NormalHours = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.HoursId);
            
            AddColumn("dbo.Schematics", "Hours_HoursId", c => c.Int());
            CreateIndex("dbo.Schematics", "Hours_HoursId");
            AddForeignKey("dbo.Schematics", "Hours_HoursId", "dbo.Hours", "HoursId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Schematics", "Hours_HoursId", "dbo.Hours");
            DropIndex("dbo.Schematics", new[] { "Hours_HoursId" });
            DropColumn("dbo.Schematics", "Hours_HoursId");
            DropTable("dbo.Hours");
        }
    }
}
