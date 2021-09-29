namespace WeDevelop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hiqserbim : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Kategoria", "sherbimId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Kategoria", "sherbimId", c => c.Int(nullable: false));
        }
    }
}
