namespace aQord.ASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredAnnotationToPropInHours : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Hours", "Schematics_Id", "dbo.Schematics");
            DropIndex("dbo.Hours", new[] { "Schematics_Id" });
            AlterColumn("dbo.Hours", "Schematics_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Hours", "Schematics_Id");
            AddForeignKey("dbo.Hours", "Schematics_Id", "dbo.Schematics", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Hours", "Schematics_Id", "dbo.Schematics");
            DropIndex("dbo.Hours", new[] { "Schematics_Id" });
            AlterColumn("dbo.Hours", "Schematics_Id", c => c.Int());
            CreateIndex("dbo.Hours", "Schematics_Id");
            AddForeignKey("dbo.Hours", "Schematics_Id", "dbo.Schematics", "Id");
        }
    }
}
