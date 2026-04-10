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
        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }
        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }
    }
}