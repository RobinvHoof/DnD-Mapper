using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AL;
using DTO;
using BLL.Models;

namespace BLL
{
    public class CampaignManager
    {
        private readonly ICampaignCollection _campaignCollection;
        private readonly UserManager _userManager;

        public CampaignManager()
        {
            _campaignCollection = FL.ICampaignCollectionFactory.GetICampaignCollection();
            _userManager = new UserManager();
        }

        public CampaignManager(ICampaignCollection campaignCollection, UserManager userManager)
        {
            _campaignCollection = campaignCollection;
            _userManager = userManager;
        }

        /// <summary>
        /// Create a new campaign with given specifications
        /// </summary>
        /// <param name="name">Name that should be assigned to the campaign</param>
        /// <param name="DMID">User ID of the User account that should be linked as Campaign DM</param>
        /// <returns>Result of creation action</returns>
        public bool CreateCampaign(string name, int DMID)
        {
            if (_userManager.GetUser(DMID) == null || name == null || name.Length == 0)
            {
                return false;
            }

            CampaignDTO campaign = new CampaignDTO { Name=name, DM = new UserDTO { ID = DMID } };            
            return _campaignCollection.AddCampaign(campaign);
        }

        /// <summary>
        /// Delete a Campaign with given ID
        /// </summary>
        /// <param name="campaignID">ID of the Campaign that should be delted</param>
        /// <returns>Result of the deletion action</returns>
        public bool DeleteCampaign(int campaignID)
        {
            Campaign campaign = GetCampaign(campaignID);

            if (campaign == null)
            {
                return false;
            }

            campaign.Clear();

            return (_campaignCollection.DeleteCampaign(campaignID));
        }

        /// <summary>
        /// Get Campaign object with give ID
        /// </summary>
        /// <param name="campaignID">ID of the Campaign that needs to be aquired</param>
        /// <returns>Campaign objective with the requested Campaign ID</returns>
        public Campaign GetCampaign(int campaignID)
        {
            CampaignDTO campaignDTO = _campaignCollection.GetCampaign(campaignID);
            if (campaignDTO == null)
            {
                return null;
            }

            return new Campaign(campaignDTO);
        }        
    }
}
