namespace WeDevelop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kerkesattttt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Kerkesat", "Pershkrimi", c => c.String(maxLength: 1000));
            AddColumn("dbo.Kerkesat", "Dokumenti", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Kerkesat", "Dokumenti");
            DropColumn("dbo.Kerkesat", "Pershkrimi");
        }
    }
}
