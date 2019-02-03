using System.Web;
using System.Web.Optimization;

namespace BitSolutions
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            /*Boostrap*/
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap/bootstrap.min.css"));

            /*Login*/
            bundles.Add(new ScriptBundle("~/bundles/LoginJs").Include(
                      "~/Scripts/own/demo.js"));

            bundles.Add(new StyleBundle("~/Content/LoginCss").Include(
                      "~/Content/css/right.dark.css",
                      "~/Content/css/demo.css"));

            /*Base*/
            bundles.Add(new StyleBundle("~/Content/BaseCss").Include(
                      "~/Content/font-awesome/css/font-awesome.min.css",
                      "~/Content/jquery.scrollbar/jquery.scrollbar.css",
                      "~/Content/ionrangeslider/css/ion.rangeSlider.css",
                      "~/Content/ionrangeslider/css/ion.rangeSlider.skinFlat.css",
                      "~/Content/bootstrap-switch/bootstrap-switch.min.css",
                      "~/Content/morris/morris.css",
                      "~/Content/bootstrap-select/bootstrap-select.min.css",
                      "~/Content/css/right.dark.css",
                      "~/Content/css/demo.css"));

            bundles.Add(new ScriptBundle("~/bundles/BaseJS").Include(
                      "~/Scripts/own/jquery.scrollbar/jquery.scrollbar.min.js",
                      "~/Scripts/own/bootstrap-tabdrop/bootstrap-tabdrop.min.js",
                      "~/Scripts/own/jquery-sparkline/jquery.sparkline.min.js",
                      "~/Scripts/own/ionrangeslider/js/ion.rangeSlider.min.js",
                      "~/Scripts/own/inputNumber/js/inputNumber.js",
                      "~/Scripts/own/bootstrap-switch/js/bootstrap-switch.min.js",
                      "~/Scripts/own/raphael/raphael.min.js",
                      "~/Scripts/own/morris.js/morris.min.js",
                      "~/Scripts/own/bootstrap-select/js/bootstrap-select.min.js",
                      "~/Scripts/own/template/index.js",
                      "~/Scripts/own/main.js",
                      "~/Scripts/own/demo.js"));


        }
    }
}
