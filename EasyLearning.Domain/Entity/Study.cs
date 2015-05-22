using EasyLearning.Domain.Abstract;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace EasyLearning.Domain.Entity
{
    public class Study : AuditableEntity<long>
    {
        [Required]
        public string Name { get; set; }
        [AllowHtml]
        [Required, DataType(DataType.MultilineText)]
        public string Summary { get; set; }
        public string VideoUrl { get; set; }
        public string VideoName { get; set; }
        public string VideoType { get; set; }
        public string Assignment { get; set; }
        public string NoteUrl { get; set; }
        public string NoteName { get; set; }
        public string NoteType { get; set; }
        public long CourseID { get; set; }
        public virtual Course Course { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
