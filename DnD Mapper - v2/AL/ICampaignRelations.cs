using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AL
{
    public interface ICampaignRelations
    {        
        public bool AddCampaignPlayerLink(int campaignID, int userID);
        public bool RemoveCampaignPlayerLink(int campaignID, int userID);
    }
}
