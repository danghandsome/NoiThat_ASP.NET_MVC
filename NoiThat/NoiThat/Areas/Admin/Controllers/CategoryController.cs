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
    public class CategoryController : BaseController
    {
        CategoryDAO categoryDAO= new CategoryDAO();
        LinkDAO linkDAO= new LinkDAO();
        public ActionResult Index()
        {

            return View(categoryDAO.getList("Index"));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryDAO.getRow(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.ListCat = new SelectList(categoryDAO.getList("Index"), "Id", "Name",0);
            ViewBag.ListOrder = new SelectList(categoryDAO.getList("Index"), "Orders", "Name",0);
            return View();
        }

        // POST: Admin/Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                //Xử lý thêm
                category.Slug = XString.StrSlug(category.Name);
                if (category.ParentId == null)
                {
                    category.ParentId = 0;
                }
                if (category.Orders == null)
                {
                    category.Orders = 1;
                }
                else
                {
                    category.Orders++;
                }
                //upload file
                var imge = Request.Files["img"];
                if (imge.ContentLength > 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jepg", ".png", ".gif" };
                    //kiểm tra tập tin
                    if (FileExtentions.Contains(imge.FileName.Substring(imge.FileName.LastIndexOf("."))))
                    {
                        string imgName = category.Slug + imge.FileName.Substring(imge.FileName.LastIndexOf("."));
                        category.Img = imgName;
                        string PathDir = "~/Public/images/categorys/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        imge.SaveAs(PathFile);
                    }
                }
                //end upload file
                category.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                category.CreatedAt = DateTime.Now;
                if (categoryDAO.Insert(category) == 1) 
                {
                    Link link = new Link();
                    link.Slug = category.Slug;
                    link.TableId = category.id;
                    link.TypeLink = "category";
                    linkDAO.Insert(link);
                TempData["message"] = new XMessage("success", "Thêm thành công");
                }

                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(categoryDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(categoryDAO.getList("Index"), "Orders", "Name", 0);
            return View(category);
        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryDAO.getRow(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListCat = new SelectList(categoryDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(categoryDAO.getList("Index"), "Orders", "Name", 0);
            return View(category);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = XString.StrSlug(category.Name);
                if (category.ParentId == null)
                {
                    category.ParentId = 0;
                }
                if (category.Orders == null)
                {
                    category.Orders = 1;
                }
                else
                {
                    category.Orders++;
                }
                //upload file
                var imge = Request.Files["img"];
                if (imge.ContentLength > 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jepg", ".png", ".gif" };
                    //kiểm tra tập tin
                    if (FileExtentions.Contains(imge.FileName.Substring(imge.FileName.LastIndexOf("."))))
                    {
                        string imgName = category.Slug + imge.FileName.Substring(imge.FileName.LastIndexOf("."));
                        category.Img = imgName;
                        string PathDir = "~/Public/images/categorys/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);

                        //Xóa file
                        if (category.Img != null)
                        {
                            string DelPath = Path.Combine(Server.MapPath(PathDir), category.Img);
                            System.IO.File.Delete(DelPath);//Xoa hinh
                        }

                        imge.SaveAs(PathFile);
                    }
                }
                //end upload file

                category.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
                category.UpdatedAt = DateTime.Now;
                if (categoryDAO.Update(category)==1)
                {
                    Link link = linkDAO.getRow(category.id, "category");
                    link.Slug = category.Slug;
                    linkDAO.Update(link);
                }
                TempData["message"] = new XMessage("success", "Cập nhật thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(categoryDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(categoryDAO.getList("Index"), "Orders", "Name", 0);
            return View(category);
        }

        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.ListCat = new SelectList(categoryDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(categoryDAO.getList("Index"), "Orders", "Name", 0);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryDAO.getRow(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = categoryDAO.getRow(id);
            Link link = linkDAO.getRow(category.id, "category");
            if (categoryDAO.Delete(category) == 1)
            {
                linkDAO.Delete(link);
            }
            //xoa hinh anh
            string PathDir = "~/Public/images/categorys/";
            if (category.Img != null)
            {
                string DelPath = Path.Combine(Server.MapPath(PathDir), category.Img);
                System.IO.File.Delete(DelPath);//Xoa hinh
            }

            TempData["message"] = new XMessage("success", "Xóa thành công");
            return RedirectToAction("Trash", "Category");
        }

        public ActionResult Trash()
        {
            return View(categoryDAO.getList("Trash"));

        }

        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại sp không tồn tại");
                return RedirectToAction("Index","Category");
            }
            Category category = categoryDAO.getRow(id);
            if (category == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Category");
            }
            category.Status = (category.Status == 1) ? 2 : 1;
            category.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            category.UpdatedAt = DateTime.Now;
            categoryDAO.Update(category);
            TempData["message"] = new XMessage("success", "Thay đổi trạng thái thành công");
            return RedirectToAction("Index", "Category");

        }

        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại sp không tồn tại");
                return RedirectToAction("Trash", "Category");
            }
            Category category = categoryDAO.getRow(id);
            if (category == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Trash", "Category");
            }
            category.Status = 0;//Trạng thái rác =0
            category.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            category.UpdatedAt = DateTime.Now;
            categoryDAO.Update(category);
            TempData["message"] = new XMessage("success", "Xóa vào thùng rác thành công");

            return RedirectToAction("Index", "Category");

        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại sp không tồn tại");
                return RedirectToAction("Index", "Category");
            }
            Category category = categoryDAO.getRow(id);
            if (category == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Category");
            }
            category.Status = 2;//Trạng thái index =2
            category.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            category.UpdatedAt = DateTime.Now;
            categoryDAO.Update(category);
            TempData["message"] = new XMessage("success", "Khôi phục thành công");
            return RedirectToAction("Index", "Category");

        }

        
    }
}
