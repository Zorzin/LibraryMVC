namespace LibraryMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class searchhistoryremovebook : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SearchHistories", "Book_BookID", "dbo.Books");
            DropIndex("dbo.SearchHistories", new[] { "Book_BookID" });
            DropColumn("dbo.SearchHistories", "Book_BookID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SearchHistories", "Book_BookID", c => c.Int());
            CreateIndex("dbo.SearchHistories", "Book_BookID");
            AddForeignKey("dbo.SearchHistories", "Book_BookID", "dbo.Books", "BookID");
        }
    }
}
