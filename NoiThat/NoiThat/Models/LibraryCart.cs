using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoiThat.Models
{
    public static class LibraryCart
    {
        private static NoiThatDBContext data = new NoiThatDBContext();
        public static List<CartItem> GetCart()
        {
            List<CartItem> list = HttpContext.Current.Session["MyCart"] as List<CartItem>;
            if (list == null)
            {
                list = new List<CartItem>();
                HttpContext.Current.Session["MyCart"] = list;
            }
            return list;
        }

        public static int TotalQuantity()
        {
            int s = 0;
            List<CartItem> listCart = HttpContext.Current.Session["MyCart"] as List<CartItem>;
            if (listCart != null)
                s = listCart.Sum(m => m.Qty);
            return s;
        }

        public static decimal TotalPrice()
        {
            decimal tt = 0;     
            List<CartItem> listcart = HttpContext.Current.Session["MyCart"] as List<CartItem>;
            if (listcart != null)
                tt = listcart.Sum(m => m.Amount);
            return tt;
        }

        //public static string GetFullNameLogin()
        //{
        //    var guest = HttpContext.Current.Session["Account"] as tb_guest;
        //    return guest != null ? guest.Fullname : "Tài khoản vô danh";
        //}

        //public static bool CheckUser(string username, string email)
        //{
        //    var check = data.tb_guests.Any(m => m.Username == username || m.Email == email);
        //    if (check == true)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
    }
}