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
                "dbo.SailTrips",
                c => new
                    {
                        SailTripId = c.Int(nullable: false, identity: true),
                        DepartureTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ArrivalTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        WeatherConditions = c.String(),
                        Comments = c.String(),
                        RegularTripId = c.Int(),
                        ExpectedArrivalTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        PurposeAndArea = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Captain_PersonId = c.Int(),
                        Logbook_LogbookId = c.Int(),
                        Boat_BoatId = c.Int(),
                    })
                .PrimaryKey(t => t.SailTripId)
                .ForeignKey("dbo.Persons", t => t.Captain_PersonId)
                .ForeignKey("dbo.Logbooks", t => t.Logbook_LogbookId)
                .ForeignKey("dbo.Boats", t => t.Boat_BoatId)
                .Index(t => t.Captain_PersonId)
                .Index(t => t.Logbook_LogbookId)
                .Index(t => t.Boat_BoatId);
            
            CreateTable(
                "dbo.Logbooks",
                c => new
                    {
                        LogbookId = c.Int(nullable: false, identity: true),
                        ActualDepartureTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ActualArrivalTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
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
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        EventDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EventTitle = c.String(),
                        SignUpReq = c.Boolean(nullable: false),
                        Description = c.String(),
                        Event_EventId = c.Int(),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.Events", t => t.Event_EventId)
                .Index(t => t.Event_EventId);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        RobeWorks = c.Boolean(nullable: false),
                        Navigation = c.Boolean(nullable: false),
                        Motor = c.Boolean(nullable: false),
                        Drabant = c.Boolean(nullable: false),
                        Gaffelrigger = c.Boolean(nullable: false),
                        Night = c.Boolean(nullable: false),
                        Level = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TeamId);
            
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
                        RegularTrip_SailTripId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Person_PersonId, t.RegularTrip_SailTripId })
                .ForeignKey("dbo.Persons", t => t.Person_PersonId, cascadeDelete: true)
                .ForeignKey("dbo.SailTrips", t => t.RegularTrip_SailTripId, cascadeDelete: true)
                .Index(t => t.Person_PersonId)
                .Index(t => t.RegularTrip_SailTripId);
            
            CreateTable(
                "dbo.SailClubMemberTeams",
                c => new
                    {
                        SailClubMember_PersonId = c.Int(nullable: false),
                        Team_TeamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SailClubMember_PersonId, t.Team_TeamId })
                .ForeignKey("dbo.SailClubMembers", t => t.SailClubMember_PersonId, cascadeDelete: true)
                .ForeignKey("dbo.Teams", t => t.Team_TeamId, cascadeDelete: true)
                .Index(t => t.SailClubMember_PersonId)
                .Index(t => t.Team_TeamId);
            
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
            DropForeignKey("dbo.SailTrips", "Boat_BoatId", "dbo.Boats");
            DropForeignKey("dbo.SailTrips", "Logbook_LogbookId", "dbo.Logbooks");
            DropForeignKey("dbo.SailClubMemberTeams", "Team_TeamId", "dbo.Teams");
            DropForeignKey("dbo.SailClubMemberTeams", "SailClubMember_PersonId", "dbo.SailClubMembers");
            DropForeignKey("dbo.Logbooks", "FiledBy_PersonId", "dbo.SailClubMembers");
            DropForeignKey("dbo.PersonRegularTrips", "RegularTrip_SailTripId", "dbo.SailTrips");
            DropForeignKey("dbo.PersonRegularTrips", "Person_PersonId", "dbo.Persons");
            DropForeignKey("dbo.PersonLogbooks", "Logbook_LogbookId", "dbo.Logbooks");
            DropForeignKey("dbo.PersonLogbooks", "Person_PersonId", "dbo.Persons");
            DropForeignKey("dbo.PersonEvents", "Event_EventId", "dbo.Events");
            DropForeignKey("dbo.PersonEvents", "Person_PersonId", "dbo.Persons");
            DropForeignKey("dbo.Events", "Event_EventId", "dbo.Events");
            DropForeignKey("dbo.SailTrips", "Captain_PersonId", "dbo.Persons");
            DropIndex("dbo.SailClubMembers", new[] { "SailClubMemberId" });
            DropIndex("dbo.SailClubMembers", new[] { "PersonId" });
            DropIndex("dbo.SailClubMemberTeams", new[] { "Team_TeamId" });
            DropIndex("dbo.SailClubMemberTeams", new[] { "SailClubMember_PersonId" });
            DropIndex("dbo.PersonRegularTrips", new[] { "RegularTrip_SailTripId" });
            DropIndex("dbo.PersonRegularTrips", new[] { "Person_PersonId" });
            DropIndex("dbo.PersonLogbooks", new[] { "Logbook_LogbookId" });
            DropIndex("dbo.PersonLogbooks", new[] { "Person_PersonId" });
            DropIndex("dbo.PersonEvents", new[] { "Event_EventId" });
            DropIndex("dbo.PersonEvents", new[] { "Person_PersonId" });
            DropIndex("dbo.Events", new[] { "Event_EventId" });
            DropIndex("dbo.Logbooks", new[] { "FiledBy_PersonId" });
            DropIndex("dbo.SailTrips", new[] { "Boat_BoatId" });
            DropIndex("dbo.SailTrips", new[] { "Logbook_LogbookId" });
            DropIndex("dbo.SailTrips", new[] { "Captain_PersonId" });
            DropTable("dbo.SailClubMembers");
            DropTable("dbo.SailClubMemberTeams");
            DropTable("dbo.PersonRegularTrips");
            DropTable("dbo.PersonLogbooks");
            DropTable("dbo.PersonEvents");
            DropTable("dbo.Teams");
            DropTable("dbo.Events");
            DropTable("dbo.Persons");
            DropTable("dbo.Logbooks");
            DropTable("dbo.SailTrips");
            DropTable("dbo.Boats");
        }
    }
}
