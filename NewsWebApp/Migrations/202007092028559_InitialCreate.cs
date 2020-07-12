namespace NewsWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NewsItem", "ImagePath", c => c.String());
            DropColumn("dbo.NewsItem", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NewsItem", "Image", c => c.String());
            DropColumn("dbo.NewsItem", "ImagePath");
        }
    }
}
