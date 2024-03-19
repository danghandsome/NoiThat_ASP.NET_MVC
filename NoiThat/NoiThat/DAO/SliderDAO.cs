using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using NoiThat.Models;

namespace NoiThat.DAO
{
    public class SliderDAO
    {
        private NoiThatDBContext db = new NoiThatDBContext();

        public List<Slider> getListByPosition(string position)
        {
            return db.Sliders
                .Where(m=>m.Position==position && m.Status==1)
                .OrderBy(m=>m.Orders)
                .ToList();
        }


        //Trả về danh sách các mẫu tin
        public List<Slider> getList(string status="All")
        {
            if (status == "All")
            {
                return db.Sliders.ToList(); //Select * from Sliders
            }
            else
            {
                if (status == "Index")
                {
                    //Lấy ra những mẫu tin có trạng thái !=0
                    return db.Sliders.Where(m=>m.Status!=0).ToList();
                }
                if (status == "Trash")
                {
                    //Lấy ra những mẫu tin có trạng thái == 0
                    return db.Sliders.Where(m => m.Status == 0).ToList();
                }
            }
            return db.Sliders.ToList();
        }

        //Trả về 1 mẫu tin
        public Slider getRow(int? id) 
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Sliders.Find(id);
            }
        }
        
        //thêm mẫu tin
        public int Insert(Slider row)
        {
            db.Sliders.Add(row);
            return db.SaveChanges();
        }

        //Cap nhat mẫu tin
        public int Update(Slider row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //Cap nhat mẫu tin
        public int Delete(Slider row)
        {
            db.Sliders.Remove(row);
            return db.SaveChanges();
        }
    }
}