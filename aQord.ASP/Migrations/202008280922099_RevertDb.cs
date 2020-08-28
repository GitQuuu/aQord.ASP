namespace aQord.ASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RevertDb : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SchemaHours", "SchematicsId", "dbo.Schematics");
            DropIndex("dbo.SchemaHours", new[] { "SchematicsId" });
            DropTable("dbo.SchemaHours");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SchemaHours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SchemaId = c.Int(nullable: false),
                        AkkordHours = c.Double(nullable: false),
                        NormalHours = c.Double(nullable: false),
                        SchematicsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.SchemaHours", "SchematicsId");
            AddForeignKey("dbo.SchemaHours", "SchematicsId", "dbo.Schematics", "Id", cascadeDelete: true);
        }
    }
}
