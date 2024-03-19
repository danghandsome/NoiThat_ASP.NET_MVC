using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NoiThat
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_Start()
        {
            //Lưu thông tin đăng nhập quản lý
            Session["UserAdmin"] = "";
            Session["UserId"] = "";
            Session["Roles"] = "";

            //Lưu thông tin đăng nhập người dùng
            Session["UserCustomer"] = "";
            Session["FullName"] = "";
            Session["Img"] = "";
            Session["Phone"] = "";
            Session["Address"] = "";
            Session["Email"] = "";
            //Giỏ hàng
            Session["MyCart"] = "";

            //Sản phẩm
            Session["ProductId"] = "";
        }
    }
}
