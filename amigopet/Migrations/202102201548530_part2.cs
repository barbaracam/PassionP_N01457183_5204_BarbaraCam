namespace amigopet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class part2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pets", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pets", "PicExtension");
        }
    }
}
