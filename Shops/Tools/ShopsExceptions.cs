using System;

namespace Shops.Tools
{
    public class ShopsExceptions : Exception
    {
        public ShopsExceptions()
        {
        }

        public ShopsExceptions(string message)
            : base(message)
        {
        }

        public ShopsExceptions(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}