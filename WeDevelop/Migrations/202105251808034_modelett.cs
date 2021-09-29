namespace WeDevelop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelett : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Kerkesat",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Id_Sherbimi = c.Int(nullable: false),
                        Id_Perdoruesi = c.String(),
                        Pranuar = c.Boolean(nullable: false),
                        Progresi = c.String(),
                        Sherbimet_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sherbimet", t => t.Sherbimet_Id)
                .Index(t => t.Sherbimet_Id);
            
            CreateTable(
                "dbo.Sherbimet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Emri = c.String(),
                        Pershkrimi = c.String(maxLength: 200),
                        Imazh = c.String(),
                        Cmimi = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Shportat",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Id_Sherbimi = c.Int(nullable: false),
                        Id_Klienti = c.String(),
                        Sherbimet_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sherbimet", t => t.Sherbimet_Id)
                .Index(t => t.Sherbimet_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shportat", "Sherbimet_Id", "dbo.Sherbimet");
            DropForeignKey("dbo.Kerkesat", "Sherbimet_Id", "dbo.Sherbimet");
            DropIndex("dbo.Shportat", new[] { "Sherbimet_Id" });
            DropIndex("dbo.Kerkesat", new[] { "Sherbimet_Id" });
            DropTable("dbo.Shportat");
            DropTable("dbo.Sherbimet");
            DropTable("dbo.Kerkesat");
        }
    }
}
