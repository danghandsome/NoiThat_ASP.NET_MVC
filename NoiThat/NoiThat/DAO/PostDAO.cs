using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using NoiThat.Models;
using PagedList;

namespace NoiThat.DAO
{
    public class PostDAO
    {
        private NoiThatDBContext db = new NoiThatDBContext();

        public IPagedList<PostInfo> getListByTopicId(string slug,int pageSize,int pageNumber)
        {
            IPagedList<PostInfo> list = db.Posts.Join(
                db.Topic, p => p.TopicId, t => t.id, (p, t) => new PostInfo
                {
                    id = p.id,
                    Topic = t.Name,
                    Title = p.Title,
                    Slug = p.Slug,
                    Detail = p.Detail,
                    MetaKey = p.MetaKey,
                    MetaDes = p.MetaDes,
                    Img = p.Img,
                    CreatedBy = p.CreatedBy,
                    CreatedAt = p.CreatedAt,
                    UpdatedBy = p.UpdatedBy,
                    UpdatedAt = p.UpdatedAt,
                    Status = p.Status,
                    PostType = p.PostType
                }
                ).Where(m => m.Status != 1 && m.PostType == "Post")
                .OrderByDescending(m => m.CreatedAt)
                .ToPagedList(pageNumber, pageSize);
            return list;
        }


        //Trả về danh sách các mẫu tin
        public List<PostInfo> getList(string type = "Post")
        {
            List<PostInfo> list = db.Posts.Join(
                db.Topic, p => p.TopicId, t => t.id, (p, t) => new PostInfo
                {
                    id = p.id,
                    Topic = t.Name,
                    Title = p.Title,
                    Slug = p.Slug,
                    Detail = p.Detail,
                    MetaKey = p.MetaKey,
                    MetaDes = p.MetaDes,
                    Img = p.Img,
                    CreatedBy = p.CreatedBy,
                    CreatedAt = p.CreatedAt,
                    UpdatedBy = p.UpdatedBy,
                    UpdatedAt = p.UpdatedAt,
                    Status = p.Status,
                    PostType = p.PostType
                }
                ).Where(m => m.Status != 0 && m.PostType == type).ToList();
            return list;
        }
        public List<Post> getList(string status="All", string type="Post")
        {

            if (status == "All")
            {
                return db.Posts.Where(m => m.PostType == type).ToList(); //Select * from Posts
            }
            else
            {
                if (status == "Index")
                {
                    //Lấy ra những mẫu tin có trạng thái !=0
                    return db.Posts.Where(m=>m.Status !=0 && m.PostType == type).ToList();
                }
                if (status == "Trash")
                {
                    //Lấy ra những mẫu tin có trạng thái == 0
                    return db.Posts.Where(m => m.Status == 0 && m.PostType == type).ToList();
                }
            }
            return db.Posts.Where(m => m.PostType == type).ToList();
        }




        //Trả về 1 mẫu tin
        public Post getRow(int? id) 
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Posts.Find(id);
            }
        }
        public Post getRow(string slug) 
        {
                return db.Posts.Where(m=>m.PostType=="Post"&&m.Slug==slug&&m.Status==1).FirstOrDefault();
        }
        public Post getRow(string slug, string posttype) 
        {
                return db.Posts.Where(m=>m.PostType==posttype&&m.Slug==slug&&m.Status==1).FirstOrDefault();
        }

        public int getCount()
        {
            return db.Posts.Count();
        }
        
        //thêm mẫu tin
        public int Insert(Post row)
        {
            db.Posts.Add(row);
            return db.SaveChanges();
        }

        //Cap nhat mẫu tin
        public int Update(Post row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //Xoa mẫu tin
        public int Delete(Post row)
        {
            db.Posts.Remove(row);
            return db.SaveChanges();
        }
    }
}