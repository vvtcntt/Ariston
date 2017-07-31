using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ariston.Models;
using System.Globalization;
using System.Text;
namespace Ariston.Controllers.Display
{
    public class DefaultController : Controller
    {
        //
        // GET: /Default/
        private AristonContext db = new AristonContext();
        string nUrl = "";
        public string UrlProduct(int idCate)
        {
            var ListMenu = db.tblGroupProducts.Where(p => p.id== idCate).ToList();
            for (int i = 0; i < ListMenu.Count; i++)
            {
                nUrl = " <a href=\"/ListProduct/" + ListMenu[i].Tag + "-"+ListMenu[i].id+".aspx\" title=\"" + ListMenu[i].Name + "\"> " + " " + ListMenu[i].Name + "</a> <i></i>" + nUrl;
                string ids = ListMenu[i].ParentID.ToString();
                if (ids != null && ids != "")
                {
                    int id = int.Parse(ListMenu[i].ParentID.ToString());
                    UrlProduct(id);
                }
            }
            return nUrl;
        }
        List<string> Mangphantu = new List<string>();
        public List<string> Arrayid(int idParent)
        {

            var ListMenu = db.tblGroupProducts.Where(p => p.ParentID == idParent).ToList();

            for (int i = 0; i < ListMenu.Count; i++)
            {
                Mangphantu.Add(ListMenu[i].id.ToString());
                int id = int.Parse(ListMenu[i].id.ToString());
                Arrayid(id);

            }

            return Mangphantu;
        }
        public ActionResult Index(string sx)
        {
            tblConfig tblconfig  = db.tblConfigs.First();
            ViewBag.Title = "<title>" + tblconfig.Title + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + tblconfig.Title + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblconfig.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblconfig.Keywords + "\" /> ";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Binhnuocnonglanhariston.vn\" />";
            string meta = "";
            meta += "<meta itemprop=\"name\" content=\"" + tblconfig.Title + "\" />";
            meta += "<meta itemprop=\"url\" content=\"http://Binhnuocnonglanhariston.vn\" />";
            meta += "<meta itemprop=\"description\" content=\"" + tblconfig.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\" " + tblconfig.Logo+ "\" />";
            meta += "<meta property=\"og:title\" content=\"" + tblconfig.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"http://Binhnuocnonglanhariston.vn\" />";
            meta += "<meta property=\"og:image\" content=\""+tblconfig.Logo+"\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://Binhnuocnonglanhariston.vn\" />";
            meta += "<meta property=\"og:description\" content=\"" +tblconfig.Logo+ "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta;
            ViewBag.h1="<h1 class=\"h1\">"+tblconfig.Title+"</h1>";
            StringBuilder chuoi = new StringBuilder();
            var ListCapacity = db.tblCapacities.Where(p => p.Active == true && p.Priority == true).OrderBy(p => p.Ord).ToList();
            foreach (var item in ListCapacity)
            {
                chuoi.Append("<div class=\"Product\">");
                 chuoi.Append("<div class=\"nVar\">");
                 chuoi.Append("<div class=\"Left\">");
                 chuoi.Append("<h2> <a href=\"/" + item.Tag + ".html\" title=\"" + item.Title + "\"> " + item.Name + "</a></h2>");
                 chuoi.Append("</div>");
                 chuoi.Append("<div class=\"Right\">");
                 chuoi.Append("<select class=\"Arrange\">");
                 chuoi.Append("<option value=\"0\" selected=\"selected\">Sắp xếp theo giá</option> ");
                 chuoi.Append("<option value=\"1\">Tăng dần</option>");
                 chuoi.Append("<option value=\"2\">Giảm dần</option>");
                 chuoi.Append("</select>");
                 chuoi.Append("</div>");
                 chuoi.Append("</div>");
                 chuoi.Append("<div class=\"Content\">");
                int idCap = item.id;
                var listProduct = db.tblProducts.Where(p => p.Capacity == idCap && p.Active == true && p.ViewHomes == true).OrderBy(p => p.Ord).ToList();
                for (int j = 0; j < listProduct.Count; j++)
                {
                    if(listProduct[j].Priority==true)
                    {
                        chuoi.Append("  <div class=\"Tear_Pri\">");
                        chuoi.Append("<h3><a href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"" + listProduct[j].Name + "\">" + listProduct[j].Name + "</a></h3>");
                        chuoi.Append("  <div class=\"Topsi\"></div>");
                        chuoi.Append("  <div class=\"Content_tearPri\">");
                        chuoi.Append(" <div class=\"Box_Info\">");
                        chuoi.Append(" <div class=\"Info\">");
                        chuoi.Append(listProduct[j].Info);
                        chuoi.Append(" </div>");
                        chuoi.Append(" <div class=\"Box_Qua\">");
                        chuoi.Append(" <div class=\"Content_Boxqua\">");
                        chuoi.Append(" <div class=\"boxQua_Line\"><span>" + listProduct[j].Sale + "</span></div>");

                        chuoi.Append(" </div>");
                        chuoi.Append("  </div>");
                        chuoi.Append(" </div>");

                        chuoi.Append("  <div class=\"Box_Image\">");
                        chuoi.Append("  <div class=\"price\">");
                        chuoi.Append(" <div class=\"box_PriceSale\">");
                        chuoi.Append(" <small>");
                        chuoi.Append(listProduct[j].PriceSaleString);

                        chuoi.Append("  </small>");
                        chuoi.Append("  <span class=\"kmp\"></span>");
                        chuoi.Append("   </div>");
                        chuoi.Append("  <div class=\"box_Price\">");
                        chuoi.Append(" <small>");
                        chuoi.Append(" <span class=\"gny\"></span>");
                        chuoi.Append(listProduct[j].PriceString);

                        chuoi.Append(" </small>");
                        chuoi.Append(" </div>");
                        chuoi.Append(" </div>");
                        chuoi.Append("<div class=\"img\">");
                        chuoi.Append("<a href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"" + listProduct[j].Name + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a>");
                        chuoi.Append(" </div>");
                        chuoi.Append("  </div>");

                        chuoi.Append("  </div>");

                        chuoi.Append("  </div>");


                    }
                    else
                    {
                         chuoi.Append("<div class=\"Tear_1\">");
                        if (listProduct[j].New == true)
                        {
                             chuoi.Append("<div class=\"New\">");
                             chuoi.Append("</div>");
                        }
                         chuoi.Append("<div class=\"img\">");
                         chuoi.Append("<a href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"" + listProduct[j].Name + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a>");
                         chuoi.Append("</div>");
                         chuoi.Append("<h3><a class=\"Name\" href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"" + listProduct[j].Name + "\">" + listProduct[j].Name + "</a></h3>");
                         chuoi.Append("<p class=\"Price\">Giá : <span>" + string.Format("{0:#,#}", listProduct[j].Price) + " đ</span></p>");
                         chuoi.Append("<p class=\"PriceSale\"> Khuyến mại : <span>" + string.Format("{0:#,#}", listProduct[j].PriceSale) + " đ</span></p>");
                         chuoi.Append(" </div> ");
                    }
                    
                }
                 chuoi.Append("</div>");
                 chuoi.Append("</div>");
            }
            var GroupProduct = db.tblGroupProducts.Where(p => p.Active == true && p.Priority == true).OrderBy(p => p.Ord).ToList();
            StringBuilder chuoi1 = new StringBuilder();
            foreach(var item in GroupProduct)
            {
                chuoi1.Append("<div class=\"Product\">");
              chuoi1.Append("<div class=\"nVar\">");
              chuoi1.Append("<div class=\"Left\">");
              chuoi1.Append("<h2> <a href=\"/ListProduct/" + item.Tag + "-"+item.id+".aspx\" title=\"" + item.Title + "\"> " + item.Name + "</a></h2>");
              chuoi1.Append("</div>");
              chuoi1.Append("<div class=\"Right\">");
              chuoi1.Append("<select class=\"Arrange\">");
              chuoi1.Append("<option value=\"0\" selected=\"selected\">Sắp xếp theo giá</option> ");
              chuoi1.Append("<option value=\"1\">Tăng dần</option>");
              chuoi1.Append("<option value=\"2\">Giảm dần</option>");
              chuoi1.Append("</select>");
              chuoi1.Append("</div>");
              chuoi1.Append("</div>");
              chuoi1.Append("<div class=\"Content\">");
                int idCate = item.id;
                List<string> Mang = new List<string>();
                Mang = Arrayid(idCate);
                Mang.Add(idCate.ToString());
                var listProduct = db.tblProducts.Where(p => Mang.Contains(p.idCate.ToString()) && p.Active == true && p.ViewHomes == true).OrderBy(p => p.Ord).ToList();
                for (int j = 0; j < listProduct.Count; j++)
                {
                    if (listProduct[j].Priority == true)
                    {


                        chuoi1.Append("  <div class=\"Tear_Pri\">");
                        chuoi1.Append("<h3><a href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"" + listProduct[j].Name + "\">" + listProduct[j].Name + "</a></h3>");
                        chuoi1.Append("  <div class=\"Topsi\"></div>");
                        chuoi1.Append("  <div class=\"Content_tearPri\">");
                        chuoi1.Append(" <div class=\"Box_Info\">");
                        chuoi1.Append(" <div class=\"Info\">");
                        chuoi1.Append(listProduct[j].Info);
                        chuoi1.Append(" </div>");
                        chuoi1.Append(" <div class=\"Box_Qua\">");
                        chuoi1.Append(" <div class=\"Content_Boxqua\">");
                        chuoi1.Append(" <div class=\"boxQua_Line\"><span>" + listProduct[j].Sale + "</span></div>");

                        chuoi1.Append(" </div>");
                        chuoi1.Append("  </div>");
                        chuoi1.Append(" </div>");

                        chuoi1.Append("  <div class=\"Box_Image\">");
                        chuoi1.Append("  <div class=\"price\">");
                        chuoi1.Append(" <div class=\"box_PriceSale\">");
                        chuoi1.Append(" <small>");
                        chuoi1.Append(listProduct[j].PriceSaleString);

                        chuoi1.Append("  </small>");
                        chuoi1.Append("  <span class=\"kmp\"></span>");
                        chuoi1.Append("   </div>");
                        chuoi1.Append("  <div class=\"box_Price\">");
                        chuoi1.Append(" <small>");
                        chuoi1.Append(" <span class=\"gny\"></span>");
                        chuoi1.Append(listProduct[j].PriceString);

                        chuoi1.Append(" </small>");
                        chuoi1.Append(" </div>");
                        chuoi1.Append(" </div>");
                        chuoi1.Append("<div class=\"img\">");
                        chuoi1.Append("<a href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"" + listProduct[j].Name + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a>");
                        chuoi1.Append(" </div>");
                        chuoi1.Append("  </div>");

                        chuoi1.Append("  </div>");

                        chuoi1.Append("  </div>");

                    }
                    else
                    {
                      chuoi1.Append("<div class=\"Tear_1\">");
                        if (listProduct[j].New == true)
                        {
                          chuoi1.Append("<div class=\"New\">");
                          chuoi1.Append("</div>");
                        }
                      chuoi1.Append("<div class=\"img\">");
                      chuoi1.Append("<a href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"" + listProduct[j].Name + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a>");
                      chuoi1.Append("</div>");
                      chuoi1.Append("<h3><a class=\"Name\" href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"" + listProduct[j].Name + "\">" + listProduct[j].Name + "</a></h3>");
                      chuoi1.Append("<p class=\"Price\">Giá : <span>" + string.Format("{0:#,#}", listProduct[j].Price) + " đ</span></p>");
                      chuoi1.Append("<p class=\"PriceSale\"> Khuyến mại : <span>" + string.Format("{0:#,#}", listProduct[j].PriceSale) + " đ</span></p>");
                      chuoi1.Append(" </div> ");
                    }
                  
                }
              chuoi1.Append("</div>");
              chuoi1.Append("</div>");
                Mangphantu.Clear();
            }
            ViewBag.chuoi = chuoi; ViewBag.chuoi1 = chuoi1;
                return View();
        }
        public PartialViewResult SlidePartial()
        {
            var listSlide = db.tblImages.Where(p => p.idCate == 1 && p.Active == true).OrderBy(p => p.Ord).Take(5).ToList();
            StringBuilder chuoi = new StringBuilder();
            for (int i = 0; i < listSlide.Count;i++ )
            {
                if(i==0)
                 chuoi.Append("url("+listSlide[i].Images+") 0 0 no-repeat");
                else
                     chuoi.Append(","+"url("+listSlide[i].Images+") "+(i*1200)+"px 0 no-repeat");
            }
            ViewBag.url = chuoi;
            return PartialView(listSlide);
        } 
        List<SelectListItem> carlist = new List<SelectListItem>();
        public PartialViewResult TopPartial()
        {
            tblConfig config = db.tblConfigs.FirstOrDefault();
           
            var Menu=db.tblGroupProducts.Where(p=>p.ParentID==null&& p.Active==true).OrderBy(p=>p.Ord).ToList();
            StringBuilder Chuoi=new StringBuilder();
       
            for (int i = 0; i < Menu.Count; i++)
            { 
                Chuoi.Append("<li class=\"li1\">");
                 Chuoi.Append(" <a href=\"/ListProduct/" + Menu[i].Tag + "-" + Menu[i].id + ".aspx\" title=\"" + Menu[i].Name + "\">" + Menu[i].Name + "</a>");

                 int idCate = Menu[i].id;
                 var Menu1 = db.tblGroupProducts.Where(p => p.ParentID == idCate && p.Active == true).OrderBy(p => p.Ord).ToList();
                         if(Menu1.Count>0)
                         {  
                             
                               Chuoi.Append("<ul class=\"ul2\">");
                             for(int j=0;j<Menu1.Count;j++)
                             {

                                 Chuoi.Append("<li class=\"li2\">");
                                  Chuoi.Append("<a href=\"/ListProduct/"+Menu1[j].Tag+"-"+Menu1[j].id+".aspx\" title=\"" + Menu1[j].Name + "\">" + Menu1[j].Name + "</a>");
                                 int idCate1= Menu1[j].id;
                                 var Menu2=db.tblGroupProducts.Where(p=>p.ParentID==idCate1 && p.Active==true).OrderBy(p=>p.Ord).ToList();
                                
                                 if(Menu2.Count>0)
                                 { 
                                      Chuoi.Append("<ul class=\"ul3\">");
                                    for(int k=0;k<Menu2.Count;k++)
                                    {
                                          Chuoi.Append("<li class=\"li3\">");
                                      Chuoi.Append("<a href=\"/ListProduct/" + Menu2[k].Tag + "-" + Menu2[k].id + ".aspx\" title=\"" + Menu2[k].Name + "\">› " + Menu2[k].Name + "</a>");
                                          Chuoi.Append(" </li>");
                                    } 
                                         Chuoi.Append("</ul>");
                                 }
                                     Chuoi.Append("</li>");                         
                             }                        
                               Chuoi.Append("</ul>");
                         }
                        
                    Chuoi.Append(" </li>");
            }
            ViewBag.Menu = Chuoi;


            if(Request.Browser.IsMobileDevice)
            {
                var menuModel = db.tblGroupProducts.Where(m => m.ParentID == null).OrderBy(m => m.id).ToList();
                carlist.Clear();
                string strReturn = "---";
                foreach (var item in menuModel)
                {
                    carlist.Add(new SelectListItem { Text = item.Name, Value = item.id.ToString() });
                    StringClass.DropDownListFor(item.id, carlist, strReturn);
                    strReturn = "---";
                }

                ViewBag.one = carlist;
            }
          
            
            
            return PartialView(config);

        }
        public PartialViewResult partialNewsHomes()
        {
            StringBuilder chuoi = new StringBuilder();
            var tblnews = db.tblNews.Where(p => p.Active == true).OrderByDescending(p => p.Ord).Take(13).ToList();
            for (int i = 0; i < tblnews.Count;i++ )
            {
                chuoi.Append("<h4><a href=\"/News/" + tblnews[i].Tag + "-" + tblnews[i].id + ".aspx\" title=\"" + tblnews[i].Name + "\">");
                if(tblnews[i].ViewHomes==true)
                {
                    chuoi.Append("<span class=\"rel\"></span>");
                }
                else
                {
                    chuoi.Append("<span class=\"blue\"></span>");
                }
                 
                chuoi.Append(tblnews[i].Name);
                chuoi.Append("</a></h4>");
            }
            ViewBag.chuoi = chuoi;
            var tblvideo = db.tblVideos.FirstOrDefault();
            return PartialView(tblvideo);
        }
        [HttpPost]
        public ActionResult Dropdownlist(int id)
        {
            var listProduct = db.tblGroupProducts.Find(id);

            return Redirect("/ListProduct/" + listProduct.Tag + "." + id + ".aspx");
        }
        public PartialViewResult FootterPartial()
        {
            tblConfig tblconfig = db.tblConfigs.Find(1);
            string chinhsach = "";
            var listchinhscach = db.tblNews.Where(p => p.idCate == 12 && p.Active == true).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < listchinhscach.Count;i++ )
            {
                chinhsach+="<a href=\"/News/"+listchinhscach[i].Tag+"-"+listchinhscach[i].id+".aspx\" title=\""+listchinhscach[i].Name+"\">"+listchinhscach[i].Name+"</a>"; 
            }
            ViewBag.chinhsach = chinhsach;
            string sanpham = "";
            var listProduct = db.tblGroupProducts.Where(p => p.Priority==true && p.Active == true).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < listProduct.Count;i++ )
            {
                sanpham += "<h3><a href=\"/ListProduct/" + listProduct[i].Tag + "-" + listProduct[i].id + ".aspx\" title=\"" + listProduct[i].Title + "\">" + listProduct[i].Name + "</a></h3>";
            }
            ViewBag.sanpham = sanpham;
            tblMap tblmap = db.tblMaps.First();
            ViewBag.maps = tblmap.Content;
            var listCapacity = db.tblCapacities.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            string dungtich = "";
            foreach(var item in listCapacity)
            {
                dungtich+= "<h3><a href=\"/" + item.Tag + ".html\" title=\"" + item.Title + "\">" + item.Name + "</a></h3>";
            }
            ViewBag.dungtich = dungtich;
            var Imagesadw = db.tblImages.Where(p => p.Active == true && p.idCate == 5).OrderByDescending(p => p.Ord).Take(1).ToList();
            if (Imagesadw.Count > 0)
                ViewBag.Chuoiimg = "<a href=\"" + Imagesadw[0].Url + "\" title=\"" + Imagesadw[0].Name + "\"><img src=\"" + Imagesadw[0].Images + "\" alt=\"" + Imagesadw[0].Name + "\" style=\"max-width:100%;\" /> </a>";
            var listUrl = db.tblUrls.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            StringBuilder resultUrl = new StringBuilder();
            for(int i=0;i<listUrl.Count;i++)
            {
                resultUrl.Append("<h3 style = \"margin:0px; display:inline-block;   font-weight:normal\" ><a style = \"font-size:12px; margin:0px; color:#FFF\" href = \""+listUrl[i].Url+"\" title = \""+listUrl[i].Name+ "\" > " + listUrl[i].Name + "</a ></h3 >");
            }
            ViewBag.resultUrl = resultUrl.ToString();
            return PartialView(tblconfig);
        }
        public ActionResult Search(FormCollection collection)
        {
            if (collection["btnSearch"] != null)
            {
                if (collection["txtSearch"]!="")
                {

                Session["txtSearch"] = collection["txtSearch"];
                return Redirect("/SearchProduct");
                }
                 


            }

            return RedirectToAction("Index");
        }
        public PartialViewResult MenuMobile()
        {
            StringBuilder chuoi = new StringBuilder();
            var listMenu = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID==null).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < listMenu.Count; i++)
            {
                 chuoi.Append("<li> <a href=\"/ListProduct/" + listMenu[i].Tag + "-" + listMenu[i].id + ".aspx\">" + listMenu[i].Name + "</a>");
                int idCate = listMenu[i].id;
                var LitsMenu1 = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID == idCate).OrderBy(p => p.Ord).ToList();
                if (LitsMenu1.Count > 0)
                {
                    chuoi.Append("<ul>");
                    for (int j = 0; j < LitsMenu1.Count; j++)
                    {
                         chuoi.Append("<li><a href=\"/ListProduct/" + LitsMenu1[j].Tag + "-" + LitsMenu1[j].id + ".aspx \">" + LitsMenu1[j].Name + "</a>");

                       int idCate1 = LitsMenu1[j].id;
                       var Listmenu2 = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID == idCate1).OrderBy(p => p.Ord).ToList();
                        if (Listmenu2.Count > 0)
                        {
                             chuoi.Append(" <ul>");
                            for (int k = 0; k < Listmenu2.Count; k++)
                            {
                                 chuoi.Append("<li><a href=\"/ListProduct/" + Listmenu2[k].Tag + "-" + Listmenu2[k].id + ".aspx\">" + Listmenu2[k].Name + "</a></li>");
                            }
                             chuoi.Append(" </ul>");
                        }
                         chuoi.Append("</li>");
                    }
                     chuoi.Append("</ul>");
                }
                 chuoi.Append("</li>");
            }
            ViewBag.chuoimenu = chuoi;
            return PartialView();
       }
        string chuoilevel = "";
        public string GetLevel(int idCate, int level)
        {
            var ListMenu = db.tblGroupProducts.Where(p => p.ParentID == idCate).ToList();

            for (int i = 0; i < ListMenu.Count; i++)
            {

                int id = ListMenu[i].id;
                tblGroupProduct tblgroupproduct = db.tblGroupProducts.Find(id);
                tblgroupproduct.Level = level + 1;
                db.SaveChanges();
                var kiemtra = db.tblGroupProducts.Where(p => p.ParentID == id).ToList();

                if (kiemtra.Count > 0)
                {
                    int levels = int.Parse(tblgroupproduct.Level.ToString());
                    GetLevel(id, levels);
                }
            }
            return chuoilevel;
        }
        public ActionResult Action()
        {
            var listgroup = db.tblGroupProducts.Where(p => p.ParentID == null).ToList();
            for (int i = 0; i < listgroup.Count; i++)
            {
                int id = listgroup[i].id;
                tblGroupProduct tblgroupproduct = db.tblGroupProducts.Find(id);
                tblgroupproduct.Level = 0;
                db.SaveChanges();
                var kiemtra = db.tblGroupProducts.Where(p => p.ParentID == id).ToList();
                if (kiemtra.Count > 0)
                {


                    GetLevel(id, 0);
                }
            }
            return View();
        }
    }
}
