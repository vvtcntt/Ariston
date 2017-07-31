using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ariston.Models;
using PagedList;
using PagedList.Mvc;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Xml;
using System.Data.Entity;
namespace Ariston.Controllers.Admin.Capacity
{
    public class CapacityadController : Controller
    {
         AristonContext db = new AristonContext();
        // GET: Capacityad
         public ActionResult Index(int? page, string id, FormCollection collection)
         {
             if ((Request.Cookies["Username"] == null))
             {
                 return RedirectToAction("LoginIndex", "Login");
             }
             if (ClsCheckRole.CheckQuyen(14, 0, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
             {
                 var ListCap = db.tblCapacities.OrderBy(p=>p.Ord).ToList();

                 const int pageSize = 20;
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
                 if (Session["Thongbao"] != null && Session["Thongbao"] != "")
                 {

                     ViewBag.thongbao = Session["Thongbao"].ToString();
                     Session["Thongbao"] = "";
                 }
                 if (collection["btnDelete"] != null)
                 {
                     foreach (string key in Request.Form.Keys)
                     {
                         var checkbox = "";
                         if (key.StartsWith("chk_"))
                         {
                             checkbox = Request.Form["" + key];
                             if (checkbox != "false")
                             {
                                 if (ClsCheckRole.CheckQuyen(14, 3, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
                                 {
                                     int ids = Convert.ToInt32(key.Remove(0, 4));
                                     tblCapacity tblcapacity = db.tblCapacities.Find(ids);
                                     db.tblCapacities.Remove(tblcapacity);
                                     db.SaveChanges();
                                     return RedirectToAction("Index");
                                 }
                                 else
                                 {
                                     return Redirect("/Users/Erro");

                                 }
                             }
                         }
                     }
                 }
                 return View(ListCap.ToPagedList(pageNumber, pageSize));
             }
             else
             {
                 return Redirect("/Users/Erro");

             }
         }
         public ActionResult UpdateCapacity(string id, string Active, string Ord)
         {
             if (ClsCheckRole.CheckQuyen(14, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
             {

                 int ids = int.Parse(id);
                 var tblcap = db.tblCapacities.Find(ids);
                 tblcap.Active = bool.Parse(Active);
                 tblcap.Ord = int.Parse(Ord);
                 db.SaveChanges();
                 var result = string.Empty;
                 result = "Thành công";
                 return Json(new { result = result });
             }
             else
             {
                 var result = string.Empty;
                 result = "Bạn không có quyền thay đổi tính năng này";
                 return Json(new { result = result });

             }

         }
         public ActionResult DeleteCapacity(int id)
         {
             if (ClsCheckRole.CheckQuyen(14, 3, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
             {
                 tblCapacity tblcap = db.tblCapacities.Find(id);
                 var result = string.Empty;
                 db.tblCapacities.Remove(tblcap);
                 db.SaveChanges();
                 result = "Bạn đã xóa thành công.";
                 return Json(new { result = result });
             }
             else
             {
                 var result = string.Empty;
                 result = "Bạn không có quyền thay đổi tính năng này";
                 return Json(new { result = result });

             }
         }
         public ActionResult Create()
         {
             if ((Request.Cookies["Username"] == null))
             {
                 return RedirectToAction("LoginIndex", "Login");
             }
             if (Session["Thongbao"] != null && Session["Thongbao"] != "")
             {

                 ViewBag.thongbao = Session["Thongbao"].ToString();
                 Session["Thongbao"] = "";
             }
             if (ClsCheckRole.CheckQuyen(14, 1, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
             {
                 var pro = db.tblCapacities.OrderByDescending(p => p.Ord).ToList();
                 if (pro.Count > 0)
                     ViewBag.Ord = pro[0].Ord + 1;
                 else
                 { ViewBag.Ord = "0"; }
                 return View();
             }
             else
             {
                 return Redirect("/Users/Erro");
             }
         }

         [HttpPost]
         [ValidateInput(false)]
         public ActionResult Create(tblCapacity tblcapacity, FormCollection collection)
         {
             tblcapacity.DateCreate = DateTime.Now;
             string idUser = Request.Cookies["Username"].Values["UserID"];
             tblcapacity.idUser = int.Parse(idUser);
             tblcapacity.Tag = StringClass.NameToTag(tblcapacity.Name);
             db.tblCapacities.Add(tblcapacity);
             db.SaveChanges();
             #region[Updatehistory]
             var Groups = db.tblCapacities.Where(p => p.Active == true).OrderByDescending(p => p.id).Take(1).ToList();
             string id = Groups[0].id.ToString();
             clsSitemap.CreateSitemap(StringClass.NameToTag(tblcapacity.Name)+".html" , id, "Capacity"); 
             Updatehistoty.UpdateHistory("Add tblcapacity", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
             #endregion
             if (collection["btnSave"] != null)
             {
                 Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã thêm thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";

                 return Redirect("/Capacityad/Index");
             }
             if (collection["btnSaveCreate"] != null)
             {
                 Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã thêm thành công, mời bạn thêm danh mục  mới !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                 return Redirect("/Capacityad/Create");
             }
             return Redirect("Index");


         }
         public ActionResult Edit(int id = 0)
         {

             if ((Request.Cookies["Username"] == null))
             {
                 return RedirectToAction("LoginIndex", "Login");
             }
             if (ClsCheckRole.CheckQuyen(14, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
             {
                 tblCapacity tblcap = db.tblCapacities.Find(id);
                 if (tblcap == null)
                 {
                     return HttpNotFound();
                 }
                 return View(tblcap);
             }
             else
             {
                 return Redirect("/Users/Erro");
             }
         }

         //
         // POST: /Users/Edit/5

         [HttpPost]
         [ValidateInput(false)]
         public ActionResult Edit(tblCapacity tblcapacity, int id, FormCollection collection)
         {
             if (ModelState.IsValid)
             {
                 string idUser = Request.Cookies["Username"].Values["UserID"];
                 tblcapacity.idUser = int.Parse(idUser);
                 bool URL = (collection["URL"] == "false") ? false : true;
                 if (URL == true)
                 {
                     tblcapacity.Tag = StringClass.NameToTag(tblcapacity.Name);
                 }
                 else
                 {
                     tblcapacity.Tag = collection["NameURL"];
                 }
                 tblcapacity.DateCreate = DateTime.Now;
                 db.Entry(tblcapacity).State = EntityState.Modified;
                 db.SaveChanges();
                 clsSitemap.UpdateSitemap(StringClass.NameToTag(tblcapacity.Name) + ".html", id.ToString(), "Capacity"); 
                 #region[Updatehistory]
                 Updatehistoty.UpdateHistory("Edit tblcapacity", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                 #endregion
                 if (collection["btnSave"] != null)
                 {
                     Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã sửa  thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";

                     return Redirect("/Capacityad/Index");
                 }
                 if (collection["btnSaveCreate"] != null)
                 {
                     Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã thêm thành công, mời bạn thêm mới !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                     return Redirect("/Capacityad/Create");
                 }
             }
             return View(tblcapacity);
         }
    }
}