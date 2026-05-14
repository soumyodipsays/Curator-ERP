using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auth.DTOs
{
    public class UserProfileAddressDTO
    {
        public long AddressID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public long StateID { get; set; }
        public string State {  get; set; }
        public string PinCode { get; set; }
        public bool IsDefault { get; set; }
    }

    public class UserProfilePhoneDTO
    {
        public string Mobile {  get; set; }
    }
    public class UserProfileDetailsDTO
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
        public int OrderCount { get; set; }
        public int CartProductCount { get; set; }
    }
    public class UserProfileDTO
    {
        public UserProfileDetailsDTO User { get; set; }
        public List<UserProfilePhoneDTO> PhoneList { get; set; }
        public List<UserProfileAddressDTO> AddressList { get; set; }
        
    }

    public class NewAddressDTO
    {
        public long UserID { get; set; }
        public long AddressID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public long StateID { get; set; }
        public string PinCode { get; set; }
        public bool IsDefault { get; set; }
    }
}