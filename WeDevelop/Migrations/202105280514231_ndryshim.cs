namespace WeDevelop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ndryshim : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sherbimet", "Imazh", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sherbimet", "Imazh", c => c.Binary());
        }
    }
}
