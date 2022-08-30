using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DTO;

namespace AL
{
    public interface ICampaignFunctions
    {
        public int UpdateCampaign(CampaignDTO campaign);
    }
}
