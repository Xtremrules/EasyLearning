using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Entity
{
    public class AppUser : IdentityUser
    {
        [Required(ErrorMessage = "Please Specify your First Name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please Specify your Last Name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [NotMapped]
        [Display(Name = "Name")]
        public string FullName
        {
            get { return LastName + ", " + FirstName + " " + MiddleName; }
        }
        [Required(ErrorMessage = "Please Specify your Date of Birth of Birth")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Please Specify your Gender")]
        [Display(Name = "Gender")]
        public Sex Gender { get; set; }
        [Required(ErrorMessage = "Please Specify your State")]
        [Display(Name = "State")]
        public string State { get; set; }
        [NotMapped]
        public int Age
        {
            get { return DateTime.Now.Year - DateOfBirth.Year; }
        }

        public string ImageMine { get; set; }
        public byte[] ImageContent { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Reply> Replies { get; set; }
    }
}
