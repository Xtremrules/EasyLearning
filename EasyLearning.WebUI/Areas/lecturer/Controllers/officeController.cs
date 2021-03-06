﻿using System;
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
using EasyLearning.WebUI.Areas.lecturer.Models;

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
        IAssignmentService _assignmentService;

        public officeController(ICollegeService _collegeService, IDepartmentService _depertmentService,
           IStudentService _studentService, ICourseService _courseService,
           ILecturerService _lecturerService, IReplyService _replyService,
           ICommentService _commentService, IStudyService _studyService,
            IAssignmentService _assignmentService)
        {
            this._collegeService = _collegeService;
            this._departmentService = _depertmentService;
            this._studentService = _studentService;
            this._courseService = _courseService;
            this._lecturerService = _lecturerService;
            this._replyService = _replyService;
            this._commentService = _commentService;
            this._studyService = _studyService;
            this._assignmentService = _assignmentService;
        }

        public ActionResult Index()
        {
            ViewBag.dashboard = "active";
            var lecturer = _lecturerService.GetAll().First(x => x.AppUserID == User.Identity.GetUserId());
            var dashboard = new Dashboard
            {
                NumberOfCourses = lecturer.Courses.Count,
                NumberOfStudies = lecturer.Courses
                .SelectMany(x => x.Studies)
                .Where(x => x.CreatedBy == User.Identity.Name).Count(),
            };
            return View(dashboard);
        }

        public ActionResult profile()
        {
            var lecturer = _lecturerService.GetAll().First(x => x.AppUserID == User.Identity.GetUserId());
            return View(lecturer);
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
                var studies = course.Studies.Where(x => x.CreatedBy == User.Identity.Name).OrderByDescending(x => x.CreatedDate);
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
                    var study = await _studyService.GetByIdAsync(model.StudyID);
                    if (study != null)
                        return PartialView("Comments", study.Comments);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return null;
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

        public ActionResult students(string course)
        {
            if (string.IsNullOrEmpty(course))
                return RedirectToAction("courses");
            var Course = _courseService.GetAll().FirstOrDefault(x => x.CourseCode == course.Trim());
            var CourseStudent = Course.Students;
            ViewBag.Current = Course.CourseTitle;
            return View("students", CourseStudent);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddStudy(Study model, HttpPostedFileBase video = null, HttpPostedFileBase material = null)
        {
            ViewBag.studies = "active";
            if (ModelState.IsValid)
            {
                if (model.DeadLine > DateTime.Now)
                {
                    if (!_studyService.GetAll().Any(x => model.Name.Equals(x.Name, StringComparison.OrdinalIgnoreCase)
                   && x.CreatedBy == User.Identity.Name && x.CourseID == model.CourseID))
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
                            System.IO.File.Delete(Server.MapPath(model.NoteUrl));
                            System.IO.File.Delete(Server.MapPath(model.VideoUrl));
                            throw;
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "A study with the same name for this course have already been added by you");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Dead line must be at least 1 day");
                }
            }
            return View(model);
        }

        public ActionResult Assignments(int id)
        {
            var assignments = _studyService.GetAll().FirstOrDefault(x => x.ID == id).Assignments.ToList();
            ViewBag.StudyID = id;
            return View(assignments);
        }

        public ActionResult view(int id)
        {
            var assignment = _assignmentService.GetById(id);
            if (assignment != null)
            {
                return View(assignment);
            }
            return RedirectToAction("studies");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> view([Bind(Include = "Score,ID,StudyID,ContentType")]Assignment model)
        {
            var assignment = _assignmentService.GetById(model.ID);
            if (assignment != null)
            {
                assignment.Score = model.Score;
                try
                {
                    await _assignmentService.UpdateAsync(assignment);
                    TempData["success"] = "Assignment Graded successfully";
                    return RedirectToAction("Assignments", new { id = model.StudyID });
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
            return View(model);
        }

        public async Task<ActionResult> ViewAssign(int? id)
        {
            if (id != null)
            {
                var assignment = await _assignmentService.FindByIdAsync(id.Value);
                if (assignment != null)
                {
                    return File(Server.MapPath(assignment.AssignmentUrl), assignment.ContentType);
                }
            }
            return null;
        }

        public async Task<ActionResult> downloadAssignment(int? id)
        {
            if (id.HasValue)
            {
                var assign = await _assignmentService.FindByIdAsync(id.Value);
                if (assign != null)
                    return File(Server.MapPath(assign.AssignmentUrl), assign.ContentType, assign.SaveName);
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