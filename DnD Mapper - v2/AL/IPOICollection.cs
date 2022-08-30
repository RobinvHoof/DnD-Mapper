using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DTO;

namespace AL
{
    public interface IPOICollection
    {
        public POIDTO GetPOI(int ID);
        public List<POIDTO> GetCampaignPOIs(int campaignID);
        public int AddPOI(POIDTO _POIDTO);
        public bool RemovePOI(int POIID);
    }
}
