using EasyLearning.Domain.Abstract.Service;
using EasyLearning.Domain.Concrete;
using EasyLearning.Domain.Entity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Service
{
    public class StudentService : EntityService<Student>, IStudentService
    {
        public StudentService(EasyLearningDB context)
            : base(context) { }

        public async Task<Student> GetByRegNoAsync(string RegNo)
        {
            return await _dbset.FindAsync(RegNo);
        }

        public override IEnumerable<Student> GetAll()
        {
            return _context.Students.Include(x => x.AppUser)
                .Include(x => x.Courses)
                .Include(x => x.Department)
                .ToList();
        }
    }
}
