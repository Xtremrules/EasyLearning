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
    public class LecturerService : EntityService<Lecturer>, ILecturerService
    {
        public LecturerService(EasyLearningDB context)
            : base(context) { }

        public async Task<Lecturer> GetByRegNoAsync(string RegNo)
        {
            return await _dbset.FindAsync(RegNo);
        }

        public override async Task CreateAsync(Lecturer entity)
        {
            int? _year = System.DateTime.Now.Year;
            Department _department = await _context.Departments.FirstAsync(x => x.ID == entity.DepartmentID);
            string _departmentCode = _department.Title;
            LecturerRegNoGen GenRegNo = new LecturerRegNoGen(_year.Value, _departmentCode);
            entity.RegNo = await GenRegNo.GetRegNo();
            await base.CreateAsync(entity);
        }

        public async Task AssignCoursesAsync(string RegNo, string[] selectedCourses)
        {
            Lecturer lecturer = _dbset.Find(RegNo);
            HashSet<string> selectedCoursesHS;
            if (lecturer == null)
                return;
            if (selectedCourses == null)
                selectedCoursesHS = new HashSet<string>();
            else
                selectedCoursesHS = new HashSet<string>(selectedCourses);

            //var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var lecturerCourses = new HashSet<long>(lecturer.Courses.Select(x => x.ID));
            IEnumerable<Course> departmentCourses = _context.Departments.Where(x => x.ID == lecturer.DepartmentID)
                                                                        .FirstOrDefault().Courses;

            foreach (var course in departmentCourses)
            {
                if (selectedCoursesHS.Contains(course.ID.ToString()))
                {
                    if (!lecturerCourses.Contains(course.ID))
                    {
                        lecturer.Courses.Add(course);
                    }
                }
                else
                {
                    if (lecturerCourses.Contains(course.ID))
                    {
                        lecturer.Courses.Remove(course);
                    }
                }
            }

            await base.UpdateAsync(lecturer);
        }

        public override IEnumerable<Lecturer> GetAll()
        {
            return _context.Lecturers.Include(x => x.AppUser)
                .Include(x => x.Courses)
                .Include(x => x.Department)
                .ToList();
        }
    }
}
