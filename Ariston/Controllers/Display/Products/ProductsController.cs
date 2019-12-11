using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ariston.Models;
using System.Text;
namespace Ariston.Controllers.Display.Products
{
    public class ProductsController : Controller
    {
        //
        // GET: /Products/
        private AristonContext db = new AristonContext();

        public ActionResult Index()
        {
            return View();
        }
    

        string nUrl = "";
        public string UrlNews(int idCate)
        {
            var ListMenu = db.tblGroupProducts.Where(p => p.id == idCate).ToList();
            for (int i = 0; i < ListMenu.Count; i++)
            {
                nUrl = " <li itemprop=\"itemListElement\" itemscope itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"/ListProduct/" + ListMenu[i].Tag + "-"+ListMenu[i].id+".aspx\"> <span itemprop=\"name\">" + ListMenu[i].Name + "</span></a> <meta itemprop=\"position\" content=\"" + (ListMenu[i].Level + 2) + "\" /> </li> › " + nUrl;
                string ids = ListMenu[i].ParentID.ToString();
                if (ids != null && ids != "")
                {
                    int id = int.Parse(ListMenu[i].ParentID.ToString());
                    UrlNews(id);
                }
            }
            return nUrl;
        }
        public ActionResult ProductDetail(string tag,string id)
        {
            int nId = int.Parse(id);
            tblProduct product = db.tblProducts.First(p => p.id==nId);          
            int idp = product.id;
            string ntag = product.Tag;
            if(ntag!=tag)
            {
                return Redirect("/Product/"+ntag+"-"+product.idCate+"-"+id+".aspx");
            }
            ViewBag.Title = "<title>" + product.Title + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + product.Title + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + product.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + product.Keyword + "\" /> ";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Binhnuocnonglanhariston.vn/Product/" + StringClass.NameToTag(product.Tag) + "-" + product.idCate + "-" + product.id + ".aspx\" />";
            string meta = "";
            meta += "<meta itemprop=\"name\" content=\"" + product.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + product.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"" + product.ImageLinkThumb + "\" />";
            meta += "<meta property=\"og:title\" content=\"" + product.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"" + product.ImageLinkThumb + "\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://Binhnuocnonglanhariston.vn\" />";
            meta += "<meta property=\"og:description\" content=\"" + product.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta;
            int idMenu=int.Parse(product.idCate.ToString());
            string splq = "";
            var listProduct = db.tblProducts.Where(p => p.Active == true && p.idCate == idMenu && p.id != idp).OrderBy(p => p.Ord).Take(4).ToList(); 
            for (int i = 0; i < listProduct.Count;i++ )
            {

                splq += " <div class=\"Tear_5\">";
                splq += "  <div class=\"Top_Tear5\">";
                splq += " <a href=\"/Product/" + listProduct[i].Tag + "-" + listProduct[i].idCate + "-" + listProduct[i].id + ".aspx\" title=\"\">";
                splq += " <img src=\"" + listProduct[i].ImageLinkThumb + "\" alt=\"" + listProduct[i].Name + "\" />";
                splq += " </a>";
                splq += " </div>";
                splq += " <h3><a href=\"/Product/" + listProduct[i].Tag + "-" + listProduct[i].idCate + "-" + listProduct[i].id + ".aspx\" title=\"" + listProduct[i].Name + "\">" + listProduct[i].Name + "</a></h3>";
                splq += " </div>";
            }
            ViewBag.splq = splq;
            int idcate = int.Parse(product.idCate.ToString());
            ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"http://Binhnuocnonglanhariston.vn\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   ›" + UrlNews(idcate) + "</ol> <div itemscope itemtype=\"http://schema.org/Product\"><h1 itemprop=\"name\">" + product.Title + "</h1> <span itemprop=\"offers\" itemscope itemtype=\"http://schema.org/Offer\">   <meta itemprop=\"priceCurrency\" content=\"vnd\" />   Buy New: d<span itemprop=\"price\">" + string.Format("{0:#,#}", product.PriceSale) + "</span> <link itemprop=\"availability\" href=\"http://schema.org/InStock\" />  </span> </div>";
            var ListGroupCri = db.tblGroupCriterias.Where(p => p.idCate == idcate).ToList();
            List<int> Mangs = new List<int>();
            for (int i = 0; i < ListGroupCri.Count; i++)
            {
                Mangs.Add(int.Parse(ListGroupCri[i].idCri.ToString()));
            }
             var ListCri = db.tblCriterias.Where(p => Mangs.Contains(p.id) && p.Active == true).ToList();
            string chuoi = "";
            #region[Lọc thuộc tính]
            for (int i = 0; i < ListCri.Count; i++)
            {
                int idCre = int.Parse(ListCri[i].id.ToString());
                var ListCr = (from a in db.tblConnectCriterias
                              join b in db.tblInfoCriterias on a.idCre equals b.id
                              where a.idpd == idp && b.idCri == idCre && b.Active == true
                              select new
                              {
                                  b.Name,
                                  b.Url,
                                  b.Ord
                              }).OrderBy(p => p.Ord).ToList();
                if (ListCr.Count > 0)
                {
                    chuoi += "<tr>";
                    chuoi += "<td>" + ListCri[i].Name + "</td>";
                    chuoi += "<td>";
                    int dem = 0;
                    string num = "";
                    if (ListCr.Count > 1)
                        num = "⊹ ";
                    foreach (var item in ListCr)
                        if (item.Url != null && item.Url != "")
                        {
                            chuoi += "<a href=\"" + item.Url + "\" title=\"" + item.Name + "\">";
                            if (dem == 1)
                                chuoi += num + item.Name;
                            else
                                chuoi += num + item.Name;
                            dem = 1;
                            chuoi += "</a>";
                        }
                        else
                        {
                            if (dem == 1)
                                chuoi += num + item.Name + "</br> ";
                            else
                                chuoi += num + item.Name + "</br> "; ;
                            dem = 1;
                        }
                    chuoi += "</td>";
                    chuoi += " </tr>";
                }
            }
            #endregion
            ViewBag.chuoi = chuoi;
            if(product.Capacity.ToString()!=null & product.Capacity.ToString()!="")
            {
                int idcap = int.Parse(product.Capacity.ToString());
                var tblcapacity = db.tblCapacities.Find(idcap);
                ViewBag.capa = "<h3><a href=\"/" + tblcapacity.Tag + ".html\" title=\"" + tblcapacity.Name + "\">" + tblcapacity.Capacity + "</a><h3>";
                ViewBag.songuoisd = tblcapacity.Note;
                ViewBag.cap1 = tblcapacity.Capacity;
            }
            if(product.Capacity.ToString()!=null && product.Capacity.ToString()!="")
            {
                int idcap = int.Parse(product.Capacity.ToString());
                var listproduct = db.tblProducts.Where(p => p.Active == true && p.Capacity == idcap && p.id!=idp).OrderBy(p => p.PriceSale).ToList();
                string spk = "";
                foreach(var item in listproduct)
                {
                    spk += "<div class=\"Tear_1\">";
                    if (item.New == true)
                    {
                        spk += "<div class=\"New\">";
                        spk += "</div>";
                    }

                    spk += "<div class=\"img\">";
                    spk += "<a href=\"/Product/" + item.Tag + "-" + item.idCate + "-" + item.id + ".aspx\" title=\"" + item.Name + "\"><img src=\"" + item.ImageLinkThumb + "\" alt=\"" + item.Name + "\" /></a>";
                    spk += "</div>";
                    spk += "<h3><a class=\"Name\" href=\"/Product/" + item.Tag + "-" + item.idCate + "-" + item.id + ".aspx\" title=\"\">" + item.Name + "</a></h3>";
                    spk += "<p class=\"Price\">Giá : <span>" + string.Format("{0:#,#}", item.Price) + " đ</span></p>";
                    spk += "<p class=\"PriceSale\"> Khuyến mại : <span>" + string.Format("{0:#,#}", item.PriceSale) + " đ</span></p>";
                    spk += " </div> ";
                }
                ViewBag.spk = spk;
            }
            //load danh sách ảnh liên quan
             string resultImage="";
             var listImages = db.tblImageProducts.Where(p => p.idProduct == idp).ToList();
             for (int i = 0; i < listImages.Count;i++ )
             {
                 resultImage+= "<li><img src=\""+listImages[i].Images+"\" alt=\""+product.Name+"\"></li>";
             }
             ViewBag.resultImages = resultImage; string address = product.Address.ToString();

            string resultAddress = "";
            if (address != null && address != "")
            {
                int idaddress = int.Parse(address);
                if (db.tblAddresses.FirstOrDefault(p => p.id == idaddress) != null)
                    resultAddress = db.tblAddresses.FirstOrDefault(p => p.id == idaddress).Name;
            }
            ViewBag.address = resultAddress;
            return View(product);
        }
        public ActionResult Command(FormCollection collection,string tag)
        {
            if (collection["btnOrder"] != null)
            {

                Session["idProduct"] = collection["idPro"];
              Session["idMenu"] = collection["idCate"];
              Session["OrdProduct"] = collection["txtOrd"];
              Session["Url"] = Request.Url.ToString();
              return RedirectToAction("OrderIndex", "Order");            


            }
            return View();
        }
        public ActionResult SearchProduct()
        {
            if(Session["txtSearch"]!=""&& Session["txtSearch"]!=null)
            {
                string Name = Session["txtSearch"].ToString();
                string chuoi="";
                var groupPproduct = db.tblProducts.Where(p => p.Name.Contains(Name)).ToList();
                ViewBag.Title = "<title>" + Name + "</title>";
                ViewBag.Description = "<meta name=\"description\" content=\"" + Name + "\"/>";
                ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + Name + "\" /> ";
                Session["txtSearch"] = "";
                Session["txtSearch"] = "";
                ViewBag.chuoi = Name;
            return View(groupPproduct);
             
            }
            return View();

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
        public ActionResult ListProduct(string tag,string id)
        {
            string chuoi = "";
            int nId = int.Parse(id);
            tblGroupProduct groupPproduct = db.tblGroupProducts.First(p => p.id == nId);
            int idcate = groupPproduct.id;
            ViewBag.mota = groupPproduct.Content;
            ViewBag.Title = "<title>" + groupPproduct.Title + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + groupPproduct.Title + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + groupPproduct.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + groupPproduct.Keyword + "\" /> ";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Binhnuocnonglanhariston.vn/ListProduct/" + StringClass.NameToTag(groupPproduct.Tag) + "-" + groupPproduct.id + ".aspx\" />";
            string meta = "";
            meta += "<meta itemprop=\"name\" content=\"" + groupPproduct.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + groupPproduct.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"" + groupPproduct.Images + "\" />";
            meta += "<meta property=\"og:title\" content=\"" + groupPproduct.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"" + groupPproduct.Images + "\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://Binhnuocnonglanhariston.vn\" />";
            meta += "<meta property=\"og:description\" content=\"" + groupPproduct.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta;
           
            var ListGroup = db.tblGroupProducts.Where(p => p.ParentID == idcate && p.Active == true).OrderBy(p => p.Ord).ToList();
            if(ListGroup.Count==0)
            { 
                chuoi += "<div class=\"Product\">";
                chuoi += "<div class=\"nVar\">";
                chuoi += "<div class=\"Left\">";
                chuoi += "<span>" + groupPproduct.Name + "</span>";
                chuoi += "</div>";
                chuoi += "<div class=\"Right\">";
                chuoi += "<select class=\"Arrange\">";
                chuoi += "<option value=\"0\" selected=\"selected\">Sắp xếp theo giá</option> ";
                chuoi += "<option value=\"1\">Tăng dần</option>";
                chuoi += "<option value=\"2\">Giảm dần</option>";
                chuoi += "</select>";
                chuoi += "</div>";
                chuoi += "</div>";
                chuoi += "<div class=\"Content\">";
                List<string> Mangpd = new List<string>();
                Mangpd = Arrayid(idcate);
                Mangpd.Add(idcate.ToString());
                var listProduct = db.tblProducts.Where(p => Mangpd.Contains(p.idCate.ToString()) && p.Active == true).OrderBy(p => p.Ord).ToList();
               
                    for (int j = 0; j < listProduct.Count; j++)
                    {
                        chuoi += "<div class=\"Tear_1\">";
                        if (listProduct[j].New == true)
                        {
                            chuoi += "<div class=\"New\">";
                            chuoi += "</div>";
                        }

                        chuoi += "<div class=\"img\">";
                        chuoi += "<a href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"" + listProduct[j].Name + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a>";
                        chuoi += "</div>";
                        chuoi += "<h3><a class=\"Name\" href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"\">" + listProduct[j].Name + "</a></h3>";
                        chuoi += "<p class=\"Price\">Giá : <span>" + string.Format("{0:#,#}", listProduct[j].Price) + " đ</span></p>";
                        chuoi += "<p class=\"PriceSale\"> Khuyến mại : <span>" + string.Format("{0:#,#}", listProduct[j].PriceSale) + " đ</span></p>";
                        chuoi += " </div> ";
                    }              
                chuoi += "</div>";
                chuoi += "</div>";
                Mangphantu.Clear();
            }
            else
            {
                 for (int i = 0; i < ListGroup.Count;i++ )
                {
                     
                        Mangphantu.Clear();
                        chuoi += "<div class=\"Product\">";
                        chuoi += "<div class=\"nVar\">";
                        chuoi += "<div class=\"Left\">";
                        chuoi += "<h2> <a href=\"/ListProduct/" + ListGroup[i].Tag + "-" + ListGroup[i].id + ".aspx\" title=\"" + ListGroup[i].Name + "\">" + ListGroup[i].Name + "</h2>";
                        chuoi += "</div>";
                        chuoi += "<div class=\"Right\">";

                        chuoi += "<select class=\"Arrange\">";
                        chuoi += "<option value=\"0\" selected=\"selected\">Sắp xếp theo giá</option> ";
                        chuoi += "<option value=\"1\">Tăng dần</option>";
                        chuoi += "<option value=\"2\">Giảm dần</option>";
                        chuoi += "</select>";
                        chuoi += "</div>";
                        chuoi += "</div>";
                        chuoi += "<div class=\"Content\">";
                        List<string> Mangpd = new List<string>();
                        int idcates = ListGroup[i].id;
                        Mangpd = Arrayid(idcates);
                        Mangpd.Add(idcates.ToString());
                        var listProduct = db.tblProducts.Where(p => Mangpd.Contains(p.idCate.ToString()) && p.Active == true && p.idCate != idcate).OrderBy(p => p.Ord).ToList();

                            for (int j = 0; j < listProduct.Count; j++)
                            {
                                chuoi += "<div class=\"Tear_1\">";
                                if (listProduct[j].New == true)
                                {
                                    chuoi += "<div class=\"New\">";
                                    chuoi += "</div>";
                                }

                                chuoi += "<div class=\"img\">";
                                chuoi += "<a href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"" + listProduct[j].Name + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a>";
                                chuoi += "</div>";
                                chuoi += "<h3><a class=\"Name\" href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"\">" + listProduct[j].Name + "</a></h3>";
                                chuoi += "<p class=\"Price\">Giá : <span>" + string.Format("{0:#,#}", listProduct[j].Price) + " đ</span></p>";
                                chuoi += "<p class=\"PriceSale\"> Khuyến mại : <span>" + string.Format("{0:#,#}", listProduct[j].PriceSale) + " đ</span></p>";
                                chuoi += " </div> ";

                            }
                            Mangphantu.Clear();
              
                chuoi += "</div>";
                chuoi += "</div>";
                               Mangphantu.Clear();

                }

            }
            ViewBag.chuoi = chuoi;

            //URL
            ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"http://BinhnuocnonglanhAriston.vn\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   ›" + UrlNews(groupPproduct.id) + "</ol> <h1>" + groupPproduct.Title + "</h1>";

            return View();


        }
        public ActionResult ListCapacity(string tag)
        {

            string chuoi = "";
            var tblcapacity = db.tblCapacities.First(p => p.Tag == tag);
            int idcap = tblcapacity.id;
            ViewBag.Title = "<title>" + tblcapacity.Title + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + tblcapacity.Title + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblcapacity.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblcapacity.Keyword + "\" /> ";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Binhnuocnonglanhariston.vn/" + StringClass.NameToTag(tblcapacity.Tag) + ".html\" />";
            string meta = "";
            meta += "<meta itemprop=\"name\" content=\"" + tblcapacity.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + tblcapacity.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"" + tblcapacity.Images + "\" />";
            meta += "<meta property=\"og:title\" content=\"" + tblcapacity.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"" + tblcapacity.Images + "\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://Binhnuocnonglanhariston.vn\" />";
            meta += "<meta property=\"og:description\" content=\"" + tblcapacity.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta;
            chuoi += "<div class=\"Product\">";
            chuoi += "<div class=\"nVar\">";
            chuoi += "<div class=\"Left\">";
            chuoi += "<span> " + tblcapacity.Name + "</span>";
            chuoi += "</div>";
            chuoi += "<div class=\"Right\">";
            chuoi += "<select class=\"Arrange\">";
            chuoi += "<option value=\"0\" selected=\"selected\">Sắp xếp theo giá</option> ";
            chuoi += "<option value=\"1\">Tăng dần</option>";
            chuoi += "<option value=\"2\">Giảm dần</option>";
            chuoi += "</select>";
            chuoi += "</div>";
            chuoi += "</div>";
            chuoi += "<div class=\"Content\">";
            List<string> Mangpd = new List<string>();
            var listProduct = db.tblProducts.Where(p => p.Capacity == idcap && p.Active == true).OrderBy(p => p.Ord).ToList();
            for (int j = 0; j < listProduct.Count; j++)
            {
                chuoi += "<div class=\"Tear_1\">";
                if (listProduct[j].New == true)
                {
                    chuoi += "<div class=\"New\">";
                    chuoi += "</div>";
                }

                chuoi += "<div class=\"img\">";
                chuoi += "<a href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"" + listProduct[j].Name + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a>";
                chuoi += "</div>";
                chuoi += "<a class=\"Name\" href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"\">" + listProduct[j].Name + "</a>";
                chuoi += "<p class=\"Price\">Giá : <span>" + string.Format("{0:#,#}", listProduct[j].Price) + " đ</span></p>";
                chuoi += "<p class=\"PriceSale\"> Khuyến mại : <span>" + string.Format("{0:#,#}", listProduct[j].PriceSale) + " đ</span></p>";
                chuoi += " </div> ";

            }
            chuoi += "</div>";
            chuoi += "</div>";
            ViewBag.chuoi = chuoi;
            ViewBag.nUrl = "<a href=\"http://BinhnuocnonglanhAriston.vn\" title=\"Trang chu\" rel=\"nofollow\"><span class=\"iCon\"></span>Trang chủ</a> / <h1>"+tblcapacity.Title+"</h1>";
            ViewBag.Content = tblcapacity.Content;            
            return View();
        }
        public ActionResult detail(string tag)
        {
            var config = db.tblConfigs.FirstOrDefault();

            ViewBag.Title = "<title> " + config.TitleSale + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + config.TitleSale + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + config.TitleSale + "\" /> ";

            var listProductSalePriority = db.tblProducts.Where(p => p.Active == true && p.Priority == true && p.ProductSale == true).OrderBy(p => p.Ord).ToList();
            StringBuilder resultPriority = new StringBuilder();
            foreach (var item in listProductSalePriority)
            {
                resultPriority.Append("<div class=\"item\">");
                resultPriority.Append("<div class=\"contentItem\">");
                resultPriority.Append("<div class=\"img\">");
                resultPriority.Append("<a href=\"/Product/" + item.Tag + "-"+item.idCate+ "-" + item.id + ".aspx\" title=\"" + item.Name + "\"><img src=\"" + item.ImageLinkThumb + "\" title=\"" + item.Name + "\" /></a>");
                resultPriority.Append("</div>");
                resultPriority.Append("<a href=\"/Product/" + item.Tag + "-" + item.idCate + "-" + item.id + ".aspx\" title=\"" + item.Name + "\" class=\"name\">" + item.Name + "</a>");
                resultPriority.Append("<div class=\"buy\">");
                resultPriority.Append("<span class=\"note\">Giá chỉ từ</span>");
                resultPriority.Append("<span class=\"price\">" + string.Format("{0:#,#}", item.PriceSale) + "<samp>đ</samp></span>");
                resultPriority.Append("<a href=\"/Product/" + item.Tag + "-" + item.idCate + "-" + item.id + ".aspx\" title=\"" + item.Name + "\">Xem ngay  &raquo;</a>");
                resultPriority.Append("</div>");
                resultPriority.Append("</div>");
                resultPriority.Append("</div>");
            }
            ViewBag.resultPriority = resultPriority.ToString();


            //var listProductSyn = db.tblProductSyns.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            //if (listProductSyn.Count > 0)
            //{
            //    ViewBag.CheckSyn = "oke";
            //    StringBuilder resultSyn = new StringBuilder();
            //    foreach (var item in listProductSyn)
            //    {
            //        resultSyn.Append(" <div class=\"item\">");
            //        resultSyn.Append("<div class=\"contentItem\">");
            //        resultSyn.Append("<div class=\"img\">");
            //        resultSyn.Append("<a href=\"/syn/" + item.Tag + "\" title=\"" + item.Name + "\"><img src=\"" + item.ImageLinkDetail + "\" title=\"" + item.Name + "\" /></a>");
            //        resultSyn.Append(" </div>");
            //        resultSyn.Append("<a href=\"/syn/" + item.Tag + "\" title=\"" + item.Name + "\" class=\"name\">" + item.Name + "</a>");
            //        resultSyn.Append("<div class=\"buy\">");
            //        resultSyn.Append("<span class=\"note\">Giá chỉ từ</span>");
            //        resultSyn.Append("<span class=\"price\">" + string.Format("{0:#,#}", item.PriceSale) + "<samp>đ</samp></span>");
            //        resultSyn.Append("<a href=\"/syn/" + item.Tag + "\" title=\"" + item.Name + "\" >Xem ngay  &raquo;</a>");
            //        resultSyn.Append("</div>");
            //        resultSyn.Append("</div>");
            //        resultSyn.Append("</div>");
            //    }
            //    ViewBag.resultSyn = resultSyn.ToString();
            //}

            var listProductSale = db.tblProducts.Where(p => p.Active == true && p.ProductSale == true).OrderBy(p => p.idCate).ToList();
            StringBuilder resultSale = new StringBuilder();
            foreach (var item in listProductSale)
            {
                resultSale.Append("<div class=\"item\">");
                resultSale.Append("<div class=\"contentItem\">");
                resultSale.Append("<div class=\"img\">");
                resultSale.Append("<a href=\"/Product/" + item.Tag + "-" + item.idCate + "-" + item.id + ".aspx\" title=\"" + item.Name + "\"><img src=\"" + item.ImageLinkThumb + "\" title=\"" + item.Name + "\" /></a>");
                resultSale.Append("</div>");
                resultSale.Append("<a href=\"/Product/" + item.Tag + "-" + item.idCate + "-" + item.id + ".aspx\" title=\"" + item.Name + "\" class=\"name\">" + item.Name + "</a>");
                resultSale.Append("<div class=\"buy\">");
                resultSale.Append("<span class=\"note\">Giá chỉ từ</span>");
                resultSale.Append("<span class=\"price\">" + string.Format("{0:#,#}", item.PriceSale) + "<samp>đ</samp></span>");
                resultSale.Append("<a href=\"/Product/" + item.Tag + "-" + item.idCate + "-" + item.id + ".aspx\" title=\"" + item.Name + "\">Xem ngay  &raquo;</a>");
                resultSale.Append("</div>");
                resultSale.Append("</div>");

                resultSale.Append("</div>");
            }
            ViewBag.resultSale = resultSale.ToString();
            return View(config);
        }

    }
}
