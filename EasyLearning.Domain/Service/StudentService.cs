using EasyLearning.Domain.Abstract.Service;
using EasyLearning.Domain.Concrete;
using EasyLearning.Domain.Entity;
using EasyLearning.Domain.Generate;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Service
{
    public class StudentService : EntityService<Student>, IStudentService
    {
        public StudentService(EasyLearningDB context)
            : base(context) { }

        public async Task<Student> GetByRegNoAsync(string RegNo)
        {
            return await _dbset.FindAsync(RegNo);
        }

        public override async Task CreateAsync(Student entity)
        {
            int _year = System.DateTime.Now.Year;
            Department department = await _context.Departments.FirstAsync(x => x.ID == entity.DepartmentID);
            string _departmentCode = department.Title;
            entity.RegNo = await new StudentRegNoGen(_year, _departmentCode).GetRegNo();
            var courses = department.Courses.Where(x => x.Level == entity.Level);
            entity.Courses = new List<Course>();
            foreach (var course in courses)
                entity.Courses.Add(course);
            await base.CreateAsync(entity);
        }

        public override IEnumerable<Student> GetAll()
        {
            return _context.Students.Include(x => x.AppUser)
                .Include(x => x.Courses)
                .Include(x => x.Department)
                .ToList();
        }
    }
}
