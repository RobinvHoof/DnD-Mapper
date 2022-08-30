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
    public class UserTests
    {
        CampaignManager _campaignManager;

        UserDALMock _userDALMock;
        CampaignDALMock _campaignDALMock;
        POIDALMock _POIDALMock;
        LinkDALMock _linkDALMock;

        Campaign testCampaign1;
        Campaign testCampaign2;


        [TestInitialize()]
        public void Initialize()
        {
            _userDALMock = new UserDALMock(new UserDTO() { ID = 1, Username = "TestUser1" }, new UserDTO() { ID = 2, Username = "TestUser2" }, new UserDTO() { ID = 3, Username = "TestDM"});
            _POIDALMock = new POIDALMock();
            _linkDALMock = new LinkDALMock(_userDALMock, _campaignDALMock);

            _campaignDALMock = new CampaignDALMock(_POIDALMock, _linkDALMock);

            CampaignDTO testCampaignDTO1 = new CampaignDTO() { ID = 1, DM = new UserDTO() { ID = 1, Username = "TestUser1" }, Map = new Bitmap(1, 1), Name = "TestCampaign1", Players = null, POIs = null };
            CampaignDTO testCampaignDTO2 = new CampaignDTO() { ID = 1, DM = new UserDTO() { ID = 1, Username = "TestUser1" }, Map = new Bitmap(1, 1), Name = "TestCampaign2", Players = null, POIs = null };
            _campaignDALMock.AddCampaign(testCampaignDTO1);
            _campaignDALMock.AddCampaign(testCampaignDTO2);

            _POIDALMock.AddPOI(new POIDTO() { ID = 1, Name = "TestPOI", Description = "Test", LinkedCampaignID = 1, RegionName = "TestRegion" });

            testCampaign1 = new Campaign(_linkDALMock, _campaignDALMock, _POIDALMock, _userDALMock, _campaignDALMock.GetCampaign(1));
            testCampaign2 = new Campaign(_linkDALMock, _campaignDALMock, _POIDALMock, _userDALMock, _campaignDALMock.GetCampaign(2));

            _campaignManager = new CampaignManager(_campaignDALMock, new UserManager(_userDALMock));
        }

        // GetCampaigns Test
        [TestMethod()]
        public void GetSingleCampaignTest()
        {
            testCampaign1.AddPlayer(new User(null, _linkDALMock, _campaignDALMock) { ID = 2, Username = "TestUser2" });

            User user = new User(_campaignManager, _linkDALMock, _campaignDALMock) { ID = 2, Username = _userDALMock.GetUser(2).Username };
            List<Campaign> campaigns = user.GetCampaigns();
            Assert.IsTrue(campaigns.Count == 1);
        }

        [TestMethod()]
        public void GetTwoCampaignsTest()
        {
            testCampaign1.AddPlayer(new User(null, _linkDALMock, _campaignDALMock) { ID = 2, Username = "TestUser2" });
            testCampaign2.AddPlayer(new User(null, _linkDALMock, _campaignDALMock) { ID = 2, Username = "TestUser2" });

            User user = new User(_campaignManager, _linkDALMock, _campaignDALMock) { ID = 2, Username = _userDALMock.GetUser(2).Username };
            List<Campaign> campaigns = user.GetCampaigns();
            Assert.IsTrue(campaigns.Count == 2);
        }

        [TestMethod()]
        public void GetNoCampaignsTest()
        {
            User user = new User(null, _linkDALMock, _campaignDALMock) { ID = 2, Username = _userDALMock.GetUser(2).Username };
            List<Campaign> campaigns = user.GetCampaigns();
            Assert.IsTrue(campaigns.Count == 0);
        }


        // GetDMCampaigns Tests
        [TestMethod()]
        public void GetSignleDMCampaignsTest()
        {
            CampaignDTO testCampaignDTO1 = new CampaignDTO() { ID = 3, DM = new UserDTO() { ID = 3, Username = "TestDM" }, Map = new Bitmap(1, 1), Name = "TestCampaign3", Players = null, POIs = null };
            _campaignDALMock.AddCampaign(testCampaignDTO1);

            User user = new User(_campaignManager, _linkDALMock, _campaignDALMock) { ID = 3, Username = _userDALMock.GetUser(3).Username };
            List<Campaign> campaigns = user.GetDMCampaigns();
            Assert.IsTrue(campaigns.Count == 1);
        }

        [TestMethod()]
        public void GetTwoDMCampaignsTest()
        {
            CampaignDTO testCampaignDTO1 = new CampaignDTO() { ID = 3, DM = new UserDTO() { ID = 3, Username = "TestDM" }, Map = new Bitmap(1, 1), Name = "TestCampaign3", Players = null, POIs = null };
            _campaignDALMock.AddCampaign(testCampaignDTO1);

            CampaignDTO testCampaignDTO2 = new CampaignDTO() { ID = 4, DM = new UserDTO() { ID = 3, Username = "TestDM" }, Map = new Bitmap(1, 1), Name = "TestCampaign4", Players = null, POIs = null };
            _campaignDALMock.AddCampaign(testCampaignDTO2);

            User user = new User(_campaignManager, _linkDALMock, _campaignDALMock) { ID = 3, Username = _userDALMock.GetUser(3).Username };
            List<Campaign> campaigns = user.GetDMCampaigns();
            Assert.IsTrue(campaigns.Count == 2);
        }

        [TestMethod()]
        public void GetNoDMCampaignsTest()
        { 
            User user = new User(_campaignManager, _linkDALMock, _campaignDALMock) { ID = 3, Username = _userDALMock.GetUser(3).Username };
            List<Campaign> campaigns = user.GetDMCampaigns();
            Assert.IsTrue(campaigns.Count == 0);
        }
    }
}