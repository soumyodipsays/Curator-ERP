using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auth.DTOs
{
    public class UserProfileDTO
    {
        public long UserID { get; set; }

        public bool IsActive { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
        public string Avatar_url { get; set; }

        public DateTime CreatedOn { get; set; }

        public string FirstName { get; set; }

        public string MiddleInitials { get; set; }

        public string LastName { get; set; }

        public string Mobile { get; set; }

        public int OrderCount { get; set; }

        public int CartProductCount { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public int? StateID { get; set; }

        public string PinCode { get; set; }
    }
}