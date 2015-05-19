using EasyLearning.Domain.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyLearning.Domain.Entity
{
    public class Student : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string RegNo { get; set; }
        [Required]
        public string AppUserID { get; set; }
        [Required]
        public int DepartmentID { get; set; }
        [Required]
        public Level Level { get; set; }
        public virtual Department Department { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}