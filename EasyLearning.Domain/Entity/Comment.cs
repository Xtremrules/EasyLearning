﻿using EasyLearning.Domain.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EasyLearning.Domain.Entity
{
    public class Comment : AuditableEntity<long>
    {
        [Required]
        public long StudyID { get; set; }
        [Required]
        public string AppUserID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required, AllowHtml]
        public string Commnet { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual Study Study { get; set; }
        public virtual ICollection<Reply> Replies { get; set; }
    }
}