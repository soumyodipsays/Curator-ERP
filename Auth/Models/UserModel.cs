using System;

namespace Auth.Models
{
    public class UserModel
    {
        public long UserID { get; set; }

        public string UserName { get; set; }
        public string UserMobile { get; set; }
        public byte[] Password { get; set; }

        public long PersonID { get; set; }
        public long? ParentsID { get; set; }
        public char? ParentsGender { get; set; }

        public string UserTypeCode { get; set; }
        public string Designation { get; set; }

        public short? NumberOfRowsPerPage { get; set; }

        public bool CanAccessFinancialInformation { get; set; }
        public bool CanAccessAdmin { get; set; }

        public DateTime? PasswordResetOn { get; set; }

        public bool IsActive { get; set; }

        public string FcmID { get; set; }
        public string DeviceID { get; set; }

        public bool IsDeviceIDUpdated { get; set; }

        public string ProfileImage { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
    }

    public class AddressModel
    {
        public long AddressID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State {  get; set; }
        public string PinCode { get; set; }
        public bool IsDefault { get; set; }
    }
}