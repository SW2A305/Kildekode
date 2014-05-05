namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Study2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Level = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TeamId);
            
            AddColumn("dbo.People", "RobeWorks", c => c.Boolean());
            AddColumn("dbo.People", "Navigation", c => c.Boolean());
            AddColumn("dbo.People", "Motor", c => c.Boolean());
            AddColumn("dbo.People", "Drabant", c => c.Boolean());
            AddColumn("dbo.People", "Gaffelrigger", c => c.Boolean());
            AddColumn("dbo.People", "Night", c => c.Boolean());
            AddColumn("dbo.People", "Team_TeamId", c => c.Int());
            CreateIndex("dbo.People", "Team_TeamId");
            AddForeignKey("dbo.People", "Team_TeamId", "dbo.Teams", "TeamId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.People", "Team_TeamId", "dbo.Teams");
            DropIndex("dbo.People", new[] { "Team_TeamId" });
            DropColumn("dbo.People", "Team_TeamId");
            DropColumn("dbo.People", "Night");
            DropColumn("dbo.People", "Gaffelrigger");
            DropColumn("dbo.People", "Drabant");
            DropColumn("dbo.People", "Motor");
            DropColumn("dbo.People", "Navigation");
            DropColumn("dbo.People", "RobeWorks");
            DropTable("dbo.Teams");
        }
    }
}
