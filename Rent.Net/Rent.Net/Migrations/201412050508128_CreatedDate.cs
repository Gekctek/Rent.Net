namespace Rent.Net.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Requests", "Created", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "Created");
            DropColumn("dbo.Payments", "Created");
        }
    }
}
