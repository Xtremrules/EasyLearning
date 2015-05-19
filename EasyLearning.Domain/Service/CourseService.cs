using EasyLearning.Domain.Abstract.Service;
using EasyLearning.Domain.Concrete;
using EasyLearning.Domain.Entity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Service
{
    public class CourseService : EntityService<Course>, ICourseService
    {
        public CourseService(EasyLearningDB context)
            : base(context) { }

        public async Task<Course> GetByIdAsync(long ID)
        {
            return await _dbset.FindAsync(ID);
        }

        public override IEnumerable<Course> GetAll()
        {
            return _context.Courses
                .Include(x => x.Departments)
                .Include(x => x.Lecturers)
                .Include(x => x.Students)
                .Include(x => x.Studies).ToList();
        }
    }
}
