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
using System.Threading.Tasks;

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
            return View();
        }

        //public ActionResult courses(int? id, int? page)
        //{
        //    int pageSize = 10;
        //    int pageNumber = page ?? 1;
        //    if (id == null)
        //    {
                
        //    }
        //}

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