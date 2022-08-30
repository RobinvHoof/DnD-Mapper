using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BLLTests.Mocks;
using DTO;
using BLL.Models;

namespace BLL.Tests
{
    [TestClass()]
    public class UserManagerTests
    {
        private UserManager _userManager;

        private UserDALMock _userDALMock;

        [TestInitialize()]
        public void Initialize()
        {
            UserDTO testUser = new UserDTO() { ID = 1, Username = "TestUser", PasswordHash = Hasher.CreateHash("TestPassword") };

            _userDALMock = new UserDALMock(testUser);
            _userManager = new UserManager(_userDALMock);
        }


        // RegisterUser Tests
        [TestMethod()]
        public void RegisterCorrectUserTest()
        {
            UserManager.RegisterState registerState = _userManager.RegisterUser("Test", "Test");

            UserDTO user = _userDALMock.GetUser("Test");
            Assert.IsTrue(registerState == UserManager.RegisterState.Success &&
                        user != null &&
                        user.Username == "Test" &&
                        user.PasswordHash == Hasher.CreateHash("Test"));
        }

        [TestMethod()]
        public void RegisterUserWithInvalidUsernameTest()
        {
            UserManager.RegisterState registerState = _userManager.RegisterUser("", "Test");

            UserDTO user = _userDALMock.GetUser("") ?? _userDALMock.GetUser(2);
            Assert.IsTrue(registerState == UserManager.RegisterState.InvalidInput &&
                        user == null);
        }

        [TestMethod()]
        public void RegisterUserWithInvalidPasswordTest()
        {
            UserManager.RegisterState registerState = _userManager.RegisterUser("Test", "");

            UserDTO user = _userDALMock.GetUser("Test") ?? _userDALMock.GetUser(2);
            Assert.IsTrue(registerState == UserManager.RegisterState.InvalidInput &&
                        user == null);
        }

        [TestMethod()]
        public void RegisterUserWithDuplicateUsernameTest()
        {
            _userManager.RegisterUser("Test", "Test");
            UserManager.RegisterState registerState = _userManager.RegisterUser("Test", "Test");

            Assert.IsTrue(registerState == UserManager.RegisterState.UsernameInUse);
        }

        // AttemptLogin Tests
        [TestMethod()]
        public void AttemptCorrectLoginTest()
        {
            User user = _userManager.AttemptLogin("TestUser", "TestPassword");
            Assert.IsTrue(user != null &&
                user.Username == "TestUser");
        }

        [TestMethod()]
        public void AttemptLoginWithIncorrectPasswordTest()
        {
            User user = _userManager.AttemptLogin("TestUser", "Wrong");
            Assert.IsTrue(user == null);
        }

        [TestMethod()]
        public void AttemptLoginWithNonExistantUsernameTest()
        {
            User user = _userManager.AttemptLogin("Wrong", "TestPassword");
            Assert.IsTrue(user == null);
        }

        // GetUser Tests
        [TestMethod()]
        public void GetValidUserByIDTest()
        {
            User user = _userManager.GetUser(1);
            Assert.IsTrue(user != null &&
                        user.Username == "TestUser");
        }

        [TestMethod()]
        public void GetValidUserByUsernameTest()
        {
            User user = _userManager.GetUser("TestUser");
            Assert.IsTrue(user != null &&
                        user.ID == 1);
        }

        [TestMethod()]
        public void GetInvalidUserByIDTest()
        {
            User user = _userManager.GetUser(-1);
            Assert.IsTrue(user == null);
        }

        [TestMethod()]
        public void GetInvalidUserByUsernameTest()
        {
            User user = _userManager.GetUser("Wrong");
            Assert.IsTrue(user == null);
        }
    }
}
