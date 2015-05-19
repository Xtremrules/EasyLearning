using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using EasyLearning.Domain.Entity;
using System.Data.Entity;
using EasyLearning.Domain.Abstract;
using System.Threading;

namespace EasyLearning.Domain.Concrete
{
    public class EasyLearningDB : IdentityDbContext<AppUser>
    {
        public EasyLearningDB()
            : base("EasyLearningDB") { }

        public DbSet<College> Colleges { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Study> Studies { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reply> Replies { get; set; }

        public static EasyLearningDB Create()
        {
            return new EasyLearningDB();
        }

        public override int SaveChanges()
        {
            ImplementAuditableEntity();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            ImplementAuditableEntity();
            return base.SaveChangesAsync();
        }

        void ImplementAuditableEntity()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity
                && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in modifiedEntries)
            {
                IAuditableEntity entity = entry.Entity as IAuditableEntity;
                if (entity != null)
                {
                    string identityName = Thread.CurrentPrincipal.Identity.Name;
                    DateTime now = DateTime.Now;
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedBy = identityName;
                        entity.CreatedDate = now;
                    }
                    else
                    {
                        base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                        base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                    }

                    entity.UpdatedBy = identityName;
                    entity.UpdatedDate = now;
                }
            }
        }

    }
}
