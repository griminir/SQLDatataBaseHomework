using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient; // important NuGet
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Dapper; // important NuGet

namespace DataAccessLibrary
{
    public class SQLDataAccess
    {
        public List<T> LoadData<T, U>(string sqlStatement, U Parameters, string connectionString)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sqlStatement, Parameters).ToList();
                return rows;
            }
        }

        public void SaveData<T>(string sqlStatement, T parameters, string connectionString)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(sqlStatement, parameters);
            }
        }
    }
}
