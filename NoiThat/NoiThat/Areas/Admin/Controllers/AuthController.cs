using NoiThat.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoiThat.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        NoiThatDBContext db = new NoiThatDBContext();
        // GET: Admin/Auth
        public ActionResult Login()
        {
            ViewBag.Error = "";
            return View();
        }

        [HttpPost]
        public ActionResult DoLogin(FormCollection field)
        {
            ViewBag.Error = "";
            string username = field["username"];
            string password = field["password"];
            User user = db.Users
                .Where(m => (m.Roles == "Admin" || m.Roles == "Employ" )&& m.UserName == username)
                .FirstOrDefault();
            if (user != null)
            {
                if (user.CountError >= 5 && (user.Roles!="Admin" || user.Roles != "Employ"))
                {
                    ViewBag.Error = "<p class=\"login-box-msg text-danger\">Gõ bậy đánh chết cha mày giờ</p>";

                }
                else
                {
                    //Co tai khoan
                    if (user.Password.Equals(password))
                    {
                        user.CountError = 0;
                        Session["UserAdmin"] = username;
                        Session["UserId"] = user.id.ToString();
                        Session["FullName"] = user.FullName;
                        Session["Img"] = user.Img;
                        Session["Roles"] = user.Roles;
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        if (user.CountError == null)
                        {
                            user.CountError = 0;
                        }
                        else
                        {
                            user.CountError++;
                        }
                        db.Entry(User).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.Error = "<p class=\"login-box-msg text-danger\">Sai mật khẩu</p>";
                    }
                }
            }
            else
            {
                ViewBag.Error = "<p class=\"login-box-msg text-danger\">Tài khoản <strong>" + username + "</strong> không tồn tại</p>";
            }
            return View("Login");
        }

        public ActionResult Logout()
        {
            Session["UserAdmin"] = "";
            Session["UserId"] = "";
            Session["FullName"] = "";
            Session["Img"] = "";
            return Redirect("~/Admin/login");
        }
    }
}