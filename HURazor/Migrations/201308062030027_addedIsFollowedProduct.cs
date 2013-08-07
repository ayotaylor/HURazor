namespace HURazor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedIsFollowedProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "isFollowed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "isFollowed");
        }
    }
}
