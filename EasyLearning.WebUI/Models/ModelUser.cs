using EasyLearning.Domain.Entity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EasyLearning.WebUI.Models
{
    public class ModelUser : CreateViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Specify your First Name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please Specify your Last Name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
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
    }
}