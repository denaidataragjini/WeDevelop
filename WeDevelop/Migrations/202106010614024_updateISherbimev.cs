namespace WeDevelop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateISherbimev : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sherbimet", "CfareKerkojme", c => c.String());
            AddColumn("dbo.Sherbimet", "CfareJapim", c => c.String());
            AlterColumn("dbo.Sherbimet", "Pershkrimi", c => c.String(maxLength: 400));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sherbimet", "Pershkrimi", c => c.String(maxLength: 200));
            DropColumn("dbo.Sherbimet", "CfareJapim");
            DropColumn("dbo.Sherbimet", "CfareKerkojme");
        }
    }
}
