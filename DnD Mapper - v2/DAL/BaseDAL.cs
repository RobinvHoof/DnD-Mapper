using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public abstract class BaseDAL
    {
        const string connectionString = "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = DnD_Mapper; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";

        protected DataTable runQuery(SqlCommand cmd)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) 
            {
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader dataReader = cmd.ExecuteReader();

                DataTable dataTable = new DataTable();
                dataTable.Load(dataReader);
                
                return dataTable;
            }
        }

        protected int runNonQuery(SqlCommand cmd)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        protected object runScalarQuery(SqlCommand cmd)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                return cmd.ExecuteScalar();
            }
        }

        protected SqlCommand commandBuilder(string baseQuery, params SqlParameter[] parameters)
        {
            SqlCommand sqlCommand = new SqlCommand(baseQuery);
            foreach (SqlParameter sqlParameter in parameters)
            {
                sqlCommand.Parameters.Add(sqlParameter);
            }

            return sqlCommand;
        }
    }
}
