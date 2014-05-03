namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Event4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Created", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "Created");
        }
    }
}
