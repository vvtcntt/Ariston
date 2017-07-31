using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ariston
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Chi-nhanh", "ListNew/{Tag}-{id}.aspx", new { controller = "NewsDisplay", action = "ListNews", tag = UrlParameter.Optional }, new { controller = "^N.*", action = "^ListNews$" });
            routes.MapRoute("ListManufacturers", "9/{Tag}.aspx", new { controller = "Agency", action = "AgencyList", tag = UrlParameter.Optional }, new { controller = "^A.*", action = "^AgencyList$" });
            routes.MapRoute("DetailManufacturers", "NhaPhanPhoi/{Tag}.aspx", new { controller = "Agency", action = "AgencyDetail", tag = UrlParameter.Optional }, new { controller = "^A.*", action = "^AgencyDetail$" });
            routes.MapRoute("ChitietNew", "News/{Tag}-{id}.aspx", new { controller = "NewsDisplay", action = "NewsDetail", tag = UrlParameter.Optional }, new { controller = "^N.*", action = "^NewsDetail$" });
            routes.MapRoute("Chi_Tiet", "Product/{Tag}-{id1}-{id}.aspx", new { controller = "Products", action = "ProductDetail", tag = UrlParameter.Optional }, new { controller = "^P.*", action = "^ProductDetail$" });
            routes.MapRoute("Danh_Sach", "ListProduct/{Tag}-{id}.aspx", new { controller = "Products", action = "ListProduct", tag = UrlParameter.Optional }, new { controller = "^P.*", action = "^ListProduct$" });
            routes.MapRoute("ListCap", "{Tag}.html", new { controller = "Products", action = "ListCapacity", tag = UrlParameter.Optional }, new { controller = "^P.*", action = "^ListCapacity$" });
            routes.MapRoute(name: "Tin-tuc", url: "Tin-tuc", defaults: new { controller = "NewDisplay", action = "ListNew" });
            routes.MapRoute(name: "Nha-phan-phoi", url: "Nha-phan-phoi", defaults: new { controller = "MenufacturersDisplay", action = "MenufacturerList" });
            routes.MapRoute(name: "Contact", url: "Contact", defaults: new { controller = "Contacts", action = "Index" });
            routes.MapRoute(name: "SearchProduct", url: "SearchProduct", defaults: new { controller = "Products", action = "SearchProduct" });
            routes.MapRoute(name: "Order", url: "Order", defaults: new { controller = "Order", action = "OrderIndex" });
            routes.MapRoute(name: "Maps", url: "Ban-do", defaults: new { controller = "MapsDisplay", action = "Index" });
            routes.MapRoute(name: "Admin", url: "Admin", defaults: new { controller = "Login", action = "LoginIndex" });
            routes.MapRoute(name: "Default", url: "{controller}/{action}/{id}", defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
