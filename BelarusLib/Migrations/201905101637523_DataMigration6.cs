namespace BelarusLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration6 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Photographies", "PhotographyPhoto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Photographies", "PhotographyPhoto", c => c.String(nullable: false));
        }
    }
}
