using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ariston.Models;
namespace Ariston.Controllers.Display.Maps
{
    public class MapsDisplayController : Controller
    {
        //
        // GET: /MapsDisplay/
        private AristonContext db = new AristonContext();

        public ActionResult Index()
        {
            var Map = db.tblMaps.First();
            ViewBag.Title = "<title>" + Map.Name + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + Map.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + Map.Name + "\" /> ";
            return View(Map);
        }

    }
}
