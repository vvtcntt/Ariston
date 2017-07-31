using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ariston.Models;
using PagedList;
using PagedList.Mvc;
namespace Ariston.Controllers.Display.Agency
{
    public class AgencyController : Controller
    {
       
        private AristonContext db = new AristonContext();
        // GET: Agency
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
                nUrl = " <a href=\"/9/" + ListMenu[i].Tag + "-"+ListMenu[i].id+".aspx\" title=\"" + ListMenu[i].Name + "\"> " + " " + ListMenu[i].Name + "</a> <i></i>" + nUrl;
                string ids = ListMenu[i].ParentID.ToString();
                if (ids != null && ids != "")
                {
                    int id = int.Parse(ListMenu[i].ParentID.ToString());
                    UrlNews(id);
                }

            }
            return nUrl;
        }
        public ActionResult AgencyDetail(string tag, tblAgency tblagencty)
        {

            int idNew;
            string Chuoi = tag;
            string[] Mang = Chuoi.Split('-');
            int one = int.Parse(Mang.Length.ToString());
            string chuoi1 = Mang[one - 1].ToString();
            string[] Mang1 = chuoi1.Split('.');
            idNew = int.Parse(Mang1[0].ToString());
            tblagencty = db.tblAgencies.Find(idNew);
            string chuoinew = "";
            var listManufacturers = db.tblAgencies.Where(p => p.idMenu == tblagencty.idMenu && p.id != tblagencty.id && p.Active == true).OrderByDescending(p => p.Ord).Take(3).ToList();
            for (int i = 0; i < listManufacturers.Count; i++)
            {
                chuoinew += "<a href=\"/NhaPhanPhoi/" + listManufacturers[i].Tag + "-" + listManufacturers[i].id + ".aspx\" title=\"" + listManufacturers[i].Name + "\"> - " + listManufacturers[i].Name + "</a>";


            }
            ViewBag.chuoinew = chuoinew;
            ViewBag.Title = "<title>" + tblagencty.Name + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblagencty.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblagencty.Name + "\" /> ";
            //var GroupManufacturers = db.tblGroupAgencies.First(p => p.id == tblagencty.idMenu);
            //int dodai = GroupManufacturers.Level.Length / 5;
            //int idcate=int.Parse(tblagencty.idMenu.ToString());
            ViewBag.nUrl = "<a href=\"/\" title=\"Trang chu\" rel=\"nofollow\"><span class=\"iCon\"></span>Trang chủ</a> / Nhà phân phối";

            return View(tblagencty);


        }
        public ActionResult AgencyList(string tag, int? page)
        {

            int idMenu;
            string Chuoi = tag;
            string[] Mang = Chuoi.Split('-');
            int one = int.Parse(Mang.Length.ToString());
            string chuoi1 = Mang[one - 1].ToString();
            string[] Mang1 = chuoi1.Split('.');
            idMenu = int.Parse(Mang1[0].ToString());
            tblGroupAgency groupagency = db.tblGroupAgencies.Find(idMenu);
            int idCate = groupagency.id;
            var LisstAgency = db.tblAgencies.Where(p =>p.Active == true).OrderByDescending(p => p.Ord).ToList();
            ViewBag.Name = groupagency.Name;
            ViewBag.Title = "<title>" + groupagency.Name + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + groupagency.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + groupagency.Name + "\" /> ";
         
            ViewBag.nUrl = "<a href=\"/\" title=\"Trang chu\" rel=\"nofollow\"><span class=\"iCon\"></span>Trang chủ</a> /" + groupagency.Name;
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

            return View(LisstAgency.ToPagedList(pageNumber, pageSize));


        }
    }
}