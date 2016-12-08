namespace LibraryMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Filesidsearchhistory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SearchHistories", "BookID", "dbo.Books");
            DropIndex("dbo.Borrows", new[] { "Reader_Id" });
            DropIndex("dbo.SearchHistories", new[] { "BookID" });
            DropIndex("dbo.SearchHistories", new[] { "Reader_Id" });
            DropColumn("dbo.Borrows", "ReaderID");
            DropColumn("dbo.SearchHistories", "ReaderID");
            RenameColumn(table: "dbo.SearchHistories", name: "BookID", newName: "Book_BookID");
            RenameColumn(table: "dbo.Borrows", name: "Reader_Id", newName: "ReaderID");
            RenameColumn(table: "dbo.SearchHistories", name: "Reader_Id", newName: "ReaderID");
            AddColumn("dbo.SearchHistories", "URL", c => c.String());
            AlterColumn("dbo.Borrows", "ReaderID", c => c.String(maxLength: 128));
            AlterColumn("dbo.SearchHistories", "ReaderID", c => c.String(maxLength: 128));
            AlterColumn("dbo.SearchHistories", "Book_BookID", c => c.Int());
            CreateIndex("dbo.Borrows", "ReaderID");
            CreateIndex("dbo.SearchHistories", "ReaderID");
            CreateIndex("dbo.SearchHistories", "Book_BookID");
            AddForeignKey("dbo.SearchHistories", "Book_BookID", "dbo.Books", "BookID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SearchHistories", "Book_BookID", "dbo.Books");
            DropIndex("dbo.SearchHistories", new[] { "Book_BookID" });
            DropIndex("dbo.SearchHistories", new[] { "ReaderID" });
            DropIndex("dbo.Borrows", new[] { "ReaderID" });
            AlterColumn("dbo.SearchHistories", "Book_BookID", c => c.Int(nullable: false));
            AlterColumn("dbo.SearchHistories", "ReaderID", c => c.Int(nullable: false));
            AlterColumn("dbo.Borrows", "ReaderID", c => c.Int(nullable: false));
            DropColumn("dbo.SearchHistories", "URL");
            RenameColumn(table: "dbo.SearchHistories", name: "ReaderID", newName: "Reader_Id");
            RenameColumn(table: "dbo.Borrows", name: "ReaderID", newName: "Reader_Id");
            RenameColumn(table: "dbo.SearchHistories", name: "Book_BookID", newName: "BookID");
            AddColumn("dbo.SearchHistories", "ReaderID", c => c.Int(nullable: false));
            AddColumn("dbo.Borrows", "ReaderID", c => c.Int(nullable: false));
            CreateIndex("dbo.SearchHistories", "Reader_Id");
            CreateIndex("dbo.SearchHistories", "BookID");
            CreateIndex("dbo.Borrows", "Reader_Id");
            AddForeignKey("dbo.SearchHistories", "BookID", "dbo.Books", "BookID", cascadeDelete: true);
        }
    }
}
