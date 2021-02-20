namespace amigopet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_alltables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PetWalkers",
                c => new
                    {
                        PetWalkerID = c.Int(nullable: false, identity: true),
                        PetWalkername = c.String(),
                        PetWalkerBio = c.String(),
                    })
                .PrimaryKey(t => t.PetWalkerID);
            
            AddColumn("dbo.Appointments", "PetWalkerID", c => c.Int(nullable: false));
            AddColumn("dbo.Pets", "PetWalker_PetWalkerID", c => c.Int());
            CreateIndex("dbo.Appointments", "PetWalkerID");
            CreateIndex("dbo.Pets", "PetWalker_PetWalkerID");
            AddForeignKey("dbo.Pets", "PetWalker_PetWalkerID", "dbo.PetWalkers", "PetWalkerID");
            AddForeignKey("dbo.Appointments", "PetWalkerID", "dbo.PetWalkers", "PetWalkerID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "PetWalkerID", "dbo.PetWalkers");
            DropForeignKey("dbo.Pets", "PetWalker_PetWalkerID", "dbo.PetWalkers");
            DropIndex("dbo.Pets", new[] { "PetWalker_PetWalkerID" });
            DropIndex("dbo.Appointments", new[] { "PetWalkerID" });
            DropColumn("dbo.Pets", "PetWalker_PetWalkerID");
            DropColumn("dbo.Appointments", "PetWalkerID");
            DropTable("dbo.PetWalkers");
        }
    }
}
