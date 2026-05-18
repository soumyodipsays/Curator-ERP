using Auth.DTOs;
using Auth.Models;
using Dapper;
using DataAccessLayer.Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Auth.DAL
{
    public class AuthDAL
    {
        private readonly DapperConn _db = new DapperConn();

        public long InsertUpdateUser(UserRegistrationDto userDto)
        {
            var parameters = new DynamicParameters();
            var proc = "spUser_InsertUpdate";
            // Map DTO to SP Parameters
            parameters.Add("@AdminUserName", userDto.AdminUserName);
            parameters.Add("@Email", userDto.Email);
            parameters.Add("@CustomerID", userDto.CustomerID);
            parameters.Add("@UserID", userDto.UserID ?? 0); // Handle 0 for new signup
            parameters.Add("@UserName", userDto.UserName);
            parameters.Add("@Password", userDto.Password);
            parameters.Add("@FirstName", userDto.FirstName);
            parameters.Add("@LastName", userDto.LastName);
            parameters.Add("@NewUserID", dbType: DbType.Int64, direction: ParameterDirection.Output);

            // Execute and return the new ID
            return _db.ExecuteWithOutput<long>(proc, parameters, "@NewUserID");
        }

        public UserModel ValidateUser(UserLoginDTO userDto)
        {
            var parameters = new DynamicParameters();
            var proc = "spValidate_User";
            // Map DTO to SP Parameters
            parameters.Add("@Email", userDto.Email);
            parameters.Add("@Password", userDto.Password);

            // Return the user record from the database
            return _db.ExecuteSingleRow<UserModel>(proc, parameters);
        }

        public void SaveRefreshToken(long UserID, string RefreshToken, DateTime RefreshTokenExpiry)
        {
            var parameters = new DynamicParameters();
            var proc = "spRefreshToken_Insert";

            parameters.Add("@UserID", UserID);
            parameters.Add("@RefreshToken", RefreshToken);
            parameters.Add("@RefreshTokenExpiry", RefreshTokenExpiry);

            _db.ExecuteWithoutReturn(proc, parameters);
        }

        public UserModel GetUserByRefreshToken(string RefreshToken) 
        {
            var parameters = new DynamicParameters();
            var proc = "spRefreshToken_GetUserByRefreshToken";
            parameters.Add("@RefreshToken", RefreshToken);

            return _db.ExecuteSingleRow<UserModel>(proc, parameters);
        }

        public void InsertUpdateOTP(OTP_DTO otpDto)
        {
            var parameters = new DynamicParameters();
            var proc = "spOTP_InsertUpdate";

            parameters.Add("@UserID", otpDto.UserID);
            parameters.Add("@Email", otpDto.Email);
            parameters.Add("@OTP", otpDto.OTP);

            _db.ExecuteWithoutReturn(proc, parameters);
        }

        public void ValidateOTP(OTP_DTO otpDto)
        {
            var parameters = new DynamicParameters();
            var proc = "spOTP_Verify";

            parameters.Add("@UserID", otpDto.UserID);
            parameters.Add("@Email", otpDto.Email);
            parameters.Add("@OTP", otpDto.OTP);

            _db.ExecuteWithoutReturn(proc, parameters);
        }

        public void ResetPassword(UserLoginDTO resetDto)
        {
            var parameters = new DynamicParameters();
            var proc = "spUser_ResetPassword";

            parameters.Add("@Email", resetDto.Email);
            parameters.Add("@OTP", resetDto.Password);

            _db.ExecuteWithoutReturn(proc, parameters);
        }

        public UserDTO GetUserById(long userId)
        {
            var parameters = new DynamicParameters();
            var proc = "spUser_GetById";

            parameters.Add("@UserID", userId);

            return _db.ExecuteSingleRow<UserDTO>(proc, parameters);
        }

        public UserProfileDTO GetUserProfileByID(long userId) 
        { 
            var parameters = new DynamicParameters();
            var proc = "sp_GetUserProfile";

            parameters.Add("@UserID", userId);
            return _db.ExecuteMultipleResultSet(proc, multi =>
            new UserProfileDTO
            {
                User = multi.Read<UserProfileDetailsDTO>().FirstOrDefault(),

                PhoneList = multi.Read<UserProfilePhoneDTO>().ToList(),
                AddressList = multi.Read<UserProfileAddressDTO>().ToList(),
            }, parameters);
        }

        public void UserProfileUpdate(UpdateUserDetailsDTO userDto)
        {
            var parameters = new DynamicParameters();
            var proc = "sp_UpdateUserDetails";
            // Map DTO to SP Parameters
            parameters.Add("@CustomerID", userDto.CustomerID);
            parameters.Add("@UserID", userDto.UserID); // Handle 0 for new signup
            parameters.Add("@UserName", userDto.UserName);
            parameters.Add("@FirstName", userDto.FirstName);
            parameters.Add("@LastName", userDto.LastName);
            parameters.Add("@PhoneNumber", userDto.PhoneNumber);
            parameters.Add("@Address1", userDto.Address1);
            parameters.Add("@Address2", userDto.Address2);

            // Execute and return the new ID
            _db.ExecuteWithoutReturn(proc, parameters);
        }

        public List<StateModel> GetAllStates()
        {
            var paramters = new DynamicParameters();
            var proc = "spState_DDL";

            return _db.ExecuteMultipleRow<StateModel>(proc, paramters);
        }

        public void AddOrEditUserAddress(NewAddressDTO newAddressDTO)
        {
            var parameters = new DynamicParameters();
            var proc = "sp_UpdateUserDetails";
            // Map DTO to SP Parameters
            parameters.Add("@AddressID", newAddressDTO.AddressID ); // Handle 0 for new signup
            parameters.Add("@UserID", newAddressDTO.UserID ); // Handle 0 for new signup
            parameters.Add("@Address1", newAddressDTO.Address1);
            parameters.Add("@Address2", newAddressDTO.Address2);
            parameters.Add("@City", newAddressDTO.City);
            parameters.Add("@StateID", newAddressDTO.StateID);
            parameters.Add("@PinCode", newAddressDTO.PinCode);
            parameters.Add("@isDefault", newAddressDTO.IsDefault);


            // Execute and return the new ID
            _db.ExecuteWithoutReturn(proc, parameters);
        }

        public void RemoveUserAddressByID(long UserID, long AddressID)
        {
            var parameters = new DynamicParameters();
            var proc = "sp_RemoveAddress";
            // Map DTO to SP Parameters
            parameters.Add("@AddressID", AddressID); 
            parameters.Add("@UserID", UserID);

            // Execute
            _db.ExecuteWithoutReturn(proc, parameters);
        }

        public List<AddressModel> GetAllAddressByUserID(long userID)
        {
            var parameters = new DynamicParameters();
            var proc = "sp_GetAddressesByUserID";

            parameters.Add("@UserID", userID);

            return _db.ExecuteMultipleRow<AddressModel>(proc, parameters);
        }
    }
}