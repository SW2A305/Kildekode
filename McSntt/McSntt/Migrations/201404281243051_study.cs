namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class study : DbMigration
    {
        public override void Up()
        {
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
            
            AddColumn("dbo.People", "Team_TeamId", c => c.Int());
            CreateIndex("dbo.People", "Team_TeamId");
            AddForeignKey("dbo.People", "Team_TeamId", "dbo.Teams", "TeamId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.People", "Team_TeamId", "dbo.Teams");
            DropIndex("dbo.People", new[] { "Team_TeamId" });
            DropColumn("dbo.People", "Team_TeamId");
            DropTable("dbo.Teams");
        }
    }
}
