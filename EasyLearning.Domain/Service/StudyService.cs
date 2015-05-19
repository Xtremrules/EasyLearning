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

        public override IEnumerable<Study> GetAll()
        {
            return _context.Studies.Include(x => x.Comments)
                .Include(x => x.Course).ToList();
        }
    }
}
