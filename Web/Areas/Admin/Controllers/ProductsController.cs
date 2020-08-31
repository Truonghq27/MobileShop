using Models;
using Models.Models.DataModels;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Areas.Admin.Models;
using Web.Controllers;

namespace Web.Areas.Admin.Controllers
{
    [CustomAuth(Roles = "VIEW")]
    public class ProductsController : BaseController
    {
        MobileShopContext db = new MobileShopContext();
        // GET: Admin/Products
        public ActionResult Index()
        {
            return View(db.Products.Where(x => x.Status == true).ToList());
        }

        //[CustomAuth(Roles = "ADD")]
        public ActionResult Create()
        {
            var countProvider = db.Providers.Where(x => x.Status == 1).Count();
            var countCategories = db.Categories.Where(x => x.Status == 1).Count();
            ViewBag.ProviderId = new SelectList(db.Providers.Where(x => x.Status == 1), "ProviderId", "ProviderName");
            ViewBag.category = db.Categories.ToList();
            if (countProvider <= 0)
            {
                setAlert("Error !", "Chưa có thương hiệu, vui lòng thêm mới thương hiệu trước khi thêm mới sản phẩm !!", "top-right", "error", 7000);
                return RedirectToAction("Providers", "Categories");
            }
            if (countCategories <= 0)
            {
                setAlert("Error !", "Chưa có danh mục, vui lòng thêm mới danh mục trước khi thêm mới sản phẩm !!", "top-right", "error", 7000);
                return RedirectToAction("Index", "Categories");
            }
            //Lấy thuộc tính sản phẩm
            ViewBag.TypeAttr = db.TypeAttrs.Include(x => x.Attributes).Where(x => x.Attributes.Count() > 0).AsEnumerable();
            return View();
        }
        //[CustomAuth(Roles = "ADD")]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel p)
        {
            ViewBag.ProviderId = new SelectList(db.Providers.Where(x => x.Status == 1), "ProviderId", "ProviderName");
            ViewBag.category = db.Categories.ToList();
            ViewBag.TypeAttr = db.TypeAttrs.Include(x => x.Attributes).Where(x => x.Attributes.Count() > 0).AsEnumerable();
            if (ViewBag.ProviderId == null)
            {
                return RedirectToAction("CreateProvider", "Categories");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    Product product = new Product();
                    product.ProductName = p.ProductName;
                    product.CategoryId = p.CategoryId;
                    product.ProviderId = p.ProviderId;
                    product.PriceIn = p.PriceIn;
                    product.PriceOut = p.PriceOut;
                    product.Discount = p.Discount;
                    product.Quantity = p.Quantity;
                    product.FeatureImage = p.FeatureImage;
                    product.Images = p.Images;
                    product.Description = p.Description;
                    product.Specifications = p.Specifications;
                    product.ProductDetail = p.ProductDetail;
                    product.CreateDate = DateTime.Now;
                    product.Condition = p.Condition;//trạng thái sản phẩm
                    product.Status = true;
                    product.ProductAttrs = p.ProductAttrs;
                    db.Products.Add(product);
                    db.SaveChanges();
                    setAlert("Success !", "Bạn đã thêm mới sản phẩm thành công !", "top-right", "success", 4000);
                    return RedirectToAction("Index", "Products");
                }
                catch (Exception)
                {
                    setAlert("Error !", "Có lỗi khi thêm mới sản phẩm !", "top-right", "error", 5000);
                    return View(p);
                }
            }
            return View(p);
        }

