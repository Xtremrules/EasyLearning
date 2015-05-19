namespace EasyLearning.Domain.Migrations
{
    using EasyLearning.Domain.Entity;
    using EasyLearning.Domain.Identity;
    using EasyLearning.Domain.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;

    internal sealed class Configuration : DbMigrationsConfiguration<EasyLearning.Domain.Concrete.EasyLearningDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(EasyLearning.Domain.Concrete.EasyLearningDB context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Database.Log = s => Debug.WriteLine(s);

            // Create Admin User
            AppUserManager usermanager = new AppUserManager(new UserStore<AppUser>(context));
            AppRoleManager rolemanager = new AppRoleManager(new RoleStore<AppRole>(context));

            if (!rolemanager.RoleExists(Roles.Admin))
                rolemanager.Create(new AppRole(Roles.Admin));
            if (!rolemanager.RoleExists(Roles.Lecturer))
                rolemanager.Create(new AppRole(Roles.Lecturer));
            if (!rolemanager.RoleExists(Roles.Students))
                rolemanager.Create(new AppRole(Roles.Students));
            if(!rolemanager.RoleExists(Roles.Study))
                rolemanager.Create(new AppRole(Roles.Study));

            AppUser user = usermanager.FindByName(Owner.UserName);
            if (user == null)
            {
                usermanager.Create(new AppUser
                {
                    DateOfBirth = Owner.DateOfBirth,
                    Email = Owner.Email,
                    FirstName = Owner.FirstName,
                    Gender = Owner.Gender,
                    LastName = Owner.LastName,
                    EmailConfirmed = true,
                    MiddleName = Owner.MiddleName,
                    State = Owner.state,
                    UserName = Owner.UserName,
                });
                user = usermanager.FindByName(Owner.UserName);
            }

            if (!usermanager.IsInRole(user.Id, Roles.Admin))
                usermanager.AddToRole(user.Id, Roles.Admin);
            context.SaveChanges();
        }
    }
}
