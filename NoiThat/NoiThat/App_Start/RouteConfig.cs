using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NoiThat
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Khai Bao Url = cố định
            routes.MapRoute(
                name: "TatCaSanPham",
                url: "tat-ca-san-pham",
                defaults: new { controller = "Site", action = "Product", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "TatCaBaiViet",
                url: "tat-ca-bai-viet",
                defaults: new { controller = "Site", action = "Post", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "LienHe",
                url: "lien-he",
                defaults: new { controller = "Lienhe", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "GioHang",
                url: "gio-hang",
                defaults: new { controller = "Giohang", action = "Index", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "Thanhtoan",
                url: "thanh-toan",
                defaults: new { controller = "Giohang", action = "ThanhToan", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "TimKiem",
                url: "tim-kiem",
                defaults: new { controller = "Timkiem", action = "Index", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "DangNhap",
                url: "dang-nhap",
                defaults: new { controller = "Khachhang", action = "DangNhap", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "DangKy",
                url: "dang-ky",
                defaults: new { controller = "Khachhang", action = "DangKy", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ThongTinCaNhan",
                url: "thong-tin-ca-nhan",
                defaults: new { controller = "Khachhang", action = "ThongTinCaNhan", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "DonHangCaNhan",
                url: "don-hang-ca-nhan",
                defaults: new { controller = "Khachhang", action = "DonHangCaNhan", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ChinhSuaThongTin",
                url: "chinh-sua-thong-tin",
                defaults: new { controller = "Khachhang", action = "ChinhSuaThongTin", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "DatHangThanhCong",
                url: "thanh-cong",
                defaults: new { controller = "Giohang", action = "DatHangThanhCong", id = UrlParameter.Optional }
            );


            //Khai bao url động-Nam ke tren default
            routes.MapRoute(
                name: "SiteSlug",
                url: "{slug}",
                defaults: new { controller = "Site", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Site", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
