namespace WeDevelop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kerkesere : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Kerkesat", "Sherbimet_Id", "dbo.Sherbimet");
            DropIndex("dbo.Kerkesat", new[] { "Sherbimet_Id" });
            DropColumn("dbo.Kerkesat", "Id_Sherbimi");
            RenameColumn(table: "dbo.Kerkesat", name: "Sherbimet_Id", newName: "Id_Sherbimi");
            AlterColumn("dbo.Kerkesat", "Id_Sherbimi", c => c.Int(nullable: false));
            CreateIndex("dbo.Kerkesat", "Id_Sherbimi");
            AddForeignKey("dbo.Kerkesat", "Id_Sherbimi", "dbo.Sherbimet", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Kerkesat", "Id_Sherbimi", "dbo.Sherbimet");
            DropIndex("dbo.Kerkesat", new[] { "Id_Sherbimi" });
            AlterColumn("dbo.Kerkesat", "Id_Sherbimi", c => c.Int());
            RenameColumn(table: "dbo.Kerkesat", name: "Id_Sherbimi", newName: "Sherbimet_Id");
            AddColumn("dbo.Kerkesat", "Id_Sherbimi", c => c.Int(nullable: false));
            CreateIndex("dbo.Kerkesat", "Sherbimet_Id");
            AddForeignKey("dbo.Kerkesat", "Sherbimet_Id", "dbo.Sherbimet", "Id");
        }
    }
}
