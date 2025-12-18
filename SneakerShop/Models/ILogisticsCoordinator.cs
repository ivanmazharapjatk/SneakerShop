namespace SneakerShop.Models;

public interface ILogisticsCoordinator
{
    public List<Supply>? AssignedSupplies { get; set; }
    public void AskForSupply (Supplier supplier);
}