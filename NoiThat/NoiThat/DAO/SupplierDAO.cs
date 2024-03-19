using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using NoiThat.Models;

namespace NoiThat.DAO
{
    public class SupplierDAO
    {
        private NoiThatDBContext db = new NoiThatDBContext();
        //Trả về danh sách các mẫu tin
        public List<Supplier> getList(string status="All")
        {
            if (status == "All")
            {
                return db.Suppliers.ToList(); //Select * from Suppliers
            }
            else
            {
                if (status == "Index")
                {
                    //Lấy ra những mẫu tin có trạng thái !=0
                    return db.Suppliers.Where(m=>m.Status!=0).ToList();
                }
                if (status == "Trash")
                {
                    //Lấy ra những mẫu tin có trạng thái == 0
                    return db.Suppliers.Where(m => m.Status == 0).ToList();
                }
            }
            return db.Suppliers.ToList();
        }

        //Trả về 1 mẫu tin
        public Supplier getRow(int? id) 
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Suppliers.Find(id);
            }
        }

        public int getCount()
        {
            return db.Suppliers.Count();
        }

        //thêm 
        public int Insert(Supplier row)
        {
            db.Suppliers.Add(row);
            return db.SaveChanges();
        }

        //Cap nhat 
        public int Update(Supplier row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //Xóa
        public int Delete(Supplier row)
        {
            db.Suppliers.Remove(row);
            return db.SaveChanges();
        }
    }
}