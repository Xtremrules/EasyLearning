using EasyLearning.Domain.Entity;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Abstract.Service
{
    public interface IDepartmentService : IEntityService<Department>
    {
        Task<Department> GetbyIdAsync(int ID);
        Task AddCourse(Course courseToAdd, Department department);
        Task AddNoneDepartmentalCourses(int DepartmentToUpdateId, int DepartmentToCopyFromId, string[] selectedCourses);
    }
}
