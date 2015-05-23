using EasyLearning.Domain.Abstract;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EasyLearning.Domain.Entity
{
    public class Reply : AuditableEntity<long>
    {
        [Required]
        public long CommentID { get; set; }
        [AllowHtml, DataType(DataType.MultilineText)]
        public string Content { get; set; }
        [Required]
        public virtual Comment Comment { get; set; }
    }
}
