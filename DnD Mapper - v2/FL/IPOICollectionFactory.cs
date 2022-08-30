using System;
using AL;

namespace FL
{
    public static class IPOICollectionFactory
    {
        public static IPOICollection GetIPOICollection()
        {
            return new DAL.POIDAL();
        }
    }
}
