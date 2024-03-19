using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NoiThat.Models
{
    [Table("Links")]
    public class Link
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Slug { get; set; }

        [Required]
        public string TypeLink { get; set; }

        public int TableId { get; set; }
    }
}