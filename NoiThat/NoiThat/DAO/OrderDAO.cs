using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using NoiThat.Models;

namespace NoiThat.DAO
{
    public class OrderDAO
    {
        private NoiThatDBContext db = new NoiThatDBContext();
        //Trả về danh sách các mẫu tin
        public List<Order> getList(string status = "All")
        {
            if (status == "All")
            {
                return db.Orders.ToList(); //Select * from Orders
            }
            else
            {
                if (status == "Index")
                {
                    //Lấy ra những mẫu tin có trạng thái !=0
                    return db.Orders.Where(m => m.Status != 0).ToList();
                }
                if (status == "Trash")
                {
                    //Lấy ra những mẫu tin có trạng thái == 0
                    return db.Orders.Where(m => m.Status == 0).ToList();
                }
            }
            return db.Orders.ToList();
        }

        public List<Order> getListByUserId(int userid)
        {
            return db.Orders.Where(m => m.UserId == userid).ToList();
        }

        //Trả về 1 mẫu tin
        public Order getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Orders.Find(id);
            }
        }

        public List<Order> getAll()
        {
            return db.Orders.ToList();
        }

        //thêm mẫu tin
        public int Insert(Order row)
        {
            row.CreatedDate = DateTime.Now;
            row.UpdatedAt = DateTime.Now;
            db.Orders.Add(row);
            return db.SaveChanges();
        }


        //Cap nhat mẫu tin
        public int Update(Order row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //Cap nhat mẫu tin
        public int Delete(Order row)
        {
            db.Orders.Remove(row);
            return db.SaveChanges();
        }
    }
}