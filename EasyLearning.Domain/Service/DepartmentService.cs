using EasyLearning.Domain.Abstract.Service;
using EasyLearning.Domain.Concrete;
using EasyLearning.Domain.Entity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Service
{
    public class DepartmentService : EntityService<Department>, IDepartmentService
    {
        public DepartmentService(EasyLearningDB context)
            : base(context) { }

        public async Task<Department> GetbyIdAsync(int ID)
        {
            return await _dbset.FindAsync(ID);
        }

        public async Task AddCourse(Course courseToAdd, Department department)
        {
            _context.Courses.Add(courseToAdd);
            department.Courses.Add(courseToAdd);
            await base.UpdateAsync(department);
        }


        public override IEnumerable<Department> GetAll()
        {
            return _context.Departments.Include(x => x.College)
                .Include(x => x.Courses)
                .Include(x => x.Lecturers)
                .Include(x => x.Students)
                .ToList();
        }


        public async Task AddNoneDepartmentalCourses(int DepartmentToUpdateId, int DepartmentToCopyFromId, string[] selectedCourses)
        {
            Department DepartmentToUpdate = _dbset.Find(DepartmentToUpdateId);
            HashSet<string> selectedCoursesHS;
            if (DepartmentToUpdate == null)
                return;
            if (selectedCourses == null)
            {
                //DepartmentToUpdate.Courses = new List<Course>();
                ////return;
                selectedCoursesHS = new HashSet<string>();
            }
            else
            {
                selectedCoursesHS = new HashSet<string>(selectedCourses);
            }

            var CurrentCourses = new HashSet<long>(DepartmentToUpdate.Courses.Select(c => c.ID));
            Department DepartmentToCopyFrom = _dbset.Find(DepartmentToCopyFromId);
            IEnumerable<Course> PotentialCourseToCopy = DepartmentToCopyFrom.Courses.Where(x => x.DepartmentCode == DepartmentToCopyFrom.Title);

            foreach (var course in PotentialCourseToCopy)
            {
                if (selectedCoursesHS.Contains(course.ID.ToString()))
                {
                    if (!CurrentCourses.Contains(course.ID))
                    {
                        DepartmentToUpdate.Courses.Add(course);
                    }
                }
                else
                {
                    if (CurrentCourses.Contains(course.ID))
                    {
                        DepartmentToUpdate.Courses.Remove(course);
                    }
                }

                await base.UpdateAsync(DepartmentToUpdate);
            }
        }
    }
}
