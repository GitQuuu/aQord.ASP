namespace aQord.ASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDataannotationToSchematicModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Schematics", "WeekNumber", c => c.Int());
            AlterColumn("dbo.Schematics", "ShelterRateAmountOfDays", c => c.Double());
            AlterColumn("dbo.Schematics", "MileageAllowanceAmountOfKm", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Schematics", "MileageAllowanceAmountOfKm", c => c.Double(nullable: false));
            AlterColumn("dbo.Schematics", "ShelterRateAmountOfDays", c => c.Double(nullable: false));
            AlterColumn("dbo.Schematics", "WeekNumber", c => c.Int(nullable: false));
        }
    }
}
