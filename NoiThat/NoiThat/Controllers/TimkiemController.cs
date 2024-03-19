using NoiThat.DAO;
using NoiThat.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace NoiThat.Controllers
{
    public class TimkiemController : Controller
    {
        LinkDAO linkDAO = new LinkDAO();
        ProductDAO productDAO = new ProductDAO();
        PostDAO postDAO = new PostDAO();
        CategoryDAO categoryDAO = new CategoryDAO();
        SupplierDAO supplierDAO = new SupplierDAO();
        // GET: Search
        public ActionResult TimKiem(string slug, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5;

            Category category = categoryDAO.getRow(slug);
            ViewBag.Category = category;

            //Danh muc theo loai cap 3
            List<int> listcatid = new List<int>();
            listcatid.Add(category.id);//cap1
            List<Category> listcat2 = categoryDAO.getListByParentId(category.id);
            if (listcat2.Count() != 0)
            {
                foreach (var cate2 in listcat2)
                {
                    listcatid.Add(cate2.id);//Cap 2
                    List<Category> listcat3 = categoryDAO.getListByParentId(cate2.id);
                    if (listcat3.Count() != 0)
                    {
                        foreach (var cate3 in listcat3)
                        {
                            listcatid.Add(cate3.id);//Cap 3
                        }
                    }
                }
            }
            IPagedList<ProductInfo> list = productDAO.getListListByCatId(listcatid, pageSize, pageNumber);
            return View("TimKiem", list);
        }
    }
}