using Data.Models;

namespace Api2.Dtos;

public class UserWithProductResponse
{
    public Guid Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public List<Product> Products { get; set; } = [];
}
