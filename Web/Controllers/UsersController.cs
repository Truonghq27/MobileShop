using Models;
using Models.Models.DataModels;
using Models.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Data.Entity;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Web.Areas.Admin.Models;
using Facebook;
using System.Configuration;

namespace Web.Controllers
{
    public class UsersController : BaseController
    {
        private Uri RediredtUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        MobileShopContext db = new MobileShopContext();
        // GET: Users/ information current user
        [CustomersAutherize]
        public ActionResult Index()
        {
            int? id = int.Parse(Request.Cookies["InfoCustomer"]["Id"]);
            if (id == null)
            {
                return HttpNotFound();
            }
            return View(db.Customers.Where(u => u.CustomerId == id).SingleOrDefault());
        }
        [HttpPost]
        [CustomersAutherize]
        public ActionResult Index(Customer c, HttpPostedFileBase imageUpload)
        {
            HttpCookie cookie = Request.Cookies["InfoCustomer"];
            var id = cookie["id"];
            if (id == null)
            {
                return HttpNotFound();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    var current = db.Customers.Find(int.Parse(id));
                    current.FullName = c.FullName;
                    current.DateofBirth = c.DateofBirth;
                    current.Gender = c.Gender;
                    if (imageUpload != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(imageUpload.FileName);
                        string extention = Path.GetExtension(imageUpload.FileName);
                        filename = filename + extention;
                        string extentionLower = extention.ToLower();
                        var extLower = extentionLower.Substring(extentionLower.LastIndexOf('.') + 1);
                        if (extLower == "png" || extLower == "jpg" || extLower == "jpeg")
                        {
                            string fullpath = Path.Combine(Server.MapPath("~/Images/"), filename);
                            current.Avatar = filename;
                            imageUpload.SaveAs(fullpath);
                            db.SaveChanges();
                            cookie["Name"] = current.FullName;
                            cookie["Avatar"] = current.Avatar;
                            Response.Cookies.Add(cookie);
                            cookie.Expires = DateTime.Now.AddDays(2);
                            ModelState.Clear();
                            setAlert("Success !", "Cập nhập thông tin thành công.", "bottom-left", "success", 5000);
                            return RedirectToAction("Index", "Users");
                        }
                        else
                        {
                            setAlert("Lỗi !", "Hãy chọn đúng định dạng đuôi .png, jpg, .jpeg", "bottom-left", "error", 5000);
                            return RedirectToAction("Index", "Users");
                        }
                    }
                    else
                    {
                        db.SaveChanges();
                        cookie["Name"] = current.FullName;
                        cookie["Avatar"] = current.Avatar;
                        Response.Cookies.Add(cookie);
                        cookie.Expires = DateTime.Now.AddDays(2);
                        ModelState.Clear();
                        setAlert("Success !", "Cập nhập thông tin thành công.", "bottom-left", "success", 5000);
                        return RedirectToAction("Index", "Users");
                    }
                }
                return View(c);
            }
            catch (Exception)
            {
                setAlert("Lỗi !", "Không thể chỉnh sửa thông tin", "bottom-left", "error", 5000);
                return RedirectToAction("Index", "Users");
            }

        }

