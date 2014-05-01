namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
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
                .ForeignKey("dbo.People", t => t.FiledBy_PersonId)
                .Index(t => t.FiledBy_PersonId);
            
            CreateTable(
                "dbo.People",
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
                        SailClubMemberId = c.Int(),
                        Position = c.Int(),
                        Username = c.String(),
                        PasswordHash = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Logbook_LogbookId = c.Int(),
                    })
                .PrimaryKey(t => t.PersonId)
                .ForeignKey("dbo.Logbooks", t => t.Logbook_LogbookId)
                .Index(t => t.Logbook_LogbookId);
            
            CreateTable(
                "dbo.RegularTrips",
                c => new
                    {
                        RegularTripId = c.Int(nullable: false, identity: true),
                        ExpectedArrivalTime = c.DateTime(nullable: false),
                        PurposeAndArea = c.String(),
                        SailTripId = c.Int(nullable: false),
                        DepartureTime = c.DateTime(nullable: false),
                        ArrivalTime = c.DateTime(nullable: false),
                        WeatherConditions = c.String(),
                        Comments = c.String(),
                        Boat_BoatId = c.Int(),
                        Logbook_LogbookId = c.Int(),
                        Captain_PersonId = c.Int(),
                    })
                .PrimaryKey(t => t.RegularTripId)
                .ForeignKey("dbo.Boats", t => t.Boat_BoatId)
                .ForeignKey("dbo.Logbooks", t => t.Logbook_LogbookId)
                .ForeignKey("dbo.People", t => t.Captain_PersonId)
                .Index(t => t.Boat_BoatId)
                .Index(t => t.Logbook_LogbookId)
                .Index(t => t.Captain_PersonId);
            
            CreateTable(
                "dbo.PersonRegularTrips",
                c => new
                    {
                        Person_PersonId = c.Int(nullable: false),
                        RegularTrip_RegularTripId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Person_PersonId, t.RegularTrip_RegularTripId })
                .ForeignKey("dbo.People", t => t.Person_PersonId, cascadeDelete: true)
                .ForeignKey("dbo.RegularTrips", t => t.RegularTrip_RegularTripId, cascadeDelete: true)
                .Index(t => t.Person_PersonId)
                .Index(t => t.RegularTrip_RegularTripId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Logbooks", "FiledBy_PersonId", "dbo.People");
            DropForeignKey("dbo.People", "Logbook_LogbookId", "dbo.Logbooks");
            DropForeignKey("dbo.PersonRegularTrips", "RegularTrip_RegularTripId", "dbo.RegularTrips");
            DropForeignKey("dbo.PersonRegularTrips", "Person_PersonId", "dbo.People");
            DropForeignKey("dbo.RegularTrips", "Captain_PersonId", "dbo.People");
            DropForeignKey("dbo.RegularTrips", "Logbook_LogbookId", "dbo.Logbooks");
            DropForeignKey("dbo.RegularTrips", "Boat_BoatId", "dbo.Boats");
            DropIndex("dbo.PersonRegularTrips", new[] { "RegularTrip_RegularTripId" });
            DropIndex("dbo.PersonRegularTrips", new[] { "Person_PersonId" });
            DropIndex("dbo.RegularTrips", new[] { "Captain_PersonId" });
            DropIndex("dbo.RegularTrips", new[] { "Logbook_LogbookId" });
            DropIndex("dbo.RegularTrips", new[] { "Boat_BoatId" });
            DropIndex("dbo.People", new[] { "Logbook_LogbookId" });
            DropIndex("dbo.Logbooks", new[] { "FiledBy_PersonId" });
            DropTable("dbo.PersonRegularTrips");
            DropTable("dbo.RegularTrips");
            DropTable("dbo.People");
            DropTable("dbo.Logbooks");
            DropTable("dbo.Boats");
        }
    }
}
