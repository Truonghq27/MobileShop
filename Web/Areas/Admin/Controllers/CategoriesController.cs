using Models;
using Models.Models.DataModels;
using Models.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Web.Controllers;

namespace Web.Areas.Admin.Controllers
{
    public class CategoriesController : BaseController
    {
        MobileShopContext db = new MobileShopContext();
        // GET: Admin/Categories
        public ActionResult Index()
        {
            return View(db.Categories.Where(x => (x.Status == 1 || x.Status == 0) && x.ParentId == null).OrderBy(x => x.Orderby).ToList());
        }
        public ActionResult Getdata()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var cate = db.Categories.Where(x => x.Status == 1 && x.ParentId == null).OrderBy(x => x.Orderby).ToList();
            return Json(new { data = cate }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Create Categories
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.ParentId = new SelectList(db.Categories.Where(x => x.Status == 1 && x.ParentId == null).OrderBy(x => x.Orderby).ToList(), "CategoryId", "CategoryName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateCategories c)
        {
            ViewBag.ParentId = new SelectList(db.Categories.Where(x => x.Status == 1 && x.ParentId == null).OrderBy(x => x.Orderby).ToList(), "CategoryId", "CategoryName");
            if (ModelState.IsValid)
            {
                var sortOderbyNull = db.Categories.Where(x => x.ParentId == null && x.Status != 10).OrderBy(x => x.Orderby).Count();
                var sortOderbyNotNull = db.Categories.Where(x => x.ParentId != null && x.Status != 10).OrderBy(x => x.Orderby).Count();
                Category cate = new Category();
                if (c.ParentId == null)
                {
                    cate.Orderby = sortOderbyNull + 1;
                }
                else
                {
                    cate.Orderby = sortOderbyNotNull + 1;
                }
                cate.Status = 1;
                cate.CategoryName = c.CategoryName;
                cate.ParentId = c.ParentId;
                db.Categories.Add(cate);
                db.SaveChanges();
                if (cate.ParentId == null)
                {
                    return RedirectToAction("Index", "Categories");
                }
                else
                {
                    return RedirectToAction("ParentCate", "Categories");
                }
            }
            return View(c);
        }

        /// <summary>
        /// Edit Categories
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("Unauthorized");
            }
            var category = db.Categories.Where(x => x.CategoryId == id).SingleOrDefault();
            if (category == null)
            {
                return View("Unauthorized");
            }
            var findThisHasParentCategory = db.Categories.Where(x => x.ParentId != null && x.ParentId == category.CategoryId).Count();
            if (findThisHasParentCategory > 0)
            {
                ViewBag.showmsg = "Không thể chọn làm danh mục con vì danh mục này đã chứa danh mục con !";
                return View(category);

            }
            ViewBag.ParentId = new SelectList(db.Categories.Where(x => x.Status == 1 && x.ParentId == null && x.CategoryId != id).OrderBy(x => x.Orderby).ToList(), "CategoryId", "CategoryName", category.ParentId);
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category c)
        {
            ViewBag.ParentId = new SelectList(db.Categories.Where(x => x.Status == 1 && x.ParentId == null && x.CategoryId != c.CategoryId).OrderBy(x => x.Orderby).ToList(), "CategoryId", "CategoryName", c.ParentId);
            var editCate = db.Categories.SingleOrDefault(x => x.CategoryId == c.CategoryId);
            var listToOrderingParentNull = db.Categories.Where(x => (x.Status == 0 || x.Status == 1) && x.ParentId == null).ToList();
            var listToOrderingParentNotNull = db.Categories.Where(x => (x.Status == 0 || x.Status == 1) && x.ParentId != null).ToList();
            var sortParentCategory = 0;
            var sortCategory = 0;
            if (ModelState.IsValid)
            {
                try
                {
                    if (editCate != null)
                    {
                        editCate.CategoryName = c.CategoryName;
                        editCate.Status = c.Status;
                        editCate.ParentId = c.ParentId;
                        editCate.Orderby = c.Orderby;
                        if (c.ParentId == null)
                        {
                            var sortOrderby = c.Orderby;
                            foreach (var item in listToOrderingParentNull)
                            {
                                if (item.CategoryId != c.CategoryId && item.Orderby >= c.Orderby)
                                {
                                    item.Orderby = ++sortOrderby;
                                }
                            }
                            foreach (var item in listToOrderingParentNotNull)
                            {
                                item.Orderby = ++sortParentCategory;
                            }
                        }
                        else
                        {
                            var sortOrderby = c.Orderby;
                            foreach (var item in listToOrderingParentNotNull)
                            {
                                if (item.CategoryId != c.CategoryId && item.Orderby >= c.Orderby)
                                {
                                    item.Orderby = ++sortOrderby;
                                }
                            }
                            foreach (var item in listToOrderingParentNull)
                            {
                                item.Orderby = ++sortCategory;
                            }
                        }
                        db.SaveChanges();
                        if (editCate.ParentId == null)
                        {
                            return RedirectToAction("Index", "Categories");
                        }
                        return RedirectToAction("ParentCate", "Categories");
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("Edit", "Categories");
                }
            }
            return View(c);
        }
        [HttpPost]

        //JSON/Categories/delete category
        public JsonResult Delete(int id)
        {
            var result = db.Categories.Where(x => (x.Status == 0 || x.Status == 1) && x.CategoryId == id).SingleOrDefault();
            var sortOrderby = db.Categories.Where(x => (x.Status == 0 || x.Status == 1) && x.ParentId == null && x.CategoryId != id).OrderBy(x => x.Orderby).ToList();
            var sortOrderbyParent = db.Categories.Where(x => (x.Status == 0 || x.Status == 1) && x.ParentId != null && x.CategoryId != id).OrderBy(x => x.Orderby).ToList();
            if (result != null)
            {
                result.Status = 10; //delete with status = 10;
                if (result.ParentId == null)
                {
                    var oderby = 1;
                    foreach (var item in sortOrderby)
                    {
                        item.Orderby = oderby++;
                    }
                }
                else
                {
                    var oderby = 1;
                    foreach (var item in sortOrderbyParent)
                    {
                        item.Orderby = oderby++;
                    }
                }
                db.SaveChanges();
                return Json(new { success = "Xoá thành công !" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = "Có gì đó không đúng !" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Categories Parent List and Create new Parnet use JSON
        /// </summary>
        /// <returns></returns>
        public ActionResult ParentCate()
        {
            return View();
        }
        public ActionResult GetdataParentCate()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = db.Categories.Where(x => (x.Status == 1 || x.Status == 0) && x.ParentId != null).OrderBy(x => x.Orderby).ToList();
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        //View/JSON/Providers/getall
        public ActionResult Providers()
        {
            return View();
        }
        public JsonResult GetdataProviders()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var data = db.Providers.Where(x => x.Status == 1 || x.Status == 0).ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        //JSON/Providers/create new provider
        public JsonResult CreateProvider(Provider provider)
        {
            if (ModelState.IsValid)
            {
                var countProvider = db.Providers.Where(x => x.Status != 10).Count();
                provider.Orderby = countProvider + 1;
                db.Providers.Add(provider);
                db.SaveChanges();
                return Json(new { success = "Thêm mới thành công !!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = "Có gì đó không đúng !!" }, JsonRequestBehavior.AllowGet);
            }
        }

        //JSON/Providers/Getid
        public JsonResult GetIdProvider(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var provider = db.Providers.Where(x => (x.Status == 1 || x.Status == 0) && x.ProviderId == id).FirstOrDefault();
            return Json(provider, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EditProvider(Provider provider)
        {
            var result = db.Providers.Where(x => (x.Status == 1 || x.Status == 0) && x.ProviderId == provider.ProviderId).FirstOrDefault();
            var sortProvider = db.Providers.OrderBy(x => x.Orderby).Where(x => x.Status != 10 && x.ProviderId != provider.ProviderId && x.Orderby >= provider.Orderby);
            if (result != null)
            {
                if (result.Orderby != provider.Orderby)
                {
                    var order = provider.Orderby;
                    foreach (var item in sortProvider)
                    {
                        item.Orderby = ++order;
                    }
                }
                result.Orderby = provider.Orderby;
                result.ProviderName = provider.ProviderName;
                result.Status = provider.Status;
                db.SaveChanges();
                return Json(new { success = "Chỉnh sửa thành công !!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = "Có gì đó không đúng !!" }, JsonRequestBehavior.AllowGet);
            }
        }

        //JSON/Providers/delete provider
        public JsonResult DeleteProvider(int id)
        {
            var provider = db.Providers.Where(x => (x.Status == 1 || x.Status == 0) && x.ProviderId == id).FirstOrDefault();
            if (provider != null)
            {
                provider.Status = 10; //delete with status = 10;
                db.SaveChanges();
                return Json(new { success = "Xoá thành công !!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = "Có gì đó không đúng!!" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}