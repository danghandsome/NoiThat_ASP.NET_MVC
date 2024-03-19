using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Compilation;
using NoiThat.Models;
using PagedList;

namespace NoiThat.DAO
{
    public class ProductDAO
    {
        private NoiThatDBContext db = new NoiThatDBContext();
        //Trả về danh sách các mẫu tin
        public List<ProductInfo> getList(string status = "All")
        {
            List<ProductInfo> listCat = new List<ProductInfo>();
            if (status == "All")
            {
                listCat = db.Products.Join(
                    db.Categories, p => p.CatId, c => c.id,
                    (p, c) => new ProductInfo
                    {
                        id = p.id,
                        CatId = p.CatId,
                        Name = p.Name,
                        CatName = c.Name,
                        Slug = p.Slug,
                        Detail = p.Detail,
                        MetaKey = p.MetaKey,
                        MetaDes = p.MetaDes,
                        Img = p.Img,
                        Number = p.Number,
                        SupplierId = p.SupplierId,
                        Price = p.Price,
                        Origin = p.Origin,
                        Material = p.Material,
                        Width = p.Width,
                        Length = p.Length,
                        PriceSale = p.PriceSale,
                        CreatedBy = p.CreatedBy,
                        CreatedAt = p.CreatedAt,
                        UpdatedBy = p.UpdatedBy,
                        UpdatedAt = p.UpdatedAt,
                        Status = p.Status
                    }
                 )
                .OrderByDescending(m => m.CreatedAt).ToList();

            }

            if (status == "Index")
            {
                //Lấy ra những mẫu tin có trạng thái !=0
                listCat = db.Products.Join(
                db.Categories, p => p.CatId, c => c.id,
                (p, c) => new ProductInfo
                {
                    id = p.id,
                    CatId = p.CatId,
                    Name = p.Name,
                    CatName = c.Name,
                    Slug = p.Slug,
                    Detail = p.Detail,
                    MetaKey = p.MetaKey,
                    MetaDes = p.MetaDes,
                    Img = p.Img,
                    Number = p.Number,
                    SupplierId = p.SupplierId,
                    Price = p.Price,
                    Origin = p.Origin,
                    Material = p.Material,
                    Width = p.Width,
                    Length = p.Length,
                    PriceSale = p.PriceSale,
                    CreatedBy = p.CreatedBy,
                    CreatedAt = p.CreatedAt,
                    UpdatedBy = p.UpdatedBy,
                    UpdatedAt = p.UpdatedAt,
                    Status = p.Status
                }
             )
            .Where(m => m.Status != 0)
            .OrderByDescending(m => m.CreatedAt).ToList();

            }
            if (status == "Trash")
            {
                //Lấy ra những mẫu tin có trạng thái == 0
                listCat = db.Products.Join(
                db.Categories, p => p.CatId, c => c.id,
                (p, c) => new ProductInfo
                {
                    id = p.id,
                    CatId = p.CatId,
                    Name = p.Name,
                    CatName = c.Name,
                    Slug = p.Slug,
                    Detail = p.Detail,
                    MetaKey = p.MetaKey,
                    MetaDes = p.MetaDes,
                    Img = p.Img,
                    Number = p.Number,
                    SupplierId = p.SupplierId,
                    Price = p.Price,
                    Origin = p.Origin,
                    Material = p.Material,
                    Width = p.Width,
                    Length = p.Length,
                    PriceSale = p.PriceSale,
                    CreatedBy = p.CreatedBy,
                    CreatedAt = p.CreatedAt,
                    UpdatedBy = p.UpdatedBy,
                    UpdatedAt = p.UpdatedAt,
                    Status = p.Status
                }
                )
                .Where(m => m.Status == 0)
                .OrderByDescending(m => m.CreatedAt).ToList();
            }
            return listCat.Join(
                db.Suppliers, p => p.SupplierId, c => c.id,
                    (p, c) => new ProductInfo
                    {
                        id = p.id,
                        CatId = p.CatId,
                        Name = p.Name,
                        CatName = c.Name,
                        Slug = p.Slug,
                        Detail = p.Detail,
                        MetaKey = p.MetaKey,
                        MetaDes = p.MetaDes,
                        Img = p.Img,
                        Number = p.Number,
                        SupplierId = p.SupplierId,
                        Price = p.Price,
                        Origin = p.Origin,
                        Material = p.Material,
                        Width = p.Width,
                        Length = p.Length,
                        PriceSale = p.PriceSale,
                        CreatedBy = p.CreatedBy,
                        CreatedAt = p.CreatedAt,
                        UpdatedBy = p.UpdatedBy,
                        UpdatedAt = p.UpdatedAt,
                        Status = p.Status
                    }
                 ).ToList();
        }
        // tich diem

