using Bogus;
using Data.Models;

namespace Api1.Data;

public static class DataStore
{
    private static readonly List<User> _users = [];
    private static readonly List<Product> _products = [];

    private static readonly Faker<User> _userFaker;
    private static readonly Faker<Product> _productFaker;

    static DataStore()
    {
        _userFaker = new Faker<User>()
            .RuleFor(u => u.Id, f => Guid.NewGuid())
            .RuleFor(u => u.FullName, f => f.Name.FullName())
            .RuleFor(u => u.Email, f => f.Internet.Email());

        _productFaker = new Faker<Product>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.ProductName, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price()));

        SeedData();
    }

    public static IEnumerable<User> GetAllUsers() => _users;

    public static User? GetUserById(Guid id) => _users.FirstOrDefault(u => u.Id == id);

    public static IEnumerable<Product> GetProductsByUserId(Guid userId) =>
        [.. _products.Where(p => p.UserId == userId)];

    public static Product CreateProductForUser(Guid userId, string name, decimal price)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ProductName = name,
            Price = price
        };

        _products.Add(product);
        return product;
    }

    public static User CreateUser(string fullName, string email)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = fullName,
            Email = email
        };

        _users.Add(user);
        return user;
    }

    private static void SeedData()
    {
        var users = _userFaker.Generate(5);
        _users.AddRange(users);

        foreach (var user in users)
        {
            var userProducts = _productFaker.Clone()
                .RuleFor(p => p.UserId, _ => user.Id)
                .Generate(new Random().Next(1, 4));

            _products.AddRange(userProducts);
        }
    }
}