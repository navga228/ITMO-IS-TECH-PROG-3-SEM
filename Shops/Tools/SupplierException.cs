using System;

namespace Shops.Tools
{
    public class SupplierException : Exception
    {
        public SupplierException()
        {
        }

        public SupplierException(string message)
            : base(message)
        {
        }

        public SupplierException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}