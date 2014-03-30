namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMemberSince : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "MemberSince", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "MemberSince");
        }
    }
}
