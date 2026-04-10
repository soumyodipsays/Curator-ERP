using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Auth.DTOs
{
    public class UserLoginDTO
    {
        [DisplayName("UserName")]
        public string UserName { get; set; }

        [DisplayName("Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }
    }
}