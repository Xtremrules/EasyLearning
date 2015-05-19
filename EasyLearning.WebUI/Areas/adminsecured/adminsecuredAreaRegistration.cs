using System.Web.Mvc;

namespace EasyLearning.WebUI.Areas.adminsecured
{
    public class adminsecuredAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "adminsecured";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "adminsecured_default",
                "adminsecured/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}