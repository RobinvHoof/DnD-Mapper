using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Tests
{
    [TestClass()]
    public class HasherTests
    {
        string testHash = Hasher.CreateHash("Test");

        [TestInitialize()]
        public void Initialize()
        {

        }

        // CreateHash Tests
        [TestMethod()]
        public void CreateHashValidDuplicityTest()
        {
            Assert.IsTrue(Hasher.CreateHash("Test") == testHash);
        }

        [TestMethod()]
        public void CreateHashInalidDuplicityTest()
        {
            Assert.IsTrue(Hasher.CreateHash("Test2") != testHash);
        }

        // CompareHashToString Tests
        [TestMethod()]
        public void CompareValidStringToHashTest()
        {
            Assert.IsTrue(Hasher.CompareStringToHash("Test", testHash));
        }

        [TestMethod()]
        public void CompareInvalidStringToHashTest()
        {
            Assert.IsTrue(!Hasher.CompareStringToHash("Test2", testHash));
        }
    }
}