using EasyLearning.Domain.Abstract.Service;
using EasyLearning.Domain.Concrete;
using EasyLearning.Domain.Entity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Service
{
    public class ReplyService : EntityService<Reply>, IReplyService
    {
        public ReplyService(EasyLearningDB context)
            : base(context) { }

        public async Task<Reply> GetByIdAsync(long Id)
        {
            return await _dbset.FindAsync(Id);
        }

        public override IEnumerable<Reply> GetAll()
        {
            return _context.Replies.Include(x => x.AppUser)
                .Include(x => x.Comment).ToList();
        }
    }
}
