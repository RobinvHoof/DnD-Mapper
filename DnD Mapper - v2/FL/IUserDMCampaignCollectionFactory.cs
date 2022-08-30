using System;
using AL;

namespace FL
{
    public static class IUserDMCampaignCollectionFactory
    {
        public static IUserDMCampaignCollection GetIUserDMCampaignCollection()
        {
            return new DAL.CampaignDAL(ICampaignRelationCollectionFactory.GetICampaignRelationCollection(), 
                                            IUserCollectionFactory.GetIUserCollection(),
                                            IPOICollectionFactory.GetIPOICollection());
        }
    }
}
