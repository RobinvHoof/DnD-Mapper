using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DTO;

namespace AL
{
    public interface ICampaignCollection
    {
        public CampaignDTO GetCampaign(int ID);
        public bool AddCampaign(CampaignDTO campaignDTO);
        public bool DeleteCampaign(int campaignID);
    }
}
