using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using NoiThat.Models;

namespace NoiThat.DAO
{
    public class TopicDAO
    {
        private NoiThatDBContext db = new NoiThatDBContext();
        //Trả về danh sách các mẫu tin
        public List<Topic> getList(string status="All")
        {
            if (status == "All")
            {
                return db.Topic.ToList(); //Select * from Topic
            }
            else
            {
                if (status == "Index")
                {
                    //Lấy ra những mẫu tin có trạng thái !=0
                    return db.Topic.Where(m=>m.Status!=0).ToList();
                }
                if (status == "Trash")
                {
                    //Lấy ra những mẫu tin có trạng thái == 0
                    return db.Topic.Where(m => m.Status == 0).ToList();
                }
            }
            return db.Topic.ToList();
        }

        //Trả về 1 mẫu tin
        public Topic getRow(int? id) 
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Topic.Find(id);
            }
        }
        public Topic getRow(string slug)
        {

            return db.Topic.Where(m => m.Slug == slug && m.Status == 1).FirstOrDefault();
        }
        public int getCount()
        {
            return db.Topic.Count();
        }

        //thêm mẫu tin
        public int Insert(Topic row)
        {
            db.Topic.Add(row);
            return db.SaveChanges();
        }

        //Cap nhat mẫu tin
        public int Update(Topic row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //Cap nhat mẫu tin
        public int Delete(Topic row)
        {
            db.Topic.Remove(row);
            return db.SaveChanges();
        }
    }
}