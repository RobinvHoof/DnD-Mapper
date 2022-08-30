using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DnD_Mapper___v2.Models
{
    public class CreateCampaignModel
    {
        [Required,
            MinLength(3, ErrorMessage = "Campaign name must be at least 3 characters long"),
            MaxLength(256, ErrorMessage = "Campaign name is too long")]
        public string Name {get; set;}
    }
}
