using EasyLearning.Domain.Entity;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Abstract.Service
{
    public interface IActivityService : IEntityService<Activity>
    {
        Task DeleteActivity(long CourseID, string userId);
    }
}
