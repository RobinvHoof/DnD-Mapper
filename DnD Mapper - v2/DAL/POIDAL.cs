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
    public class POIDAL: BaseDAL, IPOICollection, IPOIFunctions
    {
        public POIDTO GetPOI(int ID)
        {
            string query = "SELECT * FROM dbo.POIs WHERE Id=@ID";

            SqlParameter idParam = new SqlParameter("@ID", SqlDbType.Int);
            idParam.Value = ID;

            SqlCommand cmd = base.commandBuilder(query, idParam);
            DataTable dataTable = base.runQuery(cmd);

            POIDTO _POIDTO = new POIDTO();
            if (dataTable.Rows.Count > 0)
            {
                _POIDTO.ID = Convert.ToInt32(dataTable.Rows[0]["Id"]);
                _POIDTO.Name = Convert.ToString(dataTable.Rows[0]["Name"]);
                _POIDTO.Description = Convert.ToString(dataTable.Rows[0]["Description"]);
                _POIDTO.RegionName = Convert.ToString(dataTable.Rows[0]["RegionName"]);
                _POIDTO.LinkedCampaignID = Convert.ToInt32(dataTable.Rows[0]["LinkedCampaignID"]);
            }
            return _POIDTO;
        }

        public List<POIDTO> GetCampaignPOIs(int campaignID)
        {
            string query = "SELECT * FROM dbo.POIs WHERE LinkedCampaignID=@ID";

            SqlParameter idParam = new SqlParameter("@ID", SqlDbType.Int);
            idParam.Value = campaignID;

            SqlCommand cmd = base.commandBuilder(query, idParam);
            DataTable dataTable = base.runQuery(cmd);

            List<POIDTO> POIDTOs = new List<POIDTO>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    POIDTOs.Add(new POIDTO {
                        ID = Convert.ToInt32(row["Id"]),
                        Name = Convert.ToString(row["Name"]),
                        Description = Convert.ToString(row["Description"]),
                        RegionName = Convert.ToString(row["RegionName"]),
                        LinkedCampaignID = Convert.ToInt32(row["LinkedCampaignID"])
                    });                    
                }
            }
            return POIDTOs;
        }

        public int AddPOI(POIDTO _POIDTO)
        {
            string query = "INSERT INTO dbo.POIs VALUES (@Name, @Description, @RegionName, @CampaignID) SELECT SCOPE_IDENTITY()";

            SqlParameter nameParam = new SqlParameter("@Name", SqlDbType.VarChar, 50);
            nameParam.Value = _POIDTO.Name;

            SqlParameter descriptionParam = new SqlParameter("@Description", SqlDbType.VarChar);
            descriptionParam.Value = _POIDTO.Description ?? "";

            SqlParameter regionParam = new SqlParameter("@RegionName", SqlDbType.VarChar, 50);
            regionParam.Value = _POIDTO.RegionName ?? "";

            SqlParameter campaignIDParam = new SqlParameter("@CampaignID", SqlDbType.Int);
            campaignIDParam.Value = _POIDTO.LinkedCampaignID;


            SqlCommand cmd = base.commandBuilder(query, nameParam, descriptionParam, regionParam, campaignIDParam);
            return Convert.ToInt32(base.runScalarQuery(cmd));
        }

        public bool RemovePOI(int POIID)
        {
            string query = "DELETE FROM dbo.POIs WHERE Id=@Id";

            SqlParameter idParam = new SqlParameter("@Id", SqlDbType.Int);
            idParam.Value = POIID;

            SqlCommand cmd = base.commandBuilder(query, idParam);
            return (base.runNonQuery(cmd) == 1);
        }

        public int UpdatePOI(POIDTO POI)
        {
            string query = "Update dbo.POIs SET Name=@Name, RegionName=@RegionName, Description=@Description WHERE Id=@Id";

            SqlParameter idParam = new SqlParameter("@Id", SqlDbType.Int);
            idParam.Value = POI.ID;

            SqlParameter nameParam = new SqlParameter("@Name", SqlDbType.VarChar, 50);
            nameParam.Value = POI.Name;

            SqlParameter descriptionParam = new SqlParameter("@Description", SqlDbType.VarChar);
            descriptionParam.Value = POI.Description;

            SqlParameter regionParam = new SqlParameter("@RegionName", SqlDbType.VarChar, 50);
            regionParam.Value = POI.RegionName;

            SqlCommand cmd = base.commandBuilder(query, idParam, nameParam, descriptionParam, regionParam);
            return base.runNonQuery(cmd);
        }
    }
}
