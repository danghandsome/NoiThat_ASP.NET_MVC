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
    public class SupplierController : BaseController
    {
        SupplierDAO supplierDAO = new SupplierDAO();
        LinkDAO linkDAO = new LinkDAO();
        public ActionResult Index()
        {
            return View(supplierDAO.getList("Index"));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = supplierDAO.getRow(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // GET: Admin/supplier/Create
        public ActionResult Create()
        {
            ViewBag.ListOrder = new SelectList(supplierDAO.getList("Index"), "Orders", "Name", 0);
            return View();
        }

        // POST: Admin/supplier/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                //Xử lý thêm
                supplier.Slug = XString.StrSlug(supplier.Name);
                
                if (supplier.Orders == null)
                {
                    supplier.Orders = 1;
                }
                else
                {
                    supplier.Orders+=1;
                }

                //upload file
                var imge = Request.Files["img"];
                if(imge.ContentLength > 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jepg", ".png", ".gif" };
                    //kiểm tra tập tin
                    if (FileExtentions.Contains(imge.FileName.Substring(imge.FileName.LastIndexOf("."))))
                    {
                        string imgName = supplier.Slug + imge.FileName.Substring(imge.FileName.LastIndexOf("."));
                        supplier.Img = imgName;
                        string PathDir = "~/Public/images/suppliers/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        imge.SaveAs(PathFile);
                    }
                }
                //end upload file

                supplier.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                supplier.CreatedAt = DateTime.Now;
                if (supplierDAO.Insert(supplier) == 1)
                {
                    Link link = new Link();
                    link.Slug = supplier.Slug;
                    link.TableId = supplier.id;
                    link.TypeLink = "supplier";
                    linkDAO.Insert(link);
                }
                TempData["message"] = new XMessage("success", "Thêm thành công");

                return RedirectToAction("Index","supplier");
            }
            ViewBag.ListOrder = new SelectList(supplierDAO.getList("Index"), "Orders", "Name", 0);

            return View(supplier);
        }

        // GET: Admin/supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = supplierDAO.getRow(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListOrder = new SelectList(supplierDAO.getList("Index"), "Orders", "Name", 0);

            return View(supplier);
        }

        // POST: Admin/supplier/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                supplier.Slug = XString.StrSlug(supplier.Name);
                
                if (supplier.Orders == null)
                {
                    supplier.Orders = 1;
                }
                else
                {
                    supplier.Orders++;
                }

                //upload file
                var imge = Request.Files["img"];
                if (imge.ContentLength > 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jepg", ".png", ".gif" };
                    //kiểm tra tập tin
                    if (FileExtentions.Contains(imge.FileName.Substring(imge.FileName.LastIndexOf("."))))
                    {
                        string imgName = supplier.Slug + imge.FileName.Substring(imge.FileName.LastIndexOf("."));
                        supplier.Img = imgName;
                        string PathDir = "~/Public/images/suppliers/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);

                        //Xóa file
                        if (supplier.Img != null)
                        {
                            string DelPath = Path.Combine(Server.MapPath(PathDir), supplier.Img);
                            System.IO.File.Delete(DelPath);//Xoa hinh
                        }
                        imge.SaveAs(PathFile);
                    }
                }
                //end upload file

                supplier.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
                supplier.UpdatedAt = DateTime.Now;
                if (supplierDAO.Update(supplier) == 1)
                {
                    Link link = linkDAO.getRow(supplier.id, "supplier");
                    link.Slug = supplier.Slug;
                    linkDAO.Update(link);
                }
                TempData["message"] = new XMessage("success", "Cập nhật thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListOrder = new SelectList(supplierDAO.getList("Index"), "Orders", "Name", 0);
            return View(supplier);
        }

        // GET: Admin/supplier/Delete/5
        public ActionResult Delete(int? id)
        {
            //ViewBag.ListCat = new SelectList(supplierDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(supplierDAO.getList("Index"), "Orders", "Name", 0);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = supplierDAO.getRow(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Admin/supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = supplierDAO.getRow(id);
            Link link = linkDAO.getRow(supplier.id, "supplier");

            //xoa hinh anh
            string PathDir = "~/Public/images/suppliers/";
            if (supplier.Img != null)
            {
                string DelPath = Path.Combine(Server.MapPath(PathDir), supplier.Img);
                System.IO.File.Delete(DelPath);//Xoa hinh
            }

            if (supplierDAO.Delete(supplier) == 1)
            {
                linkDAO.Delete(link);
            }

            TempData["message"] = new XMessage("success", "Xóa thành công");
            return RedirectToAction("Trash", "supplier");
        }

        public ActionResult Trash()
        {
            return View(supplierDAO.getList("Trash"));

        }

        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại sp không tồn tại");
                return RedirectToAction("Index", "supplier");
            }
            Supplier supplier = supplierDAO.getRow(id);
            if (supplier == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "supplier");
            }
            supplier.Status = (supplier.Status == 1) ? 2 : 1;
            supplier.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            supplier.UpdatedAt = DateTime.Now;
            supplierDAO.Update(supplier);
            TempData["message"] = new XMessage("success", "Thay đổi trạng thái thành công");
            return RedirectToAction("Index", "supplier");

        }

        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại sp không tồn tại");
                return RedirectToAction("Trash", "supplier");
            }
            Supplier supplier = supplierDAO.getRow(id);
            if (supplier == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Trash", "supplier");
            }
            supplier.Status = 0;//Trạng thái rác =0
            supplier.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            supplier.UpdatedAt = DateTime.Now;
            supplierDAO.Update(supplier);
            TempData["message"] = new XMessage("success", "Xóa vào thùng rác thành công");

            return RedirectToAction("Index", "supplier");

        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại sp không tồn tại");
                return RedirectToAction("Index", "supplier");
            }
            Supplier supplier = supplierDAO.getRow(id);
            if (supplier == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "supplier");
            }
            supplier.Status = 2;//Trạng thái index =2
            supplier.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            supplier.UpdatedAt = DateTime.Now;
            supplierDAO.Update(supplier);
            TempData["message"] = new XMessage("success", "Khôi phục thành công");
            return RedirectToAction("Index", "supplier");

        }

        public String NameSupplier(int productid)
        {
            Supplier supplier = supplierDAO.getRow(productid);
            if (supplier == null)
            {
                return "";
            }
            else
            {
                return supplier.Name;
            }
        }
    }
}
