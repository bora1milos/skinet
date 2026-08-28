using Core.Entities;
using Infrastracture.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(StoreContext context) : ControllerBase
{
    private readonly StoreContext m_context = context;

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await m_context.Set<Product>().ToListAsync();
        if (products == null || products.Count == 0)
        {
            return NotFound("No products found.");
        }

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await m_context.Set<Product>().FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        m_context.Set<Product>().Add(product);
        await m_context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest("Product ID mismatch.");
        }

        m_context.Set<Product>().Update(product);
        await m_context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await m_context.Set<Product>().FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        m_context.Set<Product>().Remove(product);
        await m_context.SaveChangesAsync();


        return NoContent();
    }
}
