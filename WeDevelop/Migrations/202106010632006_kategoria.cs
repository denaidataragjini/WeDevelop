namespace WeDevelop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kategoria : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Kategoria",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmerKat = c.String(),
                        sherbimId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Sherbimet", "Kategoria_Id", c => c.Int());
            CreateIndex("dbo.Sherbimet", "Kategoria_Id");
            AddForeignKey("dbo.Sherbimet", "Kategoria_Id", "dbo.Kategoria", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sherbimet", "Kategoria_Id", "dbo.Kategoria");
            DropIndex("dbo.Sherbimet", new[] { "Kategoria_Id" });
            DropColumn("dbo.Sherbimet", "Kategoria_Id");
            DropTable("dbo.Kategoria");
        }
    }
}
