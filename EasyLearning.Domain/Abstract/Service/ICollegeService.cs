using EasyLearning.Domain.Entity;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Abstract.Service
{
    public interface ICollegeService : IEntityService<College>
    {
        Task<College> GetByIdAsync(int Id);
    }
}