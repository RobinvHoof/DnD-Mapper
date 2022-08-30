using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DTO;
using AL;

namespace BLL.Models
{
    public class POI
    {
        public readonly IPOIFunctions _POIFunctions;

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RegionName { get; set; }
        public int LinkedCampaignID { get; set; }

        public POI()
        {
            _POIFunctions = FL.IPOIFunctionsFactory.GetIPOIFunctions();
        }   
        
        public POI(IPOIFunctions POIFunctions)
        {
            _POIFunctions = POIFunctions;
        }
        
        public bool Update()
        {
            int updates = _POIFunctions.UpdatePOI( 
                new POIDTO
                { 
                    ID = ID,
                    Name = Name,
                    RegionName = RegionName,
                    Description = Description
                });
            return updates > 0;
        }
    }
}
