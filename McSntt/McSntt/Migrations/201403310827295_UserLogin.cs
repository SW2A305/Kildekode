namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserLogin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "Username", c => c.String());
            AddColumn("dbo.People", "PasswordHash", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "PasswordHash");
            DropColumn("dbo.People", "Username");
        }
    }
}
