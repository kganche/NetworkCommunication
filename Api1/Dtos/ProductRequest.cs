namespace Api1.Dtos;

public class ProductRequest
{
    public Guid UserId { get; set; }

    public string ProductName { get; set; }

    public decimal Price { get; set; }
}
