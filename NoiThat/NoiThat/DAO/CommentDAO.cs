using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using NoiThat.Models;

namespace NoiThat.DAO
{
    public class CommentDAO
    {
        private NoiThatDBContext db = new NoiThatDBContext();

        public List<Comment> ListComment(int parentId, int productId)
        {
            return db.Comments.Where(x => x.ParentId == parentId && x.ProductID == productId).ToList();
        }

        public List<CommentInfo> ListCommentInfo(int parentId, int productId)
        {
            var model = (from a in db.Comments
                         join b in db.Users
                         on a.UserID equals b.id
                         where a.ProductID == productId && a.ParentId == parentId

                         select new
                         {
                             id = a.id,
                             CommentMSG = a.CommentMSG,
                             DateCmt = a.DateCmt,
                             Img=b.Img,
                             ProductId = a.ProductID,
                             UserID = a.UserID,
                             Name = b.UserName,
                             ParentId = a.ParentId,
                             Rate = a.Rate,
                         }).AsEnumerable().Select(x => new CommentInfo()
                         {
                             id = x.id,
                             CommentMSG = x.CommentMSG,
                             UserID = x.UserID,
                             DateCmt = x.DateCmt,
                             ParentId = x.ParentId,
                             ProductID = x.ProductId,
                             Name = x.Name,
                             Img = x.Img,
                             Rate = x.Rate,
                         });
            return model.OrderByDescending(y => y.id).ToList();
        }

        public bool Insert(Comment row)
        {
            db.Comments.Add(row);
            db.SaveChanges();
            return true;
        }

        public int Update(Comment row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        public int Delete(Comment row)
        {
            db.Comments.Remove(row);
            return db.SaveChanges();
        }
    }
}