using Api1.Data;
using Api1.Dtos;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api1.Controllers;

[ApiController]
public class ProductController : ControllerBase
{
    [HttpGet("Products/{userId}")]
    public ActionResult<IEnumerable<Product>> GetProductsByUserId(Guid userId)
    {
        var productsForUser = DataStore.GetProductsByUserId(userId);
        return Ok(productsForUser);
    }

    [HttpPost("Product")]
    public IActionResult CreateProduct(ProductRequest product)
    {
        if (product == null)
        {
            return BadRequest();
        }

        DataStore.CreateProductForUser(product.UserId, product.ProductName, product.Price);

        return Ok(product);
    }
}
