using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiThat.Models
{
    [Table("Categorys")]

    public class Category
    {
        [Key]
        public int id { get; set; }
        [Required]
        //Đổi tên cách 1
        //[Display(Name = "Tên danh mục")]
        public string Name { get; set; }
        public string Slug { get; set; }
        public int? ParentId { get; set; }
        public int? Orders { get; set; }

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

    }
}