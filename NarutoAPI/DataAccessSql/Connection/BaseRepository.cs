using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataAccessSql.Connection
{
    public class BaseRepository
    {
        protected string _connectionString { get; }

        protected BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected async Task<int> ExecuteAsync(string query, object param = null, CommandType? commandType = null)
        {
            using IDbConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            return await conn.ExecuteAsync(query, param, commandType: commandType);
        }
        protected async Task<T> QueryFirstOrDefaultAsync<T>(string query, object param = null, CommandType? commandType = null)
        {
            using IDbConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            return await conn.QueryFirstOrDefaultAsync<T>(query, param, commandType: commandType);
        }
        protected async Task<IEnumerable<T>> QueryAsync<T>(string query, object param = null, CommandType? commandType = null)
        {
            using IDbConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            return await conn.QueryAsync<T>(query, param, commandType: commandType);
        }
    }
}
