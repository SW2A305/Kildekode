namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Event3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "SignUpMsg", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "SignUpMsg");
        }
    }
}
