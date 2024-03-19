using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;


namespace NoiThat.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string FullName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Img { get; set; }
        public int? CountError { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public int? Gender { get; set; }
        public string Roles { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int Status { get; set; }
        public int Score { get; set; }


    }
}