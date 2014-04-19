namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BoatsMissingImagePath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Boats", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Boats", "ImagePath");
        }
    }
}