        // GET: Users/ ListOrders
        [CustomersAutherize]
        public ActionResult Orders()
        {
            if (Request.Cookies["InfoCustomer"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var id = int.Parse(Request.Cookies["InfoCustomer"]["Id"]);
            return View(db.Orders.Where(o => o.CustomerId == id).Include(x => x.OrderDetails).AsEnumerable());
        }
        [CustomersAutherize]
        public ActionResult OrderDetail(string id)
        {
            if (Request.Cookies["InfoCustomer"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return RedirectToAction("Orders");
            }
            var userid = int.Parse(Request.Cookies["InfoCustomer"]["Id"]);
            Order orders = db.Orders.Where(o => o.CodeOrder == id).Include(o => o.OrderDetails).AsEnumerable().SingleOrDefault();
            if (orders == null)
            {
                return RedirectToAction("Orders");
            }
            return View(orders);
        }
        //JSON: Users/ canceled order
        [HttpPost]
        public JsonResult CaneledOrder(int id)
        {
            var result = db.Orders.Where(o => o.OrderId == id).SingleOrDefault();
            if (result != null)
            {
                if ((result.Status == 0 || result.Status == 1) && result.TimeExpires >= DateTime.Now)
                {
                    result.Status = -2;//CaneledOrder with status = -2;
                    db.SaveChanges();
                    return Json(new { success = "Huỷ đơn hàng thành công !" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "Bạn không thể huỷ đơn hàng" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { error = "Có gì đó không đúng" }, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomersAutherize]
        public ActionResult Address()
        {
            return View();
        }

        // POST: Users/ Login - Register - Logout - returnUrl
        private ActionResult RedireactToLocal(string next)
        {
            if (Url.IsLocalUrl(next))
            {
                return Redirect(next);
            }
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public ActionResult Login(string next)
        {
            ViewBag.ReturnUrl = next;
            if (Request.Cookies["InfoCustomer"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginCustomer c, string next)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = db.Customers.SingleOrDefault(x => x.Email == c.Email);
                    if (user != null)
                    {
                        var checkpwd = BCrypt.Net.BCrypt.Verify(c.Password, user.Password);
                        if (checkpwd)
                        {
                            HttpCookie cookie = new HttpCookie("InfoCustomer");
                            cookie["id"] = user.CustomerId.ToString();
                            cookie["Email"] = user.Email;
                            cookie["Avatar"] = user.Avatar;
                            cookie["CreateDate"] = user.CreateDate.ToString("dd/MM/yyyy HH:mm");
                            cookie.Expires = DateTime.Now.AddDays(2);
                            Response.Cookies.Add(cookie);
                            if (c.RememberMe == true)
                            {
                                Response.Cookies["Email"].Value = c.Email;
                                Response.Cookies["Email"].Expires = DateTime.Now.AddDays(15);
                                ViewBag.Email = Request.Cookies["Email"].Value;
                            }
                            setAlert("", "Đăng nhập thành công !", "top-right", "success", 5000);
                            return RedireactToLocal(next);
                        }
                        else
                        {
                            ModelState.AddModelError("Email", "Email hoặc mật khẩu không chính xác");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "Email hoặc mật khẩu không chính xác");
                    }
                }
                catch (Exception)
                {
                    setAlert("Lỗi!", "Không thể đăng nhập vào lúc này, vui lòng thử lại sau.", "bottom-left", "error", 5000);
                    return View(c);
                }
            }
            return View(c);
        }

        public ActionResult LoginFacebook()
        {
            var facebook = new FacebookClient();
            var loginUrl = facebook.GetLoginUrl(new
            {
                client_id = "306811353878359",
                client_secret = "2162640f817925fed0799a5047d89a70",
                redirect_uri = RediredtUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });
            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallback(string code)
        {
            var facebook = new FacebookClient();
            dynamic result = facebook.Post("oauth/access_token", new
            {
                client_id = "306811353878359",
                client_secret = "2162640f817925fed0799a5047d89a70",
                redirect_uri = RediredtUri.AbsoluteUri,
                code = code
            });
            var accessToken = result.access_token;
            if (!String.IsNullOrEmpty(accessToken))
            {
                facebook.AccessToken = accessToken;
                dynamic me = facebook.Get("me?fields=link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");
                string email = me.email;
                string userName = me.email;
                string firstname = me.first_name;
                string middleName = me.middle_name;
                string lastName = me.last_name;
                var resultcustomer = db.Customers.FirstOrDefault(x => x.Email == email);
                if (resultcustomer == null)
                {
                    Customer customer = new Customer();
                    customer.Email = email;
                    customer.FullName = firstname + " " + middleName + " " + lastName;
                    customer.Avatar = me.picture.data.url;
                    customer.Status = 0;
                    customer.ActiveCode = Guid.NewGuid();
                    customer.DateofBirth = new DateTime(1960, 01, 01);
                    customer.CreateDate = DateTime.Now;
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    HttpCookie cookie = new HttpCookie("InfoCustomer");
                    cookie["id"] = customer.CustomerId.ToString();
                    cookie["Email"] = customer.Email;
                    cookie["Avatar"] = customer.Avatar;
                    cookie["CreateDate"] = customer.CreateDate.ToString("dd/MM/yyyy HH:mm");
                    cookie.Expires = DateTime.Now.AddDays(2);
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    HttpCookie cookie = new HttpCookie("InfoCustomer");
                    cookie["id"] = resultcustomer.CustomerId.ToString();
                    cookie["Email"] = resultcustomer.Email;
                    cookie["Avatar"] = resultcustomer.Avatar;
                    cookie["CreateDate"] = resultcustomer.CreateDate.ToString("dd/MM/yyyy HH:mm");
                    cookie.Expires = DateTime.Now.AddDays(2);
                    Response.Cookies.Add(cookie);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            if (Request.Cookies["InfoCustomer"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(CustomerViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Customer cus = new Customer();
                    if (db.Customers.Any(x => x.Email == c.Email))
                    {
                        setAlert("Lỗi!", "Email đã được sử dụng.", "bottom-left", "error", 12000);
                        return View(c);
                    }
                    cus.FullName = c.FullName;
                    cus.Email = c.Email;
                    var pwd = BCrypt.Net.BCrypt.HashPassword(c.Password);
                    cus.Password = pwd;
                    cus.CreateDate = DateTime.Now;
                    cus.Status = 1;
                    db.Customers.Add(cus);
                    db.SaveChanges();
                    if (Request.Cookies["Email"] != null)
                    {
                        Response.Cookies["Email"].Expires = DateTime.Now.AddDays(-1);
                    }
                    setAlert("Success", "Đăng ký tài khoản thành công !", "bottom-left", "success", 5000);
                    return RedirectToAction("Login");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Có lỗi khi đăng ký, vui lòng thử lại sau !");
                    return View(c);
                }
            }
            return View();
        }

        [CustomersAutherize]
        public ActionResult Logout(string next)
        {
            Response.Cookies["InfoCustomer"].Expires = DateTime.Now.AddDays(-2);
            Response.Cookies["Avatar"].Expires = DateTime.Now.AddDays(-2);
            return RedireactToLocal(next);
        }

        [CustomersAutherize]
        public ActionResult ChangePassword()
        {
            int? id = int.Parse(Request.Cookies["InfoCustomer"]["Id"]);
            if (id == null)
            {
                return HttpNotFound();
            }
            return View();
        }
        [HttpPost]
        [CustomersAutherize]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            int? id = int.Parse(Request.Cookies["InfoCustomer"]["Id"]);
            if (id == null)
            {
                return HttpNotFound();
            }
            Customer customer = db.Customers.Where(c => c.CustomerId == id).SingleOrDefault();
            if (ModelState.IsValid)
            {
                if (customer != null)
                {
                    var checkpwd = BCrypt.Net.BCrypt.Verify(model.Password, customer.Password);
                    if (checkpwd)
                    {
                        if (model.Password == model.NewPassword)
                        {
                            ModelState.AddModelError("error", "Mật khẩu mới phải khác mật khẩu cũ.");
                            return View(model);
                        }
                        customer.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
                        db.SaveChanges();
                        setAlert("Success", "Thay dổi mật khẩu thành công, vui lòng đăng nhập lại !", "bottom-left", "success", 5000);
                        Response.Cookies["InfoCustomer"].Expires = DateTime.Now.AddDays(-2);
                        Response.Cookies["Avatar"].Expires = DateTime.Now.AddDays(-2);
                        return RedirectToAction("Login", "Users");
                    }
                    else
                    {
                        ModelState.AddModelError("error", "Mật khẩu cũ không đúng");
                    }
                }
                else
                {
                    ModelState.AddModelError("null", "Không thể thay đổi mật khẩu vào lúc này, vui lòng thử lại sau !");
                }
            }
            return View(model);
        }

        /// <summary>
        /// send email to Users (non action)
        /// </summary>
        /// <param name="email"></param>
        /// <param name="activeCode"></param>
        /// <param name="emailFor"></param>
        [NonAction]
        public void SendVerifiedLinkEmail(string email, string activeCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/Users/" + emailFor + "/" + activeCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("c1709h@gmail.com", "ResetPassWord no-reply");
            var toEmail = new MailAddress(email);
            string subject = "";
            string body = "";
            if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "<br/><br/>Đã có một yêu cầu đặt đặt lại mật khẩu web Mobile Shop" +
            "nếu đó là bạn, vui lòng click vào link bên dưới, link sẽ tồn tại trong 24h." +
            " <br/><br/><a href='" + link + "'>" + link + "</a>";
            }

            var smtp = new SmtpClient();
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        // POST: Users/forgot pwd
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgotPassword(string Email)
        {
            if (Request.Cookies["InfoCustomer"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            string success = "";
            string error = "";
            var account = db.Customers.Where(x => x.Email == Email).SingleOrDefault();
            if (account != null)
            {
                //Send Email for reset pwd
                string resetCode = Guid.NewGuid().ToString();
                SendVerifiedLinkEmail(account.Email, resetCode, "ResetPassword");
                account.ResetPasswordCode = resetCode;
                account.ExpiredTime = DateTime.Now.AddDays(1);
                //
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                success = "Một email đã được gửi đến bạn, vui lòng kiểm tra hộp thư trong Email của bạn";
            }
            else
            {
                error = "Email không hợp lệ hoặc được đăng ký";
            }
            ViewBag.error = error;
            ViewBag.success = success;
            return View();
        }

        // POST: Users/Reset pwd - res_pwd confirm
        [AllowAnonymous]
        public ActionResult ResetPassword(string id)
        {
            if (Request.Cookies["InfoCustomer"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return HttpNotFound();
            }
            //verify the reset password link
            var user = db.Customers.Where(x => x.ResetPasswordCode == id).FirstOrDefault();
            if (user != null)
            {
                if (user.ExpiredTime < DateTime.Now)
                {
                    user.ResetPasswordCode = "";
                    user.ExpiredTime = null;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                ResetPasswordModel model = new ResetPasswordModel();
                model.ResetCode = id;
                return View(model);
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (Request.Cookies["InfoCustomer"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            //verify the reset password link
            var success = "";
            if (ModelState.IsValid)
            {
                var cus = db.Customers.Where(x => x.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                if (cus != null)
                {
                    if (cus.ExpiredTime < DateTime.Now)
                    {
                        cus.ResetPasswordCode = "";
                        cus.ExpiredTime = null;
                        db.SaveChanges();
                        return RedirectToAction("Index", "Home");
                    }
                    cus.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
                    cus.ResetPasswordCode = "";
                    cus.ExpiredTime = null;
                    db.SaveChanges();
                    success = "Mật khẩu mới đã được thay đổi thành công, hãy thử ";
                    ViewBag.success = success;
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        // View Partial: Users/right header
        public PartialViewResult RightHeader()
        {
            if (Request.Cookies["InfoCustomer"] == null)
            {
                return PartialView("_RightHeader");
            }
            int? _id = int.Parse(Request.Cookies["InfoCustomer"]["Id"]);
            return PartialView("_RightHeader", db.Customers.Find(_id));
        }

        // View Partial: Users/right header
        public PartialViewResult MainleftUser()
        {
            if (Request.Cookies["InfoCustomer"] == null)
            {
                return PartialView("_MainleftUser");
            }
            int? _id = int.Parse(Request.Cookies["InfoCustomer"]["Id"]);
            if (_id == null)
            {
                return PartialView("_MainleftUser");
            }
            return PartialView("_MainleftUser", db.Customers.Find(_id));
        }
    }
}