namespace aQord.ASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPropertiesToStoreSignature : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schematics", "MySignature", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Schematics", "MySignature");
        }
    }
}
