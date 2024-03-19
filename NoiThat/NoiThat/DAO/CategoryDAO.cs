using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using NoiThat.Models;

namespace NoiThat.DAO
{
    public class CategoryDAO
    {
        private NoiThatDBContext db = new NoiThatDBContext();
        public List<Category> getListByParentId(int parentid = 1)
        {
            return db.Categories.Where(m => m.ParentId == parentid && m.Status == 1).OrderBy(m => m.Orders).ToList();
        }


        //Trả về danh sách các mẫu tin
        public List<Category> getList(string status = "All")
        {
            List<Category> list = new List<Category>();
            switch (status)
            {
                case "Index":
                    {
                        list = db.Categories.Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Categories.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        list = db.Categories.ToList();
                        break;
                    }

            }

            return list;
        }

        //Trả về 1 mẫu tin
        public Category getRow(int? id)
        {
            return db.Categories.Find(id);
        }
        public Category getRow(string slug)
        {

            return db.Categories.Where(m => m.Slug == slug && m.Status == 1).FirstOrDefault();
        }

        public int getCount()
        {
            return db.Categories.Count();
        }

        //thêm mẫu tin
        public int Insert(Category row)
        {
            db.Categories.Add(row);
            return db.SaveChanges();
        }

        //Cap nhat mẫu tin
        public int Update(Category row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //Cap nhat mẫu tin
        public int Delete(Category row)
        {
            db.Categories.Remove(row);
            return db.SaveChanges();
        }
    }
}