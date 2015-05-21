using System.Linq;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Generate
{
    public class StudentRegNoGen : RegNoGen
    {
        private static string _identity = "STUD";
        protected override int _year { get; set; }
        protected override string _departmentCode { get; set; }
        public StudentRegNoGen(int Year, string DepartmentCode)
        {
            this._year = Year;
            this._departmentCode = DepartmentCode;
        }

        public override Task<string> GetRegNo()
        {
            return Task<string>.Factory.StartNew(() =>
            {
                long studCount = context.Students.LongCount(x => x.RegNo.Contains(_departmentCode));
                string studentCount = formartNumber(++studCount);
                string regno = _identity + "/" + _departmentCode + "/" + _year.ToString() + "/" + studentCount;
                return regno;
            });
        }

        string formartNumber(long number)
        {
            string value;
            if (number >= 0 && number < 10)
                value = "000" + number.ToString();
            else if (number >= 10 && number < 100)
                value = "00" + number.ToString();
            else if (number >= 100 && number < 1000)
                value = "0" + number.ToString();
            else
                value = number.ToString();
            return value;
        }
    }

}
