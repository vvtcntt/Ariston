using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ariston.Models;
using PagedList;
using PagedList.Mvc;
namespace Ariston.Controllers.Display.News
{
    public class NewsDisplayController : Controller
    {
        //
        // GET: /NewsDisplay/
        private AristonContext db = new AristonContext();

        public ActionResult Index()
        {
            return View();
        }
        string nUrl = "";
        public string UrlNews(int idCate)
        {
            var ListMenu = db.tblGroupNews.Where(p => p.id == idCate).ToList();
            for (int i = 0; i < ListMenu.Count; i++)
            {
                nUrl = " <a href=\"/ListNews/" + ListMenu[i].Tag + "-" + ListMenu[i].id + ".aspx\" title=\"" + ListMenu[i].Name + "\"> " + " " + ListMenu[i].Name + "</a> <i></i>" + nUrl;
                string ids = ListMenu[i].ParentID.ToString();
                if (ids != null && ids != "")
                {
                    int id = int.Parse(ListMenu[i].ParentID.ToString());
                    UrlNews(id);
                }

            }
            return nUrl;
        }
        public ActionResult ListNews(string tag, int? page)
        {
 
            tblGroupNew groupnew = db.tblGroupNews.First(p => p.Tag == tag);
            int idCate = groupnew.id;
            var listNews = db.tblNews.Where(p => p.idCate == idCate && p.Active == true).OrderByDescending(p => p.Ord).ToList();

            var Countlistnew = db.tblGroupNews.Where(p => p.ParentID == idCate && p.Active == true).OrderBy(p => p.Ord).ToList();
            if (Countlistnew.Count > 1)
            {

                for (int i = 0; i < Countlistnew.Count; i++)
                {
                    int idMenus = int.Parse(Countlistnew[i].id.ToString());
                    listNews = db.tblNews.Where(p => p.idCate == idMenus && p.Active == true).OrderByDescending(p => p.Ord).ToList();
                }

            }


            ViewBag.Name = groupnew.Name;
            ViewBag.Title = "<title>" + groupnew.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + groupnew.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + groupnew.Keyword + "\" /> ";

            ViewBag.nUrl = "<a href=\"http://BinhnuocnonglanhAriston.vn\" title=\"Trang chu\" rel=\"nofollow\"><span class=\"iCon\"></span>Trang chủ</a> /" + UrlNews(idCate);
            const int pageSize = 12;
            var pageNumber = (page ?? 1);
            // Thiết lập phân trang
            var ship = new PagedListRenderOptions
            {
                DisplayLinkToFirstPage = PagedListDisplayMode.Always,
                DisplayLinkToLastPage = PagedListDisplayMode.Always,
                DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                DisplayLinkToNextPage = PagedListDisplayMode.Always,
                DisplayLinkToIndividualPages = true,
                DisplayPageCountAndCurrentLocation = false,
                MaximumPageNumbersToDisplay = 5,
                DisplayEllipsesWhenNotShowingAllPageNumbers = true,
                EllipsesFormat = "&#8230;",
                LinkToFirstPageFormat = "Trang đầu",
                LinkToPreviousPageFormat = "«",
                LinkToIndividualPageFormat = "{0}",
                LinkToNextPageFormat = "»",
                LinkToLastPageFormat = "Trang cuối",
                PageCountAndCurrentLocationFormat = "Page {0} of {1}.",
                ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.",
                FunctionToDisplayEachPageNumber = null,
                ClassToApplyToFirstListItemInPager = null,
                ClassToApplyToLastListItemInPager = null,
                ContainerDivClasses = new[] { "pagination-container" },
                UlElementClasses = new[] { "pagination" },
                LiElementClasses = Enumerable.Empty<string>()
            };
            ViewBag.ship = ship;
            string coppy = "";
            var tblconfig = db.tblConfigs.First();
            if (tblconfig.Coppy == true)
            {
                coppy += "<script src=\"/Scripts/disable-copyright.js\"></script>";
                coppy += "<link href=\"/Content/Display/Css/Coppy.css\" rel=\"stylesheet\" />";
            }
            ViewBag.coppy = coppy;
            return View(listNews.ToPagedList(pageNumber, pageSize));

        }
        public ActionResult NewsDetail(string tag,string id)
        {

            int nId = int.Parse(id);
            tblNew tblnew = db.tblNews.First(p => p.id == nId);
            string chuoinew = "";
            var listnew = db.tblNews.Where(p => p.idCate == tblnew.idCate && p.id != tblnew.id && p.Active == true).OrderByDescending(p => p.Ord).Take(3).ToList();
            for (int i = 0; i < listnew.Count; i++)
            {
                chuoinew += "<a href=\"/News/" + listnew[i].Tag + "-" + listnew[i].id + ".aspx\" title=\"" + listnew[i].Name + "\"> - " + listnew[i].Name + "</a>";


            }
            ViewBag.chuoinew = chuoinew;
            ViewBag.Title = "<title>" + tblnew.Title + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + tblnew.Title + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblnew.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblnew.Keyword + "\" /> ";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Binhnuocnonglanhariston.vn/News/" + StringClass.NameToTag(tblnew.Tag) + "-" + tblnew.id + ".aspx\" />";
            string meta = "";
            meta += "<meta itemprop=\"name\" content=\"" + tblnew.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + tblnew.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"" + tblnew.Images + "\" />";
            meta += "<meta property=\"og:title\" content=\"" + tblnew.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"" + tblnew.Images + "\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://Binhnuocnonglanhariston.vn\" />";
            meta += "<meta property=\"og:description\" content=\"" + tblnew.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta;
            int idcate = int.Parse(tblnew.idCate.ToString());
            ViewBag.nUrl = "<a href=\"http://BinhnuocnonglanhAriston.vn\" title=\"Trang chu\" rel=\"nofollow\"><span class=\"iCon\"></span>Trang chủ</a> /" + UrlNews(idcate);
            string coppy = "";
            var tblconfig = db.tblConfigs.First();
            if (tblconfig.Coppy == true)
            {
                coppy += "<script src=\"/Scripts/disable-copyright.js\"></script>";
                coppy += "<link href=\"/Content/Display/Css/Coppy.css\" rel=\"stylesheet\" />";
            }
            ViewBag.coppy = coppy;
            return View(tblnew);


        }
    }
}
