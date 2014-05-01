namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        EventDate = c.DateTime(nullable: false),
                        EventTitle = c.String(),
                        SignUpReq = c.Boolean(nullable: false),
                        Description = c.String(),
                        Event_EventId = c.Int(),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.Events", t => t.Event_EventId)
                .Index(t => t.Event_EventId);
            
            AddColumn("dbo.People", "Event_EventId", c => c.Int());
            CreateIndex("dbo.People", "Event_EventId");
            AddForeignKey("dbo.People", "Event_EventId", "dbo.Events", "EventId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.People", "Event_EventId", "dbo.Events");
            DropForeignKey("dbo.Events", "Event_EventId", "dbo.Events");
            DropIndex("dbo.People", new[] { "Event_EventId" });
            DropIndex("dbo.Events", new[] { "Event_EventId" });
            DropColumn("dbo.People", "Event_EventId");
            DropTable("dbo.Events");
        }
    }
}
