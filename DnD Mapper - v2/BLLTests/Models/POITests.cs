using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using DTO;
using BLLTests.Mocks;

namespace BLL.Models.Tests
{
    [TestClass()]
    public class POITests
    {
        UserDALMock _userDALMock;
        CampaignDALMock _campaignDALMock;
        POIDALMock _POIDALMock;
        LinkDALMock _linkDALMock;

        POI testPOI;

        [TestInitialize()]
        public void Initialize()
        {
            _userDALMock = new UserDALMock(new UserDTO() { ID = 1, Username = "TestUser1" }, new UserDTO() { ID = 2, Username = "TestUser2" });
            _POIDALMock = new POIDALMock();
            _linkDALMock = new LinkDALMock(_userDALMock, _campaignDALMock);

            _campaignDALMock = new CampaignDALMock(_POIDALMock, _linkDALMock);

            CampaignDTO testCampaignDTO = new CampaignDTO() { ID = 1, DM = new UserDTO() { ID = 1, Username = "TestUser1" }, Map = new Bitmap(1, 1), Name = "TestCampaign", Players = new List<UserDTO>() { new UserDTO() { ID = 2, Username = "TestUser2" } }, POIs = null };
            _campaignDALMock.AddCampaign(testCampaignDTO);

            testPOI = new POI(_POIDALMock) { ID = 1, Name = "TestPOI", Description = "Test", LinkedCampaignID = 1, RegionName = "TestRegion" };
            _POIDALMock.AddPOI(new POIDTO() { ID = 1, Name = "TestPOI", Description = "Test", LinkedCampaignID = 1, RegionName = "TestRegion" });

        }

        // Update Tests
        [TestMethod()]
        public void UpdateDescriptionTest()
        {
            testPOI.Description = "Changed";
            testPOI.Update();
            Assert.IsTrue(_POIDALMock.GetPOI(1).Description == "Changed");
        }

        [TestMethod()]
        public void UpdateNameTest()
        {
            testPOI.Name = "Changed";
            testPOI.Update();
            Assert.IsTrue(_POIDALMock.GetPOI(1).Name == "Changed");
        }

        [TestMethod()]
        public void UpdateRegionTest()
        {
            testPOI.RegionName = "Changed";
            testPOI.Update();
            Assert.IsTrue(_POIDALMock.GetPOI(1).RegionName == "Changed");
        }

        [TestMethod()]
        public void UpdateNoneTest()
        {
            testPOI.Update();
            Assert.IsTrue(_POIDALMock.GetPOI(1).Description == "Test");
        }
    }
}