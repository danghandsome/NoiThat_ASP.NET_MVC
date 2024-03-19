//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Web;

//namespace NoiThat.Models
//{
//    [Table("Contacts")]
//    public class Contacts
//    {
//        [Key]
//        public int Id { get; set; }

//        [Required(ErrorMessage = "Họ tên là bắt buộc.")]
//        public string Name { get; set; }

//        [Required(ErrorMessage = "Email là bắt buộc.")]
//        [EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng email.")]
//        public string Email { get; set; }

//        [Required(ErrorMessage = "Tin nhắn là bắt buộc.")]
//        public string Message { get; set; }
//    }
//}