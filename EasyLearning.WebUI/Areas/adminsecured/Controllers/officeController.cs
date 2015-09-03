using EasyLearning.Domain.Abstract.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using EasyLearning.Domain.Entity;
using System.Web;
using EasyLearning.Domain.Identity;
using PagedList;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Web.Helpers;
using EasyLearning.WebUI.Areas.adminsecured.Models;
using EasyLearning.Domain.Models;

namespace EasyLearning.WebUI.Areas.adminsecured.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class officeController : Controller
    {
        ICollegeService _collegeService;
        IDepartmentService _departmentService;
        IStudentService _studentService;
        ICourseService _courseService;
        ILecturerService _lecturerService;
        IReplyService _replyService;
        ICommentService _commentService;
        IStudyService _studyService;

        public officeController(ICollegeService _collegeService, IDepartmentService _depertmentService,
            IStudentService _studentService, ICourseService _courseService,
            ILecturerService _lecturerService, IReplyService _replyService,
            ICommentService _commentService, IStudyService _studyService)
        {
            this._collegeService = _collegeService;
            this._departmentService = _depertmentService;
            this._studentService = _studentService;
            this._courseService = _courseService;
            this._lecturerService = _lecturerService;
            this._replyService = _replyService;
            this._commentService = _commentService;
            this._studyService = _studyService;
        }

        public ActionResult Index()
        {
            Dashboard dashboard = new Dashboard
            {
                NumberOfColleges = _collegeService.GetAll().Count(),
                NumberOfCourses = _courseService.GetAll().Count(),
                NumberOfDepartment = _departmentService.GetAll().Count(),
                NumberOfLecturers = _lecturerService.GetAll().Count(),
                NumberOfStudents = _studentService.GetAll().Count(),
                NumberOfStudies = _studyService.GetAll().Count()
            };
            return View(dashboard);
        }

        public async Task<ActionResult> profile()
        {
            AppUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            return View(user);
        }

        public ActionResult upload()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> upload(HttpPostedFileBase image = null)
        {
            if (VerifyImage(image))
            {
                AppUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    bool success = await ModifyUserImage(user.Id, image);
                    if (success)
                        return RedirectToAction("profile");
                }
            }
            return View();
        }

        public ActionResult colleges(string id, int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;
            if (string.IsNullOrEmpty(id))
            {
                var colleges = _collegeService.GetAll();
                return View(colleges.ToPagedList(pageNumber, pageSize));
            }
            College college = _collegeService.GetAll().Where(x => x.Title.ToLower() == id.ToLower()).SingleOrDefault();
            if (college != null)
                return View("college", college);
            else
            {
                ModelState.Clear();
                TempData["error"] = "The college wasn't found";
                return RedirectToAction("colleges", new { id = "" });
            }
        }

        public ActionResult departments(string id, int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;
            if (!string.IsNullOrEmpty(id))
            {
                var collegeDepartment = _collegeService.GetAll().Where(x => x.Title == id).SingleOrDefault();
                ViewBag.current = id;
                if (collegeDepartment != null)
                    return View(collegeDepartment.Departments.ToPagedList(pageNumber, pageSize));
                TempData["error"] = string.Format("The College: {0}, Wast found", id);
                return RedirectToAction("department", new { id = "" });
            }
            ViewBag.current = "All Colleges";
            return View(_departmentService.GetAll().ToPagedList(pageNumber, pageSize));
        }

        public ActionResult department(string departCode)
        {
            if (!string.IsNullOrEmpty(departCode))
            {
                Department model = _departmentService.GetAll().Where(x => x.Title.ToLower() == departCode.ToLower()).LastOrDefault();
                if (model != null)
                    return View(model);
            }
            TempData["error"] = "No such department was found";
            return RedirectToAction("departments");
        }

        public ActionResult students(string id, int? page)
        {
            int pageSize = 20;
            int pageNumber = page ?? 1;
            if (id == null)
            {
                ViewBag.DepartmentName = "All";
                var AllStudents = _studentService.GetAll().OrderBy(x => x.CreatedDate);
                return View(AllStudents.ToPagedList(pageNumber, pageSize));
            }
            ViewBag.DepartmentName = _departmentService.GetAll().Where(x => x.Title == id).Single().Name;
            var students = _departmentService.GetAll().First(x => x.Title == id).Students;
            return View(students.ToPagedList(pageNumber, pageSize));
        }

        [ActionName("student-courses")]
        public async Task<ActionResult> StudentCourses(string RegNo, int? page)
        {
            if (RegNo != null)
            {
                Student student = await _studentService.GetByRegNoAsync(RegNo);
                if (student != null)
                {
                    int pageSize = 12;
                    int pageNumber = page ?? 1;
                    ViewBag.Current = student.AppUser.FullName;
                    return View("courses", student.Courses.ToPagedList(pageNumber, pageSize));
                }
            }
            TempData["error"] = "No such student was found";
            return RedirectToAction("students");
        }

        public ActionResult lecturers(string id, int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.current = "All";
                var allLecturers = _lecturerService.GetAll().OrderBy(x => x.Department.Name);
                return View(allLecturers.ToPagedList(pageNumber, pageSize));
            }
            var lecturer = _departmentService.GetAll().Where(x => x.Title.ToLower() == id.ToLower()).Single().Lecturers;
            ViewBag.current = _departmentService.GetAll().First(x => x.Title.ToLower() == id.ToLower()).Name + " Department";
            return View(lecturer.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult lecturer()
        {
            return RedirectToAction("lecturers");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> lecturer(string RegNo)
        {
            if (!string.IsNullOrEmpty(RegNo))
            {
                Lecturer lecturer = await _lecturerService.GetByRegNoAsync(RegNo);
                if (lecturer != null)
                    return View(lecturer);
            }
            TempData["error"] = "No Lecturer with such RegNo was found";
            return RedirectToAction("lecturers");
        }

        [ActionName("lecturer-courses")]
        public async Task<ActionResult> LecturerCourses(string RegNo, int? page)
        {
            Lecturer lecturer = await _lecturerService.GetByRegNoAsync(RegNo);
            if (lecturer != null)
            {
                int pageSize = 20;
                int pageNumber = page ?? 1;
                ViewBag.Current = lecturer.AppUser.FullName;
                return View("Courses", lecturer.Courses.ToPagedList(pageNumber, pageSize));
            }
            TempData["error"] = "Lecturer not found";
            return RedirectToAction("lecturers");
        }

        public ActionResult courses(string id, int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            if (id == null)
            {
                IEnumerable<Course> AllCourses = _courseService.GetAll().OrderBy(x => x.CourseCode);
                ViewBag.courses = "active";
                ViewBag.Current = "All Courses";
                return View(AllCourses.ToPagedList(pageNumber, pageSize));
            }
            Department department = _departmentService.GetAll().First(x => x.Title == id);
            var DepartmentCourses = department.Courses.OrderBy(x => x.CourseCode);
            ViewBag.Current = department.Name;
            return View(DepartmentCourses.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult course(int id)
        {
            ViewBag.courses = "active";
            var course = _courseService.GetAll().First(x => x.ID == id);
            ViewBag.DepartmentCode = course.DepartmentCode;
            return View(course);
        }

        [ActionName("add-course")]
        public ActionResult AddCourse()
        {
            ViewBag.courses = "active";
            PopulateDepartmentDropDownList();
            return View("AddCourse", new AddCourseViewModel());
        }

        [ActionName("add-course")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCourse(AddCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (VerifyEnumData(model))
                {
                    if (!VerifyIfCourseExist(model))
                    {
                        try
                        {
                            Department _department = await _departmentService.GetbyIdAsync(model.DepartmentID);
                            Course course = new Course
                            {
                                ID = model.ID,
                                DepartmentCode = _department.Title,
                                CourseTitle = model.CourseTitle,
                                Level = model.Level,
                                CourseCode = model.CourseCode,
                                CreditLoad = model.CreditLoad,
                                Semester = model.Semester,
                            };
                            await _departmentService.AddCourse(course, _department);
                            TempData["success"] = "The Course has been created";
                            return RedirectToAction("courses");
                        }
                        catch (Exception)
                        {
                            TempData["error"] = "Sorry error occurred Try again latter";
                            //ModelState.AddModelError("", ex);
                        }
                    }
                    else
                        TempData["error"] = String.Format("Either the Course Code {0} or its Title {1} has already been Added", model.CourseCode, model.CourseTitle);
                }
                else
                    TempData["error"] = "Both Semester and Level Must be Selected";
            }
            ViewBag.courses = "active";
            PopulateDepartmentDropDownList();
            return View("AddCourse", model);
        }

        [ActionName("add-college")]
        public ViewResult AddCollege()
        {
            ViewBag.college = "active";
            return View("AddCollege", new College());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("add-college")]
        public async Task<ActionResult> AddCollege(College model)
        {
            ViewBag.college = "active";
            if (ModelState.IsValid)
            {
                if (!VerifyIfCollegeExist(model))
                {
                    try
                    {
                        model.Title = model.Title.ToUpper();
                        await _collegeService.CreateAsync(model);
                        TempData["success"] = "The college was successfully created";
                        return RedirectToAction("colleges");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
                else
                    TempData["error"] = string.Format("Either the College: {0} or Title: {1}, has already been added", model.Name, model.Title);
            }
            return View("AddCollege", model);
        }

        public async Task<PartialViewResult> EditCollege(int? id)
        {
            ViewBag.currentAction = "EditCollege";
            College college = await _collegeService.GetByIdAsync(id.Value);
            EditViewModel model = new EditViewModel
            {
                Id = college.ID,
                Name = college.Name,
                Title = college.Title,
            };
            return PartialView("EditModel", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<PartialViewResult> EditCollege([Bind(Include = "Id,Name,Title")]EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                College college = await _collegeService.GetByIdAsync(model.Id);
                if (college != null)
                {
                    college.Title = model.Title;
                    college.Name = model.Name;
                    await _collegeService.UpdateAsync(college);
                    return null;
                }
                else
                    ModelState.AddModelError("", "The College wasn't found");
            }
            ViewBag.currentAction = "EditCollege";
            return PartialView("EditModel", model);
        }

        public async Task<PartialViewResult> EditDepartment(int? id)
        {
            ViewBag.currentAction = "EditDepartment";
            var department = await _departmentService.GetbyIdAsync(id.Value);
            var model = new EditViewModel
            {
                Id = department.ID,
                Name = department.Name,
                Title = department.Title
            };
            return PartialView("EditModel", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<PartialViewResult> EditDepartment([Bind(Include = "Id,Name,Title")]EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var department = await _departmentService.GetbyIdAsync(model.Id);
                if (department != null)
                {
                    department.Name = model.Name;
                    department.Title = model.Title;
                    await _departmentService.UpdateAsync(department);
                    return null;
                }
                else ModelState.AddModelError("", "The Department wasn't found");
            }
            ViewBag.currentAction = "EditDepartment";
            return PartialView("EditModel", model);
        }

        [ActionName("add-department")]
        public ViewResult AddDepartment()
        {
            ViewBag.department = "active";
            PopulateCollegeDropDownList();
            return View("AddDepartment", new Department());
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteDepartment([Bind(Include = "ID")]int ID)
        {
            var department = await _departmentService.GetbyIdAsync(ID);
            if (department != null)
            {
                try
                {
                    await _departmentService.DeleteAsync(department);
                    TempData["success"] = "The department was successfully Deleted";
                    return RedirectToAction("departments");
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
            TempData["error"] = "Sorry, Try again later";
            return RedirectToAction("departments");
        }

        [ValidateAntiForgeryToken]
        [ActionName("add-department"), HttpPost]
        public async Task<ActionResult> AddDepartment(Department model)
        {
            if (ModelState.IsValid)
            {
                if (VerifyDepartmentEnum(model))
                {
                    if (!VerifyIfDepartmentExist(model))
                    {
                        try
                        {
                            model.Title = model.Title.ToUpper();
                            await _departmentService.CreateAsync(model);
                            College college = await _collegeService.GetByIdAsync(model.CollegeID);
                            TempData["success"] = string.Format("The Department {0} has been created and successfully added to {1}", model.Name, college.Name);
                            return RedirectToAction("departments", new { id = college.Title });
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", ex.Message);
                        }
                    }
                    else
                        TempData["error"] = String.Format("Either the Department: {0} or the Title {1} has already been added", model.Name, model.Title);
                }
                else TempData["error"] = "You must Select Duration for this Department";
            }
            PopulateCollegeDropDownList(model.CollegeID);
            return View("AddDepartment", model);
        }

        [ActionName("add-lecturer")]
        public ViewResult AddLecturer()
        {
            ViewBag.Action = new ActionModel { Action = "add-lecturer", Current = "Lecturer" };
            PopulateDepartmentDropDownList();
            return View("CreateUserView", new UserCreateModel());
        }

        [ActionName("add-lecturer")]
        [ValidateAntiForgeryToken, HttpPost]
        public async Task<ActionResult> AddLecturer(UserCreateModel model, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (VerifyUserEnum(model))
                {
                    if (VerifyImage(image))
                    {
                        AppUser user = await CreateUserAsync(model, Roles.Lecturer, image);
                        if (user != null)
                        {
                            Lecturer lecturer = new Lecturer
                            {
                                AppUserID = user.Id,
                                DepartmentID = model.DepartmentID,
                            };
                            try
                            {
                                await _lecturerService.CreateAsync(lecturer);
                                Department department = await _departmentService.GetbyIdAsync(model.DepartmentID);
                                TempData["success"] = string.Format("The Lecturer {0} has been created and added to {1} Department", user.FullName, department.Name);
                                return RedirectToAction("lecturers", new { id = department.Title });
                            }
                            catch (NullReferenceException)
                            {
                                ModelState.AddModelError("", "Sorry no Academic session created yet");
                            }
                            catch (Exception ex)
                            {
                                ModelState.AddModelError("", ex.Message);
                            }
                            IdentityResult del = await UserManager.DeleteAsync(user);
                        }
                    }
                }
                else TempData["error"] = "You Must select a gender";
            }
            ViewBag.Action = new ActionModel { Action = "add-lecturer", Current = "Lecturer" };
            PopulateDepartmentDropDownList(model.DepartmentID);
            return View("CreateUserView", model);
        }

        [ActionName("add-student")]
        public ActionResult AddStudent()
        {
            PopulateDepartmentDropDownList();
            ViewBag.Action = new ActionModel { Action = "add-student", Current = "Student" };
            return View("CreateUserView", new UserCreateModel());
        }

        [ActionName("add-student")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddStudent(UserCreateModel model, Level Level, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (VerifyUserEnum(model))
                {
                    if (verifyLevelEnum(Level))
                    {
                        if (VerifyImage(image))
                        {
                            AppUser user = await CreateUserAsync(model, Roles.Students, image);
                            if (user != null)
                            {
                                Student student = new Student
                                {
                                    AppUserID = user.Id,
                                    DepartmentID = model.DepartmentID,
                                    Level = Level
                                };
                                try
                                {
                                    await _studentService.CreateAsync(student);
                                    Department department = await _departmentService.GetbyIdAsync(model.DepartmentID);
                                    TempData["success"] = string.Format("The Student {0} has been created and added to {1} Department", user.FullName, department.Name);
                                    return RedirectToAction("students", new { id = department.Title });
                                }
                                catch (Exception ex)
                                {
                                    ModelState.AddModelError("", ex.Message);
                                }
                                IdentityResult del = await UserManager.DeleteAsync(user);
                            }
                        }

                    }
                    else TempData["error"] = "You must select a level";
                }
                else TempData["error"] = "You Must select a gender";
            }
            ViewBag.Action = new ActionModel { Action = "add-student", Current = "Student" };
            PopulateDepartmentDropDownList(model.DepartmentID);
            return View("CreateUserView", model);
        }

        [ActionName("add-drop-courses")]
        public ActionResult AddDrop(string current, int? page)
        {
            if (current != null)
            {
                int pageSize = 10;
                int pageNumber = page ?? 1;
                IEnumerable<Department> departments = _departmentService.GetAll().Where(x => x.Title != current);
                List<Department> model = new List<Department>();
                ViewBag.Current = current;
                foreach (var department in departments)
                {
                    department.Courses = department.Courses.Where(x => x.DepartmentCode == department.Title).ToList();
                    model.Add(department);
                }
                return View("AddDrop", model.ToPagedList(pageNumber, pageSize));
            }
            TempData["error"] = "Sorry You must assess this page from the department";
            return RedirectToAction("departments");
        }

        [ActionName("add-drop")]
        public ActionResult AddDrop(string id, string depart)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var department = _departmentService.GetAll().Where(x => x.Title == id).SingleOrDefault();
                if (department != null)
                {
                    PopulateDepartmentAssignedCourses(department, depart);
                    ViewBag.Current = depart;
                    return View("NonCourses", department);
                }
                else TempData["error"] = "Department Not found";
            }
            else TempData["error"] = "You must select a department";
            return RedirectToAction("departments");
        }

        [ActionName("add-drop")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddDrop(string Title, string Current, string[] selectedCourses)
        {
            if (!string.IsNullOrEmpty(Title))
            {
                var _department = _departmentService.GetAll().First(x => x.Title == Current);
                Department department = _departmentService.GetAll().Where(x => x.Title.ToLower() == Title.Trim().ToLower()).SingleOrDefault();
                if (department != null)
                {
                    try
                    {
                        await _departmentService.AddNoneDepartmentalCourses(_department.ID, department.ID, selectedCourses);
                        TempData["success"] = "Operation was successful";
                        return RedirectToAction("courses", new { id = _department.Title });
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                else TempData["error"] = "Department Not found";
            }
            else TempData["error"] = "You must select a department";
            return RedirectToAction("departments");
        }

        [ActionName("assign-courses")]
        public ActionResult AssignCourses(string RegNo)
        {
            if (!string.IsNullOrEmpty(RegNo))
            {
                Lecturer lecturer = _lecturerService.GetAll().First(x => x.RegNo == RegNo);
                if (lecturer != null)
                {
                    PopulateLecturerAssignedCourses(lecturer);
                    return View("AssignCourses", lecturer);
                }
                else TempData["error"] = "No such lecturer found";
            }
            else TempData["error"] = "Must select a lecturer";
            return RedirectToAction("lecturers");
        }

        [ActionName("assign-courses")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignCourses(string RegNo, string[] selectedCourses)
        {
            if (!string.IsNullOrEmpty(RegNo))
            {
                Lecturer lecturer = await _lecturerService.GetByRegNoAsync(RegNo);
                if (lecturer != null)
                {
                    try
                    {
                        await _lecturerService.AssignCoursesAsync(RegNo, selectedCourses);
                        TempData["success"] = "Operation was successful";
                        return RedirectToAction("lecturer-courses", new { RegNo = lecturer.RegNo });
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    TempData["error"] = "Lecturer Not found";
                    return RedirectToAction("lecturers");
                }
            }
            else
            {
                TempData["error"] = "Must select a lecturer";
                return RedirectToAction("lecturers");
            }
        }

        async Task<AppUser> CreateUserAsync(UserCreateModel model, string role, HttpPostedFileBase image)
        {
            AppUser user = new AppUser
            {
                UserName = model.username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,
                DateOfBirth = model.DateOfBirth,
                State = model.State,
                ImageMine = image.ContentType,
            };

            #region Resize Image
            byte[] buffer = new byte[image.ContentLength];
            await image.InputStream.ReadAsync(buffer, 0, image.ContentLength);
            WebImage resizeImage = new WebImage(buffer);
            resizeImage.Crop(1, 1).Resize(200, 200, false, true);
            user.ImageContent = resizeImage.GetBytes();
            #endregion

            IdentityResult validPassword = await UserManager.PasswordValidator.ValidateAsync(model.Password);
            if (validPassword.Succeeded)
            {
                IdentityResult createUser = await UserManager.CreateAsync(user, model.Password);
                if (createUser.Succeeded)
                {
                    if (!await RoleManager.RoleExistsAsync(role))
                    {
                        IdentityResult roleCreate = await RoleManager.CreateAsync(new AppRole(role));
                        if (!await RoleManager.RoleExistsAsync(Roles.Study))
                            await RoleManager.CreateAsync(new AppRole(Roles.Study));
                        if (!roleCreate.Succeeded)
                        {
                            AddErrorsFromResult(roleCreate);
                            IdentityResult dell = await UserManager.DeleteAsync(user);
                            return null;
                        }
                    }
                    else
                    {
                        user = await UserManager.FindByNameAsync(model.username);
                        IdentityResult addToRole = await UserManager.AddToRolesAsync(user.Id, role, Roles.Study);
                        if (addToRole.Succeeded)
                            return user;
                        else
                        {
                            IdentityResult del = await UserManager.DeleteAsync(user);
                            AddErrorsFromResult(addToRole);
                        }
                    }
                }
                else
                    AddErrorsFromResult(createUser);
            }
            else
                AddErrorsFromResult(validPassword);
            return null;
        }

        void AddErrorsFromResult(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);
        }

        async Task<bool> ModifyUserImage(string userId, HttpPostedFileBase image)
        {
            AppUser user = await UserManager.FindByIdAsync(userId);
            byte[] buffer = new byte[image.ContentLength];
            await image.InputStream.ReadAsync(buffer, 0, image.ContentLength);
            WebImage resizeImage = new WebImage(buffer);
            resizeImage.Crop(1, 1).Resize(200, 200, false, true);
            user.ImageMine = image.ContentType;
            user.ImageContent = resizeImage.GetBytes();
            IdentityResult update = await UserManager.UpdateAsync(user);
            if (update.Succeeded)
                return true;
            AddErrorsFromResult(update);
            return false;
        }

        bool VerifyImage(HttpPostedFileBase image)
        {
            if (image != null)
            {
                if (image.ContentType.Contains("image"))
                    return true;
                ModelState.AddModelError("", "File is not an image");
                return false;
            }
            ModelState.AddModelError("", "You Must select an image");
            return false;
        }

        bool VerifyIfDepartmentExist(Department model)
        {
            bool nameTest = _departmentService.GetAll().Any(x => x.Name.ToLower() == model.Name.ToLower());
            bool titleTest = _departmentService.GetAll().Any(x => x.Title.ToLower() == model.Title.ToLower());
            if (nameTest || titleTest)
                return true;
            else
                return false;
        }

        bool VerifyEnumData(AddCourseViewModel model)
        {
            bool semester = Enum.IsDefined(typeof(Semester), model.Semester);
            bool level = Enum.IsDefined(typeof(Level), model.Level);
            if (semester && level)
                return true;
            return false;
        }

        bool VerifyIfCollegeExist(College model)
        {
            bool nameTest = _collegeService.GetAll().Any(x => x.Name.ToLower() == model.Name.ToLower());
            bool titleTest = _collegeService.GetAll().Any(x => x.Title.ToLower() == model.Title.ToLower());
            if (nameTest || titleTest)
                return true;
            else
                return false;
        }

        bool VerifyDepartmentEnum(Department model)
        {
            bool duration = Enum.IsDefined(typeof(Duration), model.Duration);
            if (duration)
                return true;
            return false;
        }

        Boolean VerifyIfCourseExist(AddCourseViewModel model)
        {
            bool code = _courseService.GetAll().Any(x => x.CourseCode.ToLower() == model.CourseCode.Trim().ToLower());
            bool title = _courseService.GetAll().Any(x => x.CourseTitle.ToLower() == model.CourseTitle.Trim().ToLower());
            if (code || title)
                return true;
            return false;
        }

        bool VerifyUserEnum(UserCreateModel model)
        {
            bool gender = Enum.IsDefined(typeof(Sex), model.Gender);
            if (gender)
                return true;
            return false;
        }

        bool verifyLevelEnum(Level Level)
        {
            if (Enum.IsDefined(typeof(Level), Level))
                return true;
            return false;
        }

        public async Task<FileContentResult> LecturerImage(string RegNo)
        {
            Lecturer lecturer = await _lecturerService.GetByRegNoAsync(RegNo);
            if (lecturer != null)
            {
                if (lecturer.AppUser.ImageMine != null)
                    return File(lecturer.AppUser.ImageContent, lecturer.AppUser.ImageMine);
            }
            return null;
        }

        void PopulateDepartmentDropDownList(object selectedDepartment = null)
        {
            var department = from d in _departmentService.GetAll()
                             orderby d.Name
                             select d;
            ViewBag.DepartmentID = new SelectList(department, "ID", "Name", selectedDepartment);
        }

        void PopulateLecturerAssignedCourses(Lecturer lecture)
        {
            var _department = _departmentService.GetAll().First(x => x.ID == lecture.DepartmentID);
            IEnumerable<Course> AllCourses = _department.Courses.Where(x => x.DepartmentCode == _department.Title);
            var lecturerCourses = new HashSet<long>(lecture.Courses.Select(c => c.ID));
            var ViewModel = new List<AssignedCourseData>();
            foreach (var course in AllCourses)
            {
                ViewModel.Add(new AssignedCourseData
                {
                    CourseID = course.ID,
                    Title = course.CourseTitle,
                    Assigned = lecturerCourses.Contains(course.ID),
                    CourseCode = course.CourseCode,
                    Unit = course.CreditLoad,
                });
            }
            ViewBag.Courses = ViewModel;
        }

        void PopulateDepartmentAssignedCourses(Department department, string current)
        {
            var CurrentDepartment = _departmentService.GetAll().First(x => x.Title == current);
            IEnumerable<Course> AllCourses = department.Courses.Where(c => c.DepartmentCode == department.Title);
            var CurrentDepartmentCourses = new HashSet<long>(CurrentDepartment.Courses.Select(c => c.ID));
            var ViewModel = new List<AssignedCourseData>();
            foreach (var course in AllCourses)
            {
                ViewModel.Add(new AssignedCourseData
                {
                    CourseCode = course.CourseCode,
                    CourseID = course.ID,
                    Title = course.CourseTitle,
                    Assigned = CurrentDepartmentCourses.Contains(course.ID),
                    Unit = course.CreditLoad,
                });
            }
            ViewBag.Courses = ViewModel;
        }

        public async Task<FileContentResult> Image()
        {
            AppUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
                if (user.ImageMine != null)
                    return File(user.ImageContent, user.ImageMine);
            return null;
        }

        void PopulateCollegeDropDownList(object selectedCollege = null)
        {
            var collegeList = from c in _collegeService.GetAll() orderby c.Name select c;
            ViewBag.CollegeID = new SelectList(collegeList, "ID", "Name", selectedCollege);
        }

        AppUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<AppUserManager>(); }
        }

        AppRoleManager RoleManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>(); }
        }
    }
}