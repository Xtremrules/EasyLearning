using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace EasyLearning.WebUI
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/*.css"));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                     "~/Scripts/modernizr-*"));
            bundles.Add(new StyleBundle("~/Content/admin").Include(
                "~/dist/css/skins/_all-skins.min.css",
                "~/dist/css/AdminLTE.min.css"));
            //BundleTable.EnableOptimizations = true;
        }
    }
}