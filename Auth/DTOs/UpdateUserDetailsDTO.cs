using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auth.DTOs
{
    public class UpdateUserDetailsDTO
    {
        public long CustomerID { get; set; }
        public long UserID { get; set; } 
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
    }
}