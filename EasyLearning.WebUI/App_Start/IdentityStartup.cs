using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using EasyLearning.Domain.Concrete;
using EasyLearning.Domain.Identity;
using Microsoft.Owin.Security.Cookies;
using EasyLearning.Domain.Entity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

[assembly: OwinStartup(typeof(EasyLearning.WebUI.IdentityStartup))]
namespace EasyLearning.WebUI
{
    public class IdentityStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<EasyLearningDB>(EasyLearningDB.Create);
            app.CreatePerOwinContext<AppRoleManager>(AppRoleManager.Create);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            app.CreatePerOwinContext<AppSignInManager>(AppSignInManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                LoginPath = new PathString("/Account/Login"),
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<AppUserManager, AppUser>(
                    validateInterval: TimeSpan.FromHours(1),
                    regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                },
                ExpireTimeSpan = TimeSpan.FromHours(1),
            });
        }
    }
}