        [ValidateInput(false)]
        //[CustomAuth(Roles = "EDIT")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("Unauthorized");
            }
            ViewBag.category = db.Categories.ToList();
            Product product = db.Products.Include(x => x.ProductAttrs).SingleOrDefault(x => x.ProductId == id);
            ViewBag.currentCategory = product.CategoryId;
            if (product == null || product.Status == false)
            {
                return View("Unauthorized");
            }
            //Lấy thuộc tính sản phẩm
            ViewBag.TypeAttr = db.TypeAttrs.Include(x => x.Attributes).Where(x => x.Attributes.Count() > 0).AsEnumerable();
            ViewBag.ProviderId = new SelectList(db.Providers.Where(x => x.Status == 1).ToList(), "ProviderId", "ProviderName", product.ProviderId);
            return View(product);
        }

        //[CustomAuth(Roles = "EDIT")]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product p)
        {
            ViewBag.category = db.Categories.ToList();
            ViewBag.TypeAttr = db.TypeAttrs.Include(x => x.Attributes).Where(x => x.Attributes.Count() > 0).AsEnumerable();
            ViewBag.ProviderId = new SelectList(db.Providers.Where(x => x.Status == 1).ToList(), "ProviderId", "ProviderName", p.ProviderId);
            if (ModelState.IsValid)
            {
                try
                {
                    var _product = db.Products.SingleOrDefault(x => x.ProductId == p.ProductId && x.Status == true);
                    if (_product != null)
                    {
                        if (p.FeatureImage != null)
                        {
                            _product.ProductName = p.ProductName;
                            _product.CategoryId = p.CategoryId;
                            _product.ProviderId = p.ProviderId;
                            _product.PriceIn = p.PriceIn;
                            _product.PriceOut = p.PriceOut;
                            _product.Discount = p.Discount;
                            _product.Quantity = p.Quantity;
                            _product.Description = p.Description;
                            _product.Specifications = p.Specifications;
                            _product.ProductDetail = p.ProductDetail;
                            _product.FeatureImage = p.FeatureImage;
                            _product.Images = p.Images;
                            _product.Condition = p.Condition;
                            db.ProductAttrs.RemoveRange(db.ProductAttrs.Where(x => x.ProductId == p.ProductId));
                            //thêm attr mới
                            if (p.ProductAttrs != null)
                            {
                                foreach (var item in p.ProductAttrs)
                                {
                                    item.ProductId = p.ProductId;
                                }
                                db.ProductAttrs.AddRange(p.ProductAttrs);
                                _product.ProductAttrs = p.ProductAttrs;
                            }
                            db.SaveChanges();
                            setAlert("Success !", "Sửa sản phẩm thành công !", "top-right", "success", 4000);
                            ModelState.Clear();
                            return RedirectToAction("Index", "Products");
                        }
                        else
                        {
                            ModelState.AddModelError("FeatureImage", "Vui lòng chọn ảnh sản phẩm");
                            return View(p);
                        }
                    }
                    else
                    {
                        setAlert("Error !", "Khôn tìm thấy sản phẩm !", "top-right", "error", 5000);
                        return View(p);
                    }

                }
                catch (Exception)
                {
                    setAlert("Error !", "Có lỗi khi sửa sản phẩm !", "top-right", "error", 5000);
                    return View(p);
                }
            }
            return View(p);
        }

        //[CustomAuth(Roles = "DELETE")]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            Product rs = db.Products.Where(x => x.ProductId == id && x.Status == true).SingleOrDefault();
            if (rs != null)
            {
                rs.Status = false;
                db.SaveChanges();
                return Json(new { success = "Xoá thành công !" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = "Có gì đó không đúng !" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Banner()
        {
            return View();
        }

        public JsonResult GetAllBanner()
        {
            var banner = db.Banners.Where(x => x.Status == 1 || x.Status == 0);
            return Json(new { data = banner }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateBanner()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateBanner(Banner banner)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var countBanner = db.Banners.Where(x => x.Status == 1 || x.Status == 0).Count();
                    if (banner.DescriptionBanner == null)
                        banner.DescriptionBanner = "Không có mô tả";
                    banner.Orderby = countBanner + 1;
                    db.Banners.Add(banner);
                    db.SaveChanges();
                    setAlert("Success !", "Thêm mới thành công !", "top-right", "success", 4000);
                    return RedirectToAction("Banner", "Products");
                }
                catch (Exception)
                {
                    setAlert("Error !", "Không thể thêm mới, vui lòng thử lại!", "top-right", "error", 4000);
                    return View(banner);
                }
            }
            return View(banner);
        }

        public ActionResult EditBanner(int? id)
        {
            if (id == null)
            {
                setAlert("Error !", "Không tìm thấy Banner!", "top-right", "error", 4000);
                return RedirectToAction("Banner", "Products");
            }
            Banner banner = db.Banners.Find(id);
            return View(banner);
        }
        [HttpPost]
        public ActionResult EditBanner(Banner banner)
        {
            var result = db.Banners.Where(x => x.BannerId == banner.BannerId).FirstOrDefault();
            if (result == null)
            {
                setAlert("Error !", "Không tìm thấy Banner!", "top-right", "error", 4000);
                return RedirectToAction("Banner", "Products");
            }
            try
            {
                var sortOrderby = db.Banners.Where(x => x.Status == 1 || x.Status == 0);
                result.DescriptionBanner = banner.DescriptionBanner;
                result.BannerImage = banner.BannerImage;
                result.Orderby = banner.Orderby;
                result.Status = banner.Status;
                int count = banner.Orderby;
                foreach (var item in sortOrderby)
                {
                    if (item.BannerId != banner.BannerId && item.Orderby >= banner.Orderby)
                    {
                        item.Orderby = ++count;
                    }
                }
                db.SaveChanges();
                setAlert("Success !", "Chỉnh sửa thành công thành công !", "top-right", "success", 4000);
                return RedirectToAction("Banner", "Products");
            }
            catch (Exception)
            {
                setAlert("Error !", "Không tìm thấy Banner!", "top-right", "error", 4000);
                return View(banner);
            }
        }

        public JsonResult DeleteBanner(int id)
        {
            var banner = db.Banners.FirstOrDefault(x => x.BannerId == id);
            var sortOrderby = db.Banners.Where(x => x.Status == 1 || x.Status == 0);
            if (banner != null)
            {
                banner.Status = -1; //
                db.SaveChanges();
                int count = 1;
                foreach (var item in sortOrderby)
                {
                    item.Orderby = count++;
                }
                db.SaveChanges();
                return Json(new { success = "Xoá thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { error = "Có lỗi khi xoá" }, JsonRequestBehavior.AllowGet);
        }
    }
}