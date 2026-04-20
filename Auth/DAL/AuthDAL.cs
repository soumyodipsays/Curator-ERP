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
    }
}