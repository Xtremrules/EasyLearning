namespace EasyLearning.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CourseLecturers", newName: "LecturerCourses");
            RenameTable(name: "dbo.CourseStudents", newName: "StudentCourses");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.StudentCourses", newName: "CourseStudents");
            RenameTable(name: "dbo.LecturerCourses", newName: "CourseLecturers");
        }
    }
}
