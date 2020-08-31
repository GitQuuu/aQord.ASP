namespace aQord.ASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BirkeHelpChangeDataTypeOfDay : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Hours", "Day", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Hours", "Day", c => c.String());
        }
    }
}
