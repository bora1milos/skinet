using Core.Entities;
using System.Text.Json;

namespace Infrastracture.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        if (!context.Products.Any())
        {
            var productsData = await File.ReadAllTextAsync("../Infrastracture/Data/SeedData/products.json");
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);
            if (products != null && products.Count > 0)
            {
                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }
        }
    }
}
