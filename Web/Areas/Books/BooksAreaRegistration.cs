using System.Web.Mvc;

namespace Web.Areas.Books
{
    public sealed class BooksAreaRegistration : AreaRegistration
    {
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Books_default",
                "Books/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }

        public override string AreaName
        {
            get
            {
                return "Books";
            }
        }
    }
}
