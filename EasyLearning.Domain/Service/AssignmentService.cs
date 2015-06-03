using EasyLearning.Domain.Abstract.Service;
using EasyLearning.Domain.Concrete;
using EasyLearning.Domain.Entity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Service
{
    public class AssignmentService : EntityService<Assignment>, IAssignmentService
    {
        public AssignmentService(EasyLearningDB context)
            : base(context) { }

        public override IEnumerable<Assignment> GetAll()
        {
            return _context.Assignments
                .Include(x => x.Student)
                .Include(x => x.Study).ToList();
        }

        public Assignment GetById(int id)
        {
            return _dbset.Find(id);
        }


        public async Task<Assignment> FindByIdAsync(int id)
        {
            return await _dbset.FindAsync(id);
        }
    }
}
