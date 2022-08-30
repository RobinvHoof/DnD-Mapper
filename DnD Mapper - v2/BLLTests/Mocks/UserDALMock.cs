using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AL;
using DTO;

namespace BLLTests.Mocks
{
    public class UserDALMock : IUserCollection
    {
        public List<UserDTO> userDTOs;

        public UserDALMock(params UserDTO[] users)
        {
            userDTOs = new List<UserDTO>(users);
        }

        public string GetPasswordHashByUsername(string username)
        {
            UserDTO user = userDTOs.Find(x => x.Username == username);

            if (user == null)
            {
                return null;
            }
            return user.PasswordHash;
        }

        public UserDTO GetUser(string username)
        {
            return userDTOs.Find(x => x.Username == username);
        }

        public UserDTO GetUser(int ID)
        {
            return userDTOs.Find(x => x.ID == ID);
        }

        public bool AddUser(UserDTO user)
        {
            if (GetUser(user.Username) != null)
            {
                return false;
            }

            user.ID = userDTOs.Count + 1;
            userDTOs.Add(user);

            return true;
        }

        public bool DeleteUser(int ID)
        {
            return userDTOs.Remove(userDTOs.Find(x => x.ID == ID));
        }
    }
}
