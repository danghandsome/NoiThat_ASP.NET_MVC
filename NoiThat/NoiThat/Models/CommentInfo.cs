using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoiThat.Models
{
    public class CommentInfo
    {
        public int id { get; set; }
        public string CommentMSG { get; set; }
        public int ProductID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public int? ParentId { get; set; }
        public int? Rate { get; set; }
        public DateTime DateCmt { get; set; }

    }
}