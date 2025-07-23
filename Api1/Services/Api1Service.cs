using Api1.Data;
using Grpc.Core;
using Api1.Grpc.Protos;

namespace Api1.Services;

public class Api1Service : Api1GrpcService.Api1GrpcServiceBase
{
    public override Task<GetUserResponse> GetUser(GetUserRequest request, ServerCallContext context)
    {
        var user = DataStore.GetUserById(Guid.Parse(request.UserId));

        return user == null
            ? throw new RpcException(new Status(StatusCode.NotFound, $"User with ID {request.UserId} not found."))
            : Task.FromResult(new GetUserResponse
        {
            UserId = user.Id.ToString(),
            Name = user.FullName,
            Email = user.Email
        });
    }

    public override Task<GetProductsForUserResponse> GetProductsForUser(GetProductsForUserRequest request, ServerCallContext context)
    {
        var products = DataStore.GetProductsByUserId(Guid.Parse(request.UserId));
        var response = new GetProductsForUserResponse();
        response.Products.AddRange(products.Select(p => new ProductResponse
        {
            ProductId = p.Id.ToString(),
            UserId = p.UserId.ToString(),
            Name = p.ProductName,
            Price = (double)p.Price
        }));
        return Task.FromResult(response);
    }
}
