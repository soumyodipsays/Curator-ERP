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
        [Required(ErrorMessage = "Password is required.")]
        [DisplayName("Password")]
        [StringLength(15, MinimumLength = 6,
        ErrorMessage = "Password must be between 6 and 15 characters.")]
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string accessToken { get; set; }
        public object user { get; set; }
    }
}