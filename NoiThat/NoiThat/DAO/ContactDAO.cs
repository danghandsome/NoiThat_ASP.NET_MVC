using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using NoiThat.Models;

namespace NoiThat.DAO
{
    public class ContactDAO
    {
        private NoiThatDBContext db = new NoiThatDBContext();
        //Trả về danh sách các mẫu tin
        public List<Contact> getList(string status="All")
        {
            if (status == "All")
            {
                return db.Contacts.ToList(); //Select * from Contacts
            }
            else
            {
                if (status == "Index")
                {
                    //Lấy ra những mẫu tin có trạng thái !=0
                    return db.Contacts.Where(m=>m.Status!=0).ToList();
                }
                if (status == "Trash")
                {
                    //Lấy ra những mẫu tin có trạng thái == 0
                    return db.Contacts.Where(m => m.Status == 0).ToList();
                }
            }
            return db.Contacts.ToList();
        }

        //Trả về 1 mẫu tin
        public Contact getRow(int? id) 
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Contacts.Find(id);
            }
        }
        
        //thêm mẫu tin
        public int Insert(Contact row)
        {
            db.Contacts.Add(row);
            return db.SaveChanges();
        }

        //Cap nhat mẫu tin
        public int Update(Contact row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //Cap nhat mẫu tin
        public int Delete(Contact row)
        {
            db.Contacts.Remove(row);
            return db.SaveChanges();
        }
    }
}