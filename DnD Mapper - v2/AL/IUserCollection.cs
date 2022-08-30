using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace AL
{
    public interface IUserCollection
    {
        string GetPasswordHashByUsername(string username);
        UserDTO GetUser(string username);
        UserDTO GetUser(int ID);
        bool AddUser(UserDTO user);
        bool DeleteUser(int ID);
    }
}
