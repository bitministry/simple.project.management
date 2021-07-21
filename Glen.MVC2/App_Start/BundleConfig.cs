using System.Web;
using System.Web.Optimization;

namespace Glen.MVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/fullcalendar.min.js",
                        "~/Scripts/jquery.tmpl.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-multiselect.js",
                      "~/Scripts/bootstrapValidator.js",
                      "~/Scripts/scripts.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/themes/base/jquery-ui.min.css",
                      "~/Content/themes/base/datepicker.css",
                      "~/Content/bootstrap.css",
                      "~/Content/fullcalendar.css",
                      "~/Content/bootstrap-multiselect.css",
                      "~/Content/bootstrapValidator.css",
                      "~/Content/style.css"));


        }
    }
}
