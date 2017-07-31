using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ariston.Models;
namespace Ariston.Controllers.Display.Left
{
    public class LeftController : Controller
    {
        //
        // GET: /Left/
        private AristonContext db = new AristonContext();

        public ActionResult Index()
        {

            return View();
        }
        public PartialViewResult SupportPartial()
        {
            tblConfig tblconfig = db.tblConfigs.Find(1);
            string chuoisuport = "";
            var listSupport = db.tblSupports.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            chuoisuport += " <div id=\"Support\">";
            chuoisuport += "<div id=\"Top_Support\"> <span>Hỗ trợ trực tuyến</span></div>";
            chuoisuport += "<div id=\"Bottom_Support\">";
            for (int i = 0; i < listSupport.Count; i++)
            {
                chuoisuport += "<div class=\"Tear_Support\">";
                chuoisuport += "<div class=\"Left\">";
                chuoisuport += "<span class=\"sp1\">" + listSupport[i].Mission + " :</span>";
                chuoisuport += "<span class=\"sp2\">" + listSupport[i].Name + " :</span>";
                chuoisuport += "</div>";
                chuoisuport += "<div class=\"Right\">";
                chuoisuport += "<div class=\"Yahoo\">";
                chuoisuport += "<a href=\"ymsgr:sendim?" + listSupport[i].Yahoo + "\">";
                chuoisuport += "<img src=\"http://opi.yahoo.com/online?u=" + listSupport[i].Yahoo + "&m=g&t=1\" alt=\"Yahoo\" class=\"imgYahoo\" />";
                chuoisuport += " </a>";
                chuoisuport += "</div>";
                chuoisuport += " <div class=\"Skype\">";
                chuoisuport += "<a href=\"Skype:" + listSupport[i].Skyper + "?chat\">";
                chuoisuport += "<img class=\"imgSkyper\" src=\"/Content/Display/iCon/skype-icon.png\" title=\"Kangaroo\" alt=\"Skyper\">";
                chuoisuport += "</a>";
                chuoisuport += "</div>";
                chuoisuport += "</div>";
                chuoisuport += " </div>";

            }
            chuoisuport += "</div><div class=\"Clear\"></div> </div>";
            ViewBag.chuoi = chuoisuport;
            return PartialView(tblconfig);

        }
        public PartialViewResult LeftContent()
        {
            return PartialView();
        }
        public PartialViewResult PartialboxNews()
        {
            var listnew = db.tblNews.Where(p => p.Active == true).OrderByDescending(p => p.Ord).Take(8).ToList();
            return PartialView(listnew);
        }
        public PartialViewResult MenuProductPartial()
        {
            string chuoi = "";
            var Menu = db.tblGroupProducts.Where(p => p.ParentID == null && p.Active == true).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < Menu.Count; i++)
            {
                chuoi += "<li class=\"li_1\">";
                chuoi += "<div class=\"MenuName\">";
                chuoi += "<a href=\"/ListProduct/" + Menu[i].Tag + "-" + Menu[i].id + ".aspx\" title=\"" + Menu[i].Name + "\">" + Menu[i].Name + "</a>";
                chuoi += "</div>";
                int idCate = Menu[i].id;
                var Menu1 = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID == idCate).OrderBy(p => p.Ord).ToList();
                if (Menu1.Count > 0)
                {

                    chuoi += "<ul class=\"ul_2\">";
                    for (int j = 0; j < Menu1.Count; j++)
                    {
                        chuoi += "<li class=\"li_2\">";
                        chuoi += " <h2><a href=\"/ListProduct/" + Menu1[j].Tag + "-" + Menu1[j].id + ".aspx\" title=\"" + Menu1[j].Name + "\">" + Menu1[j].Name + "</a> </h2>";
                        int idCate1 = Menu1[j].id;
                        var Menu2 = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID == idCate1).OrderBy(p => p.Ord).ToList();
                        if (Menu2.Count > 0)
                        {
                            chuoi += "<ul class=\"ul_3\">";
                            for (int k = 0; k < Menu2.Count; k++)
                            {
                                chuoi += "<li class=\"li_3\">";
                                chuoi += "<h3><a href=\"/ListProduct/" + Menu2[k].Tag + "-" + Menu2[k].id + ".aspx\" title=\"" + Menu2[k].Name + "\">› " + Menu2[k].Name + "</a></h3>";
                                chuoi += "</li>";
                            }

                            chuoi += "</ul>";
                        }
                        chuoi += "</li>";
                    }
                    chuoi += "</ul>";

                }
                chuoi += "</li>";

            }
            ViewBag.Menu = chuoi;
            return PartialView();
        }
        public PartialViewResult UrlPartial()
        {
            string chuoi = "";
            var tblUrl = db.tblUrls.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < tblUrl.Count; i++)
            {
                chuoi += "<a href=\"" + tblUrl[i].Url + "\" title=\"" + tblUrl[i].Name + "\" rel=\"" + tblUrl[i].Rel + "\"> - " + tblUrl[i].Name + "</a>";


            }
            ViewBag.chuoi = chuoi;
            return PartialView(db.tblConfigs.First());
        }
        public PartialViewResult LeftNewsPartail()
        {
            string chuoi = "";
            var listMenunew = db.tblGroupNews.Where(p => p.Active == true && p.ParentID == null).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < listMenunew.Count; i++)
            {
                chuoi += "<li class=\"li_n\">";
                chuoi += "<a href=\"/ListNew/" + listMenunew[i].Tag + "-" + listMenunew[i].id + ".aspx\" title=\"" + listMenunew[i].Name + "\">" + listMenunew[i].Name + "</a>";
                int idCate = listMenunew[i].id;
                var listMenunewschild = db.tblGroupNews.Where(p => p.ParentID == idCate && p.Active == true).OrderBy(p => p.Ord).ToList();
                if (listMenunewschild.Count > 0)
                {
                    chuoi += "<ul class=\"ul_n1\">";
                    for (int j = 0; j < listMenunewschild.Count; j++)
                    {
                        chuoi += "<li class=\"li_n1\">";
                        chuoi += "<a href=\"/ListNew/" + listMenunewschild[j].Tag + "-" + listMenunewschild[j].id + ".aspx\" title=\"" + listMenunewschild[j].Name + "\">› " + listMenunewschild[j].Name + "</a>";
                        chuoi += "</li>";
                    }
                    chuoi += "</ul>";
                }
                chuoi += "</li>";

            }
            ViewBag.chuoi = chuoi;
            string chuoiNPP = "";
            var GroupAgency = db.tblGroupAgencies.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            for (int j = 0; j < GroupAgency.Count; j++)
            {

                chuoiNPP += "<ul class=\"ul_n1\">";
                chuoiNPP += "<li class=\"li_n1\">";
                chuoiNPP += "<a href=\"/9/" + GroupAgency[j].Tag + "-" + GroupAgency[j].id + ".aspx\" title=\"" + GroupAgency[j].Name + "\">› " + GroupAgency[j].Name + "</a>";
                chuoiNPP += "</li>";
                chuoiNPP += "</ul>";
            }
            ViewBag.chuoiNPP = chuoiNPP;
            return PartialView();
        }
    }
}
