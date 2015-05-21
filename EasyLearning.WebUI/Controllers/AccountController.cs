using EasyLearning.Domain.Entity;
using EasyLearning.Domain.Identity;
using EasyLearning.Domain.Models;
using EasyLearning.WebUI.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyLearning.WebUI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            if (!SignInManager.AuthenticationManager.User.Identity.IsAuthenticated)
            {
                return View(new LoginViewModel());
            }
            AppUser user = await CurrentUser();
            if (user != null)
                return await RedirectBasedOnUserRole(user);
            else
                return RedirectToAction("Logout");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    //ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    //AuthManager.SignOut();
                    //AuthManager.SignIn(new AuthenticationProperties()
                    //{
                    //    IsPersistent = model.isPersistent,
                    //}, ident);
                    var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.isPersistent, false);
                    //if (string.IsNullOrEmpty(returnUrl))
                    //{
                    //    return await RedirectBasedOnUserRole(user);
                    //}
                    //else return Redirect(returnUrl);
                    switch (result)
                    {
                        case SignInStatus.Success:
                            return await RedirectToLocal(returnUrl, user);
                        case SignInStatus.Failure:
                        case SignInStatus.LockedOut:
                        case SignInStatus.RequiresVerification:
                        default:
                            ModelState.AddModelError("", "Invalid code.");
                            return View(model);
                    }
                }
                else ModelState.AddModelError("", "username or password incorrect");
            }
            return View(model);
        }

        async Task<ActionResult> RedirectToLocal(string returnUrl, AppUser user)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return await RedirectBasedOnUserRole(user);
        }

        async Task<ActionResult> RedirectBasedOnUserRole(AppUser user)
        {
            IList<string> userroles = await UserManager.GetRolesAsync(user.Id);
            if (await UserManager.IsInRoleAsync(user.Id, Roles.Admin))
                return RedirectToAction("Index", "office", new { area = "adminsecured" });
            else if (await UserManager.IsInRoleAsync(user.Id, Roles.Students))
                return RedirectToAction("Index", "student", new { area = "student" });
            else if (await UserManager.IsInRoleAsync(user.Id, Roles.Lecturer))
                return RedirectToAction("Index", "office", new { area = "lecturer" });
            else return RedirectToAction("Logout", "Account");
        }

        public ActionResult Logout()
        {
            AuthManager.SignOut();
            foreach (var item in Request.Cookies.AllKeys)
                Request.Cookies.Remove(item);
            foreach (var item in Response.Cookies.AllKeys)
                Response.Cookies.Remove(item);
            return RedirectToAction("Index", "Home");
        }

        async Task<AppUser> CurrentUser()
        {
            AppUser user = await UserManager.FindByNameAsync(HttpContext.User.Identity.Name);
            return user;
        }

        AppUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<AppUserManager>(); }
        }

        AppSignInManager SignInManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<AppSignInManager>(); }
        }

        IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}