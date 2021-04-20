namespace MyWebsite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Third : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Purchases", "PurchaseViewModel_Id", "dbo.PurchaseViewModels");
            DropIndex("dbo.Purchases", new[] { "PurchaseViewModel_Id" });
            DropColumn("dbo.Purchases", "PurchaseViewModel_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Purchases", "PurchaseViewModel_Id", c => c.Int());
            CreateIndex("dbo.Purchases", "PurchaseViewModel_Id");
            AddForeignKey("dbo.Purchases", "PurchaseViewModel_Id", "dbo.PurchaseViewModels", "Id");
        }
    }
}
