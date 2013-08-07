namespace HURazor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class followPrimaryKeyChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Follow", "FollowID", c => c.Int(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Follow", "FollowID", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
