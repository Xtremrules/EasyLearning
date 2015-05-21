using System.Linq;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Generate
{
    public class LecturerRegNoGen : RegNoGen
    {
        private static string _identity = "LECT";
        protected override int _year { get; set; }
        protected override string _departmentCode { get; set; }
        public LecturerRegNoGen(int Year, string DepartmentCode)
        {
            this._departmentCode = DepartmentCode;
            this._year = Year;
        }

        public override Task<string> GetRegNo()
        {
            return Task<string>.Factory.StartNew(() =>
            {
                long lectCount = context.Lecturers.LongCount(x => x.RegNo.Contains(_departmentCode));
                string stringLectCount = formartNumber(++lectCount);
                string result = _identity + "/" + _departmentCode + "/" + _year.ToString() + "/" + stringLectCount;
                return result;
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