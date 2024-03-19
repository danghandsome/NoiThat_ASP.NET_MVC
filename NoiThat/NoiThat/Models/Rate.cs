using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NoiThat.Models
{
    [Table("Rates")]

    public class Rate
    {
        [Key]
        public int id { get; set; }
        public int productID { get; set; }
        public int userID { get; set; }
        public int rate { get; set; }
    }
}