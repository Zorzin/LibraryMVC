namespace LibraryMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updatebook : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Books", "Year", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Books", "Year", c => c.DateTime(nullable: false));
        }
    }
}
