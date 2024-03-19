using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using NoiThat.Models;

namespace NoiThat.DAO
{
    public class ConfigDAO
    {
        private NoiThatDBContext db = new NoiThatDBContext();
        //Trả về danh sách các mẫu tin
        //public List<Config> getList(string status="All")
        //{
        //    if (status == "All")
        //    {
        //        return db.Configs.ToList(); //Select * from Configs
        //    }
        //    else
        //    {
        //        if (status == "Index")
        //        {
        //            //Lấy ra những mẫu tin có trạng thái !=0
        //            return db.Configs.Where(m=>m.Status!=0).ToList();
        //        }
        //        if (status == "Trash")
        //        {
        //            //Lấy ra những mẫu tin có trạng thái == 0
        //            return db.Configs.Where(m => m.Status == 0).ToList();
        //        }
        //    }
        //    return db.Configs.ToList();
        //}

        //Trả về 1 mẫu tin
        public Config getRow(int? id) 
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Configs.Find(id);
            }
        }
        
        //thêm mẫu tin
        public int Insert(Config row)
        {
            db.Configs.Add(row);
            return db.SaveChanges();
        }

        //Cap nhat mẫu tin
        public int Update(Config row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //Cap nhat mẫu tin
        public int Delete(Config row)
        {
            db.Configs.Remove(row);
            return db.SaveChanges();
        }
    }
}