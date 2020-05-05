namespace aQord.ASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterProjectNumberPropertiesDataTypeToInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Schematics", "ProjectNumber", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Schematics", "ProjectNumber", c => c.String());
        }
    }
}
