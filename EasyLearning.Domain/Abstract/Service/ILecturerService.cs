using EasyLearning.Domain.Entity;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Abstract.Service
{
    public interface ILecturerService : IEntityService<Lecturer>
    {
        Task<Lecturer> GetByRegNoAsync(string RegNo);
        Task AssignCoursesAsync(string RegNo, string[] selectedCourses);
    }
}
