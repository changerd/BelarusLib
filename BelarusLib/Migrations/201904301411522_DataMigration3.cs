namespace BelarusLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FactAuthors", "Fact_FactId", "dbo.Facts");
            DropForeignKey("dbo.FactAuthors", "Author_AuthorId", "dbo.Authors");
            DropIndex("dbo.FactAuthors", new[] { "Fact_FactId" });
            DropIndex("dbo.FactAuthors", new[] { "Author_AuthorId" });
            AddColumn("dbo.Facts", "AuthorId", c => c.Int(nullable: false));
            AlterColumn("dbo.Facts", "FactText", c => c.String(nullable: false));
            CreateIndex("dbo.Facts", "AuthorId");
            AddForeignKey("dbo.Facts", "AuthorId", "dbo.Authors", "AuthorId", cascadeDelete: true);
            DropTable("dbo.FactAuthors");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FactAuthors",
                c => new
                    {
                        Fact_FactId = c.Int(nullable: false),
                        Author_AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Fact_FactId, t.Author_AuthorId });
            
            DropForeignKey("dbo.Facts", "AuthorId", "dbo.Authors");
            DropIndex("dbo.Facts", new[] { "AuthorId" });
            AlterColumn("dbo.Facts", "FactText", c => c.String());
            DropColumn("dbo.Facts", "AuthorId");
            CreateIndex("dbo.FactAuthors", "Author_AuthorId");
            CreateIndex("dbo.FactAuthors", "Fact_FactId");
            AddForeignKey("dbo.FactAuthors", "Author_AuthorId", "dbo.Authors", "AuthorId", cascadeDelete: true);
            AddForeignKey("dbo.FactAuthors", "Fact_FactId", "dbo.Facts", "FactId", cascadeDelete: true);
        }
    }
}
