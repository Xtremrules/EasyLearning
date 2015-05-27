namespace EasyLearning.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class activities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AppUserID = c.String(maxLength: 128),
                        StudyID = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.AppUserID)
                .ForeignKey("dbo.Studies", t => t.StudyID, cascadeDelete: true)
                .Index(t => t.AppUserID)
                .Index(t => t.StudyID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Activities", "StudyID", "dbo.Studies");
            DropForeignKey("dbo.Activities", "AppUserID", "dbo.Users");
            DropIndex("dbo.Activities", new[] { "StudyID" });
            DropIndex("dbo.Activities", new[] { "AppUserID" });
            DropTable("dbo.Activities");
        }
    }
}
