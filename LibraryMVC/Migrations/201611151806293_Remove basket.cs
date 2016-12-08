namespace LibraryMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removebasket : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Baskets", "BookID", "dbo.Books");
            DropForeignKey("dbo.Baskets", "Reader_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Baskets", new[] { "BookID" });
            DropIndex("dbo.Baskets", new[] { "Reader_Id" });
            DropTable("dbo.Baskets");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.BasketID);
            
            CreateIndex("dbo.Baskets", "Reader_Id");
            CreateIndex("dbo.Baskets", "BookID");
            AddForeignKey("dbo.Baskets", "Reader_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Baskets", "BookID", "dbo.Books", "BookID", cascadeDelete: true);
        }
    }
}
