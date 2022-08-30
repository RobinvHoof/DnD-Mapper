using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AL;
using DTO;

namespace BLL.Models
{
    public class User
    {
        private readonly CampaignManager _campaignManager;

        private readonly IUserCampaignCollection _userCampaignCollection;
        private readonly IUserDMCampaignCollection _userDMCampaignCollection;        


        public int ID { get; set; }
        public string Username { get; set; }

        public User()
        {
            _campaignManager = new CampaignManager();
            _userCampaignCollection = FL.IUserCampaignCollectionFactory.GetIUserCampaignCollection();            
            _userDMCampaignCollection = FL.IUserDMCampaignCollectionFactory.GetIUserDMCampaignCollection();
        }

        public User(CampaignManager campaignManager, IUserCampaignCollection userCampaignCollection, IUserDMCampaignCollection userDMCampaignCollection)
        {
            _campaignManager = campaignManager;
            _userCampaignCollection = userCampaignCollection;
            _userDMCampaignCollection = userDMCampaignCollection;

        }

        public List<Campaign> GetCampaigns() 
        {
            List<CampaignDTO> campaignDTOs = _userCampaignCollection.GetUserCampaigns(ID);

            List<Campaign> campaigns = new List<Campaign>();
            foreach (CampaignDTO campaign in campaignDTOs)
            {
                campaigns.Add(new Campaign(campaign));
            }

            return campaigns;
        }

        public List<Campaign> GetDMCampaigns()
        {
            List<CampaignDTO> campaignDTOs = _userDMCampaignCollection.GetDMCampaignsForUser(ID);

            List<Campaign> campaigns = new List<Campaign>();
            foreach (CampaignDTO campaignDTO in campaignDTOs)
            {
                
                campaigns.Add(new Campaign(campaignDTO));
            }

            return campaigns;
        }
    }
}
