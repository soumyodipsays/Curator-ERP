using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DataAccessLayer.Dapper
{
    public class DapperConn
    {
        // RETURN NO VALUE.
        public void ExecuteWithoutReturn(string procName, DynamicParameters param = null)
        {
            using (var conn = DbConnectionFactory.Create())
            {
                conn.Execute(procName, param, commandType: CommandType.StoredProcedure);
            }
        }

        public T ExecuteWithOutput<T>(string procName, DynamicParameters param, string outputParamName)
        {
            using (var conn = DbConnectionFactory.Create())
            {
                conn.Execute(procName, param, commandType: CommandType.StoredProcedure);
                return param.Get<T>(outputParamName);
            }
        }

        public T ExecuteSingleRow<T>(string procName, DynamicParameters param = null)
        {
            using (var conn = DbConnectionFactory.Create())
            {
                return conn.QueryFirstOrDefault<T>(procName, param, commandType: CommandType.StoredProcedure);
            }
        }

        public List<T> ExecuteMultipleRow<T>(string procName, DynamicParameters param = null)
        {
            using (var conn = DbConnectionFactory.Create())
            {
                return conn.Query<T>(procName, param, commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}