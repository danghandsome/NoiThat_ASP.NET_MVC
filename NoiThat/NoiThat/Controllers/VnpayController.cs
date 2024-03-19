using NoiThat.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoiThat.Models.Payments;
using NoiThat.DAO;

namespace NoiThat.Controllers
{
    public class VnpayController : Controller
    {
        private NoiThatDBContext data = new NoiThatDBContext();

        // GET: Vnpay
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult VnpayReturn()
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount"));
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        var itemOrder = data.Orders.FirstOrDefault(x => x.id.ToString() == orderCode);
                        if (itemOrder != null)
                        {
                            itemOrder.Status = 1;//đã thanh toán
                            //UpdateModel(itemOrder);
                            //data.tb_orders.Attach(itemOrder);
                            //data.Entry(itemOrder).State = System.Data.Entity.EntityState.Modified;
                            data.SaveChanges();
                        }
                        //Thanh toan thanh cong
                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                    //displayTmnCode.InnerText = "Mã Website (Terminal ID):" + TerminalID;
                    //displayTxnRef.InnerText = "Mã giao dịch thanh toán:" + orderId.ToString();
                    //displayVnpayTranNo.InnerText = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                    UserDAO userDAO = new UserDAO();
                    

                    ViewBag.ThanhToanThanhCong = vnp_Amount;
                    ViewBag.TichDiem = userDAO.getScore(int.Parse(Session["UserId"].ToString())) ;
                    //displayBankCode.InnerText = "Ngân hàng thanh toán:" + bankCode;

                    
                }
            }
            //var a = UrlPayment(0, "DH3574");
            return View();
        }
    }
}