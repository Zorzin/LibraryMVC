namespace LibraryMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class News : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.News",
                c => new
                    {
                        NewsID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        AddDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.NewsID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.News");
        }
    }
}
