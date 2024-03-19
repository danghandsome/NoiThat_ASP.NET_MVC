using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiThat.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int id { get; set; }
        public int UserId { get; set; }
        [Required]
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ReceiverAddress { get; set; }
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Status { get; set; }
        public int Amount { get; set; }

        public DateTime CreatedDate { get; set; }
        public int Discount { get; set; }

        

    }
}