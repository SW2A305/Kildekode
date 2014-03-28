namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonUpdates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "PhoneNumber", c => c.String());
            AddColumn("dbo.People", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "Email");
            DropColumn("dbo.People", "PhoneNumber");
        }
    }
}
