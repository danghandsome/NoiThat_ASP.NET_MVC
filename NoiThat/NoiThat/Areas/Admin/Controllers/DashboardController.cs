using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoiThat.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            if(Session["UserAdmin"].Equals(""))
            {
                return Redirect("~/Admin/login");
            }
            return View();
        }
    }
}