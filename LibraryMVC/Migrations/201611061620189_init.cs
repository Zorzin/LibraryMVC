namespace LibraryMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Baskets",
                c => new
                    {
                        BasketID = c.Int(nullable: false, identity: true),
                        ReaderID = c.Int(nullable: false),
                        BookID = c.Int(nullable: false),
                        Reader_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.BasketID)
                .ForeignKey("dbo.Books", t => t.BookID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Reader_Id)
                .Index(t => t.BookID)
                .Index(t => t.Reader_Id);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ISBN = c.Int(nullable: false),
                        Year = c.DateTime(nullable: false),
                        Amount = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        Contents = c.String(),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.BookLabels",
                c => new
                    {
                        BookLabelID = c.Int(nullable: false, identity: true),
                        BookID = c.Int(nullable: false),
                        LabelID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookLabelID)
                .ForeignKey("dbo.Books", t => t.BookID, cascadeDelete: true)
                .ForeignKey("dbo.Labels", t => t.LabelID, cascadeDelete: true)
                .Index(t => t.BookID)
                .Index(t => t.LabelID);
            
            CreateTable(
                "dbo.Labels",
                c => new
                    {
                        LabelID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.LabelID);
            
            CreateTable(
                "dbo.BookWriters",
                c => new
                    {
                        BookWriterID = c.Int(nullable: false, identity: true),
                        WriterID = c.Int(nullable: false),
                        BookID = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookWriterID)
                .ForeignKey("dbo.Books", t => t.BookID, cascadeDelete: true)
                .ForeignKey("dbo.Writers", t => t.WriterID, cascadeDelete: true)
                .Index(t => t.WriterID)
                .Index(t => t.BookID);
            
            CreateTable(
                "dbo.Writers",
                c => new
                    {
                        WriterID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                    })
                .PrimaryKey(t => t.WriterID);
            
            CreateTable(
                "dbo.Borrows",
                c => new
                    {
                        BorrowID = c.Int(nullable: false, identity: true),
                        BookID = c.Int(nullable: false),
                        ReaderID = c.Int(nullable: false),
                        BorrowDate = c.DateTime(nullable: false),
                        ReturnDate = c.DateTime(nullable: false),
                        Deadline = c.DateTime(nullable: false),
                        Status = c.String(),
                        Reader_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.BorrowID)
                .ForeignKey("dbo.Books", t => t.BookID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Reader_Id)
                .Index(t => t.BookID)
                .Index(t => t.Reader_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Surname = c.String(),
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
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.SearchHistories",
                c => new
                    {
                        SearchHistoryID = c.Int(nullable: false, identity: true),
                        ReaderID = c.Int(nullable: false),
                        BookID = c.Int(nullable: false),
                        Name = c.String(),
                        Reader_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SearchHistoryID)
                .ForeignKey("dbo.Books", t => t.BookID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Reader_Id)
                .Index(t => t.BookID)
                .Index(t => t.Reader_Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        OverCategoryID = c.Int(nullable: false),
                        OverCategory_CategoryID = c.Int(),
                    })
                .PrimaryKey(t => t.CategoryID)
                .ForeignKey("dbo.Categories", t => t.OverCategory_CategoryID)
                .Index(t => t.OverCategory_CategoryID);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        FileID = c.Int(nullable: false, identity: true),
                        BookID = c.Int(nullable: false),
                        Source = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.FileID)
                .ForeignKey("dbo.Books", t => t.BookID, cascadeDelete: true)
                .Index(t => t.BookID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Files", "BookID", "dbo.Books");
            DropForeignKey("dbo.Categories", "OverCategory_CategoryID", "dbo.Categories");
            DropForeignKey("dbo.Books", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.SearchHistories", "Reader_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SearchHistories", "BookID", "dbo.Books");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Borrows", "Reader_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Baskets", "Reader_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Borrows", "BookID", "dbo.Books");
            DropForeignKey("dbo.BookWriters", "WriterID", "dbo.Writers");
            DropForeignKey("dbo.BookWriters", "BookID", "dbo.Books");
            DropForeignKey("dbo.BookLabels", "LabelID", "dbo.Labels");
            DropForeignKey("dbo.BookLabels", "BookID", "dbo.Books");
            DropForeignKey("dbo.Baskets", "BookID", "dbo.Books");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Files", new[] { "BookID" });
            DropIndex("dbo.Categories", new[] { "OverCategory_CategoryID" });
            DropIndex("dbo.SearchHistories", new[] { "Reader_Id" });
            DropIndex("dbo.SearchHistories", new[] { "BookID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Borrows", new[] { "Reader_Id" });
            DropIndex("dbo.Borrows", new[] { "BookID" });
            DropIndex("dbo.BookWriters", new[] { "BookID" });
            DropIndex("dbo.BookWriters", new[] { "WriterID" });
            DropIndex("dbo.BookLabels", new[] { "LabelID" });
            DropIndex("dbo.BookLabels", new[] { "BookID" });
            DropIndex("dbo.Books", new[] { "CategoryID" });
            DropIndex("dbo.Baskets", new[] { "Reader_Id" });
            DropIndex("dbo.Baskets", new[] { "BookID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Files");
            DropTable("dbo.Categories");
            DropTable("dbo.SearchHistories");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Borrows");
            DropTable("dbo.Writers");
            DropTable("dbo.BookWriters");
            DropTable("dbo.Labels");
            DropTable("dbo.BookLabels");
            DropTable("dbo.Books");
            DropTable("dbo.Baskets");
        }
    }
}
