using EasyLearning.Domain.Concrete;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Generate
{
    public abstract class RegNoGen
    {
        protected EasyLearningDB context = new EasyLearningDB();
        protected abstract int _year { get; set; }
        protected abstract string _departmentCode { get; set; }
        public abstract Task<string> GetRegNo();
    }
}