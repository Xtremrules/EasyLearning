using EasyLearning.Domain.Concrete;
using EasyLearning.Domain.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace EasyLearning.Domain.Identity
{
    public class AppUserManager: UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store)
            : base(store) { }
        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext contex)
        {
            AppUserManager manager = new AppUserManager(new UserStore<AppUser>(contex.Get<EasyLearningDB>()));

            manager.PasswordValidator = new PasswordValidator
            {
                RequireDigit = false,
                RequiredLength = 4,
                RequireLowercase = false,
            };

            manager.UserValidator = new UserValidator<AppUser>(manager)
            {
                RequireUniqueEmail = true,
                AllowOnlyAlphanumericUserNames = true
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<AppUser>(dataProtectionProvider.Create("EasyLearning App"));
            }
            return manager;
        }
    }
}
