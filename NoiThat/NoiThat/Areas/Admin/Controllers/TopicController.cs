using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NoiThat.DAO;
using NoiThat.Library;
using NoiThat.Models;

namespace NoiThat.Areas.Admin.Controllers
{
    public class TopicController : BaseController
    {
        TopicDAO topicDAO = new TopicDAO();
        LinkDAO linkDAO = new LinkDAO();
        // GET: Admin/Topic
        public ActionResult Index()
        {
            return View(topicDAO.getList("Index"));
        }

        // GET: Admin/Topic/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = topicDAO.getRow(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // GET: Admin/Topic/Create
        public ActionResult Create()
        {
            ViewBag.ListTopic = new SelectList(topicDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(topicDAO.getList("Index"), "Orders", "Name", 0);
            return View();
        }

        // POST: Admin/Topic/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Topic topic)
        {
            if (ModelState.IsValid)
            {
                //Xử lý thêm
                topic.Slug = XString.StrSlug(topic.Name);
                if (topic.ParentId == null)
                {
                    topic.ParentId = 0;
                }
                if (topic.Orders == null)
                {
                    topic.Orders = 1;
                }
                else
                {
                    topic.Orders++;
                }
                topic.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                topic.CreatedAt = DateTime.Now;
                if (topicDAO.Insert(topic) == 1)
                {
                    Link link = new Link();
                    link.Slug = topic.Slug;
                    link.TableId = topic.id;
                    link.TypeLink = "topic";
                    linkDAO.Insert(link);
                }
                TempData["message"] = new XMessage("success", "Thêm thành công");

                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(topicDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(topicDAO.getList("Index"), "Orders", "Name", 0);
            return View(topic);
        }

        // GET: Admin/Topic/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = topicDAO.getRow(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListCat = new SelectList(topicDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(topicDAO.getList("Index"), "Orders", "Name", 0);

            return View(topic);
        }

        // POST: Admin/Topic/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Topic topic)
        {
            if (ModelState.IsValid)
            {
                topic.Slug = XString.StrSlug(topic.Name);
                if (topic.ParentId == null)
                {
                    topic.ParentId = 0;
                }
                if (topic.Orders == null)
                {
                    topic.Orders = 1;
                }
                else
                {
                    topic.Orders++;
                }
                topic.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
                topic.UpdatedAt = DateTime.Now;
                if (topicDAO.Update(topic) == 1)
                {
                    Link link = linkDAO.getRow(topic.id, "topic");
                    link.Slug = topic.Slug;
                    linkDAO.Update(link);
                }
                TempData["message"] = new XMessage("success", "Cập nhật thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(topicDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(topicDAO.getList("Index"), "Orders", "Name", 0);

            return View(topic);
        }

        // GET: Admin/Topic/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.ListCat = new SelectList(topicDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(topicDAO.getList("Index"), "Orders", "Name", 0);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = topicDAO.getRow(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Admin/Topic/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Topic topic = topicDAO.getRow(id);
            Link link = linkDAO.getRow(topic.id, "topic");
            if (topicDAO.Delete(topic) == 1)
            {
                linkDAO.Delete(link);
            }

            TempData["message"] = new XMessage("success", "Xóa thành công");
            return RedirectToAction("Trash", "Topic");
        }

        public ActionResult Trash()
        {
            return View(topicDAO.getList("Trash"));

        }

        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại sp không tồn tại");
                return RedirectToAction("Index", "Topic");
            }
            Topic topic = topicDAO.getRow(id);
            if (topic == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Topic");
            }
            topic.Status = (topic.Status == 1) ? 2 : 1;
            topic.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            topic.UpdatedAt = DateTime.Now;
            topicDAO.Update(topic);
            TempData["message"] = new XMessage("success", "Thay đổi trạng thái thành công");
            return RedirectToAction("Index", "Topic");

        }

        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại sp không tồn tại");
                return RedirectToAction("Trash", "Topic");
            }
            Topic topic = topicDAO.getRow(id);
            if (topic == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Trash", "Topic");
            }
            topic.Status = 2;//Trạng thái Index =0
            topic.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            topic.UpdatedAt = DateTime.Now;
            topicDAO.Update(topic);
            TempData["message"] = new XMessage("success", "Khôi phục thành công");
            return RedirectToAction("Index", "Topic");

        }
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại sp không tồn tại");
                return RedirectToAction("Index", "Topic");
            }
            Topic topic = topicDAO.getRow(id);
            if (topic == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Topic");
            }
            topic.Status = 0;//Trạng thái rác =0
            topic.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            topic.UpdatedAt = DateTime.Now;
            topicDAO.Update(topic);
            TempData["message"] = new XMessage("success", "Xóa vào thùng rác thành công");
            return RedirectToAction("Index", "Topic");

        }

    }
    }
