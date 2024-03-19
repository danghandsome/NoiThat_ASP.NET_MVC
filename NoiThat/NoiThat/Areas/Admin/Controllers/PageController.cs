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
    public class PageController : BaseController
    {
        private PostDAO postDAO = new PostDAO();
        private LinkDAO linkDAO = new LinkDAO();
        // GET: Admin/Page
        public ActionResult Index()
        {
            return View(postDAO.getList("Index", "Page"));
        }

        // GET: Admin/Post/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = postDAO.getRow(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Admin/Post/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,TopicId,Title,Slug,Detail,MetaKey,MetaDes,Img,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Status")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.PostType = "Page";
                post.Slug = XString.StrSlug(post.Title);
                post.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                post.CreatedAt = DateTime.Now;
                if (postDAO.Insert(post)==1)
                {
                    Link link = new Link();
                    link.Slug = post.Slug;
                    link.TableId = post.id;
                    link.TypeLink = "page";
                    linkDAO.Insert(link);
                    TempData["message"] = new XMessage("success", "Thêm thành công");
                }

                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Admin/Post/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = postDAO.getRow(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Admin/Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                post.PostType = "Page";
                post.Slug = XString.StrSlug(post.Title);
                post.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
                post.UpdatedAt = DateTime.Now;
                postDAO.Update(post);

                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Admin/Post/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = postDAO.getRow(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Admin/Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = postDAO.getRow(id);
            postDAO.Delete(post);
            return RedirectToAction("Index");
        }


    }
}
