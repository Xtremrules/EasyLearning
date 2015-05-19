using EasyLearning.Domain.Entity;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Abstract.Service
{
    public interface ICourseService : IEntityService<Course>
    {
        Task<Course> GetByIdAsync(long ID);
    }
}
