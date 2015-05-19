using EasyLearning.WebUI.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EasyLearning.WebUI.Areas.adminsecured.Models
{
    public class UserCreateModel : ModelUser
    {
        [Required]
        public int DepartmentID { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string AppUserID { get; set; }
    }
}