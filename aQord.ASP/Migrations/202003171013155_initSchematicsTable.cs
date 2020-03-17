namespace aQord.ASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initSchematicsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Schematics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeOfWork = c.String(),
                        StaffRepresentative = c.String(),
                        Year = c.Int(nullable: false),
                        Firm = c.String(),
                        WorkplaceAddress = c.String(),
                        ProjectNumber = c.String(),
                        CraftsmanId = c.Int(nullable: false),
                        Name = c.String(),
                        WeekNumber = c.Int(nullable: false),
                        ShelterRateAmountOfDays = c.Double(nullable: false),
                        MileageAllowanceAmountOfKm = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Schematics");
        }
    }
}
