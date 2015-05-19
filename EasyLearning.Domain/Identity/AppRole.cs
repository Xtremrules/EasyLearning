using Microsoft.AspNet.Identity.EntityFramework;

namespace EasyLearning.Domain.Identity
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }
        public AppRole(string Name) : base(Name) { }
    }
}
