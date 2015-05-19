namespace EasyLearning.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Colleges",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Title = c.String(nullable: false, maxLength: 10),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Title = c.String(nullable: false, maxLength: 5),
                        Duration = c.Int(nullable: false),
                        CollegeID = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Colleges", t => t.CollegeID, cascadeDelete: true)
                .Index(t => t.CollegeID);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        CourseCode = c.String(nullable: false, maxLength: 7),
                        CourseTitle = c.String(nullable: false),
                        DepartmentCode = c.String(nullable: false),
                        CreditLoad = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                        Semester = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Lecturers",
                c => new
                    {
                        RegNo = c.String(nullable: false, maxLength: 128),
                        AppUserID = c.String(nullable: false, maxLength: 128),
                        DepartmentID = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.RegNo)
                .ForeignKey("dbo.Users", t => t.AppUserID, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.DepartmentID, cascadeDelete: true)
                .Index(t => t.AppUserID)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        MiddleName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Gender = c.Int(nullable: false),
                        State = c.String(nullable: false),
                        ImageMine = c.String(),
                        ImageContent = c.Binary(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        UserClaimId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.UserClaimId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        StudyID = c.Long(nullable: false),
                        AppUserID = c.String(nullable: false, maxLength: 128),
                        Content = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.AppUserID, cascadeDelete: true)
                .ForeignKey("dbo.Studies", t => t.StudyID, cascadeDelete: true)
                .Index(t => t.StudyID)
                .Index(t => t.AppUserID);
            
            CreateTable(
                "dbo.Replies",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        CommentID = c.Long(nullable: false),
                        Content = c.String(),
                        AppUserID = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.AppUserID, cascadeDelete: true)
                .ForeignKey("dbo.Comments", t => t.CommentID)
                .Index(t => t.CommentID)
                .Index(t => t.AppUserID);
            
            CreateTable(
                "dbo.Studies",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Summary = c.String(nullable: false),
                        VideoUrl = c.String(),
                        Assignment = c.String(),
                        NoteUrl = c.String(),
                        CourseID = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .Index(t => t.CourseID);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        RegNo = c.String(nullable: false, maxLength: 128),
                        AppUserID = c.String(nullable: false, maxLength: 128),
                        DepartmentID = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.RegNo)
                .ForeignKey("dbo.Users", t => t.AppUserID, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.DepartmentID, cascadeDelete: true)
                .Index(t => t.AppUserID)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Roles = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Roles)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.CourseLecturers",
                c => new
                    {
                        CourseID = c.Long(nullable: false),
                        LecturerRegNo = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CourseID, t.LecturerRegNo })
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.Lecturers", t => t.LecturerRegNo, cascadeDelete: true)
                .Index(t => t.CourseID)
                .Index(t => t.LecturerRegNo);
            
            CreateTable(
                "dbo.CourseStudents",
                c => new
                    {
                        CourseID = c.Long(nullable: false),
                        StudentRegNo = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CourseID, t.StudentRegNo })
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentRegNo, cascadeDelete: true)
                .Index(t => t.CourseID)
                .Index(t => t.StudentRegNo);
            
            CreateTable(
                "dbo.DepartmentCourse",
                c => new
                    {
                        DepartmentID = c.Int(nullable: false),
                        CourseID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.DepartmentID, t.CourseID })
                .ForeignKey("dbo.Departments", t => t.DepartmentID, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .Index(t => t.DepartmentID)
                .Index(t => t.CourseID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.DepartmentCourse", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.DepartmentCourse", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.CourseStudents", "StudentRegNo", "dbo.Students");
            DropForeignKey("dbo.CourseStudents", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.Students", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.Students", "AppUserID", "dbo.Users");
            DropForeignKey("dbo.CourseLecturers", "LecturerRegNo", "dbo.Lecturers");
            DropForeignKey("dbo.CourseLecturers", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.Lecturers", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.Lecturers", "AppUserID", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.Studies", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.Comments", "StudyID", "dbo.Studies");
            DropForeignKey("dbo.Replies", "CommentID", "dbo.Comments");
            DropForeignKey("dbo.Replies", "AppUserID", "dbo.Users");
            DropForeignKey("dbo.Comments", "AppUserID", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.Departments", "CollegeID", "dbo.Colleges");
            DropIndex("dbo.DepartmentCourse", new[] { "CourseID" });
            DropIndex("dbo.DepartmentCourse", new[] { "DepartmentID" });
            DropIndex("dbo.CourseStudents", new[] { "StudentRegNo" });
            DropIndex("dbo.CourseStudents", new[] { "CourseID" });
            DropIndex("dbo.CourseLecturers", new[] { "LecturerRegNo" });
            DropIndex("dbo.CourseLecturers", new[] { "CourseID" });
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.Students", new[] { "DepartmentID" });
            DropIndex("dbo.Students", new[] { "AppUserID" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.Studies", new[] { "CourseID" });
            DropIndex("dbo.Replies", new[] { "AppUserID" });
            DropIndex("dbo.Replies", new[] { "CommentID" });
            DropIndex("dbo.Comments", new[] { "AppUserID" });
            DropIndex("dbo.Comments", new[] { "StudyID" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.Lecturers", new[] { "DepartmentID" });
            DropIndex("dbo.Lecturers", new[] { "AppUserID" });
            DropIndex("dbo.Departments", new[] { "CollegeID" });
            DropTable("dbo.DepartmentCourse");
            DropTable("dbo.CourseStudents");
            DropTable("dbo.CourseLecturers");
            DropTable("dbo.Roles");
            DropTable("dbo.Students");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserLogins");
            DropTable("dbo.Studies");
            DropTable("dbo.Replies");
            DropTable("dbo.Comments");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.Lecturers");
            DropTable("dbo.Courses");
            DropTable("dbo.Departments");
            DropTable("dbo.Colleges");
        }
    }
}
