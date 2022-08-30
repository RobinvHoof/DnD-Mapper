using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using AL;
using DTO;
using System.ComponentModel;
using System.IO;

namespace BLL.Models
{
    public class Campaign
    {
        private readonly ICampaignRelations _campaignRelations;
        private readonly ICampaignFunctions _campaignFunctions;
        private readonly IPOICollection _POICollection;
        private readonly IUserCollection _userCollection;

        public int ID { get; set; }
        public string Name { get; set; }

        public Bitmap Map { get; set; }

        public List<User> Players { get; set; }
        public User DM { get; set; }
        public List<POI> POIs { get; set; }

        public byte[] MapBinary
        {
            get
            {
                if (Map != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        Map.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        return stream.ToArray();
                    }
                }
                else
                {
                    return new byte[] { };
                }
            }
        }

        public Campaign(CampaignDTO campaignDTO)
        {
            _campaignRelations = FL.ICampaignRelationsFactory.GetICampaignRelations();
            _campaignFunctions = FL.ICampaignFunctionsFactory.GetICampaignFunctions();
            _POICollection = FL.IPOICollectionFactory.GetIPOICollection();
            _userCollection = FL.IUserCollectionFactory.GetIUserCollection();

            Players = new List<User>();
            foreach (UserDTO userDTO in campaignDTO.Players)
            {
                Players.Add(new User
                {
                    ID = userDTO.ID,
                    Username = userDTO.Username
                });
            }

            POIs = new List<POI>();
            foreach (POIDTO pOIDTO in campaignDTO.POIs)
            {
                POIs.Add(new POI
                {
                    ID = pOIDTO.ID,
                    Name = pOIDTO.Name,
                    Description = pOIDTO.Description,
                    RegionName = pOIDTO.RegionName,
                    LinkedCampaignID = pOIDTO.LinkedCampaignID
                });
            }

            DM = new User
            {
                ID = campaignDTO.DM.ID,
                Username = campaignDTO.DM.Username
            };
            ID = campaignDTO.ID;
            Map = campaignDTO.Map;
            Name = campaignDTO.Name;
        }

        public Campaign(ICampaignRelations campaignRelations, ICampaignFunctions campaignFunctions, IPOICollection POICollection, IUserCollection userCollection, CampaignDTO campaignDTO = null)
        {
            _campaignRelations = campaignRelations;
            _campaignFunctions = campaignFunctions;
            _POICollection = POICollection;
            _userCollection = userCollection;

            if (campaignDTO != null)
            {
                Players = new List<User>();
                foreach (UserDTO userDTO in campaignDTO.Players)
                {
                    Players.Add(new User
                    {
                        ID = userDTO.ID,
                        Username = userDTO.Username
                    });
                }

                POIs = new List<POI>();
                foreach (POIDTO pOIDTO in campaignDTO.POIs)
                {
                    POIs.Add(new POI
                    {
                        ID = pOIDTO.ID,
                        Name = pOIDTO.Name,
                        Description = pOIDTO.Description,
                        RegionName = pOIDTO.RegionName,
                        LinkedCampaignID = pOIDTO.LinkedCampaignID
                    });
                }

                DM = new User
                {
                    ID = campaignDTO.DM.ID,
                    Username = campaignDTO.DM.Username
                };
                ID = campaignDTO.ID;
                Map = campaignDTO.Map;
                Name = campaignDTO.Name;
            }
        }


        public bool AddPlayer(User player)
        {
            if (DM.ID == player.ID || Players.Exists(x => x.ID == player.ID) || _userCollection.GetUser(player.ID) == null)
            {
                return false;
            }

            return _campaignRelations.AddCampaignPlayerLink(ID, player.ID);
        }

        public bool KickPlayer(int playerID)
        {
            if (Players.Exists(x => x.ID == playerID))
            {
                return _campaignRelations.RemoveCampaignPlayerLink(ID, playerID);
            }
            return false;            
        }

        public bool AddPOI(POI _POI)
        {
            if (POIs.Find(x => x.Name == _POI.Name) != null)
            {
                return false;
            }

            return _POICollection.AddPOI(new POIDTO
            {
                Name = _POI.Name,
                RegionName = _POI.RegionName,
                Description = _POI.Description,
                LinkedCampaignID = ID
            }) > 0;
        }
                
        public bool DeletePOI(int POIID)
        {
            if (POIs.Exists(x => x.ID == POIID))
            {
                return _POICollection.RemovePOI(POIID);
            }
            return false;
        }
        
        public POI GetPOI(int POIID)
        {
            return POIs.Find(x => x.ID == POIID);
        }

        public void Clear()
        {
            foreach(User player in Players)
            {
                _campaignRelations.RemoveCampaignPlayerLink(ID, player.ID);
            }

            foreach(POI _POI in POIs)
            {
                _POICollection.RemovePOI(_POI.ID);
            }
        }        

        public bool Update()
        {
            int updates = _campaignFunctions.UpdateCampaign(
                new CampaignDTO
                {
                    ID = ID,
                    Map = Map,
                    Name = Name
                });
            return updates > 0;
        }
    }
}
