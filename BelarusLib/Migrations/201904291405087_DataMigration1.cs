namespace BelarusLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Audios",
                c => new
                    {
                        AudioId = c.Int(nullable: false, identity: true),
                        AudioName = c.String(nullable: false),
                        AudioLink = c.String(nullable: false),
                        AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AudioId)
                .ForeignKey("dbo.Authors", t => t.AuthorId, cascadeDelete: true)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.Videos",
                c => new
                    {
                        VideoId = c.Int(nullable: false, identity: true),
                        VideoName = c.String(nullable: false),
                        VideoLink = c.String(nullable: false),
                        AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VideoId)
                .ForeignKey("dbo.Authors", t => t.AuthorId, cascadeDelete: true)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.Choices",
                c => new
                    {
                        ChoiceId = c.Int(nullable: false, identity: true),
                        ChoiceText = c.String(nullable: false),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChoiceId)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QuestionId = c.Int(nullable: false, identity: true),
                        QuestionText = c.String(nullable: false),
                        QuestionAnswer = c.String(),
                        QuestionDescription = c.String(),
                        QuestionImage = c.Binary(),
                        QuizId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QuestionId)
                .ForeignKey("dbo.Quizs", t => t.QuizId, cascadeDelete: true)
                .Index(t => t.QuizId);
            
            CreateTable(
                "dbo.Quizs",
                c => new
                    {
                        QuizId = c.Int(nullable: false, identity: true),
                        QuizName = c.String(nullable: false),
                        QuizDuration = c.Time(nullable: false, precision: 7),
                        QuizDescription = c.String(),
                        QuizIsPrivate = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.QuizId);
            
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        ResultId = c.Int(nullable: false, identity: true),
                        ResultScore = c.Int(nullable: false),
                        ResultDate = c.DateTime(nullable: false),
                        QuizId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ResultId)
                .ForeignKey("dbo.Quizs", t => t.QuizId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.QuizId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Results", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Results", "QuizId", "dbo.Quizs");
            DropForeignKey("dbo.Questions", "QuizId", "dbo.Quizs");
            DropForeignKey("dbo.Choices", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Videos", "AuthorId", "dbo.Authors");
            DropForeignKey("dbo.Audios", "AuthorId", "dbo.Authors");
            DropIndex("dbo.Results", new[] { "UserId" });
            DropIndex("dbo.Results", new[] { "QuizId" });
            DropIndex("dbo.Questions", new[] { "QuizId" });
            DropIndex("dbo.Choices", new[] { "QuestionId" });
            DropIndex("dbo.Videos", new[] { "AuthorId" });
            DropIndex("dbo.Audios", new[] { "AuthorId" });
            DropTable("dbo.Results");
            DropTable("dbo.Quizs");
            DropTable("dbo.Questions");
            DropTable("dbo.Choices");
            DropTable("dbo.Videos");
            DropTable("dbo.Audios");
        }
    }
}
