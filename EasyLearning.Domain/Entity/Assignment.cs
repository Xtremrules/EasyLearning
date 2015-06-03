using EasyLearning.Domain.Abstract;
using System.ComponentModel.DataAnnotations;

namespace EasyLearning.Domain.Entity
{
    public class Assignment : AuditableEntity<int>
    {
        public string StudentRegNo { get; set; }
        public long StudyID { get; set; }
        [Range(0,30, ErrorMessage = "You cant Score more than 30")]
        public int? Score { get; set; }
        public string AssignmentUrl { get; set; }
        public string ContentType { get; set; }
        public string SaveName { get; set; }
        public virtual Study Study { get; set; }
        public virtual Student Student { get; set; }
    }
}
