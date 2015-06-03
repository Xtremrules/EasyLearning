using EasyLearning.Domain.Entity;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Abstract.Service
{
    public interface IAssignmentService : IEntityService<Assignment>
    {
        Assignment GetById(int id);
        Task<Assignment> FindByIdAsync(int id);
    }
}
