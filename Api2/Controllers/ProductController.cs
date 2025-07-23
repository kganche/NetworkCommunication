using Api2.Dtos;
using Api2.GrpcServcies;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api2.Controllers;

[ApiController]
public class ProductController(
    IHttpClientFactory httpClientFactory,
    ProductGrpcService productGrpcService) : ControllerBase
{
    [HttpGet("api/product/{userId}")]
    public async Task<ActionResult<IEnumerable<UserWithProductResponse>>> GetProductsByUserId(Guid userId)
    {
        using var client = httpClientFactory.CreateClient("products");
        if (client == null)
        {
            return StatusCode(500, "HttpClient not configured properly.");
        }

        var userResponse = await client.GetFromJsonAsync<User>($"/User/{userId}");
        if (userResponse == null)
        {
            return NotFound();
        }

        var productsResponse = await client.GetFromJsonAsync<List<Product>>($"/Products/{userId}") ?? [];

        var userWithProducts = new UserWithProductResponse
        {
            Id = userResponse.Id,
            FullName = userResponse.FullName,
            Email = userResponse.Email,
            Products = productsResponse
        };

        return Ok(userWithProducts);
    }

    [HttpGet("grpc/product/{userId}")]
    public async Task<ActionResult<UserWithProductResponse>> GetProductsByUserIdGrpc(Guid userId)
    {
        try
        {
            var userWithProducts = await productGrpcService.GetProductsByUserIdAsync(userId);
            return Ok(userWithProducts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
