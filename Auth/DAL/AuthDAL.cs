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
            var p = new DynamicParameters();

            // Map DTO to SP Parameters
            p.Add("@AdminUserName", userDto.AdminUserName);
            p.Add("@CustomerID", userDto.CustomerID);
            p.Add("@UserID", userDto.UserID ?? 0); // Handle 0 for new signup
            p.Add("@UserName", userDto.UserName);
            p.Add("@Password", userDto.Password);
            p.Add("@FirstName", userDto.FirstName);
            p.Add("@LastName", userDto.LastName);

            p.Add("@NewUserID", dbType: DbType.Int64, direction: ParameterDirection.Output);

            // Execute and return the new ID
            return _db.ExecuteWithOutput<long>("spUser_InsertUpdate", p, "@NewUserID");
        }
    }
}