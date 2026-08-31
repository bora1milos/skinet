using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Data;

public class ProductRepository(StoreContext context) : IProductRepository
{
    private readonly StoreContext m_context = context;

    public void AddProduct(Product product)
    {
        m_context.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        m_context.Products.Remove(product);
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {
        return await m_context.Products.Select(p => p.Brand).Distinct().ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await m_context.Products.FindAsync(id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand = null, string? type = null, string? sort = null)
    {
        var query = m_context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(brand))
        {
            query = query.Where(p => p.Brand == brand);
        }

        if (!string.IsNullOrWhiteSpace(type))
        {
            query = query.Where(p => p.Type == type);
        }

        if (!string.IsNullOrWhiteSpace(sort))
        {
            query = sort.ToLower() switch
            {
                "name" => query.OrderBy(p => p.Name),
                "priceasc" => query.OrderBy(p => p.Price),
                "pricedesc" => query.OrderByDescending(p => p.Price),
                _ => query
            };
        }

        return await query.ToListAsync();
    }
    public async Task<IReadOnlyList<string>> GetTypesAsync()
    {
        return await m_context.Products.Select(p => p.Type).Distinct().ToListAsync();
    }
    public bool ProductExists(int id)
    {
        return m_context.Products.Any(p => p.Id == id);
    }

    public async Task<bool> SaveChangeAsync()
    {
        return await m_context.SaveChangesAsync() > 0;
    }

    public void UpdateProduct(Product product)
    {
        m_context.Entry(product).State = EntityState.Modified;
    }
}
