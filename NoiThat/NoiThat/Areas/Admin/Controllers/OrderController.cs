using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NoiThat.DAO;
using NoiThat.Models;

namespace NoiThat.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        private NoiThatDBContext db = new NoiThatDBContext();
        OrderDAO orderDAO = new OrderDAO();
        OrderDetailDAO orderDetailDAO = new OrderDetailDAO();

        // GET: Admin/Order
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        // GET: Admin/Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListChiTiet = orderDetailDAO.getList(id);
            return View(order);
        }

        // GET: Admin/Order/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,UserId,Name,Phone,Email,Note,ParentId,Orders,CreatedAt,UpdatedBy,UpdatedAt,Status")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Admin/Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,UserId,Name,Phone,Email,Note,ParentId,Orders,CreatedAt,UpdatedBy,UpdatedAt,Status")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Admin/Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã sp không tồn tại");
                return RedirectToAction("Index", "Order");
            }
            Order order = orderDAO.getRow(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Order");
            }
            order.Status = (order.Status == 1) ? 2 : 1;
            order.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            order.UpdatedAt = DateTime.Now;
            orderDAO.Update(order);
            TempData["message"] = new XMessage("success", "Thay đổi trạng thái thành công");
            return RedirectToAction("Index", "Order");

        }
        public ActionResult Trash()
        {
            return View(orderDAO.getList("Trash"));

        }
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã sp không tồn tại");
                return RedirectToAction("Trash", "Order");
            }
            Order order = orderDAO.getRow(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Trash", "Order");
            }
            order.Status = 2;//Trạng thái Index =0
            order.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            order.UpdatedAt = DateTime.Now;
            orderDAO.Update(order);
            TempData["message"] = new XMessage("success", "Khôi phục thành công");
            return RedirectToAction("Index", "Order");

        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại sp không tồn tại");
                return RedirectToAction("Index", "Order");
            }
            Order order = orderDAO.getRow(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Order");
            }
            order.Status = 0;//Trạng thái rác =0
            order.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            order.UpdatedAt = DateTime.Now;
            orderDAO.Update(order);
            TempData["message"] = new XMessage("success", "Xóa vào thùng rác thành công");
            return RedirectToAction("Index", "Order");
        }

        //0: Hủy đơn
        //1: Đang chờ
        //2: Đã xác nhận
        //3: Đang giao
        //4: thành cônng

        //11: Đang chờ +VNPay
        //12: Đã xác nhận +VNPay
        //13: Đang giao+VNPay

        public ActionResult HuyDon(int? id)
        {
            Order order = orderDAO.getRow(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Order");
            }
            if (order.Status == 1 || order.Status==2)
            {
                order.Status = 0;
                order.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
                order.UpdatedAt = DateTime.Now;
            }
            else
            {
                TempData["message"] = new XMessage("danger", "Đơn hàng này không thể hủy");
                return RedirectToAction("Index", "Order");
            }
            order.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            order.UpdatedAt = DateTime.Now;
            orderDAO.Update(order);
            TempData["message"] = new XMessage("success", "Đã hủy thành công");
            return RedirectToAction("Index", "Order");

        }
        public ActionResult DaXacMinh(int? id)
        {
            Order order = orderDAO.getRow(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Order");
            }
            if (order.Status == 1 )
            {
                order.Status = 2;
                order.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
                order.UpdatedAt = DateTime.Now;
            }
            else if (order.Status == 11 )
            {
                order.Status = 12;
                order.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
                order.UpdatedAt = DateTime.Now;
            }


            else
            {
                TempData["message"] = new XMessage("danger", "Không thể xác nhận");
                return RedirectToAction("Index", "Order");
            }
            order.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            order.UpdatedAt = DateTime.Now;
            orderDAO.Update(order);
            TempData["message"] = new XMessage("success", "Đã xác nhận đơn hàng");
            return RedirectToAction("Index", "Order");

        }
        public ActionResult DangVanChuyen(int? id)
        {
            Order order = orderDAO.getRow(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Order");
            }
            if (order.Status == 2 )
            {
                order.Status = 3;
                order.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
                order.UpdatedAt = DateTime.Now;
            }
            else if (order.Status == 12 )
            {
                order.Status = 13;
                order.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
                order.UpdatedAt = DateTime.Now;
            }

            else
            {
                TempData["message"] = new XMessage("danger", "Thất bại");
                return RedirectToAction("Index", "Order");
            }
            order.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            order.UpdatedAt = DateTime.Now;
            orderDAO.Update(order);
            TempData["message"] = new XMessage("success", "Đang giao");
            return RedirectToAction("Index", "Order");
        }
        public ActionResult ThanhCong(int? id)
        {
            Order order = orderDAO.getRow(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Order");
            }
            if (order.Status == 3 || order.Status == 13)
            {
                order.Status = 4;
                order.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
                order.UpdatedAt = DateTime.Now;
            }
            else
            {
                TempData["message"] = new XMessage("danger", "Thất bại");
                return RedirectToAction("Index", "Order");
            }
            order.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            order.UpdatedAt = DateTime.Now;
            orderDAO.Update(order);
            TempData["message"] = new XMessage("success", "Giao hàng thành công");
            return RedirectToAction("Index", "Order");
        }
    }
}
