using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using NoiThat.Models;

namespace NoiThat.DAO
{
    public class LinkDAO
    {
        private NoiThatDBContext db = new NoiThatDBContext();

        //Trả về 1 mẫu tin
        public Link getRow(int? id) 
        {

            return db.Links.Find(id);
        }
        
        public Link getRow(int tableid, string typelink) 
        {

            return db.Links.Where(m => m.TableId == tableid && m.TypeLink == typelink).FirstOrDefault();
        }
        
        public Link getRow(string slug) 
        {

            return db.Links.Where(m => m.Slug==slug).FirstOrDefault();
        }
        
        //thêm mẫu tin
        public int Insert(Link row)
        {
            db.Links.Add(row);
            return db.SaveChanges();
        }

        //Cap nhat mẫu tin
        public int Update(Link row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //Cap nhat mẫu tin
        public int Delete(Link row)
        {
            db.Links.Remove(row);
            return db.SaveChanges();
        }
    }
}