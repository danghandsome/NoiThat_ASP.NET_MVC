//using NoiThat.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace NoiThat.Controllers
//{
//    public class ContactsController : Controller
//    {
//        // GET: Contacts
//        [HttpGet]
//        public ActionResult Index()
//        {
//            return View();
//        }
//        [HttpPost]
//        public ActionResult Index(Contacts contact)
//        {
//            if (!ModelState.IsValid)
//            {
//                // Nếu dữ liệu không hợp lệ, trả về view form liên hệ để hiển thị thông báo lỗi
//                return View(contact);
//            }

//            // Lưu thông tin liên hệ vào cơ sở dữ liệu
//            // Việc lưu vào cơ sở dữ liệu ở đây chỉ là minh họa, bạn cần sử dụng cơ sở dữ liệu thực sự của bạn để lưu thông tin này
//            // Ví dụ: 
//            using (var context = new NoiThatDBContext())
//            {
//                context.Contactss.Add(contact);
//                context.SaveChanges();
//            }

//            // Sau khi lưu thông tin, chuyển hướng sang trang cảm ơn
//            return RedirectToAction("ContactConfirmation");
//        }

//        // Action để hiển thị trang cảm ơn sau khi người dùng đã gửi thông tin liên hệ thành công
//        public ActionResult ContactConfirmation()
//        {
//            return View();
//        }
//    }
//}