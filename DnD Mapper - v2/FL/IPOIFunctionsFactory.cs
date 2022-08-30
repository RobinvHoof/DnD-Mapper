using System;
using AL;

namespace FL
{
    public static class IPOIFunctionsFactory
    {
        public static IPOIFunctions GetIPOIFunctions()
        {
            return new DAL.POIDAL();
        }
    }
}
