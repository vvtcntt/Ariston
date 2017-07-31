using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ariston.Models;
using PagedList;
using PagedList.Mvc;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
namespace Ariston.Controllers.Admin.Address
{
    public class addressController : Controller
    {
        // GET: address
        private AristonContext db = new AristonContext();
        public ActionResult Index(int? page, string id, FormCollection collection)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            if (ClsCheckRole.CheckQuyen(4, 0, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                var listAddress = db.tblAddresses.ToList();

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
                                if (ClsCheckRole.CheckQuyen(4, 3, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
                                {
                                    int ids = Convert.ToInt32(key.Remove(0, 4));
                                    tblAddress tbladdress = db.tblAddresses.Find(ids);
                                    db.tblAddresses.Remove(tbladdress);
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
                return View(listAddress.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return Redirect("/Users/Erro");

            }
        }
        public ActionResult UpdateAddress(string id, string Active, string Ord)
        {
            if (ClsCheckRole.CheckQuyen(4, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {

                int ids = int.Parse(id);
                var tbladdress = db.tblAddresses.Find(ids);
                tbladdress.Active = bool.Parse(Active);
                tbladdress.Ord = int.Parse(Ord);
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
        public ActionResult DeleteAddress(int id)
        {
            if (ClsCheckRole.CheckQuyen(4, 3, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                tblAddress tbladdress = db.tblAddresses.Find(id);
                var result = string.Empty;
                db.tblAddresses.Remove(tbladdress);
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
            if (ClsCheckRole.CheckQuyen(4, 1, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                var pro = db.tblAddresses.OrderByDescending(p => p.Ord).ToList();
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
        public ActionResult Create(tblAddress tbladdress, FormCollection collection)
        {

            db.tblAddresses.Add(tbladdress);
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Add tbladdress", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            if (collection["btnSave"] != null)
            {
                Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã thêm thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";

                return Redirect("/address/Index");
            }
            if (collection["btnSaveCreate"] != null)
            {
                Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã thêm thành công, mời bạn thêm danh mục  mới !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                return Redirect("/address/Create");
            }
            return Redirect("Index");


        }
        public ActionResult Edit(int id = 0)
        {

            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            if (ClsCheckRole.CheckQuyen(4, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                tblAddress tbladdress = db.tblAddresses.Find(id);
                if (tbladdress == null)
                {
                    return HttpNotFound();
                }
                return View(tbladdress);
            }
            else
            {
                return Redirect("/Users/Erro");


            }
        }

        //
        // POST: /Users/Edit/5

        [HttpPost]
        public ActionResult Edit(tblAddress tbladdress, int id, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbladdress).State = EntityState.Modified;

                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit tbladdress", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                if (collection["btnSave"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã sửa  thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";

                    return Redirect("/address/Index");
                }
                if (collection["btnSaveCreate"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã thêm thành công, mời bạn thêm mới !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                    return Redirect("/address/Create");
                }
            }
            return View(tbladdress);
        }
        public ActionResult ProductAddress()
        {

            //address
            var listaddress = db.tblAddresses.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            var lstAddress = new List<SelectListItem>();
            foreach (var item in listaddress)
            {
                lstAddress.Add(new SelectListItem { Text = item.Name, Value = item.id.ToString() });
            }
            ViewBag.drAddress = new SelectList(lstAddress, "Value", "Text", 0);
            if (Session["Thongbao"]!=null && Session["Thongbao"]!="")
            {
                ViewBag.result = Session["Thongbao"].ToString();
                Session["Thongbao"] = "";
            }
            return View();
            //var result = string.Empty;
            //var tblproduct = db.tblProducts.FirstOrDefault(p => p.Code == Code);
            //if (tblproduct!=null)
            //{
            //    tblproduct.Address = idAddress;              
            //    db.SaveChanges();
            //    result = "Bạn đã sửa thành công nơi xuất xứ.";
            //}
            //return Json(new { result = result });
            
        }
        public ActionResult CommandEdit(FormCollection conlection)
        {


            var result = string.Empty;
            string code = conlection["ProductGetCode"];
            string idAddress= conlection["drAddress"];
            var tblproduct = db.tblProducts.FirstOrDefault(p => p.Code == code);
            int id=0;
            if (tblproduct != null)
            {
                if(idAddress!=null && idAddress != "") {
                tblproduct.Address = int.Parse(idAddress);
                    id= int.Parse(idAddress);
                    db.SaveChanges();
                }
                result = "Bạn đã sửa thành công nơi xuất xứ.";
            }
            if(id>0)
            Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã sửa thành công xuất xứ sản phẩm "+ tblproduct.Name + ", Xuất xứ : "+db.tblAddresses.FirstOrDefault(p=>p.id== id).Name +"! <button class=\"close\" data-dismiss=\"alert\">×</button></div>";
            return RedirectToAction("ProductAddress");

        }
        public JsonResult getCodeProduct(string q)
        {
            var listProduct = db.tblProducts.Where(p => p.Active == true && p.Code!=null).ToList();
            List<string> Mang = new List<string>();
            for (int i = 0; i < listProduct.Count; i++)
            {
                if (listProduct[i].Code.ToUpper().Contains(q.ToUpper()))
                        Mang.Add(listProduct[i].Code.ToString());
                 
            }
            
            return Json(new
            {
                data = Mang.Take(4),
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        public string CheckProduct(string text)
        {
            string chuoi = "";
            var listProduct = db.tblProducts.Where(p => p.Code == text).ToList();
            if (listProduct.Count > 0)
            {
                chuoi = "Bạn đang sửa xuất xứ cho sản phẩm : " + listProduct[0].Name + " ?";

            }
            Session["Check"] = listProduct.Count;
            return chuoi;
        }

    }
}