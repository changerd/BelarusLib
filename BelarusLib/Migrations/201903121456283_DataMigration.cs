namespace BelarusLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        AuthorId = c.Guid(nullable: false),
                        AuthorFullName = c.String(),
                        AuthorPhoto = c.Binary(),
                        AuthorBirthDate = c.DateTime(nullable: false),
                        AuthorBiography = c.String(),
                    })
                .PrimaryKey(t => t.AuthorId);
            
            CreateTable(
                "dbo.Compositions",
                c => new
                    {
                        CompositionId = c.Guid(nullable: false),
                        CompositionName = c.String(),
                        CompositionDescription = c.String(),
                        CompositionLink = c.String(),
                        AuthorId = c.Guid(nullable: false),
                        TypeCompositiionId = c.Guid(nullable: false),
                        TypeComposition_TypeCompositionId = c.Guid(),
                    })
                .PrimaryKey(t => t.CompositionId)
                .ForeignKey("dbo.Authors", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.TypeCompositions", t => t.TypeComposition_TypeCompositionId)
                .Index(t => t.AuthorId)
                .Index(t => t.TypeComposition_TypeCompositionId);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        GenreId = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.GenreId);
            
            CreateTable(
                "dbo.TypeCompositions",
                c => new
                    {
                        TypeCompositionId = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.TypeCompositionId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        Birth = c.DateTime(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.GenreCompositions",
                c => new
                    {
                        Genre_GenreId = c.Guid(nullable: false),
                        Composition_CompositionId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Genre_GenreId, t.Composition_CompositionId })
                .ForeignKey("dbo.Genres", t => t.Genre_GenreId, cascadeDelete: true)
                .ForeignKey("dbo.Compositions", t => t.Composition_CompositionId, cascadeDelete: true)
                .Index(t => t.Genre_GenreId)
                .Index(t => t.Composition_CompositionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Compositions", "TypeComposition_TypeCompositionId", "dbo.TypeCompositions");
            DropForeignKey("dbo.GenreCompositions", "Composition_CompositionId", "dbo.Compositions");
            DropForeignKey("dbo.GenreCompositions", "Genre_GenreId", "dbo.Genres");
            DropForeignKey("dbo.Compositions", "AuthorId", "dbo.Authors");
            DropIndex("dbo.GenreCompositions", new[] { "Composition_CompositionId" });
            DropIndex("dbo.GenreCompositions", new[] { "Genre_GenreId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Compositions", new[] { "TypeComposition_TypeCompositionId" });
            DropIndex("dbo.Compositions", new[] { "AuthorId" });
            DropTable("dbo.GenreCompositions");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.TypeCompositions");
            DropTable("dbo.Genres");
            DropTable("dbo.Compositions");
            DropTable("dbo.Authors");
        }
    }
}
