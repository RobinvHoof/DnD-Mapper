using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using DTO;
using AL;

using BLLTests.Mocks;

namespace BLL.Models.Tests
{
    [TestClass()]
    public class CampaignTests
    {
        UserDALMock _userDALMock;
        CampaignDALMock _campaignDALMock;
        POIDALMock _POIDALMock;
        LinkDALMock _linkDALMock;

        Campaign testCampaign;

        [TestInitialize()]
        public void Initialize()
        {
            _userDALMock = new UserDALMock(new UserDTO() { ID = 1, Username = "TestUser1" }, new UserDTO() { ID = 2, Username = "TestUser2" });
            _POIDALMock = new POIDALMock();
            _linkDALMock = new LinkDALMock(_userDALMock, _campaignDALMock);

            _campaignDALMock = new CampaignDALMock(_POIDALMock, _linkDALMock);
            testCampaign = new Campaign(_linkDALMock, _campaignDALMock, _POIDALMock, _userDALMock) { ID = 1, DM = new User(null, _linkDALMock, _campaignDALMock) { ID = 1, Username = "TestUser1" }, Map = new Bitmap(1, 1), Name = "TestCampaign", Players = new List<User>(), POIs = new List<POI>() { new POI() { ID = 1, Name = "TestPOI", Description = "Test", LinkedCampaignID = 1, RegionName = "TestRegion" } } };

            CampaignDTO testCampaignDTO = new CampaignDTO() { ID = 1, DM = new UserDTO() { ID = 1, Username = "TestUser1" }, Map = new Bitmap(1, 1), Name = "TestCampaign", Players = new List<UserDTO>() { new UserDTO() { ID = 2, Username = "TestUser2" } }, POIs = null };
            _campaignDALMock.AddCampaign(testCampaignDTO);

            _POIDALMock.AddPOI(new POIDTO() { ID = 1, Name = "TestPOI", Description = "Test", LinkedCampaignID = 1, RegionName = "TestRegion" });
        }

        // Update Tests
        [TestMethod()]
        public void UpdateCampaignMapTest()
        {
            testCampaign.Map = new Bitmap(2, 2);
            testCampaign.Update();
            Assert.IsTrue(_campaignDALMock.GetCampaign(1).Map.Width == 2);
        }

        [TestMethod()]
        public void UpdateCampaignNameTest()
        {
            testCampaign.Name = "Changed";
            testCampaign.Update();
            Assert.IsTrue(_campaignDALMock.GetCampaign(1).Name == "Changed");
        }

        [TestMethod()]
        public void UpdateNoneTest()
        {
            testCampaign.Update();
            Assert.IsTrue(_campaignDALMock.GetCampaign(1).Name == "TestCampaign" &&
                        _campaignDALMock.GetCampaign(1).Map.Width == 1);
        }


        // AddPOI Tests
        [TestMethod()]
        public void AddValidPOITest()
        {
            bool state = testCampaign.AddPOI(new POI() { ID = 2, Description = "Test2", LinkedCampaignID = 1, Name = "TestPOI2", RegionName = "TestRegion" });
            POIDTO POIDTO = _POIDALMock.GetPOI(2);

            Assert.IsTrue(state &&
                        POIDTO != null &&
                        POIDTO.ID == 2 &&
                        POIDTO.Name == "TestPOI2");
        }

        [TestMethod()]
        public void AddPOIWithDuplicateNameTest()
        {
            bool state = testCampaign.AddPOI(new POI() { ID = 2, Description = "Test", LinkedCampaignID = 1, Name = "TestPOI", RegionName = "TestRegion" });
            POIDTO POIDTO = _POIDALMock.GetPOI(2);

            Assert.IsTrue(!state);
        }

        // DeletePOI Tests
        [TestMethod()]
        public void DeleteValidPOITest()
        {
            bool state = testCampaign.DeletePOI(1);
            Assert.IsTrue(state &&
                        _POIDALMock.GetPOI(1) == null &&
                        _POIDALMock.POIDTOs.Count == 0);
        }

        [TestMethod()]
        public void DeletePOIWithInvalidIDTest()
        {
            bool state = testCampaign.DeletePOI(-1);
            Assert.IsTrue(!state &&
                        _POIDALMock.GetPOI(1) != null &&
                        _POIDALMock.POIDTOs.Count == 1);
        }


