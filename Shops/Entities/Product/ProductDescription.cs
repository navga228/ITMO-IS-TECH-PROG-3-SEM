namespace Shops.Entities
{
    public class ProductDescription
    {
        public ProductDescription(int amount, int price)
        {
            Amount = amount;
            Price = price;
        }

        public int Amount { get; set; } // Количество
        public int Price { get; set; }
    }
}