        public List<Product> getListByCatId(int catid)
        {
            List<Product> list = db.Products
                .Where(m => m.Status == 1 && m.CatId == catid)
                .OrderByDescending(m => m.CreatedAt)
                .ToList();
            return list;
        }

        public List<Product> getAll()
        {
            return db.Products.ToList();
        }

        public List<ProductInfo> getListListByCatId(List<int> listcatid, int limit)
        {

            List<ProductInfo> listCat = db.Products.Join(
                    db.Categories, p => p.CatId, c => c.id,
                    (p, c) => new ProductInfo
                    {
                        id = p.id,
                        CatId = p.CatId,
                        Name = p.Name,
                        CatName = c.Name,
                        Slug = p.Slug,
                        Detail = p.Detail,
                        MetaKey = p.MetaKey,
                        MetaDes = p.MetaDes,
                        Img = p.Img,
                        Number = p.Number,
                        SupplierId = p.SupplierId,
                        Price = p.Price,
                        Origin = p.Origin,
                        Material = p.Material,
                        Width = p.Width,
                        Length = p.Length,
                        PriceSale = p.PriceSale,
                        CreatedBy = p.CreatedBy,
                        CreatedAt = p.CreatedAt,
                        UpdatedBy = p.UpdatedBy,
                        UpdatedAt = p.UpdatedAt,
                        Status = p.Status
                    }
                 )
                .Where(m => m.Status == 1)
                .OrderByDescending(m => m.CreatedAt).ToList();


            
            List<ProductInfo> list = listCat
                .Join(
                    db.Suppliers, p => p.SupplierId, c => c.id,
                    (p, c) => new ProductInfo
                    {
                        id = p.id,
                        CatId = p.CatId,
                        Name = p.Name,
                        CatName = p.CatName,
                        Slug = p.Slug,
                        Detail = p.Detail,
                        MetaKey = p.MetaKey,
                        MetaDes = p.MetaDes,
                        Img = p.Img,
                        Number = p.Number,
                        SupName = c.Name,
                        Price = p.Price,
                        Origin = p.Origin,
                        Material = p.Material,
                        Width = p.Width,
                        Length = p.Length,
                        PriceSale = p.PriceSale,
                        SupplierId = p.SupplierId,
                        CreatedBy = p.CreatedBy,
                        CreatedAt = p.CreatedAt,
                        UpdatedBy = p.UpdatedBy,
                        UpdatedAt = p.UpdatedAt,
                        Status = p.Status
                    }
                 )
                .Where(m => m.Status == 1 && listcatid.Contains(m.CatId))
                .OrderByDescending(m => m.CreatedAt)
                .Take(limit)
                .ToList();
            return list;
        }

