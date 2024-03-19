using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NoiThat.Models
{
    public class NoiThatDBContext:DbContext
    {
        public NoiThatDBContext() :base("name=StrConnect") { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Topic> Topic { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rate> Rates { get; set; }
        //public DbSet<Contacts> Contactss { get; set; }

        //public System.Data.Entity.DbSet<NoiThat.Models.CommentInfo> CommentInfoes { get; set; }
    }
}