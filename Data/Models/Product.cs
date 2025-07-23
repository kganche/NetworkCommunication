namespace Data.Models;

public class Product
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string ProductName { get; set; }

    public decimal Price { get; set; }
}
