using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoiThat
{
    public class XCart
    {
        public List<CartItem> AddCart(CartItem cartitem, int productid)
        {
            List<CartItem> listcart;
            if (System.Web.HttpContext.Current.Session["MyCart"].Equals(""))
            {
                listcart = new List<CartItem>();
                listcart.Add(cartitem);
                System.Web.HttpContext.Current.Session["MyCart"] = listcart;
            }
            else
            {
                listcart = (List<CartItem>)System.Web.HttpContext.Current.Session["MyCart"];

                //da co trong gio hang
                if (listcart.Where(m => m.ProductId == cartitem.ProductId).Count() > 0)
                {
                    int pos = 0;
                    foreach (var item in listcart)
                    {
                        if (item.ProductId == cartitem.ProductId)
                        {
                            listcart[pos].Qty++;
                            listcart[pos].Amount = listcart[pos].Price * listcart[pos].Qty;
                        }
                        pos++;
                    }
                    System.Web.HttpContext.Current.Session["MyCart"] = listcart;
                }
                //chua co trong gio hang
                else
                {
                    listcart.Add(cartitem);
                    System.Web.HttpContext.Current.Session["MyCart"] = listcart;
                }
            }
            return listcart;
        }
        public void UpdateCart()
        {

        }
        public void DelCart(int productid)
        {
            if (!System.Web.HttpContext.Current.Session["MyCart"].Equals(""))
            {
                List<CartItem> listcart = (List<CartItem>)System.Web.HttpContext.Current.Session["MyCart"];
                int pos = 0;
                foreach (var item in listcart)
                {
                    if (item.ProductId == productid)
                    {
                        listcart.RemoveAt(pos);
                        break;
                    }
                    pos++;
                }
                System.Web.HttpContext.Current.Session["MyCart"] = listcart;

            }
        }
        public void CartPlus(int productid)
        {
            if (!System.Web.HttpContext.Current.Session["MyCart"].Equals(""))
            {
                List<CartItem> listcart = (List<CartItem>)System.Web.HttpContext.Current.Session["MyCart"];
                int pos = 0;
                foreach (var item in listcart)
                {
                    if (item.ProductId == productid)
                    {
                        listcart[pos].Qty++;
                        listcart[pos].Amount = listcart[pos].Price * listcart[pos].Qty; 
                        break;
                    }
                    pos++;
                }
                System.Web.HttpContext.Current.Session["MyCart"] = listcart;

            }
        }
        public void CartMinus(int productid)
        {
            if (!System.Web.HttpContext.Current.Session["MyCart"].Equals(""))
            {
                List<CartItem> listcart = (List<CartItem>)System.Web.HttpContext.Current.Session["MyCart"];
                int pos = 0;
                foreach (var item in listcart)
                {
                    if (item.ProductId == productid && listcart[pos].Qty>1)
                    {
                        listcart[pos].Qty--;
                        listcart[pos].Amount = listcart[pos].Price * listcart[pos].Qty; 
                        break;
                    }
                    pos++;
                }
                System.Web.HttpContext.Current.Session["MyCart"] = listcart;

            }
        }
        public List<CartItem> getCart()
        {
            if (System.Web.HttpContext.Current.Session["MyCart"].Equals(""))
            {
                return null;
            }
            return (List<CartItem>)System.Web.HttpContext.Current.Session["MyCart"];
        }

    }
}