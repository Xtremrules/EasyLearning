using EasyLearning.Domain.Entity;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Abstract.Service
{
    public interface IStudentService : IEntityService<Student>
    {
        Task<Student> GetByRegNoAsync(string RegNo);
    }
}
