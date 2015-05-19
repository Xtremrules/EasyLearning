using EasyLearning.Domain.Entity;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Abstract.Service
{
    public interface IDepartmentService : IEntityService<Department>
    {
        Task<Department> GetbyIdAsync(int ID);
    }
}
