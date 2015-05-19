using EasyLearning.Domain.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EasyLearning.Domain.Entity
{
    public class College : AuditableEntity<int>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [StringLength(10)]
        public string Title { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
    }
}
