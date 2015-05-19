using EasyLearning.Domain.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EasyLearning.Domain.Entity
{
    public class Course : AuditableEntity<long>
    {
        [Required]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "Course Title should not be more or less than 7 characters")]
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }
        [Required, Display(Name = "Course Title")]
        public string CourseTitle { get; set; }
        [Required, HiddenInput(DisplayValue = false)]
        public string DepartmentCode { get; set; }
        [Required, Display(Name = "Credit Load"), Range(0, 30)]
        public int CreditLoad { get; set; }
        [Required]
        public Level Level { get; set; }
        [Required]
        public Semester Semester { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Lecturer> Lecturers { get; set; }
        public virtual ICollection<Study> Studies { get; set; }
    }
}
