using System;
using AL;

namespace FL
{
    public static class ICampaignCollectionFactory
    {
        public static ICampaignCollection GetICampaignCollection()
        {
            return new DAL.CampaignDAL(ICampaignRelationCollectionFactory.GetICampaignRelationCollection(), IUserCollectionFactory.GetIUserCollection(), IPOICollectionFactory.GetIPOICollection());
        }
    }
}
