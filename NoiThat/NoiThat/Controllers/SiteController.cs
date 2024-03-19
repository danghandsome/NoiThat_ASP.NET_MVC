using NoiThat.DAO;
using NoiThat.Models;
using NoiThat.Public.common;
using PagedList;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace NoiThat.Controllers
{
    public class SiteController : Controller
    {
        RateDAO rateDAO = new RateDAO();
        LinkDAO linkDAO = new LinkDAO();
        ProductDAO productDAO = new ProductDAO();
        PostDAO postDAO = new PostDAO();
        CategoryDAO categoryDAO = new CategoryDAO();
        SupplierDAO supplierDAO = new SupplierDAO();
        TopicDAO topicDAO = new TopicDAO();
        CommentDAO commentDAO = new CommentDAO();

        public ActionResult Index(string slug = null, int? page = null)
        {
            if (slug == null)
            {
                //trang chu, ko viet code
                return this.Home();
            }
            else
            {
                //tim slug co trong bang link
                Link link = linkDAO.getRow(slug);
                if (link != null)
                {
                    //slug co trong bang link
                    string typelink = link.TypeLink;
                    switch (typelink)
                    {
                        case "category":
                            {
                                return this.ProductCategory(slug, page);
                            }
                        case "topic":
                            {
                                return this.PostTopic(slug, page);

                            }
                        case "page":
                            {
                                return this.PostPage(slug);
                            }

                        default:
                            {
                                return this.Error404(slug);

                            }
                    }
                }
                else
                {
                    Product p = productDAO.getRow(slug);

                    if (p != null)
                    {
                        ProductInfo productInfo = new ProductInfo();
                        productInfo.CatId = p.CatId;
                        productInfo.id = p.id;
                        productInfo.Name = p.Name;
                        productInfo.Slug = p.Slug;
                        productInfo.Detail = p.Detail;
                        productInfo.MetaKey = p.MetaKey;
                        productInfo.MetaDes = p.MetaDes;
                        productInfo.Img = p.Img;
                        productInfo.Number = p.Number;
                        productInfo.Price = p.Price;
                        productInfo.PriceSale = p.PriceSale;
                        productInfo.SupplierId = p.SupplierId;
                        productInfo.CreatedBy = p.CreatedBy;
                        productInfo.CreatedAt = p.CreatedAt;
                        productInfo.UpdatedBy = p.UpdatedBy;
                        productInfo.UpdatedAt = p.UpdatedAt;
                        productInfo.Status = p.Status;
                        productInfo.Origin = p.Origin;
                        productInfo.Material = p.Material;
                        productInfo.Width = p.Width;
                        productInfo.Length = p.Length;
                        productInfo.CatName = categoryDAO.getRow(productInfo.CatId).Name;
                        productInfo.SupName = supplierDAO.getRow(productInfo.SupplierId).Name;
                        return this.ProductDetail(productInfo);
                    }
                    else
                    {
                        Post post = postDAO.getRow(slug, "post");
                        if (post != null)
                        {
                            return this.PostDetail(post);
                        }
                        else
                        {

                            return this.Error404(slug);
                        }
                    }
                    //slug ko co trong bang link
                }
            }
        }

        public ActionResult Home()
        {
            List<Category> list = categoryDAO.getListByParentId(0);
            return View("Home", list);
        }
        public ActionResult HomeProduct(int id)
        {
            Category category = categoryDAO.getRow(id);
            ViewBag.Category = category;

            //Danh muc theo loai cap 3
            List<int> listcatid = new List<int>();
            listcatid.Add(id);//cap1
            List<Category> listcat2 = categoryDAO.getListByParentId(id);
            if (listcat2.Count() != 0)
            {
                foreach (var cate2 in listcat2)
                {
                    listcatid.Add(cate2.id);//Cap 2
                    List<Category> listcat3 = categoryDAO.getListByParentId(cate2.id);
                    if (listcat3.Count() != 0)
                    {
                        foreach (var cate3 in listcat3)
                        {
                            listcatid.Add(cate3.id);//Cap 3
                        }
                    }
                }
            }
            List<ProductInfo> list = productDAO.getListListByCatId(listcatid, 8);
            return View("HomeProduct", list);
        }


        //nhom action product
        public ActionResult Product(int? page)
        {
            int pageNumber = page ?? 1; //trang hiện tại
            int pageSize = 5; //Số mẫu tin hiển thị trên 1 trang

            IPagedList<ProductInfo> list = productDAO.getList(pageSize, pageNumber);
            ViewBag.Sotrang = 5;
            return View("Product", list);
        }
        public ActionResult ProductCategory(string slug, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5;

            Category category = categoryDAO.getRow(slug);
            ViewBag.Category = category;

            //Danh muc theo loai cap 3
            List<int> listcatid = new List<int>();
            listcatid.Add(category.id);//cap1
            List<Category> listcat2 = categoryDAO.getListByParentId(category.id);
            if (listcat2.Count() != 0)
            {
                foreach (var cate2 in listcat2)
                {
                    listcatid.Add(cate2.id);//Cap 2
                    List<Category> listcat3 = categoryDAO.getListByParentId(cate2.id);
                    if (listcat3.Count() != 0)
                    {
                        foreach (var cate3 in listcat3)
                        {
                            listcatid.Add(cate3.id);//Cap 3
                        }
                    }
                }
            }
            IPagedList<ProductInfo> list = productDAO.getListListByCatId(listcatid, pageSize, pageNumber);
            return View("ProductCategory", list);
        }

        public ActionResult ProductSearch(string slug, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5;

            // Logic lấy thông tin danh mục dựa trên slug
            Category category = categoryDAO.getRow(slug);
            ViewBag.Category = category;

            // Tạo danh sách ID danh mục cần tìm kiếm
            List<int> listcatid = new List<int>();
            listcatid.Add(category.id); // Thêm danh mục cấp 1

            // Lấy danh sách danh mục cấp 2 và cấp 3 để thêm vào danh sách cần tìm kiếm
            List<Category> listcat2 = categoryDAO.getListByParentId(category.id);
            foreach (var cate2 in listcat2)
            {
                listcatid.Add(cate2.id); // Thêm danh mục cấp 2
                List<Category> listcat3 = categoryDAO.getListByParentId(cate2.id);
                foreach (var cate3 in listcat3)
                {
                    listcatid.Add(cate3.id); // Thêm danh mục cấp 3
                }
            }

            // Lấy danh sách sản phẩm dựa trên danh mục đã chọn
            IPagedList<ProductInfo> list = productDAO.getListListByCatId(listcatid, pageSize, pageNumber);

            // Trả về view "ProductCategory" với danh sách sản phẩm được lấy ra
            return View("ProductCategory", list);
        }


        public ActionResult ProductDetail(ProductInfo product)
        {
            Random rd = new Random();
            List<Product> listCatId = productDAO.getListByCatId(product.CatId);
            List<Product> listRandom = new List<Product>();
            int dem = 0;
            while (dem < 3)
            {
                Product productRD = listCatId[rd.Next(0, listCatId.Count() - 1)];
                if (productRD.id != product.id)
                {
                    listRandom.Add(productRD);
                    dem++;
                }
            }
            UserLogin userLogin = new UserLogin();
            if (Session["UserCustomer"] != "")
            {
                userLogin.UserID = int.Parse(Session["UserId"].ToString());
                userLogin.UserName = Session["FullName"].ToString();
            }

            ViewBag.UserID = userLogin.UserID;


            ViewBag.ListComment = new CommentDAO().ListCommentInfo(0, product.id);

            ViewBag.Product = listRandom;
            return View("ProductDetail", product);
        }


        [ChildActionOnly]
        public ActionResult _ChildComment(int parentid, int productid)
        {
            var data = new CommentDAO().ListCommentInfo(parentid, productid);
            UserLogin userLogin = new UserLogin();
            if (Session["UserCustomer"] != "")
            {
                userLogin.UserID = int.Parse(Session["UserId"].ToString());
                userLogin.UserName = Session["FullName"].ToString();
            }
            for (int k = 0; k < data.Count; k++)
            {
                data[k].UserID = userLogin.UserID;
            }
            return PartialView("~/Views/Shared/_ChildComment.cshtml", data);
        }


        [HttpPost]
        public JsonResult AddNewComment(int productid, int userid, int parentid, string commentmsg, string rate)
        {
            try
            {
                Comment comment = new Comment();

                comment.CommentMSG = commentmsg;
                comment.ProductID = productid;
                comment.UserID = userid;
                comment.ParentId = parentid;
                comment.Rate = int.Parse(rate);
                comment.DateCmt = DateTime.Now;

                bool addcomment = commentDAO.Insert(comment);
                if (addcomment == true)
                {
                    return Json(new
                    {
                        status = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false
                    });
                }
            }
            catch
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        public ActionResult GetComment(int productid)
        {
            var data = new CommentDAO().ListCommentInfo(0, productid);
            return PartialView("~/Views/Shared/_ChildComment.cshtml", data);
        }

        //nhom post
        public ActionResult Post(int? page)
        {
            List<PostInfo> list = postDAO.getList("Post");


            return View("Post", list);
        }


        public ActionResult PostTopic(string slug, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5;

            Topic topic = topicDAO.getRow(slug);
            ViewBag.Topic = topic;
            IPagedList<PostInfo> list = postDAO.getListByTopicId(slug, pageSize, pageNumber);
            return View("PostTopic", list);
        }
        public ActionResult PostPage(string slug)
        {
            Post post = postDAO.getRow(slug, "page");
            return View("PostPage", post);
        }
        public ActionResult PostDetail(Post post)
        {
            return View("PostDetail", post);
        }
        //Nha cung cap
        public ActionResult Supplier()
        {
            List<Supplier> list = supplierDAO.getList();
            return View("Supplier", list);
        }

        public ActionResult SupplierDetail(Supplier supplier)
        {

            return View("SupplierDetail", supplier);
        }

        //Ham Loi
        public ActionResult Error404(string slug)
        {

            return View("Error404");
        }
        public ActionResult AddRate(int productid, int rate)
        {
            if (rateDAO.getRow(int.Parse(Session["UserId"].ToString()), productid) == null)
            {
                Rate rate1 = new Rate();
                rate1.productID = productid;
                rate1.rate = rate;
                rate1.userID = int.Parse(Session["UserId"].ToString());
                rateDAO.Insert(rate1);
            }

            // Chuyển hướng trở lại ProductDetail
            return Index(productDAO.getRow(productid).Slug);
        }
        public ActionResult Livingroom()
        { 
            return View(); 
        }

    }
}