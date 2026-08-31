using Core.Entities;
using Core.Interfaces;
using Infrastracture.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository productRepository) : ControllerBase
{
    private readonly IProductRepository m_productRepository = productRepository;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await m_productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery] string? brand, [FromQuery] string? type, [FromQuery] string? sort)
    {
        var products = await m_productRepository.GetProductsAsync(brand, type, sort);

        return Ok(products);
    }

    [HttpGet("brands")]
    public async Task<IActionResult> GetBrands()
    {
        var brands = await m_productRepository.GetBrandsAsync();
        if (brands == null || brands.Count == 0)
        {
            return NotFound("No brands found.");
        }

        return Ok(brands);
    }

    [HttpGet("types")]
    public async Task<IActionResult> GetTypes()
    {
        var types = await m_productRepository.GetTypesAsync();
        if (types == null || types.Count == 0)
        {
            return NotFound("No types found.");
        }

        return Ok(types);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        m_productRepository.AddProduct(product);
        if (await m_productRepository.SaveChangeAsync())
        {
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        return BadRequest("Failed to create product.");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id || !m_productRepository.ProductExists(id))
        {
            return BadRequest("Product ID mismatch.");
        }

        m_productRepository.UpdateProduct(product);
        return await SaveChangesAsync();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await m_productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound("Product not found.");
        }

        m_productRepository.DeleteProduct(product);
        return await SaveChangesAsync();
    }


    // Preferred: private lambda-style (expression-bodied) helper
    private async Task<IActionResult> SaveChangesAsync() =>
        await m_productRepository.SaveChangeAsync()
            ? NoContent()
            : BadRequest("Problem saving changes");
}
