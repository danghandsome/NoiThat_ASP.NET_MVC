using NoiThat.DAO;
using NoiThat.Library;
using NoiThat.Models;
using NoiThat.Models.Payments;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoiThat.Controllers
{
    public class GiohangController : Controller
    {
        ProductDAO productDAO = new ProductDAO();
        UserDAO userDAO = new UserDAO();
        OrderDAO orderDAO = new OrderDAO();
        OrderDetailDAO orderdetailDAO = new OrderDetailDAO();

        XCart xcart = new XCart();
        // GET: Cart
        public ActionResult Index()
        {
            List<CartItem> listcart = xcart.getCart();

            return View("Index", listcart);
        }

        public ActionResult CartAdd(int productid)
        {
            Product product = productDAO.getRow(productid);
            CartItem cartitem = new CartItem(product.id, product.Name, product.Img, product.Price, 1);
            List<CartItem> listcart = xcart.AddCart(cartitem, productid);
            return RedirectToAction("Index", "Giohang");
        }

        public ActionResult CartDel(int productid)
        {
            xcart.DelCart(productid);
            return RedirectToAction("Index", "Giohang");
        }

        public ActionResult CartPlus(int productid)
        {
            xcart.CartPlus(productid);
            return RedirectToAction("Index", "Giohang");
        }
        public ActionResult CartMinus(int productid)
        {
            xcart.CartMinus(productid);
            return RedirectToAction("Index", "Giohang");
        }

        //Thanh toan
        public ActionResult ThanhToan()
        {
            List<CartItem> listcart = xcart.getCart();

            //Kiem tra dang nhap trang nguoi dung
            if (Session["UserCustomer"].Equals(""))
            {
                return Redirect("~/dang-nhap");
            }
            int userid = int.Parse(Session["UserId"].ToString());
            User user = userDAO.getRow(userid);
            ViewBag.user = user;  //CÁI KHÚC NÀY VÔ TRANG THANH TOÁN THÔI

            return View("ThanhToan", listcart);
        }

        #region Thanh toán vnpay
        public string UrlPayment(int TypePaymentVN, string orderCode)
        {
            NoiThatDBContext db = new NoiThatDBContext();
            var urlPayment = "";
            var order = db.Orders.FirstOrDefault(x => x.id.ToString() == orderCode);
            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            var Price = (long)order.Amount * 100;
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", Price.ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (TypePaymentVN == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (TypePaymentVN == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (TypePaymentVN == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }
            vnpay.AddRequestData("vnp_CreateDate", order.CreatedAt.ToString("yyyyMMddHHmmss"));//sai đinh dang
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng :" + order.id.ToString());
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.id.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            return urlPayment;
        }
        #endregion

        public ActionResult DatMua(FormCollection field)
        {
            int typePayment = int.Parse(field["TypePayment"].ToString());
            //int typePayment = Convert.ToInt32(field["typepayment"]); // lấy giá trị của input có tên là "typepayment"
            // xử lí các thao tác tiếp theo

            //var order = db.DatHang.FirstOrDefault(x => x.ID.ToString() == orderCode);

            var code = new { Success = false, Code = 0, UrlVNPay = "", UrlMomo = "", MaDonHang = "", SoTien = "" };
            int userid = int.Parse(Session["UserId"].ToString());
            User user = userDAO.getRow(userid);
            String note = field["Note"];
            Order order = new Order();
            order.ReceiverName = (string)Session["FullName"];
            order.ReceiverPhone = (string)Session["Phone"];
            order.ReceiverAddress = (string)Session["Address"];
            order.UserId = userid;
            order.Note = note;
            if (field["diemthuong"] != "")
            {
                order.Discount = int.Parse(field["diemthuong"].ToString());
                order.Amount -= order.Discount;
                user.Score -= order.Discount;
            }
            else
            {
                order.Discount = 0;
            }
            order.CreatedAt = DateTime.Now;
            order.Status = 1;
            if (orderDAO.Insert(order) == 1)
            {
                List<CartItem> listcart = xcart.getCart();
                foreach (CartItem cartitem in listcart)
                {
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.OrderId = order.id;
                    orderDetail.ProductId = cartitem.ProductId;
                    orderDetail.Price = cartitem.Price;
                    orderDetail.Quantity = cartitem.Qty;
                    orderDetail.Amount = cartitem.Amount;
                    order.Amount += (int)cartitem.Amount;
                    orderdetailDAO.Insert(orderDetail);
                }
                orderDAO.Update(order);
                user.Score += int.Parse(((int)order.Amount / 1000).ToString()); 
                
                userDAO.Update(user);
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var UrlVNPay = UrlPayment(2, order.id.ToString());
                code = new { Success = true, Code = typePayment, UrlVNPay = UrlVNPay, UrlMomo = "", MaDonHang = order.id.ToString(), SoTien = order.Amount.ToString() };
                var list_cart1 = LibraryCart.GetCart();
                if (typePayment == 1)
                {
                    
                    Session["MyCart"] = "";
                    var strSanPham = "";
                    var thanhtien = "";
                    foreach (var sp in orderdetailDAO.getList(order.id))
                    {
                        string scc = productDAO.getRow(sp.ProductId).Name.ToLower() + "<span style='color:white'>";
                        for (int i = productDAO.getRow(sp.ProductId).Name.Length; i < 55; i++)
                        {
                            scc += "_";
                        }
                        scc = scc + "</span>" + sp.Quantity;
                        strSanPham += "<tr>";
                        strSanPham += "<td>" + scc + "</td>";
                        strSanPham += "<td>" + sp.Amount + "</td>";
                        strSanPham += "</tr>";
                        thanhtien += sp.Price * sp.Quantity;
                    }
                    user.Score += int.Parse(((int)order.Amount / 10000).ToString());
                    userDAO.Update(user);
                    Session["MyCart"] = "";
                    string contentCustumer = System.IO.File.ReadAllText(Server.MapPath("~/assets/template/sendmail.html"));
                    contentCustumer = contentCustumer.Replace("{{MaDon}}", order.id.ToString());
                    contentCustumer = contentCustumer.Replace("{{SanPham}}", strSanPham);
                    contentCustumer = contentCustumer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentCustumer = contentCustumer.Replace("{{TenKhachHang}}", order.ReceiverName);
                    //contentCustumer = contentCustumer.Replace("{{SoLuong}}", );
                    contentCustumer = contentCustumer.Replace("{{Phone}}", order.ReceiverPhone);
                    contentCustumer = contentCustumer.Replace("{{Email}}", (string)Session["Email"]);
                    contentCustumer = contentCustumer.Replace("{{DiaChiNhanHang}}", order.ReceiverAddress);
                    contentCustumer = contentCustumer.Replace("{{ThanhTien}}", order.Amount.ToString());
                    contentCustumer = contentCustumer.Replace("{{TongTien}}", order.Amount.ToString());


                    new MailHelper().SendMail((string)Session["Email"], "Đơn hàng mới từ OnlineShop", contentCustumer);
                    //MAIL
                    return DatHangThanhCong(order.Amount, user.Score);

                }
                if (typePayment == 2)
                {
                    
                    Session["MyCart"] = "";
                    var strSanPham = "";
                    var thanhtien = "";
                    foreach (var sp in orderdetailDAO.getList(order.id))
                    {
                        string scc = productDAO.getRow(sp.ProductId).Name.ToLower() + "<span style='color:white'>";
                        for (int i = productDAO.getRow(sp.ProductId).Name.Length; i < 55; i++)  
                        {
                            scc += "_";
                        }
                        scc = scc + "</span>" + sp.Quantity;
                        strSanPham += "<tr>";
                        strSanPham += "<td>" + scc + "</td>";
                        strSanPham += "<td>" + sp.Amount + "</td>";
                        strSanPham += "</tr>";
                        thanhtien += sp.Price * sp.Quantity;
                    }
                    user.Score += int.Parse(((int)order.Amount / 10000).ToString());
                    userDAO.Update(user);
                    Session["MyCart"] = "";
                    string contentCustumer = System.IO.File.ReadAllText(Server.MapPath("~/assets/template/sendmail.html"));
                    contentCustumer = contentCustumer.Replace("{{MaDon}}", order.id.ToString());
                    contentCustumer = contentCustumer.Replace("{{SanPham}}", strSanPham);
                    contentCustumer = contentCustumer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentCustumer = contentCustumer.Replace("{{TenKhachHang}}", order.ReceiverName);
                    //contentCustumer = contentCustumer.Replace("{{SoLuong}}", );
                    contentCustumer = contentCustumer.Replace("{{Phone}}", order.ReceiverPhone);
                    contentCustumer = contentCustumer.Replace("{{Email}}", (string)Session["Email"]);
                    contentCustumer = contentCustumer.Replace("{{DiaChiNhanHang}}", order.ReceiverAddress);
                    contentCustumer = contentCustumer.Replace("{{ThanhTien}}", order.Amount.ToString());
                    contentCustumer = contentCustumer.Replace("{{TongTien}}", order.Amount.ToString());


                    new MailHelper().SendMail((string)Session["Email"], "Đơn hàng mới từ OnlineShop", contentCustumer);
                    return Redirect(UrlVNPay);
                }
            }
            Session["MyCart"] = "";
            return Redirect("~/thanh-cong");
        }

        public ActionResult DatHangThanhCong(int tongtien, int diem)
        {
            ViewBag.ThanhToanThanhCong = "Số tiền thanh toán (VND):" + tongtien;
            ViewBag.TichDiem=diem;
            return View("DatHangThanhCong");
        }

    }
}