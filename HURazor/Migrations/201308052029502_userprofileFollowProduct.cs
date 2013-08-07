namespace HURazor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userprofileFollowProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Price",
                c => new
                    {
                        PriceID = c.Int(nullable: false, identity: true),
                        PriceOne = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PriceID)
                .ForeignKey("dbo.Product", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID);
            
            AddForeignKey("dbo.Follow", "UserId", "dbo.UserProfile", "UserId", cascadeDelete: true);
            CreateIndex("dbo.Follow", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Price", new[] { "ProductID" });
            DropIndex("dbo.Follow", new[] { "UserId" });
            DropForeignKey("dbo.Price", "ProductID", "dbo.Product");
            DropForeignKey("dbo.Follow", "UserId", "dbo.UserProfile");
            DropTable("dbo.Price");
        }
    }
}
