using EasyLearning.Domain.Abstract.Service;
using EasyLearning.Domain.Concrete;
using EasyLearning.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Service
{
    public class ActivityService : EntityService<Activity>, IActivityService
    {
        public ActivityService(EasyLearningDB context)
            : base(context) { }

        public async Task DeleteActivity(long CourseID, string userId)
        {
            AppUser user = _context.Users.Find(userId);
            List<Activity> activities = user.Activities.ToList();
            foreach (var activity in activities)
            {
                if (activity.Study.CourseID == CourseID)
                    //user.Activities.Remove(activity);
                    _context.Activities.Remove(activity);
            }
            await _context.SaveChangesAsync();
        }
    }
}