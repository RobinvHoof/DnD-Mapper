using Microsoft.AspNetCore.Mvc;
using System.Web;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Models;

using BLL;
using System.Drawing;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace DnD_Mapper___v2.Controllers
{
    public class CampaignController : Controller
    {
        private readonly CampaignManager _campaignManager;
        private readonly UserManager _userManager;

        public CampaignController(CampaignManager campaignManager, UserManager userManager)
        {
            _campaignManager = campaignManager;
            _userManager = userManager;
        }

        // View Display Methods
        [HttpGet]
        public IActionResult List()
        {
            if (!CheckLogin())
            {
                return Redirect("../Home/Index");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!CheckLogin())
            {
                return Redirect("../Home/Index");
            }

            return View();
        }

        // Complex View Methods
        [HttpGet]
        public IActionResult Index(int campaignID, bool DMMode = false)
        {
            if (!CheckLogin())
            {
                return Redirect("../Home/Index");
            }

            Campaign campaign = _campaignManager.GetCampaign(campaignID);
            if (DMMode && CheckDMIdentity(campaignID))
            {
                ViewBag.DMMode = true;
            }

            Models.CampaignModel campaignModel = BuildModel(campaign);

            return View(campaignModel/*BuildModel(campaign)*/);
        }

        [HttpGet]
        public IActionResult Players(int campaignID, bool DMMode = false)
        {
            if (!CheckLogin())
            {
                return Redirect("../Home/Index");
            }

            Campaign campaign = _campaignManager.GetCampaign(campaignID);
            if (DMMode && CheckDMIdentity(campaignID))
            {
                ViewBag.DMMode = true;
            }

            return View(BuildModel(campaign));
        }

        [HttpGet]
        public IActionResult Delete(int campaignID)
        {
            if (CheckDMIdentity(campaignID))
            {
                _campaignManager.DeleteCampaign(campaignID);
            }
            return Redirect("./List");
        }

        [HttpGet]
        public IActionResult Kick(int campaignID, int playerID)
        {
            if (!CheckDMIdentity(campaignID))
            {
                return Redirect("./Players?campaignID=" + campaignID + "&DMMode=false");
            }

            Campaign campaign = _campaignManager.GetCampaign(campaignID);
            if (campaign != null)
            {
                campaign.KickPlayer(playerID);
            }
            return Redirect("./Players?campaignID=" + campaignID + "&DMMode=true");
        }

        [HttpGet]
        public IActionResult CampaignDetails(int campaignID)
        {
            if (!CheckDMIdentity(campaignID))
            {
                return Redirect("../Home/Index");
            }
            ViewBag.DMMode = true;

            Campaign campaign = _campaignManager.GetCampaign(campaignID);
            return View(BuildModel(campaign));
        }


        // POST Methods
        [HttpPost]
        public IActionResult Create(Models.CreateCampaignModel campaignModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            int userID = Convert.ToInt32(HttpContext.Request.Cookies["user_id"]);
            _campaignManager.CreateCampaign(campaignModel.Name, userID);
            return Redirect("./List");        
        }
       
        [HttpPost]
        public IActionResult AddPlayer(string username, int campaignID)
        {
            if (!CheckDMIdentity(campaignID))
            {
                return Redirect("./Players?campaignID=" + campaignID + "&DMMode=false");
            }

            User user = _userManager.GetUser(username);
            Campaign campaign = _campaignManager.GetCampaign(campaignID);

            if (user != null && campaign != null)
            {
                campaign.AddPlayer(user);
            }
            return Redirect("./Players?campaignID=" + campaignID + "&DMMode=true");
        }

        [HttpPost]
        public IActionResult CampaignDetails(Models.CampaignModel campaignModel)
        {
            if (!CheckDMIdentity(campaignModel.ID))
            {
                return Redirect("./Index?campaignID=" + campaignModel.ID + "&DMMode=false");
            }

            byte[] mapBinary = null;
            if (Request.Form.Files.Count > 0)
            {
                IFormFile image = Request.Form.Files.First();
                using (BinaryReader binaryReader = new BinaryReader(image.OpenReadStream()))
                {
                    mapBinary = binaryReader.ReadBytes((int)image.OpenReadStream().Length);
                }
            }

            Campaign origionalCampaign = _campaignManager.GetCampaign(campaignModel.ID);
            campaignModel.DM ??= origionalCampaign.DM;
            campaignModel.Players ??= origionalCampaign.Players;

            ViewBag.DMMode = true;
            ModelState.Remove("DM");
            ModelState.Remove("Players");
            
            TryValidateModel(campaignModel);
            if (!ModelState.IsValid)
            {
                
                Models.CampaignModel model = BuildModel(origionalCampaign);

                model.Map = mapBinary;
                return View(model);
            }

            Bitmap map = null;
            if (mapBinary != null)
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(Bitmap));
                map = (Bitmap)converter.ConvertFrom(mapBinary);
            }

            origionalCampaign.Name = campaignModel.Name;
            if (mapBinary != null) { origionalCampaign.Map = map;  }
            origionalCampaign.Update();

            return Redirect("./Index?campaignID=" + campaignModel.ID + "&DMMode=true");
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
                Map = campaign.MapBinary,
                POIs = campaign.POIs
            };
        }
    }
}
