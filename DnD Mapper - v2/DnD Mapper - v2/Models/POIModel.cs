using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web;
using System.Linq;
using System.Threading.Tasks;


namespace DnD_Mapper___v2.Models
{
    public class POIModel
    {
        [Required]       
        public int ID { get; set; }

        [Required,
            MinLength(3, ErrorMessage = "Campaign Name must have at least 3 characters"),
            MaxLength(256, ErrorMessage = "Campaign Name cant be longer than 256 characters"),
            DataType(DataType.Text, ErrorMessage = "Campaign Name is an incorect type"),
            DisplayName("Campaign Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string RegionName { get; set; }
    }
}
