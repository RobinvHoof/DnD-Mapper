using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Drawing;

using DTO;
using AL;

namespace DAL
{
    public class CampaignDAL: BaseDAL, ICampaignCollection, ICampaignFunctions, IUserDMCampaignCollection
    {
        private readonly ICampaignRelationCollection _campaignRelationCollection;
        private readonly IUserCollection _userCollection;
        private readonly IPOICollection _POICollection;

        public CampaignDAL(ICampaignRelationCollection campaignRelationCollection, IUserCollection userCollection, IPOICollection POICollection)
        {
            _campaignRelationCollection = campaignRelationCollection;
            _userCollection = userCollection;
            _POICollection = POICollection;
        }

        public CampaignDTO GetCampaign(int ID)
        {
            string query = "SELECT * FROM dbo.Campaigns WHERE Id=@Id";

            SqlParameter idParam = new SqlParameter("@Id", SqlDbType.Int);
            idParam.Value = ID;

            SqlCommand cmd = base.commandBuilder(query, idParam);
            DataTable dataTable = base.runQuery(cmd);

            CampaignDTO campaignDTO = null;

            if (dataTable.Rows.Count > 0)
            {
                byte[] mapBytes = (byte[])dataTable.Rows[0]["Map"];
                Bitmap map = null;
                if (mapBytes.Length != 0)
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(typeof(Bitmap));
                    map = (Bitmap)converter.ConvertFrom(mapBytes);
                }

                campaignDTO = new CampaignDTO
                {
                    ID = Convert.ToInt32(dataTable.Rows[0]["Id"]),
                    Name = Convert.ToString(dataTable.Rows[0]["Name"]),
                    DM = _userCollection.GetUser(Convert.ToInt32(dataTable.Rows[0]["DMId"])),
                    Map = map,
                    Players = _campaignRelationCollection.GetCampaignPlayers(ID),
                    POIs = _POICollection.GetCampaignPOIs(ID)
                };
            }

            return campaignDTO;
        }

        public List<CampaignDTO> GetDMCampaignsForUser(int userID)
        {
            string query = "SELECT * FROM dbo.Campaigns WHERE DMId=@UserId";

            SqlParameter idParam = new SqlParameter("@UserID", SqlDbType.Int);
            idParam.Value = userID;

            SqlCommand cmd = base.commandBuilder(query, idParam);
            DataTable dataTable = base.runQuery(cmd);

            List<CampaignDTO> CampaignList = new List<CampaignDTO>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    CampaignList.Add(GetCampaign(Convert.ToInt32(row["Id"])));
                }
            }
            return CampaignList;
        }

        public bool AddCampaign(CampaignDTO campaignDTO)
        {
            string query = "INSERT INTO dbo.Campaigns VALUES (@DMId, @Map, @Name)";

            SqlParameter DMIdParam = new SqlParameter("@DMId", SqlDbType.Int);
            DMIdParam.Value = campaignDTO.DM.ID;

            SqlParameter mapParam = new SqlParameter("@Map", SqlDbType.VarBinary);
            mapParam.Value = campaignDTO.MapBinary;

            SqlParameter nameParam = new SqlParameter("@Name", SqlDbType.VarChar, 256);
            nameParam.Value = campaignDTO.Name;

            SqlCommand cmd = base.commandBuilder(query, DMIdParam, mapParam, nameParam);
            return (base.runNonQuery(cmd) == 1);
        }

        public bool DeleteCampaign(int campaignID)
        {
            string query1 = "DELETE FROM dbo.Campaigns WHERE Id=@Id";
            string query2 = "DELETE FROM dbo.CampaignPlayers WHERE CampaignID=@Id";

            SqlParameter idParam1 = new SqlParameter("@Id", SqlDbType.Int);
            idParam1.Value = campaignID;

            SqlParameter idParam2 = new SqlParameter("@Id", SqlDbType.Int);
            idParam2.Value = campaignID;
                
            SqlCommand cmd1 = base.commandBuilder(query1, idParam1);
            SqlCommand cmd2 = base.commandBuilder(query2, idParam2);

            return (base.runNonQuery(cmd1) == 1 && base.runNonQuery(cmd2) >= 0);
        }

        public int UpdateCampaign(CampaignDTO campaign)
        {
            string query = "Update dbo.Campaigns SET Map=@Map, Name=@Name WHERE Id=@Id";

            SqlParameter idParam = new SqlParameter("@Id", SqlDbType.Int);
            idParam.Value = campaign.ID;

            SqlParameter mapParam = new SqlParameter("@Map", SqlDbType.VarBinary);
            mapParam.Value = campaign.MapBinary;

            SqlParameter nameParam = new SqlParameter("@Name", SqlDbType.VarChar, 256);
            nameParam.Value = campaign.Name;

            SqlCommand cmd = base.commandBuilder(query, idParam, mapParam, nameParam);
            return base.runNonQuery(cmd);
        }
    }
}
