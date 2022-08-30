using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

using AL;
using DTO;

namespace DAL
{
    public class LinkTableDAL: BaseDAL, ICampaignRelations, ICampaignRelationCollection, IUserCampaignCollection
    {
        private readonly IUserCollection _userCollection;
        private readonly ICampaignCollection _campaignCollection;

        public LinkTableDAL(IUserCollection userCollection, ICampaignCollection campaignCollection)
        {
            _userCollection = userCollection;
            _campaignCollection = campaignCollection;
        }        


        public List<CampaignDTO> GetUserCampaigns(int userID)
        {
            string query = "SELECT CampaignID FROM dbo.CampaignPlayers WHERE UserID=@UserID";

            SqlParameter idParam = new SqlParameter("@UserID", SqlDbType.Int);
            idParam.Value = userID;

            SqlCommand cmd = base.commandBuilder(query, idParam);
            DataTable dataTable = base.runQuery(cmd);

            List<CampaignDTO> CampaignList = new List<CampaignDTO>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    CampaignList.Add(_campaignCollection.GetCampaign(Convert.ToInt32(row["CampaignID"])));
                }
            }
            return CampaignList;
        }


        public List<UserDTO> GetCampaignPlayers(int campaignID)
        {
            string query = "SELECT UserID FROM dbo.CampaignPlayers WHERE CampaignID=@CampaignID";

            SqlParameter idParam = new SqlParameter("@CampaignID", SqlDbType.Int);
            idParam.Value = campaignID;

            SqlCommand cmd = base.commandBuilder(query, idParam);
            DataTable dataTable = base.runQuery(cmd);

            List<UserDTO> CampaignIDList = new List<UserDTO>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    CampaignIDList.Add(_userCollection.GetUser(Convert.ToInt32(row["UserID"])));
                }
            }

            return CampaignIDList;
        }


        public bool AddCampaignPlayerLink(int campaignID, int userID)
        {
            string query = "INSERT INTO dbo.CampaignPlayers VALUES (@From, @To)";
            SqlCommand cmd = buildLinkQuery(query, campaignID, userID);
            return base.runNonQuery(cmd) == 1;
        }

        public bool RemoveCampaignPlayerLink(int campaignID, int userID)
        {
            string query = "DELETE FROM dbo.CampaignPlayers WHERE CampaignID=@From AND UserID=@To";
            SqlCommand cmd = buildLinkQuery(query, campaignID, userID);
            return base.runNonQuery(cmd) == 1;
        }

        private SqlCommand buildLinkQuery(string baseQuery, int from, int to)
        {
            SqlParameter fromParam = new SqlParameter("@From", SqlDbType.Int);
            fromParam.Value = from;

            SqlParameter toParam = new SqlParameter("@To", SqlDbType.Int);
            toParam.Value = to;

            return base.commandBuilder(baseQuery, fromParam, toParam);
        }
    }
}
