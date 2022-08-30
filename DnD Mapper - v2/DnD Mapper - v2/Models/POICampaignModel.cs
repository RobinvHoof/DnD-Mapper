using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DnD_Mapper___v2.Models
{
    public class POICampaignModel : CampaignModel
    {
        // POI Details
        public int POIID { get; set; }

        [Required,
            MaxLength(256, ErrorMessage = "PoI Name cant be longer than 256 characters"),
            DataType(DataType.Text, ErrorMessage = "PoI Name is of an incorect type"),
            DisplayName("Name")]
        public string POIName { get; set; }
        
        [MaxLength(256, ErrorMessage = "Region Name cant be longer than 256 characters"),
            DataType(DataType.Text, ErrorMessage = "Region Name is of an incorect type"),
            DisplayName("Region Name")]
        public string RegionName { get; set; }
        
        [DataType(DataType.Text, ErrorMessage = "Description is an of incorect type"),
            DisplayName("Description")]
        public string Description { get; set; }
    }
}
