namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudyTeacherAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teams", "Teacher_PersonId", c => c.Int());
            CreateIndex("dbo.Teams", "Teacher_PersonId");
            AddForeignKey("dbo.Teams", "Teacher_PersonId", "dbo.SailClubMembers", "PersonId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Teams", "Teacher_PersonId", "dbo.SailClubMembers");
            DropIndex("dbo.Teams", new[] { "Teacher_PersonId" });
            DropColumn("dbo.Teams", "Teacher_PersonId");
        }
    }
}
