using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using BLLTests.Mocks;
using BLL.Models;
using DTO;

namespace BLL.Tests
{
    [TestClass()]
    public class CampaignManagerTests
    {
        CampaignManager _campaignManager;

        CampaignDALMock _campaignDALMock;
        UserDALMock _userDALMock;
        POIDALMock _POIDALMock;
        LinkDALMock _linkDALMock;

        [TestInitialize()]
        public void Initialize()
        {
            _userDALMock = new UserDALMock(new UserDTO() { ID = 1, Username = "TestUser1" }, new UserDTO() { ID = 2, Username = "TestUser2" });
            _POIDALMock = new POIDALMock( new POIDTO() { ID = 1, Name = "TestPOI", Description = "Test", LinkedCampaignID = 1, RegionName = "TestRegion" });
            _linkDALMock = new LinkDALMock(_userDALMock, _campaignDALMock);

            CampaignDTO testCampaign = new CampaignDTO() { ID = 1, DM = _userDALMock.GetUser(1), Map = new Bitmap(1, 1), Name = "TestCampaign", Players = new List<UserDTO>() { _userDALMock.GetUser(2) }, POIs = null };
            _campaignDALMock = new CampaignDALMock(_POIDALMock, _linkDALMock, testCampaign);

            _campaignManager = new CampaignManager(_campaignDALMock, new UserManager(_userDALMock));
        }

        // CreateCampaign Tests
        [TestMethod()]
        public void CreateValidCampaignTest()
        {
            _campaignManager.CreateCampaign("Test", 2);

            CampaignDTO campaign = _campaignDALMock.GetCampaign(2);
            Assert.IsTrue(campaign != null &&
                        campaign.Name == "Test" &&
                        campaign.DM.ID == 2);
        }

        [TestMethod()]
        public void CreateCampaignWithInvalidDMIDTest()
        {
            _campaignManager.CreateCampaign("Test", -1);

            CampaignDTO campaign = _campaignDALMock.GetCampaign(2);
            Assert.IsTrue(campaign == null);
        }

        [TestMethod()]
        public void CreateCampaignWithNoNameTest()
        {
            _campaignManager.CreateCampaign("", 2);

            CampaignDTO campaign = _campaignDALMock.GetCampaign(2);
            Assert.IsTrue(campaign == null);
        }

        // DeleteCampaign Tests
        [TestMethod()]
        public void DeleteValidCampaignTest()
        {
            bool initialExists = _campaignDALMock.GetCampaign(1) != null;
            bool state = _campaignManager.DeleteCampaign(1);
            Assert.IsTrue(initialExists && state &&
                        _campaignDALMock.GetCampaign(1) == null);
        }

        [TestMethod()]
        public void DeleteNonExistantCampaignTest()
        {
            bool state = _campaignManager.DeleteCampaign(-1);
            Assert.IsTrue(!state);
        }


        // GetCampaign Tests
        [TestMethod()]
        public void GetValidCampaignTest()
        {
            Campaign campaign = _campaignManager.GetCampaign(1);
            Assert.IsTrue(campaign != null && campaign.Name == "TestCampaign");
        }

        [TestMethod()]
        public void GetNonExistantCampaignTest()
        {
            Campaign campaign = _campaignManager.GetCampaign(-1);
            Assert.IsTrue(campaign == null);
        }
    }
}