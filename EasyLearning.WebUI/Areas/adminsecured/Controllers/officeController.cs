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

        public ActionResult department(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Department model = _departmentService.GetAll().Where(x => x.Title.ToLower() == id.ToLower()).LastOrDefault();
                if (model != null)
                    return View(model);
            }
            TempData["error"] = "No such department was found";
            return RedirectToAction("departments");
        }

        public ActionResult students(int? id, int? page)
        {
            int pageSize = 20;
            int pageNumber = page ?? 1;
            if (id == null)
            {
                ViewBag.DepartmentName = "All Students";
                var AllStudents = _studentService.GetAll().OrderBy(x => x.CreatedDate);
                return View(AllStudents.ToPagedList(pageNumber, pageSize));
            }
            ViewBag.DepartmentName = _departmentService.GetAll().Where(x => x.ID == id.Value).Single().Name;
            var students = _studentService.GetAll().Where(x => x.DepartmentID == id.Value);
            return View(students.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult lecturers(string id, int? page)
        {
            int pageSize = 5;
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

        [ActionName("add-department")]
        public ViewResult AddDepartment()
        {
            ViewBag.department = "active";
            PopulateCollegeDropDownList();
            return View("AddDepartment", new Department());
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
            PopulateDepartmentDropDownList();
            return View("AddLecturer", new UserCreateModel());
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
            PopulateDepartmentDropDownList(model.DepartmentID);
            return View("AddLecturer", model);
        }

        [ActionName("add-student")]
        public ActionResult AddStudent()
        {
            return View("AddStudent", new UserCreateModel());
        }

        [ActionName("add-student")]
        [HttpPost,ValidateAntiForgeryToken]
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
                        IdentityResult addToRole = await UserManager.AddToRoleAsync(user.Id, role);
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

        bool VerifyIfCollegeExist(College model)
        {
            bool nameTest = _collegeService.GetAll().Any(x => x.Name.ToLower() == model.Name.ToLower());
            bool titleTest = _collegeService.GetAll().Any(x => x.Title.ToLower() == model.Title.ToLower());
            if (nameTest || titleTest)
                return true;
            else
                return false;
        }

        private bool VerifyDepartmentEnum(Department model)
        {
            bool duration = Enum.IsDefined(typeof(Duration), model.Duration);
            if (duration)
                return true;
            return false;
        }

        private bool VerifyUserEnum(UserCreateModel model)
        {
            bool gender = Enum.IsDefined(typeof(Sex), model.Gender);
            if (gender)
                return true;
            return false;
        }

        void PopulateDepartmentDropDownList(object selectedDepartment = null)
        {
            var department = from d in _departmentService.GetAll()
                             orderby d.Name
                             select d;
            ViewBag.DepartmentID = new SelectList(department, "ID", "Name", selectedDepartment);
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