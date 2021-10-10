namespace Shops.Entities
{
    public class ProductDescription
    {
        public ProductDescription()
        {
            Amount = 0;
            Price = 0;
        }

        public ProductDescription(int amount, int price)
        {
            Amount = amount;
            Price = price;
        }

        public int Amount { get; set; } // Количество
        public int Price { get; set; }

        // public Product DescriptionOfProduct { get; set; } // К какому продукту привязано описание
    }
}