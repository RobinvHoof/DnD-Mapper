using System;
using System.Collections.Generic;

using AL;
using DTO;
using BLL.Models;

namespace BLL
{
    public class UserManager
    {
        public enum RegisterState
        {
            Success,
            UsernameInUse,
            InvalidInput,
            SQLError
        }

        private readonly IUserCollection _userCollection;
        

        public UserManager()
        {
            _userCollection = FL.IUserCollectionFactory.GetIUserCollection();
        }

        public UserManager(IUserCollection userCollection)
        {
            _userCollection = userCollection;
        }

        /// <summary>
        /// Attempts to log in the site User with provided Username and Password
        /// </summary>
        /// <param name="username">Username of the User account that is being attempted to access</param>
        /// <param name="password">Password of the corresponding user account</param>
        /// <returns>Return an object of the User account if the login succeeds, otherwise return null</returns>
        public User AttemptLogin(string username, string password)
        {
            string registeredPasswordHash = _userCollection.GetPasswordHashByUsername(username);
            bool loginResult = Hasher.CompareStringToHash(password, registeredPasswordHash);

            if (loginResult)
            {                
                return GetUser(username);
            }
            return null;
        }

        /// <summary>
        /// Register a new User
        /// </summary>
        /// <param name="username">Username that should be given to the User account</param>
        /// <param name="password">Password for the User account</param>
        /// <returns>Result of the registration action</returns>
        public RegisterState RegisterUser(string username, string password)
        {
            if (username == null || password == null || username.Length < 3 || password.Length < 3)
            {
                return RegisterState.InvalidInput;
            }

            if (_userCollection.GetUser(username) != null)
            {
                return RegisterState.UsernameInUse;
            }

            string passwordHash = Hasher.CreateHash(password);

            if (_userCollection.AddUser(new UserDTO { Username = username, PasswordHash = passwordHash }))
            {
                return RegisterState.Success;
            } else
            {
                return RegisterState.SQLError;
            }
        }

        /// <summary>
        /// Get User object by corresponding User ID
        /// </summary>
        /// <param name="ID">ID of User account</param>
        /// <returns>Object of User account that is linked to the ID</returns>
        public User GetUser(int ID)
        {
            UserDTO userDTO = _userCollection.GetUser(ID);

            if (userDTO == null)
            {
                return null;
            }

            return new User
            { 
                ID = userDTO.ID,
                Username = userDTO.Username,                
            };
        }

        /// <summary>
        /// Get User object by corresponding Username
        /// </summary>
        /// <param name="username">Username of User account</param>
        /// <returns>Object of User account with given Username</returns>
        public User GetUser(string username)
        {
            UserDTO userDTO = _userCollection.GetUser(username);

            if (userDTO == null)
            {
                return null;
            }

            return new User
            {
                ID = userDTO.ID,
                Username = userDTO.Username,
            };
        }
    }
}
