namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
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
                        SignUpMsg = c.String(),
                        Created = c.Boolean(nullable: false),
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
                        Level = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TeamId);
            
            CreateTable(
                "dbo.Lectures",
                c => new
                    {
                        LectureId = c.Int(nullable: false, identity: true),
                        DateOfLecture = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        RopeWorksLecture = c.Boolean(nullable: false),
                        Navigation = c.Boolean(nullable: false),
                        Motor = c.Boolean(nullable: false),
                        Drabant = c.Boolean(nullable: false),
                        Gaffelrigger = c.Boolean(nullable: false),
                        Night = c.Boolean(nullable: false),
                        Team_TeamId = c.Int(),
                    })
                .PrimaryKey(t => t.LectureId)
                .ForeignKey("dbo.Teams", t => t.Team_TeamId)
                .Index(t => t.Team_TeamId);
            
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
            
            CreateTable(
                "dbo.StudentMembers",
                c => new
                    {
                        PersonId = c.Int(nullable: false),
                        Lecture_LectureId = c.Int(),
                        AssociatedTeam_TeamId = c.Int(),
                        RopeWorks = c.Boolean(nullable: false),
                        Navigation = c.Boolean(nullable: false),
                        Motor = c.Boolean(nullable: false),
                        Drabant = c.Boolean(nullable: false),
                        Gaffelrigger = c.Boolean(nullable: false),
                        Night = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PersonId)
                .ForeignKey("dbo.SailClubMembers", t => t.PersonId)
                .ForeignKey("dbo.Lectures", t => t.Lecture_LectureId)
                .ForeignKey("dbo.Teams", t => t.AssociatedTeam_TeamId)
                .Index(t => t.PersonId)
                .Index(t => t.Lecture_LectureId)
                .Index(t => t.AssociatedTeam_TeamId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentMembers", "AssociatedTeam_TeamId", "dbo.Teams");
            DropForeignKey("dbo.StudentMembers", "Lecture_LectureId", "dbo.Lectures");
            DropForeignKey("dbo.StudentMembers", "PersonId", "dbo.SailClubMembers");
            DropForeignKey("dbo.SailClubMembers", "PersonId", "dbo.Persons");
            DropForeignKey("dbo.SailTrips", "Boat_BoatId", "dbo.Boats");
            DropForeignKey("dbo.SailTrips", "Logbook_LogbookId", "dbo.Logbooks");
            DropForeignKey("dbo.Lectures", "Team_TeamId", "dbo.Teams");
            DropForeignKey("dbo.Logbooks", "FiledBy_PersonId", "dbo.SailClubMembers");
            DropForeignKey("dbo.PersonRegularTrips", "RegularTrip_SailTripId", "dbo.SailTrips");
            DropForeignKey("dbo.PersonRegularTrips", "Person_PersonId", "dbo.Persons");
            DropForeignKey("dbo.PersonLogbooks", "Logbook_LogbookId", "dbo.Logbooks");
            DropForeignKey("dbo.PersonLogbooks", "Person_PersonId", "dbo.Persons");
            DropForeignKey("dbo.PersonEvents", "Event_EventId", "dbo.Events");
            DropForeignKey("dbo.PersonEvents", "Person_PersonId", "dbo.Persons");
            DropForeignKey("dbo.Events", "Event_EventId", "dbo.Events");
            DropForeignKey("dbo.SailTrips", "Captain_PersonId", "dbo.Persons");
            DropIndex("dbo.StudentMembers", new[] { "AssociatedTeam_TeamId" });
            DropIndex("dbo.StudentMembers", new[] { "Lecture_LectureId" });
            DropIndex("dbo.StudentMembers", new[] { "PersonId" });
            DropIndex("dbo.SailClubMembers", new[] { "SailClubMemberId" });
            DropIndex("dbo.SailClubMembers", new[] { "PersonId" });
            DropIndex("dbo.PersonRegularTrips", new[] { "RegularTrip_SailTripId" });
            DropIndex("dbo.PersonRegularTrips", new[] { "Person_PersonId" });
            DropIndex("dbo.PersonLogbooks", new[] { "Logbook_LogbookId" });
            DropIndex("dbo.PersonLogbooks", new[] { "Person_PersonId" });
            DropIndex("dbo.PersonEvents", new[] { "Event_EventId" });
            DropIndex("dbo.PersonEvents", new[] { "Person_PersonId" });
            DropIndex("dbo.Lectures", new[] { "Team_TeamId" });
            DropIndex("dbo.Events", new[] { "Event_EventId" });
            DropIndex("dbo.Logbooks", new[] { "FiledBy_PersonId" });
            DropIndex("dbo.SailTrips", new[] { "Boat_BoatId" });
            DropIndex("dbo.SailTrips", new[] { "Logbook_LogbookId" });
            DropIndex("dbo.SailTrips", new[] { "Captain_PersonId" });
            DropTable("dbo.StudentMembers");
            DropTable("dbo.SailClubMembers");
            DropTable("dbo.PersonRegularTrips");
            DropTable("dbo.PersonLogbooks");
            DropTable("dbo.PersonEvents");
            DropTable("dbo.Lectures");
            DropTable("dbo.Teams");
            DropTable("dbo.Events");
            DropTable("dbo.Persons");
            DropTable("dbo.Logbooks");
            DropTable("dbo.SailTrips");
            DropTable("dbo.Boats");
        }
    }
}
