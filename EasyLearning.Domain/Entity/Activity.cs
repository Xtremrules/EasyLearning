using EasyLearning.Domain.Abstract;

namespace EasyLearning.Domain.Entity
{
    public class Activity : AuditableEntity<int>
    {
        public string AppUserID { get; set; }
        public long StudyID { get; set; }
        public virtual Study Study { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}