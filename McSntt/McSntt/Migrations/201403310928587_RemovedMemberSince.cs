namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedMemberSince : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.People", "MemberSince");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "MemberSince", c => c.DateTime());
        }
    }
}
