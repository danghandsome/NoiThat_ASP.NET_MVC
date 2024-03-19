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
    public class ProductController : BaseController
    {
        private ProductDAO productDAO = new ProductDAO();
        CategoryDAO categoryDAO = new CategoryDAO();
        SupplierDAO supplierDAO = new SupplierDAO();

        // GET: Admin/Product
        public ActionResult Index()
        {
            return View(productDAO.getList("Index"));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = productDAO.getRow(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.ListCat = new SelectList(categoryDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListSup = new SelectList(supplierDAO.getList("Index"), "Orders", "Name", 0);
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                //Xử lý thêm
                product.Slug = XString.StrSlug(product.Name);
                //upload file
                var imge = Request.Files["img"];
                if (imge.ContentLength > 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jepg", ".png", ".gif" };
                    //kiểm tra tập tin
                    if (FileExtentions.Contains(imge.FileName.Substring(imge.FileName.LastIndexOf("."))))
                    {
                        string imgName = product.Slug + imge.FileName.Substring(imge.FileName.LastIndexOf("."));
                        product.Img = imgName;
                        string PathDir = "~/Public/images/products/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        imge.SaveAs(PathFile);
                    }
                }
                //end upload file
                product.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                product.CreatedAt = DateTime.Now;
                productDAO.Insert(product);
                
                TempData["message"] = new XMessage("success", "Thêm thành công");

                return RedirectToAction("Index", "product");
            }
            ViewBag.ListCat = new SelectList(categoryDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListSup = new SelectList(supplierDAO.getList("Index"), "Orders", "Name", 0);

            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = productDAO.getRow(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListCat = new SelectList(categoryDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListSup = new SelectList(supplierDAO.getList("Index"), "Orders", "Name", 0);

            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                product.Slug = XString.StrSlug(product.Name);

                //upload file
                var imge = Request.Files["img"];
                if (imge.ContentLength > 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jepg", ".png", ".gif" };
                    //kiểm tra tập tin
                    if (FileExtentions.Contains(imge.FileName.Substring(imge.FileName.LastIndexOf("."))))
                    {
                        string imgName = product.Slug + imge.FileName.Substring(imge.FileName.LastIndexOf("."));
                        product.Img = imgName;
                        string PathDir = "~/Public/images/products/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);

                        //Xóa file
                        if (product.Img != null)
                        {
                            string DelPath = Path.Combine(Server.MapPath(PathDir), product.Img);
                            System.IO.File.Delete(DelPath);//Xoa hinh
                        }

                        imge.SaveAs(PathFile);
                    }
                }
                //end upload file

                product.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
                product.UpdatedAt = DateTime.Now;
                productDAO.Update(product);
                TempData["message"] = new XMessage("success", "Cập nhật thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(categoryDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListSup = new SelectList(supplierDAO.getList("Index"), "Orders", "Name", 0);
            return View(product);
        }

        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.ListCat = new SelectList(productDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListSup = new SelectList(supplierDAO.getList("Index"), "Orders", "Name", 0);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = productDAO.getRow(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = productDAO.getRow(id);

            //xoa hinh anh
            string PathDir = "~/Public/images/products/";
            if (product.Img != null)
            {
                string DelPath = Path.Combine(Server.MapPath(PathDir), product.Img);
                System.IO.File.Delete(DelPath);//Xoa hinh
            }

            productDAO.Delete(product);
            
            TempData["message"] = new XMessage("success", "Xóa thành công");
            return RedirectToAction("Trash", "product");
        }

        public ActionResult Trash()
        {
            return View(productDAO.getList("Trash"));

        }

        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã sp không tồn tại");
                return RedirectToAction("Index", "Product");
            }
            Product product = productDAO.getRow(id);
            if (product == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Product");
            }
            product.Status = (product.Status == 1) ? 2 : 1;
            product.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            product.UpdatedAt = DateTime.Now;
            productDAO.Update(product);
            TempData["message"] = new XMessage("success", "Thay đổi trạng thái thành công");
            return RedirectToAction("Index", "Product");

        }

        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã sp không tồn tại");
                return RedirectToAction("Trash", "Product");
            }
            Product product = productDAO.getRow(id);
            if (product == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Trash", "Product");
            }
            product.Status = 0;//Trạng thái Index =0
            product.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            product.UpdatedAt = DateTime.Now;
            productDAO.Update(product);
            TempData["message"] = new XMessage("success", "Khôi phục thành công");
            return RedirectToAction("Index", "Product");

        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại sp không tồn tại");
                return RedirectToAction("Index", "Product");
            }
            Product product = productDAO.getRow(id);
            if (product == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Product");
            }
            product.Status = 2;//Trạng thái rác =0
            product.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            product.UpdatedAt = DateTime.Now;
            productDAO.Update(product);
            TempData["message"] = new XMessage("success", "Xóa vào thùng rác thành công");
            return RedirectToAction("Index", "Product");

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
    }
}
