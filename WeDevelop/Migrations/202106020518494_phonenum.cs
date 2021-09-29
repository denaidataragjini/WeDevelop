namespace WeDevelop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class phonenum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Kerkesat", "NrTelefoni", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Kerkesat", "NrTelefoni");
        }
    }
}
