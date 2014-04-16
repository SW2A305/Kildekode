namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBoats : DbMigration
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
                        Owner_PersonId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Owner_PersonId)
                .Index(t => t.Owner_PersonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Boats", "Owner_PersonId", "dbo.People");
            DropIndex("dbo.Boats", new[] { "Owner_PersonId" });
            DropTable("dbo.Boats");
        }
    }
}
