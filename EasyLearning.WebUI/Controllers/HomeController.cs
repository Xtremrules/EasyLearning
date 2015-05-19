using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EasyLearning.Domain.Abstract.Service;
using System.Web.Mvc;

namespace EasyLearning.WebUI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        ICollegeService collegeService;
        public HomeController(ICollegeService college)
        {
            collegeService = college;
        }
        public ActionResult Index()
        {
            var college = collegeService.GetAll();
            return View(college);
        }
    }
}