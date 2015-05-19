using System.ComponentModel.DataAnnotations;

namespace EasyLearning.WebUI.Models
{
    public class CreateViewModel
    {
        [Required]
        public string username { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, Compare("Password", ErrorMessage = "Password doesn't match"), DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ComparePassword { get; set; }
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber), Required]
        public string PhoneNumber { get; set; }
    }
}