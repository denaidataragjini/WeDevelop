namespace WeDevelop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Kerkesat", "Seen", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Kerkesat", "Seen");
        }
    }
}
