using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DTO;
using AL;

namespace BLLTests.Mocks
{
    class CampaignDALMock: ICampaignCollection, ICampaignFunctions, IUserDMCampaignCollection
    {
        private readonly IPOICollection _POICollection;
        private readonly ICampaignRelationCollection _campaignRelationCollection;

        public List<CampaignDTO> campaignDTOs;

        public CampaignDALMock(IPOICollection POICollection, ICampaignRelationCollection campaignRelationCollection, params CampaignDTO[] campaigns)
        {
            _POICollection =  POICollection;
            _campaignRelationCollection = campaignRelationCollection;

            campaignDTOs = new List<CampaignDTO>(campaigns);
        }

        public CampaignDTO GetCampaign(int ID)
        {
            CampaignDTO campaign = campaignDTOs.Find(x => x.ID == ID);
            if (campaign == null)
            {
                return null;
            }

            campaign.POIs = _POICollection.GetCampaignPOIs(ID);
            campaign.Players = _campaignRelationCollection.GetCampaignPlayers(ID);
            return campaign;
        }

        public List<CampaignDTO> GetDMCampaignsForUser(int userID)
        {
            List<CampaignDTO> matchList = campaignDTOs.FindAll(x => x.DM.ID == userID);

            List<CampaignDTO> returnList = new List<CampaignDTO>();
            foreach(CampaignDTO campaign in matchList)
            {
                returnList.Add(GetCampaign(campaign.ID));
            }
            return returnList;
        }

        public bool AddCampaign(CampaignDTO campaignDTO)
        {
            campaignDTO.ID = campaignDTOs.Count + 1;
            campaignDTOs.Add(campaignDTO);
            return true;
        }

        public bool DeleteCampaign(int campaignID)
        {
            return campaignDTOs.Remove(GetCampaign(campaignID));
        }

        public int UpdateCampaign(CampaignDTO campaign)
        {
            CampaignDTO campaignDTO = campaignDTOs.Find(x => x.ID == campaign.ID);
            
            if (campaignDTO == null)
            {
                return 0;
            }

            campaignDTO.Map = campaign.Map;
            campaignDTO.Name = campaign.Name;
            return 1;
        }
    }
}
