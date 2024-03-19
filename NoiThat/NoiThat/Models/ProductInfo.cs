using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoiThat.Models
{
    public class ProductInfo
    {
        public int id { get; set; }
        public int CatId { get; set; }
        public string Name { get; set; }
        public string CatName { get; set; }
        public string SupName { get; set; }
        public string Slug { get; set; }
        public string Detail { get; set; }
        public string MetaKey { get; set; }
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
    }
}