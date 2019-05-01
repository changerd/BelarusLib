namespace BelarusLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Facts",
                c => new
                    {
                        FactId = c.Int(nullable: false, identity: true),
                        FactText = c.String(),
                    })
                .PrimaryKey(t => t.FactId);
            
            CreateTable(
                "dbo.FactAuthors",
                c => new
                    {
                        Fact_FactId = c.Int(nullable: false),
                        Author_AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Fact_FactId, t.Author_AuthorId })
                .ForeignKey("dbo.Facts", t => t.Fact_FactId, cascadeDelete: true)
                .ForeignKey("dbo.Authors", t => t.Author_AuthorId, cascadeDelete: true)
                .Index(t => t.Fact_FactId)
                .Index(t => t.Author_AuthorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FactAuthors", "Author_AuthorId", "dbo.Authors");
            DropForeignKey("dbo.FactAuthors", "Fact_FactId", "dbo.Facts");
            DropIndex("dbo.FactAuthors", new[] { "Author_AuthorId" });
            DropIndex("dbo.FactAuthors", new[] { "Fact_FactId" });
            DropTable("dbo.FactAuthors");
            DropTable("dbo.Facts");
        }
    }
}
