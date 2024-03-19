using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using NoiThat.Models;

namespace NoiThat.DAO
{
    public class UserDAO
    {
        private NoiThatDBContext db = new NoiThatDBContext();
        //Trả về danh sách các mẫu tin
        public List<User> getList(string status="All")
        {
            if (status == "All")
            {
                return db.Users.ToList(); //Select * from Users
            }
            
            else
            {
                if (status == "Index")
                {
                    //Lấy ra những mẫu tin có trạng thái !=0
                    return db.Users.Where(m=>m.Status!=0 ).ToList();
                }
                if (status == "Trash")
                {
                    //Lấy ra những mẫu tin có trạng thái == 0
                    return db.Users.Where(m => m.Status == 0).ToList();
                }
            }
            return db.Users.ToList();
        }

        //Trả về 1 mẫu tin
        public User getRow(int? id) 
        {
            
                return db.Users.Find(id);
        }
        public int getScore(int? id)
        {

            return db.Users.Find(id).Score;
        }
        public User getRow(string username) 
        {
            
                return db.Users.Where(m => m.Status == 1 && m.UserName == username).FirstOrDefault();
        }
        
        //thêm mẫu tin
        public int Insert(User row)
        {
            db.Users.Add(row);
            return db.SaveChanges();
        }

        //Cap nhat mẫu tin
        public int Update(User row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //Cap nhat mẫu tin
        public int Delete(User row)
        {
            db.Users.Remove(row);
            return db.SaveChanges();
        }
    }
}