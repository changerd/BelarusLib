namespace BelarusLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Photographies",
                c => new
                    {
                        PhotographyId = c.Int(nullable: false, identity: true),
                        PhotographyPhoto = c.String(nullable: false),
                        AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PhotographyId)
                .ForeignKey("dbo.Authors", t => t.AuthorId, cascadeDelete: true)
                .Index(t => t.AuthorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Photographies", "AuthorId", "dbo.Authors");
            DropIndex("dbo.Photographies", new[] { "AuthorId" });
            DropTable("dbo.Photographies");
        }
    }
}
