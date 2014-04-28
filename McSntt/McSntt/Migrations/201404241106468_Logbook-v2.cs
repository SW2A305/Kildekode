namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Logbookv2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Logbooks", "BoatId", "dbo.Boats");
            DropIndex("dbo.Logbooks", new[] { "BoatId" });
            AddColumn("dbo.Logbooks", "DamageInflicted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Logbooks", "DamageDescription", c => c.String());
            AddColumn("dbo.Logbooks", "AnswerFromBoatChief", c => c.String());
            AddColumn("dbo.Logbooks", "FiledBy_PersonId", c => c.Int());
            AddColumn("dbo.People", "Logbook_Id", c => c.Int());
            AddColumn("dbo.RegularTrips", "Logbook_Id", c => c.Int());
            CreateIndex("dbo.Logbooks", "FiledBy_PersonId");
            CreateIndex("dbo.People", "Logbook_Id");
            CreateIndex("dbo.RegularTrips", "Logbook_Id");
            AddForeignKey("dbo.RegularTrips", "Logbook_Id", "dbo.Logbooks", "Id");
            AddForeignKey("dbo.People", "Logbook_Id", "dbo.Logbooks", "Id");
            AddForeignKey("dbo.Logbooks", "FiledBy_PersonId", "dbo.People", "PersonId");
            DropColumn("dbo.Logbooks", "DamageReport");
            DropColumn("dbo.Logbooks", "SailTripId");
            DropColumn("dbo.Logbooks", "BoatId");
            DropColumn("dbo.Logbooks", "DepartureTime");
            DropColumn("dbo.Logbooks", "ArrivalTime");
            DropColumn("dbo.Logbooks", "WeatherConditions");
            DropColumn("dbo.Logbooks", "Comments");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Logbooks", "Comments", c => c.String());
            AddColumn("dbo.Logbooks", "WeatherConditions", c => c.String());
            AddColumn("dbo.Logbooks", "ArrivalTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Logbooks", "DepartureTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Logbooks", "BoatId", c => c.Int(nullable: false));
            AddColumn("dbo.Logbooks", "SailTripId", c => c.Int(nullable: false));
            AddColumn("dbo.Logbooks", "DamageReport", c => c.String());
            DropForeignKey("dbo.Logbooks", "FiledBy_PersonId", "dbo.People");
            DropForeignKey("dbo.People", "Logbook_Id", "dbo.Logbooks");
            DropForeignKey("dbo.RegularTrips", "Logbook_Id", "dbo.Logbooks");
            DropIndex("dbo.RegularTrips", new[] { "Logbook_Id" });
            DropIndex("dbo.People", new[] { "Logbook_Id" });
            DropIndex("dbo.Logbooks", new[] { "FiledBy_PersonId" });
            DropColumn("dbo.RegularTrips", "Logbook_Id");
            DropColumn("dbo.People", "Logbook_Id");
            DropColumn("dbo.Logbooks", "FiledBy_PersonId");
            DropColumn("dbo.Logbooks", "AnswerFromBoatChief");
            DropColumn("dbo.Logbooks", "DamageDescription");
            DropColumn("dbo.Logbooks", "DamageInflicted");
            CreateIndex("dbo.Logbooks", "BoatId");
            AddForeignKey("dbo.Logbooks", "BoatId", "dbo.Boats", "Id", cascadeDelete: true);
        }
    }
}
