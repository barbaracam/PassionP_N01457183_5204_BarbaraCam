namespace amigopet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_table : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pets", "AppointmentID", "dbo.Appointments");
            DropIndex("dbo.Pets", new[] { "AppointmentID" });
            RenameColumn(table: "dbo.Pets", name: "AppointmentID", newName: "Appointment_AppointmentID");
            AddColumn("dbo.Appointments", "PetID", c => c.Int(nullable: false));
            AlterColumn("dbo.Pets", "Appointment_AppointmentID", c => c.Int());
            CreateIndex("dbo.Appointments", "PetID");
            CreateIndex("dbo.Pets", "Appointment_AppointmentID");
            AddForeignKey("dbo.Appointments", "PetID", "dbo.Pets", "PetID", cascadeDelete: true);
            AddForeignKey("dbo.Pets", "Appointment_AppointmentID", "dbo.Appointments", "AppointmentID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pets", "Appointment_AppointmentID", "dbo.Appointments");
            DropForeignKey("dbo.Appointments", "PetID", "dbo.Pets");
            DropIndex("dbo.Pets", new[] { "Appointment_AppointmentID" });
            DropIndex("dbo.Appointments", new[] { "PetID" });
            AlterColumn("dbo.Pets", "Appointment_AppointmentID", c => c.Int(nullable: false));
            DropColumn("dbo.Appointments", "PetID");
            RenameColumn(table: "dbo.Pets", name: "Appointment_AppointmentID", newName: "AppointmentID");
            CreateIndex("dbo.Pets", "AppointmentID");
            AddForeignKey("dbo.Pets", "AppointmentID", "dbo.Appointments", "AppointmentID", cascadeDelete: true);
        }
    }
}
