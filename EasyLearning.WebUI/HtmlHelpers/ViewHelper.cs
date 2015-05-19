using EasyLearning.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Mvc;

namespace EasyLearning.WebUI.HtmlHelpers
{
    public static class ViewHelper
    {
        public static MvcHtmlString GetFullName(this HtmlHelper html, string name)
        {
            var user = UserManager.FindByNameAsync(name);
            string fullName = user.Result.LastName + " " + user.Result.FirstName;
            return new MvcHtmlString(fullName);
        }

        //public static MvcHtmlString DepartmentDuration(this HtmlHelper html, Level level)
        //{
        //    string result;
        //    if (level == Level.Fourth)
        //        result = "Four Years";
        //    else if(level == Level.Fifth)
        //        result = "Five"
        //}


        static AppUserManager UserManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
    }
}