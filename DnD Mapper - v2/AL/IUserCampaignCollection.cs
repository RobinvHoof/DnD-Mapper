using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DTO;

namespace AL
{
    public interface IUserCampaignCollection
    {
        public List<CampaignDTO> GetUserCampaigns(int userID);
    }
}
