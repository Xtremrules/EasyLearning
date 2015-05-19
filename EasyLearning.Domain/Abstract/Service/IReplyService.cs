using EasyLearning.Domain.Entity;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Abstract.Service
{
    public interface IReplyService : IEntityService<Reply>
    {
        Task<Reply> GetByIdAsync(long Id);
    }
}
