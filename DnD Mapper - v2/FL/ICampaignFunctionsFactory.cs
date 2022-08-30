using System;
using AL;

namespace FL
{
    public static class ICampaignFunctionsFactory
    {
        public static ICampaignFunctions GetICampaignFunctions()
        {
            return new DAL.CampaignDAL(ICampaignRelationCollectionFactory.GetICampaignRelationCollection(), 
                                            IUserCollectionFactory.GetIUserCollection(),
                                            IPOICollectionFactory.GetIPOICollection());
        }
    }
}
