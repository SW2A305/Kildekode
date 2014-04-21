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
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        NickName = c.String(),
                        ImagePath = c.String(),
                        Operational = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        MemberId = c.Int(),
                        Position = c.Int(),
                        Username = c.String(),
                        PasswordHash = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.PersonId);
            
            CreateTable(
                "dbo.RegularTrips",
                c => new
                    {
                        RegularTripId = c.Int(nullable: false, identity: true),
                        CaptainId = c.Int(nullable: false),
                        ExpectedArrivalTime = c.DateTime(nullable: false),
                        PurposeAndArea = c.String(),
                        SailTripId = c.Int(nullable: false),
                        BoatId = c.Int(nullable: false),
                        DepartureTime = c.DateTime(nullable: false),
                        ArrivalTime = c.DateTime(nullable: false),
                        WeatherConditions = c.String(),
                        Comments = c.String(),
                    })
                .PrimaryKey(t => t.RegularTripId)
                .ForeignKey("dbo.Boats", t => t.BoatId, cascadeDelete: true)
                .ForeignKey("dbo.People", t => t.CaptainId, cascadeDelete: true)
                .Index(t => t.CaptainId)
                .Index(t => t.BoatId);
            
            CreateTable(
                "dbo.PersonRegularTrips",
                c => new
                    {
                        Person_PersonId = c.Int(nullable: false),
                        RegularTrip_RegularTripId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Person_PersonId, t.RegularTrip_RegularTripId })
                .ForeignKey("dbo.People", t => t.Person_PersonId, cascadeDelete: false)
                .ForeignKey("dbo.RegularTrips", t => t.RegularTrip_RegularTripId, cascadeDelete: false)
                .Index(t => t.Person_PersonId)
                .Index(t => t.RegularTrip_RegularTripId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersonRegularTrips", "RegularTrip_RegularTripId", "dbo.RegularTrips");
            DropForeignKey("dbo.PersonRegularTrips", "Person_PersonId", "dbo.People");
            DropForeignKey("dbo.RegularTrips", "CaptainId", "dbo.People");
            DropForeignKey("dbo.RegularTrips", "BoatId", "dbo.Boats");
            DropIndex("dbo.PersonRegularTrips", new[] { "RegularTrip_RegularTripId" });
            DropIndex("dbo.PersonRegularTrips", new[] { "Person_PersonId" });
            DropIndex("dbo.RegularTrips", new[] { "BoatId" });
            DropIndex("dbo.RegularTrips", new[] { "CaptainId" });
            DropTable("dbo.PersonRegularTrips");
            DropTable("dbo.RegularTrips");
            DropTable("dbo.People");
            DropTable("dbo.Boats");
        }
    }
}
