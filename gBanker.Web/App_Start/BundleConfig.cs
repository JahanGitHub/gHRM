using System.Web;
using System.Web.Optimization;

namespace gHRM.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/gapi").Include(
                       "~/Content/ui-libs/google-chart/jsapi"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                       "~/Scripts/bootstrap.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));                        
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/jtable").Include(
                        "~/Scripts/jtable/jquery.jtable.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/validateunobtrusive").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js"
                        ));
            
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/gbUtility").Include(
                        "~/Scripts/gHRMUtilityJs.js"));

            bundles.Add(new ScriptBundle("~/bundles/searchable").Include(
           "~/Scripts/search_puggin.js"));

            bundles.Add(new ScriptBundle("~/bundles/alert").Include(
                      "~/Content/alert/js/alert.js"));

            // ================== Style ==========================
            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Scripts/jtable/themes/lightcolor/blue/jtable.min.css",
                        "~/Content/gHRM-custom.css"
                        ));
            bundles.Add(new StyleBundle("~/Bootstrap/css").Include("~/Content/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/alert").Include(
                "~/Content/alert/themes/default/theme.css",
                "~/Content/alert/css/alert.css"));


            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/resizable.css",
                        "~/Content/themes/base/selectable.css",
                        "~/Content/themes/base/accordion.css",
                        "~/Content/themes/base/autocomplete.css",
                        "~/Content/themes/base/button.css",
                        "~/Content/themes/base/dialog.css",
                        "~/Content/themes/base/slider.css",
                        "~/Content/themes/base/tabs.css",
                        "~/Content/themes/base/progressbar.css",
                        "~/Content/themes/base/theme.css",
                        "~/Content/themes/base/core.css",                      
                       "~/Content/themes/base/datepicker.css"));
            
            bundles.Add(new ScriptBundle("~/bundles/Script-calendar").Include(
                                 "~/Scripts/ScheduleManager/script-custom-calendar.js"));

            ////////////////////////////
            //bundles.Add(new StyleBundle("~/bundles/corecss").Include(
            //            "~/content/bootstrap.css",
            //            "~/content/ui-libs/font-awesome-4.3.0/fonts/font-awesome.css",
            //            "~/Content/charisma-master/css/charisma-app.css",
            //            "~/Content/charisma-master/bower_components/fullcalendar/dist/fullcalendar.css",
            //            "~/Content/charisma-master/bower_components/fullcalendar/dist/fullcalendar.print.css",
            //            "~/Content/charisma-master/bower_components/chosen/chosen.css",
            //            "~/Content/charisma-master/bower_components/colorbox/example3/colorbox.css",
            //            "~/Content/charisma-master/bower_components/responsive-tables/responsive-tables.css",
            //            "~/Content/charisma-master/bower_components/bootstrap-tour/build/css/bootstrap-tour.css",
            //            "~/Content/charisma-master/css/jquery.noty.css",
            //            "~/Content/charisma-master/css/noty_theme_default.css",
            //            "~/Content/charisma-master/css/elfinder.css",
            //            "~/Content/charisma-master/css/elfinder.theme.css",
            //            "~/Content/charisma-master/css/jquery.iphone.toggle.css",
            //            "~/Content/charisma-master/css/uploadify.css",
            //            "~/Content/charisma-master/css/animate.min.css",
            //            "~/Content/themes/base/jquery-ui.css",
            //            "~/Content/charisma-master/css/custom.css",
            //            "~/Content/alert/css/alert.css",
            //            "~/Content/alert/themes/light/theme.css",
            //            "~/Scripts/jtable/themes/lightcolor/blue/jtable.css",
            //            "~/Content/css/kendo/kendo.common.css",
            //            "~/Content/css/kendo/kendo.bootstrap.min.css",
            //            "~/Assets/css/gBanker6-custom.css"
            //    ));


            bundles.Add(new StyleBundle("~/bundles/logincorecss").Include(
                    "~/Content/charisma-master/css/bootstrap-cerulean.min.css",
                    "~/Content/charisma-master/css/charisma-app.css",
                    "~/Content/charisma-master/bower_components/bootstrap-tour/build/css/bootstrap-tour.min.css",
                    "~/content/ui-libs/font-awesome-4.3.0/fonts/font-awesome.css",
                    "~/Content/charisma-master/css/animate.min.css",
                    "~/Content/charisma-master/css/custom.css"
                ));


        }
    }
}