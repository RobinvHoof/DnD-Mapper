using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

using AL;
using DTO;

namespace DAL
{
    public class UserDAL: BaseDAL, IUserCollection
    {
        public string GetPasswordHashByUsername(string username)
        {
            string query = "SELECT PasswordHash FROM dbo.Users WHERE Username=@Username";

            SqlParameter usernameParam = new SqlParameter("@Username", SqlDbType.VarChar, 50);
            usernameParam.Value = username;

            SqlCommand cmd = base.commandBuilder(query, usernameParam);
            DataTable userDataTable = base.runQuery(cmd);

            string passwordHash = null;
            if (userDataTable.Rows.Count > 0)
            {
                passwordHash = Convert.ToString(userDataTable.Rows[0]["PasswordHash"]);
            }

            return passwordHash;
        }


        public UserDTO GetUser(string username)
        {
            string query = "SELECT * FROM dbo.Users WHERE Username=@Username";

            SqlParameter usernameParam = new SqlParameter("@Username", SqlDbType.VarChar, 50);
            usernameParam.Value = username;

            SqlCommand cmd = base.commandBuilder(query, usernameParam);
            DataTable dataTable = base.runQuery(cmd);

            UserDTO user = null;
            if (dataTable.Rows.Count > 0)
            {
                user = new UserDTO
                {
                    ID = Convert.ToInt32(dataTable.Rows[0]["Id"]),
                    Username = Convert.ToString(dataTable.Rows[0]["Username"]),
                    PasswordHash = Convert.ToString(dataTable.Rows[0]["PasswordHash"])
                };
            }
            return user;
        }

        public UserDTO GetUser(int ID)
        {
            string query = "SELECT * FROM dbo.Users WHERE Id=@Id";

            SqlParameter idParam = new SqlParameter("@Id", SqlDbType.Int);
            idParam.Value = ID;

            SqlCommand cmd = base.commandBuilder(query, idParam);
            DataTable dataTable = base.runQuery(cmd);

            UserDTO user = null;
            if (dataTable.Rows.Count > 0)
            {
                user = new UserDTO
                {
                    ID = Convert.ToInt32(dataTable.Rows[0]["Id"]),
                    Username = Convert.ToString(dataTable.Rows[0]["Username"]),
                    PasswordHash = Convert.ToString(dataTable.Rows[0]["PasswordHash"])
                };
            }
            return user;
        }


        public bool AddUser(UserDTO user)
        {
            string query = "INSERT INTO dbo.Users VALUES (@Username, @PasswordHash)";

            SqlParameter usernameParam = new SqlParameter("@Username", SqlDbType.VarChar, 50);
            usernameParam.Value = user.Username;

            SqlParameter passwordHashParam = new SqlParameter("@PasswordHash", SqlDbType.Char, 64);
            passwordHashParam.Value = user.PasswordHash;

            SqlCommand cmd = base.commandBuilder(query, usernameParam, passwordHashParam);
            return (base.runNonQuery(cmd) == 1);
        }


        public bool DeleteUser(int userID)
        {
            string query = "DELETE FROM dbo.Users WHERE Id=@ID";

            SqlParameter idParam = new SqlParameter("@ID", SqlDbType.Int);
            idParam.Value = userID;

            SqlCommand cmd = base.commandBuilder(query, idParam);
            return (base.runNonQuery(cmd) == 1);
        }
    }
}
