namespace SneakerShop.Models;

public class Employee
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Position { get; set; }
    public static decimal BaseSalary { get; set; } = 2500m;
    public bool Bonus { get; set; }
    public decimal salaryNow { get; set; }
    public int ClearanceLevel { get; set; }
}