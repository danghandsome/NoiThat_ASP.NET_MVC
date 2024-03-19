using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiThat.Models
{
    [Table("Posts")]
    public class Post
    {
        [Key]
        public int id { get; set; }

        public int TopicId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Slug { get; set; }
        [Required]
        public string Detail { get; set; }

        [Required]
        public string MetaKey { get; set; }

        [Required]
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