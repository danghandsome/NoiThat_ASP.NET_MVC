using NoiThat.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NoiThat.DAO
{
    public class RateDAO
    {
        private NoiThatDBContext db = new NoiThatDBContext();
        //Trả về danh sách các mẫu tin
        public List<Rate> getList()
        {
            return db.Rates.ToList();
        }

        public List<Rate> getListByUserId(int userid)
        {
            return db.Rates.Where(m => m.userID == userid).ToList();
        }
        public List<Rate> getListByProductId(int productId)
        {
            return db.Rates.Where(m => m.productID == productId).ToList();
        }

        //Trả về 1 mẫu tin
        public Rate getRow(int userid, int productid)
        {
            return db.Rates.Where(r => r.productID == productid && r.userID == userid).FirstOrDefault();
        }

        public double AvarageRate(int productid)
        {
            double aRate = 0;
            foreach (var rate in getListByProductId(productid))
            {
                aRate += rate.rate;
            }
            if (getListByProductId(productid).Count() != 0)
            {

                return Math.Round(aRate / getListByProductId(productid).Count());
            }
            return 0;
        }

        public int SoNguoiDanhGia(int productid, int rate)
        {
            return db.Rates.Where(r => r.productID == productid && r.rate == rate).Count();
        }

        //thêm mẫu tin
        public int Insert(Rate row)
        {
            db.Rates.Add(row);
            return db.SaveChanges();
        }


        //Cap nhat mẫu tin
        public int Update(Rate row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //Cap nhat mẫu tin
        public int Delete(Rate row)
        {
            db.Rates.Remove(row);
            return db.SaveChanges();
        }
    }
}