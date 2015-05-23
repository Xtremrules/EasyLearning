using EasyLearning.Domain.Abstract.Service;
using EasyLearning.Domain.Concrete;
using EasyLearning.Domain.Entity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Service
{
    public class CommentService : EntityService<Comment>, ICommentService
    {
        public CommentService(EasyLearningDB context)
            : base(context) { }

        public async Task<Comment> GetByIdAsync(long Id)
        {
            return await _dbset.FindAsync(Id);
        }

        public override IEnumerable<Comment> GetAll()
        {
            return _context.Comments
                .Include(x => x.Replies)
                .Include(x => x.Study).ToList();
        }
    }
}
