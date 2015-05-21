using EasyLearning.Domain.Abstract.Service;
using EasyLearning.Domain.Identity;
using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using System.Threading.Tasks;
using PagedList;
using EasyLearning.Domain.Entity;

namespace EasyLearning.WebUI.Areas.student.Controllers
{
    public class studentController : Controller
    {
        ICollegeService _collegeService;
        IDepartmentService _departmentService;
        IStudentService _studentService;
        ICourseService _courseService;
        ILecturerService _lecturerService;
        IReplyService _replyService;
        ICommentService _commentService;
        IStudyService _studyService;

        public studentController(ICollegeService _collegeService, IDepartmentService _depertmentService,
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

        public ActionResult courses(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            var courses = _studentService.GetAll().First(x => x.AppUserID == User.Identity.GetUserId()).Courses;
            return View(courses.ToPagedList(pageNumber, pageSize));
        }

        public async Task<ActionResult> Studies(int? id, int? page)
        {
            if (id != null)
            {
                int pageSize = 10;
                int pageNumber = page ?? 1;
                var course = await _courseService.GetByIdAsync((long)id.Value);
                return View(course.Studies.ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("courses");
        }

        public async Task<FileContentResult> Image()
        {
            AppUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
                if (user.ImageMine != null)
                    return File(user.ImageContent, user.ImageMine);
            return null;
        }

        AppUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<AppUserManager>(); }
        }
    }
}