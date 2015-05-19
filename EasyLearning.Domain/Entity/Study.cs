using EasyLearning.Domain.Abstract;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace EasyLearning.Domain.Entity
{
    public class Study : AuditableEntity<long>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Summary { get; set; }
        public string VideoUrl { get; set; }
        public string Assignment { get; set; }
        public string NoteUrl { get; set; }
        public long CourseID { get; set; }
        public virtual Course Course { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
