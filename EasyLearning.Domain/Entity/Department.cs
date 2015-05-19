using EasyLearning.Domain.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EasyLearning.Domain.Entity
{
    public class Department : AuditableEntity<int>
    {
        [Required]
        public string Name { get; set; }
        [Required, Display(Name = "Department Code")]
        [StringLength(5, MinimumLength = 3, ErrorMessage = "Just a Short name of a Department and shouldn't Exceed 5 characters")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Course Duration")]
        public Duration Duration { get; set; }
        [Required(ErrorMessage = "You must select a college")]
        public int CollegeID { get; set; }
        public virtual College College { get; set; }
        public virtual ICollection<Lecturer> Lecturers { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
