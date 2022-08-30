using System;
using AL;

namespace FL
{
    public static class IUserCampaignCollectionFactory
    {
        public static IUserCampaignCollection GetIUserCampaignCollection()
        {
            return new DAL.LinkTableDAL(IUserCollectionFactory.GetIUserCollection(), ICampaignCollectionFactory.GetICampaignCollection());
        }
    }
}
