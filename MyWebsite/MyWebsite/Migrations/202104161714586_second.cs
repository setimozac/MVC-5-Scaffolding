namespace MyWebsite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PurchaseViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PurchaseDate = c.DateTime(nullable: false),
                        Payment = c.Int(nullable: false),
                        ShippingDate = c.DateTime(nullable: false),
                        address = c.String(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        Description = c.String(nullable: false),
                        Price = c.Double(nullable: false),
                        Purchase_Id = c.Int(),
                        Shipping_ShippingId = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Purchases", t => t.Purchase_Id)
                .ForeignKey("dbo.Shippings", t => t.Shipping_ShippingId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Purchase_Id)
                .Index(t => t.Shipping_ShippingId)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.Purchases", "PurchaseViewModel_Id", c => c.Int());
            CreateIndex("dbo.Purchases", "PurchaseViewModel_Id");
            AddForeignKey("dbo.Purchases", "PurchaseViewModel_Id", "dbo.PurchaseViewModels", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseViewModels", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PurchaseViewModels", "Shipping_ShippingId", "dbo.Shippings");
            DropForeignKey("dbo.Purchases", "PurchaseViewModel_Id", "dbo.PurchaseViewModels");
            DropForeignKey("dbo.PurchaseViewModels", "Purchase_Id", "dbo.Purchases");
            DropIndex("dbo.PurchaseViewModels", new[] { "User_Id" });
            DropIndex("dbo.PurchaseViewModels", new[] { "Shipping_ShippingId" });
            DropIndex("dbo.PurchaseViewModels", new[] { "Purchase_Id" });
            DropIndex("dbo.Purchases", new[] { "PurchaseViewModel_Id" });
            DropColumn("dbo.Purchases", "PurchaseViewModel_Id");
            DropTable("dbo.PurchaseViewModels");
        }
    }
}
