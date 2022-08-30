using System;
using AL;

namespace FL
{
    public static class IUserCollectionFactory
    {
        public static IUserCollection GetIUserCollection()
        {
            return new DAL.UserDAL();
        }
    }
}
