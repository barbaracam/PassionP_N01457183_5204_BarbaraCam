namespace amigopet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_petcontrollers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "AppointmentTime", c => c.String());
            DropColumn("dbo.Appointments", "AppointmentDay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appointments", "AppointmentDay", c => c.DateTime(nullable: false));
            DropColumn("dbo.Appointments", "AppointmentTime");
        }
    }
}
