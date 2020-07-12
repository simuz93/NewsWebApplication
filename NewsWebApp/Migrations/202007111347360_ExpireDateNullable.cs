namespace NewsWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExpireDateNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NewsItem", "ExpiringDatetime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NewsItem", "ExpiringDatetime", c => c.DateTime(nullable: false));
        }
    }
}
