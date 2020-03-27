namespace aQord.ASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class storeArrayWithEFCFapproch2ndTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schematics", "NormalHoursData", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Schematics", "NormalHoursData");
        }
    }
}
