namespace WeDevelop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class karta : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Karta",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        sherbimId = c.Int(nullable: false),
                        userId = c.String(),
                        Sherbime_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sherbimet", t => t.Sherbime_Id)
                .Index(t => t.Sherbime_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Karta", "Sherbime_Id", "dbo.Sherbimet");
            DropIndex("dbo.Karta", new[] { "Sherbime_Id" });
            DropTable("dbo.Karta");
        }
    }
}
