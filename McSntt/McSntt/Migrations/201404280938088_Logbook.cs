namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Logbook : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Logbooks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActualDepartureTime = c.DateTime(nullable: false),
                        ActualArrivalTime = c.DateTime(nullable: false),
                        DamageInflicted = c.Boolean(nullable: false),
                        DamageDescription = c.String(),
                        AnswerFromBoatChief = c.String(),
                        FiledBy_PersonId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.FiledBy_PersonId)
                .Index(t => t.FiledBy_PersonId);
            
            AddColumn("dbo.People", "Logbook_Id", c => c.Int());
            AddColumn("dbo.RegularTrips", "Logbook_Id", c => c.Int());
            CreateIndex("dbo.People", "Logbook_Id");
            CreateIndex("dbo.RegularTrips", "Logbook_Id");
            AddForeignKey("dbo.RegularTrips", "Logbook_Id", "dbo.Logbooks", "Id");
            AddForeignKey("dbo.People", "Logbook_Id", "dbo.Logbooks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Logbooks", "FiledBy_PersonId", "dbo.People");
            DropForeignKey("dbo.People", "Logbook_Id", "dbo.Logbooks");
            DropForeignKey("dbo.RegularTrips", "Logbook_Id", "dbo.Logbooks");
            DropIndex("dbo.RegularTrips", new[] { "Logbook_Id" });
            DropIndex("dbo.People", new[] { "Logbook_Id" });
            DropIndex("dbo.Logbooks", new[] { "FiledBy_PersonId" });
            DropColumn("dbo.RegularTrips", "Logbook_Id");
            DropColumn("dbo.People", "Logbook_Id");
            DropTable("dbo.Logbooks");
        }
    }
}
