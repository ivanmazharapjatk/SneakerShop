namespace SneakerShop.Models;

public interface ICustomerSupportAgent
{
    public string? ContactNumber { get; set; }

    public void RespondToReview(Review review);
}