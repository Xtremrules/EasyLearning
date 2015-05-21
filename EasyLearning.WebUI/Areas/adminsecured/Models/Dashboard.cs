using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyLearning.WebUI.Areas.adminsecured.Models
{
    public class Dashboard
    {
        public int NumberOfColleges { get; set; }
        public int NumberOfDepartment { get; set; }
        public int NumberOfLecturers { get; set; }
        public long NumberOfStudents { get; set; }
        public long NumberOfCourses { get; set; }
        public long NumberOfStudies { get; set; }
    }
}