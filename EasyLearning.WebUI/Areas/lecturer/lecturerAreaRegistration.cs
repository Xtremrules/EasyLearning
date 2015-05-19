using System.Web.Mvc;

namespace EasyLearning.WebUI.Areas.lecturer
{
    public class lecturerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "lecturer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "lecturer_default",
                "lecturer/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}