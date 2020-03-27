namespace aQord.ASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class storeDoubleArrayWithEFCFapproch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schematics", "HoursInAkkordData", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Schematics", "HoursInAkkordData");
        }
    }
}
