using EasyLearning.Domain.Entity;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Abstract.Service
{
    public interface ICommentService : IEntityService<Comment>
    {
        Task<Comment> GetByIdAsync(long Id);
    }
}
