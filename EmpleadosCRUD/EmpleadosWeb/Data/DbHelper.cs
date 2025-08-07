
using System.Data;
using Microsoft.Data.SqlClient;

namespace EmpleadosWeb.Data
{
    public class DbHelper
    {
        private readonly string _connectionString;

        public DbHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand(query, conn);
            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            using var adapter = new SqlDataAdapter(cmd);
            var table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand(query, conn);
            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        public object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand(query, conn);
            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            conn.Open();
            return cmd.ExecuteScalar();
        }
    }
}
