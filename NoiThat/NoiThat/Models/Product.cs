using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NoiThat.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int id { get; set; }

        public int CatId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Slug { get; set; }
        [Required]
        public string Detail { get; set; }

        [Required]
        public string MetaKey { get; set; }

        [Required]
        public string MetaDes { get; set; }
        public string Img { get; set; }
        public string Origin { get; set; }
        public string Material { get; set; }
        public int? Width { get; set; }
        public int? Length { get; set; }
        public int Number { get; set; }
        public int SupplierId { get; set; }
        public decimal Price { get; set; }
        public decimal PriceSale { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int Status { get; set; }
        public decimal OriginalPrice { get; set; }

    }
}