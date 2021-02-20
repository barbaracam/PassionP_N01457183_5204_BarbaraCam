namespace amigopet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_appointmenttable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentID = c.Int(nullable: false, identity: true),
                        AppointmentDay = c.DateTime(nullable: false),
                        AppointmentNote = c.String(),
                    })
                .PrimaryKey(t => t.AppointmentID);
            
            AddColumn("dbo.Pets", "AppointmentID", c => c.Int(nullable: false));
            CreateIndex("dbo.Pets", "AppointmentID");
            AddForeignKey("dbo.Pets", "AppointmentID", "dbo.Appointments", "AppointmentID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pets", "AppointmentID", "dbo.Appointments");
            DropIndex("dbo.Pets", new[] { "AppointmentID" });
            DropColumn("dbo.Pets", "AppointmentID");
            DropTable("dbo.Appointments");
        }
    }
}
