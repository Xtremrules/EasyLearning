using EasyLearning.Domain.Abstract.Service;
using EasyLearning.Domain.Concrete;
using EasyLearning.Domain.Entity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Service
{
    public class DepartmentService : EntityService<Department>, IDepartmentService
    {
        public DepartmentService(EasyLearningDB context)
            : base(context) { }

        public async Task<Department> GetbyIdAsync(int ID)
        {
            return await _dbset.FindAsync(ID);
        }

        public override IEnumerable<Department> GetAll()
        {
            return _context.Departments.Include(x => x.College)
                .Include(x => x.Courses)
                .Include(x => x.Lecturers)
                .Include(x => x.Students)
                .ToList();
        }
    }
}
