namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Study3 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.People", name: "Team_TeamId", newName: "AssociatedTeam_TeamId");
            RenameIndex(table: "dbo.People", name: "IX_Team_TeamId", newName: "IX_AssociatedTeam_TeamId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.People", name: "IX_AssociatedTeam_TeamId", newName: "IX_Team_TeamId");
            RenameColumn(table: "dbo.People", name: "AssociatedTeam_TeamId", newName: "Team_TeamId");
        }
    }
}
