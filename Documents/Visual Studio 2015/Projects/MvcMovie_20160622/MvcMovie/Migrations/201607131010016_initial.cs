namespace MvcMovie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Metric",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SessionID = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                        MetricName = c.String(nullable: false),
                        MetricValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserSession",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        SessionID = c.String(),
                        partner_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Partner", t => t.partner_ID)
                .Index(t => t.partner_ID);
            
            CreateTable(
                "dbo.Partner",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        partnerName = c.String(nullable: false, maxLength: 100),
                        partnerRef = c.String(nullable: false, maxLength: 40),
                        country = c.String(nullable: false),
                        createdDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserSession", "partner_ID", "dbo.Partner");
            DropIndex("dbo.UserSession", new[] { "partner_ID" });
            DropTable("dbo.Partner");
            DropTable("dbo.UserSession");
            DropTable("dbo.Metric");
        }
    }
}
