using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyLearning.WebUI.Areas.adminsecured.Models
{
    public class ActionModel
    {
        public string Action { get; set; }
        public string Controller
        {
            get { return "office"; }
        }
        public string Current { get; set; }
    }
}