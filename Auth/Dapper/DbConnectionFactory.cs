using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Auth.Dapper
{
    public class DbConnectionFactory
    {
        public static SqlConnection Create()
        {
            var connStr = ConfigurationManager.ConnectionStrings["DBConnection"]?.ConnectionString;

            if (string.IsNullOrEmpty(connStr))
                throw new Exception("Database connection string 'DBConnection' not found in config.");

            return new SqlConnection(connStr);
        }
    }
}