using System.Collections.Generic;

namespace Shops.Entities
{
    public class Batches
    {
        public Supplier Supplier { get; set; }

        public Dictionary<Product, ProductDescription> DeliveredProducts { get; set; } =
            new Dictionary<Product, ProductDescription>();
    }
}