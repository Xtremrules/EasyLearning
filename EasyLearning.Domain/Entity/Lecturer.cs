using EasyLearning.Domain.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyLearning.Domain.Entity
{
    public class Lecturer : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string RegNo { get; set; }
        [Required]
        public string AppUserID { get; set; }
        public virtual AppUser AppUser { get; set; }
        [Required]
        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
        //[ForeignKey("DepartmentID")]
        public virtual Department Department { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
