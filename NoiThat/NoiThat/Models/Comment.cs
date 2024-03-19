using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiThat.Models
{
    [Table("Comments")]

    public class Comment
    {
        [Key]
        public int id { get; set; }

        public string CommentMSG { get; set; }

        public int ProductID { get; set; }
        public int UserID { get; set; }
        public int? ParentId { get; set; }
        public int? Rate { get; set; }
        public DateTime DateCmt { get; set; }
    }
}