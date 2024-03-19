using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoiThat.Models
{
    public class PostInfo
    {
        public int id { get; set; }

        public string Topic { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Detail { get; set; }
        public string MetaKey { get; set; }
        public string MetaDes { get; set; }
        
        public string Img { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int Status { get; set; }
        public string PostType { get; set; }
    }
}