        // AddPlayer Tests
        [TestMethod()]
        public void AddValidPlayerTest()
        {
            bool state = testCampaign.AddPlayer(new User(null, _linkDALMock, _campaignDALMock) { ID = 2, Username = "TestUser2" });
            Assert.IsTrue(state &&
                        _linkDALMock.GetCampaignPlayers(1).Exists(x => x.ID == 2));
        }

        [TestMethod()]
        public void AddNonExistantPlayerTest()
        {
            bool state = testCampaign.AddPlayer(new User(null, _linkDALMock, _campaignDALMock) { ID = -1, Username = "TestUser0" });
            Assert.IsTrue(!state &&
                        !_linkDALMock.GetCampaignPlayers(1).Exists(x => x.ID == -1));
        }

        [TestMethod()]       
        public void AddDMPlayerTest()
        {
            bool state = testCampaign.AddPlayer(new User(null, _linkDALMock, _campaignDALMock) { ID = 1, Username = "TestUser1" });
            Assert.IsTrue(!state &&
                        !_linkDALMock.GetCampaignPlayers(1).Exists(x => x.ID == 1));
        }

        [TestMethod()]
        public void AddDuplicatePlayerTest()
        {
            testCampaign.AddPlayer(new User(null, _linkDALMock, _campaignDALMock) { ID = 2, Username = "TestUser2" });
            Campaign campaing = new Campaign(_linkDALMock, _campaignDALMock, _POIDALMock, _userDALMock, _campaignDALMock.GetCampaign(1));

            bool state = campaing.AddPlayer(new User(null, _linkDALMock, _campaignDALMock) { ID = 2, Username = "TestUser2" });

            Assert.IsTrue(!state &&
                        _linkDALMock.GetCampaignPlayers(1).Exists(x => x.ID == 2) &&
                        _linkDALMock.GetCampaignPlayers(1).Count == 1);
        }

        
        // GetPOI Tests
        [TestMethod()]
        public void GetSinglePOITest()
        {
            CampaignDTO campaignDTO = new CampaignDTO() { ID = 2, DM = new UserDTO() { ID = 1, Username = "TestUser1" }, Map = new Bitmap(1, 1), Name = "POITestCampaign", Players = null, POIs = null };
            _campaignDALMock.AddCampaign(campaignDTO);

            Campaign campaign = new Campaign(_linkDALMock, _campaignDALMock, _POIDALMock, _userDALMock, _campaignDALMock.GetCampaign(2));
            campaign.AddPOI(new POI(_POIDALMock) { ID = 11, Description = "Test1", LinkedCampaignID = 2, Name = "Test1", RegionName = "Test" });

            campaign = new Campaign(_linkDALMock, _campaignDALMock, _POIDALMock, _userDALMock, _campaignDALMock.GetCampaign(2));
            Assert.IsTrue(campaign.POIs.Count == 1);
        }

        [TestMethod()]
        public void GetTwoPOIsTest()
        {
            CampaignDTO campaignDTO = new CampaignDTO() { ID = 2, DM = new UserDTO() { ID = 1, Username = "TestUser1" }, Map = new Bitmap(1, 1), Name = "POITestCampaign", Players = null, POIs = null };
            _campaignDALMock.AddCampaign(campaignDTO);

            Campaign campaign = new Campaign(_linkDALMock, _campaignDALMock, _POIDALMock, _userDALMock, _campaignDALMock.GetCampaign(2));
            campaign.AddPOI(new POI(_POIDALMock) { ID = 11, Description = "Test1", LinkedCampaignID = 2, Name = "Test1", RegionName = "Test" });
            campaign.AddPOI(new POI(_POIDALMock) { ID = 12, Description = "Test2", LinkedCampaignID = 2, Name = "Test2", RegionName = "Test" });

            campaign = new Campaign(_linkDALMock, _campaignDALMock, _POIDALMock, _userDALMock, _campaignDALMock.GetCampaign(2));
            Assert.IsTrue(campaign.POIs.Count == 2);
        }

        [TestMethod()]
        public void GetNoPOIsTest()
        {
            CampaignDTO campaignDTO = new CampaignDTO() { ID = 2, DM = new UserDTO() { ID = 1, Username = "TestUser1" }, Map = new Bitmap(1, 1), Name = "POITestCampaign", Players = null, POIs = null };
            _campaignDALMock.AddCampaign(campaignDTO);

            Campaign campaign = new Campaign(_linkDALMock, _campaignDALMock, _POIDALMock, _userDALMock, _campaignDALMock.GetCampaign(2));

            Assert.IsTrue(campaign.POIs.Count == 0);
        }
    }
}