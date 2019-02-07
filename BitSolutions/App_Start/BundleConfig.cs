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
                      "~/Content/ionrangeslider/ion.rangeSlider.css",
                      "~/Content/ionrangeslider/ion.rangeSlider.skinFlat.css",
                      "~/Content/bootstrap-switch/bootstrap-switch.min.css",
                      "~/Content/morris/morris.css",
                      "~/Content/selectize/selectize.default.css",
                      "~/Content/selectize/selectize.bootstrap3.css",
                      "~/Content/bootstrap-datepicker/bootstrap-datepicker.min.css",
                      "~/Content/bootstrap-timepicker/bootstrap-timepicker.min.css",
                      "~/Content/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css",
                      "~/Content/bootstrap-select/bootstrap-select.min.css",
                      "~/Content/datatables/dataTables.bootstrap.css",
                      "~/Content/css/right.dark.css",
                      "~/Content/css/demo.css",
                      "~/Content/own/Style.css"));

            bundles.Add(new ScriptBundle("~/bundles/BaseJS").Include(
                      "~/Scripts/own/jquery.scrollbar/jquery.scrollbar.min.js",
                      "~/Scripts/own/bootstrap-tabdrop/bootstrap-tabdrop.min.js",
                      "~/Scripts/own/jquery-sparkline/jquery.sparkline.min.js",
                      "~/Scripts/own/ionrangeslider/ion.rangeSlider.min.js",
                      "~/Scripts/own/inputNumber/inputNumber.js",
                      "~/Scripts/own/bootstrap-switch/bootstrap-switch.min.js",
                      "~/Scripts/own/raphael/raphael.min.js",
                      "~/Scripts/own/morris/morris.min.js",
                      "~/Scripts/own/selectize/selectize.min.js",
                      "~/Scripts/own/bootstrap-datepicker/bootstrap-datepicker.min.js",
                      "~/Scripts/own/bootstrap-timepicker/bootstrap-timepicker.min.js",
                      "~/Scripts/own/bootstrap-select/bootstrap-select.min.js",
                      "~/Scripts/own/template/index.js",
                      "~/Scripts/own/template/controls.js",
                      "~/Scripts/own/datatables/jquery.dataTables.js",
                      "~/Scripts/own/dataTables/bootstrap.js",
                      "~/Scripts/own/dataTables/table_data.js",
                      "~/Scripts/own/main.js",
                      "~/Scripts/own/demo.js"));

            /*File input CSS*/
            bundles.Add(new StyleBundle("~/Content/FileInputCss").Include(
                      "~/Content/jasny/jasny-bootstrap.css"));

            /*File input JS*/
            bundles.Add(new StyleBundle("~/Content/FileInputJS").Include(
                      "~/Scripts/own/jasny/jasny-bootstrap.js"));

        }
    }
}
