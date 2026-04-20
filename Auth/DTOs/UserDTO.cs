using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auth.DTOs
{
    public class UserDTO
    {
        public long UserID { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string UserTypeCode { get; set; }

        public bool IsActive { get; set; }
    }
}