namespace BelarusLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "AuthorShortDescription", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Authors", "AuthorShortDescription");
        }
    }
}
