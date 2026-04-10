using Auth.DTOs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DataAccessLayer.Dapper;

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

        public void ValidateUser(UserLoginDTO userDto)
        {
            var parameters = new DynamicParameters();
            var proc = "spValidate_User";
            // Map DTO to SP Parameters
            parameters.Add("@Email", userDto.Email);
            parameters.Add("@Password", userDto.Password);

            _db.ExecuteWithoutReturn(proc, parameters);
        }
    }
}