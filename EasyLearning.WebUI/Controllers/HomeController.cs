using EasyLearning.Domain.Abstract.Service;
using System.Web.Mvc;

namespace EasyLearning.WebUI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            //ViewBag.Message = "WHAT YOU NEED TO KNOW ABOUT ONLINE LEARNING AND TEACHING APPLICATION";

            return View();
        }

        public ActionResult Contact()
        {
            //ViewBag.Message = "WE ARE HERE TO MAKE YOU LEARN WITH EASE...";

            return View();
        }
    }
}