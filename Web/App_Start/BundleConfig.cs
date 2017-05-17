using System.Collections.Generic;
using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles, ICollection<string> areas)
        {
            bundles.Add(new StyleBundle("~/bundles/css/shared").Include(
                "~/Scripts/boostrapv3/css/bootstrap.min.css",
                "~/Content/jquery.dataTables.css",
                "~/Scripts/jquery-notifications/css/messenger.css",
                "~/Scripts/jquery-notifications/css/messenger-theme-future.css"
                )
                );
            
            bundles.Add(new ScriptBundle("~/bundles/js/shared").Include(
                "~/Scripts/jquery-1.11.3.js",
                "~/Scripts/jquery.ui.widget.js",
                "~/Scripts/boostrapv3/js/bootstrap.min.js",
                "~/Scripts/underscore.js",
                "~/Scripts/jquery.dataTables.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery-notifications/js/messenger.min.js",
                "~/Scripts/validator.js",
                "~/Scripts/validationRules.js",

                "~/Scripts/app.js",
                "~/Areas/Books/Scripts/dynamicList.js",
                "~/Areas/Books/Scripts/bookImage.js",
                "~/Areas/Books/Scripts/bookControl.js",
                "~/Areas/Books/Scripts/service.js",
                "~/Areas/Books/Scripts/page.js"                
                ));
        }
    }
}