namespace aQord.ASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEmployterSignaturePropToSchematicModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schematics", "EmployerSignature", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Schematics", "EmployerSignature");
        }
    }
}
