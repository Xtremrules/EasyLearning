using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EasyLearning.Domain.Abstract
{
    public abstract class Entity<T> : BaseEntity, IEntity<T>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput(DisplayValue = false)]
        public T ID { get; set; }
    }
}
