namespace SneakerShop.Models
{
    public class Stock
    {
        public int Quantity { get; set; }

        public int GetStock()
        {
            return Quantity;
        }
    }
}