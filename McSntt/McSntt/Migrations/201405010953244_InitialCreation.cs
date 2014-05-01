namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Boats",
                c => new
                    {
                        BoatId = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        NickName = c.String(),
                        ImagePath = c.String(),
                        Operational = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BoatId);
            
            CreateTable(
                "dbo.Logbooks",
                c => new
                    {
                        LogbookId = c.Int(nullable: false, identity: true),
                        ActualDepartureTime = c.DateTime(nullable: false),
                        ActualArrivalTime = c.DateTime(nullable: false),
                        DamageInflicted = c.Boolean(nullable: false),
                        DamageDescription = c.String(),
                        AnswerFromBoatChief = c.String(),
                        FiledBy_PersonId = c.Int(),
                    })
                .PrimaryKey(t => t.LogbookId)
                .ForeignKey("dbo.SailClubMembers", t => t.FiledBy_PersonId)
                .Index(t => t.FiledBy_PersonId);
            
            CreateTable(
                "dbo.Persons",
                c => new
                    {
                        PersonId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Address = c.String(),
                        Postcode = c.String(),
                        Cityname = c.String(),
                        DateOfBirth = c.String(),
                        BoatDriver = c.Boolean(nullable: false),
                        Gender = c.Int(nullable: false),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.PersonId);
            
            CreateTable(
                "dbo.RegularTrips",
                c => new
                    {
                        RegularTripId = c.Int(nullable: false, identity: true),
                        ExpectedArrivalTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        PurposeAndArea = c.String(),
                        SailTripId = c.Int(nullable: false),
                        DepartureTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ArrivalTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        WeatherConditions = c.String(),
                        Comments = c.String(),
                        Boat_BoatId = c.Int(),
                        Logbook_LogbookId = c.Int(),
                        Captain_PersonId = c.Int(),
                    })
                .PrimaryKey(t => t.RegularTripId)
                .ForeignKey("dbo.Boats", t => t.Boat_BoatId)
                .ForeignKey("dbo.Logbooks", t => t.Logbook_LogbookId)
                .ForeignKey("dbo.Persons", t => t.Captain_PersonId)
                .Index(t => t.Boat_BoatId)
                .Index(t => t.Logbook_LogbookId)
                .Index(t => t.Captain_PersonId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        EventTitle = c.String(),
                        EventCreatedBy = c.String(),
                        SignUpReq = c.Boolean(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.EventId);
            
            CreateTable(
                "dbo.PersonEvents",
                c => new
                    {
                        Person_PersonId = c.Int(nullable: false),
                        Event_EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Person_PersonId, t.Event_EventId })
                .ForeignKey("dbo.Persons", t => t.Person_PersonId, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.Event_EventId, cascadeDelete: true)
                .Index(t => t.Person_PersonId)
                .Index(t => t.Event_EventId);
            
            CreateTable(
                "dbo.PersonLogbooks",
                c => new
                    {
                        Person_PersonId = c.Int(nullable: false),
                        Logbook_LogbookId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Person_PersonId, t.Logbook_LogbookId })
                .ForeignKey("dbo.Persons", t => t.Person_PersonId, cascadeDelete: true)
                .ForeignKey("dbo.Logbooks", t => t.Logbook_LogbookId, cascadeDelete: true)
                .Index(t => t.Person_PersonId)
                .Index(t => t.Logbook_LogbookId);
            
            CreateTable(
                "dbo.PersonRegularTrips",
                c => new
                    {
                        Person_PersonId = c.Int(nullable: false),
                        RegularTrip_RegularTripId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Person_PersonId, t.RegularTrip_RegularTripId })
                .ForeignKey("dbo.Persons", t => t.Person_PersonId, cascadeDelete: true)
                .ForeignKey("dbo.RegularTrips", t => t.RegularTrip_RegularTripId, cascadeDelete: true)
                .Index(t => t.Person_PersonId)
                .Index(t => t.RegularTrip_RegularTripId);
            
            CreateTable(
                "dbo.SailClubMembers",
                c => new
                    {
                        PersonId = c.Int(nullable: false),
                        SailClubMemberId = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                        Username = c.String(),
                        PasswordHash = c.String(),
                    })
                .PrimaryKey(t => t.PersonId)
                .ForeignKey("dbo.Persons", t => t.PersonId)
                .Index(t => t.PersonId)
                .Index(t => t.SailClubMemberId, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SailClubMembers", "PersonId", "dbo.Persons");
            DropForeignKey("dbo.Logbooks", "FiledBy_PersonId", "dbo.SailClubMembers");
            DropForeignKey("dbo.PersonRegularTrips", "RegularTrip_RegularTripId", "dbo.RegularTrips");
            DropForeignKey("dbo.PersonRegularTrips", "Person_PersonId", "dbo.Persons");
            DropForeignKey("dbo.PersonLogbooks", "Logbook_LogbookId", "dbo.Logbooks");
            DropForeignKey("dbo.PersonLogbooks", "Person_PersonId", "dbo.Persons");
            DropForeignKey("dbo.PersonEvents", "Event_EventId", "dbo.Events");
            DropForeignKey("dbo.PersonEvents", "Person_PersonId", "dbo.Persons");
            DropForeignKey("dbo.RegularTrips", "Captain_PersonId", "dbo.Persons");
            DropForeignKey("dbo.RegularTrips", "Logbook_LogbookId", "dbo.Logbooks");
            DropForeignKey("dbo.RegularTrips", "Boat_BoatId", "dbo.Boats");
            DropIndex("dbo.SailClubMembers", new[] { "SailClubMemberId" });
            DropIndex("dbo.SailClubMembers", new[] { "PersonId" });
            DropIndex("dbo.PersonRegularTrips", new[] { "RegularTrip_RegularTripId" });
            DropIndex("dbo.PersonRegularTrips", new[] { "Person_PersonId" });
            DropIndex("dbo.PersonLogbooks", new[] { "Logbook_LogbookId" });
            DropIndex("dbo.PersonLogbooks", new[] { "Person_PersonId" });
            DropIndex("dbo.PersonEvents", new[] { "Event_EventId" });
            DropIndex("dbo.PersonEvents", new[] { "Person_PersonId" });
            DropIndex("dbo.RegularTrips", new[] { "Captain_PersonId" });
            DropIndex("dbo.RegularTrips", new[] { "Logbook_LogbookId" });
            DropIndex("dbo.RegularTrips", new[] { "Boat_BoatId" });
            DropIndex("dbo.Logbooks", new[] { "FiledBy_PersonId" });
            DropTable("dbo.SailClubMembers");
            DropTable("dbo.PersonRegularTrips");
            DropTable("dbo.PersonLogbooks");
            DropTable("dbo.PersonEvents");
            DropTable("dbo.Events");
            DropTable("dbo.RegularTrips");
            DropTable("dbo.Persons");
            DropTable("dbo.Logbooks");
            DropTable("dbo.Boats");
        }
    }
}
