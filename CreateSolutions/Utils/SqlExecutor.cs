using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;

namespace CreateSolutions.Utils
{
    internal static class SqlExecutor
    {
        internal static IEnumerable<T> Execute<T>(string connectionString, string query, object param)
        {
            IEnumerable<T> returnValue = null;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                returnValue = connection.Query<T>(query, param);
            }

            return returnValue;
        }

        internal static int ExecuteScalar(string connectionString, string query, object param)
        {
            var result = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                result = connection.ExecuteScalar<int>(query, param);
            }

            return result;
        }
    }
}
