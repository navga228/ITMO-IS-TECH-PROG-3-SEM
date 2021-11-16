using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Entities;
using Shops.Tools;

namespace Shops.Entities.Delivery
{
    public class SupplierServise
    {
        public Supplier AddSupplier(string name)
        {
            Supplier newSupplier = new Supplier(name);
            return newSupplier;
        }
    }
}