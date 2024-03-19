using NoiThat.DAO;
using NoiThat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoiThat.Controllers
{
    public class ModuleController : Controller
    {
        private MenuDAO menuDAO = new MenuDAO();
        private SliderDAO sliderDAO = new SliderDAO();
        private CategoryDAO categoryDAO = new CategoryDAO();
        private SupplierDAO supplierDAO = new SupplierDAO();
        // GET: Module
        public ActionResult MainMenu()
        {
            List<Menu> list = menuDAO.getListByParentId("mainmenu", 0);
            return View("MainMenu", list);
        }
        public ActionResult MainMenuSub(int id)
        {
            Menu menu = menuDAO.getRow(id);
            List<Menu> list = menuDAO.getListByParentId("mainmenu", id);
            if (list.Count == 0)
            {


                return View("MainMenuSub1", menu);//ko co con
            }
            else
            {
                ViewBag.Menu = menu;
                return View("MainMenuSub2", list);//co con
            }
        }

        //SlideShow
        public ActionResult Slideshow()
        {
            List<Slider> list = sliderDAO.getListByPosition("Slideshow");
            return View("Slideshow",list);
        }

        public ActionResult ListCategory()
        {
            List<Category> list = categoryDAO.getListByParentId(0);
            return View("ListCategory", list);
        }
        public ActionResult ListSupplier()
        {
            List<Supplier> list = supplierDAO.getList("Index");
            return View("ListSupplier", list);
        }
        public ActionResult ListTopic()
        {
            List<Category> list = categoryDAO.getListByParentId(0);
            return View("ListTopic", list);
        }

        public ActionResult MenuFooter()
        {
            List<Menu> list = menuDAO.getListByParentId("footermenu", 0);
            return View("MenuFooter", list);
        }

        public ActionResult SupplierFooter()
        {
            List<Supplier> list = supplierDAO.getList();
            return View("SupplierFooter", list);
        }
    }
}