using System;
using AL;

namespace FL
{
    public static class ICampaignRelationsFactory
    {
        public static ICampaignRelations GetICampaignRelations()
        {
            return new DAL.LinkTableDAL(IUserCollectionFactory.GetIUserCollection(), ICampaignCollectionFactory.GetICampaignCollection());
        }
    }
}
