using NoiThat.DAO;
using NoiThat.Library;
using NoiThat.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoiThat.Areas.Admin.Controllers
{
    public class MenuController : BaseController
    {
        CategoryDAO categoryDAO = new CategoryDAO();
        TopicDAO topicDAO = new TopicDAO();
        PostDAO postDAO = new PostDAO();
        MenuDAO menuDAO = new MenuDAO();
        SupplierDAO supplierDAO = new SupplierDAO();

        // GET: Admin/Menu
        public ActionResult Index()
        {
            ViewBag.ListCategory = categoryDAO.getList("Index");
            ViewBag.ListTopic = topicDAO.getList("Index");
            ViewBag.ListPage = postDAO.getList("Index", "Page");
            List<Menu> menus = menuDAO.getList("Index");
            return View("Index", menus);
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {

            //Category
            if (!string.IsNullOrEmpty(form["ThemCategory"]))
            {
                if (!string.IsNullOrEmpty(form["itemcat"]))
                {
                    var listItem = form["itemcat"];
                    var listArr = listItem.Split(',');
                    foreach (var row in listArr)
                    {
                        int id = int.Parse(row);
                        Category category = categoryDAO.getRow(id);
                        Menu menu = new Menu();
                        menu.Name = category.Name;
                        menu.Link = category.Slug;
                        menu.TableId = category.id;
                        menu.TypeMenu = "category";
                        menu.Position = form["Position"];
                        menu.ParentId = 0;
                        menu.Orders = 0;
                        menu.Status = 2;
                        menuDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm menu thành công");

                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn danh mục sản phẩm");
                }
            }

            //Topic
            if (!string.IsNullOrEmpty(form["ThemTopic"]))
            {
                if (!string.IsNullOrEmpty(form["itemTopic"]))
                {
                    var listItem = form["itemTopic"];
                    var listArr = listItem.Split(',');
                    foreach (var row in listArr)
                    {
                        int id = int.Parse(row);
                        Topic topic = topicDAO.getRow(id);
                        Menu menu = new Menu();
                        menu.Name = topic.Name;
                        menu.Link = topic.Slug;
                        menu.TableId = topic.id;
                        menu.TypeMenu = "topic";
                        menu.Position = form["Position"];
                        menu.ParentId = 0;
                        menu.Orders = 0;
                        menu.Status = 2;
                        menuDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm menu thành công");

                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn danh mục sản phẩm");
                }
            }

            //Page
            if (!string.IsNullOrEmpty(form["ThemPage"]))
            {
                if (!string.IsNullOrEmpty(form["itemPage"]))
                {
                    var listItem = form["itemPage"];
                    var listArr = listItem.Split(',');
                    foreach (var row in listArr)
                    {
                        int id = int.Parse(row);
                        Post post = postDAO.getRow(id);
                        Menu menu = new Menu();
                        menu.Name = post.Title;
                        menu.Link = post.Slug;
                        menu.TableId = post.id;
                        menu.TypeMenu = "page";
                        menu.Position = form["Position"];
                        menu.ParentId = 0;
                        menu.Orders = 0;
                        menu.Status = 2;
                        menuDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm menu thành công");

                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn danh mục sản phẩm");
                }
            }

            //ThemCustom
            if (!string.IsNullOrEmpty(form["ThemCustom"]))
            {
                if (!string.IsNullOrEmpty(form["name"]) && !string.IsNullOrEmpty(form["link"]))
                {
                    Menu menu = new Menu();
                    menu.Name = form["name"];
                    menu.Link = form["link"];
                    menu.TypeMenu = "custom";
                    menu.Position = form["Position"];
                    menu.ParentId = 0;
                    menu.Orders = 0;
                    menu.Status = 2;
                    menuDAO.Insert(menu);
                    TempData["message"] = new XMessage("success", "Thêm menu thành công");
                }

                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa nhập đủ thông tin");
                }
            }
            return RedirectToAction("Index", "Menu");
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.ListMenu = new SelectList(menuDAO.getList("Index"), "Id", "Name");
            ViewBag.ListOrder = new SelectList(menuDAO.getList("Index"), "Orders", "Name");
            Menu menu = menuDAO.getRow(id);
            return View("Edit", menu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Menu menu)
        {
            if (ModelState.IsValid)
            {
                if (menu.ParentId == null)
                {
                    menu.ParentId = 0;
                }
                if (menu.Orders == null)
                {
                    menu.Orders = 1;
                }
                else
                {
                    menu.Orders++;
                }

                menuDAO.Update(menu);
                
                TempData["message"] = new XMessage("success", "Cập nhật thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListMenu = new SelectList(menuDAO.getList("Index"), "Id", "Name");
            ViewBag.ListOrder = new SelectList(menuDAO.getList("Index"), "Orders", "Name");

            return View(menu);
        }

        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger","Mẫu tin không tồn tại");
                return RedirectToAction("Index");
            }

            Menu menu = menuDAO.getRow(id);
            if(menu == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index");
            }

            menu.Status = (menu.Status == 1) ? 2 : 1;

            menuDAO.Update(menu);
            TempData["message"] = new XMessage("success", "Cập nhật thành công");
            return RedirectToAction("Index");
        }
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger","Mẫu tin không tồn tại");
                return RedirectToAction("Index");
            }

            Menu menu = menuDAO.getRow(id);
            if(menu == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index");
            }

            menu.Status = 0;

            menuDAO.Update(menu);
            TempData["message"] = new XMessage("success", "Thêm vào thùng rác thành công");
            return RedirectToAction("Index");
        }
        public ActionResult ReTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger","Mẫu tin không tồn tại");
                return RedirectToAction("Index");
            }

            Menu menu = menuDAO.getRow(id);
            if(menu == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index");
            }

            menu.Status = 2;

            menuDAO.Update(menu);
            TempData["message"] = new XMessage("success", "Khôi phục thành công");
            return RedirectToAction("Trash");
        }

        public ActionResult Trash()
        {
            List<Menu> menu = menuDAO.getList("Trash");
            return View("Trash",menu);
        }


    }
}