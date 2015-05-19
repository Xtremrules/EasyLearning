using EasyLearning.Domain.Abstract.Service;
using EasyLearning.Domain.Concrete;
using EasyLearning.Domain.Entity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Service
{
    class LecturerService : EntityService<Lecturer>, ILecturerService
    {
        public LecturerService(EasyLearningDB context)
            : base(context) { }

        public async Task<Lecturer> GetByRegNoAsync(string RegNo)
        {
            return await _dbset.FindAsync(RegNo);
        }

        public override IEnumerable<Lecturer> GetAll()
        {
            return _context.Lecturers.Include(x => x.AppUser)
                .Include(x => x.Courses)
                .Include(x => x.Department)
                .ToList();
        }
    }
}
