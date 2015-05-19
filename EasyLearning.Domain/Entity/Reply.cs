using EasyLearning.Domain.Abstract;
using System.ComponentModel.DataAnnotations;

namespace EasyLearning.Domain.Entity
{
    public class Reply : AuditableEntity<long>
    {
        [Required]
        public long CommentID { get; set; }
        public string AppUserID { get; set; }
        public string UserName { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual Comment Comment { get; set; }
    }
}
