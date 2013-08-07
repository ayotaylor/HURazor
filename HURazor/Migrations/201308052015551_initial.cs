namespace HURazor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
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
                    })
                .PrimaryKey(t => t.ProductID);
            
            CreateTable(
                "dbo.Follow",
                c => new
                    {
                        FollowID = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FollowID)
                .ForeignKey("dbo.Product", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Follow", new[] { "ProductID" });
            DropForeignKey("dbo.Follow", "ProductID", "dbo.Product");
            DropTable("dbo.Follow");
            DropTable("dbo.Product");
        }
    }
}
