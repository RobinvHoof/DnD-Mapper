using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BLL;
using BLL.Models;

namespace DnD_Mapper___v2.Controllers
{
    public class POIController : Controller
    {
        private readonly CampaignManager _campaignManager;

        public POIController(CampaignManager campaignManager)
        {
            _campaignManager = campaignManager;
        }

        // View Return GET Methods
        [HttpGet]
        public IActionResult Create(int campaignID)
        {
            Campaign campaign = _campaignManager.GetCampaign(campaignID);
            return View(new Models.POICampaignModel
            {
                ID = campaign.ID,
                Name = campaign.Name
            });
        }


        // Complicated GET Methods
        [HttpGet]
        public IActionResult GetPOIInfo(int campaignID, int POIID)
        {
            Campaign campaign = _campaignManager.GetCampaign(campaignID);
            POI _POI = campaign.GetPOI(POIID);

            return View(@"./Partials/_POIInfoPartial", BuildModel(_POI));
        }

        // POST Methods
        [HttpPost]
        public IActionResult Create(Models.POICampaignModel POIModel)
        {
            Campaign campaign = _campaignManager.GetCampaign(POIModel.ID);

            ModelState.Remove("DM");
            ModelState.Remove("Players");

            if (!ModelState.IsValid)
            {
                return View(new Models.POICampaignModel() { Name = campaign.Name, ID = campaign.ID });
            }

            campaign.AddPOI(new POI
            {
                Name = POIModel.POIName,
                Description = POIModel.Description,
                RegionName = POIModel.RegionName
            });
            return Redirect("../Campaign/Index?campaignID=" + campaign.ID + "&DMMode=true");
        }

        [HttpPost]
        public IActionResult Edit(Models.POICampaignModel POIModel)
        {
            Campaign campaign = _campaignManager.GetCampaign(POIModel.ID);

            ModelState.Remove("DM");
            ModelState.Remove("Players");

            if (!ModelState.IsValid)
            {
                return View(POIModel);
            }

            POI _POI = campaign.GetPOI(POIModel.POIID);
            if (_POI != null)
            {
                _POI.Name = POIModel.POIName;
                _POI.Description = POIModel.Description ?? "";
                _POI.RegionName = POIModel.RegionName ?? "";
                _POI.Update();
            }

            return Redirect("../../Campaign/Index?campaignID=" + POIModel.ID + "&DMMode=true");
        }

        // 
        [HttpGet]
        public IActionResult Delete(int campaignID, int POIID)
        {
            if (!CheckDMIdentity(campaignID))
            {
                return Redirect("../../Campaign/Index?campaignID=" + campaignID + "&DMMode=false");
            }

            _campaignManager.GetCampaign(campaignID).DeletePOI(POIID);
            return Redirect("../../Campaign/Index?campaignID=" + campaignID + "&DMMode=true");
        }

        [HttpGet]
        public IActionResult Edit(int campaignID, int POIID)
        {
            if (!CheckDMIdentity(campaignID))
            {
                return Redirect("../../Campaign/Index?campaignID=" + campaignID + "&DMMode=false");
            }

            Campaign campaign = _campaignManager.GetCampaign(campaignID);
            POI _POI = campaign.GetPOI(POIID);

            return View(BuildModel(campaign, _POI));
        }

        // Helper Methods
        private bool CheckLogin()
        {
            return HttpContext.Request.Cookies.Keys.Contains("user_id");
        }

        private bool CheckDMIdentity(int campaignID)
        {
            int userID = Convert.ToInt32(HttpContext.Request.Cookies["user_id"]);
            Campaign campaign = _campaignManager.GetCampaign(campaignID);

            if (campaign != null)
            {
                return campaign.DM.ID == userID;
            }
            return false;
        }

        private Models.CampaignModel BuildModel(Campaign campaign)
        {
            return new Models.CampaignModel
            {
                ID = campaign.ID,
                DM = campaign.DM,
                Name = campaign.Name,
                Players = campaign.Players,
                Map = campaign.MapBinary
            };
        }

        private Models.POIModel BuildModel(POI _POI)
        {
            return new Models.POIModel
            {
                ID = _POI.ID,
                Name = _POI.Name,
                Description = _POI.Description,
                RegionName = _POI.RegionName
            };
        }

        private Models.POICampaignModel BuildModel(Campaign campaign, POI _POI)
        {
            return new Models.POICampaignModel
            {
                ID = campaign.ID,
                DM = campaign.DM,
                Name = campaign.Name,
                Players = campaign.Players,
                Map = campaign.MapBinary,

                POIID = _POI.ID,
                POIName = _POI.Name,
                Description = _POI.Description,
                RegionName = _POI.RegionName
            };
        }
    }
}
