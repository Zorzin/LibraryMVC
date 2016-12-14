namespace LibraryMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinalSBD : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categories", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Writers", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Writers", "Surname", c => c.String(nullable: false));
            AlterColumn("dbo.Labels", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.News", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.News", "Content", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.News", "Content", c => c.String());
            AlterColumn("dbo.News", "Title", c => c.String());
            AlterColumn("dbo.Labels", "Name", c => c.String());
            AlterColumn("dbo.Writers", "Surname", c => c.String());
            AlterColumn("dbo.Writers", "Name", c => c.String());
            AlterColumn("dbo.Categories", "Name", c => c.String());
        }
    }
}
