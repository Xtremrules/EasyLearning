namespace EasyLearning.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class assignmentProp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assignments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StudentRegNo = c.String(maxLength: 128),
                        StudyID = c.Long(nullable: false),
                        Score = c.Int(nullable: false),
                        AssignmentUrl = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Students", t => t.StudentRegNo)
                .ForeignKey("dbo.Studies", t => t.StudyID, cascadeDelete: true)
                .Index(t => t.StudentRegNo)
                .Index(t => t.StudyID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assignments", "StudyID", "dbo.Studies");
            DropForeignKey("dbo.Assignments", "StudentRegNo", "dbo.Students");
            DropIndex("dbo.Assignments", new[] { "StudyID" });
            DropIndex("dbo.Assignments", new[] { "StudentRegNo" });
            DropTable("dbo.Assignments");
        }
    }
}
