using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NoiThat.DAO;
using NoiThat.Library;
using NoiThat.Models;

namespace NoiThat.Areas.Admin.Controllers
{
    public class PostController : BaseController
    {
        private PostDAO postDAO = new PostDAO();
        private TopicDAO topicDAO = new TopicDAO();

        // GET: Admin/Post
        public ActionResult Index()
        {
            return View(postDAO.getList("Index"));
        }

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
            ViewBag.ListTopic = new SelectList(topicDAO.getList("Index"), "Id", "Name", 0);
            return View();
        }

        // POST: Admin/Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {

                //upload file
                var imge = Request.Files["img"];
                if (imge.ContentLength > 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jepg", ".png", ".gif" };
                    //kiểm tra tập tin
                    if (FileExtentions.Contains(imge.FileName.Substring(imge.FileName.LastIndexOf("."))))
                    {
                        string imgName = post.Slug + imge.FileName.Substring(imge.FileName.LastIndexOf("."));
                        post.Img = imgName;
                        string PathDir = "~/Public/images/posts/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        imge.SaveAs(PathFile);
                    }
                }
                //end upload file
                post.Slug = XString.StrSlug(post.Title);
                post.PostType = "Post";
                post.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                post.CreatedAt = DateTime.Now;
                postDAO.Insert(post);
                TempData["message"] = new XMessage("success", "Thêm thành công");

                return RedirectToAction("Index", "Post");
            }
            ViewBag.ListTopic = new SelectList(topicDAO.getList("Index"), "Id", "Name", 0);


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
            ViewBag.ListTopic = new SelectList(topicDAO.getList("Index"), "Id", "Name", 0);


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
                post.Slug = XString.StrSlug(post.Title);

                //upload file
                var imge = Request.Files["img"];
                if (imge.ContentLength > 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jepg", ".png", ".gif" };
                    //kiểm tra tập tin
                    if (FileExtentions.Contains(imge.FileName.Substring(imge.FileName.LastIndexOf("."))))
                    {
                        string imgName = post.Slug + imge.FileName.Substring(imge.FileName.LastIndexOf("."));
                        post.Img = imgName;
                        string PathDir = "~/Public/images/posts/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);

                        //Xóa file
                        if (post.Img != null)
                        {
                            string DelPath = Path.Combine(Server.MapPath(PathDir), post.Img);
                            System.IO.File.Delete(DelPath);//Xoa hinh
                        }

                        imge.SaveAs(PathFile);
                    }
                }
                //end upload file

                post.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
                post.UpdatedAt = DateTime.Now;
                postDAO.Update(post);
                TempData["message"] = new XMessage("success", "Cập nhật thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListTopic = new SelectList(topicDAO.getList("Index"), "Id", "Name", 0);

            return View(post);
        }

        // GET: Admin/Post/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.ListTopic = new SelectList(postDAO.getList("Index"), "Id", "Name", 0);

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

        // POST: Admin/post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = postDAO.getRow(id);

            //xoa hinh anh
            string PathDir = "~/Public/images/posts/";
            if (post.Img != null)
            {
                string DelPath = Path.Combine(Server.MapPath(PathDir), post.Img);
                System.IO.File.Delete(DelPath);//Xoa hinh
            }

            postDAO.Delete(post);

            TempData["message"] = new XMessage("success", "Xóa thành công");
            return RedirectToAction("Trash", "post");
        }

        public ActionResult Trash()
        {
            return View(postDAO.getList("Trash"));

        }

        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã sp không tồn tại");
                return RedirectToAction("Index", "Post");
            }
            Post post = postDAO.getRow(id);
            if (post == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Post");
            }
            post.Status = (post.Status == 1) ? 2 : 1;
            post.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            post.UpdatedAt = DateTime.Now;
            postDAO.Update(post);
            TempData["message"] = new XMessage("success", "Thay đổi trạng thái thành công");
            return RedirectToAction("Index", "Post");

        }

        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã sp không tồn tại");
                return RedirectToAction("Trash", "Post");
            }
            Post post = postDAO.getRow(id);
            if (post == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Trash", "Post");
            }
            post.Status = 2;//Trạng thái Index =0
            post.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            post.UpdatedAt = DateTime.Now;
            postDAO.Update(post);
            TempData["message"] = new XMessage("success", "Khôi phục thành công");
            return RedirectToAction("Index", "Post");

        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại sp không tồn tại");
                return RedirectToAction("Index", "Post");
            }
            Post post = postDAO.getRow(id);
            if (post == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Post");
            }
            post.Status = 0;//Trạng thái rác =0
            post.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            post.UpdatedAt = DateTime.Now;
            postDAO.Update(post);
            TempData["message"] = new XMessage("success", "Xóa vào thùng rác thành công");
            return RedirectToAction("Index", "Post");

        }

        public String PostImg(int? postid)
        {
            Post post = postDAO.getRow(postid);
            if (post == null)
            {
                return "";
            }
            else
            {
                return post.Img;
            }
        }
        public String PostName(int? postid)
        {
            Post post = postDAO.getRow(postid);
            if (post == null)
            {
                return "";
            }
            else
            {
                return post.Title;
            }
        }
    }
}
