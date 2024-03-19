using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using NoiThat.Models;

namespace NoiThat.DAO
{
    public class OrderDetailDAO
    {
        private NoiThatDBContext db = new NoiThatDBContext();
        //Trả về danh sách các mẫu tin
        public List<OrderDetail> getList(int? orderid)
        {

            return db.OrderDetails
                .Where(m=>m.OrderId==orderid)
                .ToList();
        }

        public List<OrderDetail> getAll()
        {

            return db.OrderDetails.ToList();
        }

        //Trả về 1 mẫu tin
        public OrderDetail getRow(int? id) 
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.OrderDetails.Find(id);
            }
        }
        
        //thêm mẫu tin
        public int Insert(OrderDetail row)
        {
            db.OrderDetails.Add(row);
            return db.SaveChanges();
        }

        //Cap nhat mẫu tin
        public int Update(OrderDetail row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //Cap nhat mẫu tin
        public int Delete(OrderDetail row)
        {
            db.OrderDetails.Remove(row);
            return db.SaveChanges();
        }
        public int SoLuotMua(int productId)
        {
            int soluotmua = 0;
            foreach (var item in db.OrderDetails)
            {
                if (item.ProductId == productId)
                {
                    soluotmua++;
                }
            }
            return soluotmua;
        }
    }
}