namespace WeDevelop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ndrshshernim : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sherbimet", "Ditet", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sherbimet", "Ditet");
        }
    }
}
