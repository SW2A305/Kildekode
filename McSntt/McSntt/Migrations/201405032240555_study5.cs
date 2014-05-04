namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class study5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "RopeWorks", c => c.Boolean());
            AddColumn("dbo.Lectures", "RopeWorks", c => c.Boolean(nullable: false));
            DropColumn("dbo.People", "RopeWorks");
            DropColumn("dbo.Lectures", "RopeWorks");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Lectures", "RopeWorks", c => c.Boolean(nullable: false));
            AddColumn("dbo.People", "RopeWorks", c => c.Boolean());
            DropColumn("dbo.Lectures", "RopeWorks");
            DropColumn("dbo.People", "RopeWorks");
        }
    }
}
