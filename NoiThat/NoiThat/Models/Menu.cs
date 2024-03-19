using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiThat.Models
{
    [Table("Menus")]
    public class Menu
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Link { get; set; }
        public string TypeMenu { get; set; }
        public string Position { get; set; }
        public int? TableId { get; set; }
        public int? ParentId { get; set; }
        public int? Orders { get; set; }
        public int Status { get; set; }

    }
}