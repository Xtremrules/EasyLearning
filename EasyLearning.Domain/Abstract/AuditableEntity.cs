using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Abstract
{
    public abstract class AuditableEntity : BaseEntity, IAuditableEntity
    {
        [ScaffoldColumn(false), DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
        [MaxLength(256), ScaffoldColumn(false)]
        public string CreatedBy { get; set; }
        [ScaffoldColumn(false), DataType(DataType.DateTime)]
        public DateTime UpdatedDate { get; set; }
        [MaxLength(256), ScaffoldColumn(false)]
        public string UpdatedBy { get; set; }
    }

    public abstract class AuditableEntity<T> : Entity<T>, IAuditableEntity
    {
        [ScaffoldColumn(false), DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
        [MaxLength(256), ScaffoldColumn(false)]
        public string CreatedBy { get; set; }
        [ScaffoldColumn(false), DataType(DataType.DateTime)]
        public DateTime UpdatedDate { get; set; }
        [MaxLength(256), ScaffoldColumn(false)]
        public string UpdatedBy { get; set; }
    }
}
