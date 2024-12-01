using CapstonePrototype.Dto.Product;
using CapstonePrototype.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SealBackend.Dto;

namespace CapstonePrototype.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService productService):ControllerBase
{
    private readonly IProductService _productService = productService;

    [HttpPost("add")]
    public async Task<ActionResult<ServiceResponse<ProductDto>>> AddProduct(ProductInsertDto product)
    {
        var response = await _productService.CreateNewProduct(product);
        if(response.Success)return Ok(response);
        return BadRequest(response);
    }

    [HttpGet("search/{productInq}")]
    public async Task<ActionResult<ServiceResponse<List<ProductDto>>>> SearchProduct(string productInq)
    {
        var response = await _productService.SearchProduct(productInq);
        if(response.Success)return Ok(response);
        return BadRequest(response);
    }
}