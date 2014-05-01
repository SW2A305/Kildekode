namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Event2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Event_EventId", c => c.Int());
            CreateIndex("dbo.Events", "Event_EventId");
            AddForeignKey("dbo.Events", "Event_EventId", "dbo.Events", "EventId");
            DropColumn("dbo.Events", "EventCreatedBy");
        }

        public override void Down()
        {
            AddColumn("dbo.Events", "EventCreatedBy", c => c.String());
            DropForeignKey("dbo.Events", "Event_EventId", "dbo.Events");
            DropIndex("dbo.Events", new[] { "Event_EventId" });
            DropColumn("dbo.Events", "Event_EventId");
        }
    }
}
