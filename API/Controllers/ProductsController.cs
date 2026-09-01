using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IGenericRepository<Product> genericRepository) : ControllerBase
{
    private readonly IGenericRepository<Product> m_genericRepository = genericRepository;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await m_genericRepository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery] string? brand, [FromQuery] string? type, [FromQuery] string? sort)
    {
        var spec = new ProductSpecification(brand, type, sort);
        var products = await m_genericRepository.ListAsync(spec);
        return Ok(products);
    }

    [HttpGet("brands")]
    public async Task<IActionResult> GetBrands()
    {
        var spec = new BrandListSpecification();
        var brands = await m_genericRepository.ListAsync(spec);
        if (brands == null || brands.Count == 0)
        {
            return NotFound("No brands found.");
        }

        return Ok(brands);
    }

    [HttpGet("types")]
    public async Task<IActionResult> GetTypes()
    {
        var spec = new TypeListSpecifications();
        var types = await m_genericRepository.ListAsync(spec);
        if (types == null || types.Count == 0)
        {
            return NotFound("No types found.");
        }

        return Ok(types);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        m_genericRepository.Add(product);
        if (await m_genericRepository.SaveAllAsync())
        {
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        return BadRequest("Failed to create product.");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id || !m_genericRepository.Exists(id))
        {
            return BadRequest("Product ID mismatch.");
        }

        m_genericRepository.Update(product);
        return await SaveChangesAsync();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await m_genericRepository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound("Product not found.");
        }

        m_genericRepository.Remove(product);
        return await SaveChangesAsync();
    }


    // Preferred: private lambda-style (expression-bodied) helper
    private async Task<IActionResult> SaveChangesAsync() =>
        await m_genericRepository.SaveAllAsync()
            ? NoContent()
            : BadRequest("Problem saving changes");
}
