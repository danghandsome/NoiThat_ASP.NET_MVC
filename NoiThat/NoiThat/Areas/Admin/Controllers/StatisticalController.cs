using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoiThat.DAO;
using NoiThat.Models;

namespace NoiThat.Areas.Admin.Controllers
{
    public class StatisticalController : Controller
    {
        ProductDAO productDAO = new ProductDAO();
        UserDAO userDAO = new UserDAO();
        OrderDAO orderDAO = new OrderDAO();
        OrderDetailDAO orderdetailDAO = new OrderDetailDAO();

        XCart xcart = new XCart();
        // GET: Admin/Statistical
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetStatistical(string fromDate, string toDate)
        {
            var query = from o in orderDAO.getAll()
                        join od in orderdetailDAO.getAll()
                        on o.id equals od.OrderId
                        join p in productDAO.getAll()
                        on od.ProductId equals p.id
                        select new
                        {
                            CreatedDate = o.CreatedDate,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice
                        };
            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreatedDate >= startDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreatedDate < endDate);
            }

            var result = query
    .AsEnumerable()
    .GroupBy(x => x.CreatedDate.Date)
    .Select(x => new
    {
        Date = x.Key,
        TotalBuy = x.Sum(y => y.Quantity * y.OriginalPrice),
        TotalSell = x.Sum(y => y.Quantity * y.Price),
    })
    .Select(x => new
    {
        Date = x.Date,
        DoanhThu = x.TotalSell,
        LoiNhuan = x.TotalSell - x.TotalBuy
    });

            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

    }
}