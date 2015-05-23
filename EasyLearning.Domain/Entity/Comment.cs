using EasyLearning.Domain.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EasyLearning.Domain.Entity
{
    public class Comment : AuditableEntity<long>
    {
        [Required]
        public long StudyID { get; set; }
        [Required, AllowHtml, DataType(DataType.MultilineText)]
        public string Content { get; set; }
        public virtual Study Study { get; set; }
        public virtual ICollection<Reply> Replies { get; set; }
    }
}