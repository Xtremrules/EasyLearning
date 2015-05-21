
namespace EasyLearning.WebUI.Areas.adminsecured.Models
{
    public class AssignedCourseData
    {
        public long CourseID { get; set; }
        public string Title { get; set; }
        public bool Assigned { get; set; }
        public string CourseCode { get; set; }
        public int Unit { get; set; }
    }
}