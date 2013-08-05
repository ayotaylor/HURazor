namespace HURazor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nameEmailFollow : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Follow",
                c => new
                    {
                        FollowID = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FollowID)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                        Price = c.Decimal(precision: 18, scale: 2),
                        ImageUrl = c.String(),
                        DetailsUrl = c.String(),
                        ASIN = c.String(),
                        Product_ProductID = c.Int(),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.Product", t => t.Product_ProductID)
                .Index(t => t.Product_ProductID);
            
            AddColumn("dbo.UserProfile", "FirstName", c => c.String());
            AddColumn("dbo.UserProfile", "LastName", c => c.String());
            AddColumn("dbo.UserProfile", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropIndex("dbo.Product", new[] { "Product_ProductID" });
            DropIndex("dbo.Follow", new[] { "ProductID" });
            DropIndex("dbo.Follow", new[] { "UserId" });
            DropForeignKey("dbo.Product", "Product_ProductID", "dbo.Product");
            DropForeignKey("dbo.Follow", "ProductID", "dbo.Product");
            DropForeignKey("dbo.Follow", "UserId", "dbo.UserProfile");
            DropColumn("dbo.UserProfile", "Email");
            DropColumn("dbo.UserProfile", "LastName");
            DropColumn("dbo.UserProfile", "FirstName");
            DropTable("dbo.Product");
            DropTable("dbo.Follow");
        }
    }
}
