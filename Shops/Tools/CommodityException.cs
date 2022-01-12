using System;

namespace Shops.Tools
{
    public class CommodityException : Exception
    {
        public CommodityException()
        {
        }

        public CommodityException(string message)
            : base(message)
        {
        }

        public CommodityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}