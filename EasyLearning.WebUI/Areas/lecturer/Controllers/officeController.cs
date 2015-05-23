using System;
using System.Collections.Generic;
using System.Linq;
using EasyLearning.Domain.Identity;
using Microsoft.AspNet.Identity.Owin;
using EasyLearning.Domain.Abstract.Service;
using EasyLearning.Domain.Entity;
using EasyLearning.Domain.Models;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Threading.Tasks;
using System.IO;

namespace EasyLearning.WebUI.Areas.lecturer.Controllers
{
    [Authorize(Roles = Roles.Lecturer)]
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
            ViewBag.dashboard = "active";
            return View();
        }

        public ActionResult courses(int? page)
        {
            ViewBag.course = "active";
            int pageSize = 10;
            int pageNumber = page ?? 1;
            var courses = _lecturerService.GetAll().First(x => x.AppUserID == User.Identity.GetUserId()).Courses;
            return View(courses.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult study(int? id)
        {
            ViewBag.studies = "active";
            if (id != null)
            {
                var study = _studyService.GetAll().First(x => x.ID == id.Value);
                return View(study);
            }
            return RedirectToAction("studies");
        }

        public async Task<ActionResult> Studies(int? id, int? page)
        {
            ViewBag.studies = "active";
            int pageSize = 10;
            int pageNumber = page ?? 1;
            if (id != null)
            {
                var course = await _courseService.GetByIdAsync((long)id.Value);
                ViewBag.Current = course.CourseCode;
                ViewBag.CourseID = course.ID;
                var studies = course.Studies.Where(x => x.CreatedBy == User.Identity.Name);
                return View(studies.ToPagedList(pageNumber, pageSize));
            }
            ViewBag.Current = "All Studies";
            var AllStudies = _studyService.GetAll().Where(x => x.CreatedBy == User.Identity.Name);
            return View(AllStudies.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AddStudy(int? id)
        {
            if (id != null)
            {
                ViewBag.studies = "active";
                Study model = new Study { CourseID = id.Value };
                ViewBag.Current = _courseService.GetAll().First(x => x.ID == id.Value).CourseTitle;
                return View(model);
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

        public async Task<ActionResult> course(int? id)
        {
            ViewBag.course = "active";
            if (id != null)
            {
                var course = await _courseService.GetByIdAsync(id.Value);
                if (course != null)
                {
                    return View(course);
                }
            }
            return RedirectToAction("courses");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddStudy(Study model, HttpPostedFileBase video = null, HttpPostedFileBase material = null)
        {
            ViewBag.studies = "active";
            if (ModelState.IsValid)
            {
                string newVideoPath = "";
                string newMaterialPath = "";
                string StudyVideoPath = "~/Study Videos";
                string studyMaterialPath = "~/Materials";
                bool validVideo = VideoIsValid(video);
                bool materIsValid = MaterialIsValid(material);
                if (validVideo)
                {
                    if (!Directory.Exists(Server.MapPath(StudyVideoPath)))
                        Directory.CreateDirectory(Server.MapPath(StudyVideoPath));

                    var videoPath = Path.Combine(Server.MapPath(StudyVideoPath), Guid.NewGuid().ToString() + " " + video.FileName);
                    video.SaveAs(videoPath);

                    videoPath = videoPath.Substring(videoPath.LastIndexOf("\\"));
                    string[] splitVideoPath = videoPath.Split('\\');
                    newVideoPath = splitVideoPath[1];
                    newVideoPath = StudyVideoPath + "/" + newVideoPath;

                    model.VideoName = video.FileName;
                    model.VideoType = video.ContentType;
                    model.VideoUrl = newVideoPath;
                }
                if (materIsValid)
                {
                    if (!Directory.Exists(Server.MapPath(studyMaterialPath)))
                        Directory.CreateDirectory(Server.MapPath(studyMaterialPath));

                    var materialPath = Path.Combine(Server.MapPath(studyMaterialPath), Guid.NewGuid().ToString() + " " + material.FileName);
                    material.SaveAs(materialPath);

                    materialPath = materialPath.Substring(materialPath.LastIndexOf("\\"));
                    string[] splitMaterialPath = materialPath.Split('\\');
                    newMaterialPath = splitMaterialPath[1];
                    newMaterialPath = studyMaterialPath + "/" + newMaterialPath;

                    model.NoteName = material.FileName;
                    model.NoteType = material.ContentType;
                    model.NoteUrl = newMaterialPath;
                }

                try
                {
                    await _studyService.CreateAsync(model);
                    TempData["success"] = "The study was created successfully";
                    return RedirectToAction("studies", new { id = model.CourseID });
                }
                catch (Exception)
                {

                    throw;
                }
            }
            System.IO.File.Delete(Server.MapPath(model.NoteUrl));
            System.IO.File.Delete(Server.MapPath(model.VideoUrl));
            //return File(Server.MapPath(newVideoPath), video.ContentType, video.FileName);
            return View(model);
        }

        public async Task<FileContentResult> Image()
        {
            AppUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
                if (user.ImageMine != null)
                    return File(user.ImageContent, user.ImageMine);
            return null;
        }

        bool VideoIsValid(HttpPostedFileBase video)
        {
            if (video != null)
            {
                if (video.ContentType.Contains("video") && video.ContentLength > 0)
                    return true;
            }
            return false;
        }

        bool MaterialIsValid(HttpPostedFileBase material)
        {
            if (material != null)
                if (material.ContentLength > 0 && (material.FileName.Contains(".docx") || material.FileName.Contains(".pdf") || material.FileName.Contains("doc")))
                    return true;
            return false;
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

        AppUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<AppUserManager>(); }
        }
    }
}