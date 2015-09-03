using EasyLearning.Domain.Entity;
using EasyLearning.Domain.Identity;
using EasyLearning.Domain.Models;
using EasyLearning.WebUI.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EasyLearning.WebUI.Areas.adminsecured.Controllers
{
    public class AccountController : Controller
    {
        // GET: adminsecured/Account
        [AllowAnonymous]
        public ActionResult Index()
        {
            return RedirectToAction("Login");
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
                return RedirectToAction("index", "office", new { area = "adminsecured" });
            else
                return RedirectToAction("Logout", "Account", new { area = "" });
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
                    if (await UserManager.IsInRoleAsync(user.Id, Roles.Admin))
                    {
                        var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.isPersistent, false);
                        switch (result)
                        {
                            case SignInStatus.Success:
                                return RedirectToLocal(returnUrl, user);
                            case SignInStatus.Failure:
                            case SignInStatus.LockedOut:
                            case SignInStatus.RequiresVerification:
                            default:
                                ModelState.AddModelError("", "Invalid code.");
                                return View(model);
                        }
                    }
                }
                ModelState.AddModelError("", "username or password incorrect");
            }
            return View(model);
        }

        ActionResult RedirectToLocal(string returnUrl, AppUser user)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("index", "office", new { area = "adminsecured" });
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