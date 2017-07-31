using System.Web;
using System.Web.Optimization;

namespace Ariston
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/Display/Css/Style").Include(
                 "~/Content/Display/Css/demo.css",
                 "~/Content/Display/Css/Default.css",
                 "~/Content/Display/Css/Default_Res.css",
                 "~/Content/Display/Css/News.css",
                 "~/Content/Display/Css/News_Res.css",
                 "~/Content/Display/Css/nivo-slider.css",
                 "~/Content/Display/Css/Order.css",
                 "~/Content/Display/Css/Order_Res.css",
                 "~/Content/Display/Css/Product.css",
                 "~/Content/Display/Css/Product_Res.css",
                 "~/Content/Display/Css/style.css",
                   "~/Content/Display/Css/pgwslideshow.css",
                 "~/Content/Display/Css/styles.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
