namespace BelarusLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Compositions", "TypeComposition_TypeCompositionId", "dbo.TypeCompositions");
            DropIndex("dbo.Compositions", new[] { "TypeComposition_TypeCompositionId" });
            RenameColumn(table: "dbo.Compositions", name: "TypeComposition_TypeCompositionId", newName: "TypeCompositionId");
            AlterColumn("dbo.Compositions", "TypeCompositionId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Compositions", "TypeCompositionId");
            AddForeignKey("dbo.Compositions", "TypeCompositionId", "dbo.TypeCompositions", "TypeCompositionId", cascadeDelete: true);
            DropColumn("dbo.Compositions", "TypeCompositiionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Compositions", "TypeCompositiionId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.Compositions", "TypeCompositionId", "dbo.TypeCompositions");
            DropIndex("dbo.Compositions", new[] { "TypeCompositionId" });
            AlterColumn("dbo.Compositions", "TypeCompositionId", c => c.Guid());
            RenameColumn(table: "dbo.Compositions", name: "TypeCompositionId", newName: "TypeComposition_TypeCompositionId");
            CreateIndex("dbo.Compositions", "TypeComposition_TypeCompositionId");
            AddForeignKey("dbo.Compositions", "TypeComposition_TypeCompositionId", "dbo.TypeCompositions", "TypeCompositionId");
        }
    }
}
