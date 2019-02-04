using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: null,
                url: "",
                defaults: new
                {
                    Controller = "Book",
                    Action = "List",
                    specialization = (string)null,
                    pageno = 1
                }
            );

            routes.MapRoute(
                name: null,
                url: "BookListPage{pageno}",
                defaults: new
                {
                    Controller = "Book",
                    Action = "List",
                    specialization = (string)null
                }
            );

            routes.MapRoute(
                name: null,
                url: "{specialization}",
                defaults: new
                {
                    Controller = "Book",
                    Action = "List",
                    pageno = 1
                }
            );

            routes.MapRoute(
                name: null,
                url: "{specialization}/Page{pageno}",
                defaults: new
                {
                    Controller = "Book",
                    Action = "List"
                },
                constraints: new
                {
                    pageno = @"\d+"
                }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Book", action = "List", id = UrlParameter.Optional }
            );
        }
    }
}