        public IPagedList<ProductInfo> getListListByCatId(List<int> listcatid, int pageSize, int pageNumber)
        {
            List<ProductInfo> listCat = db.Products.Join(
                    db.Categories, p => p.CatId, c => c.id,
                    (p, c) => new ProductInfo
                    {
                        id = p.id,
                        CatId = p.CatId,
                        Name = p.Name,
                        CatName = c.Name,
                        Slug = p.Slug,
                        Detail = p.Detail,
                        MetaKey = p.MetaKey,
                        MetaDes = p.MetaDes,
                        Img = p.Img,
                        Number = p.Number,
                        SupplierId = p.SupplierId,
                        Price = p.Price,
                        Origin = p.Origin,
                        Material = p.Material,
                        Width = p.Width,
                        Length = p.Length,
                        PriceSale = p.PriceSale,
                        CreatedBy = p.CreatedBy,
                        CreatedAt = p.CreatedAt,
                        UpdatedBy = p.UpdatedBy,
                        UpdatedAt = p.UpdatedAt,
                        Status = p.Status
                    }
                 )
                .Where(m => m.Status == 1)
                .OrderByDescending(m => m.CreatedAt).ToList();


            IPagedList<ProductInfo> list = listCat
                .Join(
                    db.Suppliers, p => p.SupplierId, c => c.id,
                    (p, c) => new ProductInfo
                    {
                        id = p.id,
                        CatId = p.CatId,
                        Name = p.Name,
                        CatName = p.CatName,
                        SupName = c.Name,
                        Slug = p.Slug,
                        Detail = p.Detail,
                        MetaKey = p.MetaKey,
                        MetaDes = p.MetaDes,
                        Img = p.Img,
                        Number = p.Number,
                        SupplierId = p.SupplierId,
                        Price = p.Price,
                        Origin = p.Origin,
                        Material = p.Material,
                        Width = p.Width,
                        Length = p.Length,
                        PriceSale = p.PriceSale,
                        CreatedBy = p.CreatedBy,
                        CreatedAt = p.CreatedAt,
                        UpdatedBy = p.UpdatedBy,
                        UpdatedAt = p.UpdatedAt,
                        Status = p.Status
                    }
                 )
                .Where(m => m.Status == 1 && listcatid.Contains(m.CatId))
                .OrderByDescending(m => m.CreatedAt)
                .ToPagedList(pageNumber, pageSize);

            return list;
        }

        public IPagedList<ProductInfo> getList(int pageSize, int pageNumber)
        {
            List<ProductInfo> listCat = db.Products.Join(
                    db.Categories, p => p.CatId, c => c.id,
                    (p, c) => new ProductInfo
                    {
                        id = p.id,
                        CatId = p.CatId,
                        Name = p.Name,
                        CatName = c.Name,
                        Slug = p.Slug,
                        Detail = p.Detail,
                        MetaKey = p.MetaKey,
                        MetaDes = p.MetaDes,
                        Img = p.Img,
                        Number = p.Number,
                        SupplierId = p.SupplierId,
                        Price = p.Price,
                        Origin = p.Origin,
                        Material = p.Material,
                        Width = p.Width,
                        Length = p.Length,
                        PriceSale = p.PriceSale,
                        CreatedBy = p.CreatedBy,
                        CreatedAt = p.CreatedAt,
                        UpdatedBy = p.UpdatedBy,
                        UpdatedAt = p.UpdatedAt,
                        Status = p.Status
                    }
                 )
                .Where(m => m.Status == 1)
                .OrderByDescending(m => m.CreatedAt).ToList();

            
            IPagedList<ProductInfo> list = listCat
                .Join(
                    db.Suppliers, p => p.SupplierId, c => c.id,
                    (p, c) => new ProductInfo
                    {
                        id = p.id,
                        CatId = p.CatId,
                        Name = p.Name,
                        CatName = p.CatName,
                        SupName = c.Name,
                        Slug = p.Slug,
                        Detail = p.Detail,
                        MetaKey = p.MetaKey,
                        MetaDes = p.MetaDes,
                        Img = p.Img,
                        Number = p.Number,
                        SupplierId = p.SupplierId,
                        Price = p.Price,
                        Origin = p.Origin,
                        Material = p.Material,
                        Width = p.Width,
                        Length = p.Length,
                        PriceSale = p.PriceSale,
                        CreatedBy = p.CreatedBy,
                        CreatedAt = p.CreatedAt,
                        UpdatedBy = p.UpdatedBy,
                        UpdatedAt = p.UpdatedAt,
                        Status = p.Status
                    }
                 )
                .Where(m => m.Status == 1)
                .OrderByDescending(m => m.CreatedAt)
                .ToPagedList(pageNumber, pageSize);

            return list;
        }
        //Trả về 1 mẫu tin
        public Product getRow(int? id)
        {

            return db.Products.Find(id);
        }
        public Product getRow(string slug)
        {
            return db.Products.Where(m => m.Slug == slug && m.Status == 1).FirstOrDefault();
        }

        //thêm mẫu tin
        public int Insert(Product row)
        {
            db.Products.Add(row);
            return db.SaveChanges();
        }

        //Cap nhat mẫu tin
        public int Update(Product row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //Cap nhat mẫu tin
        public int Delete(Product row)
        {
            db.Products.Remove(row);
            return db.SaveChanges();
        }
    }
}