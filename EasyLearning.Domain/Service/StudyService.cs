using EasyLearning.Domain.Abstract.Service;
using EasyLearning.Domain.Concrete;
using EasyLearning.Domain.Entity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Service
{
    public class StudyService : EntityService<Study>, IStudyService
    {
        public StudyService(EasyLearningDB context)
            : base(context) { }

        public async Task<Study> GetByIdAsync(long Id)
        {
            return await _dbset.FindAsync(Id);
        }

        public override async Task CreateAsync(Study entity)
        {
            IEnumerable<Student> AllStudentsWithStudyCourse = _context.Courses.Where(x => x.ID == entity.CourseID).SelectMany(x => x.Students);
            List<AppUser> Users = AllStudentsWithStudyCourse.Select(x => x.AppUser).ToList();
            foreach (var user in Users)
            {
                user.Activities = new List<Activity>();
                user.Activities.Add(new Activity
                {
                    AppUserID = user.Id,
                    StudyID = entity.ID
                });
            }
            await base.CreateAsync(entity);
        }

        public override IEnumerable<Study> GetAll()
        {
            return _context.Studies.Include(x => x.Comments)
                .Include(x => x.Course).ToList();
        }
    }
}
