using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace NoiThat.Models
{
    [Table("Topics")]
    public class Topic
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Slug { get; set; }
        public int? ParentId { get; set; }
        public int? Orders { get; set; }

        [Required]
        public string MetaKey { get; set; }

        [Required]
        public string MetaDes { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int Status { get; set; }

    }
}