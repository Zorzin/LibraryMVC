namespace LibraryMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LibraryValues : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LibraryValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MaxBorrows = c.Int(nullable: false),
                        MaxBorrowTime = c.Int(nullable: false),
                        TimeToCollectBook = c.Int(nullable: false),
                        MaxBooksToCollect = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LibraryValues");
        }
    }
}
