using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AL;
using DTO;

namespace BLLTests.Mocks
{
    class LinkDALMock: ICampaignRelations, ICampaignRelationCollection, IUserCampaignCollection
    {
        private UserDALMock _userDALMock;
        private CampaignDALMock _campaignDALMock;

        Dictionary<int, List<int>> campaignPlayers;

        public LinkDALMock(UserDALMock userDALMock, CampaignDALMock campaignDALMock)
        {
            _userDALMock = userDALMock;
            _campaignDALMock = campaignDALMock;

            campaignPlayers = new Dictionary<int, List<int>>();
        }

        public List<CampaignDTO> GetUserCampaigns(int userID)
        {
            List<CampaignDTO> partakingCampaigns = new List<CampaignDTO>();

            foreach(KeyValuePair<int, List<int>> entry in campaignPlayers)
            {
                if(entry.Value.Exists(x => x == userID))
                {
                    partakingCampaigns.Add(_campaignDALMock.GetCampaign(entry.Key));
                }
            }
            return partakingCampaigns;
        }


        public List<UserDTO> GetCampaignPlayers(int campaignID)
        {
            if (!campaignPlayers.ContainsKey(campaignID))
            {
                return new List<UserDTO>();
            }

            List<int> playerIDs = campaignPlayers[campaignID];

            List<UserDTO> returnList = new List<UserDTO>();

            foreach(int id in playerIDs)
            {
                returnList.Add(_userDALMock.GetUser(id));
            }
            return returnList;
        }


        public bool AddCampaignPlayerLink(int campaignID, int userID)
        {
            if (!campaignPlayers.ContainsKey(campaignID))
            {
                campaignPlayers.Add(campaignID, new List<int>());
            }
           
            campaignPlayers[campaignID].Add(userID);
            return true;
        }

        public bool RemoveCampaignPlayerLink(int campaignID, int userID)
        {
            if (!campaignPlayers.ContainsKey(campaignID))
            {
                return false;
            }

            return campaignPlayers[campaignID].Remove(userID);
        }
    }
}
