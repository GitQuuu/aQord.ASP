namespace aQord.ASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreateByPropToSchematicsModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schematics", "CreatedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Schematics", "CreatedBy");
        }
    }
}
