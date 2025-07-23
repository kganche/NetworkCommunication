using Api1.Grpc.Protos;
using Api2.Dtos;
using Data.Models;

namespace Api2.GrpcServcies;

public class ProductGrpcService(Api1GrpcService.Api1GrpcServiceClient api1GrpcServiceClient)
{
    public async Task<UserWithProductResponse> GetProductsByUserIdAsync(Guid userId)
    {
        var userResponse = await api1GrpcServiceClient.GetUserAsync(new GetUserRequest { UserId = userId.ToString() })
            ?? throw new Exception($"User with ID {userId} not found.");

        var productsResponse = await api1GrpcServiceClient.GetProductsForUserAsync(new GetProductsForUserRequest { UserId = userId.ToString() });
        
        var userWithProducts = new UserWithProductResponse
        {
            Id = Guid.Parse(userResponse.UserId),
            FullName = userResponse.Name,
            Email = userResponse.Email,
            Products = [.. productsResponse.Products.Select(p => new Product
            {
                Id = Guid.Parse(p.ProductId),
                UserId = Guid.Parse(p.UserId),
                ProductName = p.Name,
                Price = (decimal)p.Price
            })]
        };
        return userWithProducts;
    }
}
