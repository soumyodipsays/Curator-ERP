using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auth.DTOs
{
    public class UserRegistrationDto
    {
        // Hidden/Session Fields
        public string AdminUserName { get; set; }
        public long CustomerID { get; set; }

        public long? UserID { get; set; } // Nullable for new registrations
        public string UserName { get; set; }
        public string Password { get; set; }
       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? NewUserID { get; set; }

    }
}