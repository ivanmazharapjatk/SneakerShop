namespace SneakerShop.Models;

public class Order
{
    public DateTime OrderDate { get; set; }
    public string PaymentInfo { get; set; }
    public string Status { get; set; }
    public decimal TotalAmount { get; set; }
}
