namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Study4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lectures",
                c => new
                    {
                        LectureId = c.Int(nullable: false, identity: true),
                        DateOfLecture = c.DateTime(nullable: false),
                        RopeWorks = c.Boolean(nullable: false),
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
            
            AddColumn("dbo.People", "Lecture_LectureId", c => c.Int());
            CreateIndex("dbo.People", "Lecture_LectureId");
            AddForeignKey("dbo.People", "Lecture_LectureId", "dbo.Lectures", "LectureId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lectures", "Team_TeamId", "dbo.Teams");
            DropForeignKey("dbo.People", "Lecture_LectureId", "dbo.Lectures");
            DropIndex("dbo.Lectures", new[] { "Team_TeamId" });
            DropIndex("dbo.People", new[] { "Lecture_LectureId" });
            DropColumn("dbo.People", "Lecture_LectureId");
            DropTable("dbo.Lectures");
        }
    }
}
