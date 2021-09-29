namespace WeDevelop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatimkat : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sherbimet", "Kategoria_Id", "dbo.Kategoria");
            DropIndex("dbo.Sherbimet", new[] { "Kategoria_Id" });
            AddColumn("dbo.Sherbimet", "KategoriId", c => c.Int(nullable: false));
            AddColumn("dbo.Kategoria", "Sherbime_Id", c => c.Int());
            CreateIndex("dbo.Kategoria", "Sherbime_Id");
            AddForeignKey("dbo.Kategoria", "Sherbime_Id", "dbo.Sherbimet", "Id");
            DropColumn("dbo.Sherbimet", "Kategoria_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sherbimet", "Kategoria_Id", c => c.Int());
            DropForeignKey("dbo.Kategoria", "Sherbime_Id", "dbo.Sherbimet");
            DropIndex("dbo.Kategoria", new[] { "Sherbime_Id" });
            DropColumn("dbo.Kategoria", "Sherbime_Id");
            DropColumn("dbo.Sherbimet", "KategoriId");
            CreateIndex("dbo.Sherbimet", "Kategoria_Id");
            AddForeignKey("dbo.Sherbimet", "Kategoria_Id", "dbo.Kategoria", "Id");
        }
    }
}
