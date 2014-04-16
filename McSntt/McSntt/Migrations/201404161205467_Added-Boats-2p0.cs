namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBoats2p0 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Boats", "Owner_PersonId", "dbo.People");
            DropIndex("dbo.Boats", new[] { "Owner_PersonId" });
            AddColumn("dbo.Boats", "Operational", c => c.Boolean(nullable: false));
            DropColumn("dbo.Boats", "Owner_PersonId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Boats", "Owner_PersonId", c => c.Int());
            DropColumn("dbo.Boats", "Operational");
            CreateIndex("dbo.Boats", "Owner_PersonId");
            AddForeignKey("dbo.Boats", "Owner_PersonId", "dbo.People", "PersonId");
        }
    }
}
