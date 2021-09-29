namespace WeDevelop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class useriii : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Kerkesat", "Id_Perdoruesi", c => c.String(maxLength: 128));
            CreateIndex("dbo.Kerkesat", "Id_Perdoruesi");
            AddForeignKey("dbo.Kerkesat", "Id_Perdoruesi", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Kerkesat", "Id_Perdoruesi", "dbo.AspNetUsers");
            DropIndex("dbo.Kerkesat", new[] { "Id_Perdoruesi" });
            AlterColumn("dbo.Kerkesat", "Id_Perdoruesi", c => c.String());
        }
    }
}
