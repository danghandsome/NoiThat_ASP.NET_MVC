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
    public class UserController : BaseController
    {
        private NoiThatDBContext db = new NoiThatDBContext();
        UserDAO userDAO = new UserDAO();

        // GET: Admin/User
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Admin/User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Admin/User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Admin/User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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

        public ActionResult Trash()
        {
            return View(userDAO.getList("Trash"));

        }

        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã sp không tồn tại");
                return RedirectToAction("Index", "User");
            }
            User user = userDAO.getRow(id);
            if (user == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "User");
            }
            user.Status = (user.Status == 1) ? 2 : 1;
            user.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            user.UpdatedAt = DateTime.Now;
            userDAO.Update(user);
            TempData["message"] = new XMessage("success", "Thay đổi trạng thái thành công");
            return RedirectToAction("Index", "User");

        }

        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã sp không tồn tại");
                return RedirectToAction("Trash", "User");
            }
            User user = userDAO.getRow(id);
            if (user == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Trash", "User");
            }
            user.Status = 2;//Trạng thái Index =0
            user.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            user.UpdatedAt = DateTime.Now;
            userDAO.Update(user);
            TempData["message"] = new XMessage("success", "Khôi phục thành công");
            return RedirectToAction("Index", "User");

        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại sp không tồn tại");
                return RedirectToAction("Index", "User");
            }
            User user = userDAO.getRow(id);
            if (user == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "User");
            }
            user.Status = 0;//Trạng thái rác =0
            user.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            user.UpdatedAt = DateTime.Now;
            userDAO.Update(user);
            TempData["message"] = new XMessage("success", "Xóa vào thùng rác thành công");
            return RedirectToAction("Index", "User");

        }

        public String NameCustomer(int userid)
        {
            User user = userDAO.getRow(userid);
            if (user == null)
            {
                return "";
            }
            else
            {
                return user.FullName;
            }
        }

    }
}
