using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DTO;
using AL;

namespace BLLTests.Mocks
{
    class POIDALMock: IPOICollection, IPOIFunctions
    {
        public List<POIDTO> POIDTOs;

        public POIDALMock(params POIDTO[] POIs)
        {
            POIDTOs = new List<POIDTO>(POIs);
        }

        public POIDTO GetPOI(int ID)
        {
            return POIDTOs.Find(x => x.ID == ID);
        }

        public List<POIDTO> GetCampaignPOIs(int campaignID)
        {
            return POIDTOs.FindAll(x => x.LinkedCampaignID == campaignID);
        }

        public int AddPOI(POIDTO _POIDTO)
        {
            _POIDTO.ID = POIDTOs.Count + 1;
            POIDTOs.Add(_POIDTO);
            return 1;
        }

        public bool RemovePOI(int POIID)
        {
            return POIDTOs.Remove(GetPOI(POIID));
        }

        public int UpdatePOI(POIDTO POI)
        {
            POIDTO _POIDTO = POIDTOs.Find(x => x.ID == POI.ID);

            if (_POIDTO == null)
            {
                return 0;
            }

            _POIDTO.Name = POI.Name;
            _POIDTO.RegionName = POI.RegionName;
            _POIDTO.Description = POI.Description;
            return 1;
        }
    }
}
