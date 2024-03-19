using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NoiThat.Models
{
    [Table("Suppliers")]
    public class Supplier
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Img { get; set; }
        public string Address { get; set; }
        public int? Orders { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int Status { get; set; }

    }
}