using EasyLearning.Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace EasyLearning.WebUI.Areas.adminsecured.Models
{
    public class AddCourseViewModel
    {
        public long ID { get; set; }
        [Required]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "Course Title should not be more or less than 7 characters")]
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }
        [Required, Display(Name = "Course Title")]
        public string CourseTitle { get; set; }
        public string DepartmentCode { get; set; }
        [Required(ErrorMessage = "Select a department")]
        public int DepartmentID { get; set; }
        [Required, Display(Name = "Credit Load"), Range(0, 30)]
        public int CreditLoad { get; set; }
        [Required]
        public Level Level { get; set; }
        [Required]
        public Semester Semester { get; set; }
    }
}