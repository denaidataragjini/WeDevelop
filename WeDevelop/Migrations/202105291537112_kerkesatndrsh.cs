namespace WeDevelop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kerkesatndrsh : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Kerkesat", "DataEFillimit", c => c.DateTime(nullable: false));
            AddColumn("dbo.Kerkesat", "DataEMbarimit", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Kerkesat", "DataEMbarimit");
            DropColumn("dbo.Kerkesat", "DataEFillimit");
        }
    }
}
