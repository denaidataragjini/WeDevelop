namespace WeDevelop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kategorit : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Kerkesat", "NrTelefoni");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Kerkesat", "NrTelefoni", c => c.String());
        }
    }
}
