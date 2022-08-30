using System;
using AL;

namespace FL
{
    public static class ICampaignRelationCollectionFactory
    {
        public static ICampaignRelationCollection GetICampaignRelationCollection()
        {
            return new DAL.LinkTableDAL(IUserCollectionFactory.GetIUserCollection(), null);
        }
    }
}
