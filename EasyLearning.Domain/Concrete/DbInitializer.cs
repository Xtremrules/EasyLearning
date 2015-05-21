using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using EasyLearning.Domain.Identity;
using EasyLearning.Domain.Models;
using EasyLearning.Domain.Entity;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EasyLearning.Domain.Concrete
{
    public class DbInitializer : CreateDatabaseIfNotExists<EasyLearningDB>
    {
        protected override void Seed(EasyLearningDB context)
        {
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
            if (!rolemanager.RoleExists(Roles.Study))
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
                }, Owner.Password);
                user = usermanager.FindByName(Owner.UserName);
            }

            if (!usermanager.IsInRole(user.Id, Roles.Admin))
                usermanager.AddToRole(user.Id, Roles.Admin);
            if (!usermanager.IsInRole(user.Id, Roles.Study))
                usermanager.AddToRole(user.Id, Roles.Study);

            context.SaveChanges();
        }
    }
}
