using System.Collections.Generic;
using Shops.Tools;

namespace Shops.Entities
{
    public class Supplier
    { // Поставщик
        private static int _nextSupplierId;

        public Supplier(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new SupplierException("Name of supplier is null or empty");
            }

            SupplierId = _nextSupplierId;
            _nextSupplierId++;
            Suppliername = name;
        }

        public string Suppliername { get; }
        public int SupplierId { get; }
        public override int GetHashCode()
        {
            return SupplierId;
        }
    }
}