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
using EasyLearning.Domain.Models;
using EasyLearning.WebUI.Areas.student.Models;

namespace EasyLearning.WebUI.Areas.student.Controllers
{
    [Authorize(Roles = Roles.Students)]
    public class studentController : Controller
    {
        IStudentService _studentService;
        ICourseService _courseService;
        IReplyService _replyService;
        ICommentService _commentService;
        IStudyService _studyService;
        IActivityService _activityService;

        public studentController(IActivityService _activityService,
           IStudentService _studentService, ICourseService _courseService,
            IReplyService _replyService, ICommentService _commentService,
            IStudyService _studyService)
        {
            this._activityService = _activityService;
            this._studentService = _studentService;
            this._courseService = _courseService;
            this._replyService = _replyService;
            this._commentService = _commentService;
            this._studyService = _studyService;
        }

        public ActionResult Index()
        {
            ViewBag.dashboard = "active";
            var student = _studentService.GetAll().First(x => x.AppUserID == User.Identity.GetUserId());
            var dashboard = new Dashboard
            {
                NumberOfCourses = student.Courses.Count,
                NumberOfStudies = student.Courses.SelectMany(x => x.Studies).Count(),
                NumberOfNewStudies = student.AppUser.Activities.Count
            };
            return View(dashboard);
        }

        public ActionResult profile()
        {
            var student = _studentService.GetAll().First(x => x.AppUserID == User.Identity.GetUserId());
            return View(student);
        }

        public ActionResult search(string q, int? page)
        {
            ViewBag.q = q;
            int pageSize = 10;
            int pageNumber = page ?? 1;
            var courses = _studentService.GetAll().First(x => x.AppUserID == User.Identity.GetUserId()).Courses;
            var searchResult = courses.SelectMany(x => x.Studies).Where(x => x.Name.ToLower().Contains(q.ToLower()));
            return View(searchResult.ToPagedList(pageNumber, pageSize));
        }

        public async Task<ActionResult> courses(int? page)
        {
            AppUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            ViewBag.Activites = user.Activities.ToList();
            ViewBag.course = "active";
            int pageSize = 10;
            int pageNumber = page ?? 1;
            var courses = _studentService.GetAll().First(x => x.AppUserID == user.Id).Courses;
            return View(courses.ToPagedList(pageNumber, pageSize));
        }

        public async Task<ActionResult> Studies(int? id, int? page)
        {
            if (id != null)
            {
                ViewBag.studies = "active";
                int pageSize = 10;
                int pageNumber = page ?? 1;
                await _activityService.DeleteActivity((long)id.Value, User.Identity.GetUserId());
                var course = await _courseService.GetByIdAsync((long)id.Value);
                return View(course.Studies.OrderByDescending(x => x.CreatedDate).ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("courses");
        }

        public ActionResult study(int? id)
        {
            if (id != null)
            {
                ViewBag.studies = "active";
                var study = _studyService.GetAll().First(x => x.ID == id.Value);
                return View(study);
            }
            return RedirectToAction("courses");
        }

        public async Task<ActionResult> course(int? id)
        {
            if (id != null)
            {
                ViewBag.studies = "active";
                var course = await _courseService.GetByIdAsync(id.Value);
                if (course != null)
                {
                    return View(course);
                }
            }
            return RedirectToAction("courses");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddComment(Comment model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _commentService.CreateAsync(model);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return RedirectToAction("study", new { id = model.StudyID });
        }

        public async Task<ActionResult> Video(int? id)
        {
            if (id != null)
            {
                var study = await _studyService.GetByIdAsync(id.Value);
                if (study != null)
                    return File(Server.MapPath(study.VideoUrl), study.VideoType, study.VideoName);
            }
            return null;
        }

        public async Task<ActionResult> CommentImage(string username)
        {
            AppUser user = await UserManager.FindByNameAsync(username);
            if (user != null)
                return File(user.ImageContent, user.ImageMine);
            return null;
        }

        public async Task<ActionResult> Note(int? id)
        {
            if (id != null)
            {
                var study = await _studyService.GetByIdAsync(id.Value);
                if (study != null)
                    return File(Server.MapPath(study.NoteUrl), study.NoteType, study.NoteName);
            }
            return null;
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