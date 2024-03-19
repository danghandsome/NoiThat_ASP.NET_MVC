using Facebook;
using GoogleAuthentication.Services;
using Newtonsoft.Json;
using NoiThat.DAO;
using NoiThat.Library;
using NoiThat.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Xml.Linq;

namespace NoiThat.Controllers
{
    public class KhachhangController : Controller
    {
        // GET: Khachhang
        UserDAO userDAO = new UserDAO();
        OrderDAO orderDAO = new OrderDAO();
        OrderDetailDAO detailDAO=new OrderDetailDAO();
        ProductDAO productDAO = new ProductDAO();
        public ActionResult DangNhap()
        {
            ViewBag.Error = "";
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "1246301172713746",
                redirect_uri = "https://localhost:44346/Khachhang/FacebookRedirect",
                scope = "public_profile,email"
            });
            ViewBag.Url = loginUrl;

            var ClientID = "679615721941-oea3muamldbssq6ccg57d1m4b27v6ddf.apps.googleusercontent.com";
            var url = "https://localhost:44346/Khachhang/GoogleLoginCallback";
            var response = GoogleAuth.GetAuthUrl(ClientID, url);
            ViewBag.response = response;

            return View();
        }

        public async Task<ActionResult> GoogleLoginCallback(string code)
        {

            var ClientSecret = "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";
            var ClientID = "679615721941-oea3muamldbssq6ccg57d1m4b27v6ddf.apps.googleusercontent.com";
            var url = "https://localhost:44346/Khachhang/GoogleLoginCallback";
            var token = await GoogleAuth.GetAuthAccessToken(code, ClientID, ClientSecret, url);
            var userProfile = await GoogleAuth.GetProfileResponseAsync(token.AccessToken.ToString());
            var googleUser = JsonConvert.DeserializeObject<GoogleProfile>(userProfile);

            User user = new User();
            user.FullName = googleUser.Name;
            user.Email = googleUser.Email;
            user.Phone = "0359258471";
            user.Address = googleUser.Locate;
            user.UserName = googleUser.Email;
            user.Password = googleUser.Email;
            user.Img = "avt_1.jpg";
            user.Status = 1;
            user.CreatedAt = DateTime.Now;
            if (userDAO.Insert(user) == 1)
            {
                Session["UserCustomer"] = user.UserName;
                Session["UserId"] = user.id;
                Session["FullName"] = user.FullName;
                Session["Img"] = user.Img;
                Session["Phone"] = user.Phone;
                Session["Address"] = user.Address;
                Session["Email"] = user.Email;
                return Redirect("~/");
            }
            return View();
        }

        public ActionResult FacebookRedirect(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Get("/oauth/access_token", new
            {
                client_id = "1246301172713746",
                client_secret = "0d3c35b770c86e68aaacc20df7816cf3",
                redirect_uri = "https://localhost:44346/Khachhang/FacebookRedirect",
                code = code
            });

            fb.AccessToken = result.access_token;
            dynamic me = fb.Get("/me?fields=name,email");
            string name = me.name;
            string email = me.email;
            string phone = "0359258471";
            int status = 1;
            
            using (var dbContext = new NoiThatDBContext())
            {
                var user = new User
                {
                    FullName = name,
                    Email = email,
                    Phone = phone,
                    UserName = email,
                    Password = email,
                    Status = status,
                    Img = "avt_1.jpg",
                    CreatedAt = DateTime.Now
                };
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
                Session["UserCustomer"] = user.UserName;
                Session["UserId"] = user.id;
                Session["FullName"] = user.FullName;
                Session["Img"] = user.Img;
                Session["Phone"] = user.Phone;
                Session["Address"] = user.Address;
                Session["Email"] = user.Email;
            }
            return Redirect("~/");

        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection filled)
        {
            String username = filled["username"];
            String password = filled["password"];
            User rowUser = userDAO.getRow(username);
            String strError = "";
            if (rowUser == null)
            {
                strError = "Tên đăng nhập không đúng";
            }
            else
            {
                if (password.Equals(rowUser.Password))
                {
                    Session["UserCustomer"] = username;
                    Session["UserId"] = rowUser.id;
                    Session["FullName"] = rowUser.FullName;
                    Session["Img"] = rowUser.Img;
                    Session["Phone"] = rowUser.Phone;
                    Session["Address"] = rowUser.Address;
                    Session["Email"] = rowUser.Email;
                    return Redirect("~/");
                }
                else
                {
                    strError = "Sai mật khẩu";
                }
            }
            ViewBag.Error = "<span class='text-danger>'" + strError + "</div>";
            return View("DangNhap");
        }

        public ActionResult DangKy()
        {
            ViewBag.Error = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(User user, FormCollection filled)
        {
            String strError = "";
            List<User> list = userDAO.getList();
            if (ModelState.IsValid)
            {
                foreach (var item in list)
                {
                    if (user.UserName.Equals(item.UserName))
                    {
                        strError = "Tên đăng nhập đã tồn tại";
                    }
                    else
                    {
                        if (user.Email.Equals(item.Email))
                        {
                            strError = "Email đã tồn tại";
                        }
                        else
                        {
                            if (!filled["RePassword"].Equals(user.Password))
                            {
                                strError = "Nhập lại mật khẩu không đúng";
                            }
                            else
                            {
                                if (!XString.chkMK(user.Password))
                                {
                                    strError = "Mật khẩu phải có ít nhất 6 ký tự, một số, một ký tự in hoa và một ký tự đặc biệt";
                                }
                                else
                                {
                                    //upload file
                                    user.Img = "abc";
                                    //end upload file
                                    user.Status = 1;
                                    user.CreatedAt = DateTime.Now;
                                    Session["UserCustomer"] = user.UserName;
                                    Session["UserId"] = user.id;
                                    Session["FullName"] = user.FullName;
                                    Session["Img"] = user.Img;
                                    Session["Phone"] = user.Phone;
                                    Session["Address"] = user.Address;
                                    Session["Email"] = user.Email;
                                    userDAO.Insert(user);
                                    return Redirect("~/");
                                }
                            }
                        }
                    }

                }
                
            }

            //String strError = "";
            //User user = new User();
            //
            //user.FullName = filled["FullName"];
            //user.UserName = filled["UserName"];
            //user.Password = filled["Password"];
            //user.Email = filled["Email"];
            //user.Phone = filled["Phone"];
            //user.Address = filled["Address"];
            //user.Status = 1;


            ViewBag.Error = "<p class=\"login-box-msg text-danger\">" + strError + "</strong></p>";
            return View(user);
        }

        public ActionResult DangXuat()
        {
            Session["UserCustomer"] = "";
            Session["UserId"] = "";
            Session["FullName"] = "";
            Session["Img"] = "";
            Session["Phone"] = "";
            Session["Address"] = "";
            Session["Email"] = "";
            return Redirect("~/dang-nhap");
        }

        public ActionResult DonHangCaNhan()
        {
            ViewBag.DonHangCaNhan = orderDAO.getListByUserId(Convert.ToInt32(Session["UserId"]));
            return View("DonHangCaNhan");
        }


        public ActionResult ThongTinCaNhan()
        {
            int id = int.Parse(Session["UserId"].ToString());
            User user = userDAO.getRow(id);
            return View(user);
        }

        public ActionResult ChinhSuaThongTin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = userDAO.getRow(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSuaThongTin(User user)
        {

            if (ModelState.IsValid)
            {
                user.UpdatedAt = DateTime.Now;
                userDAO.Update(user);
            }
            return RedirectToAction("ThongTinCaNhan");
        }
        public String ProductImg(int? productid)
        {
            Product product = productDAO.getRow(productid);
            if (product == null)
            {
                return "";
            }
            else
            {
                return product.Img;
            }
        }
        public String ProductName(int? productid)
        {
            Product product = productDAO.getRow(productid);
            if (product == null)
            {
                return "";
            }
            else
            {
                return product.Name;
            }
        }

        public ActionResult HuyDon(int? id)
        {
            String strThongbao = "";
            Order order = orderDAO.getRow(id);
            if (order == null)
            {
                strThongbao = "Mẫu tin không tồn tại";
            }
            if (order.Status == 1 || order.Status == 2)
            {
                order.Status = 0;
                order.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
                order.UpdatedAt = DateTime.Now;
                strThongbao = "Đã hủy thành công";
            }
            else
            {
                strThongbao = "Đơn hàng này không thể hủy";
            }
            order.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            order.UpdatedAt = DateTime.Now;
            orderDAO.Update(order);
            ViewBag.ThongBao = "<p class=\"login-box-msg text-danger\">" + strThongbao + "</strong></p>";

            return RedirectToAction("DonHangCaNhan", "Khachhang");
        }
    }

}
