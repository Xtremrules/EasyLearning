using EasyLearning.Domain.Entity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Identity
{
    public class AppSignInManager : SignInManager<AppUser,string>
    {
        public AppSignInManager(AppUserManager UserManager, IAuthenticationManager AuthManager)
            : base(UserManager, AuthManager) { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(AppUser user)
        {
            return user.GenerateUserIdentityAsync((AppUserManager)UserManager);
        }

        public static AppSignInManager Create(IdentityFactoryOptions<AppSignInManager> options, IOwinContext contex)
        {
            return new AppSignInManager(contex.GetUserManager<AppUserManager>(), contex.Authentication);
        }
    }
}
