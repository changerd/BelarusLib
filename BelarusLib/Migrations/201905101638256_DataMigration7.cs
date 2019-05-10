namespace BelarusLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photographies", "PhotographyPhoto", c => c.Binary(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photographies", "PhotographyPhoto");
        }
    }
}
